using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Raft/Dialogue/DialogueData")]
public class SO_VoiceData : ScriptableObject
{
	public enum VoiceDataPlayMode
	{
		None = 0,
		InOrder = 1,
		InOrderOneAtATime = 3,
		Random = 2,
		Single = 4
	}

	public class Instance_VoiceData
	{
		public List<SO_VoiceLine> voiceLines;

		public bool isActive;

		public int currentVoiceLineIndex;

		public Action<Instance_VoiceData> OnStarted;

		public Action<SO_VoiceLine> OnVoiceLinePlayed;

		public Action<Instance_VoiceData> OnCanceled;

		public SO_VoiceData voiceData;

		public Network_Player startedPlayer;

		private Dialogue _attachedDialogue;

		private Coroutine coroutine_playdialouge;

		private EventInstance eventInstance;

		public int CurrentVoiceLineIndex
		{
			get
			{
				if (currentVoiceLineIndex >= voiceLines.Count)
				{
					return voiceLines.Count - 1;
				}
				return currentVoiceLineIndex;
			}
		}

		public Dialogue AttachedDialogue => _attachedDialogue;

		public Instance_VoiceData(SO_VoiceData vD, Network_Player player)
		{
			voiceLines = vD.voiceLines;
			voiceData = vD;
			startedPlayer = player;
		}

		public void Play(Dialogue dialogue, int voiceLineIndex = -1)
		{
			_attachedDialogue = dialogue;
			Dialogue.TrackDialogue(this);
			OnStarted?.Invoke(this);
			coroutine_playdialouge = CoroutineManager.Singleton.AddCoroutine(PlayDialogue(voiceLineIndex));
		}

		public void Cancel(bool resetVoiceLineIndex = true)
		{
			if (resetVoiceLineIndex)
			{
				currentVoiceLineIndex = 0;
			}
			Dialogue.UnTrackDialogue(this);
			OnCanceled?.Invoke(this);
			if (eventInstance.isValid())
			{
				eventInstance.stop(STOP_MODE.IMMEDIATE);
				eventInstance.release();
				eventInstance.clearHandle();
			}
			isActive = false;
			CoroutineManager.Singleton.StopCoroutine(coroutine_playdialouge);
		}

		public string GetCurrentSubtitleText()
		{
			string localization = voiceData.voiceLines[CurrentVoiceLineIndex].Localization;
			if (AttachedDialogue.voiceActor == null)
			{
				return localization;
			}
			return AttachedDialogue.voiceActor.displayName + Environment.NewLine + localization;
		}

		public float GetNormalizedVolume()
		{
			if (eventInstance.isValid())
			{
				float value = 0f;
				float finalvalue = 0f;
				eventInstance.getParameterByName("lipsync", out value, out finalvalue);
				return Mathf.Clamp01(finalvalue);
			}
			return 0f;
		}

		private float PlayIndex(int index)
		{
			if (!voiceLines.IsIndexWithinRange(index))
			{
				return 0f;
			}
			SO_VoiceLine sO_VoiceLine = voiceLines[index];
			if (voiceData.twoDimensional)
			{
				eventInstance = SoundManager.PlayProgrammerSound(sO_VoiceLine.eventRef, sO_VoiceLine.eventKey);
			}
			else if (AttachedDialogue != null)
			{
				eventInstance = SoundManager.PlayProgrammerSound(sO_VoiceLine.eventRef, sO_VoiceLine.eventKey, AttachedDialogue.transform);
			}
			OnVoiceLinePlayed?.Invoke(sO_VoiceLine);
			return sO_VoiceLine.Duration;
		}

		private IEnumerator PlayDialogue(int voiceLineIndex)
		{
			yield return new WaitForSeconds(voiceData.startDelay);
			isActive = true;
			switch (voiceData.playMode)
			{
			case VoiceDataPlayMode.None:
			case VoiceDataPlayMode.Random:
				if (voiceLineIndex >= 0)
				{
					currentVoiceLineIndex = voiceLineIndex;
					yield return new WaitForSeconds(PlayIndex(currentVoiceLineIndex));
				}
				break;
			case VoiceDataPlayMode.InOrder:
				while (isActive && currentVoiceLineIndex < voiceLines.Count)
				{
					yield return new WaitForSeconds(PlayIndex(currentVoiceLineIndex));
					currentVoiceLineIndex++;
					if (currentVoiceLineIndex >= voiceLines.Count)
					{
						currentVoiceLineIndex = voiceLines.Count;
					}
				}
				break;
			case VoiceDataPlayMode.InOrderOneAtATime:
				yield return new WaitForSeconds(PlayIndex(currentVoiceLineIndex));
				currentVoiceLineIndex.IncrementCycle(1, 0, voiceLines.Count - 1);
				break;
			case VoiceDataPlayMode.Single:
				currentVoiceLineIndex = voiceLineIndex;
				yield return new WaitForSeconds(PlayIndex(currentVoiceLineIndex));
				break;
			}
			yield return new WaitForSeconds(voiceData.endDelay);
			bool resetVoiceLineIndex = voiceData.playMode != VoiceDataPlayMode.InOrderOneAtATime;
			Cancel(resetVoiceLineIndex);
			yield return null;
		}
	}

	public int priority;

	public float startDelay = 1f;

	public float endDelay = 1f;

	public bool twoDimensional;

	public bool canBeCanceled;

	public bool considerNPCBusyDuringDialogue = true;

	public VoiceDataPlayMode playMode = VoiceDataPlayMode.InOrder;

	public List<SO_VoiceLine> voiceLines;

	public float totalVoiceLineDuration;

	public void RefreshVoicelineDurations()
	{
		_ = totalVoiceLineDuration;
		totalVoiceLineDuration = 0f;
		foreach (SO_VoiceLine voiceLine in voiceLines)
		{
			if (voiceLine != null)
			{
				voiceLine.RefreshDuration();
				totalVoiceLineDuration += voiceLine.Duration;
			}
		}
	}

	public int GetVoiceLineFromPlayMode()
	{
		if (playMode == VoiceDataPlayMode.Random)
		{
			return UnityEngine.Random.Range(0, voiceLines.Count);
		}
		return -1;
	}
}
