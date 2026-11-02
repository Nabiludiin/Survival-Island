using UnityEngine;

public class ScaleWeight : MonoBehaviour
{
	[SerializeField]
	private RaycastInteractable_Redirect redirect;

	private int weight;

	public int Weight => weight;

	public void Initialize(RaycastInteractable target, int weight)
	{
		this.weight = weight;
		redirect.SetRaycastInteractableTarget(target);
	}
}
