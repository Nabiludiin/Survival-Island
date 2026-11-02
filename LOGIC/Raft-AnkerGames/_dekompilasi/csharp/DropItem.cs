using System;
using UnityEngine;

public class DropItem : MonoBehaviour
{
	[SerializeField]
	private Collider collider;

	[SerializeField]
	private float despawnTime = 20f;

	private void Awake()
	{
		if (GetComponent<WaterFloatSemih_Rigidbody>() != null)
		{
			WaterFloatSemih_Rigidbody component = GetComponent<WaterFloatSemih_Rigidbody>();
			component.OnHitWater = (Action)Delegate.Combine(component.OnHitWater, new Action(OnHitWater));
		}
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Combine(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
		UnityEngine.Object.Destroy(gameObject, despawnTime);
	}

	private void OnEnable()
	{
		if (collider != null)
		{
			collider.isTrigger = false;
		}
		if (gameObject != null)
		{
			gameObject.layer = LayerMask.NameToLayer("DropItem");
		}
	}

	private void OnDestroy()
	{
		NetworkIDManager.RemoveNetworkID(GetComponent<PickupItem_Networked>());
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Remove(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (transform != null && transform.parent == null && Helper.ObjectIsOnLayer(collision.gameObject, LayerMasks.MASK_Block))
		{
			transform.SetParent(SingletonGeneric<GameManager>.Singleton.lockedPivot);
		}
	}

	private void OnWorldShift(Vector3 shift)
	{
		if (gameObject != null && gameObject.activeInHierarchy && transform != null && transform.parent == null)
		{
			transform.position -= shift;
		}
	}

	private void OnHitWater()
	{
		if (collider != null)
		{
			collider.isTrigger = true;
		}
		if (gameObject != null)
		{
			gameObject.layer = LayerMask.NameToLayer("Item");
		}
	}
}
