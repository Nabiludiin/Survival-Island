using UnityEngine;
using UnityEngine.InputSystem;

public class RemovePlaceables : MonoBehaviour
{
	[SerializeField]
	private float removeTime = 3f;

	private float removeTimer;

	private Network_Player playerNetwork;

	private CanvasHelper canvas;

	private Block currentBlock;

	private bool showingText;

	[SerializeField]
	private InputActionReference inputRemove;

	private void Awake()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
	}

	private void Start()
	{
		canvas = ComponentManager<CanvasHelper>.Value;
	}

	private void Update()
	{
		if (CanvasHelper.ActiveMenu != MenuType.None || !playerNetwork.IsLocalPlayer || ChatTextFieldController.IsChatWindowSelected || !playerNetwork.PlayerItemManager.CanSwitch())
		{
			if (showingText)
			{
				ResetRemove();
			}
		}
		else if (currentBlock == null)
		{
			if (Helper.HitAtCursor(out var hit, 5f, LayerMasks.MASK_Block))
			{
				if (hit.collider.gameObject.CompareTag("IgnoreRemovePlaceables"))
				{
					if (showingText)
					{
						ResetRemove();
					}
					return;
				}
				Block componentInParent = hit.transform.GetComponentInParent<Block>();
				if (componentInParent != null && componentInParent.canBeRemoved && componentInParent.buildableItem.settings_buildable.Placeable && Helper.LocalPlayerIsWithinDistance(hit.point, Player.UseDistance * 2f))
				{
					canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Remove"), inputRemove.action.GetBindingDisplayString(), MyInput.Keybinds["Remove"].MainKey, 3, 0, clearAllTexts: false);
					showingText = true;
					if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputRemove, "Remove") && componentInParent != currentBlock)
					{
						currentBlock = componentInParent;
						removeTimer = 0f;
					}
				}
				else if (showingText)
				{
					ResetRemove();
				}
			}
			else if (showingText)
			{
				ResetRemove();
			}
		}
		else if (CheckForCurrentBlock())
		{
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputRemove, "Remove"))
			{
				if (Application.isEditor)
				{
					removeTimer += Time.deltaTime * 10f;
				}
				else
				{
					removeTimer += Time.deltaTime;
				}
			}
			else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(inputRemove, "Remove"))
			{
				ResetRemove();
			}
			float num = removeTime;
			if (currentBlock != null && currentBlock.buildableItem != null && (currentBlock.buildableItem.UniqueIndex == 277 || currentBlock.buildableItem.UniqueIndex == 341))
			{
				num = 1f;
			}
			float num2 = num / GameModeValueManager.GetCurrentGameModeValue().toolVariables.removeSpeedMultiplier;
			if (removeTimer >= num2)
			{
				PickupBlock(currentBlock);
				ResetRemove();
				currentBlock = null;
			}
			canvas.SetLoadCircle(removeTimer > 0f);
			canvas.SetLoadCircle(removeTimer / num2);
		}
		else
		{
			ResetRemove();
			currentBlock = null;
		}
	}

	private bool CheckForCurrentBlock()
	{
		bool result = false;
		if (Helper.HitAllAtCursor(out var allHits, 5f, LayerMasks.MASK_Block))
		{
			for (int i = 0; i < allHits.Length; i++)
			{
				if (allHits[i].transform.GetComponentInParent<Block>().ObjectIndex == currentBlock.ObjectIndex)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	private void ResetRemove()
	{
		removeTimer = 0f;
		currentBlock = null;
		canvas.SetLoadCircle(state: false);
		showingText = false;
		canvas.displayTextManager.HideDisplayTexts();
	}

	private void PickupBlock(Block block)
	{
		if (ReturnItemsFromBlock(block, playerNetwork, giveItems: false))
		{
			BlockCreator.RemoveBlockNetwork(block, playerNetwork, updateRaftBounds: true);
			AchievementHandler.AddBuildRemoveCount();
		}
	}

	public static bool ReturnItemsFromBlock(Block block, Network_Player player, bool giveItems)
	{
		if (block.itemToReturnOnDestroy != null)
		{
			switch (block.buildableItem.UniqueIndex)
			{
			case 42:
			case 140:
			case 296:
			case 389:
			case 456:
			case 457:
			case 458:
			case 640:
			{
				Storage_Small component5 = block.gameObject.GetComponent<Storage_Small>();
				if (!(component5 != null))
				{
					break;
				}
				if (component5.IsOpen)
				{
					return false;
				}
				Inventory inventoryReference = component5.GetInventoryReference();
				for (int i = 0; i < inventoryReference.GetSlotCount(); i++)
				{
					Slot slot2 = inventoryReference.GetSlot(i);
					if (!slot2.IsEmpty && giveItems)
					{
						player.Inventory.AddItem(slot2.itemInstance);
					}
				}
				return true;
			}
			case 173:
			{
				Reciever component4 = block.gameObject.GetComponent<Reciever>();
				if (component4 != null && !component4.battery.BatterySlotIsEmpty && giveItems)
				{
					component4.battery.Take(player, component4.battery.BatteryUses);
				}
				return true;
			}
			case 113:
			{
				ResearchTable component2 = block.gameObject.GetComponent<ResearchTable>();
				if (component2 != null)
				{
					Slot slot = component2.Slot;
					if (slot != null && !slot.IsEmpty)
					{
						ItemInstance itemInstance = slot.itemInstance;
						if (giveItems)
						{
							player.Inventory.AddItem(itemInstance);
						}
					}
				}
				return true;
			}
			case 172:
			{
				TrophyTextwriter component6 = block.gameObject.GetComponent<TrophyTextwriter>();
				if (component6 == null)
				{
					return false;
				}
				if (component6.GetIsBeingEdited())
				{
					return false;
				}
				return true;
			}
			case 195:
			case 196:
			case 197:
			{
				TrophyHolder componentInChildren2 = block.gameObject.GetComponentInChildren<TrophyHolder>();
				if (componentInChildren2 == null)
				{
					return false;
				}
				TrophyTextwriter componentInParent = componentInChildren2.GetComponentInParent<TrophyTextwriter>();
				if (componentInParent != null && componentInParent.GetIsBeingEdited())
				{
					return false;
				}
				Item_Base currentItem = componentInChildren2.GetCurrentItem();
				if (currentItem != null && giveItems)
				{
					player.Inventory.AddItem(currentItem.UniqueName, 1);
				}
				return true;
			}
			case 257:
			{
				TextWriterObject componentInChildren3 = block.gameObject.GetComponentInChildren<TextWriterObject>();
				if (componentInChildren3 == null)
				{
					return false;
				}
				string text = componentInChildren3.GetText();
				ItemInstance itemInstance2 = new ItemInstance(ItemManager.GetItemByIndex(257), 1, 1, text);
				itemInstance2.exclusiveString = text;
				if (giveItems)
				{
					player.Inventory.AddItem(itemInstance2);
				}
				return true;
			}
			case 259:
			{
				Sprinkler componentInChildren8 = block.gameObject.GetComponentInChildren<Sprinkler>();
				if (componentInChildren8 != null && !componentInChildren8.battery.BatterySlotIsEmpty && giveItems)
				{
					componentInChildren8.battery.Take(player, componentInChildren8.battery.BatteryUses);
				}
				break;
			}
			case 293:
			{
				BatteryCharger componentInChildren9 = block.gameObject.GetComponentInChildren<BatteryCharger>();
				if (componentInChildren9 != null)
				{
					if (!componentInChildren9.batteryL.BatterySlotIsEmpty && giveItems)
					{
						componentInChildren9.batteryL.Take(player, componentInChildren9.batteryL.BatteryUses);
					}
					if (!componentInChildren9.batteryR.BatterySlotIsEmpty && giveItems)
					{
						componentInChildren9.batteryR.Take(player, componentInChildren9.batteryR.BatteryUses);
					}
				}
				break;
			}
			case 316:
			{
				TrophyHolder componentInChildren5 = block.gameObject.GetComponentInChildren<TrophyHolder>();
				if (componentInChildren5 != null && componentInChildren5.IsOccupied)
				{
					Item_Base currentItem2 = componentInChildren5.GetCurrentItem();
					if (currentItem2 != null && giveItems)
					{
						player.Inventory.AddItem(currentItem2.UniqueName, 1);
					}
				}
				break;
			}
			case 322:
			{
				Battery componentInChildren = block.GetComponentInChildren<Battery>();
				if (componentInChildren != null && giveItems)
				{
					componentInChildren.Take(player, componentInChildren.BatteryUses);
				}
				break;
			}
			case 263:
			{
				RadioPlayer componentInChildren7 = block.GetComponentInChildren<RadioPlayer>();
				if (componentInChildren7 != null && componentInChildren7.HasCassette)
				{
					Item_Base itemFromCassetIndex = componentInChildren7.GetItemFromCassetIndex(componentInChildren7.CassetteIndex);
					if (itemFromCassetIndex != null && giveItems)
					{
						player.Inventory.AddItem(itemFromCassetIndex.UniqueName, 1);
					}
				}
				break;
			}
			case 274:
			{
				Placeable_BiofuelExtractor componentInChildren4 = block.GetComponentInChildren<Placeable_BiofuelExtractor>();
				if (componentInChildren4 != null && componentInChildren4.outputTank.IsFull && giveItems)
				{
					player.Inventory.AddItem(ItemManager.GetItemByIndex(286).UniqueName, 1);
				}
				break;
			}
			case 363:
			{
				Placeable_RecyclerExtractor componentInChildren6 = block.GetComponentInChildren<Placeable_RecyclerExtractor>();
				if (!(componentInChildren6 != null))
				{
					break;
				}
				if (componentInChildren6.battery != null && !componentInChildren6.battery.BatterySlotIsEmpty && giveItems)
				{
					componentInChildren6.battery.Take(player, componentInChildren6.battery.BatteryUses);
				}
				if (componentInChildren6.outputTank != null && componentInChildren6.outputTank.CurrentTankAmount > 0f)
				{
					int amount = Mathf.FloorToInt(componentInChildren6.outputTank.CurrentTankAmount / componentInChildren6.outputTank.minimumHarvestAmount);
					if (giveItems)
					{
						player.Inventory.AddItem(ItemManager.GetItemByIndex(364).UniqueName, amount);
					}
				}
				break;
			}
			case 366:
			{
				CookingTable_Juicer component3 = block.GetComponent<CookingTable_Juicer>();
				if (component3 != null && component3.battery != null && !component3.battery.BatterySlotIsEmpty && giveItems)
				{
					component3.battery.Take(player, component3.battery.BatteryUses);
				}
				break;
			}
			case 540:
			case 642:
			{
				CookingStand_Electric component = block.GetComponent<CookingStand_Electric>();
				if (component != null && component.electricCookingStand != null && component.electricCookingStand.battery != null && !component.electricCookingStand.battery.BatterySlotIsEmpty)
				{
					component.electricCookingStand.battery.Take(player, component.electricCookingStand.battery.BatteryUses);
				}
				break;
			}
			}
		}
		else if (!GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.unlimitedResources)
		{
			CostMultiple[] newCost = block.buildableItem.settings_recipe.NewCost;
			foreach (CostMultiple costMultiple in newCost)
			{
				int amount2 = Mathf.CeilToInt((float)costMultiple.amount * 0.5f);
				if (giveItems)
				{
					player.Inventory.AddItem(costMultiple.items[0].UniqueName, amount2);
				}
			}
		}
		return true;
	}
}
