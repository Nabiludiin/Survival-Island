using UnityEngine;

[CreateAssetMenu(fileName = "ValidPointGeneratorSettings", menuName = "Raft/ValidPointGeneratorSettings")]
public class SO_ValidPointGeneratorSettings : ScriptableObject
{
	public LayerMask layerMask;

	public float minPointY;

	public float maxPointY = 1000f;

	[Range(-1f, 1f)]
	public float minNormalAngle;

	[Range(-1f, 1f)]
	public float maxNormalAngle = 1f;

	public int generationDistanceBetweenPoints = 10;
}
