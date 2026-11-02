using FMODUnity;
using UnityEngine;

public class RadiationZone : DamageArea_Tick
{
	[SerializeField]
	private StudioEventEmitter emitter_radiation;

	protected override void Update()
	{
		base.Update();
		if (emitter_radiation != null)
		{
			Network_Player localPlayerInArea = GetLocalPlayerInArea();
			emitter_radiation.SetParameter("hazmat", (localPlayerInArea == null || !localPlayerInArea.HazmatSuit.Equipped) ? 0f : 1f);
		}
	}
}
