using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hammer : MonoBehaviour
{
	public EventReference er_reinforce;

	[SerializeField]
	private int blockRepairAmount;

	[SerializeField]
	private BlockCreator blockCreator;

	[SerializeField]
	private ParticleController woodParticles;

	[SerializeField]
	private Item_Base repairItem;

	[SerializeField]
	private Item_Base reinforceItem;

	[SerializeField]
	private GhostDisplayer reinForceGhostPrefab;

	private GhostDisplayer reinforceGhostObject;

	private Network_Player playerNetwork;

	private CanvasHelper canvas;

	public EventReference er_swing;

	[SerializeField]
	private InputActionReference inputUse;

	private void Start()
	{
		canvas = ComponentManager<CanvasHelper>.Value;
		playerNetwork = GetComponentInParent<Network_Player>();
		woodParticles.particleParent.SetParent(null);
	}

	private void Update()
	{
		if (playerNetwork.IsLocalPlayer)
		{
			HandleRepairingAndReinforcement();
		}
	}

	public void OnSelect()
	{
		blockCreator.gameObject.SetActive(value: true);
		blockCreator.OnHammerSelect();
	}

	public void OnDeSelect()
	{
		blockCreator.OnHammerDeselect();
		blockCreator.SetGhostBlockVisibility(visible: false);
		blockCreator.gameObject.SetActive(value: false);
		if (canvas != null)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
		if (reinforceGhostObject != null)
		{
			reinforceGhostObject.Remove();
		}
	}

	public void OnHammerStart()
	{
		RuntimeManager.PlayOneShotAttached(er_swing, gameObject);
	}

	private void HandleRepairingAndReinforcement()
	{
		if (PlayerItemManager.IsBusy || CanvasHelper.ActiveMenu != MenuType.None)
		{
			canvas.displayTextManager.HideDisplayTexts();
			return;
		}
		Block block = null;
		if (Helper.HitAllAtCursor(out var allHits, Player.UseDistance * 2f, LayerMasks.MASK_Block))
		{
			RaycastHit[] array = allHits;
			for (int i = 0; i < array.Length; i++)
			{
				RaycastHit hit = array[i];
				Block componentInParent = hit.transform.GetComponentInParent<Block>();
				if (componentInParent != null)
				{
					int uniqueIndex = componentInParent.buildableItem.UniqueIndex;
					bool flag = false;
					switch (uniqueIndex)
					{
					case 2:
					case 8:
					case 189:
					case 382:
					case 383:
					case 387:
					case 503:
						block = componentInParent;
						HandleRepairingOfBlock(componentInParent, hit);
						HandleReinforcementOfBlock(componentInParent, hit);
						flag = true;
						break;
					}
					if (flag)
					{
						break;
					}
				}
			}
		}
		if ((block == null || blockCreator.buildMenu.lastSelectedBuildabe == null || blockCreator.buildMenu.lastSelectedBuildabe != reinforceItem) && reinforceGhostObject != null)
		{
			reinforceGhostObject.Hide();
		}
		if (block == null)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	private void HandleRepairingOfBlock(Block block, RaycastHit hit)
	{
		if (block.CanBeRepaired())
		{
			canvas.displayTextManager.ShowText(block.NormalizedHealth * 100f + "% " + Helper.GetTerm("Game/Health"));
			Item_Base currentBlockType = blockCreator.GetCurrentBlockType();
			bool buttonDown = MyInput.GetButtonDown("MMB");
			if (((currentBlockType != null && currentBlockType.UniqueIndex == repairItem.UniqueIndex && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputUse, "LMB")) | buttonDown) && blockCreator.HasEnoughResourcesToBuild(repairItem.settings_recipe.NewCost))
			{
				woodParticles.SetPosition(hit.point);
				woodParticles.SetLookRotation(hit.normal);
				woodParticles.PlayParticles();
				playerNetwork.Animator.SetAnimation(PlayerAnimation.Trigger_ItemHitTrigger);
				playerNetwork.Inventory.RemoveCostMultiple(repairItem.settings_recipe.NewCost);
				block.Repair(blockRepairAmount);
				canvas.displayTextManager.HideDisplayTexts();
			}
		}
		else
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	private void HandleReinforcementOfBlock(Block block, RaycastHit hit)
	{
		if (block.CanBeReinforced())
		{
			Item_Base currentBlockType = blockCreator.GetCurrentBlockType();
			if (!(currentBlockType != null) || currentBlockType.UniqueIndex != reinforceItem.UniqueIndex)
			{
				return;
			}
			Block_Foundation block_Foundation = block as Block_Foundation;
			if (!(block_Foundation != null))
			{
				return;
			}
			canvas.displayTextManager.ShowText(Helper.GetTerm("Game/SetFortified"));
			if (reinforceGhostObject == null)
			{
				reinforceGhostObject = Object.Instantiate(reinForceGhostPrefab, block.transform.position, block.transform.rotation, block.transform);
			}
			reinforceGhostObject.meshFilter.sharedMesh = block_Foundation.armoredMesh;
			reinforceGhostObject.transform.SetParent(block_Foundation.meshRenderer.transform);
			reinforceGhostObject.transform.localPosition = new Vector3(0f, 0.02f, 0f);
			reinforceGhostObject.transform.localEulerAngles = Vector3.zero;
			reinforceGhostObject.Show();
			bool flag = blockCreator.HasEnoughResourcesToBuild(reinforceItem.settings_recipe.NewCost);
			if (flag && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputUse, "LMB"))
			{
				Message_BlockCreator_SetReinforced message = new Message_BlockCreator_SetReinforced(Messages.BlockCreator_SetReinforced, blockCreator, playerNetwork.steamID, block.ObjectIndex, fortifyValue: true);
				if (Raft_Network.IsHost)
				{
					ReinforceBlock(block_Foundation);
					playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
				else
				{
					playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
				canvas.displayTextManager.HideDisplayTexts();
			}
			if (flag)
			{
				reinforceGhostObject.meshRenderer.sharedMaterial = reinforceGhostObject.validMaterial;
			}
			else
			{
				reinforceGhostObject.meshRenderer.sharedMaterial = reinforceGhostObject.invalidMaterial;
			}
		}
		else if (reinforceGhostObject != null)
		{
			reinforceGhostObject.Hide();
		}
	}

	public bool ReinforceBlock(Block block)
	{
		if (Raft_Network.IsHost && (block == null || block.Reinforced))
		{
			return false;
		}
		block.Reinforced = true;
		block.SetHealth(block.MaxHealth);
		if (playerNetwork.IsLocalPlayer)
		{
			playerNetwork.Animator.SetAnimation(PlayerAnimation.Trigger_ItemHitTrigger);
			if (!Cheat.UseGodMode)
			{
				playerNetwork.Inventory.RemoveCostMultiple(reinforceItem.settings_recipe.NewCost);
			}
		}
		RuntimeManager.PlayOneShotAttached(er_reinforce, gameObject);
		return true;
	}
}
