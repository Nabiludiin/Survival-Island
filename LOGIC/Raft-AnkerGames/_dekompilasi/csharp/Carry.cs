using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Carry : MonoBehaviour, IRaycastable
{
	public Action<Network_Player> OnStartCarry;

	public Action<Network_Player, bool> OnStopCarry;

	private bool allowCarry = true;

	private bool allowDisplayText = true;

	public Network_Player carryingPlayer;

	public bool canJumpWhenCarried = true;

	public Rigidbody body;

	[SerializeField]
	private Collider[] colliders;

	[SerializeField]
	private PlayerAnimation carryAnimation;

	public CarryObjectType carryObjectType;

	public bool enableDropIfRaycastingOther = true;

	protected DisplayTextManager textManager;

	private float cooldownTimer;

	private Network_Player localPlayer;

	[SerializeField]
	protected InputActionReference inputInteract;

	public bool AllowCarry
	{
		get
		{
			return allowCarry;
		}
		set
		{
			allowCarry = value;
		}
	}

	public bool AllowDisplayText
	{
		get
		{
			return allowDisplayText;
		}
		set
		{
			allowDisplayText = value;
		}
	}

	public bool IsBeingCarried => carryingPlayer != null;

	private void Awake()
	{
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Combine(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
	}

	private void Start()
	{
		CanvasHelper value = ComponentManager<CanvasHelper>.Value;
		if (value != null)
		{
			textManager = value.displayTextManager;
		}
	}

	private void Update()
	{
		if (cooldownTimer > 0f)
		{
			cooldownTimer -= Time.deltaTime;
		}
		else
		{
			if (!(carryingPlayer != null) || !carryingPlayer.IsLocalPlayer)
			{
				return;
			}
			if (AllowDisplayText)
			{
				ShowStopCarryText();
			}
			else
			{
				textManager.HideDisplayTexts();
			}
			if (carryingPlayer != null)
			{
				if (carryingPlayer.PlayerScript.IsDead)
				{
					OnStopCarry?.Invoke(carryingPlayer, arg2: true);
					textManager.HideDisplayTexts();
					return;
				}
				if (OnStopCarry != null)
				{
					HandleCarryInput();
				}
			}
			if (carryingPlayer != null)
			{
				HandleBeingCarried();
			}
		}
	}

	private void OnDestroy()
	{
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Remove(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
	}

	public void OnIsRayed()
	{
		if (textManager == null || !AllowCarry)
		{
			return;
		}
		if (cooldownTimer > 0f || carryingPlayer != null || CanvasHelper.ActiveMenu != MenuType.None || !Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance) || PlayerItemManager.IsBusy)
		{
			textManager.HideDisplayTexts();
			return;
		}
		ShowCarryText();
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (OnStartCarry != null && localPlayer != null && localPlayer.CarryingComponent != null && !localPlayer.CarryingComponent.IsCarrying && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputInteract, "Interact"))
		{
			OnStartCarry(ComponentManager<Network_Player>.Value);
			cooldownTimer = 0.2f;
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (textManager != null)
		{
			textManager.HideDisplayTexts();
		}
	}

	private void OnWorldShift(Vector3 shift)
	{
		if (transform.parent == null)
		{
			transform.position -= shift;
		}
	}

	protected virtual void HandleBeingCarried()
	{
	}

	protected virtual void HandleCarryInput()
	{
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputInteract, "Interact") && (enableDropIfRaycastingOther || !(carryingPlayer != null) || !carryingPlayer.PickupScript.GetIsHoveringOverPickup()))
		{
			OnStopCarry(carryingPlayer, arg2: false);
			textManager.HideDisplayTexts();
			cooldownTimer = 0.2f;
		}
	}

	public void StartCarrying(Network_Player player)
	{
		if (!(player == null))
		{
			cooldownTimer = 0.2f;
			carryingPlayer = player;
			player.PlayerItemManager.HideItemInHand();
			player.CarryingComponent.CarriedObject = this;
			if (player.IsLocalPlayer)
			{
				PlayerItemManager.IsBusy = true;
			}
			if (carryAnimation != PlayerAnimation.None)
			{
				player.Animator.SetAnimation(carryAnimation);
			}
			DisableCollider();
		}
	}

	public void StopCarrying(Network_Player player)
	{
		if (player == null)
		{
			return;
		}
		carryingPlayer = null;
		player.CarryingComponent.CarriedObject = null;
		if (player.IsLocalPlayer)
		{
			PlayerItemManager.IsBusy = false;
			if (carryAnimation != PlayerAnimation.None)
			{
				player.Inventory.hotbar.ReselectCurrentSlot();
			}
			if (textManager != null)
			{
				textManager.HideDisplayTexts();
			}
		}
		EnableCollider();
	}

	public void AbortCarry()
	{
		EnableCollider();
		carryingPlayer = null;
	}

	public void EnableCollider()
	{
		if (colliders != null)
		{
			Collider[] array = colliders;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = true;
			}
		}
	}

	public void DisableCollider()
	{
		if (colliders != null)
		{
			Collider[] array = colliders;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
		}
	}

	private void ShowCarryText()
	{
		textManager.ShowText(Helper.GetTerm("Game/CarryAnimal"), inputInteract.action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey, 0, 1, clearAllTexts: false);
	}

	protected virtual void ShowStopCarryText()
	{
		if (ShouldDisplayDropText())
		{
			textManager.ShowText(Helper.GetTerm("Game/DropAnimal"), inputInteract.action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey, 0, 0, clearAllTexts: false);
		}
	}

	protected bool ShouldDisplayDropText()
	{
		if (!enableDropIfRaycastingOther && carryingPlayer != null && carryingPlayer.PickupScript.GetIsHoveringOverPickup())
		{
			return false;
		}
		return true;
	}
}
