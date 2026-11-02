using System;
using System.Collections;
using UnityEngine;

public class SharkBait : Block
{
	public bool isAttacked;

	private static Transform raftTransform;

	private const float checkTimerMin = 0.05f;

	private bool initialized;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (initialized)
		{
			WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Remove(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
		}
		Helper.UnparentChildrenOfComponentType<AI_StateMachine_Shark>(transform);
		if (AI_StateMachine_Shark.sharkBaitsInWorld != null && AI_StateMachine_Shark.sharkBaitsInWorld.Contains(this))
		{
			AI_StateMachine_Shark.sharkBaitsInWorld.Remove(this);
		}
	}

	public void OnHitWater()
	{
		if (!AI_StateMachine_Shark.sharkBaitsInWorld.Contains(this))
		{
			AI_StateMachine_Shark.sharkBaitsInWorld.Add(this);
		}
		if (!initialized)
		{
			initialized = true;
			transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
			transform.SetParent(null);
			gameObject.SetLayerRecursivly(LayerMasks.MASK_Default.value);
			MaxHealth = (int)((float)MaxHealth * GameModeValueManager.GetCurrentGameModeValue().itemSpecificVariables.sharkBaitHealthMultiplier);
			SetHealth(MaxHealth);
			UnityEngine.Object.Destroy(GetComponent<Rigidbody>());
			GetComponent<WaterFloatOxygenBounds>().setToSurface = true;
			StartCoroutine(CheckForRemove());
			WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Combine(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
		}
	}

	private void OnWorldShift(Vector3 shift)
	{
		transform.position -= shift;
	}

	private IEnumerator CheckForRemove()
	{
		float distance = 0f;
		while (distance < 500f)
		{
			yield return new WaitForSeconds(3f);
			if (raftTransform == null)
			{
				Raft value = ComponentManager<Raft>.Value;
				if (value != null)
				{
					raftTransform = value.transform;
				}
			}
			if (!(raftTransform == null) && !isAttacked)
			{
				distance = Vector3.Distance(transform.position, raftTransform.position);
			}
		}
		Helper.UnparentChildrenOfComponentType<AI_StateMachine_Shark>(transform);
		UnityEngine.Object.Destroy(gameObject);
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Sharkbait(RGDType.Block_Sharkbait, this);
	}
}
