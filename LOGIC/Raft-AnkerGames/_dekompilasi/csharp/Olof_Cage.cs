using UnityEngine;

public class Olof_Cage : MonoBehaviour
{
	[SerializeField]
	private AnimatorController animatorController;

	public void OnEnable()
	{
		if (animatorController != null)
		{
			animatorController.SetBool("Caged", value: true);
		}
	}
}
