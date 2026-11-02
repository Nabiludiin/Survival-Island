using System;
using UltimateWater;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Throwable))]
public class FishingRod : MonoBehaviour
{
	public Bobber bobber;

	public Transform fishingLineStart;

	public Animator rodAnimator;

	[SerializeField]
	private Vector3 ropeFOVOffset;

	[SerializeField]
	private GameObject ropeMesh;

	[SerializeField]
	private MeshFilter bobberMeshFilter;

	[SerializeField]
	private MeshRenderer bobberMeshRenderer;

	[SerializeField]
	private FishingBaitHandler fishingBaitHandler;

	[SerializeField]
	private bool isMetalRod;

	private Network_Player playerNetwork;

	private Throwable throwable;

	private PersonController personController;

	[SerializeField]
	private Rope rope;

	private PlayerAnimator playerAnimator;

	private CanvasHelper canvas;

	private bool canUseRod = true;

	private void Awake()
	{
		personController = GetComponentInParent<PersonController>();
		this.throwable = GetComponent<Throwable>();
		Throwable throwable = this.throwable;
		throwable.Action_OnWaterHit = (Action)Delegate.Combine(throwable.Action_OnWaterHit, new Action(OnWaterHit));
		Throwable throwable2 = this.throwable;
		throwable2.Action_OnThrow = (Action)Delegate.Combine(throwable2.Action_OnThrow, new Action(OnThrow));
	}

	private void Start()
	{
		canvas = ComponentManager<CanvasHelper>.Value;
		playerNetwork = GetComponentInParent<Network_Player>();
		playerAnimator = playerNetwork.Animator;
		PersonController.CameraSubmersionAction = (Action<SubmersionState>)Delegate.Combine(PersonController.CameraSubmersionAction, new Action<SubmersionState>(CameraSubmersionState));
		Bobber bobber = this.bobber;
		bobber.OnFishEscape = (Action)Delegate.Combine(bobber.OnFishEscape, new Action(OnFishEscaped));
		Bobber bobber2 = this.bobber;
		bobber2.OnFishGrabBait = (Action)Delegate.Combine(bobber2.OnFishGrabBait, new Action(OnFishGrabBait));
	}

	private void Update()
	{
		SetRopePositionAndLayer();
		if (!playerNetwork.IsLocalPlayer)
		{
			return;
		}
		bool flag = personController.SubmersionState != SubmersionState.None || personController.controllerType == ControllerType.Water || !canUseRod;
		if (flag && !throwable.IsLocked)
		{
			throwable.ResetCanThrow();
		}
		throwable.IsLocked = flag;
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Fire"];
		InputAction action2 = PlayerInput.GetPlayerByIndex(0).actions["Context"];
		if (!throwable.InWater)
		{
			canvas.displayTextManager.ShowText(Helper.GetTerm("Menu/Fishing/OpenBaitMenu"), action2.GetBindingDisplayString(), MyInput.Keybinds["RMB"].MainKey, 1, 1, clearAllTexts: false);
		}
		else
		{
			canvas.displayTextManager.HideDisplayTexts(1);
		}
		if (throwable.InHand && !throwable.IsLocked && CanvasHelper.ActiveMenu == MenuType.None && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action2, "RMB"))
		{
			canvas.OpenMenu(MenuType.FishingBait);
			canvas.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Fishing);
		}
		else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(action2, "RMB") && CanvasHelper.ActiveMenu == MenuType.FishingBait)
		{
			canvas.CloseMenu(MenuType.FishingBait);
			canvas.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Inventory);
		}
		fishingBaitHandler.SetBaitModelFromCurrentBait(bobberMeshRenderer, bobberMeshFilter);
		if (CanvasHelper.ActiveMenu != MenuType.None || throwable.IsLocked)
		{
			return;
		}
		if (bobber.FishIsOnHook)
		{
			canvas.displayTextManager.ShowText("", action.GetBindingDisplayString(), MyInput.Keybinds["LMB"].MainKey);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "LMB") || SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "RMB"))
			{
				PullItemsFromSea();
			}
		}
		else if (throwable.InHand)
		{
			if (throwable.CanThrow && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "LMB"))
			{
				playerAnimator.anim.SetBool("HookThrow", value: true);
				rodAnimator.SetBool("HookThrow", value: true);
			}
		}
		else if (throwable.InWater && (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(action, "LMB") || SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(action, "RMB")))
		{
			ResetRod();
			playerAnimator.SetAnimation(PlayerAnimation.Trigger_FishingRetract);
			rodAnimator.SetTrigger("FishingRetract");
		}
	}

	private void OnDestroy()
	{
		PersonController.CameraSubmersionAction = (Action<SubmersionState>)Delegate.Remove(PersonController.CameraSubmersionAction, new Action<SubmersionState>(CameraSubmersionState));
	}

	public void OnDeSelect()
	{
		if (rodAnimator != null)
		{
			rodAnimator.SetBool("HookThrow", value: false);
			rodAnimator.SetBool("FightingFish", value: false);
		}
		if (playerAnimator != null)
		{
			playerAnimator.anim.SetBool("HookThrow", value: false);
			playerAnimator.anim.SetBool("FightingFish", value: false);
		}
		if (rope != null)
		{
			rope.gameObject.SetActive(value: false);
		}
		if (canvas != null && canvas.displayTextManager != null)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	private void SetRopePositionAndLayer()
	{
		if (playerNetwork.IsLocalPlayer)
		{
			int value = LayerMasks.MASK_Default.value;
			if (playerNetwork.IsLocalPlayer && !playerNetwork.currentModel.thirdPersonSettings.ThirdPersonModel && throwable.InHand)
			{
				value = LayerMasks.MASK_HandCamera.value;
			}
			ropeMesh.SetLayerSafe(value);
			bobberMeshFilter.gameObject.SetLayerSafe(value);
		}
		if (playerNetwork.IsLocalPlayer && !playerNetwork.currentModel.thirdPersonSettings.ThirdPersonModel)
		{
			rope.SetPosition(0, fishingLineStart, Helper.WorldPointToFOVPointLocal(fishingLineStart.position, fishingLineStart));
			rope.SetPosition(1, bobber.transform);
		}
		else
		{
			rope.SetPosition(0, fishingLineStart);
			rope.SetPosition(1, bobber.transform);
		}
	}

	private void OnFishGrabBait()
	{
		playerAnimator.anim.SetBool("FightingFish", value: true);
		rodAnimator.SetBool("FightingFish", value: true);
	}

	private void OnFishEscaped()
	{
		ResetRod();
		canvas.displayTextManager.HideDisplayTexts();
		fishingBaitHandler.ConsumeBait();
	}

	private void CameraSubmersionState(SubmersionState state)
	{
		if (gameObject.activeInHierarchy && state == SubmersionState.Full)
		{
			ResetRod();
		}
	}

	private void PullItemsFromSea()
	{
		Item_Base randomItemFromCurrentBaitPool = fishingBaitHandler.GetRandomItemFromCurrentBaitPool(isMetalRod);
		if (randomItemFromCurrentBaitPool != null)
		{
			playerNetwork.Inventory.AddItem(randomItemFromCurrentBaitPool.UniqueName, 1);
			fishingBaitHandler.ConsumeBait();
		}
		playerAnimator.SetAnimation(PlayerAnimation.Trigger_FishingRetract);
		rodAnimator.SetTrigger("FishingRetract");
		ParticleManager.PlaySystem("WaterSplash_Hook", throwable.throwableObject.position + Vector3.up * 0.1f);
		ResetRod();
		if (playerNetwork.Inventory.RemoveDurabillityFromHotSlot())
		{
			rope.gameObject.SetActive(value: false);
		}
	}

	private void ResetRod()
	{
		bobber.StopBobbing();
		throwable.ResetCanThrow();
		throwable.PickupThrowable();
		throwable.throwableObjectBody.isKinematic = true;
		playerAnimator.anim.SetBool("HookThrow", value: false);
		playerAnimator.anim.SetBool("FightingFish", value: false);
		rodAnimator.SetBool("HookThrow", value: false);
		rodAnimator.SetBool("FightingFish", value: false);
		canvas.displayTextManager.HideDisplayTexts();
		canUseRod = false;
		Invoke("ResetUseRod", 0.2f);
	}

	private void ResetUseRod()
	{
		canUseRod = true;
	}

	private void OnThrow()
	{
		if (!fishingBaitHandler.HasBaitOfCurrentType())
		{
			fishingBaitHandler.EquipBait(null);
		}
		playerAnimator.anim.SetBool("HookThrow", value: false);
		rodAnimator.SetBool("HookThrow", value: false);
	}

	private void OnWaterHit()
	{
		ParticleManager.PlaySystem("WaterSplash_Hook", bobber.transform.position + Vector3.up * 0.1f);
		if (playerNetwork.IsLocalPlayer)
		{
			bobber.StartBobbing();
		}
		bobber.transform.rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
	}
}
