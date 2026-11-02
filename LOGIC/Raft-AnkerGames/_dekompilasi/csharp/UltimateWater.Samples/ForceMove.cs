using UnityEngine;

namespace UltimateWater.Samples;

public class ForceMove : MonoBehaviour
{
	[SerializeField]
	private Rigidbody _Rigidbody;

	[SerializeField]
	private float _Force;

	private void FixedUpdate()
	{
		_Rigidbody.AddForce(transform.forward * _Force);
	}
}
