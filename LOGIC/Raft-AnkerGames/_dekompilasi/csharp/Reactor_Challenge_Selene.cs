using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

public class Reactor_Challenge_Selene : MonoBehaviour_ID_Network
{
	[SerializeField]
	private Fight_Challenge_Selene[] cranks;

	[SerializeField]
	private QuestType questToCompleteWholeReactor = QuestType.TemperanceReactorComplete;

	[SerializeField]
	private QuestInteractable interactableDoor;

	[SerializeField]
	private ParticleSystem coolingParticle;

	[SerializeField]
	private ParticleSystem radiationBubblingParticles;

	[SerializeField]
	private EnvironmentLightAffector radiationLightAffector;

	[SerializeField]
	private float fadeWaterTime = 5f;

	[SerializeField]
	private float fadeLightTime = 2f;

	[SerializeField]
	private MeshRenderer radiatedWaterRenderer;

	[SerializeField]
	private Color radiatedColor;

	[SerializeField]
	private Color radiatedFresnel;

	[SerializeField]
	private Color radiatedFoam;

	[SerializeField]
	private Color radiatedEmission;

	[SerializeField]
	private Color cleanColor;

	[SerializeField]
	private Color cleanFresnel;

	[SerializeField]
	private Color cleanFoam;

	[SerializeField]
	private Color cleanEmission;

	[SerializeField]
	private StudioEventEmitter emitter_reactorBottom;

	[SerializeField]
	private StudioEventEmitter emitter_reactorTop;

	[SerializeField]
	private StudioEventEmitter emitter_coolDown;

	private bool isCompleted;

	private MaterialPropertyBlock matProp;

	private void Awake()
	{
		Fight_Challenge_Selene[] array = cranks;
		foreach (Fight_Challenge_Selene obj in array)
		{
			obj.OnCompletion = (Action<Fight_Challenge_Selene>)Delegate.Combine(obj.OnCompletion, (Action<Fight_Challenge_Selene>)((Fight_Challenge_Selene completedCrank) =>
			{
				UpdateChallengeState();
			}));
		}
		if (Raft_Network.IsHost)
		{
			SaveAndLoad.LoadComplete = (Action)Delegate.Combine(SaveAndLoad.LoadComplete, new Action(OnLoadOrWorldRecieved));
		}
		else
		{
			Raft_Network.OnWorldReceivedLate = (Action)Delegate.Combine(Raft_Network.OnWorldReceivedLate, new Action(OnLoadOrWorldRecieved));
		}
		UpdateChallengeState();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		SaveAndLoad.LoadComplete = (Action)Delegate.Remove(SaveAndLoad.LoadComplete, new Action(OnLoadOrWorldRecieved));
		Raft_Network.OnWorldReceivedLate = (Action)Delegate.Remove(Raft_Network.OnWorldReceivedLate, new Action(OnLoadOrWorldRecieved));
	}

	public void TEMPEDITOR_COMPLETE()
	{
		if (Application.isPlaying)
		{
			CompleteReactorChallenge();
		}
		else
		{
			Debug.Log("Game needs to be in play mode");
		}
	}

	private void OnLoadOrWorldRecieved()
	{
		if (QuestProgressTracker.HasFinishedQuest(questToCompleteWholeReactor))
		{
			CompleteReactorChallenge();
		}
	}

	private void UpdateChallengeState()
	{
		if (IsAllCranksCompleted())
		{
			CompleteReactorChallenge();
		}
	}

	private bool IsAllCranksCompleted()
	{
		Fight_Challenge_Selene[] array = cranks;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].isCompleted)
			{
				return false;
			}
		}
		return true;
	}

	private void CompleteReactorChallenge()
	{
		if (!isCompleted)
		{
			isCompleted = true;
			QuestProgressTracker.FinishQuest(questToCompleteWholeReactor);
			coolingParticle.Play();
			Invoke("DoorOpen", 2f);
			FadeOutReactorLights();
			CoroutineManager.Singleton.AddCoroutine(FadeWaterToBlue());
			DisableAllRadiationEmission();
			if (emitter_reactorBottom != null)
			{
				emitter_reactorBottom.SetParameter("reactor", 1f);
			}
			if (emitter_reactorTop != null)
			{
				emitter_reactorTop.EventInstance.keyOff();
			}
			if (!SaveAndLoad.IsGameLoading && emitter_coolDown != null)
			{
				emitter_coolDown.Play();
			}
		}
	}

	private void FadeOutReactorLights()
	{
		radiationLightAffector.TurnOffAffectorsSoft(fadeLightTime);
	}

	private IEnumerator FadeWaterToBlue()
	{
		if (matProp == null)
		{
			matProp = new MaterialPropertyBlock();
		}
		float timer = 0f;
		while (true)
		{
			timer += Time.deltaTime;
			float num = Mathf.Clamp01(timer / fadeWaterTime);
			radiatedWaterRenderer.GetPropertyBlock(matProp);
			matProp.SetColor("_Color", Color.Lerp(radiatedColor, cleanColor, num));
			matProp.SetColor("_FresnelColor", Color.Lerp(radiatedFresnel, cleanFresnel, num));
			matProp.SetColor("_FoamColor", Color.Lerp(radiatedFoam, cleanFoam, num));
			matProp.SetColor("_Emission", Color.Lerp(radiatedEmission, cleanEmission, num));
			radiatedWaterRenderer.SetPropertyBlock(matProp);
			if (!(num >= 1f))
			{
				yield return null;
				continue;
			}
			break;
		}
	}

	private void DisableAllRadiationEmission()
	{
		DisableRadiationParticleEmission(radiationBubblingParticles);
		ParticleSystem[] componentsInChildren = radiationBubblingParticles.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem ps in componentsInChildren)
		{
			DisableRadiationParticleEmission(ps);
		}
	}

	private void DisableRadiationParticleEmission(ParticleSystem ps)
	{
		if (!(ps == null))
		{
			ParticleSystem.EmissionModule emission = ps.emission;
			emission.enabled = false;
		}
	}

	private void DoorOpen()
	{
		if (Raft_Network.IsHost && !interactableDoor.IsAtMaxStateIndex)
		{
			interactableDoor.SendOnInteractNetwork(null, forceSuccess: true);
		}
	}
}
