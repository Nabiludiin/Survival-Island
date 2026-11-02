using System;
using System.Collections.Generic;
using UnityEngine;

public class Electricity_Challenge : MonoBehaviour
{
	public bool drawGizmos = true;

	public bool drawOrder = true;

	public MeshPathBase_Electricity[] electricityConnectorsInOrder;

	private void Awake()
	{
		ComponentManager<Electricity_Challenge>.Value = this;
		if (Raft_Network.IsHost)
		{
			SaveAndLoad.LoadComplete = (Action)Delegate.Combine(SaveAndLoad.LoadComplete, new Action(RefreshElectricity));
		}
		else
		{
			Raft_Network.OnWorldReceivedLate = (Action)Delegate.Combine(Raft_Network.OnWorldReceivedLate, new Action(RefreshElectricity));
		}
	}

	private void OnDestroy()
	{
		SaveAndLoad.LoadComplete = (Action)Delegate.Remove(SaveAndLoad.LoadComplete, new Action(RefreshElectricity));
		Raft_Network.OnWorldReceivedLate = (Action)Delegate.Remove(Raft_Network.OnWorldReceivedLate, new Action(RefreshElectricity));
	}

	private void OnDrawGizmos()
	{
		if (!drawGizmos)
		{
			return;
		}
		List<MeshPathBase> list = new List<MeshPathBase>();
		for (int i = 0; i < electricityConnectorsInOrder.Length; i++)
		{
			MeshPathBase_Electricity meshPathBase_Electricity = electricityConnectorsInOrder[i];
			if (meshPathBase_Electricity == null)
			{
				continue;
			}
			if (drawOrder && i < electricityConnectorsInOrder.Length - 1)
			{
				_ = electricityConnectorsInOrder[i + 1] != null;
			}
			for (int j = 0; j < electricityConnectorsInOrder.Length; j++)
			{
				MeshPathBase_Electricity meshPathBase_Electricity2 = electricityConnectorsInOrder[j];
				if (!(meshPathBase_Electricity2 == null) && !(meshPathBase_Electricity2 == meshPathBase_Electricity) && !list.Contains(meshPathBase_Electricity2))
				{
					Vector3 position = meshPathBase_Electricity.connectPoint.position;
					Vector3 position2 = meshPathBase_Electricity2.connectPoint.position;
					float num = Vector3.Distance(position, position2);
					_ = position + (position2 - position).normalized * (num / 2f);
					_ = Color.red;
					if (num < meshPathBase_Electricity.pathPrefab.maxLength)
					{
						_ = Color.green;
					}
					_ = meshPathBase_Electricity.pathPrefab.maxLength * 1.5f;
				}
			}
			list.Add(meshPathBase_Electricity);
		}
	}

	public void RefreshElectricity()
	{
		for (int i = 0; i < electricityConnectorsInOrder.Length - 1; i++)
		{
			MeshPathBase_Electricity meshPathBase_Electricity = electricityConnectorsInOrder[i];
			MeshPathBase_Electricity meshPathBase_Electricity2 = electricityConnectorsInOrder[i + 1];
			MeshPathConnection connection = MeshPath.GetConnection(meshPathBase_Electricity, meshPathBase_Electricity2);
			if (connection != null && connection.path != null && connection.path is MeshPath_Igloo)
			{
				MeshPath_Igloo meshPath_Igloo = connection.path as MeshPath_Igloo;
				if (meshPath_Igloo.vfxInstance != null)
				{
					if (connection.baseA == meshPathBase_Electricity2 && connection.baseB == meshPathBase_Electricity)
					{
						meshPath_Igloo.vfxInstance.reverse = true;
					}
					meshPath_Igloo.vfxInstance.SetActive(meshPathBase_Electricity.HasElectricity);
				}
			}
			meshPathBase_Electricity2.HasElectricity = connection != null && meshPathBase_Electricity.HasElectricity;
		}
	}
}
