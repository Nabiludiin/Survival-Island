using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Elevator : MonoBehaviour_ID_Network
{
	public bool isFunctioning = true;

	[SerializeField]
	private float timePerFloor = 1f;

	[SerializeField]
	private Transform transformToMove;

	[SerializeField]
	private Animator insideDoor;

	[SerializeField]
	private ElevatorFloor[] floors;

	[SerializeField]
	private int currentFloorIndex;

	[SerializeField]
	private bool moving;

	[SerializeField]
	private StudioEventEmitterSustain emitter_running;

	[SerializeField]
	private StudioEventEmitter emitter_doorOpen;

	[SerializeField]
	private StudioEventEmitter emitter_doorClose;

	private ElevatorFloorDisplay[] floorDisplays;

	private Coroutine moveCoroutine;

	private InteractableButton_Network_Forwarder buttonForwarder;

	public int CurrentFloor => currentFloorIndex;

	public int FloorCount
	{
		get
		{
			if (!floors.ContainsItems())
			{
				return 0;
			}
			return floors.Length;
		}
	}

	private void OnEnable()
	{
		OpenDoorAtCurrentFloor(playSound: false);
		DisplayCurrentFloorNumber();
	}

	private void Awake()
	{
		buttonForwarder = new InteractableButton_Network_Forwarder(GetComponentsInChildren<InteractableButton_Networked>(includeInactive: true));
	}

	public void RestoreRGD(RGD_Elevator rgd)
	{
		if (!(rgd == null) && rgd.CurrentFloor >= 0)
		{
			CloseAllDoors(playSound: false);
			TeleportElevatorToFloor(rgd.CurrentFloor);
			OpenDoorAtCurrentFloor(playSound: false);
			DisplayCurrentFloorNumber();
		}
	}

	private bool ButtonPress(int buttonIndex)
	{
		if (Raft_Network.IsHost)
		{
			if (!isFunctioning)
			{
				return false;
			}
			if (moveCoroutine != null)
			{
				return false;
			}
			if (currentFloorIndex == buttonIndex)
			{
				return false;
			}
			if (moving)
			{
				return false;
			}
			if (!IsFloorIndexValid(buttonIndex))
			{
				return false;
			}
		}
		else if (moveCoroutine != null)
		{
			StopCoroutine(moveCoroutine);
		}
		moveCoroutine = StartCoroutine(MoveToFloorCoroutine(buttonIndex, instant: false));
		return true;
	}

	private void CloseAllDoors(bool playSound)
	{
		if (floors.ContainsItems())
		{
			ElevatorFloor[] array = floors;
			foreach (ElevatorFloor elevatorFloor in array)
			{
				if (elevatorFloor != null)
				{
					elevatorFloor.SetDoorState(state: false);
				}
			}
		}
		if (insideDoor != null)
		{
			insideDoor.SetBool("Open", value: false);
		}
		if (playSound)
		{
			emitter_doorClose?.PlaySafe();
		}
	}

	private IEnumerator MoveToFloorCoroutine(int floorIndex, bool instant)
	{
		ElevatorFloor currentFloor = floors[currentFloorIndex];
		ElevatorFloor elevatorFloor = floors[floorIndex];
		CloseAllDoors(playSound: true);
		moving = true;
		emitter_running?.Play();
		bool goingUpwards = elevatorFloor.displayFloorNumber > currentFloor.displayFloorNumber;
		int numberOfFloorsToMove = Mathf.Abs(elevatorFloor.displayFloorNumber - currentFloor.displayFloorNumber);
		for (int i = 1; i < numberOfFloorsToMove + 1; i++)
		{
			if (!instant)
			{
				yield return new WaitForSeconds(timePerFloor);
			}
			DisplayFloorNumber(currentFloor.displayFloorNumber + i * (goingUpwards ? 1 : (-1)));
		}
		TeleportPlayersToFloor(currentFloorIndex, floorIndex);
		TeleportItemsToFloor(currentFloorIndex, floorIndex);
		TeleportCarriables(currentFloorIndex, floorIndex);
		TeleportElevatorToFloor(floorIndex);
		moveCoroutine = null;
		if (!instant)
		{
			yield return new WaitForSeconds(timePerFloor / 2f);
		}
		OpenDoorAtCurrentFloor(playSound: true);
		moving = false;
		emitter_running?.Stop();
	}

	private Vector3 GetLocalFloorPosition(int floorIndex)
	{
		return transform.InverseTransformPoint(GetWorldFloorPosition(floorIndex));
	}

	private Vector3 GetWorldFloorPosition(int floorIndex)
	{
		if (IsFloorIndexValid(floorIndex))
		{
			return floors[floorIndex].teleportPoint.position;
		}
		return Vector3.zero;
	}

	private bool IsFloorIndexValid(int floorIndex)
	{
		if (floors == null)
		{
			return false;
		}
		if (floorIndex >= 0)
		{
			return floorIndex < floors.Length;
		}
		return false;
	}

	private void DisplayCurrentFloorNumber()
	{
		if (IsFloorIndexValid(currentFloorIndex))
		{
			ElevatorFloor elevatorFloor = floors[currentFloorIndex];
			if (elevatorFloor != null)
			{
				DisplayFloorNumber(elevatorFloor.displayFloorNumber);
			}
		}
	}

	private void DisplayFloorNumber(int displayNumber)
	{
		if (floorDisplays == null)
		{
			floorDisplays = GetComponentsInChildren<ElevatorFloorDisplay>(includeInactive: true);
		}
		ElevatorFloorDisplay[] array = floorDisplays;
		foreach (ElevatorFloorDisplay elevatorFloorDisplay in array)
		{
			if (elevatorFloorDisplay != null)
			{
				elevatorFloorDisplay.SetFloorIndex(displayNumber);
			}
		}
	}

	private void TeleportElevatorToFloor(int floorIndex)
	{
		Vector3 localFloorPosition = GetLocalFloorPosition(floorIndex);
		transformToMove.localPosition = localFloorPosition;
		currentFloorIndex = floorIndex;
	}

	private void TeleportPlayersToFloor(int startFloorIndex, int endFloorIndex)
	{
		Vector3 worldFloorPosition = GetWorldFloorPosition(startFloorIndex);
		Vector3 vector = GetWorldFloorPosition(endFloorIndex) - worldFloorPosition;
		List<Network_Player> list = new List<Network_Player>();
		foreach (Collider overlap in floors[startFloorIndex].gizmoBox.GetOverlaps(includeInactiveObjects: true, LayerMasks.MASK_Players, 1f))
		{
			Network_Player componentInParent = overlap.GetComponentInParent<Network_Player>();
			if (componentInParent != null && !list.Contains(componentInParent) && componentInParent.transform.parent == null)
			{
				componentInParent.transform.position += vector;
				componentInParent.PersonController.SetNetworkPosition(componentInParent.transform.position);
				list.Add(componentInParent);
			}
		}
	}

	private void TeleportItemsToFloor(int startFloorIndex, int endFloorIndex)
	{
		Vector3 worldFloorPosition = GetWorldFloorPosition(startFloorIndex);
		Vector3 vector = GetWorldFloorPosition(endFloorIndex) - worldFloorPosition;
		Debug.DrawRay(worldFloorPosition, vector, Color.magenta, 20f);
		foreach (Collider overlap in floors[startFloorIndex].gizmoBox.GetOverlaps(includeInactiveObjects: true, LayerMasks.MASK_DroppedItem, 1f))
		{
			YieldHandler component = overlap.GetComponent<YieldHandler>();
			if (component != null)
			{
				component.transform.position += vector;
			}
		}
	}

	private void TeleportCarriables(int startFloorIndex, int endFloorIndex)
	{
		Vector3 worldFloorPosition = GetWorldFloorPosition(startFloorIndex);
		Vector3 vector = GetWorldFloorPosition(endFloorIndex) - worldFloorPosition;
		Debug.DrawRay(worldFloorPosition, vector, Color.magenta, 20f);
		foreach (Collider overlap in floors[startFloorIndex].gizmoBox.GetOverlaps(includeInactiveObjects: true, LayerMasks.MASK_RaycastInteractable, 1f))
		{
			Carry component = overlap.GetComponent<Carry>();
			if (component != null && !component.IsBeingCarried)
			{
				component.transform.position += vector;
			}
		}
	}

	private void OpenDoorAtCurrentFloor(bool playSound)
	{
		floors[CurrentFloor].SetDoorState(state: true);
		if (insideDoor != null)
		{
			insideDoor.SetBool("Open", value: true);
		}
		if (playSound)
		{
			emitter_doorOpen?.PlaySafe();
		}
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
