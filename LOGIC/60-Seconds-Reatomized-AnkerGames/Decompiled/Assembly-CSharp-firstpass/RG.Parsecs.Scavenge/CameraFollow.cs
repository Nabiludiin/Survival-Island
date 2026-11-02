using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	private GameObject _followedObject;

	[SerializeField]
	public float Speed;

	[SerializeField]
	public Vector3 Offset;

	private void FixedUpdate()
	{
		transform.position = Vector3.Lerp(transform.position, _followedObject.transform.position + Offset, Speed * Time.deltaTime);
	}
}
