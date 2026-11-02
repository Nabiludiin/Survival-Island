using System;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeshPathBase : MonoBehaviour_ID_Network, IRaycastable, SceneEventInterface
{
	public static Action OnFinishedCreatingMeshPath;

	public static bool RecentlyCreatedMeshPath = false;

	public static bool MeshPathCreationIsObstructed = false;

	public static MeshPathBase StartBase = null;

	public static MeshPathBase CurrentlyAimedAtBase = null;

	public static float CurrentSlackSetting = -0.5f;

	public static float TargetSlackSetting = CurrentSlackSetting;

	public const float DefaultSlackSetting = -0.5f;

	public Transform connectPoint;

	public MeshPath pathPrefab;

	public string termStartDraggingRope = "Game/Zipline/StartRope";

	public string termEndDraggingRope = "Game/Zipline/AbortRope";

	public bool isPathSlackModifyable = true;

	protected CanvasHelper canvas;

	private Network_Player localPlayer;

	public static bool IsCreatingMeshPath => StartBase != null;

	private void Start()
	{
		canvas = ComponentManager<CanvasHelper>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		MeshPath.RemovePathsFromBase(this);
		if (IsCreatingMeshPath && StartBase == this)
		{
			AbortCreatingMeshPath();
		}
	}

	void IRaycastable.OnIsRayed()
	{
		if (localPlayer == null || CanvasHelper.ActiveMenu != MenuType.None)
		{
			return;
		}
		CurrentlyAimedAtBase = this;
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		if (IsCreatingMeshPath && StartBase != this)
		{
			if (!MeshPathCreationIsObstructed && !MeshPath.DoesConnectionExist(StartBase, this))
			{
				canvas.displayTextManager.ShowText(Helper.GetTerm(termEndDraggingRope), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey, 0, 0, clearAllTexts: false);
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
				{
					FinishCreatingZipline();
					canvas.displayTextManager.HideDisplayTexts();
				}
			}
		}
		else if (CanStartCreatingPath())
		{
			canvas.displayTextManager.ShowText(Helper.GetTerm(termStartDraggingRope), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				StartCreatingZipline();
				canvas.displayTextManager.HideDisplayTexts();
			}
		}
	}

	void IRaycastable.OnRayEnter()
	{
	}

	void IRaycastable.OnRayExit()
	{
		if (canvas != null)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
		if (CurrentlyAimedAtBase == this)
		{
			CurrentlyAimedAtBase = null;
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			StartBase = null;
			CurrentlyAimedAtBase = null;
			RecentlyCreatedMeshPath = false;
			MeshPathCreationIsObstructed = false;
			OnFinishedCreatingMeshPath = null;
		}
		CurrentSlackSetting = -0.5f;
	}

	public static void AbortCreatingMeshPath()
	{
		PlayerItemManager.IsBusy = false;
		StartBase = null;
		MeshPathCreationIsObstructed = false;
		TargetSlackSetting = -0.5f;
		CurrentSlackSetting = -0.5f;
	}

	public virtual void OnPathSucessfullyCreated(MeshPathBase baseB, MeshPathConnection pathConnection)
	{
	}

	protected virtual bool CanStartCreatingPath()
	{
		return !PlayerItemManager.IsBusy;
	}

	protected virtual void FinishCreatingZipline()
	{
		if (IsCreatingMeshPath)
		{
			Message_MeshPath message = new Message_MeshPath(Messages.MeshPath_Create, localPlayer.Network.NetworkIDManager, StartBase, this, TargetSlackSetting);
			if (Raft_Network.IsHost)
			{
				if (MeshPath.CreateMeshPath(StartBase, this, TargetSlackSetting, replicating: false))
				{
					localPlayer.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
			else
			{
				localPlayer.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			RecentlyCreatedMeshPath = true;
			Invoke("ResetRecentlyCreatedMeshPath", 0.2f);
			OnFinishedCreatingMeshPath?.Invoke();
		}
		AbortCreatingMeshPath();
	}

	private void StartCreatingZipline()
	{
		if (!IsCreatingMeshPath)
		{
			PlayerItemManager.IsBusy = true;
			StartBase = this;
			RecentlyCreatedMeshPath = false;
		}
	}

	private void ResetRecentlyCreatedMeshPath()
	{
		RecentlyCreatedMeshPath = false;
	}
}
