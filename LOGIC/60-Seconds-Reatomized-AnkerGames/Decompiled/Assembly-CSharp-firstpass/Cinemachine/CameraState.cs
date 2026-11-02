using System;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine;

public struct CameraState
{
	public struct CustomBlendable(UnityEngine.Object custom, float weight)
	{
		public UnityEngine.Object m_Custom = custom;

		public float m_Weight = weight;
	}

	public static Vector3 kNoPoint = new Vector3(float.NaN, float.NaN, float.NaN);

	private CustomBlendable mCustom0;

	private CustomBlendable mCustom1;

	private CustomBlendable mCustom2;

	private CustomBlendable mCustom3;

	private List<CustomBlendable> m_CustomOverflow;

	public LensSettings Lens { get; set; }

	public Vector3 ReferenceUp { get; set; }

	public Vector3 ReferenceLookAt { get; set; }

	public bool HasLookAt => ReferenceLookAt == ReferenceLookAt;

	public Vector3 RawPosition { get; set; }

	public Quaternion RawOrientation { get; set; }

	internal Vector3 PositionDampingBypass { get; set; }

	public float ShotQuality { get; set; }

	public Vector3 PositionCorrection { get; set; }

	public Quaternion OrientationCorrection { get; set; }

	public Vector3 CorrectedPosition => RawPosition + PositionCorrection;

	public Quaternion CorrectedOrientation => RawOrientation * OrientationCorrection;

	public Vector3 FinalPosition => RawPosition + PositionCorrection;

	public Quaternion FinalOrientation
	{
		get
		{
			if (Mathf.Abs(Lens.Dutch) > 0.0001f)
			{
				return CorrectedOrientation * Quaternion.AngleAxis(Lens.Dutch, Vector3.forward);
			}
			return CorrectedOrientation;
		}
	}

	public static CameraState Default => new CameraState
	{
		Lens = LensSettings.Default,
		ReferenceUp = Vector3.up,
		ReferenceLookAt = kNoPoint,
		RawPosition = Vector3.zero,
		RawOrientation = Quaternion.identity,
		ShotQuality = 1f,
		PositionCorrection = Vector3.zero,
		OrientationCorrection = Quaternion.identity,
		PositionDampingBypass = Vector3.zero
	};

	public int NumCustomBlendables { get; private set; }

	public CustomBlendable GetCustomBlendable(int index)
	{
		switch (index)
		{
		case 0:
			return mCustom0;
		case 1:
			return mCustom1;
		case 2:
			return mCustom2;
		case 3:
			return mCustom3;
		default:
			index -= 4;
			if (m_CustomOverflow != null && index < m_CustomOverflow.Count)
			{
				return m_CustomOverflow[index];
			}
			return new CustomBlendable(null, 0f);
		}
	}

	private int FindCustomBlendable(UnityEngine.Object custom)
	{
		if (mCustom0.m_Custom == custom)
		{
			return 0;
		}
		if (mCustom1.m_Custom == custom)
		{
			return 1;
		}
		if (mCustom2.m_Custom == custom)
		{
			return 2;
		}
		if (mCustom3.m_Custom == custom)
		{
			return 3;
		}
		if (m_CustomOverflow != null)
		{
			for (int i = 0; i < m_CustomOverflow.Count; i++)
			{
				if (m_CustomOverflow[i].m_Custom == custom)
				{
					return i + 4;
				}
			}
		}
		return -1;
	}

	public void AddCustomBlendable(CustomBlendable b)
	{
		int num = FindCustomBlendable(b.m_Custom);
		if (num >= 0)
		{
			b.m_Weight += GetCustomBlendable(num).m_Weight;
		}
		else
		{
			num = NumCustomBlendables++;
		}
		switch (num)
		{
		case 0:
			mCustom0 = b;
			return;
		case 1:
			mCustom1 = b;
			return;
		case 2:
			mCustom2 = b;
			return;
		case 3:
			mCustom3 = b;
			return;
		}
		if (m_CustomOverflow == null)
		{
			m_CustomOverflow = new List<CustomBlendable>();
		}
		m_CustomOverflow.Add(b);
	}

	public static CameraState Lerp(CameraState stateA, CameraState stateB, float t)
	{
		t = Mathf.Clamp01(t);
		float t2 = t;
		CameraState result = new CameraState
		{
			Lens = LensSettings.Lerp(stateA.Lens, stateB.Lens, t),
			ReferenceUp = Vector3.Slerp(stateA.ReferenceUp, stateB.ReferenceUp, t),
			RawPosition = Vector3.Lerp(stateA.RawPosition, stateB.RawPosition, t),
			ShotQuality = Mathf.Lerp(stateA.ShotQuality, stateB.ShotQuality, t),
			PositionCorrection = Vector3.Lerp(stateA.PositionCorrection, stateB.PositionCorrection, t),
			OrientationCorrection = Quaternion.Slerp(stateA.OrientationCorrection, stateB.OrientationCorrection, t)
		};
		Vector3 v = Vector3.zero;
		if (!stateA.HasLookAt || !stateB.HasLookAt)
		{
			result.ReferenceLookAt = kNoPoint;
		}
		else
		{
			float fieldOfView = stateA.Lens.FieldOfView;
			float fieldOfView2 = stateB.Lens.FieldOfView;
			if (!result.Lens.Orthographic && !Mathf.Approximately(fieldOfView, fieldOfView2))
			{
				LensSettings lens = result.Lens;
				lens.FieldOfView = result.InterpolateFOV(fieldOfView, fieldOfView2, Mathf.Max((stateA.ReferenceLookAt - stateA.CorrectedPosition).magnitude, stateA.Lens.NearClipPlane), Mathf.Max((stateB.ReferenceLookAt - stateB.CorrectedPosition).magnitude, stateB.Lens.NearClipPlane), t);
				result.Lens = lens;
				t2 = Mathf.Abs((lens.FieldOfView - fieldOfView) / (fieldOfView2 - fieldOfView));
			}
			result.ReferenceLookAt = Vector3.Lerp(stateA.ReferenceLookAt, stateB.ReferenceLookAt, t2);
			if (Quaternion.Angle(stateA.RawOrientation, stateB.RawOrientation) > 0.0001f)
			{
				v = result.ReferenceLookAt - result.CorrectedPosition;
			}
		}
		if (v.AlmostZero())
		{
			result.RawOrientation = UnityQuaternionExtensions.SlerpWithReferenceUp(stateA.RawOrientation, stateB.RawOrientation, t, result.ReferenceUp);
		}
		else
		{
			v = v.normalized;
			if ((v - result.ReferenceUp).AlmostZero() || (v + result.ReferenceUp).AlmostZero())
			{
				result.RawOrientation = UnityQuaternionExtensions.SlerpWithReferenceUp(stateA.RawOrientation, stateB.RawOrientation, t, result.ReferenceUp);
			}
			else
			{
				result.RawOrientation = Quaternion.LookRotation(v, result.ReferenceUp);
				Vector2 a = -stateA.RawOrientation.GetCameraRotationToTarget(stateA.ReferenceLookAt - stateA.CorrectedPosition, stateA.ReferenceUp);
				Vector2 b = -stateB.RawOrientation.GetCameraRotationToTarget(stateB.ReferenceLookAt - stateB.CorrectedPosition, stateB.ReferenceUp);
				result.RawOrientation = result.RawOrientation.ApplyCameraRotation(Vector2.Lerp(a, b, t2), result.ReferenceUp);
			}
		}
		for (int i = 0; i < stateA.NumCustomBlendables; i++)
		{
			CustomBlendable customBlendable = stateA.GetCustomBlendable(i);
			customBlendable.m_Weight *= 1f - t;
			if (customBlendable.m_Weight > 0.0001f)
			{
				result.AddCustomBlendable(customBlendable);
			}
		}
		for (int j = 0; j < stateB.NumCustomBlendables; j++)
		{
			CustomBlendable customBlendable2 = stateB.GetCustomBlendable(j);
			customBlendable2.m_Weight *= t;
			if (customBlendable2.m_Weight > 0.0001f)
			{
				result.AddCustomBlendable(customBlendable2);
			}
		}
		return result;
	}

	private float InterpolateFOV(float fovA, float fovB, float dA, float dB, float t)
	{
		float a = dA * 2f * Mathf.Tan(fovA * ((float)Math.PI / 180f) / 2f);
		float b = dB * 2f * Mathf.Tan(fovB * ((float)Math.PI / 180f) / 2f);
		float num = Mathf.Lerp(a, b, t);
		float value = 179f;
		float num2 = Mathf.Lerp(dA, dB, t);
		if (num2 > 0.0001f)
		{
			value = 2f * Mathf.Atan(num / (2f * num2)) * 57.29578f;
		}
		return Mathf.Clamp(value, Mathf.Min(fovA, fovB), Mathf.Max(fovA, fovB));
	}
}
