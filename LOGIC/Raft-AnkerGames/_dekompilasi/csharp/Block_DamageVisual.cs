using System;
using UnityEngine;

[RequireComponent(typeof(Block))]
public class Block_DamageVisual : MonoBehaviour
{
	[SerializeField]
	private DamageConnection[] damageConnections;

	[SerializeField]
	private MeshFilter meshFilter;

	private void Awake()
	{
		Block component = GetComponent<Block>();
		component.Action_OnHealthChange = (Action<Block>)Delegate.Combine(component.Action_OnHealthChange, new Action<Block>(OnHealthChange));
	}

	public void OnHealthChange(Block block)
	{
		if (block.Reinforced)
		{
			return;
		}
		for (int i = 0; i < damageConnections.Length; i++)
		{
			if (!(damageConnections[i].damagePercent > block.NormalizedHealth))
			{
				meshFilter.sharedMesh = damageConnections[i].mesh;
				break;
			}
		}
	}
}
