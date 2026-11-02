using UnityEngine;

namespace UltimateWater;

public sealed class CenterOfMass : MonoBehaviour
{
	public void Apply()
	{
		Rigidbody componentInParent = GetComponentInParent<Rigidbody>();
		if (componentInParent != null)
		{
			componentInParent.centerOfMass = componentInParent.transform.worldToLocalMatrix.MultiplyPoint3x4(transform.position);
		}
	}

	private void OnEnable()
	{
		Apply();
	}
}
