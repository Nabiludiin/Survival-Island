using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
	public bool startEnabled;

	public List<ParticleSystem> particleSystems = new List<ParticleSystem>();

	public Transform particleParent;

	private bool isPlaying;

	public bool IsPlaying
	{
		get
		{
			return isPlaying;
		}
		private set
		{
			isPlaying = value;
		}
	}

	private void Awake()
	{
		if (startEnabled)
		{
			PlayParticles();
		}
		else
		{
			StopParticles();
		}
	}

	public void PlayParticles()
	{
		IsPlaying = true;
		foreach (ParticleSystem particleSystem in particleSystems)
		{
			if (particleSystem != null)
			{
				particleSystem.PlaySafe();
			}
		}
	}

	public void StopParticles()
	{
		IsPlaying = false;
		foreach (ParticleSystem particleSystem in particleSystems)
		{
			if (particleSystem != null)
			{
				particleSystem.StopSafe();
			}
		}
	}

	public void SetPosition(Vector3 pos)
	{
		if (particleParent != null)
		{
			particleParent.position = pos;
		}
	}

	public void SetLookRotation(Vector3 dir)
	{
		if (particleParent != null)
		{
			particleParent.rotation = Quaternion.LookRotation(dir);
		}
	}

	public void SetPrewarmState(bool state)
	{
		for (int i = 0; i < particleSystems.Count; i++)
		{
			if (particleSystems[i] != null)
			{
				ParticleSystem.MainModule main = particleSystems[i].main;
				main.prewarm = state;
			}
		}
	}
}
