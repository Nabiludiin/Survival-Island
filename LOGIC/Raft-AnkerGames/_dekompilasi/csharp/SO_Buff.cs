using System;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffAsset", menuName = "Raft/Buff/Buff")]
public class SO_Buff : ScriptableObject
{
	[SerializeField]
	private uint uniqueBuffIndex;

	[SerializeField]
	private UnityEngine.Object enemyBuffScriptType;

	[SerializeField]
	private UnityEngine.Object playerBuffScriptType;

	[SerializeField]
	private BuffType buffType;

	[SerializeField]
	private BuffDisplayType buffDisplayType;

	[SerializeField]
	private float buffDuration;

	[SerializeField]
	private bool canStack;

	[SerializeField]
	private bool resetDurationOnSameBuffType;

	[SerializeField]
	private bool modifyMovementSpeed;

	[Range(-1f, 1f)]
	[SerializeField]
	private float groundMovementModifier = -0.5f;

	[Range(-1f, 1f)]
	[SerializeField]
	private float waterMovementModifier = -0.5f;

	[SerializeField]
	private bool applyInvurnerability;

	[SerializeField]
	private bool applyDeathPrevention;

	[SerializeField]
	private bool modifyOxygen;

	[Range(0f, 2f)]
	[SerializeField]
	private float oxygenModifier;

	public EventReference er_sound;

	public uint UniqueBuffIndex => uniqueBuffIndex;

	public BuffType BuffType => buffType;

	public BuffDisplayType BuffDisplayType => buffDisplayType;

	public float BuffDuration => buffDuration;

	public bool CanStack => canStack;

	public bool ResetDurationOnSameBuffType => resetDurationOnSameBuffType;

	public bool ModifyMovementSpeed => modifyMovementSpeed;

	public float GroundMovementModifier => groundMovementModifier;

	public float WaterMovementModifier => waterMovementModifier;

	public bool ApplyInvurnerability => applyInvurnerability;

	public bool ApplyDeathPrevention => applyDeathPrevention;

	public bool ModifyOxygen => modifyOxygen;

	public float OxygenModifier => oxygenModifier;

	public Buff GetEnemyBuff()
	{
		return Activator.CreateInstance(Type.GetType(enemyBuffScriptType.name)) as Buff;
	}

	public Buff GetPlayerBuff()
	{
		return Activator.CreateInstance(Type.GetType(playerBuffScriptType.name)) as Buff;
	}
}
