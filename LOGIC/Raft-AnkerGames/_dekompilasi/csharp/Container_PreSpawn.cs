using System.Collections.Generic;
using UnityEngine;

public class Container_PreSpawn : Container
{
	[SerializeField]
	private List<Transform> spawnPoints = new List<Transform>();

	protected override void Start()
	{
		base.Start();
		for (int i = 0; i < spawnPoints.Count; i++)
		{
		}
	}
}
