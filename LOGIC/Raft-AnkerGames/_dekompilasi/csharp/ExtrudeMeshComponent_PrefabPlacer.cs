using System;
using UnityEngine;

public class ExtrudeMeshComponent_PrefabPlacer : ExtrudeMeshComponent
{
	public enum SelectPrefabType
	{
		Randomize,
		InPrefabListOrder
	}

	public enum GenerationType
	{
		AmountOfPrefabs,
		AmountPerMeter
	}

	[SerializeField]
	private GenerationType generationMode = GenerationType.AmountPerMeter;

	[SerializeField]
	private int amountOfPrefabs = 1;

	[SerializeField]
	private float amountPerMeter = 0.5f;

	[SerializeField]
	private float scale = 1f;

	[SerializeField]
	private bool inheritsShapeScale = true;

	[SerializeField]
	private SelectPrefabType prefabMode;

	[SerializeField]
	private int seed;

	[SerializeField]
	private Randomizer prefabs = new Randomizer();

	private System.Random random;

	private uint lastUsedPrefabIndex;

	private void RandomizeSeed()
	{
		seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
	}

	protected override void OnExtrudeMeshGenerate()
	{
		GeneratePrefabs();
	}

	protected override void OnExtrudeMeshClear()
	{
		base.OnExtrudeMeshClear();
		ClearPrefabs();
	}

	private void GeneratePrefabs()
	{
		random = new System.Random(seed);
		ClearPrefabs();
		switch (generationMode)
		{
		case GenerationType.AmountOfPrefabs:
			GenerateSetAmount();
			break;
		case GenerationType.AmountPerMeter:
			GenerateSetAmountMeter();
			break;
		}
	}

	private void ClearPrefabs()
	{
		Transform decorationParent = GetDecorationParent();
		if (decorationParent != null)
		{
			UnityEngine.Object.DestroyImmediate(decorationParent.gameObject);
		}
	}

	private void GenerateSetAmount()
	{
		int num = amountOfPrefabs + 2;
		float num2 = 1f / (float)(num - 1);
		for (int i = 1; i < num - 1; i++)
		{
			float t = (float)i * num2;
			ExtrudeMesh.OrientedPoint orientedPoint = extrudeMeshScript.GetOrientedPoint(t);
			GeneratePrefab(orientedPoint);
		}
	}

	private void GenerateSetAmountMeter()
	{
		int num = Mathf.FloorToInt(extrudeMeshScript.PathLength * amountPerMeter) + 2;
		float num2 = 1f / (float)(num - 1);
		for (int i = 1; i < num - 1; i++)
		{
			float t = (float)i * num2;
			ExtrudeMesh.OrientedPoint orientedPoint = extrudeMeshScript.GetOrientedPoint(t);
			GeneratePrefab(orientedPoint);
		}
	}

	private void GeneratePrefab(ExtrudeMesh.OrientedPoint op)
	{
		GameObject prefabToSpawn = GetPrefabToSpawn();
		if (!(prefabToSpawn == null))
		{
			Transform transform = InstantiatePrefab(prefabToSpawn).transform;
			transform.SetParent(GetDecorationParent());
			transform.position = op.pos;
			transform.rotation = op.rot;
			transform.localScale = prefabToSpawn.transform.localScale * scale * (inheritsShapeScale ? extrudeMeshScript.shapeScale : 1f);
		}
	}

	private Transform GetDecorationParent()
	{
		Transform transform = base.transform.Find("PrefabPlacerDecorations");
		if (transform == null)
		{
			transform = new GameObject("PrefabPlacerDecorations").transform;
			transform.SetParent(base.transform);
			transform.SetSiblingIndex(0);
		}
		return transform;
	}

	private GameObject GetPrefabToSpawn()
	{
		switch (prefabMode)
		{
		case SelectPrefabType.Randomize:
			return prefabs.GetRandomItem<GameObject>(random);
		case SelectPrefabType.InPrefabListOrder:
		{
			GameObject itemByIndex = prefabs.GetItemByIndex<GameObject>(lastUsedPrefabIndex);
			lastUsedPrefabIndex = (uint)Mathf.Repeat(lastUsedPrefabIndex + 1, prefabs.GetItemCount());
			return itemByIndex;
		}
		default:
			return null;
		}
	}

	private bool IsGenerationModeSetAmount()
	{
		if (generationMode == GenerationType.AmountOfPrefabs)
		{
			return extrudeMeshScript != null;
		}
		return false;
	}

	private bool IsGenerationModeSetAmountMeter()
	{
		if (generationMode == GenerationType.AmountPerMeter)
		{
			return extrudeMeshScript != null;
		}
		return false;
	}

	private bool IsRandomPrefabMode()
	{
		if (prefabMode == SelectPrefabType.Randomize)
		{
			return extrudeMeshScript != null;
		}
		return false;
	}
}
