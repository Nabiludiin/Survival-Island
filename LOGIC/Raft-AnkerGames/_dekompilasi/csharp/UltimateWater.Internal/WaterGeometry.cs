using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater.Internal;

[Serializable]
public class WaterGeometry
{
	public enum Type
	{
		RadialGrid,
		ProjectionGrid,
		UniformGrid,
		CustomMeshes
	}

	[FormerlySerializedAs("type")]
	[Tooltip("Geometry type used for display.")]
	[SerializeField]
	private Type _Type;

	[FormerlySerializedAs("baseVertexCount")]
	[Tooltip("Water geometry vertex count at 1920x1080.")]
	[SerializeField]
	private int _BaseVertexCount = 500000;

	[Tooltip("Water geometry vertex count at 1920x1080 on systems with tessellation support. Set it a bit lower as tessellation will place additional, better distributed vertices in shader.")]
	[SerializeField]
	[FormerlySerializedAs("tesselatedBaseVertexCount")]
	private int _TesselatedBaseVertexCount = 16000;

	[SerializeField]
	[FormerlySerializedAs("adaptToResolution")]
	private bool _AdaptToResolution = true;

	[SerializeField]
	[FormerlySerializedAs("radialGrid")]
	private WaterRadialGrid _RadialGrid;

	[FormerlySerializedAs("projectionGrid")]
	[SerializeField]
	private WaterProjectionGrid _ProjectionGrid;

	[SerializeField]
	[FormerlySerializedAs("uniformGrid")]
	private WaterUniformGrid _UniformGrid;

	[SerializeField]
	[FormerlySerializedAs("customSurfaceMeshes")]
	private WaterCustomSurfaceMeshes _CustomSurfaceMeshes;

	private Water _Water;

	private Type _PreviousType;

	private int _PreviousTargetVertexCount;

	private int _ThisSystemVertexCount;

	private int _FrameCount;

	private readonly WaterQuadGeometry _QuadGeometry = new WaterQuadGeometry();

	public Type GeometryType => _Type;

	public int VertexCount => _BaseVertexCount;

	public int TesselatedBaseVertexCount => _TesselatedBaseVertexCount;

	public bool AdaptToResolution => _AdaptToResolution;

	public bool Triangular
	{
		get
		{
			if (_Type == Type.CustomMeshes)
			{
				return _CustomSurfaceMeshes.Triangular;
			}
			return false;
		}
	}

	public WaterCustomSurfaceMeshes CustomSurfaceMeshes => _CustomSurfaceMeshes;

	public Mesh[] GetMeshes(WaterGeometryType geometryType, int vertexCount, bool volume)
	{
		if (geometryType == WaterGeometryType.ProjectionGrid)
		{
			throw new InvalidOperationException("Projection grid needs camera to be retrieved. Use GetTransformedMeshes instead.");
		}
		if (_Type == Type.CustomMeshes)
		{
			geometryType = WaterGeometryType.Auto;
		}
		Matrix4x4 matrix;
		return geometryType switch
		{
			WaterGeometryType.Auto => _Type switch
			{
				Type.RadialGrid => _RadialGrid.GetTransformedMeshes(null, out matrix, vertexCount, volume), 
				Type.ProjectionGrid => _ProjectionGrid.GetTransformedMeshes(null, out matrix, vertexCount, volume), 
				Type.UniformGrid => _UniformGrid.GetTransformedMeshes(null, out matrix, vertexCount, volume), 
				Type.CustomMeshes => _CustomSurfaceMeshes.GetTransformedMeshes(null, out matrix, volume), 
				_ => throw new InvalidOperationException("Unknown water geometry type."), 
			}, 
			WaterGeometryType.RadialGrid => _RadialGrid.GetTransformedMeshes(null, out matrix, vertexCount, volume), 
			WaterGeometryType.ProjectionGrid => _ProjectionGrid.GetTransformedMeshes(null, out matrix, vertexCount, volume), 
			WaterGeometryType.UniformGrid => _UniformGrid.GetTransformedMeshes(null, out matrix, vertexCount, volume), 
			WaterGeometryType.QuadGeometry => _QuadGeometry.GetTransformedMeshes(null, out matrix, vertexCount, volume), 
			_ => throw new InvalidOperationException("Unknown water geometry type."), 
		};
	}

	public Mesh[] GetTransformedMeshes(Camera camera, out Matrix4x4 matrix, WaterGeometryType geometryType, bool volume, int vertexCount = 0)
	{
		if (vertexCount == 0)
		{
			vertexCount = ComputeVertexCountForCamera(camera);
		}
		if (_Type == Type.CustomMeshes)
		{
			geometryType = WaterGeometryType.Auto;
		}
		return geometryType switch
		{
			WaterGeometryType.Auto => _Type switch
			{
				Type.RadialGrid => _RadialGrid.GetTransformedMeshes(camera, out matrix, vertexCount, volume), 
				Type.ProjectionGrid => _ProjectionGrid.GetTransformedMeshes(camera, out matrix, vertexCount, volume), 
				Type.UniformGrid => _UniformGrid.GetTransformedMeshes(camera, out matrix, vertexCount, volume), 
				Type.CustomMeshes => _CustomSurfaceMeshes.GetTransformedMeshes(null, out matrix, volume), 
				_ => throw new InvalidOperationException("Unknown water geometry type."), 
			}, 
			WaterGeometryType.RadialGrid => _RadialGrid.GetTransformedMeshes(camera, out matrix, vertexCount, volume), 
			WaterGeometryType.ProjectionGrid => _ProjectionGrid.GetTransformedMeshes(camera, out matrix, vertexCount, volume), 
			WaterGeometryType.UniformGrid => _UniformGrid.GetTransformedMeshes(camera, out matrix, vertexCount, volume), 
			WaterGeometryType.QuadGeometry => _QuadGeometry.GetTransformedMeshes(camera, out matrix, vertexCount, volume), 
			_ => throw new InvalidOperationException("Unknown water geometry type."), 
		};
	}

	public int ComputeVertexCountForCamera(Camera camera)
	{
		if (!_AdaptToResolution)
		{
			return _ThisSystemVertexCount;
		}
		return (int)((float)_ThisSystemVertexCount * ((float)(camera.pixelWidth * camera.pixelHeight) / 2073600f) + 0.5f);
	}

	private void UpdateVertexCount()
	{
		_ThisSystemVertexCount = (SystemInfo.supportsComputeShaders ? Mathf.Min(_TesselatedBaseVertexCount, WaterQualitySettings.Instance.MaxTesselatedVertexCount) : Mathf.Min(_BaseVertexCount, WaterQualitySettings.Instance.MaxVertexCount));
	}

	internal void Awake(Water water)
	{
		_Water = water;
	}

	internal void OnEnable()
	{
		OnValidate();
		UpdateVertexCount();
		_RadialGrid.OnEnable(_Water);
		_ProjectionGrid.OnEnable(_Water);
		_UniformGrid.OnEnable(_Water);
		_CustomSurfaceMeshes.OnEnable(_Water);
	}

	internal void OnDisable()
	{
		if (_RadialGrid != null)
		{
			_RadialGrid.OnDisable();
		}
		if (_ProjectionGrid != null)
		{
			_ProjectionGrid.OnDisable();
		}
		if (_UniformGrid != null)
		{
			_UniformGrid.OnDisable();
		}
		if (_CustomSurfaceMeshes != null)
		{
			_CustomSurfaceMeshes.OnDisable();
		}
	}

	internal void OnValidate()
	{
		if (_RadialGrid == null)
		{
			_RadialGrid = new WaterRadialGrid();
		}
		if (_ProjectionGrid == null)
		{
			_ProjectionGrid = new WaterProjectionGrid();
		}
		if (_UniformGrid == null)
		{
			_UniformGrid = new WaterUniformGrid();
		}
		if (_CustomSurfaceMeshes == null)
		{
			_CustomSurfaceMeshes = new WaterCustomSurfaceMeshes();
		}
		if (_PreviousType != _Type)
		{
			switch (_PreviousType)
			{
			case Type.RadialGrid:
				_RadialGrid.RemoveFromMaterial(_Water);
				break;
			case Type.ProjectionGrid:
				_ProjectionGrid.RemoveFromMaterial(_Water);
				break;
			case Type.UniformGrid:
				_UniformGrid.RemoveFromMaterial(_Water);
				break;
			}
			switch (_Type)
			{
			case Type.RadialGrid:
				_RadialGrid.AddToMaterial(_Water);
				break;
			case Type.ProjectionGrid:
				_ProjectionGrid.AddToMaterial(_Water);
				break;
			case Type.UniformGrid:
				_UniformGrid.AddToMaterial(_Water);
				break;
			}
			_PreviousType = _Type;
		}
		UpdateVertexCount();
		if (_PreviousTargetVertexCount != _ThisSystemVertexCount)
		{
			_RadialGrid.Dispose();
			_UniformGrid.Dispose();
			_ProjectionGrid.Dispose();
			_PreviousTargetVertexCount = _ThisSystemVertexCount;
		}
	}

	internal void Update()
	{
		if (++_FrameCount > 8)
		{
			_FrameCount = 0;
		}
		switch (_FrameCount)
		{
		case 0:
			_RadialGrid.Update();
			break;
		case 3:
			_ProjectionGrid.Update();
			break;
		case 6:
			_UniformGrid.Update();
			break;
		}
	}
}
