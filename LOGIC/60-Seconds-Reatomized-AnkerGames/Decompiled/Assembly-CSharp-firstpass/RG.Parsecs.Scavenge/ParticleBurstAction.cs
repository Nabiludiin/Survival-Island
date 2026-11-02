using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ParticleBurstAction : EventAction
{
	public ParticleSystem ParticleSystem;

	public ParticleSystem[] ParticleSystems;

	public bool changeToPlayerColor;

	protected override void Act(EventActionArgs args = null)
	{
		Color32? color = null;
		_ = changeToPlayerColor;
		if (ParticleSystem != null)
		{
			if (changeToPlayerColor)
			{
				TryChangeToPlayerColor(ParticleSystem, color);
			}
			ParticleSystem.gameObject.SetActive(value: true);
			ParticleSystem.Play(withChildren: true);
			StartCoroutine(WatchIsPlayingAndTurnOff(ParticleSystem));
		}
		if (ParticleSystems == null)
		{
			return;
		}
		for (int i = 0; i < ParticleSystems.Length; i++)
		{
			ParticleSystem particleSystem = ParticleSystems[i];
			if (changeToPlayerColor)
			{
				TryChangeToPlayerColor(particleSystem, color);
			}
			particleSystem.gameObject.SetActive(value: true);
			particleSystem.Play(withChildren: true);
			StartCoroutine(WatchIsPlayingAndTurnOff(particleSystem));
		}
	}

	private IEnumerator WatchIsPlayingAndTurnOff(ParticleSystem system)
	{
		yield return null;
		yield return null;
		while (system.IsAlive(withChildren: true))
		{
			yield return null;
		}
		system.gameObject.SetActive(value: false);
	}

	private void TryChangeToPlayerColor(ParticleSystem ps, Color32? color)
	{
		if (color.HasValue)
		{
			ParticleSystem.MainModule main = ps.main;
			ParticleSystem.MinMaxGradient startColor = main.startColor;
			startColor = new ParticleSystem.MinMaxGradient(color.Value);
			main.startColor = startColor;
		}
	}
}
