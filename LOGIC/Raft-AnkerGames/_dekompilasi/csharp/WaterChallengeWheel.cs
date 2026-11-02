using FMODUnity;
using UnityEngine;

public class WaterChallengeWheel : MonoBehaviour
{
	[SerializeField]
	private Puzzle_Connector onwardsConnection;

	[SerializeField]
	private StudioEventEmitter emitter_waterFlow;

	private void Update()
	{
		if (onwardsConnection.a.IsConnected)
		{
			emitter_waterFlow.PlaySafe();
		}
		else
		{
			emitter_waterFlow.StopSafe();
		}
	}
}
