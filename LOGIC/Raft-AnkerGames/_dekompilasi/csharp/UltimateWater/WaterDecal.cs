using UltimateWater.Internal;
using UltimateWater.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater;

public class WaterDecal : MonoBehaviour, ILocalDisplacementRenderer, IDynamicWaterEffects, ILocalFoamRenderer, ILocalDiffuseRenderer
{
	[Tooltip("Used for rendering water displacements")]
	[SerializeField]
	private Material _DisplacementMaterial;

	[Tooltip("Used for rendering foam")]
	[SerializeField]
	private Material _FoamMaterial;

	[SerializeField]
	private bool _RenderDiffuse;

	[SerializeField]
	private bool _UseChildrenRenderers;

	private Renderer[] _Renderers;

	[InspectorWarning("Validation", InspectorWarningAttribute.InfoType.Error)]
	[SerializeField]
	private int _Validation;

	public Material DisplacementMaterial
	{
		get
		{
			return _DisplacementMaterial;
		}
		set
		{
			Material displacementMaterial = _DisplacementMaterial;
			_DisplacementMaterial = value;
			if (displacementMaterial == null && _DisplacementMaterial != null)
			{
				DynamicWater.AddRenderer((ILocalDisplacementRenderer)this);
			}
			if (displacementMaterial != null && _DisplacementMaterial == null)
			{
				DynamicWater.RemoveRenderer((ILocalDisplacementRenderer)this);
			}
		}
	}

	public Material FoamMaterial
	{
		get
		{
			return _FoamMaterial;
		}
		set
		{
			Material foamMaterial = _FoamMaterial;
			_FoamMaterial = value;
			if (foamMaterial == null && _FoamMaterial != null)
			{
				DynamicWater.AddRenderer((ILocalFoamRenderer)this);
			}
			if (foamMaterial != null && _FoamMaterial == null)
			{
				DynamicWater.RemoveRenderer((ILocalFoamRenderer)this);
			}
		}
	}

	public bool RenderDiffuse
	{
		get
		{
			return _RenderDiffuse;
		}
		set
		{
			bool renderDiffuse = _RenderDiffuse;
			_RenderDiffuse = value;
			if (!renderDiffuse && _RenderDiffuse)
			{
				DynamicWater.AddRenderer((ILocalFoamRenderer)this);
			}
			if (renderDiffuse && !_RenderDiffuse)
			{
				DynamicWater.RemoveRenderer((ILocalFoamRenderer)this);
			}
		}
	}

	public void RenderLocalDisplacement(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
	{
		for (int i = 0; i < _Renderers.Length; i++)
		{
			commandBuffer.DrawRenderer(_Renderers[i], _DisplacementMaterial);
		}
	}

	public void RenderLocalFoam(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
	{
		for (int i = 0; i < _Renderers.Length; i++)
		{
			commandBuffer.DrawRenderer(_Renderers[i], _FoamMaterial);
		}
	}

	public bool IsVisible(WaterCamera camera)
	{
		return true;
	}

	public void RenderLocalDiffuse(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
	{
		for (int i = 0; i < _Renderers.Length; i++)
		{
			commandBuffer.DrawRenderer(_Renderers[i], _Renderers[i].material);
		}
	}

	public void Enable()
	{
		for (int i = 0; i < _Renderers.Length; i++)
		{
			_Renderers[i].enabled = true;
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
		_Renderers = (_UseChildrenRenderers ? GetComponentsInChildren<Renderer>() : new Renderer[1] { GetComponent<Renderer>() });
		for (int i = 0; i < _Renderers.Length; i++)
		{
			_Renderers[i].enabled = false;
		}
	}

	private void OnEnable()
	{
		if (_DisplacementMaterial != null)
		{
			DynamicWater.AddRenderer((ILocalDisplacementRenderer)this);
		}
		if (_FoamMaterial != null)
		{
			DynamicWater.AddRenderer((ILocalFoamRenderer)this);
		}
		if (_RenderDiffuse)
		{
			DynamicWater.AddRenderer((ILocalDiffuseRenderer)this);
		}
	}

	private void OnDisable()
	{
		if (_DisplacementMaterial != null)
		{
			DynamicWater.RemoveRenderer((ILocalDisplacementRenderer)this);
		}
		if (_FoamMaterial != null)
		{
			DynamicWater.RemoveRenderer((ILocalFoamRenderer)this);
		}
		if (_RenderDiffuse)
		{
			DynamicWater.RemoveRenderer((ILocalDiffuseRenderer)this);
		}
	}

	private void OnValidate()
	{
		DisplacementMaterial = _DisplacementMaterial;
		FoamMaterial = _FoamMaterial;
		RenderDiffuse = _RenderDiffuse;
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

	private string Validation()
	{
		if (_DisplacementMaterial == null && _FoamMaterial == null && !_RenderDiffuse)
		{
			return "Warnings:\n- Set at least one material";
		}
		return string.Empty;
	}
}
