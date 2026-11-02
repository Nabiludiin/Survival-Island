using Pomp.Animation;
using UnityEngine;
using UnityEngine.AI;

public class AI_Movement : MonoBehaviour
{
	private class LerpData
	{
		public enum LerpType
		{
			RotationSpeed,
			MovementSpeed
		}

		private AI_Movement movement;

		private LerpType lerpType;

		private float targetSpeed;

		private float lerpDuration;

		private float lerpStartTime;

		public bool IsDone => Time.time - lerpStartTime >= lerpDuration;

		public LerpType Type => lerpType;

		public LerpData(AI_Movement movement, LerpType lerpType, float targetSpeed, float lerpDuration, float startSpeed)
		{
			this.movement = movement;
			this.lerpType = lerpType;
			AssignData(targetSpeed, lerpDuration, startSpeed);
		}

		public void AssignData(float targetSpeed, float lerpDuration, float startSpeed)
		{
			this.targetSpeed = targetSpeed;
			this.lerpDuration = lerpDuration;
			lerpStartTime = Time.time;
			if (Type == LerpType.MovementSpeed)
			{
				if (movement.MovementSpeed != startSpeed)
				{
					AssignSpeedValueToMovement(startSpeed);
				}
			}
			else if (Type == LerpType.RotationSpeed && movement.RotationSpeed != startSpeed)
			{
				AssignSpeedValueToMovement(startSpeed);
			}
		}

		public void HandleLerp()
		{
			float value = Mathf.Lerp(GetCurrentSpeed(), targetSpeed, Mathf.Clamp01((Time.time - lerpStartTime) / lerpDuration));
			AssignSpeedValueToMovement(value);
		}

		private void AssignSpeedValueToMovement(float value)
		{
			switch (lerpType)
			{
			case LerpType.RotationSpeed:
				movement.SetRotationSpeed(value, resetLerp: false);
				break;
			case LerpType.MovementSpeed:
				movement.SetMovementSpeed(value, resetLerp: false);
				break;
			}
		}

		private float GetCurrentSpeed()
		{
			return Type switch
			{
				LerpType.RotationSpeed => movement.RotationSpeed, 
				LerpType.MovementSpeed => movement.MovementSpeed, 
				_ => 0f, 
			};
		}
	}

	public bool debug;

	private CustomRayHitInfo customHitInfo;

	public CharacterController characterController;

	public WaterFloatSemih_Rigidbody waterFloat;

	public bool ignoreSlopeYRotation;

	[SerializeField]
	private float gravity;

	public bool useGravity;

	public float yVelocity;

	public bool hasRaftGroundUnderneath;

	public NavMeshAgent agent;

	public Transform model;

	public float unreachableDistanceY = 5f;

	public float unreachableDistanceXZ = 5f;

	private NavMeshPath fakePath;

	private Vector3 previousPosition;

	[HideInInspector]
	public Vector3 lastMovementDelta;

	[SerializeField]
	private float _movementSpeed = 2f;

	public float defaultMovementSpeed = 2f;

	[SerializeField]
	private float _rotationSpeed = 2f;

	public float defaultRotationSpeed = 2f;

	[SerializeField]
	private bool rotateFasterCloserToTarget;

	[SerializeField]
	private float rotateFasterRange = 1f;

	public LayerMask obstructionLayer;

	public bool castRayToGround;

	public float groundRayLength = 0.5f;

	public float groundRayRadius = 0.2f;

	public float groundRayYOffset = 0.5f;

	[SerializeField]
	private float castGroundRayInterval;

	public bool snapToGround;

	public float snapOffset;

	[SerializeField]
	private int constantGroundMaterial;

	[SerializeField]
	private int groundMaterial;

	[HideInInspector]
	public RaycastHit groundHit;

	private float castGroundRayTimer;

	public bool useCustomRayData;

	public float rayCooldown = 0.4f;

	public bool stayUnderneathWater;

	public bool showCollisionSettings;

	[SerializeField]
	private float rayLengthSide = 2f;

	[SerializeField]
	private float rayLengthFront = 3f;

	[SerializeField]
	private float rayStartOffset = 2f;

	[SerializeField]
	private float rayStartYOffset;

	[Tooltip("Size of boxcast to check collision in front")]
	[SerializeField]
	private float frontCollisionSize = 1f;

	[SerializeField]
	private Interval_Float avoidCollisionRotationSpeed;

	[SerializeField]
	private Interval_Float avoidCollisionSpeedFront;

	[Tooltip("If obstruction is infront, prefer turning upwards instead of downwards")]
	[SerializeField]
	private bool turnUpFronCollision = true;

	[SerializeField]
	private BoidSettings settings;

	private Vector3 velocity;

	[SerializeField]
	private MovementType movementType;

	public Vector3 previousWorldPosition;

	public Vector3 lastWorldVelocity;

	[SerializeField]
	private AnimationParameterController controllerParameter;

	private LerpData movementLerp;

	private LerpData rotationLerp;

	private Vector3 forward;

	private Vector3 position;

	private int lastValidGoldenIndex = -1;

	public MovementType MovementType => movementType;

	public float MovementSpeed => _movementSpeed;

	public float RotationSpeed => _rotationSpeed;

	public int GroundMaterial
	{
		get
		{
			if (!castRayToGround)
			{
				return constantGroundMaterial;
			}
			return groundMaterial;
		}
	}

	public bool UsesControllerParameter => controllerParameter != null;

	private void Awake()
	{
		fakePath = new NavMeshPath();
		if (useCustomRayData)
		{
			customHitInfo = new CustomRayHitInfo(transform, rayLengthSide, rayCooldown);
		}
	}

	private void Update()
	{
		lastWorldVelocity = (transform.position - previousWorldPosition) / Time.deltaTime;
		previousWorldPosition = transform.position;
		if (Raft_Network.IsHost)
		{
			HandleGroundRaycast();
			HandleLerpData(Time.deltaTime);
			if (movementType == MovementType.CharacterController)
			{
				CharacterControllerUpdate();
			}
		}
	}

	private void LateUpdate()
	{
		if (movementType != MovementType.None)
		{
			Vector3 movementDelta = GetMovementDelta(useFallBack: false);
			if (movementDelta != Vector3.zero)
			{
				lastMovementDelta = movementDelta;
			}
			previousPosition = transform.localPosition;
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (debug)
		{
			Vector3 vector = ((model != null) ? model.up : Vector3.up);
			Vector3 vector2 = transform.position + vector * groundRayYOffset;
			Vector3 vector3 = vector2 + Vector3.down * groundRayLength;
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine(vector2, vector3);
			Gizmos.DrawWireSphere(vector2, groundRayRadius);
			Gizmos.DrawWireSphere(vector3, groundRayRadius);
		}
	}

	private void CharacterControllerUpdate()
	{
		bool flag = false;
		bool flag2 = false;
		if (Physics.SphereCast(transform.position + Vector3.up * groundRayYOffset, groundRayRadius, Vector3.down, out var hitInfo, 100f + BlockCreator.MaxHeight, LayerMasks.MASK_GroundMask_Raft, QueryTriggerInteraction.Ignore))
		{
			flag = true;
			if (hitInfo.distance < groundRayLength)
			{
				flag2 = true;
			}
		}
		hasRaftGroundUnderneath = false;
		if (flag2)
		{
			yVelocity = 0f;
			if (transform.ParentedToRaft())
			{
				hasRaftGroundUnderneath = true;
			}
		}
		else if (flag)
		{
			if (useGravity)
			{
				yVelocity -= gravity * Time.deltaTime;
			}
			if (yVelocity != 0f)
			{
				characterController.Move(Vector3.up * yVelocity * Time.deltaTime);
			}
			hasRaftGroundUnderneath = true;
		}
		else
		{
			yVelocity = 0f;
		}
	}

	private void HandleGroundRaycast()
	{
		if (!castRayToGround)
		{
			return;
		}
		bool flag = castGroundRayInterval <= 0f;
		if (!flag)
		{
			castGroundRayTimer += Time.deltaTime;
			if (castGroundRayTimer >= castGroundRayInterval)
			{
				castGroundRayTimer = 0f;
				flag = true;
			}
		}
		if (flag && RayCastHitDownwards(LayerMasks.MASK_GroundMask, model.up, -model.up, groundRayLength))
		{
			groundMaterial = ((!Helper.ObjectIsOnLayer(groundHit.transform.gameObject, LayerMasks.MASK_GroundMask_NonRaft)) ? 1 : 2);
			if (snapToGround)
			{
				float y = groundHit.point.y - model.transform.position.y + snapOffset;
				model.localPosition = new Vector3(0f, y, 0f);
			}
		}
	}

	public bool RayCastHitDownwards(LayerMask layer, Vector3 upDirection, Vector3 rayDir, float rayLenght)
	{
		Vector3 vector = transform.position + upDirection * groundRayYOffset;
		bool flag = Physics.SphereCast(vector, groundRayRadius, rayDir, out groundHit, rayLenght, layer, QueryTriggerInteraction.Ignore);
		if (debug)
		{
			if (!(model != null))
			{
				_ = Vector3.right;
			}
			else
			{
				_ = model.right;
			}
			Vector3 end = vector + rayDir.normalized * rayLenght;
			Debug.DrawLine(vector, end, Color.magenta, 2f);
			if (flag)
			{
				Debug.DrawLine(vector, groundHit.point, Color.red, 2f);
			}
		}
		return flag;
	}

	public void ChangeMovementSpeedTowards(float target, float speed)
	{
		SetMovementSpeed(Mathf.MoveTowards(MovementSpeed, target, Time.deltaTime * speed));
	}

	public void LerpMovementSpeedTowards(float target, float time, float initialSpeed)
	{
		if (movementLerp == null)
		{
			movementLerp = new LerpData(this, LerpData.LerpType.MovementSpeed, target, time, initialSpeed);
		}
		else
		{
			movementLerp.AssignData(target, time, initialSpeed);
		}
	}

	public void ChangeRotationSpeedTowards(float target, float speed)
	{
		SetRotationSpeed(Mathf.MoveTowards(RotationSpeed, target, Time.deltaTime * speed));
	}

	public void LerpRotationSpeedTowards(float target, float time, float initialSpeed)
	{
		if (rotationLerp == null)
		{
			rotationLerp = new LerpData(this, LerpData.LerpType.RotationSpeed, target, time, initialSpeed);
		}
		else
		{
			rotationLerp.AssignData(target, time, initialSpeed);
		}
	}

	public void SetMovementType(MovementType type = MovementType.Translate)
	{
		movementType = type;
		switch (type)
		{
		case MovementType.Translate:
		case MovementType.Translate_Simple:
			SetTranslateActive();
			break;
		case MovementType.NavmeshAgent:
			SetNavmeshAgentActive();
			break;
		case MovementType.CharacterController:
			SetCharacterControllerActive();
			break;
		case MovementType.None:
			DisableMovementTypes();
			break;
		case MovementType.Rigidbody_LookObstruction:
			DisableMovementTypes();
			break;
		}
		if (!Raft_Network.IsHost)
		{
			if (agent != null && agent.enabled)
			{
				agent.TurnOffSafe();
			}
			if (characterController != null)
			{
				characterController.enabled = false;
			}
		}
	}

	private void DisableMovementTypes()
	{
		if (Raft_Network.IsHost)
		{
			if (characterController != null)
			{
				yVelocity = 0f;
				characterController.enabled = false;
			}
			if (agent != null)
			{
				agent.TurnOffSafe();
			}
		}
	}

	private void SetCharacterControllerActive()
	{
		if (Raft_Network.IsHost)
		{
			if (characterController != null)
			{
				yVelocity = 0f;
				characterController.enabled = true;
			}
			if (agent != null)
			{
				agent.TurnOffSafe();
			}
			if (model != null)
			{
				model.localEulerAngles = Vector3.zero;
			}
		}
	}

	private void SetNavmeshAgentActive()
	{
		if (Raft_Network.IsHost)
		{
			if (characterController != null)
			{
				characterController.enabled = false;
			}
			if (agent != null)
			{
				agent.TurnOnSafe();
			}
		}
	}

	private void SetTranslateActive()
	{
		if (Raft_Network.IsHost)
		{
			if (characterController != null)
			{
				characterController.enabled = false;
			}
			if (agent != null)
			{
				agent.TurnOffSafe();
			}
		}
	}

	public Vector3 GetMovementDelta(bool useFallBack)
	{
		if (useFallBack && lastMovementDelta != Vector3.zero)
		{
			return lastMovementDelta;
		}
		return transform.localPosition - previousPosition;
	}

	public Vector3 GetWorldVelocity()
	{
		return lastWorldVelocity;
	}

	public void SetMovementSpeed(float value, bool resetLerp = true)
	{
		if (UsesControllerParameter)
		{
			controllerParameter.ApplyParameter(AnimationParameterController.AnimationParamData.ID.MOVEMENT, value);
		}
		_movementSpeed = value;
		if (resetLerp)
		{
			movementLerp = null;
		}
	}

	public void SetRotationSpeed(float value, bool resetLerp = true)
	{
		_rotationSpeed = value;
		if (resetLerp)
		{
			rotationLerp = null;
		}
	}

	private void Move_RigidForward(Transform self, float movementSpeed, Rigidbody rigid)
	{
		if (!(self == null))
		{
			Vector3 vector = self.forward * movementSpeed;
			Vector3 vector2 = rigid.position + vector * Time.deltaTime;
			if (stayUnderneathWater && vector2.y > -0.1f)
			{
				vector2.y = -0.1f;
			}
			rigid.MovePosition(vector2);
		}
	}

	public void MoveLook_Rigid(Transform self, Vector3 position, float movementSpeed, float rotationSpeed, Rigidbody rigid)
	{
		if (!(self == null))
		{
			Move_RigidForward(self, movementSpeed, rigid);
			Look(self, position, rotationSpeed);
		}
	}

	public void Move_Translate(Transform self, Vector3 position, float movementSpeed)
	{
		if (!(self == null))
		{
			self.position = Vector3.MoveTowards(self.position, position, Time.deltaTime * movementSpeed);
		}
	}

	public void MoveForward_Translate(Transform self, float movementSpeed)
	{
		if (!(self == null))
		{
			self.position += self.forward * Time.deltaTime * movementSpeed;
		}
	}

	public void MoveLook_Translate(Transform self, Vector3 position, float movementSpeed, float rotateSpeed)
	{
		if (!(self == null))
		{
			MoveForward_Translate(self, movementSpeed);
			Look(self, position, rotateSpeed);
		}
	}

	public void Move_Teleport(Transform self, Vector3 position)
	{
		if (!(self == null))
		{
			self.position = position;
		}
	}

	public void MoveLookObstruction_Translate(Transform self, Vector3 position, float movementSpeed, float rotationSpeed)
	{
		if (self == null)
		{
			return;
		}
		if (customHitInfo == null)
		{
			customHitInfo = new CustomRayHitInfo(self, rayLengthSide, rayCooldown);
		}
		if (customHitInfo.lastUpdateTimeStamp == 0f || Time.time - customHitInfo.lastUpdateTimeStamp >= rayCooldown)
		{
			Vector3 vector = self.position - self.forward * rayStartOffset;
			Vector3 halfExtents = new Vector3(frontCollisionSize / 2f, frontCollisionSize / 2f, frontCollisionSize / 2f);
			customHitInfo.hitFront = Physics.BoxCast(vector, halfExtents, self.forward, out customHitInfo.forward.hit, Quaternion.identity, rayLengthFront, obstructionLayer);
			customHitInfo.left.ray = new Ray(vector, self.forward - self.right * 0.5f);
			customHitInfo.right.ray = new Ray(vector, self.forward + self.right * 0.5f);
			customHitInfo.left.DoRay(obstructionLayer);
			customHitInfo.right.DoRay(obstructionLayer);
			if (!customHitInfo.hitFront)
			{
				customHitInfo.ResetBoolHits(p_value: false);
			}
			customHitInfo.lastUpdateTimeStamp = Time.time;
		}
		if (debug)
		{
			customHitInfo.left.DebugRay();
			customHitInfo.right.DebugRay();
			customHitInfo.up.DebugRay();
			customHitInfo.down.DebugRay();
		}
		if (customHitInfo.hitLeft)
		{
			self.Rotate(self.up, Time.deltaTime * avoidCollisionRotationSpeed.GetValueFromNormalized(customHitInfo.left.HitNormalDistance()), Space.World);
		}
		else if (customHitInfo.hitRight)
		{
			self.Rotate(self.up, (0f - Time.deltaTime) * avoidCollisionRotationSpeed.GetValueFromNormalized(customHitInfo.right.HitNormalDistance()), Space.World);
		}
		if (customHitInfo.hitFront)
		{
			int num = ((!turnUpFronCollision) ? 1 : (-1));
			self.Rotate(self.right, Time.deltaTime * avoidCollisionSpeedFront.GetValueFromNormalized(customHitInfo.forward.HitNormalDistance(rayLengthFront)) * (float)num, Space.World);
		}
		if (!customHitInfo.hitFront && !customHitInfo.hitRight && !customHitInfo.hitLeft)
		{
			MoveLook_Translate(self, position, movementSpeed, rotationSpeed);
		}
		else
		{
			MoveForward_Translate(self, movementSpeed);
		}
	}

	public void MoveLookObstruction_RigidBodyMove(Transform self, Vector3 position, float movementSpeed, float rotationSpeed, Rigidbody rigid)
	{
		if (self == null)
		{
			return;
		}
		if (customHitInfo == null)
		{
			customHitInfo = new CustomRayHitInfo(self, rayLengthSide, rayCooldown);
		}
		_ = self.forward;
		if (customHitInfo.lastUpdateTimeStamp == 0f || Time.time - customHitInfo.lastUpdateTimeStamp >= rayCooldown)
		{
			Vector3 vector = self.position - self.forward * rayStartOffset;
			Vector3 halfExtents = new Vector3(frontCollisionSize / 2f, frontCollisionSize / 2f, frontCollisionSize / 2f);
			customHitInfo.hitFront = Physics.BoxCast(vector, halfExtents, self.forward, out customHitInfo.forward.hit, Quaternion.identity, rayLengthFront, obstructionLayer);
			customHitInfo.left.ray = new Ray(vector, self.forward - self.right * 0.5f);
			customHitInfo.right.ray = new Ray(vector, self.forward + self.right * 0.5f);
			customHitInfo.hitLeft = customHitInfo.left.DoRay(obstructionLayer);
			customHitInfo.hitRight = customHitInfo.right.DoRay(obstructionLayer);
			if (!customHitInfo.hitFront)
			{
				customHitInfo.ResetBoolHits(p_value: false);
			}
			customHitInfo.lastUpdateTimeStamp = Time.time;
		}
		if (debug)
		{
			customHitInfo.left.DebugRay();
			customHitInfo.right.DebugRay();
			customHitInfo.up.DebugRay();
			customHitInfo.down.DebugRay();
		}
		if (customHitInfo.hitLeft)
		{
			self.Rotate(self.up, Time.deltaTime * avoidCollisionRotationSpeed.GetValueFromNormalized(customHitInfo.left.HitNormalDistance()), Space.World);
		}
		else if (customHitInfo.hitRight)
		{
			self.Rotate(self.up, (0f - Time.deltaTime) * avoidCollisionRotationSpeed.GetValueFromNormalized(customHitInfo.right.HitNormalDistance()), Space.World);
		}
		if (customHitInfo.hitFront)
		{
			int num = ((!turnUpFronCollision) ? 1 : (-1));
			self.Rotate(self.right, Time.deltaTime * avoidCollisionSpeedFront.GetValueFromNormalized(customHitInfo.forward.HitNormalDistance(rayLengthFront)) * (float)num, Space.World);
		}
		if (!customHitInfo.hitFront && !customHitInfo.hitRight && !customHitInfo.hitLeft)
		{
			MoveLook_Rigid(self, position, movementSpeed, rotationSpeed, rigid);
		}
		else
		{
			Move_RigidForward(self, movementSpeed, rigid);
		}
	}

	public void MoveLookObstruction_CharacterController(Transform self, Vector3 direction, float movementSpeed, float rotationSpeed)
	{
		if (!(self == null))
		{
			Vector3 vector = self.position - self.forward * rayStartOffset + self.up * rayStartYOffset;
			new Vector3(frontCollisionSize / 2f, frontCollisionSize / 2f, frontCollisionSize / 2f);
			Vector3 vector2 = self.position + self.forward - self.right * 0.5f;
			Vector3 vector3 = self.position + self.forward + self.right * 0.5f;
			bool flag = Physics.Raycast(vector, vector2 - self.position, out var hitInfo, rayLengthSide, obstructionLayer);
			float normalizedValue = 1f - Mathf.Clamp01(hitInfo.distance / rayLengthSide);
			bool flag2 = Physics.Raycast(vector, vector3 - self.position, out hitInfo, rayLengthSide, obstructionLayer);
			float normalizedValue2 = 1f - Mathf.Clamp01(hitInfo.distance / rayLengthSide);
			if (debug)
			{
				Debug.DrawLine(vector, vector + (vector2 - self.position) * rayLengthSide, flag ? Color.red : Color.green);
				Debug.DrawLine(vector, vector + (vector3 - self.position) * rayLengthSide, flag2 ? Color.red : Color.green);
			}
			if (flag)
			{
				self.Rotate(self.up, Time.deltaTime * avoidCollisionRotationSpeed.GetValueFromNormalized(normalizedValue), Space.World);
			}
			else if (flag2)
			{
				self.Rotate(self.up, (0f - Time.deltaTime) * avoidCollisionRotationSpeed.GetValueFromNormalized(normalizedValue2), Space.World);
			}
			if (!flag2 && !flag)
			{
				MoveDirection_CharacterController(direction, movementSpeed);
			}
			else
			{
				MoveDirection_CharacterController(self.forward, movementSpeed);
			}
		}
	}

	public void Look(Transform self, Vector3 position, float rotateSpeed, bool ignoreZRotation = false)
	{
		if (self == null)
		{
			return;
		}
		if (ignoreZRotation)
		{
			position.y = self.position.y;
		}
		Vector3 vector = position - self.position;
		if (rotateFasterCloserToTarget)
		{
			float magnitude = vector.magnitude;
			float num = Vector3.Angle(self.forward, vector);
			if (magnitude <= rotateFasterRange && num > 75f)
			{
				SetRotationSpeed(RotationSpeed + Time.deltaTime * 3f);
			}
			else
			{
				SetRotationSpeed(Mathf.Lerp(rotateSpeed, defaultRotationSpeed, Time.deltaTime * 3f));
			}
		}
		LookDirection(self, vector, rotateSpeed);
	}

	public void LookDirection(Transform self, Vector3 direction, float rotateSpeed, bool ignoreZRotation = false)
	{
		if (direction != Vector3.zero)
		{
			if (ignoreZRotation)
			{
				direction.y = 0f;
			}
			float z = self.rotation.eulerAngles.z;
			self.rotation = Quaternion.Slerp(self.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotateSpeed);
			self.rotation = Quaternion.Euler(self.rotation.eulerAngles.x, self.rotation.eulerAngles.y, z);
		}
	}

	public void LookDirection(Vector3 direction, float rotateSpeed, bool ignoreZRotation = false)
	{
		LookDirection(model, direction, rotateSpeed, ignoreZRotation);
	}

	public void MoveDirection_CharacterController(Vector3 direction, float movementSpeed)
	{
		direction.Normalize();
		Vector3 vector = direction;
		if (characterController != null)
		{
			characterController.Move(vector * movementSpeed * Time.deltaTime);
		}
		if (model == null)
		{
			Debug.LogError("Can't rotate model on charactercontroller, please attach a model to AI_Movement script on " + transform.gameObject);
			return;
		}
		Quaternion b = Quaternion.LookRotation(direction);
		if (model != null)
		{
			model.rotation = Quaternion.Lerp(model.rotation, b, Time.deltaTime * RotationSpeed);
		}
	}

	public bool IsValidNavmeshPath(Network_Player player, bool snapToClosestNavmesh = false)
	{
		Vector3 feetPosition = player.FeetPosition;
		if (snapToClosestNavmesh || !player.PersonController.controller.isGrounded)
		{
			Vector3 closestPointOnNavMesh = Helper.GetClosestPointOnNavMesh(feetPosition, 2f);
			if (closestPointOnNavMesh == feetPosition)
			{
				return false;
			}
			return IsValidNavmeshPath(closestPointOnNavMesh);
		}
		return IsValidNavmeshPath(Helper.GetClosestPointOnNavMesh(player.FeetPosition, 2f));
	}

	public bool IsValidNavmeshPath(Vector3 destination)
	{
		if (agent == null)
		{
			return false;
		}
		if (agent.isOnNavMesh && agent.enabled && agent.CalculatePath(destination, fakePath) && fakePath.status == NavMeshPathStatus.PathComplete)
		{
			return true;
		}
		return false;
	}

	public bool MoveLook_Navmesh(Vector3 position, float movementSpeed)
	{
		if (agent == null)
		{
			Debug.Log("Can't use MoveLook_Navmesh because agent is null", gameObject);
			return false;
		}
		if (!agent.enabled || !agent.isOnNavMesh)
		{
			return false;
		}
		agent.updateRotation = false;
		agent.speed = movementSpeed;
		if (agent.destination != position)
		{
			agent.ResumeSafe();
			agent.SetDestination(position);
		}
		if (model == null)
		{
			Debug.LogError("Can't rotate model on navmesh, please attach a model to AI_Movement script on " + transform.gameObject);
		}
		else
		{
			Vector3 normalized = GetMovementDelta(useFallBack: false).normalized;
			if (normalized != Vector3.zero)
			{
				if (ignoreSlopeYRotation)
				{
					normalized.y = 0f;
				}
				Quaternion b = Quaternion.LookRotation(normalized);
				model.rotation = Quaternion.Lerp(model.rotation, b, Time.deltaTime * RotationSpeed);
			}
		}
		if (Mathf.Abs(agent.destination.y - position.y) >= unreachableDistanceY)
		{
			return false;
		}
		if (position.DistanceXZ(agent.destination) >= unreachableDistanceXZ)
		{
			return false;
		}
		return agent.pathStatus == NavMeshPathStatus.PathComplete;
	}

	public void GoldenRuleMovement(Vector3 target, Rigidbody rigid, float movementSpeed)
	{
		Vector3 vector = (target - position).normalized * settings.targetWeight;
		bool flag = IsHeadingForCollision(out var hit);
		if (flag)
		{
			Vector3 vector2 = ObstacleRays();
			if (debug)
			{
				Debug.DrawRay(GoldenRuleRayPos(), vector2.normalized * settings.collisionAvoidDst, Color.green, 1f);
			}
			vector += vector2.normalized * settings.avoidCollisionWeight;
		}
		else
		{
			lastValidGoldenIndex = -1;
		}
		if (settings.lerpRotation)
		{
			rigid.transform.rotation = Quaternion.Lerp(rigid.rotation, Quaternion.LookRotation(vector), Time.deltaTime * settings.rotationSpeed);
		}
		else
		{
			rigid.transform.rotation = Quaternion.RotateTowards(rigid.rotation, Quaternion.LookRotation(vector), Time.deltaTime * settings.rotationSpeed);
		}
		Vector3 b = rigid.position + rigid.transform.forward * movementSpeed * Time.deltaTime;
		if (flag && hit.collider != null)
		{
			float num = Vector3.Distance(rigid.position, b);
			float num2 = Vector3.Distance(transform.position, hit.point);
			if (num > num2)
			{
				b = rigid.position + rigid.transform.forward * num2 * Time.deltaTime;
			}
		}
		if (b.y > settings.surfaceHeightLevel)
		{
			b.y = settings.surfaceHeightLevel;
		}
		rigid.MovePosition(b);
		position = rigid.position;
		forward = rigid.transform.forward;
	}

	private Vector3 GoldenRuleRayPos()
	{
		return transform.position + transform.forward * settings.rayOffset;
	}

	private bool IsHeadingForCollision(out RaycastHit hit)
	{
		_ = Vector3.zero;
		if ((GoldenRuleRayPos() + forward * settings.avoidSurfaceDistance).y >= settings.surfaceHeightLevel)
		{
			hit = default;
			return true;
		}
		if (Physics.SphereCast(GoldenRuleRayPos(), settings.boundsRadius, forward, out hit, settings.collisionAvoidDst, settings.obstacleMask))
		{
			return true;
		}
		return false;
	}

	private Vector3 ObstacleRays()
	{
		Vector3[] directions = BoidHelper.directions;
		if (lastValidGoldenIndex != -1)
		{
			Vector3 vector = transform.TransformDirection(directions[lastValidGoldenIndex]);
			Ray ray = new Ray(GoldenRuleRayPos(), vector);
			if ((ray.origin + vector * settings.avoidSurfaceDistance).y <= settings.surfaceHeightLevel && !Physics.SphereCast(ray, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask))
			{
				return vector;
			}
		}
		for (int i = 0; i < directions.Length; i++)
		{
			Vector3 vector2 = transform.TransformDirection(directions[i]);
			Ray ray2 = new Ray(GoldenRuleRayPos(), vector2);
			if ((ray2.origin + vector2 * settings.avoidSurfaceDistance).y <= settings.surfaceHeightLevel)
			{
				if (debug)
				{
					Debug.DrawRay(ray2.origin, ray2.direction.normalized * settings.collisionAvoidDst, Color.red, 1f);
				}
				if (!Physics.SphereCast(ray2, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask))
				{
					lastValidGoldenIndex = i;
					return vector2;
				}
			}
		}
		return -forward;
	}

	private void HandleLerpData(float time)
	{
		if (movementLerp != null && !movementLerp.IsDone)
		{
			movementLerp.HandleLerp();
		}
		if (rotationLerp != null && !rotationLerp.IsDone)
		{
			rotationLerp.HandleLerp();
		}
	}
}
