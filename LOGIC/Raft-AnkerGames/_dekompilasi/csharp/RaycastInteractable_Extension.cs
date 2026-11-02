using UnityEngine;

public class RaycastInteractable_Extension : MonoBehaviour
{
	[SerializeField]
	private RaycastInteractable raycastInteractable;

	private void Awake()
	{
		IRaycastable[] components = GetComponents<IRaycastable>();
		raycastInteractable.AddRaycastables(components);
	}
}
