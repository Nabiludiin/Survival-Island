using System.Collections;
using UnityEngine;

public class ParticleSystemHider : MonoBehaviour
{
	[Header("THIS NEEDS TO BE PLACED NEXT TO A PARTICLESYSTEM")]
	public bool hideAfterLifeTime;

	private ParticleSystem particles;

	private void Awake()
	{
		particles = GetComponent<ParticleSystem>();
	}

	private void OnEnable()
	{
		if (hideAfterLifeTime)
		{
			StartCoroutine(HideSystem(particles.main.startLifetime.constant * 1.1f));
		}
	}

	private IEnumerator HideSystem(float timeDelay)
	{
		yield return new WaitForSeconds(timeDelay);
		PoolManager.ReturnObject(gameObject);
	}
}
