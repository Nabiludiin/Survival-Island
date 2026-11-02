using UnityEngine;

namespace RG.Parsecs.Survival;

public class ExpeditionCharacterSlotController : MonoBehaviour
{
	[SerializeField]
	[Header("References")]
	[Tooltip("Reference to toggle that is used for this character slot.")]
	private NavigationOverrideToggle _toggle;

	public bool IsOn
	{
		get
		{
			return _toggle.isOn;
		}
		set
		{
			_toggle.isOn = value;
		}
	}

	public bool Interactable
	{
		get
		{
			return _toggle.interactable;
		}
		set
		{
			_toggle.interactable = value;
		}
	}

	public bool Togglable
	{
		get
		{
			return _toggle.Togglable;
		}
		set
		{
			_toggle.Togglable = value;
		}
	}

	public Character Character { get; private set; }

	public void SetVisible(bool visible)
	{
		gameObject.SetActive(visible);
	}

	public void SetCharacter(Character character)
	{
		Character = character;
		if (Character.RuntimeData.HasStatusPreventingGoingOnExpeditions() | !Character.CanLock())
		{
			_toggle.interactable = false;
		}
		if (Character.RuntimeData.IsAlive())
		{
			_toggle.image.sprite = Character.StaticData.Icons.JournalIcon;
		}
		else
		{
			_toggle.image.sprite = Character.StaticData.Icons.DeadIcon;
		}
	}
}
