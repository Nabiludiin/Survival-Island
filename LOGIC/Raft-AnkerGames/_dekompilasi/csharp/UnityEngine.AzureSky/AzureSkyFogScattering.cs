namespace UnityEngine.AzureSky;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[ImageEffectAllowedInSceneView]
[AddComponentMenu("Azure[Sky]/Fog Scattering")]
public class AzureSkyFogScattering : MonoBehaviour
{
	public Material fogScatteringMaterial;

	private Camera playerCamera;

	private Camera m_camera;

	private Transform m_cameraTransform;

	private Vector3[] m_frustumCorners = new Vector3[4];

	private Rect m_rect = new Rect(0f, 0f, 1f, 1f);

	private Matrix4x4 m_frustumCornersArray;

	public LayerMask excludeLayers = 0;

	public CameraClearFlags clearFlags = CameraClearFlags.Nothing;

	public float farClipPlaneDistance = 150f;

	private Camera m_tmpCam;

	private void Start()
	{
		m_camera = (playerCamera = GetComponent<Camera>());
		m_cameraTransform = m_camera.transform;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (Application.isEditor && !Application.isPlaying)
		{
			return;
		}
		if (playerCamera != null && m_camera != null && playerCamera.fieldOfView != m_camera.fieldOfView)
		{
			m_camera.fieldOfView = playerCamera.fieldOfView;
		}
		if (m_camera == null)
		{
			return;
		}
		if (fogScatteringMaterial == null)
		{
			Graphics.Blit(source, destination);
			return;
		}
		m_camera.CalculateFrustumCorners(m_rect, m_camera.farClipPlane, m_camera.stereoActiveEye, m_frustumCorners);
		m_frustumCornersArray = Matrix4x4.identity;
		m_frustumCornersArray.SetRow(0, m_cameraTransform.TransformVector(m_frustumCorners[0]));
		m_frustumCornersArray.SetRow(2, m_cameraTransform.TransformVector(m_frustumCorners[1]));
		m_frustumCornersArray.SetRow(3, m_cameraTransform.TransformVector(m_frustumCorners[2]));
		m_frustumCornersArray.SetRow(1, m_cameraTransform.TransformVector(m_frustumCorners[3]));
		fogScatteringMaterial.SetMatrix("_FrustumCorners", m_frustumCornersArray);
		Graphics.Blit(source, destination, fogScatteringMaterial, 0);
		if (Application.isPlaying)
		{
			if (excludeLayers.value != 0)
			{
				m_camera = GetTmpCam();
			}
			if ((bool)m_camera && excludeLayers.value != 0)
			{
				m_camera.cullingMask = excludeLayers;
			}
		}
	}

	private Camera GetTmpCam()
	{
		if (m_camera == null)
		{
			m_camera = GetComponent<Camera>();
		}
		if (m_tmpCam == null)
		{
			string n = "Exclude Fog Scattering";
			Transform transform = base.transform.Find(n);
			if (transform == null)
			{
				GameObject gameObject = new GameObject(n, typeof(Camera));
				m_tmpCam = gameObject.GetComponent<Camera>();
				m_tmpCam.transform.SetParent(base.transform);
				Transform transform2 = m_tmpCam.transform;
				Vector3 localPosition = (m_tmpCam.transform.localEulerAngles = Vector3.zero);
				transform2.localPosition = localPosition;
				m_tmpCam.transform.localScale = Vector3.one;
				m_tmpCam.depthTextureMode = DepthTextureMode.None;
				m_tmpCam.CopyFrom(m_camera);
				m_tmpCam.clearFlags = clearFlags;
				m_tmpCam.depth = m_camera.depth + 1f;
				m_tmpCam.allowHDR = false;
				m_tmpCam.farClipPlane = farClipPlaneDistance;
			}
			else
			{
				m_tmpCam = transform.GetComponent<Camera>();
			}
		}
		return m_tmpCam;
	}
}
