using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlantComponent : MonoBehaviour
{
	[SerializeField]
	private Plant plantPrefab;

	[SerializeField]
	private PlantManager plantManager;

	private bool isDisplayingText;

	private Network_Player playerNetwork;

	private CanvasHelper canvas;

	private void Start()
	{
		canvas = ComponentManager<CanvasHelper>.Value;
		playerNetwork = GetComponentInParent<Network_Player>();
	}

	private void Update()
	{
		if (CanvasHelper.ActiveMenu != MenuType.None || !playerNetwork.IsLocalPlayer)
		{
			return;
		}
		Cropplot cropplot = GetCropplot();
		if (cropplot != null)
		{
			if (cropplot.IsFull)
			{
				return;
			}
			if (cropplot.AcceptsPlantType(plantPrefab.item))
			{
				InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
				LocalizationParameters.itemX = plantPrefab.item.settings_Inventory.DisplayName;
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/PlantItemX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey, 0, 0, clearAllTexts: false);
				isDisplayingText = true;
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
				{
					isDisplayingText = false;
					canvas.displayTextManager.HideDisplayTexts(0);
					Message_PlantSeed message_PlantSeed = new Message_PlantSeed(Messages.PlantManager_PlantSeed, plantManager, cropplot, plantPrefab, waterPlantedSeed: false);
					if (Raft_Network.IsHost)
					{
						message_PlantSeed.plantObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
						message_PlantSeed.waterPlantedSeed = ComponentManager<WeatherManager>.Value.GetCurrentWeatherType() == UniqueWeatherType.Rain;
						plantManager.PlantSeed(cropplot, plantPrefab, message_PlantSeed.plantObjectIndex, message_PlantSeed.waterPlantedSeed);
						playerNetwork.Network.RPC(message_PlantSeed, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					}
					else
					{
						message_PlantSeed.plantObjectIndex = 0u;
						playerNetwork.SendP2P(message_PlantSeed, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					}
				}
			}
			else
			{
				isDisplayingText = false;
				canvas.displayTextManager.HideDisplayTexts(0);
			}
		}
		else if (isDisplayingText)
		{
			canvas.displayTextManager.HideDisplayTexts(0);
			isDisplayingText = false;
		}
	}

	public void OnDeSelect()
	{
		if (canvas != null)
		{
			canvas.displayTextManager.HideDisplayTexts();
			isDisplayingText = false;
		}
	}

	private Cropplot GetCropplot()
	{
		if (Helper.HitAtCursor(out var hit, Player.UseDistance, LayerMasks.MASK_Block) && hit.transform.tag == "Cropplot")
		{
			return hit.transform.gameObject.GetComponent<Cropplot>();
		}
		return null;
	}
}
