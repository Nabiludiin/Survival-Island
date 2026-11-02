using UnityEngine;

public class ThrowableImpactCheck : MonoBehaviour
{
	[SerializeField]
	public Throwable throwable;

	private int lastInstanceIDImpact;

	private void OnCollisionEnter(Collision collision)
	{
		if (throwable.throwablePhysicsCollider.enabled)
		{
			throwable.OnThrowablePhysicsImpact(collision);
		}
	}
}
