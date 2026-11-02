using UnityEngine;

[CreateAssetMenu(fileName = "BuffAsset", menuName = "Raft/Buff/Buff_DOT")]
public class SO_Buff_DOT : SO_Buff
{
	[SerializeField]
	private int tickDamage = 1;

	[SerializeField]
	private float tickRate = 1f;

	public int TickDamage => tickDamage;

	public float TickRate => tickRate;

	public int TotalDotDamage => Mathf.FloorToInt(BuffDuration / TickRate * (float)TickDamage);
}
