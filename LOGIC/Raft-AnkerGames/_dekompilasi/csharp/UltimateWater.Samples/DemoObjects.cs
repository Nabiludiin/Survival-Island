using System.Collections.Generic;
using UnityEngine;

namespace UltimateWater.Samples;

public class DemoObjects : MonoBehaviour
{
	private enum Type
	{
		FishingRod,
		Boat,
		UserInput,
		Generator
	}

	[SerializeField]
	private float _Radius = 1f;

	[SerializeField]
	private float _Speed = 1f;

	[SerializeField]
	private Vector3 _Center = Vector3.zero;

	[SerializeField]
	private Type _Type = Type.Boat;

	[SerializeField]
	private float _Force;

	[SerializeField]
	private float _DisableTime = 1024f;

	private float _Angle;

	private void Awake()
	{
		_Center = transform.position;
		Invoke("Disable", _DisableTime);
	}

	private void FixedUpdate()
	{
		switch (_Type)
		{
		case Type.Boat:
			Boat();
			break;
		case Type.FishingRod:
			FishingRod();
			break;
		case Type.UserInput:
			UserInput();
			break;
		case Type.Generator:
			Generator();
			break;
		}
	}

	private void Disable()
	{
		enabled = false;
	}

	private void FishingRod()
	{
		_Angle += _Speed * Time.deltaTime;
		transform.position = _Center + _Radius * new Vector3(Mathf.Cos(_Angle), 0f, Mathf.Sin(_Angle));
		transform.eulerAngles = new Vector3(0f, (0f - _Angle) * 57.29578f, 0f);
	}

	private void Boat()
	{
		transform.position += transform.forward * _Speed * Time.deltaTime;
	}

	private void UserInput()
	{
		float num = Input.GetAxis("Horizontal") * _Speed * Time.deltaTime;
		float num2 = Input.GetAxis("Vertical") * _Speed * Time.deltaTime;
		transform.position += transform.forward * num2;
		transform.position += transform.right * num;
		if (Input.GetKey(KeyCode.Q))
		{
			transform.position -= transform.up * _Speed * Time.deltaTime * 0.33f;
		}
		if (Input.GetKey(KeyCode.E))
		{
			transform.position += transform.up * _Speed * Time.deltaTime * 0.33f;
		}
	}

	private void Generator()
	{
		WaterRipples.AddForce(new List<WaterForce.Data>
		{
			new WaterForce.Data
			{
				Position = transform.position,
				Force = _Force * Mathf.Cos(Time.timeSinceLevelLoad * _Speed)
			}
		}, _Radius);
	}
}
