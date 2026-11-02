using System.Collections.Generic;
using UnityEngine;

public class LightSingularity : MonoBehaviour
{
	private List<LightSingularityExternal> externals = new List<LightSingularityExternal>();

	private LightSingularityManager lightManager;

	private LightSingularityExternal currentFurthestExternal;

	public float maxIntensity = 3.5f;

	public float maxRange = 120f;

	public float currentIntensity;

	[SerializeField]
	private Light lightComponent;

	public Light LightComponent
	{
		get
		{
			return lightComponent;
		}
		set
		{
			lightComponent = value;
			SetLightActive(LightComponent, value);
		}
	}

	private void Awake()
	{
		lightComponent = GetComponent<Light>();
		if (lightComponent == null)
		{
			Debug.LogError("Something is le wrong");
		}
	}

	private void OnEnable()
	{
		lightManager = ComponentManager<LightSingularityManager>.Value;
	}

	private void Start()
	{
		lightManager = ComponentManager<LightSingularityManager>.Value;
		currentIntensity = LightComponent.intensity;
	}

	public List<LightSingularityExternal> GetExternals()
	{
		return externals;
	}

	public void AddExternalToExternalList(LightSingularityExternal p_external)
	{
		if (!externals.Contains(p_external))
		{
			externals.Add(p_external);
		}
	}

	public void RemoveExternalFromList(LightSingularityExternal p_external)
	{
		externals.Remove(p_external);
	}

	private void SetLightActive(Light p_light, bool p_value)
	{
		p_light.enabled = p_value;
	}

	public void ModifyLightRange(float range)
	{
		LightComponent.range += range;
		if (LightComponent.range > maxRange)
		{
			LightComponent.range = maxRange;
		}
	}

	public void SetLightRange(float p_range)
	{
		if (p_range > maxRange)
		{
			p_range = maxRange;
		}
		LightComponent.range = p_range;
	}

	public float GetDistanceToFurthestAwayExternal()
	{
		if (currentFurthestExternal != null)
		{
			return Vector3.Distance(currentFurthestExternal.transform.position, transform.position);
		}
		return 0f;
	}

	public void UpdateFurthestExternalInfo()
	{
		float num = 0f;
		for (int i = 0; i < externals.Count; i++)
		{
			if (externals[i] != null)
			{
				float num2 = Vector3.Distance(externals[i].transform.position, transform.position);
				if (num2 > num)
				{
					num = num2;
					currentFurthestExternal = externals[i];
				}
			}
		}
	}

	public void MergeWithOtherSingularity(LightSingularity p_other)
	{
		Vector3 position = (p_other.transform.position + transform.position) / 2f;
		transform.position = position;
		for (int i = 0; i < p_other.externals.Count; i++)
		{
			if (!externals.Contains(p_other.externals[i]))
			{
				externals.Add(p_other.externals[i]);
			}
		}
		p_other.ChangeExternalsSingularity(this);
		p_other.externals.Clear();
		lightManager.RemoveLightSingularity(p_other);
		UpdateLightRangeToProper(6f);
		UpdateLightIntensity(1.5f);
	}

	public void RemoveNullExternalsCheck()
	{
		for (int num = externals.Count - 1; num >= 0; num--)
		{
			if (externals[num] == null)
			{
				externals.RemoveAt(num);
			}
		}
		if (externals.Count <= 0)
		{
			lightManager.RemoveLightSingularity(this);
		}
	}

	public void UpdateLightIntensity(float p_baseIntensity)
	{
		float num = p_baseIntensity + (float)GetExternals().Count / lightManager.lightIntensityDividedByAmountMultiplier;
		if (num > maxIntensity)
		{
			num = maxIntensity;
		}
		currentIntensity = num;
		LightComponent.intensity = num;
	}

	public void UpdateLightRangeToProper(float p_baseLightRange)
	{
		SetLightRange(p_baseLightRange + GetDistanceToFurthestAwayExternal() + (float)GetExternals().Count);
	}

	public void ChangeExternalsSingularity(LightSingularity p_new)
	{
		for (int i = 0; i < externals.Count; i++)
		{
			externals[i].SetAsPartOfSingularity(p_value: true, p_new);
		}
	}

	public void AverageOutSingularityPosition()
	{
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < externals.Count; i++)
		{
			if (!(externals[i] == null))
			{
				LightSingularityExternal component = externals[i].GetComponent<LightSingularityExternal>();
				if (component.lightOffsetPosition != null)
				{
					zero += component.lightOffsetPosition.position;
				}
			}
		}
		transform.position = zero / externals.Count;
	}

	private void OnDrawGizmos()
	{
		if (LightComponent.enabled)
		{
			Gizmos.color = new Color(255f, 255f, 255f, 0.015f);
			Gizmos.DrawSphere(transform.position, LightComponent.range / 2.5f);
		}
	}

	public void RecheckExternalConnection()
	{
		if (externals.Count < 1)
		{
			return;
		}
		List<GameObject> list = new List<GameObject>();
		List<GameObject> list2 = new List<GameObject>();
		for (int i = 0; i < externals.Count; i++)
		{
			if (lightManager.AreLightsInRangeAndInView(LightComponent.transform, externals[i].lightOffsetPosition, lightManager.singularityMergeRange))
			{
				list.Add(externals[i].gameObject);
			}
			else
			{
				list2.Add(externals[i].gameObject);
			}
		}
		for (int j = 0; j < list2.Count; j++)
		{
			LightSingularityExternal component = list2[j].GetComponent<LightSingularityExternal>();
			component.MorphedLightSingularity.RemoveExternalFromList(component);
			component.MorphedLightSingularity = null;
		}
		lightManager.CreateNewSingularityFromMultiple(list2);
	}
}
