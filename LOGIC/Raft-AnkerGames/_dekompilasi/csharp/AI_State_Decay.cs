using System.Collections;
using UnityEngine;

public class AI_State_Decay : AI_State
{
	public Renderer[] dissolvedRenderers;

	public float decayFadeAwayDuration = 2f;

	private float removeBodyTimeProgress;

	private AI_StateMachine_Animal statemachine_animal;

	private MaterialPropertyBlock dissolveProperyBlock;

	[SerializeField]
	protected bool dissolveBodyDuringDecay = true;

	public float maxTimeInDecayState = 80f;

	public virtual void OnValidate()
	{
		if (dissolvedRenderers != null && dissolvedRenderers.Length == 0)
		{
			dissolvedRenderers = transform.root.GetComponentsInChildren<Renderer>(includeInactive: true);
		}
	}

	protected override void Start()
	{
		base.Start();
		statemachine_animal = stateMachine as AI_StateMachine_Animal;
	}

	public override void EnterState()
	{
		base.EnterState();
		removeBodyTimeProgress = 0f;
		if (dissolveBodyDuringDecay)
		{
			StartCoroutine(DissolveBody());
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		if (dissolveBodyDuringDecay)
		{
			StopCoroutine(DissolveBody());
		}
	}

	public override void UpdateState()
	{
		base.UpdateState();
		removeBodyTimeProgress += Time.deltaTime;
		if (removeBodyTimeProgress >= maxTimeInDecayState)
		{
			FinalDecay();
		}
	}

	protected virtual bool CheckToSpawnOnDecay()
	{
		return true;
	}

	protected virtual void FinalDecay()
	{
		RemoveEntity();
	}

	protected void RemoveEntity()
	{
		if (stateMachine != null)
		{
			Object.Destroy(stateMachine.gameObject);
		}
	}

	protected IEnumerator DissolveBody()
	{
		while (removeBodyTimeProgress < decayFadeAwayDuration)
		{
			DissolveRendererBody(removeBodyTimeProgress, decayFadeAwayDuration);
			yield return new WaitForSeconds(0.1f);
		}
		FinalDecay();
	}

	protected void DissolveRendererBody(float currentProgress, float totallDissolveTime)
	{
		if (dissolveProperyBlock == null)
		{
			dissolveProperyBlock = new MaterialPropertyBlock();
		}
		if (dissolvedRenderers != null && dissolvedRenderers.Length != 0)
		{
			Renderer[] array = dissolvedRenderers;
			foreach (Renderer obj in array)
			{
				obj.GetPropertyBlock(dissolveProperyBlock);
				dissolveProperyBlock.SetFloat("_Dissolve", 1f - Helper.NormalizeValue(currentProgress, 0f, totallDissolveTime));
				obj.SetPropertyBlock(dissolveProperyBlock);
			}
		}
		else
		{
			Debug.Log("No renderers to be dissolved are assigned", gameObject);
		}
	}
}
