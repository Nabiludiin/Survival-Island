using I2.Loc;
using RG.Parsecs.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public abstract class CraftingCraftableController : MonoBehaviour
{
	[SerializeField]
	[FormerlySerializedAs("_availableResoruces")]
	protected PlayerResources AvailableResources;

	[SerializeField]
	private NavigationOverrideToggle _toggle;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private GameObject _disabled;

	[SerializeField]
	private TextMeshProUGUI _disabledText;

	[SerializeField]
	protected CraftingContentPanelController CraftingContentPanelController;

	[SerializeField]
	protected CraftingData CraftingData;

	[SerializeField]
	protected LocalizedString NoResourcesTerm;

	[SerializeField]
	protected LocalizedString LevelTerm;

	[SerializeField]
	private CraftingItemUITooltipContent _craftingItemUiTooltipContent;

	protected string DisabledReason;

	protected bool Togglable
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

	public void Awake()
	{
		_icon.sprite = SetSprite();
	}

	public void Show(OngoingCraftingActionData plannedCraftingAction)
	{
		_craftingItemUiTooltipContent.SetCraftingAction(plannedCraftingAction.PlannedCraftingAction);
		_toggle.interactable = true;
		OnShow(plannedCraftingAction);
		SetDisabled();
	}

	private void SetDisabled()
	{
		_disabled.SetActive(!Togglable);
		_disabledText.text = DisabledReason;
		_icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, Togglable ? 1f : 0.5f);
		_toggle.image.color = new Color(_toggle.image.color.r, _toggle.image.color.g, _toggle.image.color.b, Togglable ? 1f : 0.5f);
	}

	public abstract void SetIsOnWithoutInvokingOnValueChange(bool isOn);

	protected abstract void OnShow(OngoingCraftingActionData plannedCraftingAction);

	protected abstract Sprite SetSprite();

	public abstract void SetCraftingElement(bool isOn);
}
