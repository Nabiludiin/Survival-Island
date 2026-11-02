using UnityEngine;

public class NetCanisterPart : MonoBehaviour
{
	[SerializeField]
	private Transform netParent;

	public void OnCollisionEnter(Collision collision)
	{
		netParent.SendMessage("OnCollisionEnter", collision);
	}
}
