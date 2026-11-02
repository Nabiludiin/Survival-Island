using System;
using FMODUnity;
using UnityEngine;

public class MetalDetector : UsableTool
{
	[SerializeField]
	private MeshRenderer lampRenderer;

	[SerializeField]
	private StudioEventEmitter beepEmitter;

	[SerializeField]
	private StudioEventEmitter flatLineEmitter;

	[SerializeField]
	private StudioEventEmitter useToolEmitter;

	private TreasurePointManager treasureManager;

	private TreasurePoint currentClosestPoint;

	private float beepApexDistance = 3f;

	private float distanceToClosest = float.MaxValue;

	private float searchTimer;

	private float minimumSearchCooldown = 0.15f;

	private float searchCooldown = 3f;

	private TimerEventer durabilityTimer;

	private bool remote_shouldBeep;

	private MaterialPropertyBlock mpbLamps;

	private static readonly float[] lampThresholds = new float[10] { 0f, 0.03f, 0.05f, 0.08f, 0.14f, 0.3f, 0.4f, 0.5f, 0.7f, 1f };

	private const float searchRadius = 1000f;

	public float BeepCooldownByDistance => Helper.MapValue(0f, 1f, NormalizedDistanceToClosestTreasure, searchCooldown, minimumSearchCooldown);

	public float NormalizedDistanceToClosestTreasure
	{
		get
		{
			if (!(currentClosestPoint != null))
			{
				return 0f;
			}
			return 1f - Helper.NormalizeValue(distanceToClosest, currentClosestPoint.currentInnerRadius, currentClosestPoint.currentRevelationRadius);
		}
	}

	protected override void Start()
	{
		base.Start();
		if (treasureManager == null)
		{
			treasureManager = ComponentManager<TreasurePointManager>.Value;
		}
		if (playerNetwork.IsLocalPlayer)
		{
			durabilityTimer = new TimerEventer(1f);
		}
		if (playerNetwork != null && playerNetwork.Animator != null)
		{
			PlayerAnimator animator = playerNetwork.Animator;
			animator.Action_OnDeSerializeNetworkProperties = (Action<Message_Player_Update>)Delegate.Combine(animator.Action_OnDeSerializeNetworkProperties, new Action<Message_Player_Update>(OnNetworkProperties));
		}
	}

	protected override void OnDestroy()
	{
		if (playerNetwork != null && playerNetwork.Animator != null)
		{
			PlayerAnimator animator = playerNetwork.Animator;
			animator.Action_OnDeSerializeNetworkProperties = (Action<Message_Player_Update>)Delegate.Combine(animator.Action_OnDeSerializeNetworkProperties, new Action<Message_Player_Update>(OnNetworkProperties));
		}
		durabilityTimer = null;
	}

	protected override bool Update()
	{
		if (!playerNetwork.IsLocalPlayer)
		{
			if (remote_shouldBeep)
			{
				searchTimer += Time.deltaTime;
				UpdateBeepingSound();
			}
		}
		else
		{
			if (base.Update() && isHoldingUseButton)
			{
				searchTimer += Time.deltaTime;
				if (durabilityTimer != null && NormalizedDistanceToClosestTreasure > 0f)
				{
					durabilityTimer.DoUpdate(Time.deltaTime);
					if (durabilityTimer.IsTimeBeyondCooldown())
					{
						playerNetwork.Inventory.RemoveDurabillityFromHotSlot();
						durabilityTimer.ResetTimer();
					}
				}
				if (treasureManager != null)
				{
					currentClosestPoint = treasureManager.GetClosestTreasurePointTo(playerNetwork.FeetPosition, 1000f, includeUnBuried: true, includeUnrevealed: true, out distanceToClosest);
					if (currentClosestPoint != null && distanceToClosest <= beepApexDistance && !currentClosestPoint.isRevealedViaDetector)
					{
						currentClosestPoint.SendNetworkRevealTreasure();
					}
				}
				else
				{
					Debug.Log("treasureManager is null");
				}
			}
			if (isHoldingUseButton)
			{
				UpdateBeepingSound();
			}
		}
		if (isHoldingUseButton || remote_shouldBeep)
		{
			useToolEmitter?.PlaySafe();
		}
		else
		{
			useToolEmitter?.StopSafe();
			flatLineEmitter?.StopSafe();
			beepEmitter?.StopSafe();
		}
		UpdateLamps();
		return true;
	}

	private void UpdateLamps()
	{
		bool flag = (playerNetwork.IsLocalPlayer ? isHoldingUseButton : remote_shouldBeep);
		int num = 0;
		if (flag)
		{
			float normalizedDistanceToClosestTreasure = NormalizedDistanceToClosestTreasure;
			float num2 = 1f / (float)(lampThresholds.Length - 1);
			num = Mathf.FloorToInt(normalizedDistanceToClosestTreasure / num2);
			if (normalizedDistanceToClosestTreasure > 0f && num == 0)
			{
				num = 1;
			}
		}
		if (mpbLamps == null)
		{
			mpbLamps = new MaterialPropertyBlock();
		}
		lampRenderer.GetPropertyBlock(mpbLamps);
		mpbLamps.SetFloat("_Lights", lampThresholds[num]);
		lampRenderer.SetPropertyBlock(mpbLamps);
	}

	private void UpdateBeepingSound()
	{
		if (playerNetwork != null && !playerNetwork.IsLocalPlayer)
		{
			currentClosestPoint = treasureManager.GetClosestTreasurePointTo(playerNetwork.FeetPosition, 1000f, includeUnBuried: true, includeUnrevealed: true, out distanceToClosest);
		}
		if (distanceToClosest <= beepApexDistance)
		{
			flatLineEmitter?.PlaySafe();
			return;
		}
		flatLineEmitter?.StopSafe();
		if (currentClosestPoint != null && NormalizedDistanceToClosestTreasure > 0f && searchTimer >= BeepCooldownByDistance)
		{
			searchTimer = 0f;
			PlayBeepSound(distanceToClosest);
		}
	}

	private void PlayBeepSound(float distance)
	{
		if (beepEmitter != null)
		{
			beepEmitter.Play();
		}
	}

	private void OnNetworkProperties(Message_Player_Update msg_Update)
	{
		if (gameObject.activeInHierarchy && !playerNetwork.IsLocalPlayer)
		{
			remote_shouldBeep = msg_Update.Anim_ItemHit;
		}
	}
}
