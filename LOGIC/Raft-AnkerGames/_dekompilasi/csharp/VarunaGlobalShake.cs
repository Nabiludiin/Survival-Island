using FMODUnity;
using UnityEngine;

public class VarunaGlobalShake : MonoBehaviour
{
	[SerializeField]
	private SO_CameraShaker shakeSettings;

	[SerializeField]
	private AreaZone areaZone;

	[SerializeField]
	private float shakeInterval = 5f;

	[SerializeField]
	private StudioEventEmitter emitter_rumble;

	private ScreenEffectManager screenEffectManager;

	private Network_Water networkWater;

	private Network_Player localPlayer;

	private Quest gateEnteredQuest;

	private float lastPlayedTime;

	private Quest bossQuest;

	private void Awake()
	{
		screenEffectManager = ComponentManager<ScreenEffectManager>.Value;
		networkWater = ComponentManager<Network_Water>.Value;
		bossQuest = QuestProgressTracker.GetQuestByType(QuestType.VarunaBossKilled);
	}

	private void Update()
	{
		if (!(screenEffectManager == null) && !(networkWater == null) && bossQuest != null && !bossQuest.isQuestComplete)
		{
			float num = networkWater.WaterTime % shakeInterval;
			float num2 = Time.time - lastPlayedTime;
			if ((double)num <= 0.1 && num2 > shakeInterval / 2f)
			{
				lastPlayedTime = Time.time;
				Shake();
			}
		}
	}

	private void Shake()
	{
		if (gateEnteredQuest == null)
		{
			gateEnteredQuest = QuestProgressTracker.GetQuestByType(QuestType.VarunaBossGateEntered);
		}
		if (gateEnteredQuest != null && !gateEnteredQuest.isQuestComplete)
		{
			if (localPlayer == null)
			{
				localPlayer = ComponentManager<Network_Player>.Value;
			}
			if (localPlayer != null && areaZone.GetNormalizedDistanceToPoint(localPlayer.transform.position) > 0f)
			{
				screenEffectManager.ShakeCamera(shakeSettings);
				emitter_rumble.Play();
			}
		}
	}
}
