using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using I2.Loc;
using Steamworks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Helper
{
	private static Transform lockedPivot;

	private static CursorLockMode lockMode;

	private static Network_Player localPlayer;

	private static Raft_Network network;

	private static PickupObjectManager pickupObjectManager;

	private static Camera mainCamera;

	private static Camera localFovCamera;

	private static CanvasHelper canvas;

	public static bool CursorVisible;

	public static Camera MainCamera
	{
		get
		{
			if (mainCamera == null)
			{
				if (network == null)
				{
					network = ComponentManager<Raft_Network>.Value;
				}
				if (network == null)
				{
					return Camera.main;
				}
				Network_Player network_Player = network.GetLocalPlayer();
				if (network_Player == null)
				{
					return Camera.main;
				}
				mainCamera = network_Player.Camera;
				return mainCamera;
			}
			return mainCamera;
		}
	}

	private static Transform LockedPivot
	{
		get
		{
			if (lockedPivot == null)
			{
				lockedPivot = SingletonGeneric<GameManager>.Singleton.lockedPivot;
			}
			return lockedPivot;
		}
	}

	public static void DrawArrowPath(Vector3 pointA, Vector3 pointB, Color color, float arrowLength = 1f)
	{
		float magnitude = (pointB - pointA).magnitude;
		Mathf.FloorToInt(magnitude / arrowLength);
		_ = magnitude % arrowLength;
	}

	public static void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		pickupObjectManager = ComponentManager<PickupObjectManager>.Value;
		Raft_Network.OnWorldReceivedLate = (Action)Delegate.Remove(Raft_Network.OnWorldReceivedLate, new Action(OnWorldRecievedLate));
		Raft_Network.OnWorldReceivedLate = (Action)Delegate.Combine(Raft_Network.OnWorldReceivedLate, new Action(OnWorldRecievedLate));
		canvas = ComponentManager<CanvasHelper>.Value;
		CursorVisible = Cursor.visible;
	}

	public static void OnWorldRecievedLate()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (localPlayer != null && localPlayer.Camera != null)
		{
			localFovCamera = localPlayer.Camera.transform.Find("HandCamera").GetComponent<Camera>();
		}
	}

	public static void OnLeaveGame()
	{
		localPlayer = null;
		network = null;
		pickupObjectManager = null;
		mainCamera = null;
		canvas = null;
	}

	public static void Update()
	{
		Cursor.lockState = lockMode;
	}

	public static T GetReflectionValue<T>(UnityEngine.Object obj, string fieldName)
	{
		return (T)obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(obj);
	}

	public static void SetReflectionValue(UnityEngine.Object orgObj, UnityEngine.Object targetObj, string fieldName)
	{
		object value = orgObj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(orgObj);
		targetObj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(targetObj, value);
	}

	public static bool HitAtCursor(out RaycastHit hit, float rayDistance)
	{
		if (MainCamera == null)
		{
			hit = default;
			return false;
		}
		return Physics.Raycast(MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)), out hit, rayDistance, LayerMasks.MASK_IgnorePlayer);
	}

	public static bool HitAtCursor(out RaycastHit hit, float rayDistance, LayerMask mask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
	{
		if (MainCamera == null)
		{
			hit = default;
			return false;
		}
		return Physics.Raycast(MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)), out hit, rayDistance, mask, queryTriggerInteraction);
	}

	public static bool HitAllAtCursor(out RaycastHit[] allHits, float rayDistance, LayerMask mask)
	{
		if (MainCamera == null)
		{
			allHits = null;
			return false;
		}
		Ray ray = MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
		allHits = Physics.RaycastAll(ray, rayDistance, mask);
		if (allHits.Length != 0)
		{
			return true;
		}
		return false;
	}

	public static bool SphereHitAtCursor(out RaycastHit hit, float rayDistance, float radius, LayerMask mask)
	{
		if (MainCamera == null)
		{
			hit = default;
			return false;
		}
		return Physics.SphereCast(MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)), radius, out hit, rayDistance, mask);
	}

	public static bool SphereHitAllAtCursor(out RaycastHit[] allHits, float rayDistance, float radius, LayerMask mask)
	{
		if (MainCamera == null)
		{
			allHits = null;
			return false;
		}
		Ray ray = MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
		allHits = Physics.SphereCastAll(ray, radius, (int)mask);
		if (allHits.Length != 0)
		{
			return true;
		}
		return false;
	}

	public static Collider[] RefineCollidersWithFOV(Collider[] objectsInArea, Vector3 orgin, LayerMask p_obstructionLayer)
	{
		List<Collider> list = new List<Collider>();
		for (uint num = 0u; num < objectsInArea.Length; num++)
		{
			if (!Physics.Linecast(orgin, objectsInArea[num].transform.position, p_obstructionLayer.value))
			{
				list.Add(objectsInArea[num]);
			}
		}
		return list.ToArray();
	}

	public static bool HitAtCursor(out RaycastHit hit, float rayDistance, LayerMask mask, bool abortOnItemHover)
	{
		if (abortOnItemHover && Pickup.isHoveringOverPickup)
		{
			hit = default;
			return false;
		}
		if (MainCamera == null)
		{
			hit = default;
			return false;
		}
		return Physics.Raycast(MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)), out hit, rayDistance, mask);
	}

	public static RaycastInteractable FindInteractable(float interactDistance, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Collide)
	{
		Ray ray = MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
		if (Physics.Raycast(ray, out var hitInfo, interactDistance, LayerMasks.MASK_RaycastInteractable, queryTriggerInteraction))
		{
			if (hitInfo.collider != null)
			{
				RaycastInteractable raycastInteractable = hitInfo.collider.GetComponent<RaycastInteractable>();
				if (raycastInteractable == null)
				{
					RaycastInteractable_Redirect component = hitInfo.collider.GetComponent<RaycastInteractable_Redirect>();
					if (component != null)
					{
						raycastInteractable = component.RaycastInteractable;
					}
				}
				return raycastInteractable;
			}
		}
		else if (Physics.Raycast(ray.origin + ray.direction.normalized * interactDistance, -ray.direction, out hitInfo, interactDistance, LayerMasks.MASK_RaycastInteractable, queryTriggerInteraction) && hitInfo.collider != null)
		{
			RaycastInteractable raycastInteractable2 = hitInfo.collider.GetComponent<RaycastInteractable>();
			if (raycastInteractable2 == null)
			{
				RaycastInteractable_Redirect component2 = hitInfo.collider.GetComponent<RaycastInteractable_Redirect>();
				if (component2 != null)
				{
					raycastInteractable2 = component2.RaycastInteractable;
				}
			}
			return raycastInteractable2;
		}
		return null;
	}

	public static void SetCursorVisible(bool state)
	{
		CursorVisible = state;
		if (canvas != null && canvas.centerAim != null)
		{
			canvas.centerAim.gameObject.SetActive(!state);
		}
	}

	public static void SetCursorLockState(CursorLockMode mode)
	{
		Cursor.lockState = mode;
		lockMode = mode;
	}

	public static void SetCursorVisibleAndLockState(bool state, CursorLockMode mode)
	{
		SetCursorVisible(state);
		SetCursorLockState(mode);
	}

	public static Vector3 GetColliderExtents(Collider col)
	{
		if (col == null)
		{
			return Vector3.zero;
		}
		if (col.enabled)
		{
			if (!col.gameObject.activeInHierarchy)
			{
				col.gameObject.SetActive(value: true);
				Vector3 extents = col.bounds.extents;
				col.gameObject.SetActive(value: false);
				return extents;
			}
			return col.bounds.extents;
		}
		col.enabled = true;
		Vector3 extents2 = col.bounds.extents;
		col.enabled = false;
		return extents2;
	}

	public static Vector3 GetColliderSize(BoxCollider col)
	{
		return Vector3.Scale(col.size, col.transform.lossyScale);
	}

	public static Vector3 GetColliderCenter(Collider col)
	{
		if (col == null)
		{
			return Vector3.zero;
		}
		if (col.enabled)
		{
			if (!col.gameObject.activeInHierarchy)
			{
				col.gameObject.SetActive(value: true);
				Vector3 center = col.bounds.center;
				col.gameObject.SetActive(value: false);
				return center;
			}
			return col.bounds.center;
		}
		col.enabled = true;
		Vector3 center2 = col.bounds.center;
		col.enabled = false;
		return center2;
	}

	public static Collider[] GetCollisions(BoxCollider col, LayerMask mask, float extentsMultiplier = 1f)
	{
		return Physics.OverlapBox(col.transform.TransformPoint(col.center), GetColliderSize(col) * 0.5f * extentsMultiplier, col.transform.rotation, mask);
	}

	public static Collider[] GetCollisions(BoxCollider col, LayerMask mask, BoxCollider[] excludeColliders, float extentsMultiplier = 1f, QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal)
	{
		List<Collider> list = Physics.OverlapBox(col.transform.TransformPoint(col.center), GetColliderSize(col) * 0.5f * extentsMultiplier, col.transform.rotation, mask).ToList();
		for (int i = 0; i < list.Count; i++)
		{
			Collider collider = list[i];
			if (collider.transform == col.transform)
			{
				continue;
			}
			foreach (BoxCollider boxCollider in excludeColliders)
			{
				if (collider == boxCollider)
				{
					list.RemoveAt(i);
					i--;
				}
			}
		}
		return list.ToArray();
	}

	public static Collider[] GetCollisions(BoxCollider col, LayerMask mask, BoxCollider[] excludeColliders, Vector3 additionalExtents, QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal)
	{
		List<Collider> list = Physics.OverlapBox(col.transform.TransformPoint(col.center), GetColliderSize(col) * 0.5f + additionalExtents * 0.5f, col.transform.rotation, mask, triggerInteraction).ToList();
		for (int i = 0; i < list.Count; i++)
		{
			Collider collider = list[i];
			if (collider.transform == col.transform)
			{
				if (excludeColliders.Contains(collider))
				{
					list.RemoveAt(i);
					i--;
				}
				continue;
			}
			foreach (BoxCollider boxCollider in excludeColliders)
			{
				if (collider == boxCollider)
				{
					list.RemoveAt(i);
					i--;
				}
			}
		}
		return list.ToArray();
	}

	public static bool IsColliding(BoxCollider col, LayerMask mask, float extentsMultiplier = 1f, bool includeOwnCollider = true, QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal)
	{
		if (col == null)
		{
			return false;
		}
		Collider[] array = Physics.OverlapBox(GetColliderCenter(col), GetColliderSize(col) * 0.5f * extentsMultiplier, col.transform.rotation, mask, triggerInteraction);
		for (int i = 0; i < array.Length; i++)
		{
			if (!(array[i].transform == col.transform))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsColliding(BoxCollider col, LayerMask mask, BoxCollider[] excludeColliders, float extentsMultiplier = 1f, QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal)
	{
		Collider[] array = Physics.OverlapBox(GetColliderCenter(col), GetColliderSize(col) * 0.5f * extentsMultiplier, col.transform.rotation, mask, triggerInteraction);
		foreach (Collider collider in array)
		{
			if (collider.transform == col.transform)
			{
				continue;
			}
			bool flag = false;
			foreach (BoxCollider boxCollider in excludeColliders)
			{
				if (collider == boxCollider)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsColliding(BoxCollider col, LayerMask mask, BoxCollider[] excludeColliders, out Collider hitCollider, float extentsMultiplier = 1f, QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal)
	{
		Collider[] array = Physics.OverlapBox(GetColliderCenter(col), GetColliderSize(col) * 0.5f * extentsMultiplier, col.transform.rotation, mask, triggerInteraction);
		foreach (Collider collider in array)
		{
			if (collider.transform == col.transform)
			{
				continue;
			}
			bool flag = false;
			foreach (BoxCollider boxCollider in excludeColliders)
			{
				if (collider == boxCollider)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				hitCollider = collider;
				return true;
			}
		}
		hitCollider = null;
		return false;
	}

	public static bool IsPointInOABB(Vector3 point, Transform boxTransform, Vector3 centerOffset)
	{
		Vector3 vector = boxTransform.InverseTransformPoint(point);
		Vector3 vector2 = -Vector3.one / 2f + centerOffset;
		Vector3 vector3 = Vector3.one / 2f + centerOffset;
		if (vector.x <= vector3.x && vector.x >= vector2.x && vector.y <= vector3.y && vector.y >= vector2.y && vector.z <= vector3.z && vector.z >= vector2.z)
		{
			return true;
		}
		return false;
	}

	public static bool IsPointInOABB_XZOnly(Vector3 point, Transform boxTransform, Vector3 centerOffset)
	{
		Vector3 vector = boxTransform.InverseTransformPoint(point);
		Vector3 vector2 = -Vector3.one / 2f + centerOffset;
		Vector3 vector3 = Vector3.one / 2f + centerOffset;
		if (vector.x <= vector3.x && vector.x >= vector2.x && vector.z <= vector3.z && vector.z >= vector2.z)
		{
			return true;
		}
		return false;
	}

	public static bool IsPointInOABB_XZOnly(Vector3 point, Transform boxTransform, Vector3 customScale, Vector3 centerOffset)
	{
		Vector3 vector = boxTransform.InverseTransformPointUnscaled(point);
		Vector3 vector2 = -customScale / 2f + centerOffset;
		Vector3 vector3 = customScale / 2f + centerOffset;
		if (vector.x <= vector3.x && vector.x >= vector2.x && vector.z <= vector3.z && vector.z >= vector2.z)
		{
			return true;
		}
		return false;
	}

	public static bool IsPointInCollider(Vector3 point, Collider collider, float acceptableDistance = 0.05f)
	{
		return Vector3.Distance(collider.ClosestPoint(point), point) <= acceptableDistance;
	}

	public static bool IsLineIntersectingCircle(Vector3 lineStart, Vector3 lineEnd, Vector3 circleCenter, float circleRadius)
	{
		Vector3 vector = lineEnd - lineStart;
		Vector3 vector2 = Vector3.ClampMagnitude(Vector3.Project(circleCenter - lineStart, vector), vector.magnitude);
		Vector3 vector3 = lineStart + vector2;
		if (Vector3.Angle(vector3 - lineStart, vector) > 90f)
		{
			vector3 = lineStart;
		}
		return Vector3.Distance(vector3, circleCenter) <= circleRadius;
	}

	public static void DropItem(ItemInstance item, Vector3 position, Vector3 direction, bool parentedToRaft)
	{
		if (item != null)
		{
			uint dropObjectIndex = (Raft_Network.IsHost ? SaveAndLoad.GetUniqueObjectIndex() : 0u);
			Message_DropItem message_DropItem = new Message_DropItem(Messages.DropItem, pickupObjectManager, item, position, direction, dropObjectIndex, parentedToRaft);
			if (Raft_Network.IsHost)
			{
				network.RPC(message_DropItem, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				PickupObjectManager.DropItem(item, position, direction, message_DropItem.dropObjectIndex, parentedToRaft);
			}
			else
			{
				network.SendP2P(network.HostID, message_DropItem, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public static Network_Player ClosestPlayerToPoint(Vector3 point, bool includeDeadPlayers = true)
	{
		return ClosestPlayerToPoint(point, float.MaxValue, includeDeadPlayers);
	}

	public static Network_Player ClosestPlayerToPoint(Vector3 point, float maxDistanceFromPoint, bool includeDeadPlayers = true, bool ignoreYValue = false)
	{
		if (network == null || network.remoteUsers == null)
		{
			return null;
		}
		Network_Player result = null;
		float num = float.MaxValue;
		foreach (Network_Player value in network.remoteUsers.Values)
		{
			if (!(value == null) && (includeDeadPlayers || !value.PlayerScript.IsDead))
			{
				Vector3 a = point;
				Vector3 position = value.transform.position;
				if (ignoreYValue)
				{
					a.y = (position.y = 0f);
				}
				float num2 = Vector3.Distance(a, position);
				if (num2 <= num && num2 <= maxDistanceFromPoint)
				{
					result = value;
					num = num2;
				}
			}
		}
		return result;
	}

	public static Network_Player ClosestPlayerInWaterToPoint(Vector3 point, float maxDistanceFromPoint, bool includeDeadPlayers = true)
	{
		if (network == null || network.remoteUsers == null)
		{
			return null;
		}
		Network_Player result = null;
		float num = float.MaxValue;
		foreach (Network_Player value in network.remoteUsers.Values)
		{
			if (!(value == null) && (includeDeadPlayers || !value.PlayerScript.IsDead) && IsValidWaterTarget(value))
			{
				float num2 = Vector3.Distance(point, value.transform.position);
				if (num2 <= num && num2 <= maxDistanceFromPoint)
				{
					result = value;
					num = num2;
				}
			}
		}
		return result;
	}

	public static Network_Player GetRandomPlayer()
	{
		if (network == null)
		{
			return null;
		}
		return network.GetRandomPlayer();
	}

	public static List<Network_Player> PlayersWithinRange(Vector3 point, float range, bool includeDeadPlayers, bool ignoreYValue = false)
	{
		if (network == null)
		{
			return null;
		}
		List<Network_Player> list = null;
		foreach (Network_Player value in network.remoteUsers.Values)
		{
			if (value == null || (!includeDeadPlayers && value.PlayerScript.IsDead))
			{
				continue;
			}
			Vector3 a = point;
			Vector3 position = value.transform.position;
			if (ignoreYValue)
			{
				a.y = (position.y = 0f);
			}
			if (Vector3.Distance(a, position) <= range)
			{
				if (list == null)
				{
					list = new List<Network_Player>();
				}
				list.Add(value);
			}
		}
		return list;
	}

	public static bool IsAPlayerOnTheRaft()
	{
		if (network != null && network.remoteUsers != null)
		{
			foreach (Network_Player value in network.remoteUsers.Values)
			{
				if (value != null && value.transform.ParentedToRaft())
				{
					return true;
				}
			}
		}
		return false;
	}

	public static Cropplot[] FindNearbyCropplots(Vector3 centerPoint, float range, LayerMask cropplotLayer)
	{
		Collider[] array = Physics.OverlapSphere(centerPoint, range, cropplotLayer);
		List<Cropplot> list = new List<Cropplot>();
		for (int i = 0; i < array.Length; i++)
		{
			Cropplot componentInChildren = array[i].GetComponentInChildren<Cropplot>();
			if (componentInChildren != null)
			{
				list.Add(componentInChildren);
			}
		}
		return list.ToArray();
	}

	public static bool CheckForRoofAbovePoint(Vector3 origin, Vector3 upDirection, int amountOfBlocks)
	{
		bool result = false;
		if (Physics.Raycast(origin + upDirection * 1f, upDirection, out var _, 2.42f * (float)amountOfBlocks, LayerMasks.MASK_Block, QueryTriggerInteraction.Ignore))
		{
			result = true;
		}
		return result;
	}

	public static Vector3 GetRandomPointAroundOrigin(Vector3 origin, float minRadius, float maxRadius)
	{
		return origin + UnityEngine.Random.insideUnitSphere.normalized * UnityEngine.Random.Range(minRadius, maxRadius);
	}

	public static bool HasVisionToPlayer(Network_Player player, Vector3 sourcePoint, bool useFeetPosition = true, bool ignoreObstruction = false, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
	{
		if (ignoreObstruction)
		{
			return true;
		}
		Vector3 vector = (useFeetPosition ? player.FeetPosition : player.transform.position);
		float num = Vector3.Distance(sourcePoint, vector);
		return HasClearVision(sourcePoint, vector, LayerMasks.MASK_Obstruction, num - 0.5f, queryTriggerInteraction);
	}

	public static bool HasClearVision(Vector3 origin, Vector3 target, LayerMask mask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
	{
		Vector3 vector = target - origin;
		if (target == origin || vector.magnitude < 0.5f)
		{
			return true;
		}
		if (Physics.Raycast(origin, vector.normalized, out var _, vector.magnitude, mask, queryTriggerInteraction))
		{
			return false;
		}
		return true;
	}

	public static bool HasClearVision(Vector3 origin, Vector3 target, LayerMask mask, float rayLength, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
	{
		Vector3 vector = target - origin;
		if (target == origin || vector.magnitude < 0.5f)
		{
			return true;
		}
		if (rayLength < 0f)
		{
			rayLength = 0.1f;
		}
		if (Physics.Raycast(origin, vector.normalized, out var _, rayLength, mask, queryTriggerInteraction))
		{
			Debug.DrawRay(origin, vector.normalized * rayLength, Color.red, 1f);
			return false;
		}
		Debug.DrawRay(origin, vector.normalized * rayLength, Color.green, 1f);
		return true;
	}

	public static bool HasClearVision(Vector3 origin, Vector3 target, float radius, LayerMask mask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
	{
		Vector3 vector = target - origin;
		if (target == origin || vector.magnitude < 0.5f)
		{
			return true;
		}
		if (Physics.SphereCast(origin, radius, vector.normalized, out var _, vector.magnitude, mask, queryTriggerInteraction))
		{
			return false;
		}
		return true;
	}

	public static bool HasClearVisionAndWithinRange(Vector3 origin, Vector3 target, LayerMask mask, float visionRange)
	{
		float magnitude = (target - origin).magnitude;
		if (target == origin || magnitude < 0.5f)
		{
			return true;
		}
		if (magnitude > visionRange)
		{
			return false;
		}
		if (Physics.Linecast(origin, target, out var _, mask))
		{
			return false;
		}
		return true;
	}

	public static bool LocalPlayerIsWithinDistance(Vector3 position, float requiredDistance)
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (localPlayer == null)
		{
			Debug.LogError("Local player could not be found at Helper.LocalPlayerIsWithinDistance()");
			return false;
		}
		return Vector3.Distance(localPlayer.transform.position, position) <= requiredDistance;
	}

	public static bool IsAllPlayersWithinDistance(Vector3 point, float distance)
	{
		foreach (Network_Player value in network.remoteUsers.Values)
		{
			if (!WithinDistance(value.FeetPosition, point, distance))
			{
				return false;
			}
		}
		return true;
	}

	public static bool WithinDistance(Vector3 a, Vector3 b, float distance)
	{
		return Vector3.Distance(a, b) <= distance;
	}

	public static void SetLayerOfTransformAndChildren(Transform trans, string layerName)
	{
		for (int i = 0; i < trans.childCount; i++)
		{
			trans.GetChild(i).gameObject.layer = LayerMask.NameToLayer(layerName);
			SetLayerOfTransformAndChildren(trans.GetChild(i), layerName);
		}
	}

	public static float MapValue(float min, float max, float value, float newMin, float newMax)
	{
		value = Mathf.Clamp(value, min, max);
		return newMin + (value - min) * (newMax - newMin) / (max - min);
	}

	public static float NormalizeValue(float value, float min, float max)
	{
		return Mathf.Clamp((value - min) / (max - min), 0f, 1f);
	}

	public static Vector3 DampVector(Vector3 source, Vector3 target, float smoothing, float dt)
	{
		source.x = Mathf.Lerp(source.x, target.x, 1f - Mathf.Pow(smoothing, dt));
		source.y = Mathf.Lerp(source.y, target.y, 1f - Mathf.Pow(smoothing, dt));
		source.z = Mathf.Lerp(source.z, target.z, 1f - Mathf.Pow(smoothing, dt));
		return source;
	}

	public static float Damp(float source, float target, float smoothing, float dt)
	{
		return Mathf.Lerp(source, target, 1f - Mathf.Pow(smoothing, dt));
	}

	public static int BoolToInt(bool value)
	{
		if (!value)
		{
			return 0;
		}
		return 1;
	}

	public static bool IntToBool(int value)
	{
		if (value != 0)
		{
			return true;
		}
		return false;
	}

	public static float WrapAngle(float angle)
	{
		angle %= 360f;
		if (angle > 180f)
		{
			return angle - 360f;
		}
		return angle;
	}

	public static Vector3 XZDirectionFromAngle(float angle)
	{
		return new Vector3(Mathf.Cos(MathF.PI / 180f * angle), 0f, Mathf.Sin(MathF.PI / 180f * angle)).normalized;
	}

	public static Vector3 GetBezierCurveValue(Vector3 start, Vector3 end, float centerHeight, float normalizedTime)
	{
		Vector3 vector = end - start;
		Vector3 vector2 = start + vector / 2f + Vector3.up * centerHeight;
		Vector3 a = Vector3.Lerp(start, vector2, normalizedTime);
		Vector3 b = Vector3.Lerp(vector2, end, normalizedTime);
		return Vector3.Lerp(a, b, normalizedTime);
	}

	public static Vector3 GetBezierCurveValue(Vector3 start, Vector3 end, Vector3 centerOffset, float normalizedTime)
	{
		Vector3 vector = end - start;
		Vector3 vector2 = start + vector / 2f + centerOffset;
		Vector3 a = Vector3.Lerp(start, vector2, normalizedTime);
		Vector3 b = Vector3.Lerp(vector2, end, normalizedTime);
		return Vector3.Lerp(a, b, normalizedTime);
	}

	public static Vector3 WorldPointToFOVPoint(Vector3 worldPoint)
	{
		if (localFovCamera == null)
		{
			localFovCamera = localPlayer.Camera.transform.Find("HandCamera").GetComponent<Camera>();
		}
		Vector3 position = localFovCamera.WorldToScreenPoint(worldPoint);
		return localPlayer.Camera.ScreenToWorldPoint(position);
	}

	public static Vector3 WorldPointToFOVPointLocal(Vector3 worldPoint, Transform parent)
	{
		if (localPlayer == null)
		{
			return worldPoint;
		}
		if (localFovCamera == null)
		{
			localFovCamera = localPlayer.Camera.transform.Find("HandCamera").GetComponent<Camera>();
		}
		if (localFovCamera != null && localPlayer != null)
		{
			Vector3 position = localFovCamera.WorldToScreenPoint(worldPoint);
			Vector3 position2 = localPlayer.Camera.ScreenToWorldPoint(position);
			return parent.InverseTransformPoint(position2);
		}
		return Vector3.zero;
	}

	public static float UnwrapAngle(float angle)
	{
		if (angle >= 0f)
		{
			return angle;
		}
		angle = (0f - angle) % 360f;
		return 360f - angle;
	}

	public static float GetAngleBetweenObjects(Transform self, Transform other, bool resetYPosition)
	{
		Vector3 position = self.transform.position;
		Vector3 position2 = other.transform.position;
		if (resetYPosition)
		{
			position2.y = position.y;
		}
		return Vector3.Angle(self.forward, position2 - position);
	}

	public static string GetTerm(string term, bool applyParameters = false)
	{
		return LocalizationManager.GetTranslation(term, FixForRTL: true, 0, ignoreRTLnumbers: true, applyParameters);
	}

	public static bool ObjectIsOnLayer(GameObject objectToCompare, LayerMask layerToCompare)
	{
		return ((1 << objectToCompare.layer) & (int)layerToCompare) != 0;
	}

	public static bool ObjectIsOnLayerParentedToRaft(GameObject objectToCompare, LayerMask layerToCompare)
	{
		if (((1 << objectToCompare.layer) & (int)layerToCompare) != 0 && LockedPivot != null && objectToCompare.transform.IsChildOf(LockedPivot))
		{
			return true;
		}
		return false;
	}

	public static bool IsTransformParentedToRaft(Transform transform)
	{
		return transform.parent == LockedPivot;
	}

	public static Vector3 GetLocalPositionComparedToRoot(Transform root, Transform compareTransform)
	{
		Vector3 localPos = compareTransform.localPosition;
		LocalPositionComparedToRootRecursive(root, compareTransform.parent, ref localPos);
		return localPos;
	}

	public static Vector3 GetLocalEulerComparedToRoot(Transform root, Transform compareTransform)
	{
		Vector3 localEuler = compareTransform.localEulerAngles;
		LocalEuleromparedToRootRecursive(root, compareTransform.parent, ref localEuler);
		return localEuler;
	}

	public static Vector3 GetClosestPointOnNavMesh(Vector3 sourcePosition, float range)
	{
		if (NavMesh.SamplePosition(sourcePosition, out var hit, range, -1))
		{
			return hit.position;
		}
		return sourcePosition;
	}

	public static Vector3 GetClosestPointOnNavMeshInDirectionTo(Vector3 from, Vector3 to, float extraLength, float lengthIntervall, float navmeshSnapDistance)
	{
		Vector3 normalized = (to - from).normalized;
		int num = (int)(Vector3.Distance(from, IncrementDirection(to, normalized, extraLength)) / lengthIntervall);
		Vector3 vector = from;
		Vector3 vector2 = GetClosestPointOnNavMesh(from, navmeshSnapDistance);
		Vector3 vector3 = Vector3.zero;
		for (int i = 0; i < num; i++)
		{
			vector = IncrementDirection(vector, normalized, lengthIntervall);
			vector3 = GetClosestPointOnNavMesh(vector, navmeshSnapDistance);
			if (Vector3.Distance(vector2, vector3) > lengthIntervall * 1.25f)
			{
				return vector2;
			}
			vector2 = vector3;
		}
		return vector3;
	}

	public static Vector3 GetClosestPointOnNavMeshInDirectionTo(Vector3 from, Vector3 to, float extraLength, float lengthIntervall, float navmeshSnapDistance, NavCheckType checkType)
	{
		Vector3 vector = Vector3.zero;
		Vector3 normalized = (to - from).normalized;
		switch (checkType)
		{
		case NavCheckType.BEFORE:
			vector = from;
			break;
		case NavCheckType.AFTER:
			vector = to;
			break;
		case NavCheckType.BOTH:
			vector = from;
			break;
		}
		int num = (int)(Vector3.Distance(vector, IncrementDirection(to, normalized, extraLength)) / lengthIntervall);
		Vector3 vector2 = GetClosestPointOnNavMesh(vector, navmeshSnapDistance);
		Vector3 vector3 = Vector3.zero;
		for (int i = 0; i < num; i++)
		{
			vector = IncrementDirection(vector, normalized, lengthIntervall);
			vector3 = GetClosestPointOnNavMesh(vector, navmeshSnapDistance);
			if (Vector3.Distance(vector2, vector3) > lengthIntervall * 1.25f)
			{
				return vector2;
			}
			vector2 = vector3;
		}
		return vector3;
	}

	private static Vector3 IncrementDirection(Vector3 from, Vector3 dir, float length)
	{
		return from + dir * length;
	}

	public static void BroadCastToScene(SceneEvent sceneEvent)
	{
		List<GameObject> list = new List<GameObject>();
		list.AddRange(DontDestroyOnLoadManager.GetGameObjects());
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			Scene sceneAt = SceneManager.GetSceneAt(i);
			List<GameObject> list2 = new List<GameObject>();
			sceneAt.GetRootGameObjects(list2);
			if (list2 != null && list2.Count > 0)
			{
				list.AddRange(list2);
			}
		}
		List<SceneEventInterface> list3 = new List<SceneEventInterface>();
		foreach (GameObject item in list)
		{
			if (item != null)
			{
				item.GetComponentsInChildren(includeInactive: true, list3);
			}
			foreach (SceneEventInterface item2 in list3)
			{
				item2?.OnSceneEvent(sceneEvent);
			}
		}
	}

	public static void BroadCastLocalizeEvent()
	{
		List<GameObject> list = new List<GameObject>();
		list.AddRange(DontDestroyOnLoadManager.GetGameObjects());
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			Scene sceneAt = SceneManager.GetSceneAt(i);
			List<GameObject> list2 = new List<GameObject>();
			sceneAt.GetRootGameObjects(list2);
			if (list2 != null && list2.Count > 0)
			{
				list.AddRange(list2);
			}
		}
		List<LocalizeEventInterface> list3 = new List<LocalizeEventInterface>();
		foreach (GameObject item in list)
		{
			if (item != null)
			{
				item.GetComponentsInChildren(includeInactive: true, list3);
			}
			foreach (LocalizeEventInterface item2 in list3)
			{
				item2?.OnLocalizeEvent();
			}
		}
	}

	public static void AddEventTriggerListener(EventTrigger trigger, EventTriggerType eventType, Action<BaseEventData> callback)
	{
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = eventType;
		entry.callback.AddListener((BaseEventData eventData) =>
		{
			callback(eventData);
		});
		trigger.triggers.Add(entry);
	}

	public static bool SamePosition(Vector3 a, Vector3 b, float marginal = 0.05f)
	{
		return Vector3.Distance(a, b) <= marginal;
	}

	public static int RandomOneMinus()
	{
		if (UnityEngine.Random.Range(0, 2) != 0)
		{
			return 1;
		}
		return -1;
	}

	public static int OneMinusFromValue(float value)
	{
		if (value < 0f)
		{
			return -1;
		}
		return 1;
	}

	public static void LogBuild(string text)
	{
		if (!Application.isEditor || Settings.HelperLogToEditor)
		{
			Debug.Log(text + " :> " + DateTime.Now);
		}
	}

	public static T ObjToValue<T>(object obj)
	{
		if (obj != null && obj.GetType() == typeof(T))
		{
			return (T)obj;
		}
		return default;
	}

	public static bool IsValidTarget(Network_Player player)
	{
		if (player == null)
		{
			return false;
		}
		if (player.PlayerScript.IsDead)
		{
			return false;
		}
		return true;
	}

	public static bool IsValidWaterTarget(Network_Player player)
	{
		if (player == null)
		{
			return false;
		}
		if (player.PlayerScript.IsDead)
		{
			return false;
		}
		if (player.PersonController.HasRaftAsParent)
		{
			return false;
		}
		if (player.IsLocalPlayer)
		{
			if (player.PersonController.controllerType != ControllerType.Water)
			{
				if (player.FeetPosition.y > 1f || player.PersonController.waterFloat.HasOxygenRegion)
				{
					return false;
				}
				if (player.PersonController.DistanceToObstructionUnderneath < 2.5f || player.PersonController.controller.isGrounded)
				{
					return false;
				}
			}
			else if (player.PersonController.controllerType == ControllerType.Water && player.PersonController.waterFloat.HasOxygenRegion)
			{
				return false;
			}
		}
		else if (player.PersonController.controllerType != ControllerType.Water)
		{
			if (player.FeetPosition.y > 1f || player.PersonController.waterFloat.HasOxygenRegion)
			{
				return false;
			}
			if (player.PersonController.DistanceToObstructionUnderneath < 2.5f || player.PersonController.networkGrounded)
			{
				return false;
			}
		}
		else if (player.PersonController.controllerType == ControllerType.Water && player.PersonController.waterFloat.HasOxygenRegion)
		{
			return false;
		}
		return true;
	}

	public static Sprite GetLargeSteamAvatarFromID(CSteamID steamID)
	{
		int largeFriendAvatar = SteamFriends.GetLargeFriendAvatar(steamID);
		return GetSteamAvatarFromID(steamID, largeFriendAvatar);
	}

	public static Sprite GetMediumSteamAvatarFromID(CSteamID steamID)
	{
		int mediumFriendAvatar = SteamFriends.GetMediumFriendAvatar(steamID);
		return GetSteamAvatarFromID(steamID, mediumFriendAvatar);
	}

	public static Sprite GetSmallSteamAvatarFromID(CSteamID steamID)
	{
		int smallFriendAvatar = SteamFriends.GetSmallFriendAvatar(steamID);
		return GetSteamAvatarFromID(steamID, smallFriendAvatar);
	}

	private static Sprite GetSteamAvatarFromID(CSteamID steamID, int imageID)
	{
		if (SteamUtils.GetImageSize(imageID, out var pnWidth, out var pnHeight))
		{
			byte[] array = new byte[pnWidth * pnHeight * 4];
			if (SteamUtils.GetImageRGBA(imageID, array, (int)(pnWidth * pnHeight * 4)))
			{
				Texture2D texture2D = new Texture2D((int)pnWidth, (int)pnHeight, TextureFormat.RGBA32, mipChain: false, linear: false);
				texture2D.LoadRawTextureData(array);
				texture2D.Apply();
				return Sprite.Create(texture2D, new Rect(0f, 0f, pnWidth, 0L - (long)pnHeight), Vector2.one / 2f);
			}
		}
		return null;
	}

	public static void SetParentFromParentType(Transform transform, ParentType parentType)
	{
		if (parentType == ParentType.LockedPivot && transform.parent != SingletonGeneric<GameManager>.Singleton.lockedPivot)
		{
			transform.SetParent(SingletonGeneric<GameManager>.Singleton.lockedPivot);
		}
		else if (parentType == ParentType.Raft && transform.parent != SingletonGeneric<GameManager>.Singleton.raftTransform)
		{
			transform.SetParent(SingletonGeneric<GameManager>.Singleton.raftTransform);
		}
		else if (parentType == ParentType.None && transform.parent != null)
		{
			transform.SetParent(null);
		}
	}

	public static void ClearConsole()
	{
		Type.GetType("UnityEditor.LogEntries, UnityEditor.dll").GetMethod("Clear", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
	}

	public static T[] UnparentChildrenOfComponentType<T>(Transform parent) where T : Component
	{
		T[] componentsInChildren = parent.GetComponentsInChildren<T>(includeInactive: true);
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].transform.SetParent(null);
			}
		}
		return componentsInChildren;
	}

	public static Axis GetAxisComparedTo(Transform from, Transform to)
	{
		if (from == null || to == null)
		{
			return Axis.None;
		}
		Vector3 vector = to.InverseTransformPoint(from.position);
		if (vector.x >= 0f)
		{
			return Axis.X;
		}
		if (vector.y >= 0f)
		{
			return Axis.Y;
		}
		if (vector.z >= 0f)
		{
			return Axis.Z;
		}
		if (vector.x < 0f)
		{
			return Axis.NX;
		}
		if (vector.y < 0f)
		{
			return Axis.NY;
		}
		if (vector.z < 0f)
		{
			return Axis.NZ;
		}
		return Axis.None;
	}

	public static Axis GetAxisComparedToZOnly(Transform from, Transform to)
	{
		if (from == null || to == null)
		{
			return Axis.None;
		}
		Vector3 vector = to.InverseTransformPoint(from.position);
		if (vector.z >= 0f)
		{
			return Axis.Z;
		}
		if (vector.z < 0f)
		{
			return Axis.NZ;
		}
		return Axis.None;
	}

	private static void LocalEuleromparedToRootRecursive(Transform root, Transform parent, ref Vector3 localEuler)
	{
		if (!(parent == null) && !(parent == root))
		{
			localEuler += parent.localEulerAngles;
			LocalEuleromparedToRootRecursive(root, parent.parent, ref localEuler);
		}
	}

	private static void LocalPositionComparedToRootRecursive(Transform root, Transform parent, ref Vector3 localPos)
	{
		if (!(parent == null) && !(parent == root))
		{
			localPos += parent.localPosition;
			LocalPositionComparedToRootRecursive(root, parent.parent, ref localPos);
		}
	}
}
