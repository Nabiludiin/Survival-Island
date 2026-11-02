using UltimateWater;
using UnityEngine;

public class Network_Water : MonoBehaviour
{
	[Range(0f, 100f)]
	[SerializeField]
	private float lerpSpeed = 20f;

	private Raft_Network network;

	private Water playwayWater;

	private float networkTime;

	public float WaterTime => playwayWater.Time;

	private void Awake()
	{
		ComponentManager<Network_Water>.Value = this;
	}

	private void Start()
	{
		playwayWater = GetComponent<Water>();
		network = ComponentManager<Raft_Network>.Value;
	}

	private void Update()
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		if (network != null && !Raft_Network.IsHost && !GameManager.IsLeavingGame)
		{
			SetWaterTime(Mathf.Lerp(playwayWater.Time, networkTime, Time.deltaTime * lerpSpeed));
		}
	}

	public void SetNetworkWaterTime(float newWaterTime)
	{
		networkTime = newWaterTime;
	}

	public void SetWaterTime(float newTime)
	{
		playwayWater.Time = newTime;
	}
}
