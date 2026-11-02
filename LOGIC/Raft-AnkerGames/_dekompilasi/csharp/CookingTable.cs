using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RaycastInteractable))]
public class CookingTable : MonoBehaviour_ID_Network, IRaycastable
{
	[SerializeField]
	private SO_CookingTable_Recipe currentRecipe;

	public CookingRecipeType cookingType;

	[Tooltip("Used when cooking items that does not match a recipe")]
	[SerializeField]
	private SO_CookingTable_Recipe fallbackRecipe;

	[SerializeField]
	private GameObject finishedModel;

	[SerializeField]
	private GameObject gooModel;

	[Tooltip("What item to pick up food with")]
	[SerializeField]
	private Item_Base pickupFoodItem;

	[SerializeField]
	private ParticleSystem particle_active;

	[SerializeField]
	private ParticleSystem particle_complete;

	[SerializeField]
	private ParticleSystem particle_completeBuff;

	[SerializeField]
	private ParticleSystem particle_completeFailedRecipe;

	[SerializeField]
	private StudioEventEmitter eventEmitter_cooking;

	[SerializeField]
	[EventRef]
	private string eventRef_stopCooking;

	[SerializeField]
	[EventRef]
	private string eventRef_startCooking;

	[SerializeField]
	protected Animator anim;

	[SerializeField]
	private float startAnimDelay;

	[HideInInspector]
	public Network_Player localPlayer;

	[HideInInspector]
	public DisplayTextManager displayText;

	[SerializeField]
	private bool useKeyOffForStoppingSound;

	public string localization_requiresHeldPickupItem = "Game/RequiredItemX";

	private static SO_CookingTable_Recipe[] allRecipes;

	private CookingTable_Slot[] slots;

	private Raft_Network network;

	private float cookTimer;

	private uint finishedPortions;

	protected bool hasBeenPlaced;

	public CookingTable_Slot[] Slots => slots;

	public SO_CookingTable_Recipe CurrentRecipe
	{
		get
		{
			return currentRecipe;
		}
		set
		{
			currentRecipe = value;
		}
	}

	public float CookTimer
	{
		get
		{
			return cookTimer;
		}
		set
		{
			cookTimer = value;
		}
	}

	public uint Portions
	{
		get
		{
			return finishedPortions;
		}
		set
		{
			finishedPortions = value;
		}
	}

	protected virtual void Awake()
	{
		if (particle_complete != null)
		{
			particle_complete.StopSafe();
		}
		if (particle_completeBuff != null)
		{
			particle_completeBuff.StopSafe();
		}
		if (particle_completeFailedRecipe != null)
		{
			particle_completeFailedRecipe.StopSafe();
		}
		if (particle_active != null)
		{
			particle_active.StopSafe();
		}
	}

	protected virtual void OnBlockPlaced()
	{
		hasBeenPlaced = true;
		NetworkIDManager.AddNetworkID(this, typeof(CookingTable));
		network = ComponentManager<Raft_Network>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
		displayText = ComponentManager<DisplayTextManager>.Value;
		gooModel.SetActive(value: false);
		finishedModel.SetActive(value: false);
		InitializeSlots();
		RetrieveAllRecipes();
	}

	protected virtual void Update()
	{
		if (!hasBeenPlaced)
		{
			return;
		}
		HandleBoilSound();
		HandleParticles();
		if (Raft_Network.IsHost && IsCooking())
		{
			cookTimer += Time.deltaTime;
			if (cookTimer >= CurrentRecipe.CookTime)
			{
				Message_CookingPot_Recipe message = new Message_CookingPot_Recipe(Messages.CookingPot_Finish, network.NetworkIDManager, ObjectIndex, (int)CurrentRecipe.RecipeIndex);
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				FinishCooking();
			}
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkIDManager.RemoveNetworkID(this, typeof(CookingTable));
	}

	public void OnIsRayed()
	{
		if (hasBeenPlaced)
		{
			if (CanvasHelper.ActiveMenu != MenuType.None || PlayerItemManager.IsBusy || !Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance))
			{
				displayText.HideDisplayTexts();
			}
			else if (currentRecipe == null)
			{
				HandleStartCooking();
			}
			else
			{
				HandlePickupFood();
			}
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (hasBeenPlaced && displayText != null)
		{
			displayText.HideDisplayTexts();
		}
	}

	private void OnSlotInsertItem(Network_Player player, CookingTable_Slot slot, ItemInstance itemInstance)
	{
		Message_CookingPot_Slot message = new Message_CookingPot_Slot(Messages.CookingPot_Insert, network.NetworkIDManager, ObjectIndex, (player == null) ? ((Network_UserId)0uL) : player.steamID, slot.Index, itemInstance);
		if (Raft_Network.IsHost)
		{
			slot.InsertItem(player, itemInstance);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void OnSlotPickupItem(Network_Player player, CookingTable_Slot slot, ItemInstance itemInstance)
	{
		Message_CookingPot_Slot message = new Message_CookingPot_Slot(Messages.CookingPot_Pickup, network.NetworkIDManager, ObjectIndex, (player == null) ? ((Network_UserId)0uL) : player.steamID, slot.Index, itemInstance);
		if (Raft_Network.IsHost)
		{
			slot.RemoveItem(player);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public bool IngredientIsAlreadyPlaced(Item_Base ingredient)
	{
		CookingTable_Slot[] array = slots;
		foreach (CookingTable_Slot cookingTable_Slot in array)
		{
			if (cookingTable_Slot.HasItem && cookingTable_Slot.CurrentItem.UniqueIndex == ingredient.UniqueIndex)
			{
				return true;
			}
		}
		return false;
	}

	public virtual bool CanStartCooking()
	{
		if (finishedPortions == 0)
		{
			return GetIngredientCount() != 0;
		}
		return false;
	}

	protected virtual bool IsCooking()
	{
		if (CurrentRecipe != null)
		{
			return Portions == 0;
		}
		return false;
	}

	private void HandleStartCooking()
	{
		if (!CanStartCooking())
		{
			return;
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		displayText.ShowText(Helper.GetTerm("Game/StartCooking"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			return;
		}
		Message_CookingPot_Recipe message_CookingPot_Recipe = new Message_CookingPot_Recipe(Messages.CookingPot_Start, network.NetworkIDManager, ObjectIndex, -1);
		if (Raft_Network.IsHost)
		{
			if (Raft_Network.IsHost)
			{
				CurrentRecipe = GetRecipeFromIngredients();
				message_CookingPot_Recipe.recipeIndex = (int)CurrentRecipe.RecipeIndex;
			}
			network.RPC(message_CookingPot_Recipe, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			StartCooking(CurrentRecipe);
		}
		else
		{
			network.SendP2P(network.HostID, message_CookingPot_Recipe, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		displayText.HideDisplayTexts();
	}

	private void HandlePickupFood()
	{
		if (!(currentRecipe != null) || finishedPortions == 0)
		{
			return;
		}
		ItemInstance selectedHotbarItem = localPlayer.Inventory.GetSelectedHotbarItem();
		if (selectedHotbarItem != null && selectedHotbarItem.Valid && selectedHotbarItem.UniqueIndex == pickupFoodItem.UniqueIndex)
		{
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
			displayText.ShowText("Pickup " + currentRecipe.Result.settings_Inventory.DisplayName, action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				Message_NetworkBehaviour_ID_SteamID message = new Message_NetworkBehaviour_ID_SteamID(Messages.CookingPot_PickupFood, network.NetworkIDManager, ObjectIndex, localPlayer.steamID);
				if (Raft_Network.IsHost)
				{
					network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					PickupFood(localPlayer);
				}
				else
				{
					network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
				displayText.HideDisplayTexts();
			}
		}
		else if ((pickupFoodItem != null && selectedHotbarItem == null) || !selectedHotbarItem.Valid || (selectedHotbarItem != null && selectedHotbarItem.Valid && selectedHotbarItem.UniqueIndex != pickupFoodItem.UniqueIndex))
		{
			bool applyParameters = false;
			if (pickupFoodItem != null)
			{
				LocalizationParameters.itemX = pickupFoodItem.settings_Inventory.DisplayName;
				applyParameters = true;
			}
			displayText.ShowText(Helper.GetTerm(localization_requiresHeldPickupItem, applyParameters));
		}
		else
		{
			displayText.HideDisplayTexts();
		}
	}

	private void HandleBoilSound()
	{
		if (eventEmitter_cooking != null)
		{
			if (eventEmitter_cooking.IsPlaying() && !IsCooking())
			{
				eventEmitter_cooking.Stop();
			}
			else if (!eventEmitter_cooking.IsPlaying() && IsCooking())
			{
				eventEmitter_cooking.Play();
			}
		}
	}

	private void HandleParticles()
	{
		if (IsCooking())
		{
			if (particle_active != null)
			{
				particle_active.PlaySafe();
			}
			if (particle_complete != null)
			{
				particle_complete.StopSafe();
			}
			if (particle_completeFailedRecipe != null)
			{
				particle_completeFailedRecipe.StopSafe();
			}
			if (particle_completeBuff != null)
			{
				particle_completeBuff.StopSafe();
			}
		}
		else if (CurrentRecipe != null && Portions != 0)
		{
			if (particle_active != null)
			{
				particle_active.StopSafe();
			}
			if (CurrentRecipe == fallbackRecipe)
			{
				if (particle_complete != null)
				{
					particle_complete.PlaySafe();
				}
				if (particle_completeFailedRecipe != null)
				{
					particle_completeFailedRecipe.PlaySafe();
				}
			}
			else
			{
				if (currentRecipe.IsBuff && particle_completeBuff != null)
				{
					particle_completeBuff.PlaySafe();
				}
				if (particle_complete != null)
				{
					particle_complete.PlaySafe();
				}
			}
		}
		else
		{
			if (particle_active != null)
			{
				particle_active.StopSafe();
			}
			if (particle_complete != null)
			{
				particle_complete.StopSafe();
			}
			if (particle_completeFailedRecipe != null)
			{
				particle_completeFailedRecipe.StopSafe();
			}
			if (particle_completeBuff != null)
			{
				particle_completeBuff.StopSafe();
			}
		}
	}

	protected virtual bool StartCooking(SO_CookingTable_Recipe recipe)
	{
		if (recipe == null)
		{
			return false;
		}
		if (!CanStartCooking())
		{
			return false;
		}
		CurrentRecipe = recipe;
		cookTimer = 0f;
		finishedPortions = 0u;
		CookingTable_Slot[] array = slots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ThrowItemIntoPotAnimation();
		}
		StartCoroutine(SetAnimationState(state: true, startAnimDelay));
		return true;
	}

	private IEnumerator SetAnimationState(bool state, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (state && eventRef_startCooking != string.Empty)
		{
			RuntimeManager.PlayOneShot(eventRef_startCooking, transform.position);
		}
		else if (eventRef_stopCooking != string.Empty)
		{
			RuntimeManager.PlayOneShot(eventRef_stopCooking, transform.position);
		}
		anim.SetBool("Cooking", state);
	}

	protected virtual void FinishCooking()
	{
		if (currentRecipe.Result.UniqueIndex != 203)
		{
			AchievementHandler.UnlockAchievement(AchievementType.ach_cookedRecipe);
		}
		finishedPortions = currentRecipe.Portions;
		SetFinishedFoodModel();
		StartCoroutine(SetAnimationState(state: false, 0f));
	}

	private bool PickupFood(Network_Player player)
	{
		if (finishedPortions == 0 || CurrentRecipe == null || player == null)
		{
			return false;
		}
		if (player.IsLocalPlayer)
		{
			player.Inventory.RemoveItem(pickupFoodItem.UniqueName, 1);
			player.Inventory.AddItem(currentRecipe.Result.UniqueName, 1);
		}
		player.Animator.SetAnimation(PlayerAnimation.Trigger_GrabItem);
		finishedPortions--;
		if (finishedPortions == 0)
		{
			CurrentRecipe = null;
			gooModel.SetActive(value: false);
			finishedModel.SetActive(value: false);
		}
		return true;
	}

	private void RetrieveAllRecipes()
	{
		if (allRecipes == null)
		{
			allRecipes = Resources.LoadAll<SO_CookingTable_Recipe>("SO_CookingRecipes");
		}
	}

	private SO_CookingTable_Recipe GetRecipeFromIndex(int recipeIndex)
	{
		if (recipeIndex < 0)
		{
			return null;
		}
		RetrieveAllRecipes();
		SO_CookingTable_Recipe[] array = allRecipes;
		foreach (SO_CookingTable_Recipe sO_CookingTable_Recipe in array)
		{
			if (sO_CookingTable_Recipe.RecipeIndex == recipeIndex)
			{
				return sO_CookingTable_Recipe;
			}
		}
		return null;
	}

	private SO_CookingTable_Recipe GetRecipeFromIngredients()
	{
		RetrieveAllRecipes();
		if (allRecipes == null)
		{
			return fallbackRecipe;
		}
		Item_Base[] ingredientsToCheck = GetCurrentIngredients().ToArray();
		SO_CookingTable_Recipe[] array = allRecipes;
		foreach (SO_CookingTable_Recipe sO_CookingTable_Recipe in array)
		{
			if (sO_CookingTable_Recipe.IsValid && sO_CookingTable_Recipe.RecipeType == cookingType && sO_CookingTable_Recipe.DoesIngredientsCompleteRecipe(ingredientsToCheck))
			{
				return sO_CookingTable_Recipe;
			}
		}
		return fallbackRecipe;
	}

	private List<Item_Base> GetCurrentIngredients()
	{
		List<Item_Base> list = new List<Item_Base>();
		CookingTable_Slot[] array = slots;
		foreach (CookingTable_Slot cookingTable_Slot in array)
		{
			if (cookingTable_Slot.HasItem)
			{
				list.Add(cookingTable_Slot.CurrentItem.baseItem);
			}
		}
		return list;
	}

	private void InitializeSlots()
	{
		slots = GetComponentsInChildren<CookingTable_Slot>();
		if (slots == null)
		{
			return;
		}
		for (int i = 0; i < slots.Length; i++)
		{
			CookingTable_Slot cookingTable_Slot = slots[i];
			if (!(cookingTable_Slot == null))
			{
				cookingTable_Slot.Initialize(this);
				cookingTable_Slot.OnInsertItem = (Action<Network_Player, CookingTable_Slot, ItemInstance>)Delegate.Combine(cookingTable_Slot.OnInsertItem, new Action<Network_Player, CookingTable_Slot, ItemInstance>(OnSlotInsertItem));
				cookingTable_Slot.OnPickupItem = (Action<Network_Player, CookingTable_Slot, ItemInstance>)Delegate.Combine(cookingTable_Slot.OnPickupItem, new Action<Network_Player, CookingTable_Slot, ItemInstance>(OnSlotPickupItem));
			}
		}
	}

	private CookingTable_Slot GetSlotFromIndex(int slotIndex)
	{
		if (slotIndex < 0)
		{
			return null;
		}
		if (slots == null)
		{
			return null;
		}
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].Index == slotIndex)
			{
				return slots[i];
			}
		}
		return null;
	}

	private uint GetIngredientCount()
	{
		uint num = 0u;
		CookingTable_Slot[] array = slots;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].HasItem)
			{
				num++;
			}
		}
		return num;
	}

	private void SetFinishedFoodModel()
	{
		gooModel.SetActive(CurrentRecipe == fallbackRecipe);
		finishedModel.SetActive(CurrentRecipe != fallbackRecipe);
	}

	public override RGD Serialize_Save()
	{
		if (hasBeenPlaced)
		{
			return new RGD_Block_CookingPot(GetComponent<Block>(), new RGD_CookingPot(this));
		}
		return base.Serialize_Save();
	}

	public virtual void RestoreCookingPot(RGD_CookingPot rgdCookingPot)
	{
		if (rgdCookingPot == null)
		{
			return;
		}
		CookTimer = rgdCookingPot.cookTimer;
		Portions = rgdCookingPot.portions;
		CurrentRecipe = GetRecipeFromIndex(rgdCookingPot.recipeIndex);
		if (CurrentRecipe != null && Portions != 0)
		{
			SetFinishedFoodModel();
		}
		anim.SetBool("Cooking", CurrentRecipe != null && Portions == 0);
		if (rgdCookingPot.rgdSlots == null)
		{
			return;
		}
		for (int i = 0; i < rgdCookingPot.rgdSlots.Length; i++)
		{
			RGD_CookingPot_Slot rGD_CookingPot_Slot = rgdCookingPot.rgdSlots[i];
			CookingTable_Slot slotFromIndex = GetSlotFromIndex(rGD_CookingPot_Slot.slotIndex);
			if (slotFromIndex != null)
			{
				rGD_CookingPot_Slot.RestoreSlot(slotFromIndex);
			}
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.CookingPot_Insert:
		{
			Message_CookingPot_Slot message_CookingPot_Slot = msg as Message_CookingPot_Slot;
			CookingTable_Slot slotFromIndex = GetSlotFromIndex(message_CookingPot_Slot.slotIndex);
			Network_Player playerFromID2 = network.GetPlayerFromID(message_CookingPot_Slot.SteamID);
			if (slotFromIndex != null && !slotFromIndex.HasItem)
			{
				slotFromIndex.InsertItem(playerFromID2, message_CookingPot_Slot.rgdItemInstance.GetRealInstance());
				return true;
			}
			return false;
		}
		case Messages.CookingPot_Pickup:
		{
			Message_CookingPot_Slot message_CookingPot_Slot2 = msg as Message_CookingPot_Slot;
			CookingTable_Slot slotFromIndex2 = GetSlotFromIndex(message_CookingPot_Slot2.slotIndex);
			Network_Player playerFromID3 = network.GetPlayerFromID(message_CookingPot_Slot2.SteamID);
			if (slotFromIndex2 != null && slotFromIndex2.HasItem)
			{
				slotFromIndex2.RemoveItem(playerFromID3);
				return true;
			}
			return false;
		}
		case Messages.CookingPot_PickupFood:
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = msg as Message_NetworkBehaviour_ID_SteamID;
			Network_Player playerFromID = network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID);
			return PickupFood(playerFromID);
		}
		case Messages.CookingPot_Start:
		{
			Message_CookingPot_Recipe message_CookingPot_Recipe2 = msg as Message_CookingPot_Recipe;
			SO_CookingTable_Recipe sO_CookingTable_Recipe = null;
			if (Raft_Network.IsHost)
			{
				sO_CookingTable_Recipe = GetRecipeFromIngredients();
				message_CookingPot_Recipe2.recipeIndex = (int)sO_CookingTable_Recipe.RecipeIndex;
			}
			else
			{
				sO_CookingTable_Recipe = GetRecipeFromIndex(message_CookingPot_Recipe2.recipeIndex);
			}
			return StartCooking(sO_CookingTable_Recipe);
		}
		case Messages.CookingPot_Finish:
		{
			Message_CookingPot_Recipe message_CookingPot_Recipe = msg as Message_CookingPot_Recipe;
			SO_CookingTable_Recipe recipeFromIndex = GetRecipeFromIndex(message_CookingPot_Recipe.recipeIndex);
			if (recipeFromIndex != null)
			{
				currentRecipe = recipeFromIndex;
				FinishCooking();
			}
			return false;
		}
		default:
			return false;
		}
	}
}
