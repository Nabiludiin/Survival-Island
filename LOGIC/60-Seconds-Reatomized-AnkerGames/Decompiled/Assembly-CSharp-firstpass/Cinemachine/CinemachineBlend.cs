using System;
using UnityEngine;

namespace Cinemachine;

public class CinemachineBlend
{
	public ICinemachineCamera CamA { get; set; }

	public ICinemachineCamera CamB { get; set; }

	public AnimationCurve BlendCurve { get; set; }

	public float TimeInBlend { get; set; }

	public float BlendWeight
	{
		get
		{
			if (BlendCurve == null)
			{
				return 0f;
			}
			return BlendCurve.Evaluate(TimeInBlend);
		}
	}

	public bool IsValid
	{
		get
		{
			if (CamA == null)
			{
				return CamB != null;
			}
			return true;
		}
	}

	public float Duration { get; set; }

	public bool IsComplete => TimeInBlend >= Duration;

	public string Description
	{
		get
		{
			string arg = ((CamA != null) ? ("[" + CamA.Name + "]") : "(none)");
			string arg2 = ((CamB != null) ? ("[" + CamB.Name + "]") : "(none)");
			int num = (int)(BlendWeight * 100f);
			return $"{arg2} {num}% from {arg}";
		}
	}

	public CameraState State => CameraState.Lerp(CamA.State, CamB.State, BlendWeight);

	public bool Uses(ICinemachineCamera cam)
	{
		if (cam == CamA || cam == CamB)
		{
			return true;
		}
		if (CamA is BlendSourceVirtualCamera blendSourceVirtualCamera && blendSourceVirtualCamera.Blend.Uses(cam))
		{
			return true;
		}
		if (CamB is BlendSourceVirtualCamera blendSourceVirtualCamera2 && blendSourceVirtualCamera2.Blend.Uses(cam))
		{
			return true;
		}
		return false;
	}

	public CinemachineBlend(ICinemachineCamera a, ICinemachineCamera b, AnimationCurve curve, float duration, float t)
	{
		if (a == null || b == null)
		{
			throw new ArgumentException("Blend cameras cannot be null");
		}
		CamA = a;
		CamB = b;
		BlendCurve = curve;
		TimeInBlend = t;
		Duration = duration;
	}

	public void UpdateCameraState(Vector3 worldUp, float deltaTime)
	{
		CinemachineCore.Instance.UpdateVirtualCamera(CamA, worldUp, deltaTime);
		CinemachineCore.Instance.UpdateVirtualCamera(CamB, worldUp, deltaTime);
	}
}
