using FMODUnity;
using UnityEngine;

public class WaterChallenge : MonoBehaviour
{
	[SerializeField]
	private WaterpumpChallenge pump;

	[SerializeField]
	private Puzzle_Connector_Auto startConnection;

	[SerializeField]
	private Puzzle_Connector endConnection;

	[SerializeField]
	private FillCounter waterTankFill;

	[SerializeField]
	private TimerEventer fillTimer;

	[SerializeField]
	private float fillAmoount;

	[SerializeField]
	private StudioEventEmitter emitter_waterFlow;

	[SerializeField]
	private bool byPassPump;

	private void Update()
	{
		bool flag = pump.Active || byPassPump;
		startConnection.gameObject.SetActiveSafe(flag);
		if (endConnection.IsConnected)
		{
			emitter_waterFlow.PlaySafe();
		}
		else
		{
			emitter_waterFlow.StopSafe();
		}
		if (Raft_Network.IsHost && flag && endConnection.IsConnected && fillTimer.DoUpdate(Time.deltaTime, autoResetTimer: true))
		{
			waterTankFill.IncrementNetworked(fillAmoount);
		}
	}
}
