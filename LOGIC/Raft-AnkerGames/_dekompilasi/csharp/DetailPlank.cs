using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetailPlank : UsableTool
{
	private float _cameraOffset;

	[SerializeField]
	private Block plankPrefab;

	[SerializeField]
	private DetailPlankLength lengthSetter;

	[SerializeField]
	private float rotationDegrees = 1f;

	[SerializeField]
	private float scrollIndexLength = 0.05f;

	[SerializeField]
	private Interval_Float scrollLengthInterval;

	[SerializeField]
	private Interval_Float plankLengthInterval;

	private Transform lockedPivot;

	private Vector3 localPointA = Vector3.zero;

	private Vector3 worldAimPoint = Vector3.zero;

	private float rotation;

	[SerializeField]
	private InputActionReference inputFire;

	[SerializeField]
	private InputActionReference inputCancel;

	[SerializeField]
	private InputActionReference inputRotate;

	[SerializeField]
	private InputActionReference inputDeltaX;

	public bool IsPlacingPointB => localPointA != Vector3.zero;

	private float CameraOffset
	{
		get
		{
			return _cameraOffset;
		}
		set
		{
			_cameraOffset = Mathf.Clamp(value, scrollLengthInterval.minValue, scrollLengthInterval.maxValue);
		}
	}

	protected override void Start()
	{
		base.Start();
		lockedPivot = SingletonGeneric<GameManager>.Singleton.lockedPivot;
		lengthSetter.transform.SetParentSafe(lockedPivot);
	}

	private void OnDisable()
	{
		if (playerNetwork != null && playerNetwork.IsLocalPlayer)
		{
			lengthSetter.gameObject.SetActiveSafe(value: false);
		}
	}

	protected override bool Update()
	{
		if (base.Update() && playerNetwork != null && !playerNetwork.BedComponent.Sleeping)
		{
			if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				if (axis > 0f)
				{
					CameraOffset += scrollIndexLength;
				}
				else if (axis < 0f)
				{
					CameraOffset -= scrollIndexLength;
				}
			}
			bool flag = Helper.HitAtCursor(out var hit, Player.UseDistance * 2f, LayerMasks.MASK_Block);
			if (flag)
			{
				if (IsPlacingPointB)
				{
					worldAimPoint = ClampPointToMaxLength(hit.point);
				}
				else
				{
					worldAimPoint = hit.point;
				}
				CameraOffset = Vector3.Distance(Helper.MainCamera.transform.position, worldAimPoint);
			}
			else
			{
				Vector3 forward = Helper.MainCamera.transform.forward;
				Vector3 worldPoint = Helper.MainCamera.transform.position + forward * _cameraOffset;
				if (IsPlacingPointB)
				{
					worldAimPoint = ClampPointToMaxLength(worldPoint);
				}
				else
				{
					worldAimPoint = worldPoint;
				}
			}
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputFire, "LMB") && (IsPlacingPointB | flag))
			{
				AddPoint(lockedPivot.InverseTransformPoint(worldAimPoint));
			}
			if (!IsPlacingPointB)
			{
				if (isActiveAndEnabled)
				{
					lengthSetter.gameObject.SetActiveSafe(flag);
				}
				if (flag)
				{
					lengthSetter.transform.position = worldAimPoint;
					lengthSetter.SetLength(0.1f);
					if (Helper.MainCamera.transform.position - lengthSetter.transform.position != Vector3.zero)
					{
						lengthSetter.transform.rotation = Quaternion.LookRotation(Helper.MainCamera.transform.position - lengthSetter.transform.position, lockedPivot.up);
					}
				}
			}
			else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputCancel, "RMB"))
			{
				Abort();
			}
			else
			{
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputRotate, "Rotate"))
				{
					playerNetwork.PlayerScript.SetMouseLookScripts(canLook: false);
				}
				else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(inputRotate, "Rotate"))
				{
					playerNetwork.PlayerScript.SetMouseLookScripts(canLook: true);
				}
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputRotate, "Rotate"))
				{
					float xAxis = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.GetXAxis(inputDeltaX, "Mouse X");
					rotation = Mathf.Clamp(rotation + xAxis * rotationDegrees, -180f, 180f);
				}
				Vector3 vector = lockedPivot.TransformPoint(localPointA);
				Vector3 forward2 = worldAimPoint - vector;
				lengthSetter.transform.position = vector;
				lengthSetter.SetLength(forward2.magnitude);
				lengthSetter.transform.rotation = Quaternion.LookRotation(forward2, lockedPivot.up);
				lengthSetter.transform.Rotate(Vector3.forward, rotation, Space.Self);
				lengthSetter.gameObject.SetActiveSafe(value: true);
			}
			return true;
		}
		return false;
	}

	protected override void OnToolSelect()
	{
		base.OnToolSelect();
		if (playerNetwork.IsLocalPlayer)
		{
			localPointA = Vector3.zero;
			lengthSetter.gameObject.SetActiveSafe(value: true);
			playerNetwork.PersonController.rotateWithBuoyancy = true;
		}
	}

	protected override void OnToolDeSelect()
	{
		base.OnToolDeSelect();
		if (playerNetwork.IsLocalPlayer)
		{
			Abort();
			lengthSetter.gameObject.SetActiveSafe(value: false);
			playerNetwork.PersonController.rotateWithBuoyancy = false;
		}
	}

	private void Abort()
	{
		localPointA = Vector3.zero;
		PlayerItemManager.IsBusy = false;
		playerNetwork.PlayerScript.SetMouseLookScripts(canLook: true);
	}

	private void AddPoint(Vector3 localPosition)
	{
		if (localPointA == Vector3.zero)
		{
			localPointA = localPosition;
			PlayerItemManager.IsBusy = true;
		}
		else
		{
			Vector3 localPosition2 = localPointA;
			Abort();
			CreatePlank(localPosition2, localPosition);
		}
	}

	private Vector3 ClampPointToMaxLength(Vector3 worldPoint)
	{
		Vector3 vector = lockedPivot.TransformPoint(localPointA);
		Vector3 vector2 = Vector3.ClampMagnitude(worldPoint - vector, plankLengthInterval.maxValue);
		if (vector2.magnitude < plankLengthInterval.minValue)
		{
			vector2 = vector2.normalized * plankLengthInterval.minValue;
		}
		return vector + vector2;
	}

	private void CreatePlank(Vector3 localPosition, Vector3 secondLocalPosition)
	{
		float magnitude = (secondLocalPosition - localPosition).magnitude;
		Message_BlockCreator_PlaceBlock_DetailPlank message_BlockCreator_PlaceBlock_DetailPlank = new Message_BlockCreator_PlaceBlock_DetailPlank(Messages.BlockCreator_PlaceBlockDetailPlank, playerNetwork.BlockCreator, plankPrefab.buildableItem.UniqueIndex, 0u, 0u, 0u, localPosition, lengthSetter.transform.localRotation.eulerAngles, playerNetwork.Inventory.hotbar.GetSelectedSlotIndex(), DPS.Default, magnitude);
		if (Raft_Network.IsHost)
		{
			message_BlockCreator_PlaceBlock_DetailPlank.blockObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
			message_BlockCreator_PlaceBlock_DetailPlank.networkedObjectIndex = ((plankPrefab.networkedBehaviour != null) ? SaveAndLoad.GetUniqueObjectIndex() : 0u);
			message_BlockCreator_PlaceBlock_DetailPlank.networkedBehaviourIndex = ((plankPrefab.networkedBehaviour != null) ? NetworkUpdateManager.GetUniqueBehaviourIndex() : 0u);
			playerNetwork.Network.RPC(message_BlockCreator_PlaceBlock_DetailPlank, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			playerNetwork.BlockCreator.CreateDetailPlank(plankPrefab.buildableItem, message_BlockCreator_PlaceBlock_DetailPlank.LocalPosition, message_BlockCreator_PlaceBlock_DetailPlank.LocalEuler, message_BlockCreator_PlaceBlock_DetailPlank.dpsType, message_BlockCreator_PlaceBlock_DetailPlank.hotSlotIndex, replicating: false, message_BlockCreator_PlaceBlock_DetailPlank.blockObjectIndex, message_BlockCreator_PlaceBlock_DetailPlank.networkedObjectIndex, message_BlockCreator_PlaceBlock_DetailPlank.networkedBehaviourIndex, message_BlockCreator_PlaceBlock_DetailPlank.length);
		}
		else
		{
			playerNetwork.Network.SendP2P(playerNetwork.Network.HostID, message_BlockCreator_PlaceBlock_DetailPlank, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}
}
