using FMOD;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "VoiceLine", menuName = "Raft/Dialogue/VoiceLine")]
public class SO_VoiceLine : ScriptableObject
{
	[SerializeField]
	public string localizationTerm;

	[SerializeField]
	private float endDelay;

	[SerializeField]
	private float duration;

	public EventReference eventRef;

	public string eventKey;

	public float Duration => duration + endDelay;

	public string Localization
	{
		get
		{
			string term = Helper.GetTerm(localizationTerm);
			if (term.IsNullOrEmpty())
			{
				term = localizationTerm;
			}
			return term;
		}
	}

	public void RefreshDuration()
	{
		_ = duration;
		if (eventRef.IsNull || string.IsNullOrEmpty(eventKey))
		{
			SetDurationFromSubtitleLength();
		}
		else
		{
			SetDurationFromClipLength();
		}
	}

	private void SetDurationFromClipLength()
	{
		RESULT soundInfo = RuntimeManager.StudioSystem.getSoundInfo(eventKey, out var info);
		if (soundInfo != RESULT.OK)
		{
			UnityEngine.Debug.Log(soundInfo.ToString() + " " + eventKey, this);
			return;
		}
		MODE mODE = MODE.LOOP_NORMAL | MODE.CREATECOMPRESSEDSAMPLE | MODE.OPENONLY;
		if (RuntimeManager.CoreSystem.createSound(info.name_or_data, mODE | info.mode, ref info.exinfo, out var sound) == RESULT.OK)
		{
			sound.getSubSound(info.subsoundindex, out var subsound);
			uint length = 0u;
			subsound.getLength(out length, TIMEUNIT.MS);
			duration = (float)length * 0.001f;
			sound.release();
		}
		if (duration == 0f)
		{
			UnityEngine.Debug.Log("Duration was set to 0", this);
		}
	}

	private void SetDurationFromSubtitleLength()
	{
		string localization = Localization;
		if (localization.IsNullOrEmpty())
		{
			UnityEngine.Debug.Log("Can't set voiceline duration because localizationterm is null");
			return;
		}
		string[] array = localization.Split(' ');
		duration = Mathf.Clamp(0.35f * (float)array.Length, 3f, float.MaxValue);
		if (duration == 0f)
		{
			UnityEngine.Debug.Log("Duration was set to 0", this);
		}
	}
}
