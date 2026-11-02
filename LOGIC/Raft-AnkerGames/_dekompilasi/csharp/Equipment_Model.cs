using System;
using UnityEngine;

public class Equipment_Model : Equipment
{
	[SerializeField]
	protected bool getModelByName;

	[SerializeField]
	protected Transform localModel;

	[SerializeField]
	protected Transform remoteModel;

	[SerializeField]
	protected string localModelName;

	[SerializeField]
	protected string remoteModelName;

	protected Network_Player playerNetwork;

	protected override void Awake()
	{
		base.Awake();
		playerNetwork = GetComponentInParent<Network_Player>();
	}

	protected override void Start()
	{
		base.Start();
		if (playerNetwork.IsLocalPlayer)
		{
			ThirdPerson thirdPersonSettings = playerNetwork.currentModel.thirdPersonSettings;
			thirdPersonSettings.OnThirdpersonModelChange = (Action<bool>)Delegate.Combine(thirdPersonSettings.OnThirdpersonModelChange, new Action<bool>(ThirdPersonStateChange));
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		playerNetwork = null;
	}

	private void ThirdPersonStateChange(bool newThirdPersonState)
	{
		if (Equipped)
		{
			SetModelState(state: true);
		}
	}

	public override void Initialize(Network_Player playerNetwork)
	{
		base.Initialize(playerNetwork);
		if (!getModelByName)
		{
			return;
		}
		if (localModelName != string.Empty)
		{
			localModel = playerNetwork.currentModel.transform.Find(localModelName);
			if (localModel == null)
			{
				localModel = playerNetwork.currentModel.hatParent.Find(localModelName);
			}
		}
		if (remoteModelName != string.Empty)
		{
			remoteModel = playerNetwork.currentModel.transform.Find(remoteModelName);
			if (remoteModel == null)
			{
				remoteModel = playerNetwork.currentModel.hatParent.Find(remoteModelName);
			}
		}
	}

	public virtual void SetModelState(bool state)
	{
		if (playerNetwork == null || (playerNetwork.HazmatSuit.Equipped & state))
		{
			return;
		}
		if (!playerNetwork.IsLocalPlayer)
		{
			if (remoteModel != null)
			{
				remoteModel.gameObject.SetActiveSafe(state);
			}
			return;
		}
		bool thirdPersonModel = playerNetwork.currentModel.thirdPersonSettings.ThirdPersonModel;
		if (localModel != null)
		{
			if (thirdPersonModel)
			{
				localModel.gameObject.SetActive(value: false);
			}
			else
			{
				localModel.gameObject.SetActive(state);
			}
		}
		if (remoteModel != null)
		{
			if (thirdPersonModel)
			{
				remoteModel.gameObject.SetActive(state);
			}
			else
			{
				remoteModel.gameObject.SetActive(value: false);
			}
		}
	}

	public override void Equip(Slot_Equip equippedSlot)
	{
		base.Equip(equippedSlot);
		SetModelState(state: true);
	}

	public override void UnEquip()
	{
		base.UnEquip();
		SetModelState(state: false);
	}
}
