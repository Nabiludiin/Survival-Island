using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class BillboardRotateSetup : MonoBehaviour
{
	private Camera _Camera;

	private void Awake()
	{
		_Camera = Camera.main;
	}

	private void Start()
	{
		transform.LookAt(transform.position + _Camera.transform.rotation * Vector3.forward, _Camera.transform.rotation * Vector3.up);
	}
}
