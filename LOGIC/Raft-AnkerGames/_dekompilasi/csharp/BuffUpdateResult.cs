using System;

[Serializable]
public class BuffUpdateResult
{
	public int damageTaken;

	public float estimatedDamage;

	public void Clear()
	{
		damageTaken = 0;
		estimatedDamage = 0f;
	}
}
