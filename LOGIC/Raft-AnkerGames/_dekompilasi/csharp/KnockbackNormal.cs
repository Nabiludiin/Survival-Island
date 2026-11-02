using UnityEngine;

public class KnockbackNormal : MonoBehaviour
{
	public float knockForce = 15f;

	[Range(0f, 1f)]
	public float knockLift = 0.1f;

	private void OnCollisionEnter(Collision collision)
	{
		Network_Player component = collision.gameObject.GetComponent<Network_Player>();
		if (component != null)
		{
			Vector3 normal = collision.GetContact(0).normal;
			normal.Normalize();
			normal *= -1f;
			normal.y = knockLift;
			component.PersonController.AddExternalVelocity(normal * knockForce, 1f);
		}
	}
}
