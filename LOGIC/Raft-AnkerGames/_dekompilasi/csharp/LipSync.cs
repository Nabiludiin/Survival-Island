using System;
using System.Collections;
using UnityEngine;

public class LipSync : MonoBehaviour
{
	[SerializeField]
	private SkinnedMeshRenderer skinnedMeshRenderer;

	[SerializeField]
	public Dialogue dialogue;

	[SerializeField]
	private BlendShapeForm mouthForm;

	[SerializeField]
	private BlendShapeForm eyebrowForm;

	private Coroutine sync;

	[SerializeField]
	private float targetVolume;

	[SerializeField]
	private float volume;

	[SerializeField]
	private float volumeLerpSpeed = 1f;

	public void Start()
	{
		if (this.dialogue != null)
		{
			Dialogue dialogue = this.dialogue;
			dialogue.OnVoiceLinePlayed = (Action<SO_VoiceLine>)Delegate.Combine(dialogue.OnVoiceLinePlayed, new Action<SO_VoiceLine>(StartSync));
			Dialogue dialogue2 = this.dialogue;
			dialogue2.OnVoiceDataCancelled = (Action<SO_VoiceData.Instance_VoiceData>)Delegate.Combine(dialogue2.OnVoiceDataCancelled, new Action<SO_VoiceData.Instance_VoiceData>(StopSync));
		}
	}

	public void OnDestroy()
	{
		if (this.dialogue != null && !GameManager.IsQuittingApplication)
		{
			Dialogue dialogue = this.dialogue;
			dialogue.OnVoiceLinePlayed = (Action<SO_VoiceLine>)Delegate.Remove(dialogue.OnVoiceLinePlayed, new Action<SO_VoiceLine>(StartSync));
			Dialogue dialogue2 = this.dialogue;
			dialogue2.OnVoiceDataCancelled = (Action<SO_VoiceData.Instance_VoiceData>)Delegate.Remove(dialogue2.OnVoiceDataCancelled, new Action<SO_VoiceData.Instance_VoiceData>(StopSync));
			StopAllCoroutines();
		}
	}

	private void StartSync(SO_VoiceLine voiceLine)
	{
		sync = CoroutineManager.Singleton.AddCoroutine(PlayAnimation(voiceLine.Duration));
	}

	private void StopSync(SO_VoiceData.Instance_VoiceData instance)
	{
		if (gameObject != null && sync != null && instance != null)
		{
			CoroutineManager.Singleton.StopCoroutine(sync);
		}
	}

	private IEnumerator PlayAnimation(float time)
	{
		CoroutineManager.Singleton.AddCoroutine(PingPongBlendShapeRange(eyebrowForm, time));
		CoroutineManager.Singleton.AddCoroutine(PingPongBlendShapeRange(mouthForm, time));
		float timer = 0f;
		while (true)
		{
			timer += Time.deltaTime;
			if (timer >= time)
			{
				break;
			}
			if (dialogue != null && dialogue.currentDialogueInstance != null)
			{
				targetVolume = dialogue.currentDialogueInstance.GetNormalizedVolume();
				volume = Mathf.Lerp(volume, targetVolume, Time.deltaTime * volumeLerpSpeed);
			}
			yield return null;
		}
		volume = 0f;
	}

	private IEnumerator PingPongBlendShapeRange(BlendShapeForm form, float totalTime)
	{
		float timer = 0f;
		while (true)
		{
			float shapeTime = form.playInterval.GetRandomValue() * 2f;
			if (timer + shapeTime >= totalTime)
			{
				shapeTime = totalTime - timer;
			}
			yield return PingPongBlendShape(form.shapeIndexes[UnityEngine.Random.Range(0, form.shapeIndexes.Length)], form.valueInterval.GetRandomValue(), shapeTime);
			timer += shapeTime;
			if (timer >= totalTime)
			{
				break;
			}
			if (form.pauseInterval.maxValue > 0f)
			{
				yield return new WaitForSeconds(form.pauseInterval.GetRandomValue());
			}
			yield return null;
		}
	}

	private IEnumerator PingPongBlendShape(int blendIndex, float newValue, float totalTime)
	{
		float timer = 0f;
		int direction = 1;
		while (true)
		{
			timer += Time.deltaTime * (float)direction;
			float num = Mathf.Clamp01(timer / (totalTime / 2f));
			if (skinnedMeshRenderer != null)
			{
				skinnedMeshRenderer.SetBlendShapeWeight(blendIndex, Mathf.Lerp(0f, newValue, num) * volume);
			}
			if (num >= 1f)
			{
				direction = -1;
			}
			else if (num <= 0f && direction == -1)
			{
				break;
			}
			yield return null;
		}
	}
}
