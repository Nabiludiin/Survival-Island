using UnityEngine;

public class ElevatorFloor : MonoBehaviour
{
	public Transform teleportPoint;

	public GizmoBox gizmoBox;

	public int displayFloorNumber;

	[SerializeField]
	private Animator doorAnimator;

	public void SetDoorState(bool state)
	{
		if (doorAnimator != null)
		{
			doorAnimator.SetBool("Open", state);
		}
	}
}
