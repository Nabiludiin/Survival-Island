using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeshPath_Igloo : MeshPath, IRaycastable
{
	public QuestItemType questItemReturnedOnRemoval = QuestItemType.Temperance_ElectricalCable;

	[HideInInspector]
	public Electricity_Challenge electricityChallenge;

	[SerializeField]
	private VFX_Electricity vfxPrefab;

	[HideInInspector]
	public VFX_Electricity vfxInstance;

	private CanvasHelper canvas;

	private Network_Player localPlayer;

	private void Awake()
	{
		if (electricityChallenge == null)
		{
			electricityChallenge = GetComponentInParent<Electricity_Challenge>();
		}
	}

	private void Start()
	{
		canvas = ComponentManager<CanvasHelper>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
	}

	void IRaycastable.OnIsRayed()
	{
		if (localPlayer == null || PlayerItemManager.IsBusy || CanvasHelper.ActiveMenu != MenuType.None)
		{
			return;
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Remove"];
		canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Remove"), action.GetBindingDisplayString(), MyInput.Keybinds["Remove"].MainKey);
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Remove"))
		{
			return;
		}
		canvas.displayTextManager.HideDisplayTexts();
		MeshPathConnection blockConnection = MeshPath.GetBlockConnection(this);
		if (blockConnection == null)
		{
			return;
		}
		Message_MeshPath message = new Message_MeshPath(Messages.MeshPath_Remove, localPlayer.Network.NetworkIDManager, blockConnection.baseA, blockConnection.baseB, 0f);
		if (Raft_Network.IsHost)
		{
			if (MeshPath.RemovePath(this))
			{
				localPlayer.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			localPlayer.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
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
	}

	public override void CreatePath(Vector3 worldPointA, Vector3 worldPointB, float slack, bool replicating, int minPoints = 8, int maxPoints = 20)
	{
		base.CreatePath(worldPointA, worldPointB, slack, replicating, minPoints, maxPoints);
		if (vfxPrefab == null)
		{
			return;
		}
		vfxInstance = Object.Instantiate(vfxPrefab, transform.position, Quaternion.identity, transform);
		if (vfxInstance != null)
		{
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < points.Count; i++)
			{
				Vector3 position = points[i];
				list.Add(transform.TransformPoint(position));
			}
			vfxInstance.Init(list);
		}
	}

	public override void OnPathRemoved()
	{
		base.OnPathRemoved();
		if (electricityChallenge != null)
		{
			electricityChallenge.RefreshElectricity();
		}
		else
		{
			Debug.LogError("No assigned electricityChallenge, cant refresh electricity", this);
		}
		if (Raft_Network.IsHost)
		{
			ComponentManager<QuestItemManager>.Value.AddQuestItemsNetworked(true, new QuestItem(QuestItemManager.GetSOQuestItemFromType(questItemReturnedOnRemoval), 1));
		}
	}
}
