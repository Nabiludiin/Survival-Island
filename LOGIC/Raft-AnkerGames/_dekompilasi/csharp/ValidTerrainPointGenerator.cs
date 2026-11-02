using System.Collections.Generic;
using UnityEngine;

public class ValidTerrainPointGenerator : MonoBehaviour
{
	[SerializeField]
	private Vector2 generationBounds;

	public Vector2 generationBoundsOffset;

	public SO_ValidPointGeneratorSettings generatorSettings;

	public string requiredHitTag = string.Empty;

	[SerializeField]
	private List<Vector3> validPoints = new List<Vector3>();

	public void GeneratePoints()
	{
		Generate(generatorSettings);
	}

	public void ClearAllPoints()
	{
		validPoints.Clear();
	}

	public List<Vector3> GetValidPoints(int numberOfPoints)
	{
		if (validPoints.ContainsItems())
		{
			List<int> list = ArrayRandomizer.GenerateRandom(numberOfPoints, 0, validPoints.Count);
			List<Vector3> list2 = new List<Vector3>();
			for (int i = 0; i < list.Count; i++)
			{
				list2.Add(validPoints[list[i]]);
			}
			return list2;
		}
		return null;
	}

	private void Generate(SO_ValidPointGeneratorSettings settings)
	{
		validPoints.Clear();
		Vector3 vector = transform.position + new Vector3(generationBoundsOffset.x, 0f, generationBoundsOffset.y);
		_ = generationBounds;
		int num = (int)(vector.x - generationBounds.x / 2f);
		int num2 = (int)(vector.z - generationBounds.y / 2f);
		int num3 = (int)(vector.x + generationBounds.x / 2f);
		int num4 = (int)(vector.z + generationBounds.y / 2f);
		for (int i = num; i < num3; i += settings.generationDistanceBetweenPoints)
		{
			for (int j = num2; j < num4; j += settings.generationDistanceBetweenPoints)
			{
				Debug.DrawRay(new Vector3(i, 999f, j), Vector3.down * float.PositiveInfinity, Color.red, 4f);
				RaycastTowards(new Vector3(i, 999f, j), settings);
			}
		}
	}

	private void RaycastTowards(Vector3 from, SO_ValidPointGeneratorSettings settings)
	{
		if (Physics.Raycast(new Ray(from, Vector3.down), out var hitInfo, float.PositiveInfinity, settings.layerMask, QueryTriggerInteraction.Ignore))
		{
			bool flag = true;
			if (!string.IsNullOrEmpty(requiredHitTag) && !hitInfo.collider.CompareTag(requiredHitTag))
			{
				flag = false;
			}
			bool flag2 = CheckPointNormalViability(hitInfo, settings);
			bool flag3 = CheckPointHeightViability(hitInfo.point, settings);
			if (flag & flag2 & flag3)
			{
				validPoints.Add(hitInfo.point);
			}
		}
	}

	private bool CheckPointHeightViability(Vector3 point, SO_ValidPointGeneratorSettings parameters)
	{
		if (point.y > parameters.minPointY && point.y < parameters.maxPointY)
		{
			return true;
		}
		return false;
	}

	private bool CheckPointNormalViability(RaycastHit hit, SO_ValidPointGeneratorSettings settings)
	{
		float y = hit.normal.normalized.y;
		if (y > settings.minNormalAngle && y < settings.maxNormalAngle)
		{
			return true;
		}
		return false;
	}
}
