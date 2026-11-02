using UnityEngine;

public class Coconut : MonoBehaviour
{
	private Rigidbody body;

	private PickupItem pickup;

	private void Start()
	{
		body = GetComponent<Rigidbody>();
		pickup = GetComponent<PickupItem>();
		body.isKinematic = true;
		transform.localPosition = new Vector3(transform.localPosition.x, 2.7f, transform.localPosition.z);
	}

	public void DropFromTree()
	{
		body.isKinematic = false;
		pickup.canBePickedUp = true;
	}
}
