using System;
using UnityEngine;

[Serializable]
public class BaitConnection
{
	public Item_Base bait;

	public RandomDropper dropper;

	public Mesh bobberMesh;

	public Material bobberMaterial;

	public BaitConnection(Item_Base bait, RandomDropper dropper)
	{
		this.bait = bait;
		this.dropper = dropper;
	}
}
