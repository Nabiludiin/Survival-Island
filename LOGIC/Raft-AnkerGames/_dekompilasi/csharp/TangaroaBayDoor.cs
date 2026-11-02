using UnityEngine;

public class TangaroaBayDoor : MonoBehaviour
{
	public ParticleSystem particleSystem1;

	public ParticleSystem particleSystem2;

	public ParticleSystem particleSystem3;

	public void AnimatorMessage(string message)
	{
		switch (message)
		{
		case "Particle1":
			particleSystem1.Play();
			break;
		case "Particle2":
			particleSystem2.Play();
			break;
		case "Particle3":
			particleSystem3.Play();
			break;
		}
	}
}
