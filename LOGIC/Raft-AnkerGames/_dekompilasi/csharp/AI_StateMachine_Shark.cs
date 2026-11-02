using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(WaterPointGetter))]
public class AI_StateMachine_Shark : AI_StateMachine_Animal
{
	[Serializable]
	public class Message_NewAIShark_Update : Message_NetworkBehaviour
	{
		[SerializeField]
		private float posX;

		[SerializeField]
		private float posY;

		[SerializeField]
		private float posZ;

		[SerializeField]
		private float rotX;

		[SerializeField]
		private float rotY;

		[SerializeField]
		private float rotZ;

		public bool bitingBait;

		public bool anim_BitingRaft;

		public Vector3 Position
		{
			get
			{
				return new Vector3(posX, posY, posZ);
			}
			private set
			{
				posX = value.x;
				posY = value.y;
				posZ = value.z;
			}
		}

		public Vector3 ForwardDirection
		{
			get
			{
				return new Vector3(rotX, rotY, rotZ);
			}
			private set
			{
				rotX = value.x;
				rotY = value.y;
				rotZ = value.z;
			}
		}

		public Message_NewAIShark_Update(Messages type, MonoBehaviour_Network behaviour, AI_StateMachine_Shark stateMachineShark)
			: base(type, behaviour)
		{
			bitingBait = stateMachineShark.biteRaftState.currentTargetBlock != null || (bool)stateMachineShark.biteBaitState;
			if (bitingBait)
			{
				Position = stateMachineShark.transform.position;
			}
			else
			{
				Position = stateMachineShark.transform.localPosition;
			}
			ForwardDirection = stateMachineShark.transform.forward;
			anim_BitingRaft = stateMachineShark.animController.GetBool("BitingRaft");
		}
	}

	public Action<float> OnDamageTakenAction;

	public Action<string> OnAnimationEvent;

	public StudioEventEmitterSustain newBiteSound;

	[SerializeField]
	public StudioEventEmitter soundemitter_bitePlayer;

	[SerializeField]
	public StudioEventEmitter soundemitter_takeDamage;

	[SerializeField]
	public StudioEventEmitter soundemitter_idle;

	[SerializeField]
	public StudioEventEmitter soundemitter_splash;

	[SerializeField]
	private ParticleController woodParticles;

	[SerializeField]
	private ParticleController waterSplash;

	public GameObject trackedRotational;

	public Vector3 targetPoint;

	public LayerMask obstructionMask;

	public Network_Player targetToAttack;

	public float playerVisionRange = 10f;

	[HideInInspector]
	private float searchBlockProgress;

	[SerializeField]
	private float searchBlockInterval = 30f;

	[HideInInspector]
	private float searchBaitIntervalProgress;

	[SerializeField]
	private float searchBaitInterval = 2f;

	[SerializeField]
	private float searchBaitCooldown = 60f;

	private float searchBaitCooldownProgress;

	[HideInInspector]
	public WaterPointGetter waterPoint;

	[HideInInspector]
	public Transform lockedPivot;

	public static List<SharkBait> sharkBaitsInWorld = new List<SharkBait>();

	[SerializeField]
	private float baitVisionRange = 25f;

	public AI_State circulationSurfaceState;

	public AI_State circulationDeepState;

	public AI_State diveState;

	public AI_State wayPointChaseState;

	public AI_State diveObstructedState;

	public AI_State moveAwayFromRaftState;

	public AI_State_Attack_Block_Shark biteRaftState;

	public AI_State_Attack_Block_Shark biteBaitState;

	[SerializeField]
	private Renderer sharkRenderer;

	private MaterialPropertyBlock materialPropertyBlock;

	[HideInInspector]
	public Raft_Network network;

	[HideInInspector]
	public RaftBounds raftBounds;

	private Network_Host_Entities hostEntities;

	public int sharkHapticEvent = -1;

	public float SearchBlockInterval => searchBlockInterval / GameModeValueManager.GetCurrentGameModeValue().sharkVariables.attackRateMultiplier;

	public override void Awake()
	{
		lockedPivot = SingletonGeneric<GameManager>.Singleton.lockedPivot;
		base.Awake();
		network = ComponentManager<Raft_Network>.Value;
		if (waterPoint == null)
		{
			waterPoint = GetComponent<WaterPointGetter>();
		}
		if (!Raft_Network.IsHost)
		{
			rigid.isKinematic = true;
		}
	}

	public override void Start()
	{
		base.Start();
		raftBounds = ComponentManager<RaftBounds>.Value;
		MeasureRotationDelta component = GetComponent<MeasureRotationDelta>();
		if (component == null)
		{
			Debug.Log("measure delta was not found on", gameObject);
		}
		if (Raft_Network.IsHost)
		{
			if (component != null)
			{
				component.SetTrackedTransform(trackedRotational.transform);
			}
		}
		else
		{
			if (component != null)
			{
				component.SetTrackedTransform(transform);
			}
			UnityEngine.Object.Destroy(gameObject.GetComponent<TransformLerpMover>());
		}
		RefreshHPShader(1f);
		animController.SetFloat("SwimSpeed", 0f);
		hostEntities = ComponentManager<Network_Host_Entities>.Value;
		hostEntities.SharkCount++;
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		hostEntities.SharkCount--;
		newBiteSound.Stop();
		if (sharkHapticEvent != -1)
		{
			SimpleMonoBehaviourSingleton<HapticManager>.Instance.SharkPool.ReleaseIndex(sharkHapticEvent);
			SimpleMonoBehaviourSingleton<HapticManager>.Instance.StopAtennuated((HapticEvent)sharkHapticEvent);
			sharkHapticEvent = -1;
		}
		if (soundemitter_takeDamage != null)
		{
			soundemitter_takeDamage.Stop();
		}
	}

	protected override void OnDamageTaken(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageType, bool isLocal)
	{
		OnDamageTakenAction?.Invoke(damage);
		if (soundemitter_takeDamage != null)
		{
			soundemitter_takeDamage.Play();
		}
		if (currentState != decayState && currentState != deadState)
		{
			base.OnDamageTaken(damage, hitPoint, hitNormal, damageType, isLocal);
			RefreshHPShader();
		}
	}

	protected override void OnSwitchDamageTakenState()
	{
		if (currentState == circulationSurfaceState)
		{
			ChangeState(circulationDeepState);
		}
	}

	protected override void UpdateStateMachine()
	{
		base.UpdateStateMachine();
		if (currentState == deadState || currentState == decayState || GameModeValueManager.GetCurrentGameModeValue().gameMode == GameMode.Creative || GameModeValueManager.GetCurrentGameModeValue().gameMode == GameMode.Peaceful)
		{
			return;
		}
		searchBaitCooldownProgress += Time.deltaTime;
		if (searchBaitCooldownProgress >= searchBaitCooldown)
		{
			searchBaitIntervalProgress += Time.deltaTime;
			if (searchBaitIntervalProgress >= searchBaitInterval && currentState != wayPointChaseState && currentState != diveState && currentState != biteBaitState)
			{
				searchBaitIntervalProgress = 0f;
				SharkBait baitInRange = GetBaitInRange();
				if (baitInRange != null)
				{
					ChangeState<SharkBait>(biteBaitState, baitInRange);
					ResetSearchBaitCooldownProgress();
				}
			}
		}
		searchBlockProgress += Time.deltaTime;
		if (searchBlockProgress >= SearchBlockInterval && currentState != wayPointChaseState && currentState != diveState && currentState != biteBaitState && currentState != biteRaftState && Mathf.Abs(waterPoint.GetWaterPoint(transform.position) - transform.position.y) <= 3f)
		{
			if (Helper.IsAPlayerOnTheRaft())
			{
				ChangeState(biteRaftState);
				searchBlockProgress = 0f;
			}
			else
			{
				SetSearchBlockProgressCooldown(10f);
			}
		}
	}

	public void ResetSearchBaitCooldownProgress()
	{
		searchBaitCooldownProgress = 0f;
	}

	public void SetSearchBlockProgressCooldown(float cooldown)
	{
		searchBlockProgress = SearchBlockInterval - cooldown;
	}

	public void FindAndSetTargetToAttack()
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		targetToAttack = Helper.ClosestPlayerInWaterToPoint(transform.position, 100f, includeDeadPlayers: false);
	}

	public Vector3 GetRandomWaterPointInVision(float minRadius, float maxRadius, Transform originTarget)
	{
		if (waterPoint == null)
		{
			waterPoint = GetComponent<WaterPointGetter>();
		}
		float num = waterPoint.GetWaterPoint(transform.position);
		Vector3 vector = new Vector3(0f, num, 0f);
		int num2 = 0;
		do
		{
			num2++;
			vector = GetRandomPointInVisionAroundTarget(originTarget.position, minRadius, maxRadius);
		}
		while (num2 < 100 && vector.y >= num);
		if (vector.y < GameManager.OceanBottom + 10f)
		{
			vector.y = GameManager.OceanBottom + 10f;
		}
		return vector;
	}

	public SharkBait GetBaitInRange()
	{
		for (int i = 0; i < sharkBaitsInWorld.Count; i++)
		{
			SharkBait sharkBait = sharkBaitsInWorld[i];
			if (sharkBait == null)
			{
				sharkBaitsInWorld.RemoveAt(i);
				i--;
			}
			else if (Helper.HasClearVisionAndWithinRange(transform.position, sharkBait.transform.position, obstructionMask, baitVisionRange))
			{
				return sharkBait;
			}
		}
		return null;
	}

	public bool IsDiveViable(float depthLenghtTest)
	{
		if (Physics.Raycast(transform.position, Vector3.down * depthLenghtTest, (int)obstructionMask))
		{
			return false;
		}
		return true;
	}

	public AI_State GetDiveOrRetreatState(float depthLenghtTest)
	{
		if (IsDiveViable(depthLenghtTest))
		{
			return diveState;
		}
		return diveObstructedState;
	}

	public void PlayParticleWood()
	{
		if (woodParticles != null)
		{
			woodParticles.PlayParticles();
		}
	}

	public void StopParticleWood()
	{
		if (woodParticles != null)
		{
			woodParticles.StopParticles();
		}
	}

	public void PlayParticleWaterSplash()
	{
		if (waterSplash != null)
		{
			waterSplash.PlayParticles();
		}
	}

	public void StopParticleWaterSplash()
	{
		if (waterSplash != null)
		{
			waterSplash.StopParticles();
		}
	}

	public void RefreshHPShader(float normalizedHPValue = -1f)
	{
		if (materialPropertyBlock == null)
		{
			materialPropertyBlock = new MaterialPropertyBlock();
		}
		float value = ((normalizedHPValue == -1f) ? networkBehaviour.networkEntity.stat_health.NormalValue : normalizedHPValue);
		sharkRenderer.GetPropertyBlock(materialPropertyBlock);
		materialPropertyBlock.SetFloat("_SharkHP", value);
		sharkRenderer.SetPropertyBlock(materialPropertyBlock);
	}

	private Vector3 GetRandomPointInVisionAroundTarget(Vector3 target, float minRadius, float maxRadius)
	{
		Vector3 zero = Vector3.zero;
		int num = 0;
		do
		{
			num++;
			zero = Helper.GetRandomPointAroundOrigin(target, minRadius, maxRadius);
		}
		while (num < 100 && (!Helper.HasClearVision(transform.position, zero, obstructionMask) || IsPointUnderneathTerrain(zero)));
		return zero;
	}

	private bool IsPointUnderneathTerrain(Vector3 point, float lenghtOffset = 3000f)
	{
		Vector3 start = point;
		start.y += lenghtOffset;
		if (Physics.Linecast(start, point, obstructionMask))
		{
			return true;
		}
		return false;
	}
}
