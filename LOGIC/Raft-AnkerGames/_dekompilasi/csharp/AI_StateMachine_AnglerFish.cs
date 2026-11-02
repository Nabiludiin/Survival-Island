using System.Collections;
using UnityEngine;

public class AI_StateMachine_AnglerFish : AI_StateMachine_Animal
{
	public EnvironmentLightAffector lightAffector;

	public float timeResetLightAfterDamageTaken = 1.5f;

	public Renderer rend;

	private MaterialPropertyBlock emissionBlock;

	private Coroutine routine_emission;

	protected override void OnDamageTaken(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageType, bool isLocal)
	{
		base.OnDamageTaken(damage, hitPoint, hitNormal, damageType, isLocal);
		if (!networkBehaviour.networkEntity.IsDead)
		{
			lightAffector.PlayAffector(EnvironmentLightAffector.AffectorID.ElectricityWater);
			Invoke("ResetLight", timeResetLightAfterDamageTaken);
		}
		if (routine_emission != null)
		{
			StopCoroutine(routine_emission);
		}
		routine_emission = StartCoroutine(MatchMaterialIntensityToLightRange(timeResetLightAfterDamageTaken));
	}

	private IEnumerator MatchMaterialIntensityToLightRange(float time)
	{
		if (emissionBlock == null)
		{
			emissionBlock = new MaterialPropertyBlock();
		}
		rend.GetPropertyBlock(emissionBlock);
		float passedTime = 0f;
		while (passedTime < time)
		{
			passedTime += Time.deltaTime;
			float value = Helper.MapValue(0f, 8f, lightAffector.targetLight.range / 10f, 0f, 2.7f);
			emissionBlock.SetFloat("_EmissionIntensity", value);
			rend.SetPropertyBlock(emissionBlock);
			yield return null;
		}
		emissionBlock.SetFloat("_EmissionIntensity", 2.7f);
		rend.SetPropertyBlock(emissionBlock);
	}

	private void ResetLight()
	{
		lightAffector.StopAffectorsSoft(3f);
	}
}
