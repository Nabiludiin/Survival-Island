using FMODUnity;
using UnityEngine;

public class AI_StateMachine_ButlerBot : AI_StateMachine_Animal
{
	[SerializeField]
	private GameObject screen_Help;

	[SerializeField]
	private GameObject screen_Damaged;

	[SerializeField]
	private GameObject screen_Normal;

	[SerializeField]
	private GameObject screen_Stunned;

	[SerializeField]
	private Renderer tracksRenderer;

	[SerializeField]
	private float trackSpeedModifier = 2f;

	[SerializeField]
	private StudioEventEmitter emitter_Tracks;

	private MaterialPropertyBlock materialPropBlock;

	private float trackOffset;

	public override void Awake()
	{
		base.Awake();
	}

	protected override void UpdateStateMachine()
	{
		base.UpdateStateMachine();
		if (materialPropBlock == null)
		{
			materialPropBlock = new MaterialPropertyBlock();
		}
		float num = animController.GetInt("Speed");
		trackOffset += num * trackSpeedModifier * Time.deltaTime;
		tracksRenderer.GetPropertyBlock(materialPropBlock, 1);
		materialPropBlock.SetFloat("_Offset", trackOffset);
		tracksRenderer.SetPropertyBlock(materialPropBlock);
		if (emitter_Tracks != null)
		{
			if (num != 0f)
			{
				emitter_Tracks.PlaySafe();
			}
			else
			{
				emitter_Tracks.StopSafe();
			}
		}
	}

	protected override void OnPickedUp()
	{
	}

	protected override void OnDamageTaken(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageType, bool isLocal)
	{
		if (currentState != deadState && currentState != decayState)
		{
			ShowFace("Screen_Damaged");
			base.OnDamageTaken(damage, hitPoint, hitNormal, damageType, isLocal);
		}
	}

	protected override void OnDeath(EntityType damageType)
	{
		base.OnDeath(damageType);
		ShowFace("Screen_Stunned");
	}

	public override void OnStateMachineMessage(AIStateEvent eventType, string message)
	{
		base.OnStateMachineMessage(eventType, message);
		ShowFace(message);
	}

	protected override void Animation_Damage(float damage)
	{
		if (networkBehaviour.networkEntity.stat_health.Value > 0f)
		{
			base.Animation_Damage(damage);
		}
	}

	private void ShowFace(string faceName)
	{
		screen_Normal.SetActiveSafe(value: false);
		screen_Stunned.SetActiveSafe(value: false);
		screen_Help.SetActiveSafe(value: false);
		screen_Damaged.SetActiveSafe(value: false);
		CancelInvoke("ResetDamagedFace");
		switch (faceName)
		{
		case "Screen_Normal":
			screen_Normal.SetActiveSafe(value: true);
			break;
		case "Screen_Stunned":
			screen_Stunned.SetActiveSafe(value: true);
			break;
		case "Screen_Help":
			screen_Help.SetActiveSafe(value: true);
			break;
		case "Screen_Damaged":
			if (!screen_Stunned.gameObject.activeSelf)
			{
				screen_Damaged.SetActiveSafe(value: true);
				Invoke("ResetDamagedFace", 0.5f);
			}
			break;
		}
	}

	private void ResetDamagedFace()
	{
		ShowFace("Screen_Normal");
	}
}
