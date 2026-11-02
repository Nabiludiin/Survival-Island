using System;
using System.Collections.Generic;
using UltimateWater.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater;

[ExecuteInEditMode]
public abstract class WaterVolumeBase : MonoBehaviour
{
	[SerializeField]
	public Water _Water;

	[SerializeField]
	private WaterVolumeRenderMode _RenderMode = WaterVolumeRenderMode.Basic;

	[SerializeField]
	private bool _AffectPhysics = true;

	private Collider[] _Colliders;

	private static readonly Dictionary<Collider, WaterVolumeBase> _ColliderToVolumeCache = new Dictionary<Collider, WaterVolumeBase>();

	[InspectorWarning("Validation", InspectorWarningAttribute.InfoType.Warning)]
	[SerializeField]
	private string _Validation;

	public Water Water => _Water;

	public bool EnablePhysics => _AffectPhysics;

	public WaterVolumeRenderMode RenderMode => _RenderMode;

	public MeshRenderer[] VolumeRenderers { get; private set; }

	public MeshFilter[] VolumeFilters { get; private set; }

	protected virtual CullMode _CullMode => CullMode.Back;

	public void AssignTo(Water water)
	{
		if (water == null && SingletonGeneric<GameManager>.Singleton != null)
		{
			water = SingletonGeneric<GameManager>.Singleton.water;
		}
		if (!(_Water == water) && !(water == null))
		{
			DisposeRenderers();
			Unregister(water);
			_Water = water;
			Register(water);
			if (_RenderMode != WaterVolumeRenderMode.None && Application.isPlaying)
			{
				CreateRenderers();
			}
		}
	}

	public void EnableRenderers(bool forBorderRendering)
	{
		if (VolumeRenderers != null)
		{
			bool flag = !forBorderRendering && _Water.enabled;
			for (int i = 0; i < VolumeRenderers.Length; i++)
			{
				VolumeRenderers[i].enabled = flag;
			}
		}
	}

	public void DisableRenderers()
	{
		if (VolumeRenderers != null)
		{
			for (int i = 0; i < VolumeRenderers.Length; i++)
			{
				VolumeRenderers[i].enabled = false;
			}
		}
	}

	public bool IsPointInside(Vector3 point)
	{
		for (int i = 0; i < _Colliders.Length; i++)
		{
			if (_Colliders[i].IsPointInside(point))
			{
				return true;
			}
		}
		return false;
	}

	public static WaterVolumeBase GetWaterVolume<T>(Collider collider) where T : WaterVolumeBase
	{
		return GetWaterVolume(collider) as T;
	}

	public static WaterVolumeBase GetWaterVolume(Collider collider)
	{
		if (!_ColliderToVolumeCache.TryGetValue(collider, out var value))
		{
			value = collider.GetComponent<WaterVolumeBase>();
			if (!(value != null))
			{
				value = (_ColliderToVolumeCache[collider] = null);
			}
			else
			{
				_ColliderToVolumeCache[collider] = value;
			}
		}
		return value;
	}

	protected void OnEnable()
	{
		if (_Water == null && SingletonGeneric<GameManager>.Singleton != null)
		{
			_Water = SingletonGeneric<GameManager>.Singleton.water;
		}
		_Colliders = GetComponents<Collider>();
		gameObject.layer = WaterProjectSettings.Instance.WaterCollidersLayer;
		Register(_Water);
		if (_RenderMode != WaterVolumeRenderMode.None && _Water != null && Application.isPlaying)
		{
			CreateRenderers();
		}
	}

	protected void OnDisable()
	{
		DisposeRenderers();
		Unregister(_Water);
	}

	private void Update()
	{
		if (VolumeRenderers != null)
		{
			for (int i = 0; i < VolumeRenderers.Length; i++)
			{
				VolumeRenderers[i].SetPropertyBlock(_Water.Renderer.PropertyBlock);
			}
		}
	}

	protected void OnValidate()
	{
		_Colliders = GetComponents<Collider>();
		for (int i = 0; i < _Colliders.Length; i++)
		{
			if (!_Colliders[i].isTrigger)
			{
				_Colliders[i].isTrigger = true;
			}
		}
		if (_Water == null)
		{
			_Water = GetComponentInChildren<Water>();
		}
	}

	public void Reset()
	{
		if (_Water == null)
		{
			_Water = Utilities.GetWaterReference();
		}
	}

	protected abstract void Register(Water water);

	protected abstract void Unregister(Water water);

	internal void SetLayer(int layer)
	{
		if (VolumeRenderers != null)
		{
			for (int i = 0; i < VolumeRenderers.Length; i++)
			{
				VolumeRenderers[i].gameObject.layer = layer;
			}
		}
	}

	private void DisposeRenderers()
	{
		if (VolumeRenderers == null)
		{
			return;
		}
		for (int i = 0; i < VolumeRenderers.Length; i++)
		{
			if (VolumeRenderers[i] != null)
			{
				UnityEngine.Object.Destroy(VolumeRenderers[i].gameObject);
			}
		}
		VolumeRenderers = null;
		VolumeFilters = null;
	}

	protected virtual void CreateRenderers()
	{
		int num = _Colliders.Length;
		VolumeRenderers = new MeshRenderer[num];
		VolumeFilters = new MeshFilter[num];
		Material sharedMaterial = ((_CullMode == CullMode.Back) ? _Water.Materials.VolumeMaterial : _Water.Materials.VolumeBackMaterial);
		for (int i = 0; i < num; i++)
		{
			Collider collider = _Colliders[i];
			BoxCollider boxCollider;
			GameObject obj;
			SphereCollider sphereCollider;
			MeshCollider meshCollider;
			if ((boxCollider = collider as BoxCollider) != null)
			{
				HandleBoxCollider(out obj, boxCollider);
			}
			else if ((sphereCollider = collider as SphereCollider) != null)
			{
				HandleSphereCollider(out obj, sphereCollider);
			}
			else if ((meshCollider = collider as MeshCollider) != null)
			{
				HandleMeshCollider(out obj, meshCollider);
			}
			else
			{
				CapsuleCollider capsuleCollider;
				if (!((capsuleCollider = collider as CapsuleCollider) != null))
				{
					throw new InvalidOperationException("Unsupported collider type.");
				}
				HandleCapsuleCollider(out obj, capsuleCollider);
			}
			obj.hideFlags = HideFlags.DontSave;
			obj.name = "Volume Renderer";
			obj.layer = WaterProjectSettings.Instance.WaterLayer;
			obj.transform.SetParent(transform, worldPositionStays: false);
			UnityEngine.Object.Destroy(obj.GetComponent<Collider>());
			MeshRenderer component = obj.GetComponent<MeshRenderer>();
			component.sharedMaterial = sharedMaterial;
			component.shadowCastingMode = ShadowCastingMode.Off;
			component.receiveShadows = false;
			component.lightProbeUsage = LightProbeUsage.Off;
			component.enabled = true;
			component.SetPropertyBlock(_Water.Renderer.PropertyBlock);
			VolumeRenderers[i] = component;
			VolumeFilters[i] = component.GetComponent<MeshFilter>();
		}
	}

	private static void HandleBoxCollider(out GameObject obj, BoxCollider boxCollider)
	{
		obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		obj.transform.localScale = boxCollider.size;
		obj.transform.localPosition = boxCollider.center;
	}

	private void HandleMeshCollider(out GameObject obj, MeshCollider meshCollider)
	{
		Mesh sharedMesh = meshCollider.sharedMesh;
		if (sharedMesh == null)
		{
			throw new InvalidOperationException("MeshCollider used to mask water doesn't have a mesh assigned.");
		}
		obj = new GameObject
		{
			hideFlags = HideFlags.DontSave
		};
		obj.AddComponent<MeshFilter>().sharedMesh = sharedMesh;
		obj.AddComponent<MeshRenderer>();
	}

	private static void HandleSphereCollider(out GameObject obj, SphereCollider sphereCollider)
	{
		float num = sphereCollider.radius * 2f;
		obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		obj.transform.localScale = new Vector3(num, num, num);
		obj.transform.localPosition = sphereCollider.center;
	}

	private static void HandleCapsuleCollider(out GameObject obj, CapsuleCollider capsuleCollider)
	{
		float num = capsuleCollider.height * 0.5f;
		float num2 = capsuleCollider.radius * 2f;
		obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
		obj.transform.localPosition = capsuleCollider.center;
		switch (capsuleCollider.direction)
		{
		case 0:
			obj.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
			obj.transform.localScale = new Vector3(num, num2, num2);
			break;
		case 1:
			obj.transform.localScale = new Vector3(num2, num, num2);
			break;
		case 2:
			obj.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
			obj.transform.localScale = new Vector3(num2, num2, num);
			break;
		}
	}

	protected string Validation()
	{
		string text = string.Empty;
		if (_Water == null)
		{
			text += "warning: assign Water reference";
		}
		return text;
	}
}
