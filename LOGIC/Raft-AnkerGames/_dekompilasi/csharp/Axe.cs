using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Axe : MonoBehaviour
{
	public AxeMode mode;

	public LayerMask hitmask;

	[SerializeField]
	private ParticleController woodParticles;

	[SerializeField]
	private StudioEventEmitter axeSoundEmitter;

	public EventReference er_swing;

	private PlayerAnimator playerAnimator;

	private Network_Player playerNetwork;

	private PlayerInventory playerInventory;

	private CanvasHelper canvas;

	private float chopTimer;

	public float chopBlockTime = 2f;

	private Block aimedAtBlock;

	private Block currentBlockToRemove;

	private HarvestableTree currentTreeToChop;

	private RaycastHit rayHit;

	[SerializeField]
	private InputActionReference inputUse;

	private void Start()
	{
		canvas = ComponentManager<CanvasHelper>.Value;
		playerNetwork = GetComponentInParent<Network_Player>();
		playerAnimator = playerNetwork.Animator;
		playerInventory = playerNetwork.Inventory;
		woodParticles.particleParent.parent = null;
	}

	private void Update()
	{
		if (CanvasHelper.ActiveMenu != MenuType.None || !playerNetwork.IsLocalPlayer)
		{
			return;
		}
		Block block = null;
		bool flag = Helper.HitAtCursor(out rayHit, 5f, hitmask);
		if (flag && rayHit.transform.tag != "Tree")
		{
			block = rayHit.transform.GetComponentInParent<Block>();
			if (block != null)
			{
				if (block.buildableItem.settings_buildable.Placeable)
				{
					block = null;
				}
				else if (aimedAtBlock == null)
				{
					aimedAtBlock = block;
					aimedAtBlock.SetInstanceOutline(outlineState: true);
				}
				else if (aimedAtBlock != block)
				{
					aimedAtBlock.SetInstanceOutline(outlineState: false);
					aimedAtBlock = block;
					aimedAtBlock.SetInstanceOutline(outlineState: true);
				}
			}
		}
		if (block == null && aimedAtBlock != null)
		{
			aimedAtBlock.SetInstanceOutline(outlineState: false);
			aimedAtBlock = null;
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputUse.action, "LMB"))
		{
			if (flag)
			{
				if (!(rayHit.transform.tag != "Tree"))
				{
					return;
				}
				if (aimedAtBlock != null && !aimedAtBlock.buildableItem.settings_buildable.Placeable)
				{
					if (Application.isEditor)
					{
						chopTimer += Time.deltaTime * 4f;
					}
					else
					{
						chopTimer += Time.deltaTime;
					}
					if (aimedAtBlock != currentBlockToRemove || currentBlockToRemove == null)
					{
						chopTimer = 0f;
						currentBlockToRemove = aimedAtBlock;
					}
					float num = chopBlockTime / GameModeValueManager.GetCurrentGameModeValue().toolVariables.removeSpeedMultiplier;
					if (chopTimer >= num)
					{
						chopTimer = 0f;
						Message_BlockCreator_RemoveBlock message = new Message_BlockCreator_RemoveBlock(Messages.Axe_RemoveBlock, playerNetwork, 0uL, currentBlockToRemove, updateBounds: true);
						if (Raft_Network.IsHost)
						{
							playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
							DestroyBlock(currentBlockToRemove, updateRaftBounds: true);
						}
						else
						{
							playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
						}
					}
					canvas.SetLoadCircle(chopTimer > 0f);
					canvas.SetLoadCircle(chopTimer / num);
				}
				else
				{
					chopTimer = 0f;
					currentBlockToRemove = null;
				}
			}
			else
			{
				chopTimer = 0f;
				canvas.SetLoadCircle(state: false);
			}
		}
		else if (currentBlockToRemove != null)
		{
			currentBlockToRemove = null;
			canvas.SetLoadCircle(state: false);
		}
	}

	public void OnAxeStart()
	{
		if (gameObject.activeInHierarchy)
		{
			RuntimeManager.PlayOneShotAttached(er_swing, gameObject);
		}
	}

	public void OnAxeHit()
	{
		if (!playerNetwork)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (!playerNetwork.IsLocalPlayer || !gameObject.activeInHierarchy)
		{
			return;
		}
		currentTreeToChop = null;
		if (!Helper.HitAtCursor(out rayHit, 5f, hitmask))
		{
			return;
		}
		if (rayHit.transform.tag == "Tree")
		{
			HarvestableTree componentInParent = rayHit.transform.GetComponentInParent<HarvestableTree>();
			if (componentInParent != null && !componentInParent.Depleted)
			{
				currentTreeToChop = componentInParent;
			}
		}
		Message_AxeHit message_AxeHit = new Message_AxeHit(Messages.AxeHit, playerNetwork, playerNetwork.steamID);
		if (currentTreeToChop != null)
		{
			SimpleMonoBehaviourSingleton<HapticManager>.Instance.PlayOneShot(HapticEvent.Hit);
			message_AxeHit.treeObjectIndex = (int)currentTreeToChop.PickupNetwork.ObjectIndex;
			if (Raft_Network.IsHost)
			{
				currentTreeToChop.Harvest(playerInventory);
			}
		}
		message_AxeHit.HitPoint = rayHit.point;
		message_AxeHit.HitNormal = rayHit.normal;
		playerNetwork.Inventory.RemoveDurabillityFromHotSlot();
		if (Raft_Network.IsHost)
		{
			playerNetwork.Network.RPC(message_AxeHit, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			PlayEffect(playerNetwork.transform.parent, message_AxeHit.HitPoint, message_AxeHit.HitNormal);
		}
		else
		{
			playerNetwork.SendP2P(message_AxeHit, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		currentTreeToChop = null;
		if (mode == AxeMode.Chopping)
		{
			mode = AxeMode.None;
		}
	}

	public void OnDeSelect()
	{
		if (canvas != null)
		{
			canvas.SetLoadCircle(state: false);
		}
		if (aimedAtBlock != null)
		{
			aimedAtBlock.SetInstanceOutline(outlineState: false);
			aimedAtBlock = null;
		}
	}

	public bool DestroyBlock(Block block, bool updateRaftBounds)
	{
		if (block == null)
		{
			return false;
		}
		if (playerInventory == null)
		{
			playerInventory = ComponentManager<Network_Player>.Value.Inventory;
		}
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (playerNetwork.IsLocalPlayer)
		{
			RemovePlaceables.ReturnItemsFromBlock(block, playerNetwork, giveItems: true);
			AchievementHandler.AddBuildRemoveCount();
		}
		BlockCreator.RemoveBlock(block, playerNetwork, updateRaftBounds);
		currentBlockToRemove = null;
		return true;
	}

	public void PlayEffect(Transform effectParent, Vector3 position, Vector3 normalRotation)
	{
		if (woodParticles != null)
		{
			woodParticles.particleParent.SetParent(effectParent);
			woodParticles.SetPosition(position);
			woodParticles.SetLookRotation(normalRotation);
			woodParticles.PlayParticles();
		}
		if (axeSoundEmitter != null)
		{
			axeSoundEmitter.transform.SetParent(effectParent);
			axeSoundEmitter.transform.position = position;
			axeSoundEmitter.Play();
		}
	}
}
