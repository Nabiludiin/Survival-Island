using RG.Core.Base;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Common;

[CreateAssetMenu(fileName = "New SoundSlot", menuName = "60 Parsecs!/Sound Slot")]
public class SoundSlot : RGScriptableObject, ISaveable
{
	[Tooltip("Event to which data will be save and load")]
	[SerializeField]
	private SaveEvent _saveEvent;

	[HideInInspector]
	[SerializeField]
	private string _id;

	[Tooltip("This is the value the SoundEventName will be set to each time this object is reset.")]
	[SerializeField]
	private string _defaultSoundEventName;

	[Tooltip("This will be used to play back a sound. Can be overriden in runtime by node.")]
	[SerializeField]
	private string _soundEventName;

	private const string SOUND_SLOT_PREFIX = "soundSlot";

	public SaveEvent SaveEvent
	{
		get
		{
			return _saveEvent;
		}
		set
		{
			_saveEvent = value;
		}
	}

	public string DefaultSoundEventName => _defaultSoundEventName;

	public string SoundEventName
	{
		get
		{
			return _soundEventName;
		}
		set
		{
			_soundEventName = value;
		}
	}

	public string ID => Guid;

	public override string GuidPrefix => "soundSlot";

	public void ResetToDefaultState()
	{
		_soundEventName = _defaultSoundEventName;
	}

	public override string ToString()
	{
		return ID;
	}

	public override bool Equals(object other)
	{
		SoundSlot soundSlot = other as SoundSlot;
		if (soundSlot != null)
		{
			return ID.Equals(soundSlot.ID);
		}
		return false;
	}

	private void OnEnable()
	{
		Register();
	}

	private void OnDestroy()
	{
		Unregister();
	}

	public override int GetHashCode()
	{
		return ID.GetHashCode();
	}

	public string Serialize()
	{
		return JsonUtility.ToJson(new StringWrapper
		{
			Value = _soundEventName
		});
	}

	public void Deserialize(string jsonData)
	{
		_soundEventName = JsonUtility.FromJson<StringWrapper>(jsonData).Value;
	}

	public void Register()
	{
		if ((bool)_saveEvent)
		{
			_saveEvent.RegisterListener(this);
		}
		else
		{
			Debug.Log($"Sound slot {name} has not set Save Event");
		}
	}

	public void Unregister()
	{
		if ((bool)_saveEvent)
		{
			_saveEvent.UnregisterListener(this);
		}
	}

	public void ResetData()
	{
		_soundEventName = _defaultSoundEventName;
	}
}
