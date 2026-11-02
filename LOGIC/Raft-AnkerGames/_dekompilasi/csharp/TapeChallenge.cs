using System;
using FMODUnity;
using UnityEngine;

public class TapeChallenge : MonoBehaviour
{
	public Action OnChallengeCompleted;

	[SerializeField]
	private EnvironmentLightAffector environmentLightAffector;

	[SerializeField]
	private QuestInteractable[] tapeInteractables;

	[SerializeField]
	private ParticleSystem particle_electricity;

	[SerializeField]
	private MeshRenderer electricityWaterRenderer;

	[SerializeField]
	private DamageArea damageArea;

	public Intervaller intervaller_electricity;

	[SerializeField]
	private StudioEventEmitter emitter_ElectricityWater;

	[SerializeField]
	private float playSoundMaxPlayerYDistance = 5f;

	private Network_Water water;

	private MaterialPropertyBlock materialPropertyBlock;

	private Network_Player player;

	private bool challengeComplete;

	public bool ChallengeComplete
	{
		get
		{
			return challengeComplete;
		}
		set
		{
			if (value && !challengeComplete)
			{
				if (damageArea != null)
				{
					damageArea.enabled = false;
				}
				SetWaterElectricityState(electric: false);
				OnChallengeCompleted?.Invoke();
			}
			challengeComplete = value;
		}
	}

	private void Awake()
	{
		Intervaller intervaller = intervaller_electricity;
		intervaller.OnIntervalActivate = (Action)Delegate.Combine(intervaller.OnIntervalActivate, (Action)(() =>
		{
			SetWaterElectricityState(electric: true);
		}));
		Intervaller intervaller2 = intervaller_electricity;
		intervaller2.OnIntervalDeactivate = (Action)Delegate.Combine(intervaller2.OnIntervalDeactivate, (Action)(() =>
		{
			SetWaterElectricityState(electric: false);
		}));
		player = ComponentManager<Network_Player>.Value;
		for (int num = 0; num < tapeInteractables.Length; num++)
		{
			QuestInteractable obj = tapeInteractables[num];
			obj.OnInteract = (Action<int, bool, Network_Player, bool>)Delegate.Combine(obj.OnInteract, new Action<int, bool, Network_Player, bool>(OnInteractUpdateState));
		}
		if (Raft_Network.IsHost)
		{
			SaveAndLoad.LoadComplete = (Action)Delegate.Combine(SaveAndLoad.LoadComplete, new Action(UpdateChallengeState));
		}
		else
		{
			Raft_Network.OnWorldReceivedLate = (Action)Delegate.Combine(Raft_Network.OnWorldReceivedLate, new Action(UpdateChallengeState));
		}
		UpdateChallengeState();
	}

	private void Start()
	{
		water = ComponentManager<Network_Water>.Value;
	}

	private void OnDestroy()
	{
		player = null;
		water = null;
	}

	public void Update()
	{
		if (!ChallengeComplete)
		{
			if (intervaller_electricity != null && water != null)
			{
				intervaller_electricity.DoUpdate(water.WaterTime);
			}
			else
			{
				Debug.Log("intervaller or water ref is null", gameObject);
			}
		}
	}

	private void UpdateChallengeState()
	{
		if (AllInteractablesAtMaxIndex())
		{
			ChallengeComplete = true;
		}
	}

	private void SetWaterElectricityState(bool electric)
	{
		if (player == null)
		{
			player = ComponentManager<Network_Player>.Value;
		}
		if (electricityWaterRenderer != null)
		{
			if (materialPropertyBlock == null)
			{
				materialPropertyBlock = new MaterialPropertyBlock();
			}
			electricityWaterRenderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetFloat("_ElectricityAmount", electric ? 1f : 0f);
			electricityWaterRenderer.SetPropertyBlock(materialPropertyBlock);
		}
		if (electric)
		{
			if (player != null && Mathf.Abs(player.transform.position.y - emitter_ElectricityWater.transform.position.y) < playSoundMaxPlayerYDistance)
			{
				if (particle_electricity != null)
				{
					emitter_ElectricityWater?.PlaySafe();
				}
				particle_electricity?.Play();
				environmentLightAffector.PlayAffector(EnvironmentLightAffector.AffectorID.ElectricityWater);
			}
		}
		else
		{
			if (particle_electricity != null)
			{
				particle_electricity.Stop();
			}
			emitter_ElectricityWater?.StopSafe();
			environmentLightAffector.StopAffectors();
		}
	}

	private void OnInteractUpdateState(int objectStateIndex, bool atStart, Network_Player player, bool success)
	{
		UpdateChallengeState();
	}

	private bool AllInteractablesAtMaxIndex()
	{
		QuestInteractable[] array = tapeInteractables;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].IsAtMaxStateIndex)
			{
				return false;
			}
		}
		return true;
	}
}
