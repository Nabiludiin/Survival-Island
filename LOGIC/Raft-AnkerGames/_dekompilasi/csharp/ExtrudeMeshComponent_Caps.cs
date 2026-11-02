using UnityEngine;

public class ExtrudeMeshComponent_Caps : ExtrudeMeshComponent
{
	[SerializeField]
	private float scale = 1f;

	[SerializeField]
	private bool inheritsShapeScale = true;

	[SerializeField]
	private GameObject startCapPrefab;

	[SerializeField]
	private bool flipStartCap;

	[SerializeField]
	private GameObject endCapPrefab;

	[SerializeField]
	private bool flipEndCap;

	private GameObject startCap;

	private GameObject endCap;

	private const string startCapPrefix = "StartCap: ";

	private const string endCapPrefix = "EndCap: ";

	protected override void OnExtrudeMeshGenerate()
	{
		base.OnExtrudeMeshGenerate();
		GenerateCaps();
	}

	protected override void OnExtrudeMeshClear()
	{
		base.OnExtrudeMeshClear();
		ClearCaps();
	}

	private void GenerateCaps()
	{
		if (startCapPrefab != null)
		{
			if (!HasCorrectStartCap())
			{
				ClearStartCap();
				startCap = InstantiatePrefab(startCapPrefab);
				startCap.transform.SetParent(transform);
				startCap.transform.name = "StartCap: " + startCapPrefab.name;
			}
			if (startCap != null)
			{
				ExtrudeMesh.OrientedPoint orientedPoint = extrudeMeshScript.GetOrientedPoint(0f);
				startCap.transform.position = orientedPoint.pos;
				startCap.transform.localScale = startCapPrefab.transform.localScale * scale * (inheritsShapeScale ? extrudeMeshScript.shapeScale : 1f);
				startCap.transform.rotation = orientedPoint.rot;
				if (flipStartCap)
				{
					startCap.transform.rotation = Quaternion.LookRotation(-startCap.transform.forward, startCap.transform.up);
				}
			}
		}
		if (!(endCapPrefab != null))
		{
			return;
		}
		if (!HasCorrectEndCap())
		{
			ClearEndCap();
			endCap = InstantiatePrefab(endCapPrefab);
			endCap.transform.SetParent(transform);
			endCap.transform.name = "EndCap: " + endCapPrefab.name;
		}
		if (endCap != null)
		{
			ExtrudeMesh.OrientedPoint orientedPoint2 = extrudeMeshScript.GetOrientedPoint(1f);
			endCap.transform.position = orientedPoint2.pos;
			endCap.transform.localScale = endCapPrefab.transform.localScale * scale * (inheritsShapeScale ? extrudeMeshScript.shapeScale : 1f);
			endCap.transform.rotation = orientedPoint2.rot;
			if (flipEndCap)
			{
				endCap.transform.rotation = Quaternion.LookRotation(-endCap.transform.forward, endCap.transform.up);
			}
		}
	}

	private void ClearCaps()
	{
		ClearStartCap();
		ClearEndCap();
	}

	private void ClearStartCap()
	{
		if (startCap != null)
		{
			Object.DestroyImmediate(startCap);
			startCap = null;
		}
	}

	private void ClearEndCap()
	{
		if (endCap != null)
		{
			Object.DestroyImmediate(endCap);
			endCap = null;
		}
	}

	private bool HasCorrectStartCap()
	{
		if (startCap == null)
		{
			startCap = FindCap("StartCap: ");
		}
		if (startCap == null || startCapPrefab == null)
		{
			return false;
		}
		if (startCap.name != "StartCap: " + startCapPrefab.name)
		{
			return false;
		}
		return true;
	}

	private bool HasCorrectEndCap()
	{
		if (endCap == null)
		{
			endCap = FindCap("EndCap: ");
		}
		if (endCap == null || endCapPrefab == null)
		{
			return false;
		}
		if (endCap.name != "EndCap: " + endCapPrefab.name)
		{
			return false;
		}
		return true;
	}

	private GameObject FindCap(string namePrefix)
	{
		if (transform.childCount > 0)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				if (!(child == null) && child.name.StartsWith(namePrefix))
				{
					return child.gameObject;
				}
			}
		}
		return null;
	}
}
