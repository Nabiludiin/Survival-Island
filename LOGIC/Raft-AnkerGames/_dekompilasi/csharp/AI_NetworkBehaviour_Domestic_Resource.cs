using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class AI_NetworkBehaviour_Domestic_Resource : AI_NetworkBehaviour_Domestic
{
	public ResourceRegenerative Resource { get; set; }

	protected override void Awake()
	{
		base.Awake();
		InitializeResource();
	}

	protected override void Start()
	{
		base.Start();
		networkEntity.OnDamageTakenEvent += OnDamageTaken;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		networkEntity.OnDamageTakenEvent -= OnDamageTaken;
	}

	protected override void UpdateHost()
	{
		base.UpdateHost();
		if (IsDomestic && !IsStarving)
		{
			Resource.RegenResource(Time.deltaTime);
		}
	}

	private void OnDamageTaken(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageInflictorEntityType, bool isLocal)
	{
		if (networkEntity.IsDead)
		{
			Resource.gameObject.SetActiveSafe(value: false);
		}
	}

	protected virtual void OnResourceReady()
	{
		if (Raft_Network.IsHost)
		{
			ResourceReady();
			Message_OnResourceChange message = new Message_OnResourceChange(Messages.ResourceRegenerativeChange, Network.NetworkIDManager, ObjectIndex, p_resourceReady: true);
			if (Network != null)
			{
				Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	protected virtual void OnResourceNotReady()
	{
		Message_OnResourceChange message = new Message_OnResourceChange(Messages.ResourceRegenerativeChange, Network.NetworkIDManager, ObjectIndex, p_resourceReady: false);
		if (Raft_Network.IsHost)
		{
			ResourceNotReady();
			if (Network != null)
			{
				Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			Network.SendP2P(Network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	protected override void QuickTame()
	{
		base.QuickTame();
		OnResourceReady();
	}

	protected override void Captured()
	{
		if (!IsTamed)
		{
			OnResourceReady();
		}
		base.Captured();
	}

	protected virtual void ResourceReady()
	{
		if (!Resource.IsReady)
		{
			Resource.IsReady = true;
		}
		Resource.gameObject.SetActiveSafe(value: true);
	}

	protected virtual void ResourceNotReady()
	{
		if (Resource.IsReady)
		{
			Resource.DeplenishResource();
		}
		Resource.gameObject.SetActiveSafe(value: false);
	}

	protected virtual void InitializeResource()
	{
		Resource = GetComponentInChildren<ResourceRegenerative>();
		if (Resource != null)
		{
			if (!SaveAndLoad.IsGameLoading)
			{
				Resource.SetRegenCount(Resource.resource.originRegenTime, sendReadyEvents: false);
			}
			Resource.gameObject.SetActiveSafe(value: false);
			if (Raft_Network.IsHost)
			{
				ResourceRegenerative resource = Resource;
				resource.OnIsReady = (Action)Delegate.Combine(resource.OnIsReady, new Action(OnResourceReady));
			}
			ResourceRegenerative resource2 = Resource;
			resource2.OnIsNotReady = (Action)Delegate.Combine(resource2.OnIsNotReady, new Action(OnResourceNotReady));
		}
		else
		{
			Debug.LogError("Can't find any ResourceRegenerative.cs on children of domestic animal: " + stateMachine_Domestic.transform.name);
		}
	}

	public override RGD Serialize_Save()
	{
		if (!ConnectedToSpawner && !networkEntity.IsDead)
		{
			return new RGD_Domestic_Resource(this, Resource);
		}
		return base.Serialize_Save();
	}

	public override RGD_AI_NetworkBehaviour Create_SaveData()
	{
		if (ConnectedToSpawner && !networkEntity.IsDead)
		{
			return new RGD_Domestic_Resource(this, Resource);
		}
		return base.Create_SaveData();
	}

	public override void OnRestoreFinished(RGD_AI_NetworkBehaviour rgdNetworkBehaviour)
	{
		base.OnRestoreFinished(rgdNetworkBehaviour);
		RGD_Domestic_Resource rGD_Domestic_Resource = rgdNetworkBehaviour as RGD_Domestic_Resource;
		if (rGD_Domestic_Resource != null)
		{
			rGD_Domestic_Resource.RestoreDomesticResource(Resource);
			if (networkEntity.IsDead)
			{
				Resource.gameObject.SetActiveSafe(value: false);
			}
		}
	}

	public override void Serialize_CreateFromIDManager(List<Message_NetworkBehaviour> msgs)
	{
		base.Serialize_CreateFromIDManager(msgs);
		if (!ConnectedToSpawner)
		{
			msgs.Add(new Message_Domestic_Resource_Create(Messages.CreateDomesticResource, Network.NetworkIDManager, ObjectIndex, this));
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.CreateDomesticResource:
			if ((msg as Message_Domestic_Resource_Create).isResourceReady)
			{
				ResourceReady();
			}
			else
			{
				ResourceNotReady();
			}
			return false;
		case Messages.ResourceRegenerativeChange:
			if ((msg as Message_OnResourceChange).resourceReady)
			{
				ResourceReady();
			}
			else
			{
				ResourceNotReady();
			}
			return true;
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
