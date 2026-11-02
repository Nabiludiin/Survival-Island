using UnityEngine;

public class AI_State_AnglerFish_Dead : AI_State_Dead
{
	public Light light;

	public float deadLightRange = 1f;

	public float deadLightIntensity = 2f;

	public override void EnterState()
	{
		base.EnterState();
		light.intensity = deadLightIntensity;
		light.range = deadLightRange;
	}
}
