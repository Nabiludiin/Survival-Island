using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using UnityEngine;

public class ClawChallenge : MonoBehaviour_ID_Network
{
	[SerializeField]
	private float clawMoveXZTime = 1f;

	[SerializeField]
	private float clawMoveYTime = 1f;

	[SerializeField]
	private float slotSize = 1f;

	[SerializeField]
	private float startLocalYPosition;

	[SerializeField]
	private float clawGoDownDistance = 1f;

	[SerializeField]
	private float clawLiftDistance = 0.25f;

	[SerializeField]
	private Vector2Int startSlotIndex = Vector2Int.zero;

	[SerializeField]
	private AreaZone_PlayerCounter clawArea;

	[SerializeField]
	private Transform clawTransformHorizontal;

	[SerializeField]
	private Transform clawTransformVertical;

	[SerializeField]
	private Transform clawTransform;

	[SerializeField]
	private Transform liftCrateParent;

	[SerializeField]
	private Transform liftableObjectsParent;

	[SerializeField]
	private GameObject ui_electricityParent;

	[SerializeField]
	private GameObject ui_errorText;

	[SerializeField]
	private GameObject ui_readyText;

	public EventReference er_moveXZ;

	public EventReference er_moveY;

	public EventReference er_pickup;

	public EventReference er_drop;

	public EventReference er_containerHitFloor;

	[SerializeField]
	private Vector2Int clawSlotIndex;

	[SerializeField]
	private bool clawMovingXZ;

	[SerializeField]
	private bool clawMovingY;

	[SerializeField]
	private ClawChallengeLiftableObject objectBeingLifted;

	[SerializeField]
	private List<ClawChallengeLiftableObject> liftableObjects;

	private Queue<Action> actionQueue = new Queue<Action>();

	private Raft_Network network;

	[HideInInspector]
	public bool hasElectricity;

	private bool hasError;

	private InteractableButton_Network_Forwarder buttonForwarder;

	public Vector3 ClawTopPosition => new Vector3(clawTransform.localPosition.x, startLocalYPosition, clawTransform.localPosition.z);

	public Vector3 ClawBottomPosition => ClawTopPosition - clawArea.transform.up * clawGoDownDistance;

	public Vector3 ClawLiftPosition => ClawBottomPosition + clawArea.transform.up * clawLiftDistance;

	public int XSlotCount
	{
		get
		{
			if (!(clawArea == null))
			{
				return Mathf.FloorToInt(clawArea.transform.localScale.x / slotSize);
			}
			return 0;
		}
	}

	public int ZSlotCount
	{
		get
		{
			if (!(clawArea == null))
			{
				return Mathf.FloorToInt(clawArea.transform.localScale.z / slotSize);
			}
			return 0;
		}
	}

	public bool ClawIsMoving
	{
		get
		{
			if (!clawMovingXZ)
			{
				return clawMovingY;
			}
			return true;
		}
	}

	public bool IsLifting => objectBeingLifted != null;

	private void Awake()
	{
		buttonForwarder = new InteractableButton_Network_Forwarder(GetComponentsInChildren<InteractableButton_Networked>(includeInactive: true));
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		AreaZone_PlayerCounter areaZone_PlayerCounter = clawArea;
		areaZone_PlayerCounter.OnPlayersInAreaChanged = (Action)Delegate.Combine(areaZone_PlayerCounter.OnPlayersInAreaChanged, new Action(OnPlayersInAreaChanged));
		Quest questByType = QuestProgressTracker.GetQuestByType(QuestType.TangaroaGeneratorRepaired);
		questByType.OnQuestComplete = (Action)Delegate.Combine(questByType.OnQuestComplete, new Action(OnGeneratorQuestComplete));
		SetElectricityState(questByType.isQuestComplete);
		SetErrorState(state: false);
	}

	private void OnValidate()
	{
		if (!Application.isPlaying && clawSlotIndex != startSlotIndex)
		{
			MoveClaw(startSlotIndex, force: true, instant: true);
		}
	}

	private void OnDrawGizmos()
	{
		if (!(clawArea != null))
		{
			return;
		}
		_ = clawArea.transform.position + new Vector3(slotSize / 2f, clawArea.transform.transform.localScale.y / 2f, slotSize / 2f);
		for (int i = 0; i < XSlotCount; i++)
		{
			for (int j = 0; j < ZSlotCount; j++)
			{
				Vector3 slotPositionFromIndex = GetSlotPositionFromIndex(i, j);
				Vector3 size = new Vector3(slotSize, 0.1f, slotSize);
				Gizmos.DrawWireCube(slotPositionFromIndex, size);
			}
		}
		Gizmos.DrawLine(clawTransform.parent.TransformPoint(ClawTopPosition), clawTransform.parent.TransformPoint(ClawBottomPosition));
		Gizmos.DrawSphere(clawTransform.parent.TransformPoint(ClawLiftPosition), 0.05f);
	}

	private void Update()
	{
		if (!Raft_Network.IsHost && actionQueue.Count > 0 && !ClawIsMoving)
		{
			actionQueue.Dequeue()?.Invoke();
		}
	}

	private void FindLiftableObjects()
	{
		liftableObjects = GetComponentsInChildren<ClawChallengeLiftableObject>(includeInactive: true).ToList();
	}

	[ContextMenu("!!!!Assign indexes to liftable object")]
	private void AssignIndexesToLiftableObjects()
	{
		if (liftableObjects.ContainsItems())
		{
			for (int i = 0; i < liftableObjects.Count; i++)
			{
				liftableObjects[i].uniqueIndex = i;
			}
		}
		else
		{
			Debug.LogError("Could not assign indexes, liftableObjects did not contain anything", gameObject);
		}
	}

	private void OnPlayersInAreaChanged()
	{
		bool playersIsInArea = clawArea.PlayersIsInArea;
		SetErrorState(playersIsInArea);
	}

	private void OnGeneratorQuestComplete()
	{
		SetElectricityState(state: true);
	}

	public RGD_LandmarkItem_ClawChallenge GetRGDSave()
	{
		RGD_LandmarkItem_ClawChallenge rGD_LandmarkItem_ClawChallenge = new RGD_LandmarkItem_ClawChallenge(GetComponent<LandmarkItem>());
		if (IsLifting)
		{
			rGD_LandmarkItem_ClawChallenge.liftedUniqueIndex = objectBeingLifted.uniqueIndex;
		}
		else
		{
			rGD_LandmarkItem_ClawChallenge.liftedUniqueIndex = -1;
		}
		rGD_LandmarkItem_ClawChallenge.clawSlotIndexX = clawSlotIndex.x;
		rGD_LandmarkItem_ClawChallenge.clawSlotIndexZ = clawSlotIndex.y;
		List<RGD_ClawChallengeLiftableObject> list = new List<RGD_ClawChallengeLiftableObject>();
		for (int i = 0; i < liftableObjects.Count; i++)
		{
			ClawChallengeLiftableObject clawChallengeLiftableObject = liftableObjects[i];
			if (!(clawChallengeLiftableObject == null) && (!IsLifting || clawChallengeLiftableObject.uniqueIndex != objectBeingLifted.uniqueIndex))
			{
				RGD_ClawChallengeLiftableObject item = new RGD_ClawChallengeLiftableObject(clawChallengeLiftableObject.uniqueIndex, clawChallengeLiftableObject.transform.localPosition);
				list.Add(item);
			}
		}
		rGD_LandmarkItem_ClawChallenge.rgdLiftableObjects = list.ToArray();
		return rGD_LandmarkItem_ClawChallenge;
	}

	public void RestoreRGDSave(RGD_LandmarkItem_ClawChallenge rgd)
	{
		if (rgd.liftedUniqueIndex >= 0)
		{
			LiftObject(rgd.liftedUniqueIndex);
			MoveTransformToNewLocalPos(clawTransform, ClawLiftPosition);
		}
		MoveClaw(new Vector2Int(rgd.clawSlotIndexX, rgd.clawSlotIndexZ), force: true, instant: true);
		if (rgd.rgdLiftableObjects.ContainsItems())
		{
			RGD_ClawChallengeLiftableObject[] rgdLiftableObjects = rgd.rgdLiftableObjects;
			foreach (RGD_ClawChallengeLiftableObject rgdLiftable in rgdLiftableObjects)
			{
				ClawChallengeLiftableObject clawChallengeLiftableObject = liftableObjects.Find((ClawChallengeLiftableObject o) => o.uniqueIndex == rgdLiftable.uniqueIndex);
				if (clawChallengeLiftableObject != null)
				{
					clawChallengeLiftableObject.transform.SetParentSafe(liftableObjectsParent);
					clawChallengeLiftableObject.transform.localPosition = rgdLiftable.LocalPosition;
				}
			}
		}
		SetElectricityState(QuestProgressTracker.GetQuestByType(QuestType.TangaroaGeneratorRepaired).isQuestComplete);
		SetErrorState(state: false);
	}

	private bool ButtonPress(int buttonIndex)
	{
		if (buttonIndex < 0)
		{
			return false;
		}
		if (Raft_Network.IsHost && !hasElectricity)
		{
			return false;
		}
		return buttonIndex switch
		{
			0 => MoveClaw(clawSlotIndex + Vector2Int.up, force: false, instant: false), 
			1 => MoveClaw(clawSlotIndex + Vector2Int.right, force: false, instant: false), 
			2 => MoveClaw(clawSlotIndex + Vector2Int.down, force: false, instant: false), 
			3 => MoveClaw(clawSlotIndex + Vector2Int.left, force: false, instant: false), 
			4 => UseClaw(), 
			_ => false, 
		};
	}

	private bool UseClaw()
	{
		if (Raft_Network.IsHost)
		{
			if (hasError)
			{
				return false;
			}
			if (ClawIsMoving)
			{
				return false;
			}
		}
		StartCoroutine(UseClawCoroutine());
		return true;
	}

	private IEnumerator UseClawCoroutine()
	{
		if (!IsLifting)
		{
			ClawChallengeLiftableObject objectToLift = GetObjectInSlot(clawSlotIndex.x, clawSlotIndex.y);
			if (objectToLift != null)
			{
				clawMovingY = true;
				RuntimeManager.PlayOneShot(er_moveY, clawTransform.position);
				yield return StartCoroutine(MoveTransformToNewLocalPosCoroutine(clawTransform, ClawBottomPosition, clawMoveYTime));
				LiftObject(objectToLift.uniqueIndex);
				RuntimeManager.PlayOneShot(er_moveY, clawTransform.position);
				yield return StartCoroutine(MoveTransformToNewLocalPosCoroutine(clawTransform, ClawLiftPosition, clawMoveYTime));
				clawMovingY = false;
			}
			else
			{
				clawMovingY = true;
				RuntimeManager.PlayOneShot(er_moveY, clawTransform.position);
				yield return StartCoroutine(MoveTransformToNewLocalPosCoroutine(clawTransform, ClawBottomPosition, clawMoveYTime));
				yield return StartCoroutine(MoveTransformToNewLocalPosCoroutine(clawTransform, ClawTopPosition, clawMoveYTime));
				clawMovingY = false;
			}
		}
		else
		{
			DropObject(objectBeingLifted.uniqueIndex, clawSlotIndex.x, clawSlotIndex.y);
			clawMovingY = true;
			RuntimeManager.PlayOneShot(er_moveY, clawTransform.position);
			yield return StartCoroutine(MoveTransformToNewLocalPosCoroutine(clawTransform, ClawTopPosition, clawMoveYTime));
			clawMovingY = false;
		}
	}

	private bool MoveClaw(Vector2Int newSlotIndex, bool force, bool instant)
	{
		if (!force && Raft_Network.IsHost)
		{
			if (hasError)
			{
				return false;
			}
			if (ClawIsMoving)
			{
				return false;
			}
			if (!IsSlotIndexValid(newSlotIndex.x, newSlotIndex.y))
			{
				return false;
			}
			if (IsLifting && GetObjectInSlot(newSlotIndex.x, newSlotIndex.y) != null)
			{
				return false;
			}
		}
		Vector3 slotPositionFromIndex = GetSlotPositionFromIndex(newSlotIndex.x, newSlotIndex.y);
		bool flag = false;
		if (force || newSlotIndex.x - clawSlotIndex.x != 0)
		{
			Vector3 newLocalPosition = new Vector3(clawTransformHorizontal.parent.InverseTransformPoint(slotPositionFromIndex).x, clawTransformHorizontal.localPosition.y, clawTransformHorizontal.localPosition.z);
			if (!instant && gameObject.activeInHierarchy)
			{
				StartCoroutine(MoveTransformToNewLocalPosCoroutine(clawTransformHorizontal, newLocalPosition, clawMoveXZTime));
			}
			else
			{
				MoveTransformToNewLocalPos(clawTransformHorizontal, newLocalPosition);
			}
			flag = true;
		}
		if (force || newSlotIndex.y - clawSlotIndex.y != 0)
		{
			Vector3 newLocalPosition2 = new Vector3(clawTransformVertical.localPosition.x, clawTransformVertical.localPosition.y, clawTransformVertical.parent.InverseTransformPoint(slotPositionFromIndex).z);
			if (!instant && gameObject.activeInHierarchy)
			{
				StartCoroutine(MoveTransformToNewLocalPosCoroutine(clawTransformVertical, newLocalPosition2, clawMoveXZTime));
			}
			else
			{
				MoveTransformToNewLocalPos(clawTransformVertical, newLocalPosition2);
			}
			flag = true;
		}
		if (flag && !instant)
		{
			RuntimeManager.PlayOneShot(er_moveXZ, clawTransform.position);
		}
		clawSlotIndex = newSlotIndex;
		return true;
	}

	private IEnumerator MoveTransformToNewLocalPosCoroutine(Transform transform, Vector3 newLocalPosition, float time)
	{
		clawMovingXZ = true;
		float startTime = Time.time;
		Vector3 startPos = transform.localPosition;
		float normalizedTime = 0f;
		while (normalizedTime < 1f)
		{
			normalizedTime = Mathf.Clamp01((Time.time - startTime) / time);
			MoveTransformToNewLocalPos(transform, Vector3.Lerp(startPos, newLocalPosition, normalizedTime));
			yield return null;
		}
		clawMovingXZ = false;
	}

	private void MoveTransformToNewLocalPos(Transform transform, Vector3 newLocalPosition)
	{
		transform.localPosition = newLocalPosition;
	}

	private bool IsSlotIndexValid(int xSlotIndex, int zSlotIndex)
	{
		if (xSlotIndex < 0 || xSlotIndex >= XSlotCount || zSlotIndex < 0 || zSlotIndex >= ZSlotCount)
		{
			return false;
		}
		return true;
	}

	private void LiftObject(int liftObjectIndex)
	{
		ClawChallengeLiftableObject clawChallengeLiftableObject = liftableObjects.Find((ClawChallengeLiftableObject o) => o.uniqueIndex == liftObjectIndex);
		if (clawChallengeLiftableObject != null)
		{
			objectBeingLifted = clawChallengeLiftableObject;
			objectBeingLifted.transform.SetParentSafe(liftCrateParent);
			objectBeingLifted.transform.localPosition = Vector3.zero;
			RuntimeManager.PlayOneShot(er_pickup, clawTransform.position);
		}
	}

	private void DropObject(int liftObjectIndex, int xSlotIndex, int zSlotIndex)
	{
		ClawChallengeLiftableObject clawChallengeLiftableObject = liftableObjects.Find((ClawChallengeLiftableObject o) => o.uniqueIndex == liftObjectIndex);
		if (clawChallengeLiftableObject != null)
		{
			Vector3 slotPositionFromIndex = GetSlotPositionFromIndex(xSlotIndex, zSlotIndex);
			Vector3 vector = liftableObjectsParent.InverseTransformPoint(slotPositionFromIndex);
			Vector3 newLocalPosition = new Vector3(vector.x, clawChallengeLiftableObject.startLocalYPosition, vector.z);
			float num = 0.25f;
			clawChallengeLiftableObject.transform.SetParentSafe(liftableObjectsParent);
			StartCoroutine(MoveTransformToNewLocalPosCoroutine(clawChallengeLiftableObject.transform, newLocalPosition, num));
			clawChallengeLiftableObject = (objectBeingLifted = null);
			RuntimeManager.PlayOneShot(er_drop, clawTransform.position);
			StartCoroutine(PlayBoxHitFloorSound(slotPositionFromIndex, num));
		}
	}

	private IEnumerator PlayBoxHitFloorSound(Vector3 position, float delay)
	{
		yield return new WaitForSeconds(delay);
		RuntimeManager.PlayOneShot(er_containerHitFloor, position);
		SimpleMonoBehaviourSingleton<HapticManager>.Instance?.PlayOneShot(HapticEvent.ContainerDropped);
	}

	private Vector3 GetSlotPositionFromIndex(int xSlotIndex, int zSlotIndex)
	{
		if (!IsSlotIndexValid(xSlotIndex, zSlotIndex))
		{
			return Vector3.zero;
		}
		Vector3 right = clawArea.transform.right;
		Vector3 forward = clawArea.transform.forward;
		Vector3 vector = clawArea.transform.position + right * (slotSize / 2f) + forward * (slotSize / 2f);
		Vector3 vector2 = right * ((float)xSlotIndex * slotSize) + forward * ((float)zSlotIndex * slotSize);
		return vector + vector2;
	}

	private ClawChallengeLiftableObject GetObjectInSlot(int xSlotIndex, int zSlotIndex)
	{
		Collider[] array = Physics.OverlapSphere(GetSlotPositionFromIndex(xSlotIndex, zSlotIndex), slotSize / 3f, LayerMasks.MASK_Obstruction);
		foreach (Collider collider in array)
		{
			if (!(collider == null))
			{
				ClawChallengeLiftableObject componentInParent = collider.GetComponentInParent<ClawChallengeLiftableObject>();
				if (componentInParent != null)
				{
					return componentInParent;
				}
			}
		}
		return null;
	}

	private void SetErrorState(bool state)
	{
		hasError = state;
		ui_errorText.SetActiveSafe(state);
		ui_readyText.SetActiveSafe(!state);
	}

	private void SetElectricityState(bool state)
	{
		hasElectricity = state;
		ui_electricityParent.SetActiveSafe(state);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		Messages type = msg.Type;
		if (type == Messages.InteractableButtonPress || type == Messages.InteractableButtonPress_Int)
		{
			Message_InteractableButtonPress message_InteractableButtonPress = msg as Message_InteractableButtonPress;
			buttonForwarder.HandleMessage(message_InteractableButtonPress);
			if (message_InteractableButtonPress.success)
			{
				message_InteractableButtonPress.success = ButtonPress(message_InteractableButtonPress.buttonIndex);
			}
			return true;
		}
		return base.Deserialize(msg, remoteID);
	}
}
