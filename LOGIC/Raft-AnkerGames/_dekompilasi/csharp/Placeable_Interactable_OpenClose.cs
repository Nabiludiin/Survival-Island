using FMODUnity;
using UnityEngine;

public class Placeable_Interactable_OpenClose : Placeable_Interactable
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private string boolParameter;

	public EventReference er_open;

	public EventReference er_close;

	protected override void OnInteract(bool restoring)
	{
		base.OnInteract(restoring);
		if (currentIndex == 0)
		{
			animator.SetBool(boolParameter, value: true);
			button.localizationTerm = "Game/Close";
			if (!restoring)
			{
				RuntimeManager.PlayOneShot(er_open, transform.position);
			}
		}
		else if (currentIndex == 1)
		{
			animator.SetBool(boolParameter, value: false);
			button.localizationTerm = "Game/Open";
			if (!restoring)
			{
				RuntimeManager.PlayOneShot(er_close, transform.position);
			}
		}
	}

	protected override void BlockPlaced()
	{
		base.BlockPlaced();
		animator.enabled = true;
	}
}
