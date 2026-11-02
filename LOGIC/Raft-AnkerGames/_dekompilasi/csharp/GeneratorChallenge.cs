using System;
using FMODUnity;
using UnityEngine;

public class GeneratorChallenge : MonoBehaviour
{
	[SerializeField]
	private QuestInteractable[] childSlots;

	[SerializeField]
	private QuestType questToCompleteOnDone = QuestType.TangaroaGeneratorRepaired;

	[SerializeField]
	private GameObject particleParent;

	[SerializeField]
	private Renderer lightsRenderer;

	[SerializeField]
	private TimerEventer changeLightTimer;

	[SerializeField]
	private Animator fanAnimator;

	[SerializeField]
	private Light clawroomLight;

	[SerializeField]
	private Renderer clawroomLampRenderer;

	[SerializeField]
	private Material clawroomLampNormalMaterial;

	[SerializeField]
	private Material clawroomLampEmissiveMaterial;

	[SerializeField]
	private StudioEventEmitter eventEmitter_generatorOn;

	private bool challengeComplete;

	private MaterialPropertyBlock lightsMPB;

	private float lightValue;

	private float lightTargetValue;

	private bool hasLightsBeenMaxed;

	private void Awake()
	{
		for (int i = 0; i < childSlots.Length; i++)
		{
			QuestInteractable obj = childSlots[i];
			obj.OnInteract = (Action<int, bool, Network_Player, bool>)Delegate.Combine(obj.OnInteract, new Action<int, bool, Network_Player, bool>(OnChildInteract));
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
		lightsMPB = new MaterialPropertyBlock();
		lightsRenderer.GetPropertyBlock(lightsMPB);
		lightsMPB.SetFloat("_Lights", 0f);
		lightsRenderer.SetPropertyBlock(lightsMPB);
	}

	private void OnDestroy()
	{
		SaveAndLoad.LoadComplete = (Action)Delegate.Remove(SaveAndLoad.LoadComplete, new Action(UpdateChallengeState));
		Raft_Network.OnWorldReceivedLate = (Action)Delegate.Remove(Raft_Network.OnWorldReceivedLate, new Action(UpdateChallengeState));
	}

	private void OnEnable()
	{
		UpdateChallengeState();
	}

	private void Update()
	{
		if (challengeComplete)
		{
			if (changeLightTimer.DoUpdate(Time.deltaTime, autoResetTimer: true))
			{
				lightTargetValue = UnityEngine.Random.Range(0.35f, 0.65f);
			}
			float b = (hasLightsBeenMaxed ? lightTargetValue : 1f);
			float num = (hasLightsBeenMaxed ? 10f : 1f);
			lightValue = Mathf.Lerp(lightValue, b, Time.deltaTime * num);
			if (lightValue > 0.9f)
			{
				hasLightsBeenMaxed = true;
			}
			lightsRenderer.GetPropertyBlock(lightsMPB);
			lightsMPB.SetFloat("_Lights", lightValue);
			lightsRenderer.SetPropertyBlock(lightsMPB);
		}
	}

	private void OnChildInteract(int objectStateIndex, bool atStart, Network_Player player, bool success)
	{
		UpdateChallengeState();
		if (challengeComplete && !atStart)
		{
			QuestProgressTracker.FinishQuestRPC(questToCompleteOnDone);
		}
	}

	private void UpdateChallengeState()
	{
		if (AllChildrenAtMaxStateIndex())
		{
			challengeComplete = true;
		}
		particleParent?.SetActiveSafe(challengeComplete);
		clawroomLight?.gameObject.SetActiveSafe(challengeComplete);
		if (challengeComplete)
		{
			eventEmitter_generatorOn?.Play();
			fanAnimator.SetBool("Active", value: true);
			if (clawroomLampRenderer != null)
			{
				clawroomLampRenderer.sharedMaterial = clawroomLampEmissiveMaterial;
			}
		}
		else
		{
			eventEmitter_generatorOn?.Stop();
			fanAnimator.SetBool("Active", value: false);
			if (clawroomLampRenderer != null)
			{
				clawroomLampRenderer.sharedMaterial = clawroomLampNormalMaterial;
			}
		}
	}

	private bool AllChildrenAtMaxStateIndex()
	{
		QuestInteractable[] array = childSlots;
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
