using FMODUnity;
using UnityEngine;

public class DeltaMovementSound : MonoBehaviour
{
	[SerializeField]
	private MovementDeltaTracker deltaTracker;

	[SerializeField]
	private StudioEventEmitter emitter;

	[SerializeField]
	private float minReqDeltaMagnitude;

	[SerializeField]
	private float currentDeltaMagnitude;

	private void Update()
	{
		if (emitter == null)
		{
			enabled = false;
			return;
		}
		currentDeltaMagnitude = deltaTracker.GetAverageDeltaVector().magnitude;
		if (currentDeltaMagnitude > minReqDeltaMagnitude)
		{
			emitter.PlaySafe();
		}
		else
		{
			emitter.StopSafe();
		}
	}
}
