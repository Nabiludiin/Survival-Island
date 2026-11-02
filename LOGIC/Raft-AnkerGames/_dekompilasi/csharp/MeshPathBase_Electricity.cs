using FMODUnity;
using UnityEngine;

public class MeshPathBase_Electricity : MeshPathBase
{
	[SerializeField]
	protected bool hasElectricity;

	[SerializeField]
	protected Electricity_Challenge electricityChallenge;

	[SerializeField]
	protected ParticleSystem sparkParticles;

	[SerializeField]
	protected Renderer emissionRenderer;

	[SerializeField]
	protected QuestItemType requiredQuestItem = QuestItemType.Temperance_ElectricalCable;

	public EventReference er_connect;

	public EventReference er_disconnect;

	private bool previousHasElectricity;

	private MaterialPropertyBlock mpb;

	private MeshPath_Igloo pathIgloo;

	public bool HasElectricity
	{
		get
		{
			return hasElectricity;
		}
		set
		{
			if (hasElectricity != value)
			{
				previousHasElectricity = hasElectricity;
				hasElectricity = value;
				RefreshElectricityVisuals();
			}
		}
	}

	private void OnEnable()
	{
		RefreshElectricityVisuals();
	}

	public override void OnPathSucessfullyCreated(MeshPathBase baseB, MeshPathConnection pathConnection)
	{
		base.OnPathSucessfullyCreated(baseB, pathConnection);
		if (electricityChallenge != null && pathConnection.path != null && pathConnection.path is MeshPath_Igloo)
		{
			pathIgloo = pathConnection.path as MeshPath_Igloo;
			pathIgloo.electricityChallenge = electricityChallenge;
		}
		if (electricityChallenge != null)
		{
			electricityChallenge.RefreshElectricity();
		}
		else
		{
			Debug.Log("ElectictityChallenge reference is null", this);
		}
		if (Raft_Network.IsHost)
		{
			ComponentManager<QuestItemManager>.Value.AddQuestItemsNetworked(false, new QuestItem(QuestItemManager.GetSOQuestItemFromType(requiredQuestItem), -1));
		}
	}

	protected virtual void RefreshElectricityVisuals()
	{
		if (sparkParticles != null && HasElectricity)
		{
			sparkParticles.Play();
		}
		if (!previousHasElectricity && HasElectricity && !er_connect.IsNull)
		{
			RuntimeManager.PlayOneShot(er_connect, connectPoint.position);
		}
		else if (previousHasElectricity && !HasElectricity && !er_disconnect.IsNull)
		{
			RuntimeManager.PlayOneShot(er_disconnect, connectPoint.position);
		}
		if (emissionRenderer != null)
		{
			if (mpb == null)
			{
				mpb = new MaterialPropertyBlock();
			}
			emissionRenderer.GetPropertyBlock(mpb);
			mpb.SetFloat("_EmissionSwitch", HasElectricity ? 1f : 0f);
			emissionRenderer.SetPropertyBlock(mpb);
		}
	}

	protected override bool CanStartCreatingPath()
	{
		QuestItem questItemFromType = QuestItemManager.GetQuestItemFromType(requiredQuestItem);
		if (questItemFromType == null)
		{
			return false;
		}
		bool flag = questItemFromType.itemCount > 0;
		if (!flag)
		{
			canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Requires") + " " + questItemFromType.SOQuestItem.DisplayName);
		}
		return base.CanStartCreatingPath() & flag;
	}
}
