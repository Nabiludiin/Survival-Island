using System;
using FMODUnity;
using UnityEngine;

public class CookingSlot_Feedback : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private ParticleSystem particleSystem;

	[SerializeField]
	private CookingSlot cookingSlot;

	public EventReference er_hatchUp;

	public EventReference er_hatchDown;

	[SerializeField]
	private Battery battery;

	private bool hasItemInSlot;

	private bool isComplete;

	private void Awake()
	{
		this.cookingSlot.OnChangeEvent += SlotChanged;
		CookingSlot cookingSlot = this.cookingSlot;
		cookingSlot.OnSlotModified = (Action<CookingSlot>)Delegate.Combine(cookingSlot.OnSlotModified, new Action<CookingSlot>(SlotChanged));
		if (this.battery != null)
		{
			Battery battery = this.battery;
			battery.OnStateChange = (Action)Delegate.Combine(battery.OnStateChange, new Action(OnBatteryChange));
		}
	}

	private void OnDestroy()
	{
		this.cookingSlot.OnChangeEvent -= SlotChanged;
		CookingSlot cookingSlot = this.cookingSlot;
		cookingSlot.OnSlotModified = (Action<CookingSlot>)Delegate.Remove(cookingSlot.OnSlotModified, new Action<CookingSlot>(SlotChanged));
		if (this.battery != null)
		{
			Battery battery = this.battery;
			battery.OnStateChange = (Action)Delegate.Remove(battery.OnStateChange, new Action(OnBatteryChange));
		}
	}

	private void SlotChanged(CookingSlot slot)
	{
		hasItemInSlot = slot.CurrentItem != null;
		isComplete = slot.IsComplete;
		if (animator != null)
		{
			animator.SetBool("Open", !hasItemInSlot);
			if (hasItemInSlot)
			{
				RuntimeManager.PlayOneShot(er_hatchDown, animator.transform.position);
			}
			else
			{
				RuntimeManager.PlayOneShot(er_hatchUp, animator.transform.position);
			}
		}
		if (battery != null && battery.On && particleSystem != null)
		{
			if (hasItemInSlot && !isComplete)
			{
				particleSystem.Play();
			}
			else
			{
				particleSystem.Stop();
			}
		}
		else if (battery != null && !battery.On && particleSystem != null)
		{
			particleSystem.Stop();
		}
	}

	private void OnBatteryChange()
	{
		if (battery.On && !isComplete && hasItemInSlot)
		{
			particleSystem.PlaySafe();
		}
		else
		{
			particleSystem.StopSafe();
		}
	}
}
