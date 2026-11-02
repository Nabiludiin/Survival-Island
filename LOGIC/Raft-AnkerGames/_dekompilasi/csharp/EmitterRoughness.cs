using FMODUnity;
using UnityEngine;

public class EmitterRoughness : MonoBehaviour
{
	[SerializeField]
	private StudioEventEmitter emitter;

	[SerializeField]
	private string parameterName = "roughness";

	private void Update()
	{
		if (emitter == null)
		{
			enabled = false;
		}
		else
		{
			emitter.SetParameter(parameterName, WaterRoughnessManager.Roughness);
		}
	}
}
