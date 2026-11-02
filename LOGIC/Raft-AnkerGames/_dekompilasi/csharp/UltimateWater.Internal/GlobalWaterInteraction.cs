using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UltimateWater.Internal;

public class GlobalWaterInteraction : MonoBehaviour, ILocalDisplacementMaskRenderer, IDynamicWaterEffects
{
	[SerializeField]
	[FormerlySerializedAs("maskDisplayShader")]
	[HideInInspector]
	private Shader _MaskDisplayShader;

	[FormerlySerializedAs("intensityMask")]
	[SerializeField]
	private Texture2D _IntensityMask;

	[SerializeField]
	[FormerlySerializedAs("worldUnitsOffset")]
	private Vector2 _WorldUnitsOffset;

	[SerializeField]
	[FormerlySerializedAs("worldUnitsSize")]
	private Vector2 _WorldUnitsSize;

	private MeshRenderer _InteractionMaskRenderer;

	private Material _InteractionMaskMaterial;

	public Vector2 WorldUnitsOffset
	{
		get
		{
			return _WorldUnitsOffset;
		}
		set
		{
			_WorldUnitsOffset = value;
		}
	}

	public Vector2 WorldUnitsSize
	{
		get
		{
			return _WorldUnitsSize;
		}
		set
		{
			_WorldUnitsSize = value;
		}
	}

	public void RenderLocalMask(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
	{
		float farClipPlane = overlays.Camera.CameraComponent.farClipPlane;
		Vector3 position = overlays.Camera.transform.position;
		position.y = overlays.DynamicWater.Water.transform.position.y;
		_InteractionMaskRenderer.transform.position = position;
		_InteractionMaskRenderer.transform.localScale = new Vector3(farClipPlane, farClipPlane, farClipPlane);
		_InteractionMaskMaterial.SetVector("_OffsetScale", new Vector4(0.5f + _WorldUnitsOffset.x / _WorldUnitsSize.x, 0.5f + _WorldUnitsOffset.y / _WorldUnitsSize.y, 1f / _WorldUnitsSize.x, 1f / _WorldUnitsSize.y));
		commandBuffer.DrawMesh(_InteractionMaskRenderer.GetComponent<MeshFilter>().sharedMesh, _InteractionMaskRenderer.transform.localToWorldMatrix, _InteractionMaskMaterial, 0, 0);
	}

	public void Enable()
	{
		throw new NotImplementedException();
	}

	public void Disable()
	{
		throw new NotImplementedException();
	}

	private void Awake()
	{
		OnValidate();
		CreateMaskRenderer();
		gameObject.layer = WaterProjectSettings.Instance.WaterTempLayer;
	}

	private void OnEnable()
	{
		DynamicWater.AddRenderer(this);
	}

	private void OnDisable()
	{
		DynamicWater.RemoveRenderer(this);
	}

	private void OnValidate()
	{
		if (_MaskDisplayShader == null)
		{
			_MaskDisplayShader = Shader.Find("UltimateWater/Utility/ShorelineMaskRenderSimple");
		}
	}

	private void CreateMaskRenderer()
	{
		gameObject.AddComponent<MeshFilter>().sharedMesh = Quads.BipolarXZ;
		_InteractionMaskMaterial = new Material(_MaskDisplayShader)
		{
			hideFlags = HideFlags.DontSave
		};
		_InteractionMaskMaterial.SetTexture("_MainTex", _IntensityMask);
		_InteractionMaskRenderer = gameObject.AddComponent<MeshRenderer>();
		_InteractionMaskRenderer.sharedMaterial = _InteractionMaskMaterial;
		_InteractionMaskRenderer.enabled = false;
		transform.localRotation = Quaternion.identity;
	}
}
