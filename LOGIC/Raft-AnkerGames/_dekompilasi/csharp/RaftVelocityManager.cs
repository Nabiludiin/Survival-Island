using System;
using System.Collections.Generic;
using UnityEngine;

public class RaftVelocityManager : MonoBehaviour, SceneEventInterface
{
	public static List<SteeringWheel> steeringWheels = new List<SteeringWheel>();

	private static List<MotorWheel> motors = new List<MotorWheel>();

	private static Raft raft;

	private Quaternion prevRotation;

	private static Vector3 motorDirection = Vector3.zero;

	public static int motorStrength = 0;

	public static int extraMotorStrength = 0;

	public static float motorSpeed = 0f;

	public static List<MotorWheel> Motors => motors;

	public static Vector3 MotorDirection
	{
		get
		{
			if (MotorWheelWeightStrength == MotorWheel.WeightStrength.NotStrongEnough)
			{
				return Vector3.zero;
			}
			return motorDirection;
		}
	}

	public static MotorWheel.WeightStrength MotorWheelWeightStrength
	{
		get
		{
			if (raft != null && raft.IsAnchored)
			{
				return MotorWheel.WeightStrength.NotStrongEnough;
			}
			if (RaftWeightManager.FoundationWeight <= motorStrength)
			{
				return MotorWheel.WeightStrength.StrongEnough;
			}
			if (RaftWeightManager.FoundationWeight <= motorStrength + extraMotorStrength)
			{
				return MotorWheel.WeightStrength.Weak;
			}
			return MotorWheel.WeightStrength.NotStrongEnough;
		}
	}

	public static float MotorWheelWeightStrengthNormalized => MotorWheelWeightStrength switch
	{
		MotorWheel.WeightStrength.Weak => 0.5f, 
		MotorWheel.WeightStrength.NotStrongEnough => 1f, 
		_ => 0f, 
	};

	public void Start()
	{
		raft = ComponentManager<Raft>.Value;
	}

	public void Update()
	{
		if (raft != null && Quaternion.Angle(prevRotation, raft.transform.rotation) > 10f)
		{
			RecalculateMotors();
			prevRotation = raft.transform.rotation;
		}
	}

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			motors.Clear();
			steeringWheels.Clear();
			raft = null;
			motorSpeed = (extraMotorStrength = (motorStrength = 0));
			motorDirection = Vector3.zero;
		}
	}

	public static void AddSteeringWheel(SteeringWheel steeringWheel)
	{
		steeringWheels.Add(steeringWheel);
	}

	public static void RemoveSteeringWheel(SteeringWheel steeringWheel)
	{
		if (steeringWheels.Contains(steeringWheel))
		{
			steeringWheels.Remove(steeringWheel);
		}
	}

	public static void AddMotor(MotorWheel motor)
	{
		motors.Add(motor);
		motor.OnMotorDirty = (Action)Delegate.Combine(motor.OnMotorDirty, new Action(RecalculateMotors));
		RecalculateMotors();
	}

	public static void RemoveMotor(MotorWheel motor)
	{
		if (motors.Contains(motor))
		{
			motors.Remove(motor);
			motor.OnMotorDirty = (Action)Delegate.Remove(motor.OnMotorDirty, new Action(RecalculateMotors));
			RecalculateMotors();
		}
	}

	private static void RecalculateMotors()
	{
		motorDirection = Vector3.zero;
		motorStrength = 0;
		extraMotorStrength = 0;
		motorSpeed = 0f;
		foreach (MotorWheel motor in motors)
		{
			motorDirection += motor.PushDirection;
			motorStrength += motor.MotorStrength;
			extraMotorStrength += motor.ExtraMotorStrength;
			motorSpeed += motor.RaftSpeed;
		}
	}
}
