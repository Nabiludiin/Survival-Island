using FMODUnity;
using UltimateWater;
using UnityEngine;
using UnityEngine.InputSystem;

public class FillWaterComponent : MonoBehaviour
{
	[SerializeField]
	private Item_Base saltWaterItem;

	[SerializeField]
	private Item_Base emptyItem;

	[SerializeField]
	private TimerEventer holdToPourTimer;

	private CanvasHelper canvas;

	private PlayerInventory inventory;

	private Network_Player playerNetwork;

	private bool canvasTextVisible;

	private Plane waterPlane;

	public EventReference er_fillWater;

	public EventReference er_pourWater;

	[SerializeField]
	private InputActionReference inputFill;

	[SerializeField]
	private InputActionReference inputPour;

	private void Awake()
	{
		waterPlane = new Plane(Vector3.up, Vector3.zero);
	}

	private void Start()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
		canvas = ComponentManager<CanvasHelper>.Value;
		inventory = playerNetwork.Inventory;
	}

	private void Update()
	{
		if (canvasTextVisible)
		{
			canvas.displayTextManager.HideDisplayTexts();
			canvasTextVisible = false;
		}
		if (!playerNetwork.IsLocalPlayer || CanvasHelper.ActiveMenu != MenuType.None || PlayerItemManager.IsBusy)
		{
			return;
		}
		Slot selectedHotbarSlot = inventory.GetSelectedHotbarSlot();
		if (selectedHotbarSlot == null)
		{
			return;
		}
		ItemInstance itemInstance = selectedHotbarSlot.itemInstance;
		if (itemInstance == null || itemInstance.settings_consumeable.FoodForm != FoodForm.Fluid)
		{
			return;
		}
		FoodType foodType = itemInstance.settings_consumeable.FoodType;
		if (AimingAtTarget())
		{
			if (foodType == FoodType.None)
			{
				canvasTextVisible = true;
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/FillSaltWater"), inputFill.action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputFill.action, "Interact"))
				{
					RuntimeManager.PlayOneShot(er_fillWater, transform.position);
					canvas.displayTextManager.HideDisplayTexts();
					selectedHotbarSlot.SetItem(saltWaterItem, 1);
					itemInstance.SetUsesToMax();
					inventory.hotbar.ReselectCurrentSlot();
				}
			}
			else if (foodType == saltWaterItem.settings_consumeable.FoodType && !itemInstance.HasMaxUses)
			{
				canvasTextVisible = true;
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/FillSaltWater"), inputFill.action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputFill.action, "Interact"))
				{
					RuntimeManager.PlayOneShot(er_fillWater, transform.position);
					canvas.displayTextManager.HideDisplayTexts();
					itemInstance.SetUsesToMax();
					selectedHotbarSlot.RefreshComponents();
					inventory.hotbar.ReselectCurrentSlot();
				}
			}
		}
		if (foodType != FoodType.SaltWater && foodType != FoodType.Water)
		{
			return;
		}
		canvas.displayTextManager.ShowText(Helper.GetTerm("Game/PourLiquid"), inputPour.action.GetBindingDisplayString(), MyInput.Keybinds["RMB"].MainKey, 1, 0, clearAllTexts: false);
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputPour, "RMB"))
		{
			canvas.displayTextManager.HideDisplayTexts();
			if (holdToPourTimer.DoUpdate(Time.deltaTime, autoResetTimer: true))
			{
				RuntimeManager.PlayOneShot(er_pourWater, transform.position);
				selectedHotbarSlot.SetItem(emptyItem, 1);
				inventory.hotbar.ReselectCurrentSlot();
				playerNetwork.Animator.SetAnimation(PlayerAnimation.Trigger_Plant, triggering: true);
			}
			canvas.SetLoadCircle(holdToPourTimer.GetNormalizedProgress());
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(inputPour, "RMB"))
		{
			holdToPourTimer.ResetTimer();
			canvas.SetLoadCircle(state: false);
		}
	}

	public void OnDeSelect()
	{
		if (canvas != null)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	private bool AimingAtTarget()
	{
		Camera mainCamera = Helper.MainCamera;
		if (mainCamera != null)
		{
			Ray ray = mainCamera.ScreenPointToRay(new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f));
			float enter = 0f;
			if (waterPlane.Raycast(ray, out enter))
			{
				float num = Player.UseDistance * 1.5f;
				if (enter < num)
				{
					int num2 = Physics.OverlapSphereNonAlloc(ray.origin + enter * ray.direction, 0.01f, Water._CollidersBuffer, 1 << WaterProjectSettings.Instance.WaterCollidersLayer, QueryTriggerInteraction.Collide);
					for (int i = 0; i < num2; i++)
					{
						WaterVolumeBase waterVolume = WaterVolumeBase.GetWaterVolume(Water._CollidersBuffer[i]);
						if (waterVolume != null && waterVolume is WaterVolumeSubtract)
						{
							return false;
						}
					}
					RaycastHit hit;
					return !Helper.HitAtCursor(out hit, num, LayerMasks.MASK_Water_with_obstruction);
				}
			}
		}
		return false;
	}
}
