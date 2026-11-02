using System;
using RG.Parsecs.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace RG.Parsecs.Survival;

[Serializable]
public class CharacterIcons
{
	[SerializeField]
	[FormerlySerializedAs("JournalIcon")]
	private Sprite _journalIcon;

	[SerializeField]
	[FormerlySerializedAs("CursorIcon")]
	private Sprite _cursorIcon;

	[SerializeField]
	[FormerlySerializedAs("ButtonIcon")]
	private Sprite _buttonIcon;

	[SerializeField]
	[FormerlySerializedAs("DeadIcon")]
	private Sprite _deadIcon;

	[SerializeField]
	[FormerlySerializedAs("DisabledIcon")]
	private Sprite _disabledIcon;

	[SerializeField]
	[FormerlySerializedAs("PortraitIcon")]
	private Sprite _portraitIcon;

	public Sprite JournalIcon
	{
		get
		{
			Sprite sprite = ContentManager.GetSprite(_journalIcon.name);
			if (sprite != null)
			{
				return sprite;
			}
			return _journalIcon;
		}
	}

	public Sprite CursorIcon
	{
		get
		{
			Sprite sprite = ContentManager.GetSprite(_cursorIcon.name);
			if (sprite != null)
			{
				return sprite;
			}
			return _cursorIcon;
		}
	}

	public Sprite ButtonIcon
	{
		get
		{
			Sprite sprite = ContentManager.GetSprite(_buttonIcon.name);
			if (sprite != null)
			{
				return sprite;
			}
			return _buttonIcon;
		}
	}

	public Sprite DeadIcon
	{
		get
		{
			Sprite sprite = ContentManager.GetSprite(_deadIcon.name);
			if (sprite != null)
			{
				return sprite;
			}
			return _deadIcon;
		}
	}

	public Sprite DisabledIcon
	{
		get
		{
			Sprite sprite = ContentManager.GetSprite(_disabledIcon.name);
			if (sprite != null)
			{
				return sprite;
			}
			return _disabledIcon;
		}
	}

	public Sprite PortraitIcon
	{
		get
		{
			Sprite sprite = ContentManager.GetSprite(_portraitIcon.name);
			if (sprite != null)
			{
				return sprite;
			}
			return _portraitIcon;
		}
	}
}
