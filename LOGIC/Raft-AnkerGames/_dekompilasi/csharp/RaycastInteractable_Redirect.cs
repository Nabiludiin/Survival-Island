using UnityEngine;

public class RaycastInteractable_Redirect : MonoBehaviour
{
	[SerializeField]
	protected RaycastInteractable raycastInteractable;

	public virtual RaycastInteractable RaycastInteractable => raycastInteractable;

	public void SetRaycastInteractableTarget(RaycastInteractable target)
	{
		raycastInteractable = target;
	}
}
