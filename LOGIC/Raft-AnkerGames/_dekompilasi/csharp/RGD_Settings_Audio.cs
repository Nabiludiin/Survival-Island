using System;
using System.Runtime.Serialization;

[Serializable]
public class RGD_Settings_Audio : SerializableBehaviour
{
	public float masterVolume;

	public float musicVolume;

	public float ambienceVolume;

	public float sfxVolume;

	public float uiVolume;

	public float notebookVolume;

	public int musicRateDropdownIndex;

	public bool isMuted;

	public bool voiceChatDisabled;

	public RGD_Settings_Audio()
	{
		SetDefaults(default);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("MasterVolume", masterVolume);
		info.AddValue("MusicVolume", musicVolume);
		info.AddValue("AmbienceVolume", ambienceVolume);
		info.AddValue("SFXVolume", sfxVolume);
		info.AddValue("UIVolume", uiVolume);
		info.AddValue("NotebookVolume", notebookVolume);
		info.AddValue("MusicRateDropdown", musicRateDropdownIndex);
		info.AddValue("IsMuted", isMuted);
		info.AddValue("VoiceChatDisabled", voiceChatDisabled);
	}

	public RGD_Settings_Audio(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "MasterVolume":
				masterVolume = (float)item.Value;
				break;
			case "MusicVolume":
				musicVolume = (float)item.Value;
				break;
			case "AmbienceVolume":
				ambienceVolume = (float)item.Value;
				break;
			case "SFXVolume":
				sfxVolume = (float)item.Value;
				break;
			case "UIVolume":
				uiVolume = (float)item.Value;
				break;
			case "NotebookVolume":
				notebookVolume = (float)item.Value;
				break;
			case "MusicRateDropdown":
				musicRateDropdownIndex = (int)item.Value;
				break;
			case "IsMuted":
				isMuted = (bool)item.Value;
				break;
			case "VoiceChatDisabled":
				voiceChatDisabled = (bool)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		masterVolume = 1f;
		musicVolume = 1f;
		ambienceVolume = 1f;
		sfxVolume = 1f;
		uiVolume = 1f;
		notebookVolume = 1f;
		musicRateDropdownIndex = 3;
		isMuted = true;
		voiceChatDisabled = true;
	}
}
