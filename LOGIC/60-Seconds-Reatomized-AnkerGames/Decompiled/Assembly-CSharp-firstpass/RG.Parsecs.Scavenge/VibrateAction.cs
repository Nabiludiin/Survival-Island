using Rewired;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class VibrateAction : EventAction
{
	public VibrationSetting VibrationSetting;

	protected override void Act(EventActionArgs args = null)
	{
		if (args == null)
		{
			Debug.LogFormat("VibrateAction args are null");
		}
		else if (args.Actor == null)
		{
			Debug.LogFormat("VibrateAction args.Actor is null");
		}
		else if (args.Actor.Player == null)
		{
			Debug.LogFormat("VibrateAction args.Actor.Player is null");
		}
		else
		{
			Vibrate(VibrationSetting, args.Actor.Player);
		}
	}

	private void Vibrate(VibrationSetting setting, Player player)
	{
		Controller lastActiveController = player.controllers.GetLastActiveController();
		if (lastActiveController != null && lastActiveController is Joystick item)
		{
			Vibrate(setting, item);
		}
	}

	private void Vibrate(VibrationSetting setting, Joystick item)
	{
		if (!item.supportsVibration)
		{
			return;
		}
		float motorSpeed = setting.MotorSpeed;
		float num = 0f;
		int vibrationMotorCount = item.vibrationMotorCount;
		for (int i = 0; i < vibrationMotorCount; i++)
		{
			float vibration = item.GetVibration(i);
			if (vibration > num)
			{
				num = vibration;
			}
		}
		if (num < motorSpeed)
		{
			for (int j = 0; j < item.vibrationMotorCount; j++)
			{
				item.SetVibration(j, motorSpeed, setting.Duration, stopOtherMotors: false);
			}
		}
	}
}
