using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine;

[Serializable]
[DocumentationSorting(6.4f, DocumentationSortingAttribute.Level.UserRef)]
public struct AxisState(float maxSpeed, float accelTime, float decelTime, float val, string name, bool invert)
{
	[NoSaveDuringPlay]
	[Tooltip("The current value of the axis.")]
	public float Value = val;

	[Tooltip("The maximum speed of this axis in units/second")]
	public float m_MaxSpeed = maxSpeed;

	[Tooltip("The amount of time in seconds it takes to accelerate to MaxSpeed with the supplied Axis at its maximum value")]
	public float m_AccelTime = accelTime;

	[Tooltip("The amount of time in seconds it takes to decelerate the axis to zero if the supplied axis is in a neutral position")]
	public float m_DecelTime = decelTime;

	[FormerlySerializedAs("m_AxisName")]
	[Tooltip("The name of this axis as specified in Unity Input manager. Setting to an empty string will disable the automatic updating of this axis")]
	public string m_InputAxisName = name;

	[NoSaveDuringPlay]
	[Tooltip("The value of the input axis.  A value of 0 means no input.  You can drive this directly from a custom input system, or you can set the Axis Name and have the value driven by the internal Input Manager")]
	public float m_InputAxisValue = 0f;

	[NoSaveDuringPlay]
	[Tooltip("If checked, then the raw value of the input axis will be inverted before it is used")]
	public bool m_InvertAxis = invert;

	private float mCurrentSpeed = 0f;

	private float mMinValue = 0f;

	private float mMaxValue = 0f;

	private bool mWrapAround = false;

	private const float Epsilon = 0.0001f;

	public void Validate()
	{
		m_MaxSpeed = Mathf.Max(0f, m_MaxSpeed);
		m_AccelTime = Mathf.Max(0f, m_AccelTime);
		m_DecelTime = Mathf.Max(0f, m_DecelTime);
	}

	public void SetThresholds(float minValue, float maxValue, bool wrapAround)
	{
		mMinValue = minValue;
		mMaxValue = maxValue;
		mWrapAround = wrapAround;
	}

	public bool Update(float deltaTime)
	{
		if (!string.IsNullOrEmpty(m_InputAxisName))
		{
			try
			{
				m_InputAxisValue = CinemachineCore.GetInputAxis(m_InputAxisName);
			}
			catch (ArgumentException ex)
			{
				Debug.LogError(ex.ToString());
			}
		}
		float num = m_InputAxisValue;
		if (m_InvertAxis)
		{
			num *= -1f;
		}
		if (m_MaxSpeed > 0.0001f)
		{
			float num2 = num * m_MaxSpeed;
			if (Mathf.Abs(num2) < 0.0001f || (Mathf.Sign(mCurrentSpeed) == Mathf.Sign(num2) && Mathf.Abs(num2) < Mathf.Abs(mCurrentSpeed)))
			{
				float num3 = Mathf.Min(Mathf.Abs(num2 - mCurrentSpeed) / Mathf.Max(0.0001f, m_DecelTime) * deltaTime, Mathf.Abs(mCurrentSpeed));
				mCurrentSpeed -= Mathf.Sign(mCurrentSpeed) * num3;
			}
			else
			{
				float num4 = Mathf.Abs(num2 - mCurrentSpeed) / Mathf.Max(0.0001f, m_AccelTime);
				mCurrentSpeed += Mathf.Sign(num2) * num4 * deltaTime;
				if (Mathf.Sign(mCurrentSpeed) == Mathf.Sign(num2) && Mathf.Abs(mCurrentSpeed) > Mathf.Abs(num2))
				{
					mCurrentSpeed = num2;
				}
			}
		}
		float maxSpeed = GetMaxSpeed();
		mCurrentSpeed = Mathf.Clamp(mCurrentSpeed, 0f - maxSpeed, maxSpeed);
		Value += mCurrentSpeed * deltaTime;
		if (Value > mMaxValue || Value < mMinValue)
		{
			if (mWrapAround)
			{
				if (Value > mMaxValue)
				{
					Value = mMinValue + (Value - mMaxValue);
				}
				else
				{
					Value = mMaxValue + (Value - mMinValue);
				}
			}
			else
			{
				Value = Mathf.Clamp(Value, mMinValue, mMaxValue);
				mCurrentSpeed = 0f;
			}
		}
		return Mathf.Abs(num) > 0.0001f;
	}

	private float GetMaxSpeed()
	{
		float num = mMaxValue - mMinValue;
		if (!mWrapAround && num > 0f)
		{
			float num2 = num / 10f;
			if (mCurrentSpeed > 0f && mMaxValue - Value < num2)
			{
				float t = (mMaxValue - Value) / num2;
				return Mathf.Lerp(0f, m_MaxSpeed, t);
			}
			if (mCurrentSpeed < 0f && Value - mMinValue < num2)
			{
				float t2 = (Value - mMinValue) / num2;
				return Mathf.Lerp(0f, m_MaxSpeed, t2);
			}
		}
		return m_MaxSpeed;
	}
}
