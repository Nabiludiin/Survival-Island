using System.Collections.Generic;
using UltimateWater.Utils;
using UnityEngine;

namespace UltimateWater.Samples;

[RequireComponent(typeof(Collider))]
public class WaterRain : MonoBehaviour
{
	[Range(0f, 1f)]
	public float Intensity = 0.1f;

	[MinMaxRange(0f, 2f)]
	public MinMaxRange Size = new MinMaxRange
	{
		MinValue = 0.3f,
		MaxValue = 0.5f
	};

	[MinMaxRange(0f, 8f)]
	public MinMaxRange Life = new MinMaxRange
	{
		MinValue = 0.5f,
		MaxValue = 2f
	};

	public float Force = 5f;

	private readonly List<WaterRaindropsIME> _Simulations = new List<WaterRaindropsIME>();

	private void Update()
	{
		for (int i = 0; i < _Simulations.Count; i++)
		{
			WaterRaindropsIME waterRaindropsIME = _Simulations[i];
			if (Random.Range(0f, 1f) < Intensity)
			{
				Vector3 velocity = -transform.up * Force;
				float num = Size.Random();
				Vector2 vector = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
				waterRaindropsIME.Spawn(velocity, num, Life.Random(), vector.x, vector.y);
				for (int j = 0; j < Random.Range(0, 20); j++)
				{
					Vector2 vector2 = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 0.05f;
					waterRaindropsIME.Spawn(Vector3.zero, num * 0.2f, 0.5f + Random.Range(0f, 0.4f), vector.x + vector2.x, vector.y + vector2.y);
				}
			}
		}
	}

	private void OnTriggerEnter(Collider colliderComponent)
	{
		WaterRaindropsIME component = colliderComponent.gameObject.GetComponent<WaterRaindropsIME>();
		if (component != null)
		{
			_Simulations.Add(component);
		}
	}

	private void OnTriggerExit(Collider colliderComponent)
	{
		WaterRaindropsIME component = colliderComponent.gameObject.GetComponent<WaterRaindropsIME>();
		if (component != null)
		{
			_Simulations.Remove(component);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
	}
}
