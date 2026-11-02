using System.Collections.Generic;
using UnityEngine;

public class AI_State_Attack_Block : AI_State
{
	protected bool bitingBlock;

	protected Animator animator;

	[SerializeField]
	protected int attackBlockDamage = 5;

	protected float attackRate = 0.4f;

	protected float attackProgress;

	protected virtual Block FindBlockToAttack()
	{
		float num = float.MaxValue;
		Block block = null;
		List<Block> attackableBlocks = GetAttackableBlocks();
		for (int i = 0; i < attackableBlocks.Count; i++)
		{
			Block block2 = attackableBlocks[i];
			if (block2 == null)
			{
				continue;
			}
			int uniqueIndex = block2.buildableItem.UniqueIndex;
			if (uniqueIndex == 2 || uniqueIndex == 8 || uniqueIndex == 189 || uniqueIndex == 382 || uniqueIndex == 383 || uniqueIndex == 387)
			{
				Vector3 position = block2.transform.position;
				position.y = transform.position.y;
				float num2 = Vector3.Distance(position, transform.position);
				if (num2 < 5f)
				{
					return null;
				}
				if (num2 < num || block == null)
				{
					num = num2;
					block = block2;
				}
			}
		}
		if (block != null && block.Reinforced)
		{
			return null;
		}
		return block;
	}

	protected virtual List<Block> GetAttackableBlocks()
	{
		return BlockCreator.GetPlacedBlocks();
	}
}
