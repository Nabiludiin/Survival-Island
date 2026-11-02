using FMODUnity;
using UnityEngine;

public class PickupChanneling_TelephoneAntenna : PickupChanneling
{
	[SerializeField]
	private LineRenderer line;

	[SerializeField]
	private Transform startPoint;

	[SerializeField]
	private MeshRenderer antenna;

	[SerializeField]
	private StudioEventEmitter emitter_antenna;

	[SerializeField]
	private StudioEventEmitter emitter_pull;

	public EventReference er_success;

	private MaterialPropertyBlock matPropBlock;

	private Transform leftHandRopeTransform;

	private bool isQuitting;

	private void OnEnable()
	{
		if (matPropBlock == null)
		{
			matPropBlock = new MaterialPropertyBlock();
		}
		antenna.GetPropertyBlock(matPropBlock);
		matPropBlock.SetFloat("_EmissionSwitch", 1f);
		antenna.SetPropertyBlock(matPropBlock);
		if (emitter_antenna != null)
		{
			emitter_antenna.Play();
		}
	}

	private void OnDisable()
	{
		if (matPropBlock == null)
		{
			matPropBlock = new MaterialPropertyBlock();
		}
		if (!isQuitting)
		{
			antenna.GetPropertyBlock(matPropBlock);
			matPropBlock.SetFloat("_EmissionSwitch", 0f);
			antenna.SetPropertyBlock(matPropBlock);
			RuntimeManager.PlayOneShot(er_success, startPoint.position);
			if (emitter_antenna != null)
			{
				emitter_antenna.Stop();
			}
		}
	}

	private void OnApplicationQuit()
	{
		isQuitting = true;
	}

	protected override void InitiateChannel()
	{
		base.InitiateChannel();
		line.enabled = true;
		if (emitter_pull != null)
		{
			emitter_pull.Play();
		}
	}

	protected override void ProgressChannel(float time)
	{
		base.ProgressChannel(time);
		line.SetPosition(0, startPoint.position);
		if (player.currentModel.thirdPersonSettings.ThirdPersonModel)
		{
			line.SetPosition(1, player.currentModel.ropeConnectTransform.position);
		}
		else
		{
			line.SetPosition(1, Helper.WorldPointToFOVPoint(player.currentModel.ropeConnectTransform.position));
		}
	}

	protected override void CancelChannel()
	{
		base.CancelChannel();
		line.enabled = false;
		if (emitter_pull != null)
		{
			emitter_pull.Stop();
		}
	}
}
