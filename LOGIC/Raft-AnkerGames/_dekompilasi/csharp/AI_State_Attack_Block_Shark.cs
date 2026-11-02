using System;
using System.Collections.Generic;
using UnityEngine;

public class AI_State_Attack_Block_Shark : AI_State_Attack_Block
{
	public Block currentTargetBlock;

	public bool foundBlockToAttack;

	public bool startedBitingBlock;

	public bool isBitingRaft;

	[SerializeField]
	private float dmgTreshhold = 30f;

	[SerializeField]
	private Vector3 offsetBlock;

	[SerializeField]
	private Vector3 offsetTriangular;

	[SerializeField]
	private Vector3 offsetBait;

	private AI_StateMachine_Shark stateMachineShark;

	private int initialTargetBlockHealth;

	private bool isTargetBlockSharkBait;

	private float biteBlockRange = 4f;

	private float currentDmgRecieved;

	private Vector3 biteVertexLocalPosition;

	private Vector3 targetBitePoint;

	protected override void Awake()
	{
		base.Awake();
		stateMachineShark = stateMachine as AI_StateMachine_Shark;
	}

	protected override void Start()
	{
		base.Start();
		if (Raft_Network.IsHost)
		{
			AI_StateMachine_Shark aI_StateMachine_Shark = stateMachineShark;
			aI_StateMachine_Shark.OnDamageTakenAction = (Action<float>)Delegate.Combine(aI_StateMachine_Shark.OnDamageTakenAction, new Action<float>(OnDamageTaken));
			BlockCreator.PlaceBlockCallStack = (Action<Block, Network_Player, bool, int>)Delegate.Combine(BlockCreator.PlaceBlockCallStack, new Action<Block, Network_Player, bool, int>(OnBlockIsPlaced));
		}
	}

	protected override void Destroy()
	{
		base.Destroy();
		if (Raft_Network.IsHost)
		{
			AI_StateMachine_Shark aI_StateMachine_Shark = stateMachineShark;
			aI_StateMachine_Shark.OnDamageTakenAction = (Action<float>)Delegate.Remove(aI_StateMachine_Shark.OnDamageTakenAction, new Action<float>(OnDamageTaken));
		}
	}

	private void OnDrawGizmos()
	{
		if (currentTargetBlock != null && isBitingRaft)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(currentTargetBlock.transform.position, 0.2f);
			Gizmos.color = Color.white;
			Gizmos.DrawSphere(stateMachineShark.lockedPivot.TransformPoint(targetBitePoint), 0.2f);
			Gizmos.color = Color.magenta;
			Gizmos.DrawSphere(stateMachineShark.lockedPivot.TransformPoint(biteVertexLocalPosition), 0.2f);
		}
	}

	public void StartAttackingBlock(Block p_targetBlock)
	{
		startedBitingBlock = true;
		AttackBlock(p_targetBlock);
	}

	public bool CheckBlockDiveAwayFromBlock()
	{
		if (foundBlockToAttack && (currentTargetBlock == null || currentTargetBlock.Health <= 0 || (currentTargetBlock is SharkBait && currentTargetBlock.Health <= 0) || currentTargetBlock.Reinforced))
		{
			bitingBlock = false;
			stateMachineShark.animController.SetBool("BitingRaft", value: false);
			stateMachineShark.StopParticleWaterSplash();
			stateMachineShark.ChangeState(stateMachineShark.diveState);
			return true;
		}
		return false;
	}

	public void InitiateAttackOnBlock(Block p_targetBlock)
	{
		stateMachineShark.rigid.isKinematic = true;
		stateMachineShark.rigid.velocity = Vector3.zero;
		isBitingRaft = true;
		initialTargetBlockHealth = p_targetBlock.Health;
		stateMachineShark.animController.SetBool("BitingRaft", value: true);
		bool flag = p_targetBlock is SharkBait;
		if (flag)
		{
			stateMachineShark.transform.SetParent(p_targetBlock.transform);
		}
		else
		{
			stateMachineShark.transform.SetParent(stateMachineShark.lockedPivot);
		}
		targetBitePoint = CalculateLocalBitePosition(p_targetBlock, flag);
		isBitingRaft = true;
		DoBiteSoundAndParticles();
	}

	public Vector3 CalculateLocalBitePosition(Block targetBlock, bool isTargetBlockSharkBait)
	{
		Vector3 position = targetBlock.transform.position;
		Vector3 position2 = transform.position;
		position2.y = position.y;
		Vector3 zero = Vector3.zero;
		if (isTargetBlockSharkBait)
		{
			Vector3 normalized = (position - position2).normalized;
			zero = targetBlock.transform.position + new Vector3(0f, offsetBait.y, 0f);
			zero -= normalized * offsetBait.z;
		}
		else if (Block.IsBlockIndexFoundationTriangular(targetBlock.buildableItem.UniqueIndex))
		{
			Collider componentInChildren = targetBlock.GetComponentInChildren<MeshCollider>();
			Vector3 vector = Physics.ClosestPoint(position2, componentInChildren, componentInChildren.bounds.center, componentInChildren.transform.rotation);
			vector.y = position.y;
			Vector3 normalized2 = (position2 - vector).normalized;
			Vector3 vector2 = vector;
			vector2 += normalized2 * offsetTriangular.z;
			vector2.y += offsetTriangular.y;
			zero = vector2;
			biteVertexLocalPosition = stateMachineShark.lockedPivot.transform.InverseTransformPoint(vector);
		}
		else
		{
			Vector3 normalized3 = (position - position2).normalized;
			zero = targetBlock.transform.position + new Vector3(0f, offsetBlock.y, 0f);
			zero -= normalized3 * offsetBlock.z;
		}
		if (!isTargetBlockSharkBait)
		{
			return stateMachineShark.lockedPivot.transform.InverseTransformPoint(zero);
		}
		return targetBlock.transform.InverseTransformPoint(zero);
	}

	public void LerpTowardsTargetBitePoint(Transform p_transform, Vector3 targetPoint, bool localPosition)
	{
		if (localPosition)
		{
			p_transform.localPosition = Vector3.Lerp(p_transform.localPosition, targetPoint, Time.deltaTime * stateMachineShark.movement.MovementSpeed);
		}
		else
		{
			p_transform.position = Vector3.Lerp(p_transform.position, targetPoint, Time.deltaTime * stateMachineShark.movement.MovementSpeed);
		}
	}

	public void DoBiteSoundAndParticles()
	{
		stateMachineShark.PlayParticleWaterSplash();
		stateMachineShark.newBiteSound.Play();
		stateMachineShark.sharkHapticEvent = SimpleMonoBehaviourSingleton<HapticManager>.Instance.SharkPool.FetchNextIndex();
		if (stateMachineShark.sharkHapticEvent != -1)
		{
			SimpleMonoBehaviourSingleton<HapticManager>.Instance.StartAttenuated((HapticEvent)stateMachineShark.sharkHapticEvent, transform.position);
		}
		if (stateMachineShark.lockedPivot != null)
		{
			_ = ((currentTargetBlock != null) ? currentTargetBlock.transform.position : transform.position) - stateMachineShark.lockedPivot.position;
		}
		stateMachineShark.soundemitter_idle.Stop();
	}

	private void OnDamageTaken(float damage)
	{
		if (stateMachineShark.currentState == this)
		{
			currentDmgRecieved += damage;
			if (currentDmgRecieved >= dmgTreshhold)
			{
				stateMachineShark.ChangeState(stateMachineShark.GetDiveOrRetreatState(5f));
			}
		}
	}

	public override void UpdateState()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		base.UpdateState();
		if (isBitingRaft)
		{
			CheckBlockDiveAwayFromBlock();
			if (currentTargetBlock != null)
			{
				LerpTowardsTargetBitePoint(stateMachineShark.transform, targetBitePoint, localPosition: true);
				Vector3 worldPosition = currentTargetBlock.transform.position;
				if (Block.IsBlockIndexFoundationTriangular(currentTargetBlock.buildableItem.UniqueIndex))
				{
					worldPosition = stateMachineShark.lockedPivot.TransformPoint(biteVertexLocalPosition);
				}
				stateMachine.transform.LookAt(worldPosition);
				stateMachine.transform.eulerAngles = new Vector3(0f, stateMachine.transform.eulerAngles.y, 0f);
				attackProgress += Time.deltaTime;
				if (attackProgress >= attackRate)
				{
					AttackBlock(currentTargetBlock);
					attackProgress = 0f;
				}
			}
			return;
		}
		if (stateMachineShark.targetToAttack != null && !GameModeValueManager.GetCurrentGameModeValue().sharkVariables.isTame && Helper.HasClearVisionAndWithinRange(stateMachineShark.transform.position, stateMachineShark.targetToAttack.transform.position, stateMachineShark.obstructionMask, stateMachineShark.playerVisionRange) && Helper.IsValidWaterTarget(stateMachineShark.targetToAttack))
		{
			stateMachineShark.ChangeState(stateMachineShark.wayPointChaseState);
			return;
		}
		if (currentTargetBlock == null)
		{
			currentTargetBlock = FindBlockToAttack();
		}
		if (currentTargetBlock == null || currentTargetBlock.Reinforced)
		{
			stateMachineShark.SetSearchBlockProgressCooldown(10f);
			stateMachineShark.ChangeState(stateMachineShark.circulationSurfaceState);
		}
		else if (currentTargetBlock != null)
		{
			MoveTowardsTargetBlock(currentTargetBlock);
			if (Vector3.Distance(stateMachineShark.transform.position, currentTargetBlock.transform.position) < biteBlockRange)
			{
				InitiateAttackOnBlock(currentTargetBlock);
			}
		}
	}

	public override void EnterState()
	{
		if (Raft_Network.IsHost)
		{
			base.EnterState();
			if (stateMachineShark == null)
			{
				stateMachineShark = stateMachine as AI_StateMachine_Shark;
			}
			if (stateMachineShark.targetToAttack == null)
			{
				stateMachineShark.FindAndSetTargetToAttack();
			}
		}
	}

	public override void ExitState()
	{
		stateMachineShark.newBiteSound.Stop();
		if (stateMachineShark.sharkHapticEvent != -1)
		{
			SimpleMonoBehaviourSingleton<HapticManager>.Instance.SharkPool.ReleaseIndex(stateMachineShark.sharkHapticEvent);
			SimpleMonoBehaviourSingleton<HapticManager>.Instance.StopAtennuated((HapticEvent)stateMachineShark.sharkHapticEvent);
			stateMachineShark.sharkHapticEvent = -1;
		}
		stateMachineShark.soundemitter_idle.Play();
		stateMachineShark.StopParticleWaterSplash();
		if (Raft_Network.IsHost)
		{
			base.ExitState();
			isBitingRaft = false;
			initialTargetBlockHealth = 0;
			foundBlockToAttack = false;
			currentTargetBlock = null;
			attackProgress = 0f;
			currentDmgRecieved = 0f;
			stateMachineShark.transform.SetParentSafe(null);
			stateMachineShark.animController.SetBool("BitingRaft", value: false);
			Invoke("DelayedKinematicRemoval", 1.5f);
		}
	}

	public override void OnStateChange<T>(params T[] parameters)
	{
		base.OnStateChange(parameters);
		currentTargetBlock = parameters[0] as Block;
		foundBlockToAttack = true;
	}

	protected override Block FindBlockToAttack()
	{
		Block block = base.FindBlockToAttack();
		if (block != null)
		{
			foundBlockToAttack = true;
		}
		return block;
	}

	private bool IsBlockIndexViableForBiting(Block block)
	{
		if (block.Reinforced)
		{
			return false;
		}
		int uniqueIndex = block.buildableItem.UniqueIndex;
		if (uniqueIndex == 2 || uniqueIndex == 8 || uniqueIndex == 189)
		{
			return true;
		}
		return false;
	}

	private void OnBlockIsPlaced(Block block, Network_Player player, bool temp, int temp2)
	{
		if (stateMachineShark.currentState == this && currentTargetBlock != null && !isBitingRaft && IsBlockIndexViableForBiting(block) && Vector3.Distance(block.transform.position, stateMachineShark.transform.position) < Vector3.Distance(currentTargetBlock.transform.position, stateMachineShark.transform.position))
		{
			currentTargetBlock = block;
		}
	}

	private void MoveTowardsTargetBlock(Block p_block)
	{
		stateMachineShark.movement.GoldenRuleMovement(p_block.transform.position, stateMachineShark.rigid, stateMachine.movement.MovementSpeed);
	}

	private void DelayedKinematicRemoval()
	{
		if (stateMachineShark != null && stateMachineShark.rigid != null)
		{
			stateMachineShark.rigid.isKinematic = false;
		}
	}

	private void AttackBlock(Block p_targetBlock)
	{
		if (foundBlockToAttack)
		{
			DealDamageToBlock(p_targetBlock);
			if (!(p_targetBlock is SharkBait))
			{
				stateMachineShark.PlayParticleWood();
			}
			CheckBlockDiveAwayFromBlock();
		}
	}

	private void DealDamageToBlock(Block p_targetBlock)
	{
		p_targetBlock.Damage(attackBlockDamage);
		int num = initialTargetBlockHealth - p_targetBlock.MaxHealth;
		if (p_targetBlock.Reinforced)
		{
			stateMachineShark.ChangeState(stateMachineShark.diveState);
		}
		if (p_targetBlock.Health <= num)
		{
			stateMachineShark.ChangeState(stateMachineShark.diveState);
			if (p_targetBlock != null && p_targetBlock.Health > 0 && !isTargetBlockSharkBait)
			{
				BlockCreator.UpdateHealthNetwork(p_targetBlock);
			}
		}
		else if (p_targetBlock != null && p_targetBlock.Health > 0 && !isTargetBlockSharkBait)
		{
			BlockCreator.UpdateHealthNetwork(p_targetBlock);
		}
	}

	protected override List<Block> GetAttackableBlocks()
	{
		if (stateMachineShark == null || stateMachineShark.raftBounds == null)
		{
			return base.GetAttackableBlocks();
		}
		return stateMachineShark.raftBounds.walkableBlocks;
	}
}
