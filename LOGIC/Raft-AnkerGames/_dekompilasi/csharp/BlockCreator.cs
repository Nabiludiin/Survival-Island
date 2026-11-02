using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlockCreator : MonoBehaviour_Network, SceneEventInterface
{
	public struct UpgradePreviewData
	{
		public Block block;

		public Block ghostBlock;

		public void ShowPreview(Block block, Item_Base upgradeItem)
		{
			if (this.block != block)
			{
				ResetPreview();
			}
			if (!(block != null) || !(upgradeItem != null))
			{
				return;
			}
			Block blockPrefab = upgradeItem.settings_buildable.GetBlockPrefab(block.dpsType);
			if (!(blockPrefab == null))
			{
				this.block = block;
				DestroyGhost();
				Block block2 = (ghostBlock = UnityEngine.Object.Instantiate(blockPrefab, block.transform.position, block.transform.rotation, lockedBuildPivot));
				block2.OnStartingPlacement();
				if (block2.occupyingComponent != null && SingletonGeneric<GameManager>.Singleton != null)
				{
					block2.occupyingComponent.SetNewMaterial(SingletonGeneric<GameManager>.Singleton.ghostMaterialGreen);
				}
				SetOriginalBlockRenderState(state: false);
			}
		}

		public void SetMaterialFromResources(bool hasEnoughResources)
		{
			if (ghostBlock != null && ghostBlock.occupyingComponent != null && SingletonGeneric<GameManager>.Singleton != null)
			{
				ghostBlock.occupyingComponent.SetNewMaterial(hasEnoughResources ? SingletonGeneric<GameManager>.Singleton.ghostMaterialGreen : SingletonGeneric<GameManager>.Singleton.ghostMaterialRed);
			}
		}

		public void ResetPreview()
		{
			DestroyGhost();
			SetOriginalBlockRenderState(state: true);
		}

		private void SetOriginalBlockRenderState(bool state)
		{
			if (!(block != null) || !(block.occupyingComponent != null) || block.occupyingComponent.renderers == null)
			{
				return;
			}
			Renderer[] renderers = block.occupyingComponent.renderers;
			foreach (Renderer renderer in renderers)
			{
				if (renderer != null)
				{
					renderer.enabled = state;
				}
			}
		}

		private void DestroyGhost()
		{
			if (ghostBlock != null)
			{
				UnityEngine.Object.Destroy(ghostBlock.gameObject);
			}
		}
	}

	public static Action<Block, Network_Player, bool, int> PlaceBlockCallStack;

	public static Action<Block, Network_Player, bool, int> PlaceBlockCallStackAlways;

	public static Action<List<Block>, Network_Player> RemoveBlockCallStack;

	public static Action<List<Block>, Network_Player> RemoveBlockCallStackAlways;

	public static float MaxHeight;

	public static int FloorCount;

	public static int FoundationCount;

	public const float BlockSize = 1.5f;

	public const float HalfBlockSize = 0.75f;

	public const float FullFloorHeight = 2.42f;

	public const float HalfFloorHeight = 1.21f;

	[HideInInspector]
	public bool aimingWithPlaceable;

	[SerializeField]
	private ColliderPrefabEnabler colliderPrefabEnablerPrefab;

	[SerializeField]
	private ColliderPrefabEnabler colliderPrefabEnabler;

	[SerializeField]
	private ParticleSystem blockBuildParticles;

	[SerializeField]
	private ParticleSystem blockBuildParticlesPlaceable;

	[SerializeField]
	private static ParticleSystem blockBuildParticlesPlaceableStatic;

	[SerializeField]
	private ParticleSystem blockBreakParticles;

	[SerializeField]
	private static ParticleSystem blockBreakParticlesStatic;

	public EventReference er_createBlock;

	public EventReference er_placeBlock;

	private Raft_Network network;

	private static List<Block> placedBlocks = new List<Block>();

	private static List<Item_Base> buildableItems = new List<Item_Base>();

	private Network_Player playerNetwork;

	[HideInInspector]
	public Block selectedBlock;

	private Item_Base selectedBuildableItem;

	private Block selectedBuildablePrefab;

	private CanvasHelper canvas;

	private PlayerInventory playerInventory;

	private BlockQuad quadAtCursor;

	private BlockSurface quadSurface;

	private RaycastHit quadHit;

	private const float rotateDegreeSmooth = 5f;

	private float rotationKeyAccumulation;

	private RaftCollisionManager collisionManager;

	[HideInInspector]
	public BuildMenu buildMenu;

	private static Transform lockedBuildPivot;

	private float quickBuildTimer;

	private bool isRotating;

	public UpgradePreviewData upgradePreview;

	[SerializeField]
	private InputActionReference inputUse;

	[SerializeField]
	private InputActionReference inputRotate;

	[SerializeField]
	private InputActionReference inputRotateDelta;

	[SerializeField]
	private InputActionReference inputPlayerContext;

	[SerializeField]
	private InputActionReference inputUiContext;

	[SerializeField]
	private InputActionReference inputBlockPick;

	private PlayerInput playerInput;

	public static float blockRemoveInterval = 0.5f;

	public static float blockRemoveTimer = blockRemoveInterval;

	public float buildDelay = 2f;

	public bool IsRotating => isRotating;

	private void Awake()
	{
		if (SingletonGeneric<GameManager>.Singleton != null)
		{
			lockedBuildPivot = SingletonGeneric<GameManager>.Singleton.lockedPivot;
		}
		playerNetwork = GetComponentInParent<Network_Player>();
	}

	private void Start()
	{
		if (buildableItems == null || buildableItems.Count == 0)
		{
			buildableItems = ItemManager.GetBuildableItems();
		}
		collisionManager = ComponentManager<RaftCollisionManager>.Value;
		network = ComponentManager<Raft_Network>.Value;
		playerInventory = playerNetwork.Inventory;
		canvas = ComponentManager<CanvasHelper>.Value;
		SetBlockTypeToBuild("Block_Foundation");
		if (playerNetwork.IsLocalPlayer)
		{
			blockBreakParticlesStatic = blockBreakParticles;
			blockBuildParticlesPlaceableStatic = blockBuildParticlesPlaceable;
			buildMenu = ComponentManager<BuildMenu>.Value;
			buildMenu.Initialize(this);
			colliderPrefabEnabler = UnityEngine.Object.Instantiate(colliderPrefabEnablerPrefab, playerNetwork.Camera.transform);
			colliderPrefabEnabler.transform.localPosition = Vector3.zero;
			ThirdPerson thirdPersonSettings = playerNetwork.currentModel.thirdPersonSettings;
			thirdPersonSettings.OnThirdpersonModelChange = (Action<bool>)Delegate.Combine(thirdPersonSettings.OnThirdpersonModelChange, new Action<bool>(OnThirdPersonModelChange));
		}
		gameObject.SetActive(value: false);
		playerInput = PlayerInput.GetPlayerByIndex(0);
	}

	private new void OnDestroy()
	{
		collisionManager = null;
		network = null;
		playerInventory = null;
		canvas = null;
		playerNetwork = null;
	}

	private void Update()
	{
		if (quickBuildTimer > 0f)
		{
			quickBuildTimer -= Time.deltaTime;
			if (quickBuildTimer < 0f)
			{
				quickBuildTimer = 0f;
			}
		}
		if (!playerNetwork.IsLocalPlayer)
		{
			return;
		}
		colliderPrefabEnabler.ShowCollider();
		quadAtCursor = GetQuadAtCursor();
		if (selectedBuildableItem != null && (selectedBlock == null || selectedBuildablePrefab == null))
		{
			SetBlockTypeToBuild(selectedBuildableItem.UniqueName);
		}
		if ((selectedBlock == null || !selectedBlock.buildableItem.settings_buildable.Placeable) && CanvasHelper.ActiveMenu == MenuType.None)
		{
			canvas.displayTextManager.ShowText(Helper.GetTerm("Game/BuildMenu"), inputPlayerContext.action.GetBindingDisplayString(), MyInput.Keybinds["RMB"].MainKey, 1, 0, clearAllTexts: false);
		}
		else
		{
			canvas.displayTextManager.HideDisplayTexts(1);
		}
		if (playerInput.currentActionMap.name == "Player")
		{
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputPlayerContext, "RMB") && CanvasHelper.ActiveMenu == MenuType.None && (selectedBlock == null || !selectedBlock.buildableItem.settings_buildable.Placeable))
			{
				canvas.OpenMenuCloseOther(MenuType.BuildMenu);
				upgradePreview.ResetPreview();
			}
		}
		else if (playerInput.currentActionMap.name == "UI" && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(inputUiContext, "RMB") && CanvasHelper.ActiveMenu == MenuType.BuildMenu)
		{
			canvas.CloseMenu(MenuType.BuildMenu);
		}
		if (CanvasHelper.ActiveMenu == MenuType.None)
		{
			HandleBlockPick();
		}
		if (selectedBlock == null)
		{
			if (CanvasHelper.ActiveMenu == MenuType.None && IsBuildableItemUpgradeItem(selectedBuildableItem))
			{
				HandleUpgrade();
			}
			else
			{
				upgradePreview.ResetPreview();
			}
			return;
		}
		aimingWithPlaceable = selectedBuildableItem.settings_buildable.Placeable;
		if (selectedBuildableItem.UniqueIndex == 237 || Block.IsBlockIndexCollectionNet(selectedBuildableItem.UniqueIndex))
		{
			aimingWithPlaceable = false;
		}
		SetGhostBlockVisibility(quadAtCursor != null);
		if (quadAtCursor == null || selectedBuildablePrefab == null)
		{
			if (isRotating)
			{
				isRotating = false;
				playerNetwork.PlayerScript.SetMouseLookScripts(canLook: true);
			}
			return;
		}
		Block blockPrefab = selectedBuildableItem.settings_buildable.GetBlockPrefab(quadSurface.dpsType);
		if (blockPrefab == null)
		{
			SetGhostBlockVisibility(visible: false);
			canvas.displayTextManager.HideDisplayTexts(1);
			return;
		}
		if (selectedBlock.dpsType != blockPrefab.dpsType)
		{
			SetBlockTypeToBuild(blockPrefab.buildableItem.UniqueName);
		}
		HandleRotationOfSelectedBlock();
		HandleMirroredVersion();
		SetGhostBlockPositionAndRotation();
		BuildError buildError = CanBuildBlock(selectedBlock);
		switch (buildError)
		{
		case BuildError.PositionOccupied:
			selectedBlock.occupyingComponent.SetNewMaterial(SingletonGeneric<GameManager>.Singleton.ghostMaterialRed);
			break;
		case BuildError.PositionOccupied_Same:
			if (selectedBlock.buildableItem.settings_buildable.HideWhenOverlapSamePos)
			{
				SetGhostBlockVisibility(visible: false);
			}
			else
			{
				selectedBlock.occupyingComponent.SetNewMaterial(SingletonGeneric<GameManager>.Singleton.ghostMaterialRed);
			}
			break;
		case BuildError.UnKnown:
			SetGhostBlockVisibility(visible: false);
			break;
		case BuildError.InWater:
			selectedBlock.occupyingComponent.SetNewMaterial(SingletonGeneric<GameManager>.Singleton.ghostMaterialRed);
			break;
		case BuildError.InMenu:
			SetGhostBlockVisibility(visible: false);
			break;
		case BuildError.NoResources:
			selectedBlock.occupyingComponent.SetNewMaterial(SingletonGeneric<GameManager>.Singleton.ghostMaterialRed);
			break;
		case BuildError.NotStable:
			selectedBlock.occupyingComponent.SetNewMaterial(SingletonGeneric<GameManager>.Singleton.ghostMaterialRed);
			break;
		case BuildError.None:
			selectedBlock.occupyingComponent.SetNewMaterial(SingletonGeneric<GameManager>.Singleton.ghostMaterialGreen);
			break;
		}
		if (buildError != BuildError.InMenu)
		{
			if (selectedBlock.gameObject.activeInHierarchy)
			{
				int num = (aimingWithPlaceable ? 1 : 2);
				if (selectedBlock.isRotateable)
				{
					if (selectedBlock.canRotateFreely)
					{
						canvas.displayTextManager.ShowText(Helper.GetTerm("Game/RotateSmooth"), inputRotate.action.GetBindingDisplayString(), "RSH", MyInput.Keybinds["Rotate"].MainKey, num, 0, clearAllTexts: false);
					}
					else
					{
						canvas.displayTextManager.ShowText(Helper.GetTerm("Controls/Rotate"), inputRotate.action.GetBindingDisplayString(), MyInput.Keybinds["Rotate"].MainKey, num, 0, clearAllTexts: false);
					}
				}
				if (selectedBlock.buildableItem.settings_buildable.MirroredVersion != null && !SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
				{
					InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Special"];
					canvas.displayTextManager.ShowText(Helper.GetTerm("Controls/Mirror"), action.GetBindingDisplayString(), MyInput.Keybinds["Mirror"].MainKey, num + 1, 0, clearAllTexts: false);
				}
			}
		}
		else
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
		if (buildError != BuildError.None)
		{
			return;
		}
		bool flag = !selectedBuildableItem.settings_buildable.Placeable && GeneralSettingsBox.AllowQuickBuild && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputUse, "LMB") && quickBuildTimer <= 0f;
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputUse, "LMB") | flag)
		{
			if (flag)
			{
				quickBuildTimer = 0.25f;
			}
			if (isRotating)
			{
				isRotating = false;
			}
			if (Raft_Network.IsHost)
			{
				Message_BlockCreator_PlaceBlock message_BlockCreator_PlaceBlock = new Message_BlockCreator_PlaceBlock(Messages.BlockCreator_PlaceBlock, this, selectedBuildableItem.UniqueIndex, SaveAndLoad.GetUniqueObjectIndex(), (selectedBlock.networkedBehaviour != null) ? SaveAndLoad.GetUniqueObjectIndex() : 0u, (selectedBlock.networkedBehaviour != null) ? NetworkUpdateManager.GetUniqueBehaviourIndex() : 0u, selectedBlock.transform.localPosition, selectedBlock.transform.localEulerAngles, -1, quadSurface.dpsType);
				playerNetwork.Network.RPC(message_BlockCreator_PlaceBlock, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				CreateBlock(selectedBuildableItem, message_BlockCreator_PlaceBlock.LocalPosition, message_BlockCreator_PlaceBlock.LocalEuler, message_BlockCreator_PlaceBlock.dpsType, playerNetwork.Inventory.hotbar.GetSelectedSlotIndex(), replicating: false, message_BlockCreator_PlaceBlock.blockObjectIndex, message_BlockCreator_PlaceBlock.networkedObjectIndex, message_BlockCreator_PlaceBlock.networkedBehaviourIndex);
			}
			else
			{
				Message_BlockCreator_PlaceBlock message = new Message_BlockCreator_PlaceBlock(Messages.BlockCreator_PlaceBlock, this, selectedBuildableItem.UniqueIndex, 0u, 0u, 0u, selectedBlock.transform.localPosition, selectedBlock.transform.localEulerAngles, playerNetwork.Inventory.hotbar.GetSelectedSlotIndex(), quadSurface.dpsType);
				playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	private void OnDisable()
	{
		if (colliderPrefabEnabler != null)
		{
			colliderPrefabEnabler.HideCollider();
		}
		aimingWithPlaceable = false;
		if (canvas != null)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			FoundationCount = 0;
			MaxHeight = 1f;
			FloorCount = 1;
			RemoveBlockCallStack = null;
			RemoveBlockCallStackAlways = null;
			network = null;
			placedBlocks.Clear();
		}
	}

	private void OnThirdPersonModelChange(bool newThirdPersonState)
	{
		if (colliderPrefabEnabler != null)
		{
			colliderPrefabEnabler.SetColliderSizeFromThirdpersonState(newThirdPersonState);
		}
	}

	public static List<Block> GetPlacedBlocks()
	{
		return placedBlocks;
	}

	public static Block GetBlockByObjectIndex(uint objectIndex)
	{
		return placedBlocks.Find((Block b) => b.ObjectIndex == objectIndex);
	}

	public static void RemoveBlockNetwork(Block block, Network_Player playerRemovingBlock, bool updateRaftBounds)
	{
		Raft_Network value = ComponentManager<Raft_Network>.Value;
		Message_BlockCreator_RemoveBlock message = new Message_BlockCreator_RemoveBlock(Messages.BlockCreator_RemoveBlock, value.GetLocalPlayer().BlockCreator, (playerRemovingBlock == null) ? ((Network_UserId)0uL) : playerRemovingBlock.steamID, block, updateRaftBounds);
		if (Raft_Network.IsHost)
		{
			value.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			RemoveBlock(block, playerRemovingBlock, updateRaftBounds);
		}
		else
		{
			value.SendP2P(value.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public static void RemoveUnstableBlockNetworked(Block block, Network_Player playerRemovingBlock, bool updateRaftBounds)
	{
		Raft_Network value = ComponentManager<Raft_Network>.Value;
		Message_BlockCreator_RemoveBlock message = new Message_BlockCreator_RemoveBlock(Messages.BlockCreator_RemoveBlock, value.GetLocalPlayer().BlockCreator, (playerRemovingBlock == null) ? ((Network_UserId)0uL) : playerRemovingBlock.steamID, block, updateRaftBounds);
		value.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
	}

	public static void UpdateHealthNetwork(Block block)
	{
		Raft_Network value = ComponentManager<Raft_Network>.Value;
		if (value == null)
		{
			Debug.LogWarning("Could not update health of block because network reference is null in block creator");
			return;
		}
		Message_BlockCreator_UpdateHealth message = new Message_BlockCreator_UpdateHealth(Messages.BlockCreator_UpdateHealth, value.GetLocalPlayer().BlockCreator, block.ObjectIndex, block.Health, block.MaxHealth);
		if (Raft_Network.IsHost)
		{
			value.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			value.SendP2P(value.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public static void RemoveBlock(Block block, Network_Player playerRemovingBlock, bool updateRaftBounds)
	{
		if (SingletonGeneric<GameManager>.Singleton != null && SingletonGeneric<GameManager>.Singleton.gameObject.activeInHierarchy)
		{
			SingletonGeneric<GameManager>.Singleton.StartCoroutine(RemoveBlockCoroutine(block, playerRemovingBlock, updateRaftBounds));
		}
	}

	private static IEnumerator RemoveBlockCoroutine(Block block, Network_Player playerRemovingBlock, bool updateRaftBounds)
	{
		if (block != null)
		{
			ComponentManager<SoundManager>.Value.PlayBlockBreak(block.transform.position, Block.IsBlockIndexFoundation(block.buildableItem.UniqueIndex) || Block.IsBlockIndexCollectionNet(block.buildableItem.UniqueIndex));
		}
		HashSet<Block> destroyedBlocksFilter = new HashSet<Block>();
		yield return SingletonGeneric<GameManager>.Singleton.StartCoroutine(DestroyBlock(block, destroyedBlocksFilter, playerRemovingBlock, updateRaftBounds));
		List<Block> list = destroyedBlocksFilter.ToList();
		if (updateRaftBounds)
		{
			ComponentManager<RaftBounds>.Value.RemovedWalkableBlocks(list);
		}
		if (RemoveBlockCallStack != null)
		{
			RemoveBlockCallStack(list, playerRemovingBlock);
		}
		if (RemoveBlockCallStackAlways != null)
		{
			RemoveBlockCallStackAlways(list, playerRemovingBlock);
		}
		for (int i = 0; i < list.Count; i++)
		{
			Block block2 = list[i];
			if (!(block2 != null))
			{
				continue;
			}
			if (playerRemovingBlock != null && playerRemovingBlock.IsLocalPlayer && block.buildableItem.settings_buildable.Placeable && block2.buildableItem.settings_buildable.Placeable)
			{
				if (block2.itemToReturnOnDestroy != null && block2.itemToReturnOnDestroy.UniqueIndex != 257)
				{
					playerRemovingBlock.Inventory.AddItem(block2.itemToReturnOnDestroy.UniqueName, 1);
				}
				RemovePlaceables.ReturnItemsFromBlock(block2, playerRemovingBlock, giveItems: true);
			}
			if (block2.networkedBehaviour != null)
			{
				NetworkUpdateManager.RemoveBehaviour(block2.networkedBehaviour);
			}
			UnityEngine.Object.DestroyImmediate(block2.gameObject);
		}
		for (int j = 0; j < placedBlocks.Count; j++)
		{
			if (placedBlocks[j] == null)
			{
				placedBlocks.RemoveAt(j);
				j--;
			}
		}
	}

	public static IEnumerator DestroyBlock(Block block, HashSet<Block> destroyedBlocks, Network_Player playerRemovingBlock, bool updateRaftBounds)
	{
		int? cellIndex = null;
		if (block != null)
		{
			if (ComponentManager<BlockCollisionConsolidator>.Value != null)
			{
				cellIndex = ComponentManager<BlockCollisionConsolidator>.Value.TempActivateCellAndNeighbours(block.transform.position);
			}
			if (block.buildableItem.settings_buildable.PlayParticlesWhenRemoved)
			{
				if (block.buildableItem.settings_buildable.Placeable)
				{
					if (blockBuildParticlesPlaceableStatic != null)
					{
						blockBuildParticlesPlaceableStatic.transform.position = block.transform.position;
						blockBuildParticlesPlaceableStatic.Play();
					}
				}
				else if (blockBreakParticlesStatic != null && block.buildableItem.UniqueIndex != 54)
				{
					blockBreakParticlesStatic.transform.position = block.transform.position;
					blockBreakParticlesStatic.Play(withChildren: true);
				}
			}
			block.gameObject.SetActive(value: false);
			destroyedBlocks.Add(block);
		}
		yield return null;
		List<Block> unstableBlocks = new List<Block>();
		if (Raft_Network.IsHost && block != null)
		{
			foreach (Block placedBlock in placedBlocks)
			{
				if (!(placedBlock == block) && !(placedBlock == null))
				{
					float num = 3.5f;
					if (block.buildableItem != null)
					{
						num = block.buildableItem.settings_buildable.UnstableCheckDistance;
					}
					if (!(Vector3.Distance(placedBlock.transform.position, block.transform.position) > num) && !destroyedBlocks.Contains(placedBlock) && !placedBlock.IsStable())
					{
						destroyedBlocks.Add(placedBlock);
						unstableBlocks.Add(placedBlock);
					}
				}
			}
		}
		if (cellIndex.HasValue)
		{
			ComponentManager<BlockCollisionConsolidator>.Value.RemoveTempActivate(cellIndex.Value);
		}
		if (Raft_Network.IsHost)
		{
			for (int i = 0; i < unstableBlocks.Count; i++)
			{
				RemoveUnstableBlockNetworked(unstableBlocks[i], playerRemovingBlock, updateRaftBounds);
				yield return SingletonGeneric<GameManager>.Singleton.StartCoroutine(DestroyBlock(unstableBlocks[i], destroyedBlocks, playerRemovingBlock, updateRaftBounds));
			}
		}
	}

	public static Block ClosestFoundationToPosition(Vector3 position, bool ignoreYPosition = false)
	{
		float num = float.MaxValue;
		Block block = null;
		for (int i = 0; i < placedBlocks.Count; i++)
		{
			Block block2 = placedBlocks[i];
			if (block2 == null || !Block.IsBlockIndexFoundation(block2.buildableItem.UniqueIndex))
			{
				continue;
			}
			Vector3 position2 = block2.transform.position;
			if (ignoreYPosition)
			{
				position2.y = position.y;
			}
			if (block == null)
			{
				block = block2;
				num = Vector3.Distance(position2, position);
				continue;
			}
			float num2 = Vector3.Distance(position2, position);
			if (num2 < num)
			{
				num = num2;
				block = block2;
			}
		}
		return block;
	}

	public static Block ClosestWalkableFloorToPosition(Vector3 position, bool excludeFloorsAbovePosition, ref bool isHalfFloor)
	{
		float num = float.MaxValue;
		Block block = null;
		for (int i = 0; i < placedBlocks.Count; i++)
		{
			Block block2 = placedBlocks[i];
			if (block2 == null)
			{
				continue;
			}
			bool flag = Block.IsBlockIndexFoundation(block2.buildableItem.UniqueIndex) || Block.IsBlockIndexFloor(block2.buildableItem.UniqueIndex);
			isHalfFloor = false;
			if (!flag && Block.IsBlockIndexRaisedFloor(block2.buildableItem.UniqueIndex))
			{
				flag = true;
				isHalfFloor = true;
			}
			if (!flag)
			{
				continue;
			}
			Vector3 a = block2.transform.position;
			if (isHalfFloor)
			{
				a = block2.transform.position + block2.transform.up * 1.21f;
			}
			if (excludeFloorsAbovePosition && a.y - position.y > 1.21f)
			{
				continue;
			}
			if (block == null)
			{
				block = block2;
				num = Vector3.Distance(a, position);
				continue;
			}
			float num2 = Vector3.Distance(a, position);
			if (num2 < num)
			{
				num = num2;
				block = block2;
			}
		}
		return block;
	}

	public static void SnapBuildPosition(ref Vector3 position)
	{
		if (position.x != 0f)
		{
			position.x = (float)Mathf.RoundToInt(position.x / 0.75f) * 0.75f;
		}
		if (position.z != 0f)
		{
			position.z = (float)Mathf.RoundToInt(position.z / 0.75f) * 0.75f;
		}
		if (position.y != 0f)
		{
			position.y = (float)Mathf.RoundToInt(position.y / 1.21f) * 1.21f;
		}
	}

	public static void SnapBuildPositionWorldToLocalLockedPivot(ref Vector3 worldPosition)
	{
		worldPosition = WorldToLocalLockedPivot(worldPosition);
		SnapBuildPosition(ref worldPosition);
	}

	public static Vector3 WorldToLocalLockedPivot(Vector3 worldPosition)
	{
		return lockedBuildPivot.InverseTransformPoint(worldPosition);
	}

	public static void SnapBuildRotation(ref Vector3 euler, Block blockPrefab)
	{
		switch (blockPrefab.perpendicularAxis)
		{
		case Axis.X:
		case Axis.NX:
			euler.z = (euler.y = 0f);
			if (euler.x != 0f)
			{
				euler.x = Mathf.Round(euler.x / blockPrefab.snapRotateAmount * 10f) / 10f * blockPrefab.snapRotateAmount;
			}
			break;
		case Axis.Y:
		case Axis.NY:
			euler.x = (euler.z = 0f);
			if (euler.y != 0f)
			{
				euler.y = Mathf.Round(euler.y / blockPrefab.snapRotateAmount * 10f) / 10f * blockPrefab.snapRotateAmount;
			}
			break;
		case Axis.Z:
		case Axis.NZ:
			euler.x = (euler.y = 0f);
			if (euler.z != 0f)
			{
				euler.z = Mathf.Round(euler.z / blockPrefab.snapRotateAmount * 10f) / 10f * blockPrefab.snapRotateAmount;
			}
			break;
		}
	}

	public Item_Base GetCurrentBlockType()
	{
		return selectedBuildableItem;
	}

	public void SetBlockTypeToBuild(string uniqueBlockName)
	{
		Item_Base itemByName = ItemManager.GetItemByName(uniqueBlockName);
		SetBlockTypeToBuild(itemByName);
	}

	public void SetBlockTypeToBuild(Item_Base buildableItem)
	{
		selectedBuildableItem = buildableItem;
		selectedBuildablePrefab = selectedBuildableItem.settings_buildable.GetBlockPrefab((quadSurface != null) ? quadSurface.dpsType : DPS.Default);
		if (selectedBuildablePrefab == null)
		{
			DestroyGhostBlock();
			return;
		}
		DestroyGhostBlock();
		Block block = UnityEngine.Object.Instantiate(selectedBuildablePrefab, Vector3.zero, selectedBuildablePrefab.transform.rotation, lockedBuildPivot);
		selectedBlock = block;
		if (selectedBlock.transform.parent != null)
		{
			Vector3 eulerAngles = new Vector3(selectedBlock.transform.parent.eulerAngles.x, selectedBuildablePrefab.transform.eulerAngles.y, selectedBlock.transform.parent.eulerAngles.z);
			selectedBlock.transform.eulerAngles = eulerAngles;
		}
		selectedBlock.OnStartingPlacement();
		if (selectedBlock.occupyingComponent != null && playerNetwork.IsLocalPlayer && SingletonGeneric<GameManager>.Singleton != null)
		{
			selectedBlock.occupyingComponent.SetNewMaterial(SingletonGeneric<GameManager>.Singleton.ghostMaterialRed);
		}
		SetGhostBlockVisibility(visible: false);
	}

	public void SetGhostBlockVisibility(bool visible)
	{
		if (!(selectedBlock == null))
		{
			selectedBlock.gameObject.SetActive(visible);
		}
	}

	private void DestroyGhostBlock()
	{
		if (selectedBlock != null)
		{
			UnityEngine.Object.Destroy(selectedBlock.gameObject);
			selectedBlock = null;
		}
	}

	public void CreateNewGameLayout()
	{
		Item_Base itemByName = ItemManager.GetItemByName("Block_Foundation");
		Vector3[] array = new Vector3[4]
		{
			new Vector3(-0.75f, 0f, -0.75f),
			new Vector3(-0.75f, 0f, 0.75f),
			new Vector3(0.75f, 0f, 0.75f),
			new Vector3(0.75f, 0f, -0.75f)
		};
		for (int i = 0; i < 4; i++)
		{
			Block blockPrefab = itemByName.settings_buildable.GetBlockPrefab(DPS.Default);
			Block block = UnityEngine.Object.Instantiate(blockPrefab, array[i], blockPrefab.transform.rotation, lockedBuildPivot);
			block.transform.localPosition = array[i];
			block.OnStartingPlacement();
			placedBlocks.Add(block);
			block.ObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
			if (block.networkedBehaviour != null)
			{
				NetworkUpdateManager.SetIndexForBehaviour(block.networkedBehaviour);
				NetworkUpdateManager.AddBehaviour(block.networkedBehaviour);
			}
			block.OnFinishedPlacement();
			if (PlaceBlockCallStack != null)
			{
				PlaceBlockCallStack(block, playerNetwork, arg3: true, -1);
			}
			if (PlaceBlockCallStackAlways != null)
			{
				PlaceBlockCallStackAlways(block, playerNetwork, arg3: true, -1);
			}
			FoundationCount++;
		}
	}

	public Block CreateBlockCheat(Item_Base blockItem, Vector3 localBuildPosition, Vector3 localBuildRotation, DPS dpsType, int hotslotIndex)
	{
		if (!Raft_Network.IsHost)
		{
			return null;
		}
		Block block = CreateBlock(blockItem, localBuildPosition, localBuildRotation, dpsType, hotslotIndex, replicating: false, SaveAndLoad.GetUniqueObjectIndex(), SaveAndLoad.GetUniqueObjectIndex(), NetworkUpdateManager.GetUniqueBehaviourIndex());
		Message_BlockCreator_PlaceBlock message = new Message_BlockCreator_PlaceBlock(Messages.BlockCreator_PlaceBlock, this, blockItem.UniqueIndex, block.ObjectIndex, (!(block.networkedBehaviour == null)) ? block.networkedBehaviour.ObjectIndex : 0u, (!(block.networkedBehaviour == null)) ? block.networkedBehaviour.BehaviourIndex : 0u, block.transform.localPosition, block.transform.localEulerAngles, hotslotIndex, dpsType);
		playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		return block;
	}

	public Block CreateBlock(Item_Base blockItem, Vector3 localBuildPosition, Vector3 localBuildRotation, DPS dpsType, int hotslotIndex, bool replicating, uint blockObjectIndex, uint networkedObjectIndex, uint networkedBehaviourIndex)
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (collisionManager == null)
		{
			collisionManager = ComponentManager<RaftCollisionManager>.Value;
		}
		if (lockedBuildPivot == null)
		{
			lockedBuildPivot = SingletonGeneric<GameManager>.Singleton.lockedPivot;
		}
		if (blockItem == null)
		{
			return null;
		}
		bool placeable = blockItem.settings_buildable.Placeable;
		Block blockPrefab = blockItem.settings_buildable.GetBlockPrefab(dpsType);
		if (blockPrefab == null)
		{
			Debug.LogError("Unable to Create Block Prefab" + blockItem.UniqueName);
			return null;
		}
		if (!placeable)
		{
			SnapBuildPosition(ref localBuildPosition);
			SnapBuildRotation(ref localBuildRotation, blockPrefab);
		}
		if (!replicating)
		{
			if (canvas != null)
			{
				canvas.displayTextManager.HideDisplayTexts();
			}
			if (playerNetwork != null)
			{
				playerNetwork.PlayerScript.SetMouseLookScripts(canLook: true);
			}
		}
		DestroyGhostBlock();
		upgradePreview.ResetPreview();
		Block block = UnityEngine.Object.Instantiate(blockPrefab, localBuildPosition, blockPrefab.transform.rotation, lockedBuildPivot);
		block.transform.localPosition = localBuildPosition;
		block.transform.localEulerAngles = localBuildRotation;
		block.OnStartingPlacement();
		if (!replicating)
		{
			if (placeable)
			{
				RuntimeManager.PlayOneShot(er_placeBlock, block.transform.position);
				blockBuildParticlesPlaceable.transform.position = block.transform.position;
				blockBuildParticlesPlaceable.Play();
			}
			else
			{
				RuntimeManager.PlayOneShot(er_createBlock, block.transform.position);
				blockBuildParticles.transform.position = block.transform.position;
				blockBuildParticles.Play();
			}
			SimpleMonoBehaviourSingleton<HapticManager>.Instance?.PlayOneShot(HapticEvent.Build);
			if (!placeable && !Cheat.UseGodMode)
			{
				if (playerNetwork.IsLocalPlayer)
				{
					playerNetwork.Inventory.RemoveCostMultiple(block.buildableItem.settings_recipe.NewCost);
				}
			}
			else
			{
				playerNetwork.Animator.SetAnimation(PlayerAnimation.Trigger_PointPlace, triggering: true);
			}
		}
		placedBlocks.Add(block);
		if (block.occupyingComponent != null)
		{
			block.occupyingComponent.RestoreToDefaultMaterial();
		}
		if (blockObjectIndex == 0)
		{
			blockObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
		}
		block.ObjectIndex = blockObjectIndex;
		if (block.networkedBehaviour != null)
		{
			if (networkedObjectIndex == 0)
			{
				networkedObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
			}
			if (networkedBehaviourIndex == 0)
			{
				networkedBehaviourIndex = NetworkUpdateManager.GetUniqueBehaviourIndex();
			}
			block.networkedBehaviour.ObjectIndex = networkedObjectIndex;
			block.networkedBehaviour.BehaviourIndex = networkedBehaviourIndex;
			NetworkUpdateManager.AddBehaviour(block.networkedBehaviour);
		}
		block.OnFinishedPlacement();
		if (block.buildableItem.UniqueName.Contains("Foundation"))
		{
			FoundationCount++;
		}
		if (block.transform.localPosition.y > MaxHeight)
		{
			MaxHeight = block.transform.localPosition.y;
			FloorCount = Mathf.Clamp((int)(MaxHeight / 1.21f), 0, int.MaxValue);
		}
		selectedBuildableItem = null;
		if (PlaceBlockCallStack != null && !replicating)
		{
			PlaceBlockCallStack(block, playerNetwork, arg3: false, hotslotIndex);
		}
		if (PlaceBlockCallStackAlways != null)
		{
			PlaceBlockCallStackAlways(block, playerNetwork, replicating, hotslotIndex);
		}
		if (!Raft_Network.IsHost)
		{
			block.transform.localPosition = localBuildPosition;
		}
		if (!replicating && playerNetwork.IsLocalPlayer)
		{
			if (placeable)
			{
				selectedBlock = null;
				if (hotslotIndex >= 0)
				{
					Slot slot = playerInventory.GetSlot(hotslotIndex);
					if (slot.IsEmpty)
					{
						Debug.Log("Can't remove placeable block, slot is empty");
					}
					else
					{
						slot.RemoveItem(1);
					}
					if (slot.IsEmpty)
					{
						playerInventory.hotbar.ReselectCurrentSlot();
					}
					else
					{
						SetBlockTypeToBuild(blockItem.UniqueName);
					}
				}
			}
			else
			{
				SetBlockTypeToBuild(blockItem.UniqueName);
			}
		}
		return placedBlocks.Last();
	}

	public Block CreateDetailPlank(Item_Base blockItem, Vector3 localBuildPosition, Vector3 localBuildRotation, DPS dpsType, int hotslotIndex, bool replicating, uint blockObjectIndex, uint networkedObjectIndex, uint networkedBehaviourIndex, float length)
	{
		Block block = CreateBlock(blockItem, localBuildPosition, localBuildRotation, dpsType, hotslotIndex, replicating, blockObjectIndex, networkedObjectIndex, networkedBehaviourIndex);
		Block_DetailPlank block_DetailPlank = block as Block_DetailPlank;
		if (block_DetailPlank != null)
		{
			block_DetailPlank.lengthSetter.SetLength(length);
		}
		return block;
	}

	public Network_Player GetPlayerNetwork()
	{
		return playerNetwork;
	}

	public void OnHammerSelect()
	{
		if (playerNetwork == null || !playerNetwork.IsLocalPlayer || (!(selectedBlock == null) && (!(selectedBlock.buildableItem != null) || !selectedBlock.buildableItem.settings_buildable.Placeable)))
		{
			return;
		}
		quadSurface = null;
		if (buildMenu != null)
		{
			if (buildMenu.lastSelectedBuildabe == null)
			{
				SetBlockTypeToBuild("Block_Foundation");
			}
			else
			{
				SetBlockTypeToBuild(buildMenu.lastSelectedBuildabe.UniqueName);
			}
		}
	}

	public void OnHammerDeselect()
	{
		DestroyGhostBlock();
		upgradePreview.ResetPreview();
		if (playerNetwork != null && playerNetwork.IsLocalPlayer && buildMenu != null)
		{
			buildMenu.OnHammerDeselect();
		}
	}

	private BuildError CanBuildBlock(Block block)
	{
		if (block == null)
		{
			return BuildError.UnKnown;
		}
		if (CanvasHelper.ActiveMenu != MenuType.None)
		{
			return BuildError.InMenu;
		}
		if (block.transform.localPosition.y < -0.5f)
		{
			return BuildError.InWater;
		}
		switch (block.IsOverlapping())
		{
		case OverlappType.Overlapp:
			return BuildError.PositionOccupied;
		case OverlappType.Overlapp_Same:
			return BuildError.PositionOccupied_Same;
		default:
			if (!block.IsStable())
			{
				return BuildError.NotStable;
			}
			if (!HasEnoughResourcesToBuild(block))
			{
				return BuildError.NoResources;
			}
			return BuildError.None;
		}
	}

	public bool HasEnoughResourcesToBuild(CostMultiple[] buildCost)
	{
		if (buildCost == null)
		{
			return false;
		}
		if (!Cheat.UseGodMode)
		{
			for (int i = 0; i < buildCost.Length; i++)
			{
				if (!buildCost[i].HasEnoughInInventory(playerInventory))
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool HasEnoughResourcesToBuild(Block block)
	{
		if (block == null)
		{
			return false;
		}
		if (block.buildableItem.settings_buildable.Placeable)
		{
			return true;
		}
		return HasEnoughResourcesToBuild(block.buildableItem.settings_recipe.NewCost);
	}

	private void HandleRotationOfSelectedBlock()
	{
		if (!selectedBlock.isRotateable)
		{
			return;
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputRotate, "Rotate"))
		{
			if (selectedBlock.canRotateFreely)
			{
				isRotating = true;
				playerNetwork.PlayerScript.SetMouseLookScripts(canLook: false);
			}
			else
			{
				RotateBlock(selectedBlock, selectedBlock.rotateAmount, snap: true);
			}
		}
		if (!selectedBlock.canRotateFreely)
		{
			return;
		}
		float num = 0.2f;
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputRotate, "Rotate"))
		{
			rotationKeyAccumulation += Time.deltaTime;
			if (rotationKeyAccumulation >= num)
			{
				float xAxis = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.GetXAxis(inputRotateDelta, "Mouse X");
				RotateBlock(selectedBlock, xAxis * 5f);
			}
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(inputRotate, "Rotate"))
		{
			if (rotationKeyAccumulation < num)
			{
				RotateBlock(selectedBlock, selectedBlock.rotateAmount, snap: true);
			}
			rotationKeyAccumulation = 0f;
			isRotating = false;
			playerNetwork.PlayerScript.SetMouseLookScripts(canLook: true);
		}
	}

	private void HandleMirroredVersion()
	{
		if (selectedBlock != null && selectedBlock.buildableItem.settings_buildable.MirroredVersion != null)
		{
			Item_Base mirroredVersion = selectedBlock.buildableItem.settings_buildable.MirroredVersion;
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Special"];
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Mirror"))
			{
				SetBlockTypeToBuild(mirroredVersion);
			}
		}
	}

	private void HandleUpgrade()
	{
		if (Helper.HitAtCursor(out var hit, Player.UseDistance * 2f, LayerMasks.MASK_Block))
		{
			Block componentInParent = hit.transform.GetComponentInParent<Block>();
			if (componentInParent != null && componentInParent.buildableItem != null && componentInParent.buildableItem.settings_buildable.Upgrades.HasUpgrades())
			{
				Item_Base newItemFromUpgradeItem = componentInParent.buildableItem.settings_buildable.Upgrades.GetNewItemFromUpgradeItem(selectedBuildableItem);
				upgradePreview.ShowPreview(componentInParent, newItemFromUpgradeItem);
				bool flag = HasEnoughResourcesToBuild(upgradePreview.ghostBlock);
				upgradePreview.SetMaterialFromResources(flag);
				if (!flag || !(upgradePreview.block != null) || !(upgradePreview.ghostBlock != null))
				{
					return;
				}
				bool flag2 = GeneralSettingsBox.AllowQuickBuild && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputUse.action, "LMB") && quickBuildTimer <= 0f;
				if (!(SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputUse, "LMB") | flag2))
				{
					return;
				}
				if (flag2)
				{
					quickBuildTimer = 0.25f;
				}
				if (newItemFromUpgradeItem == null)
				{
					return;
				}
				Block blockPrefab = newItemFromUpgradeItem.settings_buildable.GetBlockPrefab(upgradePreview.block.dpsType);
				if (blockPrefab == null)
				{
					return;
				}
				Message_BlockCreator_UpgradeBlock message_BlockCreator_UpgradeBlock = new Message_BlockCreator_UpgradeBlock(Messages.BlockCreator_UpgradeBlock, this, blockPrefab.buildableItem.UniqueIndex, Raft_Network.IsHost ? SaveAndLoad.GetUniqueObjectIndex() : 0u, (Raft_Network.IsHost && blockPrefab.networkedBehaviour != null) ? SaveAndLoad.GetUniqueObjectIndex() : 0u, (Raft_Network.IsHost && blockPrefab.networkedBehaviour != null) ? NetworkUpdateManager.GetUniqueBehaviourIndex() : 0u, upgradePreview.block.transform.localPosition, upgradePreview.block.transform.localEulerAngles, -1, upgradePreview.block.dpsType, upgradePreview.block.ObjectIndex, selectedBuildableItem.UniqueIndex, playerNetwork.steamID);
				if (Raft_Network.IsHost)
				{
					if (UpgradeBlock(blockPrefab.buildableItem, message_BlockCreator_UpgradeBlock.LocalPosition, message_BlockCreator_UpgradeBlock.LocalEuler, message_BlockCreator_UpgradeBlock.dpsType, message_BlockCreator_UpgradeBlock.hotSlotIndex, replicating: false, message_BlockCreator_UpgradeBlock.blockObjectIndex, message_BlockCreator_UpgradeBlock.networkedObjectIndex, message_BlockCreator_UpgradeBlock.networkedBehaviourIndex, upgradePreview.block, selectedBuildableItem, playerNetwork) != null)
					{
						playerNetwork.Network.RPC(message_BlockCreator_UpgradeBlock, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					}
				}
				else
				{
					playerNetwork.Network.SendP2P(playerNetwork.Network.HostID, message_BlockCreator_UpgradeBlock, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
			else
			{
				upgradePreview.ResetPreview();
			}
		}
		else
		{
			upgradePreview.ResetPreview();
		}
	}

	private Block UpgradeBlock(Item_Base newBlockItem, Vector3 localBuildPosition, Vector3 localBuildRotation, DPS dpsType, int hotslotIndex, bool replicating, uint blockObjectIndex, uint networkedObjectIndex, uint networkedBehaviourIndex, Block oldBlock, Item_Base upgradeItem, Network_Player playerUpgrading)
	{
		if (oldBlock != null && newBlockItem != null && upgradeItem != null)
		{
			Block block = CreateBlock(newBlockItem, localBuildPosition, localBuildRotation, dpsType, hotslotIndex, replicating, blockObjectIndex, networkedObjectIndex, networkedBehaviourIndex);
			if (block == null)
			{
				return null;
			}
			if (block is Block_Foundation && oldBlock is Block_Foundation)
			{
				Block_Foundation block_Foundation = oldBlock as Block_Foundation;
				Block_Foundation block_Foundation2 = block as Block_Foundation;
				if (block_Foundation != null && block_Foundation2 != null)
				{
					block_Foundation2.Reinforced = block_Foundation.Reinforced;
				}
			}
			if (Raft_Network.IsHost && oldBlock.buildableItem.settings_buildable.ReturnPlaceablesWhenUpgraded)
			{
				foreach (BoxCollider allAdvBoxCollider in oldBlock.occupyingComponent.allAdvBoxColliders)
				{
					if (allAdvBoxCollider == null)
					{
						continue;
					}
					Collider[] collisions = Helper.GetCollisions(allAdvBoxCollider, LayerMasks.MASK_Block, oldBlock.occupyingComponent.allAdvBoxColliders.ToArray(), Vector3.one, QueryTriggerInteraction.Collide);
					foreach (Collider collider in collisions)
					{
						if (collider == null)
						{
							continue;
						}
						Block block2 = collider.GetComponent<Block>();
						if (block2 == null)
						{
							block2 = collider.GetComponentInParent<Block>();
						}
						if (!(block2 != null) || !block2.buildableItem.settings_buildable.Placeable || block2.dpsType != DPS.Wall || !(block2.stableComponent != null))
						{
							continue;
						}
						GizmoBox[] requiredGizmoColliders = block2.stableComponent.requiredGizmoColliders;
						foreach (GizmoBox gizmoBox in requiredGizmoColliders)
						{
							if (gizmoBox == null)
							{
								continue;
							}
							bool flag = false;
							foreach (Collider overlap in gizmoBox.GetOverlaps(includeInactiveObjects: false, LayerMasks.MASK_Block, 1f))
							{
								if (!(overlap == null))
								{
									Block block3 = overlap.GetComponent<Block>();
									if (block3 == null)
									{
										block3 = overlap.GetComponentInParent<Block>();
									}
									if (block3 != null && block3 == oldBlock)
									{
										RemoveBlockNetwork(block2, playerUpgrading, updateRaftBounds: false);
										flag = true;
										break;
									}
								}
							}
							if (flag)
							{
								break;
							}
						}
					}
				}
			}
			if (playerNetwork == null)
			{
				playerNetwork = GetComponentInParent<Network_Player>();
			}
			if (playerNetwork != null)
			{
				playerNetwork.AxeScript.DestroyBlock(oldBlock, updateRaftBounds: false);
			}
			upgradePreview.ResetPreview();
			SetBlockTypeToBuild(upgradeItem);
			return block;
		}
		return null;
	}

	public bool IsBuildableItemUpgradeItem(Item_Base buildableItem)
	{
		if (buildableItem != null)
		{
			if (buildableItem.UniqueIndex != 546 && buildableItem.UniqueIndex != 547)
			{
				return buildableItem.UniqueIndex == 548;
			}
			return true;
		}
		return false;
	}

	private void RotateBlock(Block block, float degrees, bool snap = false)
	{
		if (!(block == null))
		{
			float currentRotationY = selectedBuildablePrefab.currentRotationY;
			currentRotationY += degrees;
			if (snap)
			{
				currentRotationY = (float)Mathf.RoundToInt(currentRotationY / block.snapRotateAmount) * block.snapRotateAmount;
			}
			if (currentRotationY > 360f)
			{
				currentRotationY -= 360f;
			}
			selectedBuildablePrefab.currentRotationY = currentRotationY;
			Transform transform = block.transform;
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentRotationY, transform.eulerAngles.z);
		}
	}

	private void SetGhostBlockPositionAndRotation()
	{
		if (selectedBlock == null || quadAtCursor == null)
		{
			return;
		}
		if (selectedBlock.snapsToQuads || quadAtCursor.snapToQuadPosition)
		{
			selectedBlock.transform.position = quadAtCursor.transform.position;
		}
		else
		{
			selectedBlock.transform.position = quadHit.point;
			Debug.DrawRay(quadHit.point, quadHit.normal, Color.magenta);
		}
		if (selectedBlock.forceRotationToQuadDirection)
		{
			selectedBlock.transform.rotation = Quaternion.LookRotation(quadAtCursor.transform.forward, quadAtCursor.transform.up);
			selectedBlock.transform.Rotate(GetPerpendicularDirection(selectedBlock), selectedBuildablePrefab.currentRotationY, Space.World);
		}
		else if (quadAtCursor.snapToQuadRotation)
		{
			AlignBlockPerpendicular(selectedBlock, quadAtCursor.transform.up);
		}
		else
		{
			AlignBlockPerpendicular(selectedBlock, quadHit.normal);
		}
		if (!(selectedBlock.pivotOffset != Vector3.zero))
		{
			return;
		}
		Vector3 pivotOffset = selectedBlock.pivotOffset;
		Vector3 one = Vector3.one;
		if (selectedBlock.flipPivot)
		{
			Vector3 vector = quadAtCursor.transform.InverseTransformPoint(quadHit.point);
			if (vector.z < 0f)
			{
				one.z *= -1f;
			}
			if (vector.x < 0f)
			{
				one.x *= -1f;
			}
			if (vector.y < 0f)
			{
				one.y *= -1f;
			}
		}
		else
		{
			if (quadAtCursor.transform.forward != quadHit.normal)
			{
				one.z *= -1f;
			}
			if (quadAtCursor.transform.right != quadHit.normal)
			{
				one.x *= -1f;
			}
			if (quadAtCursor.transform.up != quadHit.normal)
			{
				one.y *= -1f;
			}
		}
		Vector3 vector2 = quadAtCursor.transform.forward * pivotOffset.z * one.z + quadAtCursor.transform.right * pivotOffset.x * one.x + quadAtCursor.transform.up * pivotOffset.y * one.y;
		selectedBlock.transform.position += vector2;
	}

	private BlockQuad GetQuadAtCursor()
	{
		RaycastHit[] array = Physics.RaycastAll(playerNetwork.CameraTransform.position, playerNetwork.CameraTransform.forward, Player.UseDistance * 2f, LayerMasks.MASK_BuildQuad);
		if (array.Length != 0)
		{
			BlockQuadHitInfo blockQuadHitInfo = default;
			float num = float.MaxValue;
			for (int i = 0; i < array.Length; i++)
			{
				RaycastHit hit = array[i];
				if (hit.transform == null)
				{
					continue;
				}
				BlockQuad component = hit.collider.transform.GetComponent<BlockQuad>();
				if (!(component != null) || !component.AcceptsBlock(selectedBuildableItem, hit.normal))
				{
					continue;
				}
				float sqrMagnitude = (transform.position - hit.point).sqrMagnitude;
				if (num != float.MaxValue && !(Mathf.Abs(sqrMagnitude - num) <= 0.05f))
				{
					continue;
				}
				if (blockQuadHitInfo.valid && blockQuadHitInfo.quad.ParentBlock != null && component.ParentBlock != null)
				{
					float sqrMagnitude2 = (blockQuadHitInfo.quad.ParentBlock.transform.position - transform.position).sqrMagnitude;
					if ((component.ParentBlock.transform.position - transform.position).sqrMagnitude < sqrMagnitude2)
					{
						blockQuadHitInfo.valid = false;
					}
				}
				if (!blockQuadHitInfo.valid)
				{
					blockQuadHitInfo.valid = true;
					num = sqrMagnitude;
					blockQuadHitInfo.quad = component;
					blockQuadHitInfo.hit = hit;
				}
			}
			if (blockQuadHitInfo.valid)
			{
				quadHit = blockQuadHitInfo.hit;
				quadSurface = blockQuadHitInfo.quad.GetSurfaceFromNormal(blockQuadHitInfo.hit.normal);
				return blockQuadHitInfo.quad;
			}
		}
		quadSurface = null;
		return null;
	}

	private void AlignBlockPerpendicular(Block block, Vector3 normalDirection)
	{
		switch (block.perpendicularAxis)
		{
		case Axis.Z:
			block.transform.forward = normalDirection;
			break;
		case Axis.Y:
			block.transform.up = normalDirection;
			break;
		case Axis.X:
			block.transform.right = normalDirection;
			break;
		case Axis.NZ:
			block.transform.forward = -normalDirection;
			break;
		case Axis.NY:
			block.transform.up = -normalDirection;
			break;
		case Axis.NX:
			block.transform.right = -normalDirection;
			break;
		}
		float num = selectedBuildablePrefab.currentRotationY;
		if (!block.isRotateable)
		{
			num = 0f;
		}
		Vector3 localEulerAngles = block.transform.localEulerAngles;
		switch (block.perpendicularAxis)
		{
		case Axis.Z:
		case Axis.NZ:
			localEulerAngles.z = num;
			break;
		case Axis.Y:
		case Axis.NY:
			localEulerAngles.y = num;
			break;
		case Axis.X:
		case Axis.NX:
			localEulerAngles.x = num;
			break;
		}
		block.transform.localEulerAngles = localEulerAngles;
	}

	private Vector3 GetPerpendicularDirection(Block block)
	{
		Transform transform = block.transform;
		return block.perpendicularAxis switch
		{
			Axis.X => transform.right, 
			Axis.Y => transform.up, 
			Axis.Z => transform.forward, 
			Axis.NX => -transform.right, 
			Axis.NY => -transform.up, 
			Axis.NZ => -transform.forward, 
			_ => transform.up, 
		};
	}

	private void HandleBlockPick()
	{
		if (selectedBlock != null && selectedBlock.buildableItem.settings_buildable.Placeable)
		{
			canvas.displayTextManager.HideDisplayTexts(3);
			return;
		}
		RaycastHit hitInfo = default;
		bool flag = Physics.Raycast(playerNetwork.CameraTransform.position, playerNetwork.CameraTransform.forward, out hitInfo, Player.UseDistance * 2f, LayerMasks.MASK_Block);
		if (flag)
		{
			canvas.displayTextManager.ShowText(Helper.GetTerm("Controls/BlockPick"), inputBlockPick.action.GetBindingDisplayString(), MyInput.Keybinds["BlockPick"].MainKey, 3, 0, clearAllTexts: false);
		}
		else
		{
			canvas.displayTextManager.HideDisplayTexts(3);
		}
		if (flag && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputBlockPick, "BlockPick"))
		{
			Block componentInParent = hitInfo.transform.GetComponentInParent<Block>();
			if (componentInParent != null && !componentInParent.buildableItem.settings_buildable.Placeable)
			{
				DestroyGhostBlock();
				upgradePreview.ResetPreview();
				buildMenu.SelectBlock(componentInParent.buildableItem);
			}
		}
	}

	private void DebugCreatorSize()
	{
		Message_NetworkBehaviour createBlocksMessage = GetCreateBlocksMessage();
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		MemoryStream memoryStream = new MemoryStream();
		binaryFormatter.Serialize(memoryStream, createBlocksMessage);
		Debug.Log("BlockCreator create message size = " + memoryStream.Length);
	}

	public override void Serialize_Create()
	{
		if (playerNetwork.steamID == network.HostID)
		{
			Message_NetworkBehaviour createBlocksMessage = GetCreateBlocksMessage();
			if (createBlocksMessage != null)
			{
				AddMessage(createBlocksMessage);
			}
			AddMessage(new Message_NetworkBehaviour(Messages.BlockCreator_CreateEndMessage, this));
		}
	}

	private Message_NetworkBehaviour GetCreateBlocksMessage()
	{
		if (placedBlocks.ContainsItems())
		{
			List<RGD_Block> list = new List<RGD_Block>();
			for (int i = 0; i < placedBlocks.Count; i++)
			{
				Block block = placedBlocks[i];
				if (!(block == null))
				{
					list.Add(block.GetBlockCreationData());
				}
			}
			return new Message_BlockCreator_Create(Messages.BlockCreator_Create, this, list.ToArray());
		}
		return null;
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.BlockCreator_Create:
		{
			Message_BlockCreator_Create message_BlockCreator_Create = msg as Message_BlockCreator_Create;
			if (message_BlockCreator_Create.rgdBlocks != null)
			{
				for (int num = 0; num < message_BlockCreator_Create.rgdBlocks.Length; num++)
				{
					RGD_Block rGD_Block = message_BlockCreator_Create.rgdBlocks[num];
					if (rGD_Block != null)
					{
						Block block2 = CreateBlock(ItemManager.GetItemByIndex(rGD_Block.BlockIndex), rGD_Block.LocalBlockPosition, rGD_Block.LocalBlockRotation, rGD_Block.DPSType, -1, replicating: true, rGD_Block.BlockObjectIndex, rGD_Block.NetworkedObjectIndex, rGD_Block.NetworkedBehaviourIndex);
						rGD_Block.RestoreBlock(block2);
					}
				}
			}
			return false;
		}
		case Messages.BlockCreator_CreateEndMessage:
		{
			RaftBounds value = ComponentManager<RaftBounds>.Value;
			if (value != null)
			{
				value.Initialize();
			}
			return false;
		}
		case Messages.BlockCreator_PlaceBlock:
		{
			Message_BlockCreator_PlaceBlock message_BlockCreator_PlaceBlock = msg as Message_BlockCreator_PlaceBlock;
			Block block6 = CreateBlock(ItemManager.GetItemByIndex(message_BlockCreator_PlaceBlock.uniqueBlockIndex), message_BlockCreator_PlaceBlock.LocalPosition, message_BlockCreator_PlaceBlock.LocalEuler, message_BlockCreator_PlaceBlock.dpsType, Raft_Network.IsHost ? (-1) : message_BlockCreator_PlaceBlock.hotSlotIndex, replicating: false, message_BlockCreator_PlaceBlock.blockObjectIndex, message_BlockCreator_PlaceBlock.networkedObjectIndex, message_BlockCreator_PlaceBlock.networkedBehaviourIndex);
			if (Raft_Network.IsHost)
			{
				if (block6.IsOverlapping() == OverlappType.None)
				{
					message_BlockCreator_PlaceBlock.LocalPosition = block6.transform.localPosition;
					message_BlockCreator_PlaceBlock.LocalEuler = block6.transform.localEulerAngles;
					message_BlockCreator_PlaceBlock.blockObjectIndex = block6.ObjectIndex;
					if (block6.networkedBehaviour != null)
					{
						message_BlockCreator_PlaceBlock.networkedObjectIndex = block6.networkedBehaviour.ObjectIndex;
						message_BlockCreator_PlaceBlock.networkedBehaviourIndex = block6.networkedBehaviour.BehaviourIndex;
					}
					return true;
				}
				UnityEngine.Object.Destroy(block6.gameObject);
				return false;
			}
			return false;
		}
		case Messages.BlockCreator_PlaceBlockDetailPlank:
		{
			Message_BlockCreator_PlaceBlock_DetailPlank message_BlockCreator_PlaceBlock_DetailPlank = msg as Message_BlockCreator_PlaceBlock_DetailPlank;
			Block block3 = playerNetwork.BlockCreator.CreateDetailPlank(ItemManager.GetItemByIndex(message_BlockCreator_PlaceBlock_DetailPlank.uniqueBlockIndex), message_BlockCreator_PlaceBlock_DetailPlank.LocalPosition, message_BlockCreator_PlaceBlock_DetailPlank.LocalEuler, message_BlockCreator_PlaceBlock_DetailPlank.dpsType, message_BlockCreator_PlaceBlock_DetailPlank.hotSlotIndex, replicating: false, message_BlockCreator_PlaceBlock_DetailPlank.blockObjectIndex, message_BlockCreator_PlaceBlock_DetailPlank.networkedObjectIndex, message_BlockCreator_PlaceBlock_DetailPlank.networkedBehaviourIndex, message_BlockCreator_PlaceBlock_DetailPlank.length);
			if (Raft_Network.IsHost)
			{
				if (block3.IsOverlapping() == OverlappType.None)
				{
					message_BlockCreator_PlaceBlock_DetailPlank.LocalPosition = block3.transform.localPosition;
					message_BlockCreator_PlaceBlock_DetailPlank.LocalEuler = block3.transform.localEulerAngles;
					message_BlockCreator_PlaceBlock_DetailPlank.blockObjectIndex = block3.ObjectIndex;
					if (block3.networkedBehaviour != null)
					{
						message_BlockCreator_PlaceBlock_DetailPlank.networkedObjectIndex = block3.networkedBehaviour.ObjectIndex;
						message_BlockCreator_PlaceBlock_DetailPlank.networkedBehaviourIndex = block3.networkedBehaviour.BehaviourIndex;
					}
					return true;
				}
				UnityEngine.Object.Destroy(block3.gameObject);
				return false;
			}
			return false;
		}
		case Messages.BlockCreator_UpgradeBlock:
		{
			Message_BlockCreator_UpgradeBlock message_BlockCreator_UpgradeBlock = msg as Message_BlockCreator_UpgradeBlock;
			Network_Player playerFromID2 = network.GetPlayerFromID(message_BlockCreator_UpgradeBlock.PlayerUpgradingSteamID);
			Block block5 = UpgradeBlock(ItemManager.GetItemByIndex(message_BlockCreator_UpgradeBlock.uniqueBlockIndex), message_BlockCreator_UpgradeBlock.LocalPosition, message_BlockCreator_UpgradeBlock.LocalEuler, message_BlockCreator_UpgradeBlock.dpsType, Raft_Network.IsHost ? (-1) : message_BlockCreator_UpgradeBlock.hotSlotIndex, replicating: false, message_BlockCreator_UpgradeBlock.blockObjectIndex, message_BlockCreator_UpgradeBlock.networkedObjectIndex, message_BlockCreator_UpgradeBlock.networkedBehaviourIndex, GetBlockByObjectIndex(message_BlockCreator_UpgradeBlock.removeBlockObjectIndex), ItemManager.GetItemByIndex(message_BlockCreator_UpgradeBlock.upgradeItemIndex), playerFromID2);
			if (Raft_Network.IsHost)
			{
				if (block5 != null)
				{
					message_BlockCreator_UpgradeBlock.LocalPosition = block5.transform.localPosition;
					message_BlockCreator_UpgradeBlock.LocalEuler = block5.transform.localEulerAngles;
					message_BlockCreator_UpgradeBlock.blockObjectIndex = block5.ObjectIndex;
					if (block5.networkedBehaviour != null)
					{
						message_BlockCreator_UpgradeBlock.networkedObjectIndex = block5.networkedBehaviour.ObjectIndex;
						message_BlockCreator_UpgradeBlock.networkedBehaviourIndex = block5.networkedBehaviour.BehaviourIndex;
					}
				}
				return block5 != null;
			}
			return false;
		}
		case Messages.BlockCreator_RemoveBlock:
		{
			Message_BlockCreator_RemoveBlock msgRemove = msg as Message_BlockCreator_RemoveBlock;
			Block block7 = placedBlocks.Find((Block b) => b.ObjectIndex == msgRemove.blockObjectIndex);
			if (block7 == null)
			{
				Debug.Log(Messages.BlockCreator_RemoveBlock.ToString() + ", NULL: Could not remove block because no block with index " + msgRemove.blockObjectIndex + " could be found");
				return false;
			}
			if (network == null)
			{
				network = ComponentManager<Raft_Network>.Value;
			}
			Network_Player playerFromID3 = network.GetPlayerFromID(msgRemove.SteamID);
			RemoveBlock(block7, playerFromID3, msgRemove.UpdateBounds);
			return true;
		}
		case Messages.BlockCreator_UpdateHealth:
		{
			Message_BlockCreator_UpdateHealth msgHealth = msg as Message_BlockCreator_UpdateHealth;
			Block block4 = placedBlocks.Find((Block b) => b.ObjectIndex == msgHealth.blockObjectIndex);
			if (block4 != null)
			{
				block4.MaxHealth = msgHealth.newMaxHealth;
				block4.SetHealth(msgHealth.newHealth);
			}
			return true;
		}
		case Messages.BlockCreator_SetReinforced:
		{
			Message_BlockCreator_SetReinforced msgFortify = msg as Message_BlockCreator_SetReinforced;
			Block block = placedBlocks.Find((Block b) => b.ObjectIndex == msgFortify.blockObjectIndex);
			Network_Player playerFromID = network.GetPlayerFromID(msgFortify.SteamID);
			if (block != null && playerFromID != null)
			{
				return playerFromID.HammerScript.ReinforceBlock(block);
			}
			Debug.LogError("Unable to find block to fortify, or player...!");
			return false;
		}
		default:
			return false;
		}
	}

	private IEnumerator CreateBlockLate(Block newBlock, Message_BlockCreator_PlaceBlock msg)
	{
		yield return new WaitForSeconds(buildDelay);
		if (newBlock.IsOverlapping() == OverlappType.None)
		{
			msg.LocalPosition = newBlock.transform.localPosition;
			msg.LocalEuler = newBlock.transform.localEulerAngles;
			msg.blockObjectIndex = newBlock.ObjectIndex;
			if (newBlock.networkedBehaviour != null)
			{
				msg.networkedObjectIndex = newBlock.networkedBehaviour.ObjectIndex;
				msg.networkedBehaviourIndex = newBlock.networkedBehaviour.BehaviourIndex;
			}
			playerNetwork.Network.RPC(msg, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			UnityEngine.Object.Destroy(newBlock.gameObject);
		}
	}
}
