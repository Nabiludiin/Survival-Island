using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine;

[DocumentationSorting(14f, DocumentationSortingAttribute.Level.UserRef)]
[RequireComponent(typeof(Camera))]
[DisallowMultipleComponent]
[ExecuteInEditMode]
[AddComponentMenu("Cinemachine/CinemachineExternalCamera")]
public class CinemachineExternalCamera : CinemachineVirtualCameraBase
{
	[Tooltip("The object that the camera is looking at.  Setting this will improve the quality of the blends to and from this camera")]
	[NoSaveDuringPlay]
	public Transform m_LookAt;

	private Camera m_Camera;

	private CameraState m_State = CameraState.Default;

	public override CameraState State => m_State;

	public override Transform LookAt
	{
		get
		{
			return m_LookAt;
		}
		set
		{
			m_LookAt = value;
		}
	}

	public override Transform Follow { get; set; }

	public override void UpdateCameraState(Vector3 worldUp, float deltaTime)
	{
		if (m_Camera == null)
		{
			m_Camera = GetComponent<Camera>();
		}
		m_State = CameraState.Default;
		m_State.RawPosition = transform.position;
		m_State.RawOrientation = transform.rotation;
		m_State.ReferenceUp = m_State.RawOrientation * Vector3.up;
		if (m_Camera != null)
		{
			m_State.Lens = LensSettings.FromCamera(m_Camera);
		}
		if (m_LookAt != null)
		{
			m_State.ReferenceLookAt = m_LookAt.transform.position;
			Vector3 vector = m_State.ReferenceLookAt - State.RawPosition;
			if (!vector.AlmostZero())
			{
				m_State.ReferenceLookAt = m_State.RawPosition + Vector3.Project(vector, State.RawOrientation * Vector3.forward);
			}
		}
	}
}
