using UnityEngine;

public class Stat_Health : Stat_Target
{
	[SerializeField]
	private float regenPerSecond;

	public void Heal()
	{
		Value = Max;
	}

	public void Regenerate()
	{
		int num = (MeetsWellBeing ? 1 : 3);
		Value += regenPerSecond * (float)num * Time.deltaTime;
	}

	public bool IsDead()
	{
		return Value <= 0f;
	}

	protected override void ChangeValue(float diff, float newValue)
	{
		base.ChangeValue(diff, newValue);
	}
}
