using System;
using System.Collections;
using FMODUnity;
using Steamworks;
using UnityEngine;

public class TangaroaCockpitLaunch : MonoBehaviour_ID_Network
{
	[SerializeField]
	private float totalCurveTime;

	[SerializeField]
	private float offsetFromCenter;

	[SerializeField]
	private AnimationCurve yPositionCurve;

	[SerializeField]
	private AnimationCurve offsetVelocityCurve;

	[SerializeField]
	private AnimationCurve xRotationCurve;

	[SerializeField]
	private AnimationCurve yRotationCurve;

	[SerializeField]
	private AnimationCurve zRotationCurve;

	[SerializeField]
	private ParticleSystem particle_sparks;

	[SerializeField]
	private ParticleSystem particle_smokeTrail;

	[SerializeField]
	private ParticleSystem particle_launch;

	[SerializeField]
	[Range(0f, 1f)]
	private float playLaunchTimeline;

	[SerializeField]
	private ParticleSystem particle_waterSplash;

	[SerializeField]
	[Range(0f, 1f)]
	private float playWaterSplashTimeline;

	[SerializeField]
	private Keypad launchKeypad;

	[SerializeField]
	private Transform transformToMove;

	[SerializeField]
	private GameObject flotationObject;

	[SerializeField]
	private SO_CameraShaker launchShakeSetting;

	[SerializeField]
	private SO_CameraShaker sparkShakeSetting;

	[SerializeField]
	private StudioEventEmitter emitter_Launch;

	private Vector3 startPosition;

	private Vector3 latestLaunchTargetLocalPositionXZ;

	private Raft_Network network;

	private bool launched;

	private void Awake()
	{
		startPosition = transformToMove.localPosition;
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		if (Raft_Network.IsHost)
		{
			Keypad keypad = launchKeypad;
			keypad.OnCodeComplete = (Action)Delegate.Combine(keypad.OnCodeComplete, new Action(OnKeypadCodeComplete));
		}
	}

	private void OnEnable()
	{
		if (launched)
		{
			SetCockpitInTimeline(1f, latestLaunchTargetLocalPositionXZ);
			flotationObject.SetActiveSafe(value: true);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		Keypad keypad = launchKeypad;
		keypad.OnCodeComplete = (Action)Delegate.Remove(keypad.OnCodeComplete, new Action(OnKeypadCodeComplete));
	}

	private void OnKeypadCodeComplete()
	{
		LaunchCockpitNetworked();
	}

	private void LaunchNetworked_Editor()
	{
		if (!Application.isPlaying)
		{
			Debug.Log("Can't launch cockpit when game is not running");
		}
		StopAllCoroutines();
		launched = false;
		LaunchCockpitNetworked();
	}

	private void Reset_Editor()
	{
		if (!Application.isPlaying)
		{
			Debug.Log("Can't launch cockpit when game is not running");
		}
		StopAllCoroutines();
		launched = false;
		Vector3 targetLocalPositionXZ = GetTargetLocalPositionXZ();
		SetCockpitInTimeline(0f, targetLocalPositionXZ);
	}

	public void LaunchCockpitNetworked()
	{
		if (Raft_Network.IsHost && !launched)
		{
			Vector3 targetLocalPositionXZ = (latestLaunchTargetLocalPositionXZ = GetTargetLocalPositionXZ());
			StopAllCoroutines();
			StartCoroutine(Launch(targetLocalPositionXZ));
			if (network == null)
			{
				network = ComponentManager<Raft_Network>.Value;
			}
			Message_TangaroaCockpitLaunch message = new Message_TangaroaCockpitLaunch(Messages.Tangaroa_CockpitLaunch, network.NetworkIDManager, ObjectIndex, targetLocalPositionXZ.x, targetLocalPositionXZ.z);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public RGD_LandmarkItem GetRGDSave()
	{
		if (!launched)
		{
			return new RGD_LandmarkItem(GetComponent<LandmarkItem>());
		}
		return new RGD_LandmarkItem_TangaroaCockpitLaunch(GetComponent<LandmarkItem>(), latestLaunchTargetLocalPositionXZ.x, latestLaunchTargetLocalPositionXZ.z);
	}

	public void RestoreRGDSave(RGD_LandmarkItem_TangaroaCockpitLaunch rgd)
	{
		if (rgd != null)
		{
			latestLaunchTargetLocalPositionXZ = new Vector3(rgd.TargetLocalX, 0f, rgd.TargetLocalZ);
			SetCockpitInTimeline(1f, latestLaunchTargetLocalPositionXZ);
			flotationObject.SetActiveSafe(value: true);
		}
	}

	private IEnumerator Launch(Vector3 targetLocalPositionXZ)
	{
		if (particle_sparks != null)
		{
			particle_sparks.Play();
		}
		if (emitter_Launch != null)
		{
			emitter_Launch.Play();
		}
		ScreenEffectManager screenEffectManager = ComponentManager<ScreenEffectManager>.Value;
		if (sparkShakeSetting != null && screenEffectManager != null && Helper.LocalPlayerIsWithinDistance(transform.position, 50f))
		{
			screenEffectManager.ShakeCamera(sparkShakeSetting);
		}
		bool hasPlayedWaterSplash = false;
		bool hasPlayedLaunch = false;
		float startTime = Time.time;
		float normalizedTime = 0f;
		while (normalizedTime < 1f)
		{
			normalizedTime = Mathf.Clamp01((Time.time - startTime) / totalCurveTime);
			SetCockpitInTimeline(normalizedTime, targetLocalPositionXZ);
			if (particle_waterSplash != null && !hasPlayedWaterSplash && normalizedTime >= playWaterSplashTimeline)
			{
				hasPlayedWaterSplash = true;
				particle_waterSplash.transform.position = transformToMove.position;
				particle_waterSplash.Play();
				flotationObject.SetActiveSafe(value: true);
			}
			if (!hasPlayedLaunch && normalizedTime >= playLaunchTimeline)
			{
				hasPlayedLaunch = true;
				particle_launch?.PlaySafe();
				particle_smokeTrail?.PlaySafe();
				particle_sparks?.StopSafe();
				if (launchShakeSetting != null && screenEffectManager != null && Helper.LocalPlayerIsWithinDistance(transform.position, 50f))
				{
					screenEffectManager.ShakeCamera(launchShakeSetting);
				}
			}
			yield return null;
		}
		particle_smokeTrail?.StopSafe();
		AchievementHandler.UnlockAchievement(AchievementType.ach_tangaroa_cockpitLaunch);
	}

	private void SetCockpitInTimeline(float normalizedTime, Vector3 targetLocalPositionXZ)
	{
		Vector3 vector = Vector3.Lerp(Vector3.zero, targetLocalPositionXZ, offsetVelocityCurve.Evaluate(normalizedTime));
		vector.y = yPositionCurve.Evaluate(normalizedTime);
		transformToMove.localPosition = startPosition + vector;
		transformToMove.localEulerAngles = new Vector3(xRotationCurve.Evaluate(normalizedTime), yRotationCurve.Evaluate(normalizedTime), zRotationCurve.Evaluate(normalizedTime));
		launched = normalizedTime > 0f;
		if (normalizedTime >= 1f && transformToMove != null)
		{
			Animator component = transformToMove.GetComponent<Animator>();
			if (component != null)
			{
				component.SetBool("Open", value: true);
			}
		}
	}

	private Vector3 GetTargetLocalPositionXZ()
	{
		Vector3 vector = -(ComponentManager<Raft>.Value.transform.position - transform.position).XZOnly().normalized;
		return transformToMove.parent.InverseTransformPoint(transform.position + vector * offsetFromCenter);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.Tangaroa_CockpitLaunch)
		{
			Message_TangaroaCockpitLaunch message_TangaroaCockpitLaunch = msg as Message_TangaroaCockpitLaunch;
			StopAllCoroutines();
			StartCoroutine(Launch(new Vector3(message_TangaroaCockpitLaunch.TargetLocalX, 0f, message_TangaroaCockpitLaunch.TargetLocalZ)));
			return false;
		}
		return base.Deserialize(msg, remoteID);
	}
}
