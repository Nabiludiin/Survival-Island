using System.Collections;
using FMODUnity;
using UnityEngine;

public class QuestInteractable_Caravan_WaterPump : QuestInteractable
{
	[SerializeField]
	private int amountOfParticles = 30;

	[SerializeField]
	private float particleDelay = 0.1f;

	[SerializeField]
	private ParticleSystem pumpParticles;

	public EventReference er_emit;

	private Coroutine pumpRoutine;

	private void OnDisable()
	{
		StopPumpRoutine();
	}

	public override void Interact(Network_Player player, bool successFull)
	{
		base.Interact(player, successFull);
		if (successFull && CurrentObjectStateIndex == 1)
		{
			StopPumpRoutine();
			pumpRoutine = StartCoroutine(Pump());
		}
	}

	private void StopPumpRoutine()
	{
		if (pumpRoutine != null)
		{
			StopCoroutine(pumpRoutine);
			pumpRoutine = null;
		}
	}

	private IEnumerator Pump()
	{
		for (int i = 0; i < amountOfParticles; i++)
		{
			pumpParticles.Play(withChildren: true);
			RuntimeManager.PlayOneShot(er_emit, pumpParticles.transform.position);
			yield return new WaitForSeconds(particleDelay);
		}
	}
}
