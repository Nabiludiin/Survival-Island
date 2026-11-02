using System;
using UnityEngine;

public class Equipment_HeadLight : Equipment_Hat
{
	[SerializeField]
	private uint durabilityLostPerSecond = 1u;

	private Transform lightParentRemote;

	private Light lightSourceLight;

	private float timer;

	private bool initialized;

	protected override void Start()
	{
		base.Start();
		if (playerNetwork.NoteBookUI != null)
		{
			NoteBookUI noteBookUI = playerNetwork.NoteBookUI;
			noteBookUI.OnBookActiveChanged = (Action<bool>)Delegate.Combine(noteBookUI.OnBookActiveChanged, new Action<bool>(OnNoteBookToggle));
		}
	}

	private void OnNoteBookToggle(bool active)
	{
		if (lightSourceLight != null)
		{
			lightSourceLight.gameObject.SetActiveSafe(!active);
		}
	}

	public override void Initialize(Network_Player playerNetwork)
	{
		initialized = true;
		base.Initialize(playerNetwork);
		if (localModel != null)
		{
			lightSourceLight = localModel.GetComponentInChildren<Light>(includeInactive: true);
		}
		if (remoteModel != null)
		{
			lightParentRemote = remoteModel.GetChild(0);
			if (lightSourceLight == null)
			{
				lightSourceLight = remoteModel.GetComponentInChildren<Light>(includeInactive: true);
			}
		}
		if (playerNetwork.IsLocalPlayer && lightSourceLight != null)
		{
			lightSourceLight.shadows = LightShadows.None;
		}
	}

	public override void Equip(Slot_Equip equippedSlot)
	{
		if (!initialized && playerNetwork != null)
		{
			Initialize(playerNetwork);
		}
		base.Equip(equippedSlot);
		if (lightSourceLight != null)
		{
			lightSourceLight.gameObject.SetActiveSafe(value: true);
		}
	}

	public override void SetModelState(bool state)
	{
		base.SetModelState(state);
		Transform newParent = localModel;
		if (!playerNetwork.IsLocalPlayer || playerNetwork.currentModel.thirdPersonSettings.ThirdPersonModel)
		{
			newParent = lightParentRemote;
		}
		if (lightSourceLight != null && lightSourceLight.transform.SetParentSafe(newParent))
		{
			Transform transform = lightSourceLight.transform;
			Vector3 localPosition = (lightSourceLight.transform.localEulerAngles = Vector3.zero);
			transform.localPosition = localPosition;
		}
	}

	public override void UpdateEquipment()
	{
		base.UpdateEquipment();
		if (!(lightSourceLight != null) || !(lightSourceLight.gameObject != null) || !lightSourceLight.gameObject.activeInHierarchy)
		{
			return;
		}
		timer += Time.deltaTime;
		if (timer >= 1f)
		{
			timer = 0f;
			if (equippedSlot != null)
			{
				equippedSlot.IncrementUses((int)(0 - durabilityLostPerSecond));
			}
		}
	}
}
