using System;
using System.Collections;
using FMODUnity;
using Steamworks;
using UnityEngine;

[RequireComponent(typeof(Network_Entity))]
public class Seagull : MonoBehaviour_NetworkTick
{
	public bool searchConstantly;

	[Header("Settings")]
	[SerializeField]
	private float turnSpeed = 10f;

	[SerializeField]
	private float flightSpeed = 5f;

	[SerializeField]
	private float diveSpeed = 10f;

	[SerializeField]
	private float stopDiveDistance = 3f;

	[SerializeField]
	private float reachPointDistance = 0.75f;

	[SerializeField]
	private float searchPlayerRadius = 2f;

	[SerializeField]
	private TimerEventer searchPlayerTimer;

	[SerializeField]
	private float searchScarecrowRadius = 6f;

	[SerializeField]
	private float raycastRadius = 0.2f;

	[SerializeField]
	private float broodTime = 320f;

	[SerializeField]
	private float removeDeadTime = 300f;

	[SerializeField]
	private Interval_Float flyAwayTimeInterval;

	[SerializeField]
	private float timeUntilEatCrop = 10f;

	[SerializeField]
	private float searchCropInterval = 5f;

	[SerializeField]
	private LayerMask itemSearchMask;

	[SerializeField]
	private LayerMask playerSearchMask;

	[SerializeField]
	private LayerMask hookSearchMask;

	[SerializeField]
	private bool dropEggInNest;

	[Header("Components")]
	public Network_Entity entityComponent;

	public Animator anim;

	[SerializeField]
	private Rigidbody rigidBody;

	[SerializeField]
	private Collider[] deadColliders;

	public PickupItem pickupItem;

	public PickupChanneling pickupItemChanneling;

	public WaterFloatSemih_Rigidbody waterFloat;

	public SeagullState currentState;

	private float currentSpeed;

	private bool reachedAltitude;

	private float animFlySpeed;

	private float animFlySpeedTarget;

	private float flyAwayTimer;

	private float flyAwayTime;

	[HideInInspector]
	public float broodTimer;

	private bool sitting;

	private Vector3 flyAwayPoint;

	private Transform pointToFollow;

	private Transform raftFollowParent;

	private Transform lockedPivotTransform;

	[HideInInspector]
	public Transform targetPoint;

	[HideInInspector]
	public Cropplot targetCropplot;

	[HideInInspector]
	public Scarecrow targetScarecrow;

	[HideInInspector]
	public BirdsNest currentNest;

	private Raft_Network network;

	private PlantManager localPlantManager;

	private Network_Host_Entities host_Entities;

	private TimerEventer findNestTimer;

	[SerializeField]
	private StudioEventEmitter eventEmitter_Damaged;

	[SerializeField]
	private StudioEventEmitter eventEmitter_Wingflap;

	[SerializeField]
	private StudioEventEmitter eventEmitter_Peck;

	[SerializeField]
	private StudioEventEmitter eventEmitter_Scream;

	[SerializeField]
	private Interval_Float screamInterval;

	[SerializeField]
	private float netPosLerpSpeed;

	[SerializeField]
	private float netRotLerpSpeed;

	private Vector3 networkPosition;

	private Vector3 networkForwardDirection;

	public float hitForce = 100f;

	public bool Sitting
	{
		get
		{
			return sitting;
		}
		set
		{
			sitting = value;
		}
	}

	private void SearchInstant()
	{
		FindCropplotAndNest();
	}

	private void Awake()
	{
		SwitchState(SeagullState.Hover);
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		raftFollowParent = SingletonGeneric<GameManager>.Singleton.raftFollowParent;
		lockedPivotTransform = SingletonGeneric<GameManager>.Singleton.lockedPivot;
		host_Entities = ComponentManager<Network_Host_Entities>.Value;
		host_Entities.SeagullCount++;
		flyAwayTime = flyAwayTimeInterval.GetRandomValue();
		entityComponent.OnDamageTakenEvent += OnDamageTaken;
		StartCoroutine(SetFlySpeed());
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Combine(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
		if (pickupItem != null && pickupItem.networkID != null)
		{
			pickupItem.networkID.ObjectIndex = ObjectIndex;
			NetworkIDManager.AddNetworkID(pickupItem.networkID, typeof(PickupItem_Networked));
		}
		if (Raft_Network.IsHost)
		{
			findNestTimer = new TimerEventer(searchCropInterval * 60f);
			PickupItem_Networked networkID = pickupItem.networkID;
			networkID.OnPickedUp = (Action)Delegate.Combine(networkID.OnPickedUp, new Action(OnPickedUp));
			TimerEventer timerEventer = findNestTimer;
			timerEventer.OnTimerReached = (Action)Delegate.Combine(timerEventer.OnTimerReached, new Action(FindCropplotAndNest));
		}
		searchPlayerTimer.Initialize();
		Invoke("LateStart", 2f);
	}

	private void LateStart()
	{
		transform.SetParent(raftFollowParent);
		pointToFollow = ComponentManager<SeagullParent>.Value.AddSeagullPoint();
	}

	private void Update()
	{
		animFlySpeed = Mathf.Lerp(animFlySpeed, animFlySpeedTarget, Time.deltaTime);
		anim.SetFloat("FlySpeed", animFlySpeed);
		if (!Raft_Network.IsHost)
		{
			if (waterFloat.enabled || waterFloat.setToSurface)
			{
				waterFloat.enabled = false;
				waterFloat.setToSurface = false;
			}
			if (!rigidBody.isKinematic)
			{
				rigidBody.isKinematic = true;
				rigidBody.useGravity = false;
			}
			if (transform.parent == null)
			{
				transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * netPosLerpSpeed);
			}
			else
			{
				transform.localPosition = Vector3.Lerp(transform.localPosition, networkPosition, Time.deltaTime * netPosLerpSpeed);
			}
			RotateTowards(networkForwardDirection, netRotLerpSpeed);
			return;
		}
		UpdateNetworkTick();
		switch (currentState)
		{
		case SeagullState.Hover:
			Update_Hover();
			break;
		case SeagullState.Dive:
			Update_Dive();
			break;
		case SeagullState.Peck:
			Update_Pick();
			break;
		case SeagullState.FlyAway:
			Update_FlyAway();
			break;
		case SeagullState.Dead:
			Update_Dead();
			break;
		case SeagullState.Ruva:
			Update_Ruva();
			break;
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		host_Entities.SeagullCount--;
		NetworkIDManager.RemoveNetworkTickID(this);
		NetworkIDManager.RemoveNetworkID(this);
		NetworkIDManager.RemoveNetworkID(entityComponent);
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Remove(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
		if (pickupItem != null && pickupItem.networkID != null)
		{
			NetworkIDManager.RemoveNetworkID(pickupItem.networkID);
		}
		if (Raft_Network.IsHost)
		{
			PickupItem_Networked networkID = pickupItem.networkID;
			networkID.OnPickedUp = (Action)Delegate.Remove(networkID.OnPickedUp, new Action(OnPickedUp));
			TimerEventer timerEventer = findNestTimer;
			timerEventer.OnTimerReached = (Action)Delegate.Remove(timerEventer.OnTimerReached, new Action(FindCropplotAndNest));
			findNestTimer = null;
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (currentState == SeagullState.None)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, searchPlayerRadius);
		}
		if (currentState == SeagullState.Peck && targetPoint != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(targetPoint.position, searchPlayerRadius);
		}
	}

	private void OnWorldShift(Vector3 shift)
	{
		if (currentState == SeagullState.Dead && GetParent() == SeagullParentEnum.None)
		{
			transform.position -= shift;
		}
		networkPosition -= shift;
	}

	private void Update_Hover()
	{
		if (pointToFollow == null)
		{
			return;
		}
		Debug.DrawLine(transform.position, pointToFollow.position, Color.green);
		currentSpeed = Mathf.Lerp(currentSpeed, flightSpeed, Time.deltaTime * 4f);
		MoveAndRotateTowardsPoint(pointToFollow.position, currentSpeed);
		float num = Mathf.Abs(transform.position.y - pointToFollow.position.y);
		reachedAltitude = num <= 2f;
		anim.SetBool("FlyingAltitude", reachedAltitude);
		anim.SetBool("Peck_Cropplot", value: false);
		if (Raft_Network.IsHost)
		{
			if (searchConstantly)
			{
				FindCropplotAndNest();
			}
			else
			{
				findNestTimer.DoUpdate(Time.deltaTime, autoResetTimer: true);
			}
		}
	}

	private void Update_FlyAway()
	{
		Debug.DrawLine(transform.position, flyAwayPoint, Color.red);
		if (MoveAndRotateTowardsPoint(flyAwayPoint, flightSpeed, 1f))
		{
			NetworkUpdateManager.SetBehaviourDead(this);
			SwitchState(SeagullState.None);
		}
	}

	private void Update_Dive()
	{
		if (targetPoint == null)
		{
			if (currentState != SeagullState.Hover)
			{
				SwitchState(SeagullState.Hover);
			}
			return;
		}
		Debug.DrawLine(transform.position, targetPoint.position, Color.green);
		currentSpeed = Mathf.SmoothStep(currentSpeed, diveSpeed, Time.deltaTime);
		if (MoveAndRotateTowardsPoint(targetPoint.position, currentSpeed, stopDiveDistance))
		{
			if (currentNest == null)
			{
				SwitchState(SeagullState.Peck);
			}
			else
			{
				SwitchState(SeagullState.Ruva);
			}
		}
	}

	private void Update_Pick()
	{
		if (targetPoint == null)
		{
			SwitchState(SeagullState.Hover);
			return;
		}
		Debug.DrawLine(transform.position, targetPoint.position, Color.green);
		currentSpeed = Helper.Damp(currentSpeed, 0.5f, 0.15f, Time.deltaTime);
		if (currentSpeed <= 0.55f)
		{
			Sitting = true;
		}
		if (MoveTowardsPoint(targetPoint.position, currentSpeed, reachPointDistance))
		{
			transform.position = targetPoint.position;
		}
		RotateTowards(targetPoint.forward, turnSpeed / 2f);
		if (searchPlayerTimer.DoUpdate(Time.deltaTime, autoResetTimer: true) && PlayerIsWithinVicinity(targetPoint.position, searchPlayerRadius))
		{
			SwitchState(SeagullState.Hover);
		}
	}

	private void Update_Dead()
	{
		if (Physics.Raycast(transform.position, Vector3.down, 0.5f, LayerMasks.MASK_Block))
		{
			if (waterFloat.enabled)
			{
				waterFloat.enabled = false;
				rigidBody.isKinematic = false;
				rigidBody.useGravity = true;
			}
			if (transform.parent != lockedPivotTransform)
			{
				transform.SetParent(lockedPivotTransform);
			}
		}
		else
		{
			waterFloat.enabled = true;
			if (transform.parent != null)
			{
				transform.SetParent(null);
			}
		}
	}

	private void Update_Ruva()
	{
		if (targetPoint == null)
		{
			SwitchState(SeagullState.Hover);
			return;
		}
		Debug.DrawLine(transform.position, targetPoint.position, Color.green);
		currentSpeed = Helper.Damp(currentSpeed, 0.3f, 0.15f, Time.deltaTime);
		Sitting = transform.parent == lockedPivotTransform;
		if (MoveTowardsPoint(targetPoint.position, currentSpeed, reachPointDistance))
		{
			transform.position = targetPoint.position;
		}
		Vector3 direction = -Vector3.Cross(Vector3.up, transform.right).normalized;
		RotateTowards(direction, turnSpeed / 2f);
		if (searchPlayerTimer.DoUpdate(Time.deltaTime, autoResetTimer: true))
		{
			if (PlayerIsWithinVicinity(targetPoint.position, searchPlayerRadius))
			{
				LeaveNest(layEggs: false);
			}
			return;
		}
		broodTimer += Time.deltaTime;
		if (broodTimer >= broodTime)
		{
			LeaveNest(layEggs: true);
		}
	}

	public void SwitchState(SeagullState newState)
	{
		switch (newState)
		{
		case SeagullState.Hover:
			if (targetScarecrow != null)
			{
				targetScarecrow.Occupied = false;
			}
			targetScarecrow = null;
			targetCropplot = null;
			currentNest = null;
			Sitting = false;
			anim.SetBool("Peck_Cropplot", value: false);
			anim.SetBool("Peck_Scarecrow", value: false);
			anim.SetBool("Nest", value: false);
			CancelInvoke();
			Invoke("Scream", screamInterval.GetRandomValue());
			transform.SetParent(raftFollowParent);
			break;
		case SeagullState.Dive:
			transform.SetParent(raftFollowParent);
			if (targetCropplot != null)
			{
				anim.SetBool("Peck_Cropplot", value: true);
			}
			else if (targetScarecrow != null)
			{
				anim.SetBool("Peck_Scarecrow", value: true);
			}
			else if (currentNest != null)
			{
				anim.SetBool("Nest", value: true);
			}
			CancelInvoke();
			currentSpeed = diveSpeed;
			break;
		case SeagullState.Peck:
			searchPlayerTimer.ResetTimer();
			anim.SetTrigger("Sit");
			transform.SetParent(lockedPivotTransform);
			CancelInvoke();
			Invoke("Peck", timeUntilEatCrop);
			break;
		case SeagullState.FlyAway:
		{
			CancelInvoke();
			Vector3 vector = -Vector3.Cross(Vector3.up, transform.right).normalized;
			flyAwayPoint = transform.position + vector * 200f;
			break;
		}
		case SeagullState.Dead:
		{
			anim.SetBool("Dead", value: true);
			pickupItemChanneling.SetPickupColliderState(state: true);
			Sitting = false;
			if (currentNest != null)
			{
				currentNest.Occuppied = false;
			}
			CancelInvoke();
			StopAllCoroutines();
			rigidBody.isKinematic = false;
			Collider[] array = deadColliders;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
			if (Raft_Network.IsHost && currentState != SeagullState.Dead)
			{
				StartCoroutine(RemoveFromGame(removeDeadTime));
			}
			break;
		}
		case SeagullState.Ruva:
			searchPlayerTimer.ResetTimer();
			anim.SetTrigger("Sit");
			broodTimer = 0f;
			transform.SetParent(lockedPivotTransform);
			CancelInvoke();
			StopAllCoroutines();
			break;
		}
		currentState = newState;
	}

	public Transform GetBirdlandingPos(Transform root)
	{
		for (int i = 0; i < root.childCount; i++)
		{
			Transform child = root.GetChild(i);
			if (child.tag == "BirdLanding")
			{
				return child;
			}
		}
		return root;
	}

	public SeagullParentEnum GetParent()
	{
		if (lockedPivotTransform == null && SingletonGeneric<GameManager>.Singleton != null)
		{
			lockedPivotTransform = SingletonGeneric<GameManager>.Singleton.lockedPivot;
		}
		if (transform == null || lockedPivotTransform == null || raftFollowParent == null)
		{
			return SeagullParentEnum.None;
		}
		if (lockedPivotTransform != null && transform.parent == lockedPivotTransform)
		{
			return SeagullParentEnum.LockedPivot;
		}
		if (transform.parent == raftFollowParent)
		{
			return SeagullParentEnum.RaftFollow;
		}
		return SeagullParentEnum.None;
	}

	private void OnDamageTaken(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageType, bool isLocal)
	{
		ParticleManager.PlaySystem("Blood_Spear", hitPoint, Quaternion.LookRotation(hitNormal));
		if (entityComponent.stat_health.IsDead() && currentState != SeagullState.Dead)
		{
			SwitchState(SeagullState.Dead);
			rigidBody.AddForce(-hitNormal * hitForce, ForceMode.Impulse);
			AchievementHandler.AddPlayerBirdKills();
		}
		else
		{
			if (currentState == SeagullState.Dead)
			{
				return;
			}
			eventEmitter_Damaged.Play();
			if (Raft_Network.IsHost)
			{
				if (currentState == SeagullState.Peck)
				{
					SwitchState(SeagullState.Hover);
				}
				else if (currentState == SeagullState.Ruva)
				{
					LeaveNest(layEggs: false);
				}
			}
		}
	}

	private void Peck()
	{
		if (targetCropplot != null)
		{
			foreach (PlantationSlot plantationSlot in targetCropplot.plantationSlots)
			{
				if (plantationSlot.busy)
				{
					if (localPlantManager == null)
					{
						localPlantManager = ComponentManager<Network_Player>.Value.PlantManager;
					}
					network.RPC(new Message_HarvestPlant(Messages.PlantManager_HarvestPlant, localPlantManager, plantationSlot.plant, giveYield: false), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					localPlantManager.Harvest(plantationSlot.plant, giveYield: false);
					break;
				}
			}
		}
		else if (targetScarecrow != null)
		{
			targetScarecrow.DamageScarecrow();
		}
		SwitchState(SeagullState.Hover);
	}

	private void FindCropplotAndNest()
	{
		if (!reachedAltitude)
		{
			return;
		}
		if (GameModeValueManager.GetCurrentGameModeValue().seagullVariables.attacksCrops)
		{
			foreach (Cropplot allCropplot in PlantManager.allCropplots)
			{
				if (!allCropplot.peckable || !allCropplot.ContainsCrops)
				{
					continue;
				}
				Vector3 vector = allCropplot.mainCollider.transform.TransformPoint(allCropplot.mainCollider.center);
				if (!CheckVisionToObject(allCropplot, vector, "Cropplot", QueryTriggerInteraction.Ignore) || PlayerIsWithinVicinity(vector, searchPlayerRadius))
				{
					continue;
				}
				bool scarecrowWasNearbyButNotValid = false;
				Scarecrow scarecrowInVicinity = GetScarecrowInVicinity(vector, searchScarecrowRadius, ref scarecrowWasNearbyButNotValid);
				if (scarecrowInVicinity == null)
				{
					if (!scarecrowWasNearbyButNotValid)
					{
						targetCropplot = allCropplot;
						targetPoint = GetBirdlandingPos(targetCropplot.transform);
						SwitchState(SeagullState.Dive);
					}
					return;
				}
				if (!scarecrowInVicinity.invurnerable)
				{
					targetScarecrow = scarecrowInVicinity;
					targetScarecrow.Occupied = true;
					targetPoint = GetBirdlandingPos(targetScarecrow.transform);
					SwitchState(SeagullState.Dive);
					return;
				}
			}
		}
		foreach (BirdsNest allNest in BirdsNest.AllNests)
		{
			if (!allNest.Occuppied)
			{
				Transform birdlandingPos = GetBirdlandingPos(allNest.transform);
				Vector3 vector2 = ((birdlandingPos != null) ? birdlandingPos.position : allNest.transform.position);
				if (CheckVisionToObject(allNest, vector2, "BirdsNest", QueryTriggerInteraction.Collide) && !PlayerIsWithinVicinity(vector2, searchPlayerRadius))
				{
					currentNest = allNest;
					currentNest.Occuppied = true;
					targetPoint = GetBirdlandingPos(currentNest.transform);
					SwitchState(SeagullState.Dive);
					break;
				}
			}
		}
	}

	private Scarecrow GetScarecrowInVicinity(Vector3 point, float radius, ref bool scarecrowWasNearbyButNotValid)
	{
		Collider[] array = Physics.OverlapSphere(point, radius, itemSearchMask);
		foreach (Collider collider in array)
		{
			if (!(collider.tag == "Scarecrow"))
			{
				continue;
			}
			Scarecrow scarecrow = collider.GetComponent<Scarecrow>();
			if (scarecrow == null)
			{
				scarecrow = collider.GetComponentInParent<Scarecrow>();
			}
			if (scarecrow != null)
			{
				if (scarecrow.Occupied)
				{
					scarecrowWasNearbyButNotValid = true;
				}
				else if (scarecrow.Health > 0)
				{
					return scarecrow;
				}
			}
		}
		return null;
	}

	private bool CheckVisionToObject<T>(T objectToCompare, Vector3 raycastTargetPoint, string tagToCheck, QueryTriggerInteraction hitTriggers) where T : MonoBehaviour
	{
		bool flag = true;
		float duration = 3f;
		Vector3 direction = raycastTargetPoint - transform.position;
		float maxDistance = direction.magnitude + 1f;
		if (Physics.SphereCast(transform.position, raycastRadius, direction, out var hitInfo, maxDistance, itemSearchMask, hitTriggers))
		{
			if (tagToCheck == string.Empty || hitInfo.transform.tag == tagToCheck)
			{
				T val = hitInfo.collider.GetComponent<T>();
				if (val == null)
				{
					val = hitInfo.collider.GetComponentInParent<T>();
				}
				if (!(val != null))
				{
					if (flag)
					{
						Debug.DrawLine(transform.position, raycastTargetPoint, Color.red, duration);
					}
					return false;
				}
				if (val == objectToCompare)
				{
					if (flag)
					{
						Debug.DrawLine(transform.position, raycastTargetPoint, Color.green, duration);
					}
					return true;
				}
				if (flag)
				{
					Debug.DrawLine(transform.position, raycastTargetPoint, Color.red, duration);
				}
			}
			else if (flag)
			{
				Debug.DrawLine(transform.position, raycastTargetPoint, Color.red, duration);
			}
		}
		return false;
	}

	private bool PlayerIsWithinVicinity(Vector3 point, float radius)
	{
		bool flag = Physics.CheckSphere(point, radius, playerSearchMask);
		bool flag2 = false;
		if (!flag)
		{
			Collider[] array = Physics.OverlapSphere(point, radius, hookSearchMask);
			for (int i = 0; i < array.Length; i++)
			{
				ThrowableImpactCheck component = array[i].GetComponent<ThrowableImpactCheck>();
				if (component != null && component.throwable.GetComponent<Hook>() != null)
				{
					flag2 = true;
					break;
				}
			}
		}
		return flag | flag2;
	}

	private IEnumerator SetFlySpeed()
	{
		float timer = 0f;
		while (true)
		{
			timer += Time.deltaTime;
			if (timer >= 5f)
			{
				timer = 0f;
				animFlySpeedTarget = UnityEngine.Random.Range(0f, 1f);
			}
			yield return null;
		}
	}

	private IEnumerator RemoveFromGame(float secondsDelay)
	{
		yield return new WaitForSeconds(secondsDelay);
		PickupObjectManager.RemovePickupItemNetwork(pickupItem.networkID);
	}

	private bool MoveTowardsPoint(Vector3 point, float speed, float reachDistance = 0.1f)
	{
		if (Vector3.Distance(transform.position, point) <= reachDistance)
		{
			return true;
		}
		Vector3 normalized = (point - transform.position).normalized;
		transform.position += normalized * Time.deltaTime * speed;
		return false;
	}

	private bool MoveAndRotateTowardsPoint(Vector3 point, float speed, float reachDistance = 0.1f)
	{
		if (Vector3.Distance(transform.position, point) <= reachDistance)
		{
			return true;
		}
		Vector3 normalized = (point - transform.position).normalized;
		MoveForward(speed);
		RotateTowards(normalized, turnSpeed);
		return false;
	}

	private void MoveForward(float speed)
	{
		transform.position += transform.forward * speed * Time.deltaTime;
	}

	private void RotateTowards(Vector3 direction, float speed)
	{
		if (direction != Vector3.zero)
		{
			Quaternion b = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Lerp(transform.rotation, b, Time.deltaTime * speed);
		}
	}

	private void LeaveNest(bool layEggs)
	{
		if (layEggs && dropEggInNest)
		{
			int num = UnityEngine.Random.Range(1, currentNest.eggs.Length + 1);
			network.RPC(new Message_Birdnest_Lay(Messages.BirdsNest_LayEgg, currentNest, num), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			currentNest.LayEgg(num);
		}
		int num2 = UnityEngine.Random.Range(1, currentNest.feathers.Length + 1);
		network.RPC(new Message_Birdnest_Lay(Messages.BirdsNest_LayFeather, currentNest, num2), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		currentNest.LayFeather(num2);
		currentNest.Occuppied = false;
		SwitchState(SeagullState.Hover);
	}

	private void OnPickedUp()
	{
		if (Raft_Network.IsHost)
		{
			NetworkIDManager.SendIDBehaviourDead(ObjectIndex, typeof(Seagull));
		}
	}

	public void Animation_Peck()
	{
		eventEmitter_Peck.Play();
	}

	public void Animation_Flap()
	{
		eventEmitter_Wingflap.Play();
	}

	private void Scream()
	{
		eventEmitter_Scream.Play();
		if (currentState == SeagullState.Hover)
		{
			Invoke("Scream", screamInterval.GetRandomValue());
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Seagull(RGDType.Entity_Seagull, this);
	}

	public override void Serialize_Update()
	{
		AddMessage(new Message_Seagull_Update(Messages.Seagull_Update, network.NetworkIDManager, ObjectIndex, this));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (this == null)
		{
			return false;
		}
		switch (msg.Type)
		{
		case Messages.Seagull_Update:
		{
			Message_Seagull_Update message_Seagull_Update = msg as Message_Seagull_Update;
			networkPosition = message_Seagull_Update.Position;
			networkForwardDirection = message_Seagull_Update.ForwardDirection;
			Sitting = message_Seagull_Update.Sitting;
			anim.SetBool("Sit", message_Seagull_Update.Anim_Sitting);
			anim.SetBool("FlyingAltitude", message_Seagull_Update.Anim_FlyingAltitude);
			anim.SetBool("Nest", message_Seagull_Update.Anim_Nest);
			anim.SetBool("Peck_Cropplot", message_Seagull_Update.Anim_Cropplot);
			anim.SetBool("Peck_Scarecrow", message_Seagull_Update.Anim_Scarecrow);
			if (message_Seagull_Update.SeagullParentEnum == SeagullParentEnum.RaftFollow)
			{
				if (transform.parent != raftFollowParent)
				{
					transform.SetParent(raftFollowParent);
				}
			}
			else if (message_Seagull_Update.SeagullParentEnum == SeagullParentEnum.LockedPivot)
			{
				if (transform.parent != lockedPivotTransform)
				{
					transform.SetParent(lockedPivotTransform);
				}
			}
			else if (transform.parent != null)
			{
				transform.SetParent(null);
			}
			currentState = message_Seagull_Update.SeagullState;
			return false;
		}
		case Messages.SetIDBehaviourDead:
			UnityEngine.Object.Destroy(gameObject);
			return true;
		case Messages.PickupChannel:
			if (pickupItemChanneling == null)
			{
				Debug.Log("Pickup channeling was null, probably something wrong on " + transform.name);
				return false;
			}
			return pickupItemChanneling.HandleNetworkMessageDeserialze(msg as Message_PickupChannel);
		default:
			return false;
		}
	}
}
