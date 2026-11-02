using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraBob : MonoBehaviour
{
	public PersonController controller;

	public float bobbingSpeed = 0.18f;

	public float bobbingSpeedSprint = 0.18f;

	public float bobbingAmount = 0.2f;

	public float midpoint = 2f;

	private float timer;

	private void Update()
	{
		if (controller.controllerType == ControllerType.Water || CanvasHelper.ActiveMenu != MenuType.None)
		{
			return;
		}
		float num = 0f;
		float f;
		float f2;
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			Vector2 vector = PlayerInput.GetPlayerByIndex(0).actions["Move"].ReadValue<Vector2>();
			f = vector.x;
			f2 = vector.y;
		}
		else
		{
			f = MyInput.GetAxis("Strafe");
			f2 = MyInput.GetAxis("Walk");
		}
		float num2 = (controller.sprinting ? bobbingSpeedSprint : bobbingSpeed);
		Vector3 localPosition = transform.localPosition;
		if (Mathf.Abs(f) == 0f && Mathf.Abs(f2) == 0f)
		{
			timer = 0f;
		}
		else
		{
			num = Mathf.Sin(timer);
			timer += num2;
			if (timer > MathF.PI * 2f)
			{
				timer -= MathF.PI * 2f;
			}
		}
		if (num != 0f)
		{
			float num3 = num * bobbingAmount;
			num3 = Mathf.Clamp(Mathf.Abs(f) + Mathf.Abs(f2), 0f, 1f) * num3;
			localPosition.y = midpoint + num3;
		}
		else
		{
			localPosition.y = midpoint;
		}
		transform.localPosition = localPosition;
	}
}
