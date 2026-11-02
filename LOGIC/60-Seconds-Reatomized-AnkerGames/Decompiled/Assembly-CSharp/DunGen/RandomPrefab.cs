using System;
using UnityEngine;

namespace DunGen;

[AddComponentMenu("DunGen/Random Props/Random Prefab")]
public class RandomPrefab : RandomProp
{
	public GameObjectChanceTable Props = new GameObjectChanceTable();

	public override void Process(System.Random randomStream, Tile tile)
	{
		if (Props.Weights.Count > 0)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Props.GetRandom(randomStream, tile.Placement.IsOnMainPath, tile.Placement.NormalizedDepth, removeFromTable: true));
			gameObject.transform.parent = transform;
			gameObject.transform.localPosition = Vector3.zero;
		}
	}
}
