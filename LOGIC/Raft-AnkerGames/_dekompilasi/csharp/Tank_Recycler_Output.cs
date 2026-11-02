using UnityEngine;

public class Tank_Recycler_Output : Tank
{
	[SerializeField]
	private Renderer[] trashCubeRenderers;

	public Animator recyclerAnimator;

	public int CubesPerTankAmount => Mathf.FloorToInt((float)trashCubeRenderers.Length / maxCapacity);

	public void SetCubeRenderersActiveComparedToTankFill()
	{
		int num = 1;
		Renderer[] array = trashCubeRenderers;
		foreach (Renderer renderer in array)
		{
			if (Mathf.FloorToInt(CurrentTankAmount / (float)num) >= CubesPerTankAmount)
			{
				renderer.enabled = true;
				if (recyclerAnimator != null)
				{
					recyclerAnimator.SetTrigger("FlapOpen");
				}
			}
			else
			{
				renderer.enabled = false;
			}
			num++;
		}
	}

	protected override void LocalTankAmountChange()
	{
		base.LocalTankAmountChange();
		SetCubeRenderersActiveComparedToTankFill();
	}
}
