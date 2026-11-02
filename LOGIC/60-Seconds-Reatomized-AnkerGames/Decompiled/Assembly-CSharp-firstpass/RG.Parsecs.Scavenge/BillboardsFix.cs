using System;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class BillboardsFix : MonoBehaviour
{
	private Camera thisCamera;

	private float _originalFov;

	public void Start()
	{
		thisCamera = GetComponent<Camera>();
		_originalFov = thisCamera.fieldOfView;
	}

	public void PreRenderAdjustFOV(Camera cam)
	{
		if (cam == thisCamera)
		{
			cam.fieldOfView = _originalFov;
		}
	}

	public void PreCullAdjustFOV(Camera cam)
	{
		_originalFov = thisCamera.fieldOfView;
		if (cam == thisCamera)
		{
			cam.fieldOfView += 10f;
			cam.ResetWorldToCameraMatrix();
			cam.ResetProjectionMatrix();
		}
	}

	public void OnEnable()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(PreCullAdjustFOV));
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(PreRenderAdjustFOV));
	}

	public void OnDisable()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(PreCullAdjustFOV));
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPreRender, new Camera.CameraCallback(PreRenderAdjustFOV));
	}
}
