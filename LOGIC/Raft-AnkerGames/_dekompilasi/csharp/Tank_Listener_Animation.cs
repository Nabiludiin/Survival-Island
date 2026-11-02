using System;
using FMODUnity;
using UnityEngine;

public class Tank_Listener_Animation : MonoBehaviour
{
	public Tank tank;

	public Animator animatorController;

	public AnimatorPropertySetting_Legit animationSettings;

	public EventReference er_TankOpened;

	public EventReference er_TankClosed;

	public void Start()
	{
		Tank tank = this.tank;
		tank.OnTankPlayerManualInteraction = (Action)Delegate.Combine(tank.OnTankPlayerManualInteraction, new Action(TriggerAnim));
	}

	public void OnDestroy()
	{
		Tank tank = this.tank;
		tank.OnTankPlayerManualInteraction = (Action)Delegate.Remove(tank.OnTankPlayerManualInteraction, new Action(TriggerAnim));
	}

	public void OnHatchOpened()
	{
		RuntimeManager.PlayOneShot(er_TankOpened, transform.position);
	}

	public void OnHatchClosed()
	{
		RuntimeManager.PlayOneShot(er_TankClosed, transform.position);
	}

	private void TriggerAnim()
	{
		switch (animationSettings.propertyType)
		{
		case AnimatorPropertyType.Float:
			animatorController.SetFloat(animationSettings.parameterName, animationSettings.floatValue);
			break;
		case AnimatorPropertyType.Bool:
			animatorController.SetBool(animationSettings.parameterName, animationSettings.boolValue);
			break;
		case AnimatorPropertyType.Trigger:
			animatorController.SetTrigger(animationSettings.parameterName);
			break;
		case AnimatorPropertyType.Int:
			animatorController.SetInteger(animationSettings.parameterName, animationSettings.intValue);
			break;
		}
	}
}
