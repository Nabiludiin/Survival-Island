using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UsableTool : MonoBehaviour
{
	protected Action OnPressUseButton;

	protected Action OnReleaseUseButton;

	[SerializeField]
	[Tooltip("Set ItemHit bool parameter in players animator")]
	private bool setItemHitAnimation;

	protected Item_Base thisItem;

	protected Network_Player playerNetwork;

	protected PlayerAnimator playerAnimator;

	protected bool isHoldingUseButton;

	private InputAction fireAction;

	public Item_Base GetItemBase => playerNetwork.PlayerItemManager.useItemController.GetCurrentItemInHand();

	protected virtual void Start()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
		playerAnimator = playerNetwork.Animator;
		thisItem = GetItemBase;
		fireAction = PlayerInput.GetPlayerByIndex(0).actions["Fire"];
	}

	protected virtual void OnDestroy()
	{
	}

	protected virtual bool Update()
	{
		if (playerNetwork == null || !playerNetwork.IsLocalPlayer || CanvasHelper.ActiveMenu != MenuType.None)
		{
			return false;
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(fireAction, thisItem.settings_usable.GetUseButtonName()))
		{
			isHoldingUseButton = true;
			if (setItemHitAnimation)
			{
				playerNetwork.Animator.anim.SetBool("ItemHit", isHoldingUseButton);
			}
			OnPressUseButton.CallSafe();
		}
		else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(fireAction, thisItem.settings_usable.GetUseButtonName()))
		{
			isHoldingUseButton = false;
			if (setItemHitAnimation)
			{
				playerNetwork.Animator.anim.SetBool("ItemHit", isHoldingUseButton);
			}
			OnReleaseUseButton.CallSafe();
		}
		return true;
	}

	public void OnSelect()
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (playerAnimator == null)
		{
			playerAnimator = playerNetwork.Animator;
		}
		OnToolSelect();
	}

	public void OnDeSelect()
	{
		OnToolDeSelect();
	}

	public void OnUse()
	{
		OnToolUse();
	}

	public void OnResetUse()
	{
		OnToolResetUse();
	}

	protected virtual void OnToolSelect()
	{
	}

	protected virtual void OnToolDeSelect()
	{
		isHoldingUseButton = false;
		if (setItemHitAnimation && playerNetwork != null && playerNetwork.IsLocalPlayer)
		{
			playerNetwork.Animator.anim.SetBool("ItemHit", value: false);
		}
	}

	protected virtual void OnToolUse()
	{
	}

	protected virtual void OnToolResetUse()
	{
	}
}
