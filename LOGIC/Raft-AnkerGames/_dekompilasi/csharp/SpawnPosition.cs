using System;
using UnityEngine;

[Serializable]
public class SpawnPosition
{
	public float horizontalOffset;

	public float horizontalSize;

	public float verticalSize;

	public float RandomHorizontalSize => UnityEngine.Random.Range(0f - horizontalSize, horizontalSize);

	public float RandomVerticalSize => UnityEngine.Random.Range(0f - verticalSize, verticalSize);
}
