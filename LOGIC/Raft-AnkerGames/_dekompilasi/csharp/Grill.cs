using UnityEngine;

public class Grill : MonoBehaviour
{
	[SerializeField]
	private Animator anim;

	[SerializeField]
	private Block_CookingStand cookingStand;

	private void OnMouseEnter()
	{
		if (anim != null)
		{
			_ = cookingStand != null;
		}
	}

	private void OnMouseExit()
	{
		if (anim != null)
		{
			_ = cookingStand != null;
		}
	}
}
