using UnityEngine;

namespace UltimateWater;

public class WaterRipplesProfile : ScriptableObject
{
	[Header("Settings")]
	[Range(0f, 1f)]
	[Tooltip("How fast wave amplitude decreases with time")]
	public float Damping = 0.3f;

	[Range(0f, 1f)]
	[Tooltip("How fast the waves spread")]
	public float Propagation = 1f;

	[Tooltip("Force inflicted by interacting objects")]
	public float Gain = 30f;

	[Tooltip("Wave amplitude decrease with depth")]
	public float HeightGain = 2f;

	[Tooltip("Wave amplitude decrease offset")]
	public float HeightOffset = 2f;

	[Tooltip("Wave height multiplier")]
	public float Amplitude = 1f;

	[Tooltip("How much smoothing is applied between iterations")]
	[Header("Smooth")]
	[Range(0f, 1f)]
	public float Sigma;

	[Header("Normals")]
	[Tooltip("How strong are wave normals")]
	public float Multiplier = 1f;

	[Tooltip("How wide is sampling distance for normal calculations")]
	public float Spread = 0.001f;

	private void OnValidate()
	{
		if (Application.isPlaying)
		{
			WaterSimulationArea[] array = Object.FindObjectsOfType<WaterSimulationArea>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].UpdateShaderVariables();
			}
		}
	}
}
