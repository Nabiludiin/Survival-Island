using System;
using System.Collections.Generic;
using UnityEngine;

public class Landmark_WaterWell : MonoBehaviour_ID_Network
{
	[SerializeField]
	private Collider waterCollisionCollider;

	public FillCounter fillCounter;

	public List<FillContainerComponent> fillContainers;

	public Collider pickupInWaterCollider;

	private void Start()
	{
		FillCounter fillCounter = this.fillCounter;
		fillCounter.OnIncrement = (Action)Delegate.Combine(fillCounter.OnIncrement, new Action(UpdateContainerFill));
		UpdateContainerFill();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		FillCounter fillCounter = this.fillCounter;
		fillCounter.OnIncrement = (Action)Delegate.Remove(fillCounter.OnIncrement, new Action(UpdateContainerFill));
	}

	private void UpdateContainerFill()
	{
		if (fillContainers == null || !fillContainers.ContainsItems())
		{
			return;
		}
		bool flag = false;
		foreach (FillContainerComponent fillContainer in fillContainers)
		{
			if (!flag && fillCounter.NormalizedAmount >= 1f)
			{
				flag = true;
			}
			fillContainer.SetNormalizedValue(fillCounter.NormalizedAmount, instant: false);
		}
		if (waterCollisionCollider != null)
		{
			waterCollisionCollider.gameObject.SetActiveSafe(!flag);
		}
		HandleIfPickupInWaterShouldBeEnabled();
	}

	public void HandleIfPickupInWaterShouldBeEnabled()
	{
		if (fillCounter.NormalizedAmount > 0.8f && pickupInWaterCollider != null)
		{
			pickupInWaterCollider.enabled = true;
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.Increment)
		{
			Message_Network_Increment message_Network_Increment = msg as Message_Network_Increment;
			if (message_Network_Increment != null)
			{
				return fillCounter.OnCounterModified(message_Network_Increment);
			}
		}
		return base.Deserialize(msg, remoteID);
	}
}
