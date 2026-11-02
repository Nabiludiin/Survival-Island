using UnityEngine;

public class JumpOutOfWaterChecker : MonoBehaviour
{
	public bool isCloseToEdge;

	public LayerMask edgeMask;

	public GameObject edge;

	private void OnTriggerStay(Collider other)
	{
		if (((1 << other.transform.gameObject.layer) & (int)edgeMask) != 0)
		{
			isCloseToEdge = true;
			edge = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (((1 << other.transform.gameObject.layer) & (int)edgeMask) != 0)
		{
			isCloseToEdge = false;
			edge = null;
		}
	}
}
