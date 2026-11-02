using UnityEngine;

public class AI_NetworkBehaviour_Llama : AI_NetworkBehaviour_Domestic_Resource
{
	[SerializeField]
	private GameObject woolModel;

	[SerializeField]
	private ObjectOffsetter nametagOffsetter;

	[SerializeField]
	private ItemObjectEnabler objectEnabler;

	[SerializeField]
	private ParticleSystem woolParticleEffect;

	protected override void InitializeResource()
	{
		base.InitializeResource();
		SetWoolState(state: true);
	}

	protected override void ResourceReady()
	{
		base.ResourceReady();
		SetWoolState(state: true);
		nametagOffsetter.SetOffset("UnShaved");
		objectEnabler.ActivateModel(0);
	}

	protected override void ResourceNotReady()
	{
		base.ResourceNotReady();
		SetWoolState(state: false);
		nametagOffsetter.SetOffset("Shaved");
		objectEnabler.ActivateModel(1);
	}

	private void SetWoolState(bool state)
	{
		if (woolModel != null)
		{
			woolModel.gameObject.SetActive(state);
		}
		if (woolParticleEffect != null && !state)
		{
			woolParticleEffect.PlaySafe();
		}
	}

	public override void SetNameTagEnabled(bool value)
	{
		base.SetNameTagEnabled(value);
		if (Resource.IsReady)
		{
			nametagOffsetter.SetOffset("UnShaved");
			objectEnabler.ActivateModel(0);
		}
		else
		{
			nametagOffsetter.SetOffset("Shaved");
			objectEnabler.ActivateModel(1);
		}
	}
}
