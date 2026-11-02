using System.Collections;
using UnityEngine;

public class QuestInteractable_Cutscene : QuestInteractable
{
	[SerializeField]
	private SO_Buff cutsceneInvurnerabilityBuff;

	[SerializeField]
	private Transform cutsceneTeleportationTransform;

	[SerializeField]
	private CutsceneType cutsceneType;

	[SerializeField]
	private float interactDelay = 5f;

	[SerializeField]
	private float maxDistanceFromAllPlayersToInteract = 5f;

	[SerializeField]
	private string localization_allPlayersNearbyRequired = "Game/PlayersNeedToBeClose";

	[SerializeField]
	private bool skipCutscene;

	private CutsceneManager cutsceneManager;

	protected override void Start()
	{
		base.Start();
		cutsceneManager = ComponentManager<CutsceneManager>.Value;
	}

	public override bool CanPlayerInteract(Network_Player player, bool overwriteInventoryValidation = false)
	{
		if (base.CanPlayerInteract(player, overwriteInventoryValidation))
		{
			return Helper.IsAllPlayersWithinDistance(transform.position, maxDistanceFromAllPlayersToInteract);
		}
		return false;
	}

	public override void Interact(Network_Player player, bool successFull)
	{
		if (Raft_Network.IsHost)
		{
			if (!skipCutscene)
			{
				if (!Helper.IsAllPlayersWithinDistance(transform.position, maxDistanceFromAllPlayersToInteract))
				{
					successFull = false;
				}
			}
			else
			{
				successFull = true;
			}
		}
		base.Interact(player, successFull);
		if (successFull)
		{
			Invoke("InteractLate", interactDelay);
		}
	}

	public override void DisplayInteractText(Network_Player player)
	{
		if (!Helper.IsAllPlayersWithinDistance(transform.position, maxDistanceFromAllPlayersToInteract))
		{
			displayText.ShowText(Helper.GetTerm(localization_allPlayersNearbyRequired));
		}
		else
		{
			base.DisplayInteractText(player);
		}
	}

	private void InteractLate()
	{
		SO_Cutscene cutsceneFromType = cutsceneManager.GetCutsceneFromType(cutsceneType);
		if (!(cutsceneFromType != null))
		{
			return;
		}
		if (!skipCutscene)
		{
			cutsceneManager.PlayCutscene(cutsceneFromType);
			CoroutineManager.Singleton.AddCoroutine(TeleportPlayer());
			return;
		}
		Network_Player localPlayer = Network.GetLocalPlayer();
		if (localPlayer != null)
		{
			if (localPlayer.PersonController.controllerType != ControllerType.Ground)
			{
				localPlayer.PersonController.SwitchControllerType(ControllerType.Ground);
			}
			localPlayer.transform.position = ((cutsceneTeleportationTransform == null) ? transform.position : cutsceneTeleportationTransform.position);
		}
	}

	private IEnumerator TeleportPlayer()
	{
		yield return new WaitForSeconds(2f);
		Network_Player localPlayer = Network.GetLocalPlayer();
		if (localPlayer == null)
		{
			Debug.LogError("Player reference was NULL, when trying to teleport player during cutscene.");
			yield break;
		}
		Player playerScript = localPlayer.PlayerScript;
		if (playerScript != null)
		{
			playerScript.ResetPlayerState();
			if (localPlayer.PersonController.controllerType != ControllerType.Ground)
			{
				localPlayer.PersonController.SwitchControllerType(ControllerType.Ground);
			}
			if (playerScript.IsDead)
			{
				playerScript.RespawnWithoutBed(clearInventory: false);
			}
			playerScript.transform.position = ((cutsceneTeleportationTransform == null) ? transform.position : cutsceneTeleportationTransform.position);
			localPlayer.Stats.buffManager.AddBuff(cutsceneInvurnerabilityBuff);
		}
	}
}
