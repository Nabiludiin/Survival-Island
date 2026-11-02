using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(WaterPointGetter))]
public class Shark : MonoBehaviour_NetworkTick, SceneEventInterface
{
	public Network_Entity entityComponent;

	[SerializeField]
	private float newPointMinRadius = 10f;

	[SerializeField]
	private float newPointMaxRadius = 15f;

	public float swimSpeed;

	[SerializeField]
	private float rotationSpeed;

	public List<Item_Base> yield = new List<Item_Base>();

	public SharkState state = SharkState.None;

	[SerializeField]
	private AnimationCurve swimSpeedCurve;

	[SerializeField]
	private float animationCurveTime = 30f;

	[HideInInspector]
	public Vector3 targetPoint;

	private float originalSwimSpeed;

	[SerializeField]
	private float timeUntilSink = 60f;

	[SerializeField]
	private float sharkRespawnTime = 180f;

	private float deadTimer;

	[SerializeField]
	private float biteRaftRange = 2f;

	[SerializeField]
	private int biteRaftDamage = 5;

	[SerializeField]
	private float biteRaftInterval = 1f;

	[SerializeField]
	private float searchBlockInterval = 10f;

	[SerializeField]
	private Vector3 biteRaftOffset = Vector3.zero;

	[SerializeField]
	private float damageTakenUntilDive = 15f;

	private float hitByPlayerDamage;

	private float biteRaftTimer;

	private float searchBlockTimer;

	[HideInInspector]
	public bool bitingRaft;

	[HideInInspector]
	public Block targetBlock;

	[HideInInspector]
	public bool foundBlockToAttack;

	private int initialRaftMaxHealth;

	private int initialRaftHealth;

	[SerializeField]
	private float playerVisionRange = 10f;

	[SerializeField]
	private float bitePlayerRange = 2f;

	[Tooltip("Distance from player where attack animation begins")]
	[SerializeField]
	private float triggerAttackRange = 2f;

	[SerializeField]
	private float driveByDistance = 5f;

	[SerializeField]
	private int attackPlayerDamage = 10;

	[SerializeField]
	private float attackSwimSpeedMultiplier = 2f;

	private Vector3 lastKnownPlayerPosition;

	private bool canAttack = true;

	public static List<SharkBait> sharkBaitInWorld = new List<SharkBait>();

	[SerializeField]
	private float baitVisionRange = 20f;

	[SerializeField]
	private float biteBaitCooldown = 30f;

	private float biteBaitTimer;

	private bool damagedDuringBait;

	private SharkBait currentSharkBait;

	[Space(10f)]
	public Animator animator;

	[SerializeField]
	private LayerMask obstructionMask = 0;

	[SerializeField]
	private ParticleController woodParticles;

	[SerializeField]
	private ParticleController waterSplash;

	public Material sharkMaterial;

	public EventReference er_biteRaft;

	private EventInstance eventInstance_biteRaft;

	[SerializeField]
	private StudioEventEmitter soundemitter_bitePlayer;

	[SerializeField]
	private StudioEventEmitter soundemitter_takeDamage;

	[SerializeField]
	private StudioEventEmitter soundemitter_idle;

	[SerializeField]
	private StudioEventEmitter soundemitter_splash;

	[Space(10f)]
	[SerializeField]
	private float networkMovementLerpSpeed = 10f;

	[SerializeField]
	private float networkRotationLerpSpeed = 5f;

	[SerializeField]
	private Vector2 networkTurnAngleLimit = new Vector2(2.25f, 3f);

	private Vector3 networkPosition;

	private Vector3 networkRotation;

	[HideInInspector]
	public List<string> networkAnimTriggers = new List<string>();

	private bool bitingBait;

	private bool hitWhileSwimming;

	private Network_Player targetToAttack;

	private WaterPointGetter waterPoint;

	[HideInInspector]
	public Transform globalBuoyancyParent;

	private float swimcurveTimer;

	private Raft_Network network;

	public bool hasBitten;

	public Transform DebugTransform;

	public int AttackPlayerDamage => attackPlayerDamage * (int)GameModeValueManager.GetCurrentGameModeValue().sharkVariables.damageMultiplier;

	public float SearchBlockInterval => searchBlockInterval / GameModeValueManager.GetCurrentGameModeValue().sharkVariables.attackRateMultiplier;

	public float RespawnTime => sharkRespawnTime / GameModeValueManager.GetCurrentGameModeValue().sharkVariables.spawnRateMultiplier;

	private void Awake()
	{
		globalBuoyancyParent = SingletonGeneric<GameManager>.Singleton.buoyancyPivot;
		waterPoint = GetComponent<WaterPointGetter>();
		originalSwimSpeed = swimSpeed;
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Combine(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
		ChangeState(SharkState.PassiveWater);
	}

	private void Start()
	{
		entityComponent.OnDamageTakenEvent += OnDamageTaken;
		network = ComponentManager<Raft_Network>.Value;
		eventInstance_biteRaft = RuntimeManager.CreateInstance(er_biteRaft);
		soundemitter_idle.Play();
		sharkMaterial.SetFloat("_SharkHP", 1f);
		RandomizeStartingValues();
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			sharkBaitInWorld.Clear();
		}
	}

	private void Update()
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		if (state == SharkState.None)
		{
			return;
		}
		if (Raft_Network.IsHost)
		{
			UpdateNetworkTick();
		}
		if (!bitingRaft && (double)transform.position.y > -0.15)
		{
			transform.position = new Vector3(transform.position.x, -0.15f, transform.position.z);
		}
		if (!Raft_Network.IsHost)
		{
			float y = transform.eulerAngles.y;
			float x = transform.eulerAngles.x;
			if (!bitingRaft || bitingBait)
			{
				transform.position = Vector3.Lerp(transform.position, networkPosition, networkMovementLerpSpeed * Time.deltaTime);
			}
			else
			{
				transform.localPosition = Vector3.Lerp(transform.localPosition, networkPosition, networkMovementLerpSpeed * Time.deltaTime);
			}
			if (networkRotation != Vector3.zero)
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(networkRotation), networkRotationLerpSpeed * Time.deltaTime);
			}
			CalculateTurning(y, transform.eulerAngles.y, x, transform.eulerAngles.x);
			return;
		}
		swimcurveTimer += Time.deltaTime;
		if (swimcurveTimer > animationCurveTime)
		{
			swimcurveTimer = 0f;
		}
		float time = swimcurveTimer / animationCurveTime;
		float value = swimSpeedCurve.Evaluate(time);
		animator.SetFloat("SwimSpeed", value);
		if (swimSpeed > originalSwimSpeed)
		{
			swimSpeed = Mathf.Lerp(swimSpeed, originalSwimSpeed, Time.deltaTime);
		}
		if (DebugTransform != null)
		{
			DebugUpdate();
			return;
		}
		SetTargetToAttack();
		if (!entityComponent.IsDead)
		{
			if (GameModeValueManager.GetCurrentGameModeValue().sharkVariables.isTame && targetToAttack != null && state != SharkState.PassiveWater && state != SharkState.PassiveSurface)
			{
				ChangeState(SharkState.PassiveSurface);
			}
			if (targetToAttack == null && state != SharkState.NoTarget)
			{
				ChangeState(SharkState.NoTarget);
			}
		}
		switch (state)
		{
		case SharkState.NoTarget:
			NoTargetUpdate();
			break;
		case SharkState.PassiveSurface:
			PassiveSurfaceUpdate();
			break;
		case SharkState.PassiveWater:
			PassiveWaterUpdate();
			break;
		case SharkState.Dive:
			DiveUpdate();
			break;
		case SharkState.DeadSink:
			DeadSinkUpdate();
			break;
		case SharkState.Dead:
			DeadUpdate();
			break;
		}
		BaitUpdate();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkIDManager.RemoveNetworkID(entityComponent);
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Remove(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
		if (eventInstance_biteRaft.isValid())
		{
			eventInstance_biteRaft.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			eventInstance_biteRaft.release();
			eventInstance_biteRaft.clearHandle();
		}
		soundemitter_idle.Stop();
		soundemitter_bitePlayer.Stop();
		soundemitter_splash.Stop();
		soundemitter_takeDamage.Stop();
	}

	private void OnDrawGizmos()
	{
	}

	public void OnDamageTaken(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageType, bool isLocal)
	{
		ParticleManager.PlaySystem("Blood_Spear", hitPoint, Quaternion.LookRotation(hitNormal));
		sharkMaterial.SetFloat("_SharkHP", entityComponent.stat_health.NormalValue);
		if (soundemitter_takeDamage != null)
		{
			soundemitter_takeDamage.Play();
		}
		if (state == SharkState.DeadSink)
		{
			return;
		}
		if (entityComponent.stat_health.IsDead() && state != SharkState.Dead)
		{
			if (Raft_Network.IsHost)
			{
				AnalyticsHelper.CustomEvent(AnalyticsEvent.SHARK_DEATH);
			}
			AchievementHandler.AddEntityKillByType(AI_NetworkBehaviourType.Shark);
			ChangeState(SharkState.Dead);
		}
		else
		{
			if (state == SharkState.Dead)
			{
				return;
			}
			TriggerAnimation("Damaged");
			if (!canAttack & !hasBitten)
			{
				TriggerAnimation("Damaged");
				canAttack = false;
				hasBitten = true;
				targetPoint = GetRandomWaterPointInVision(driveByDistance, driveByDistance, targetToAttack.transform);
			}
			if (state == SharkState.AttackRaft)
			{
				if (targetBlock is SharkBait)
				{
					damagedDuringBait = true;
					biteBaitTimer = biteBaitCooldown;
					lastKnownPlayerPosition = targetToAttack.transform.position;
					ChangeState(SharkState.Dive);
				}
				else
				{
					hitByPlayerDamage += damage;
					if (hitByPlayerDamage >= damageTakenUntilDive)
					{
						ChangeState(SharkState.Dive);
					}
				}
			}
			else if (state == SharkState.PassiveSurface && !hitWhileSwimming)
			{
				hitWhileSwimming = true;
				StopCoroutine("ResetHitWhileSwimming");
				StartCoroutine(ResetHitWhileSwimming(20f));
				ChangeState(SharkState.PassiveWater);
			}
		}
	}

	public void ChangeState(SharkState newState)
	{
		if (newState == state)
		{
			return;
		}
		switch (newState)
		{
		case SharkState.NoTarget:
			targetPoint = GetRandomWaterPointInVision(newPointMinRadius, newPointMaxRadius, globalBuoyancyParent);
			break;
		case SharkState.PassiveSurface:
			targetPoint = GetRandomSurfacePointInVision(newPointMinRadius, newPointMaxRadius);
			break;
		case SharkState.PassiveWater:
			targetPoint = GetRandomWaterPointInVision(newPointMinRadius, newPointMaxRadius, globalBuoyancyParent);
			break;
		case SharkState.AttackPlayer:
			canAttack = true;
			hasBitten = false;
			break;
		case SharkState.AttackRaft:
			initialRaftMaxHealth = (initialRaftHealth = 0);
			foundBlockToAttack = false;
			biteRaftTimer = 0f;
			bitingRaft = false;
			animator.SetBool("BitingRaft", value: false);
			targetPoint = GetRandomSurfacePointInVision(newPointMinRadius, newPointMaxRadius);
			break;
		case SharkState.Dead:
			bitingRaft = false;
			transform.SetParent(null);
			TriggerAnimation("Dead");
			deadTimer = 0f;
			if (waterSplash != null)
			{
				waterSplash.StopParticles();
			}
			CancelInvoke("WoodParticles");
			soundemitter_idle.Stop();
			eventInstance_biteRaft.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			break;
		case SharkState.Dive:
		{
			if (currentSharkBait != null)
			{
				currentSharkBait.isAttacked = false;
				currentSharkBait = null;
			}
			targetBlock = null;
			transform.SetParent(null);
			biteRaftTimer = 0f;
			hitByPlayerDamage = 0f;
			bitingRaft = false;
			animator.SetBool("BitingRaft", value: false);
			RuntimeManager.AttachInstanceToGameObject(eventInstance_biteRaft, globalBuoyancyParent);
			eventInstance_biteRaft.keyOff();
			soundemitter_idle.Play();
			if (waterSplash != null)
			{
				waterSplash.StopParticles();
			}
			if (Physics.Raycast(transform.position, Vector3.down, out var hitInfo, float.PositiveInfinity, obstructionMask))
			{
				targetPoint = hitInfo.point;
			}
			else
			{
				targetPoint = transform.position + Vector3.down * 10f + transform.forward * 5f;
			}
			break;
		}
		case SharkState.DeadSink:
			TriggerAnimation("Dead");
			break;
		}
		state = newState;
	}

	private void RandomizeStartingValues()
	{
		searchBlockTimer += UnityEngine.Random.Range(0f, searchBlockInterval / 4f);
	}

	public bool CollectYield(Network_Player yieldingPlayer)
	{
		if (yieldingPlayer == null || yield == null || yield.Count <= 0)
		{
			return false;
		}
		Item_Base item_Base = yield[0];
		if (item_Base == null)
		{
			return false;
		}
		if (yieldingPlayer.IsLocalPlayer)
		{
			yieldingPlayer.Inventory.AddItem(item_Base.UniqueName, 1);
		}
		RemoveYield();
		return true;
	}

	public void RemoveYield()
	{
		if (yield.Count > 0)
		{
			yield.RemoveAt(0);
		}
		if (yield.Count <= 0 && state != SharkState.DeadSink)
		{
			ChangeState(SharkState.DeadSink);
		}
	}

	private void DebugUpdate()
	{
		if (DebugTransform != null)
		{
			MoveTowards(DebugTransform.position, 1f);
		}
	}

	private void OnWorldShift(Vector3 shift)
	{
		if (!bitingRaft)
		{
			transform.position -= shift;
			networkPosition = transform.position;
			targetPoint -= shift;
			lastKnownPlayerPosition -= shift;
		}
	}

	private void NoTargetUpdate()
	{
		if (MoveTowards(targetPoint))
		{
			targetPoint = GetRandomSurfacePointInVision(newPointMinRadius, newPointMaxRadius);
		}
		if (targetToAttack != null)
		{
			ChangeState(SharkState.PassiveSurface);
		}
	}

	private void PassiveSurfaceUpdate()
	{
		if (targetToAttack.PersonController.controllerType == ControllerType.Ground)
		{
			if (MoveTowards(targetPoint))
			{
				targetPoint = GetRandomSurfacePointInVision(newPointMinRadius, newPointMaxRadius);
			}
			if (GameModeValueManager.GetCurrentGameModeValue().sharkVariables.isTame)
			{
				return;
			}
			if (CanSearchForBait())
			{
				SharkBait baitInRange = GetBaitInRange();
				if (baitInRange != null)
				{
					targetBlock = baitInRange;
					ChangeState(SharkState.AttackRaft);
					return;
				}
			}
			searchBlockTimer += Time.deltaTime;
			if (searchBlockTimer >= SearchBlockInterval)
			{
				searchBlockTimer = 0f;
				ChangeState(SharkState.AttackRaft);
			}
		}
		else if (targetToAttack.PersonController.controllerType == ControllerType.Water)
		{
			ChangeState(SharkState.PassiveWater);
		}
	}

	private void PassiveWaterUpdate()
	{
		if (targetToAttack.PersonController.controllerType == ControllerType.Water)
		{
			if (MoveTowards(targetPoint))
			{
				targetPoint = GetRandomWaterPointInVision(newPointMinRadius, newPointMaxRadius, targetToAttack.transform);
			}
			if (GameModeValueManager.GetCurrentGameModeValue().sharkVariables.isTame)
			{
				return;
			}
			if (Helper.HasClearVisionAndWithinRange(transform.position, targetToAttack.transform.position, obstructionMask, playerVisionRange) && !targetToAttack.PlayerScript.IsDead && !targetToAttack.BedComponent.Sleeping)
			{
				ChangeState(SharkState.AttackPlayer);
			}
			else if (CanSearchForBait())
			{
				SharkBait baitInRange = GetBaitInRange();
				if (baitInRange != null)
				{
					targetBlock = baitInRange;
					ChangeState(SharkState.AttackRaft);
				}
			}
		}
		else if (targetToAttack.PersonController.controllerType == ControllerType.Ground)
		{
			ChangeState(SharkState.PassiveSurface);
		}
	}

	private void AttackRaftUpdate()
	{
		Debug.DrawLine(transform.position, targetPoint, Color.red);
		if (foundBlockToAttack && (targetBlock == null || targetBlock.Health <= 0 || (targetBlock is SharkBait && targetBlock.Health <= 0) || targetBlock.Reinforced))
		{
			bitingRaft = false;
			animator.SetBool("BitingRaft", bitingRaft);
			if (waterSplash != null)
			{
				waterSplash.StopParticles();
			}
			ChangeState(SharkState.Dive);
			return;
		}
		if (!bitingRaft)
		{
			if (!foundBlockToAttack)
			{
				if (MoveTowards(targetPoint, 1f))
				{
					targetPoint = GetRandomSurfacePointInVision(newPointMaxRadius, newPointMaxRadius * 2f);
					if (Mathf.Abs(waterPoint.GetWaterPoint(transform.position) - transform.position.y) < 3f)
					{
						if (targetBlock == null)
						{
							targetBlock = FindBlockToAttack();
						}
						if (targetBlock != null)
						{
							if (targetBlock.Reinforced)
							{
								targetBlock = null;
							}
							else
							{
								foundBlockToAttack = true;
							}
						}
						if (foundBlockToAttack)
						{
							initialRaftMaxHealth = targetBlock.MaxHealth;
							initialRaftHealth = targetBlock.Health;
							if (targetBlock is SharkBait)
							{
								currentSharkBait = targetBlock as SharkBait;
								currentSharkBait.isAttacked = true;
							}
						}
					}
				}
				if (!GameModeValueManager.GetCurrentGameModeValue().sharkVariables.isTame && Helper.HasClearVisionAndWithinRange(transform.position, targetToAttack.transform.position, obstructionMask, playerVisionRange) && Helper.IsValidWaterTarget(targetToAttack))
				{
					ChangeState(SharkState.AttackPlayer);
					return;
				}
			}
			else
			{
				targetPoint = targetBlock.transform.position;
				if (MoveTowards(targetPoint, biteRaftRange))
				{
					if (targetBlock is SharkBait)
					{
						transform.SetParent(targetBlock.transform);
					}
					else
					{
						transform.SetParent(globalBuoyancyParent);
					}
					bitingRaft = true;
					eventInstance_biteRaft.getPlaybackState(out var pLAYBACK_STATE);
					if (pLAYBACK_STATE != PLAYBACK_STATE.PLAYING)
					{
						RuntimeManager.AttachInstanceToGameObject(eventInstance_biteRaft, targetBlock.transform);
						eventInstance_biteRaft.start();
						soundemitter_idle.Stop();
					}
				}
			}
		}
		else
		{
			Vector3 position = targetBlock.transform.position;
			Vector3 position2 = transform.position;
			position2.y = position.y;
			Vector3 normalized = (position - position2).normalized;
			targetPoint = targetBlock.transform.position + new Vector3(0f, biteRaftOffset.y, 0f);
			float num = 1f;
			if (Block.IsBlockIndexFoundationTriangular(targetBlock.buildableItem.UniqueIndex))
			{
				Collider componentInChildren = targetBlock.GetComponentInChildren<MeshCollider>();
				Vector3 a = Physics.ClosestPoint(position2 + transform.forward, componentInChildren, componentInChildren.bounds.center, componentInChildren.transform.rotation);
				a.y = position.y;
				float value = Mathf.Clamp(Vector3.Distance(a, position), 0f, 0.75f);
				num = Helper.MapValue(0f, 0.75f, value, 0f, biteRaftOffset.z);
			}
			targetPoint -= normalized * (biteRaftOffset.z * num);
			if (Vector3.Distance(transform.position, targetPoint) > 0.05f)
			{
				transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime * swimSpeed);
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, transform.eulerAngles.y, 0f), Time.deltaTime * swimSpeed);
			}
			else if (waterSplash != null && !waterSplash.IsPlaying)
			{
				waterSplash.PlayParticles();
			}
			biteRaftTimer += Time.deltaTime;
			if (biteRaftTimer >= biteRaftInterval)
			{
				biteRaftTimer -= biteRaftInterval;
				WoodParticles();
				bool flag = targetBlock is SharkBait;
				bool flag2 = targetBlock.Health - biteRaftDamage <= 0;
				if (flag)
				{
					targetBlock.Damage(biteRaftDamage);
					if (flag2)
					{
						biteBaitTimer = biteBaitCooldown;
						ChangeState(SharkState.Dive);
					}
				}
				else
				{
					targetBlock.Damage(biteRaftDamage);
					int num2 = initialRaftHealth - 100;
					if (targetBlock.MaxHealth > initialRaftMaxHealth)
					{
						initialRaftMaxHealth = targetBlock.MaxHealth;
						initialRaftHealth = targetBlock.Health;
						num2 = initialRaftHealth - 100;
					}
					if (targetBlock.Health <= num2)
					{
						ChangeState(SharkState.Dive);
						if (targetBlock != null && targetBlock.Health > 0 && !flag)
						{
							BlockCreator.UpdateHealthNetwork(targetBlock);
						}
						return;
					}
				}
				if (targetBlock != null && targetBlock.Health > 0 && !flag)
				{
					BlockCreator.UpdateHealthNetwork(targetBlock);
				}
			}
		}
		animator.SetBool("BitingRaft", bitingRaft);
	}

	private void BaitUpdate()
	{
		if (biteBaitTimer > 0f)
		{
			biteBaitTimer -= Time.deltaTime;
		}
	}

	private void DiveUpdate()
	{
		Debug.DrawLine(transform.position, targetPoint, Color.magenta);
		if (!MoveTowards(targetPoint, 1.5f))
		{
			return;
		}
		if (damagedDuringBait)
		{
			damagedDuringBait = false;
			ChangeState(SharkState.AttackPlayer);
			return;
		}
		if (targetToAttack == null)
		{
			SetTargetToAttack();
		}
		if (targetToAttack.PersonController.controllerType == ControllerType.Water)
		{
			ChangeState(SharkState.PassiveWater);
		}
		else
		{
			ChangeState(SharkState.PassiveSurface);
		}
	}

	private void DeadUpdate()
	{
		deadTimer += Time.deltaTime;
		if (deadTimer >= timeUntilSink)
		{
			ChangeState(SharkState.DeadSink);
		}
	}

	private void DeadSinkUpdate()
	{
		transform.position += Vector3.down * Time.deltaTime;
		if (transform.position.y < GameManager.OceanBottom)
		{
			ChangeState(SharkState.None);
			Network_Host_Entities value = ComponentManager<Network_Host_Entities>.Value;
			int sharkCount = value.SharkCount;
			_ = 1;
			SpawnAdditionalShark(sharkCount);
			value.SharkCount--;
			NetworkUpdateManager.SetBehaviourDead(this);
		}
	}

	public bool SpawnAdditionalShark(int sharkCount)
	{
		if (GameModeValueManager.GetCurrentGameModeValue().gameMode == GameMode.Hardcore)
		{
			int num = 5;
			if (UnityEngine.Random.Range(0, 10) <= num - sharkCount)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	private bool MoveTowards(Vector3 point, float reachedPointDistance = 3f)
	{
		if (Vector3.Distance(point, transform.position) <= reachedPointDistance)
		{
			return true;
		}
		Quaternion b = Quaternion.LookRotation(point - transform.position);
		float y = transform.eulerAngles.y;
		float x = transform.eulerAngles.x;
		transform.position += transform.forward * swimSpeed * Time.deltaTime;
		transform.rotation = Quaternion.Lerp(transform.rotation, b, Time.deltaTime * rotationSpeed);
		if (Raft_Network.IsHost)
		{
			CalculateTurning(y, transform.eulerAngles.y, x, transform.eulerAngles.x);
		}
		return false;
	}

	private Block FindBlockToAttack()
	{
		float num = float.MaxValue;
		Block block = null;
		List<Block> placedBlocks = BlockCreator.GetPlacedBlocks();
		for (int i = 0; i < placedBlocks.Count; i++)
		{
			Block block2 = placedBlocks[i];
			if (block2 == null || block2.Reinforced)
			{
				continue;
			}
			int uniqueIndex = block2.buildableItem.UniqueIndex;
			if (uniqueIndex == 2 || uniqueIndex == 8 || uniqueIndex == 189)
			{
				Vector3 position = block2.transform.position;
				position.y = transform.position.y;
				float num2 = Vector3.Distance(position, transform.position);
				if (num2 > 5f && (num2 < num || block == null))
				{
					num = num2;
					block = block2;
				}
			}
		}
		return block;
	}

	private Vector3 GetRandomSurfacePointInVision(float minRadius, float maxRadius)
	{
		if (waterPoint == null)
		{
			waterPoint = GetComponent<WaterPointGetter>();
		}
		float num = waterPoint.GetWaterPoint(transform.position);
		Vector3 randomPointInVisionAroundTarget = GetRandomPointInVisionAroundTarget(globalBuoyancyParent.position, minRadius, maxRadius);
		if (hitWhileSwimming)
		{
			num -= 8f;
		}
		else if (WaterRoughnessManager.RoughForAnimals)
		{
			num -= 3f;
		}
		if (randomPointInVisionAroundTarget.y > num)
		{
			randomPointInVisionAroundTarget.y = num - 1f;
		}
		else if (randomPointInVisionAroundTarget.y < num - 2f)
		{
			randomPointInVisionAroundTarget.y = num - 2f;
		}
		return randomPointInVisionAroundTarget;
	}

	private Vector3 GetRandomWaterPointInVision(float minRadius, float maxRadius, Transform originTarget)
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
			if (num2 >= 100)
			{
				Debug.Log("SAFETY BREAK, SHARK GetRandomWaterPointInVision");
				break;
			}
		}
		while (vector.y >= num);
		if (vector.y < GameManager.OceanBottom + 10f)
		{
			vector.y = GameManager.OceanBottom + 10f;
		}
		return vector;
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
		while (num < 100 && !Helper.HasClearVision(transform.position, zero, obstructionMask));
		return zero;
	}

	private SharkBait GetBaitInRange()
	{
		for (int i = 0; i < sharkBaitInWorld.Count; i++)
		{
			SharkBait sharkBait = sharkBaitInWorld[i];
			if (sharkBait == null)
			{
				sharkBaitInWorld.RemoveAt(i);
				i--;
			}
			else if (Helper.HasClearVisionAndWithinRange(transform.position, sharkBait.transform.position, obstructionMask, baitVisionRange))
			{
				return sharkBait;
			}
		}
		return null;
	}

	private bool CanSearchForBait()
	{
		return biteBaitTimer <= 0f;
	}

	private void SetTargetToAttack()
	{
		float num = float.MaxValue;
		Network_Player network_Player = null;
		foreach (KeyValuePair<Network_UserId, Network_Player> remoteUser in network.remoteUsers)
		{
			PersonController personController = remoteUser.Value.PersonController;
			if (remoteUser.Value.PlayerScript.IsDead)
			{
				continue;
			}
			if (network_Player == null)
			{
				network_Player = remoteUser.Value;
				num = Vector3.Distance(transform.position, network_Player.transform.position);
				continue;
			}
			float num2 = Vector3.Distance(transform.position, personController.transform.position);
			if (personController.controllerType == ControllerType.Water && network_Player.PersonController.controllerType == ControllerType.Ground)
			{
				network_Player = remoteUser.Value;
				num = num2;
			}
			else if (num2 < num)
			{
				network_Player = remoteUser.Value;
				num = num2;
			}
		}
		targetToAttack = network_Player;
	}

	private void TriggerAnimation(string name)
	{
		animator.SetTrigger(name);
		if (!networkAnimTriggers.Contains(name))
		{
			networkAnimTriggers.Add(name);
		}
	}

	private void CalculateTurning(float previousY, float currentY, float previousX, float currentX)
	{
		int num = 0;
		float num2 = Mathf.Abs(currentY - previousY);
		if (num2 < (Raft_Network.IsHost ? 3f : networkTurnAngleLimit.y))
		{
			num = 0;
		}
		else
		{
			num = ((currentY > previousY) ? 1 : (-1));
			if (num2 >= 180f)
			{
				num *= -1;
			}
		}
		float num3 = animator.GetFloat("TurnRightLeft");
		animator.SetFloat("TurnRightLeft", Mathf.Lerp(num3, num, Time.deltaTime * rotationSpeed));
		animator.SetBool("Turning", num3 > 0.05f || num3 < -0.05f);
		num2 = Mathf.Abs(currentX - previousX);
		if (num2 < (Raft_Network.IsHost ? 2.25f : networkTurnAngleLimit.x))
		{
			num = 0;
		}
		else
		{
			num = ((!(currentX > previousX)) ? 1 : (-1));
			if (num2 >= 180f)
			{
				num *= -1;
			}
		}
		num3 = animator.GetFloat("TurnUpDown");
		animator.SetFloat("TurnUpDown", Mathf.Lerp(num3, num, Time.deltaTime * rotationSpeed * 1.5f));
		if (!animator.GetBool("Turning"))
		{
			animator.SetBool("Turning", num3 > 0.05f || num3 < -0.05f);
		}
	}

	private void WoodParticles()
	{
		if (woodParticles != null)
		{
			woodParticles.PlayParticles();
		}
	}

	private IEnumerator ResetHitWhileSwimming(float timeDelay)
	{
		yield return new WaitForSeconds(timeDelay);
		hitWhileSwimming = false;
	}

	public void Attack()
	{
		if (soundemitter_bitePlayer != null)
		{
			soundemitter_bitePlayer.Play();
		}
		if (targetToAttack == null || !Raft_Network.IsHost)
		{
			return;
		}
		if (!hasBitten && !targetToAttack.PersonController.HasRaftAsParent && Vector3.Distance(transform.position, targetToAttack.transform.position) <= bitePlayerRange)
		{
			Network_Entity component = targetToAttack.GetComponent<Network_Entity>();
			network.hostNetwork.DamageEntity(component, null, AttackPlayerDamage, Vector3.zero, Vector3.zero, EntityType.Enemy);
			if (component.IsDead)
			{
				AnalyticsHelper.CustomEvent(AnalyticsEvent.SHARK_KILLPLAYER);
			}
		}
		targetPoint = GetRandomWaterPointInVision(driveByDistance, driveByDistance, targetToAttack.transform);
		animator.ResetTrigger("Attack");
		hasBitten = true;
		canAttack = false;
	}

	public void Animation_Splash()
	{
		soundemitter_splash.Play();
		Debug.Log("SPLASH");
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Shark(RGDType.Entity_Shark, this);
	}

	public override void Serialize_Update()
	{
		AddMessage(new Message_Shark(Messages.Shark_Update, this, this));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.Shark_Update:
		{
			Message_Shark message_Shark = msg as Message_Shark;
			if (this != null)
			{
				networkPosition = message_Shark.Position;
				networkRotation = message_Shark.ForwardDirection;
				if (state == SharkState.Dead)
				{
					return false;
				}
				bitingBait = message_Shark.bitingBait;
				if (!bitingRaft && message_Shark.anim_BitingRaft)
				{
					if (waterSplash != null)
					{
						waterSplash.PlayParticles();
					}
					if (!IsInvoking("WoodParticles"))
					{
						InvokeRepeating("WoodParticles", biteRaftInterval, biteRaftInterval);
					}
					soundemitter_idle.Stop();
					eventInstance_biteRaft.getPlaybackState(out var pLAYBACK_STATE);
					if (pLAYBACK_STATE != PLAYBACK_STATE.PLAYING)
					{
						RuntimeManager.AttachInstanceToGameObject(eventInstance_biteRaft, transform);
						eventInstance_biteRaft.start();
					}
				}
				else if (bitingRaft && !message_Shark.anim_BitingRaft)
				{
					if (waterSplash != null)
					{
						waterSplash.StopParticles();
					}
					CancelInvoke("WoodParticles");
					soundemitter_idle.Play();
					if (!message_Shark.bitingBait)
					{
						RuntimeManager.AttachInstanceToGameObject(eventInstance_biteRaft, globalBuoyancyParent);
					}
					else
					{
						eventInstance_biteRaft.set3DAttributes(transform.position.To3DAttributes());
					}
					eventInstance_biteRaft.keyOff();
				}
				bitingRaft = message_Shark.anim_BitingRaft;
				animator.SetBool("BitingRaft", message_Shark.anim_BitingRaft);
				string[] anim_triggers = message_Shark.anim_triggers;
				foreach (string text in anim_triggers)
				{
					if (text == "Dead")
					{
						ChangeState(SharkState.Dead);
						networkAnimTriggers.Clear();
						return false;
					}
					animator.SetTrigger(text);
				}
				if (message_Shark.bitingBait)
				{
					transform.SetParent(null);
				}
				else if (bitingRaft && transform.parent != globalBuoyancyParent)
				{
					transform.SetParent(globalBuoyancyParent);
				}
				else if (!bitingRaft && transform.parent != null)
				{
					transform.SetParent(null);
				}
			}
			return false;
		}
		case Messages.RemoveNetworkBehaviour:
			UnityEngine.Object.Destroy(gameObject);
			return true;
		case Messages.Shark_Yield:
		{
			Message_NetworkBehaviour_SteamID message_NetworkBehaviour_SteamID = msg as Message_NetworkBehaviour_SteamID;
			Network_Player playerFromID = network.GetPlayerFromID(message_NetworkBehaviour_SteamID.SteamID);
			if (playerFromID == null)
			{
				return false;
			}
			return CollectYield(playerFromID);
		}
		default:
			return false;
		}
	}
}
