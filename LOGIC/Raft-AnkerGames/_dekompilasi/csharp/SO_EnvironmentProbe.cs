using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentProbeSettings", menuName = "Raft/EnvironmentProbe", order = 1)]
public class SO_EnvironmentProbe : ScriptableObject
{
	public bool customAmbientColors;

	public Gradient skyGradient;

	public Gradient equatorGradient;

	public Gradient groundGradient;

	[Range(0f, 1f)]
	public float ambientModifier;

	[Range(0f, 1f)]
	public float particleIntensityModifier = 0.5f;

	[Tooltip("Used when reflection settings is not NONE")]
	[Range(0f, 1f)]
	public float reflectionMultiplier = 1f;

	[Tooltip("Used when reflection settings is NONE")]
	[Range(0f, 1f)]
	public float reflectionMultiplierNone = 1f;
}
