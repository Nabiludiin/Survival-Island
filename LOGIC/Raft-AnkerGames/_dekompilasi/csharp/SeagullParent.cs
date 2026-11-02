using UnityEngine;

public class SeagullParent : MonoBehaviour
{
	public int currentSeagullCount;

	[SerializeField]
	private int maxSeagullCount = 2;

	[SerializeField]
	private float hoverHeightCenter;

	[SerializeField]
	private Interval_Float hoverHeightAboveTop;

	[SerializeField]
	private Interval_Float hoverCircleRadius;

	[Tooltip("In minutes")]
	[SerializeField]
	private Interval_Float spawnSeagullInterval;

	[Range(1f, 100f)]
	[SerializeField]
	private float spawnChance = 50f;

	[SerializeField]
	private Network_Host_Entities networkEntities;

	[SerializeField]
	private SeagullPoint seagullPointPrefab;

	private Raft_Network network;

	private Transform raftTransform;

	private float spawnTimer;

	private float spawnInterval;

	private ChatManager chatManager;

	private Network_Player localPlayer;

	public Vector3 HoverCenterPoint => new Vector3(raftTransform.position.x, raftTransform.position.y + BlockCreator.MaxHeight + hoverHeightCenter, raftTransform.position.z);

	private void SpawnSeagullButton()
	{
		spawnTimer = 0f;
		SpawnSeagull();
	}

	private void Awake()
	{
		ComponentManager<SeagullParent>.Value = this;
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		raftTransform = ComponentManager<Raft>.Value.transform;
		chatManager = ComponentManager<ChatManager>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
		if (Raft_Network.IsHost && networkEntities.spawnSeagulls)
		{
			spawnTimer = 0f;
			spawnInterval = spawnSeagullInterval.GetRandomValue();
			if (GameManager.IsInNewGame || GameManager.QuickStartGame)
			{
				SpawnSeagull();
			}
		}
	}

	private void Update()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		spawnTimer += Time.deltaTime;
		if (spawnTimer >= spawnInterval * 60f)
		{
			spawnTimer = 0f;
			spawnInterval = spawnSeagullInterval.GetRandomValue();
			if (Random.Range(0f, 100f) <= spawnChance)
			{
				SpawnSeagull();
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
	}

	public Transform AddSeagullPoint()
	{
		Vector3 insideUnitSphere = Random.insideUnitSphere;
		insideUnitSphere.y = 0f;
		Vector3 position = HoverCenterPoint + insideUnitSphere.normalized * hoverCircleRadius.GetRandomValue();
		position.y += hoverHeightAboveTop.GetRandomValue();
		return Object.Instantiate(seagullPointPrefab, position, Quaternion.identity, transform).transform;
	}

	private void SpawnSeagull()
	{
		if (Raft_Network.IsHost)
		{
			if (raftTransform == null)
			{
				raftTransform = ComponentManager<Raft>.Value.transform;
			}
			int num = ((Random.Range(0, 2) != 0) ? 1 : (-1));
			int num2 = ((Random.Range(0, 2) != 0) ? 1 : (-1));
			Vector3 position = raftTransform.position;
			position += Vector3.right * 50f * num;
			position += Vector3.forward * 50f * num2;
			position += Vector3.up * 10f + Vector3.up * BlockCreator.MaxHeight;
			networkEntities.CreateSeagull(position, SaveAndLoad.GetUniqueObjectIndex(), SaveAndLoad.GetUniqueObjectIndex(), NetworkUpdateManager.GetUniqueBehaviourIndex());
			currentSeagullCount++;
		}
	}
}
