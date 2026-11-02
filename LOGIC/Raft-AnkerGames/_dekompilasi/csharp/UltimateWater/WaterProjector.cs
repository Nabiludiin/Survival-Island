using UltimateWater.Internal;
using UltimateWater.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater;

public class WaterProjector : MonoBehaviour, ILocalDiffuseRenderer, IDynamicWaterEffects, ILocalDisplacementRenderer
{
	public enum Type
	{
		Diffuse,
		Displacement
	}

	[SerializeField]
	private Type _Type;

	[SerializeField]
	private Material _Displacement;

	[SerializeField]
	private bool _UseChildrenRenderers;

	private Renderer[] renderers;

	[InspectorWarning("Warning", InspectorWarningAttribute.InfoType.Error)]
	[SerializeField]
	private string _Warning;

	public Renderer[] _Renderers
	{
		get
		{
			if (renderers != null)
			{
				return renderers;
			}
			if (_UseChildrenRenderers)
			{
				renderers = GetComponentsInChildren<Renderer>(includeInactive: true);
			}
			return renderers;
		}
		set
		{
			renderers = value;
		}
	}

	public bool IsVisible(WaterCamera camera)
	{
		if (_Type != Type.Diffuse)
		{
			return false;
		}
		if (camera.SubmersionState == SubmersionState.Full)
		{
			return false;
		}
		for (int i = 0; i < _Renderers.Length; i++)
		{
			if (Vector3.Distance(camera.CameraComponent.transform.position, _Renderers[i].bounds.center) >= 50f)
			{
				return false;
			}
		}
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera.CameraComponent);
		for (int j = 0; j < _Renderers.Length; j++)
		{
			if (GeometryUtility.TestPlanesAABB(planes, _Renderers[j].bounds))
			{
				return true;
			}
		}
		return false;
	}

	public void RenderLocalDiffuse(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
	{
		if (_Type != Type.Diffuse)
		{
			return;
		}
		for (int i = 0; i < _Renderers.Length; i++)
		{
			if (_Renderers[i] != null)
			{
				commandBuffer.DrawRenderer(_Renderers[i], _Renderers[i].sharedMaterial);
			}
		}
	}

	public void RenderLocalDisplacement(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
	{
		if (_Type == Type.Displacement && !(_Displacement == null))
		{
			for (int i = 0; i < _Renderers.Length; i++)
			{
				commandBuffer.DrawRenderer(_Renderers[i], _Displacement);
			}
		}
	}

	public void Enable()
	{
		if (_Renderers != null)
		{
			for (int i = 0; i < _Renderers.Length; i++)
			{
				_Renderers[i].enabled = true;
			}
		}
	}

	public void Disable()
	{
		for (int i = 0; i < _Renderers.Length; i++)
		{
			_Renderers[i].enabled = false;
		}
	}

	private void Awake()
	{
		if (_UseChildrenRenderers)
		{
			_Renderers = GetComponentsInChildren<Renderer>(includeInactive: true);
		}
		else
		{
			Renderer component = GetComponent<Renderer>();
			if (component != null)
			{
				_Renderers = new Renderer[1] { component };
			}
		}
		if (_Renderers == null)
		{
			Debug.LogError("[WaterProjector] : no renderers found");
			enabled = false;
			return;
		}
		for (int i = 0; i < _Renderers.Length; i++)
		{
			_Renderers[i].enabled = false;
		}
		if (_Type == Type.Displacement && _Displacement == null)
		{
			_Displacement = Resources.Load<Material>("Materials/Overlay (Displacements)");
		}
	}

	private void OnEnable()
	{
		Unregister();
		Register();
	}

	private void OnDisable()
	{
		Unregister();
	}

	private void OnDrawGizmos()
	{
		MeshFilter component = GetComponent<MeshFilter>();
		if (!(component == null) && !(component.sharedMesh == null))
		{
			Gizmos.color = new Color(0.5f, 0.2f, 1f);
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireMesh(component.sharedMesh);
		}
	}

	private void OnValidate()
	{
		if (!Application.isPlaying && _Type == Type.Displacement && _Displacement == null)
		{
			_Displacement = Resources.Load<Material>("Materials/Overlay (Displacements)");
		}
	}

	private void Register()
	{
		DynamicWater.AddRenderer(this);
	}

	private void Unregister()
	{
		DynamicWater.RemoveRenderer(this);
	}

	private string Warning()
	{
		bool flag = true;
		if (_UseChildrenRenderers)
		{
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
			if (componentsInChildren == null || componentsInChildren.Length == 0)
			{
				flag = false;
			}
		}
		else if (GetComponent<Renderer>() == null)
		{
			flag = false;
		}
		if (!flag)
		{
			return "error: no renderers found,\nadd renderer to GameObject or its children";
		}
		return string.Empty;
	}
}
