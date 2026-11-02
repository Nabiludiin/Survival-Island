using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class PoolableParticleSystem : MonoBehaviour
{
	public string PoolName;

	public ParticleSystem ParticleSystem;

	public ParticleSystem[] ParticleSystems;

	private int _systemCounter;

	public void PlayAtPosition(Vector3 position)
	{
		transform.position = position;
		Play();
	}

	public void Play()
	{
		gameObject.SetActive(value: true);
		if (ParticleSystem != null)
		{
			ParticleSystem.Play(withChildren: true);
			StartCoroutine(WatchIsPlayingAndTurnOff(ParticleSystem));
		}
		if (ParticleSystems != null)
		{
			for (int i = 0; i < ParticleSystems.Length; i++)
			{
				ParticleSystem particleSystem = ParticleSystems[i];
				particleSystem.Play(withChildren: true);
				StartCoroutine(WatchIsPlayingAndTurnOff(particleSystem));
			}
		}
	}

	private IEnumerator WatchIsPlayingAndTurnOff(ParticleSystem system)
	{
		_systemCounter++;
		yield return null;
		yield return null;
		while (system.IsAlive(withChildren: true))
		{
			yield return null;
		}
		_systemCounter--;
		if (_systemCounter == 0)
		{
			gameObject.SetActive(value: false);
			ParticleSystemPool.I.PoolObject(PoolName, this);
		}
	}
}
