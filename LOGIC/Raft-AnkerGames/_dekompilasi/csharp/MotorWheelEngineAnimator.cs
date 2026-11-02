using UnityEngine;

public class MotorWheelEngineAnimator : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem[] smokeParticles;

	private void Start()
	{
	}

	public void Animation_SmokeParticle(int particleIndex)
	{
		smokeParticles[particleIndex].Play();
	}
}
