using UnityEngine;

public class ColorMachine : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private Interval_Float waterSpeedInterval;

	[SerializeField]
	private Interval_Float barrelSpeedInterval;

	[SerializeField]
	private float lerpSpeed = 1f;

	[SerializeField]
	private Transform barrelTransform;

	private Raft raft;

	private Block_CookingStand stand;

	private float waterSpeed;

	private float barrelSpeed;

	private void OnBlockPlaced()
	{
		raft = ComponentManager<Raft>.Value;
		stand = GetComponent<Block_CookingStand>();
	}

	private void Update()
	{
		if (stand != null)
		{
			stand.fuel.SetFuelCount(raft.Moving ? int.MaxValue : 0);
			HandleAnimation();
		}
	}

	private void HandleAnimation()
	{
		if (animator == null)
		{
			return;
		}
		float b = ((!raft.Moving) ? waterSpeedInterval.minValue : waterSpeedInterval.maxValue);
		waterSpeed = Mathf.Lerp(waterSpeed, b, Time.deltaTime * lerpSpeed);
		animator.SetFloat("WheelSpeed", waterSpeed);
		b = ((!stand.fuel.IsBurning()) ? barrelSpeedInterval.minValue : barrelSpeedInterval.maxValue);
		barrelSpeed = Mathf.Lerp(barrelSpeed, b, Time.deltaTime * lerpSpeed);
		if (b == barrelSpeedInterval.minValue)
		{
			if (barrelTransform.localEulerAngles.x > 5f)
			{
				if (barrelSpeed <= 0.05f)
				{
					barrelSpeed = 0.05f;
				}
			}
			else
			{
				barrelSpeed = 0f;
			}
		}
		animator.SetFloat("BarrelSpeed", barrelSpeed);
		animator.SetBool("Open", barrelSpeed <= 0.05f);
	}
}
