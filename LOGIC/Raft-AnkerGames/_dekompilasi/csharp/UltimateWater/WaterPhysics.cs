using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

[AddComponentMenu("Ultimate Water/Water Physics")]
public sealed class WaterPhysics : MonoBehaviour
{
	[Tooltip("Controls precision of the simulation. Keep it low (1 - 2) for small and not important objects. Prefer high values (15 - 30) for ships etc.")]
	[Range(1f, 30f)]
	[SerializeField]
	[FormerlySerializedAs("sampleCount")]
	private int _SampleCount = 20;

	[Tooltip("Controls drag force. Determined experimentally in wind tunnels. Example values:\n https://en.wikipedia.org/wiki/Drag_coefficient#General")]
	[Range(0f, 6f)]
	[FormerlySerializedAs("dragCoefficient")]
	[SerializeField]
	private float _DragCoefficient = 0.9f;

	[Range(0.125f, 1f)]
	[Tooltip("Determines how many waves will be used in computations. Set it low for big objects, larger than most of the waves. Set it high for smaller objects of size comparable to many waves.")]
	[SerializeField]
	[FormerlySerializedAs("precision")]
	private float _Precision = 0.5f;

	[Tooltip("Adjust buoyancy proportionally, if your collider is bigger or smaller than the actual object. Lowering this may fix some weird behaviour of objects with extremely low density like beach balls or baloons.")]
	[SerializeField]
	[FormerlySerializedAs("buoyancyIntensity")]
	[Range(0.1f, 10f)]
	private float _BuoyancyIntensity = 1f;

	[Tooltip("Horizontal flow force intensity.")]
	[SerializeField]
	[FormerlySerializedAs("flowIntensity")]
	private float _FlowIntensity = 1f;

	[Tooltip("Temporarily supports only mesh colliders.")]
	[SerializeField]
	[FormerlySerializedAs("useImprovedDragAndFlowForces")]
	private bool _UseImprovedDragAndFlowForces;

	private Vector3[] _CachedSamplePositions;

	private int _CachedSampleIndex;

	private int _CachedSampleCount;

	private Collider _LocalCollider;

	private Rigidbody _RigidBody;

	private float _Volume;

	private float _Area = -1f;

	private float _TotalArea;

	private WaterSample[] _Samples;

	private float _NumSamplesInv;

	private Vector3 _BuoyancyPart;

	private Vector3 _ImprovedBuoyancyPart;

	private float _DragPart;

	private float _ImprovedDragPart;

	private float _FlowPart;

	private float _ImprovedFlowPart;

	private float _AverageWaterElevation;

	private bool _UseCheapDrag;

	private bool _UseCheapFlow;

	private Water _WaterOverride;

	private WaterVolumeProbe _WaterProbe;

	private float _LastPositionX;

	private float _LastPositionZ;

	private Vector3[] _DragNormals;

	private Vector3[] _DragCenters;

	private Vector3[] _DragVertices;

	private float[] _PolygonVolumes;

	private float[] _DragAreas;

	private WaterSample[] _ImprovedDragSamples;

	private static Ray _RayUp;

	private static Ray _RayDown;

	public Water AffectingWater
	{
		get
		{
			if ((object)_WaterProbe == null)
			{
				return _WaterOverride;
			}
			return _WaterProbe.CurrentWater;
		}
		set
		{
			bool flag = _WaterOverride == null;
			_WaterOverride = value;
			if (_WaterOverride == null)
			{
				if (!flag)
				{
					OnWaterLeave();
				}
				CreateWaterProbe();
			}
			else
			{
				DestroyWaterProbe();
				OnWaterLeave();
				OnWaterEnter();
			}
		}
	}

	public float BuoyancyIntensity
	{
		get
		{
			return _BuoyancyIntensity;
		}
		set
		{
			_BuoyancyIntensity = value;
			if (AffectingWater != null)
			{
				PrecomputeBuoyancy();
			}
		}
	}

	public float DragCoefficient
	{
		get
		{
			return _DragCoefficient;
		}
		set
		{
			_DragCoefficient = value;
			if (AffectingWater != null)
			{
				PrecomputeDrag();
			}
		}
	}

	public float FlowIntensity
	{
		get
		{
			return _FlowIntensity;
		}
		set
		{
			_FlowIntensity = value;
			if (AffectingWater != null)
			{
				PrecomputeFlow();
			}
		}
	}

	public float AverageWaterElevation => _AverageWaterElevation;

	public float Volume => _Volume;

	public float GetEquilibriumMass(float fluidDensity = 999.8f)
	{
		return _Volume * _BuoyancyIntensity * fluidDensity;
	}

	public float GetTotalBuoyancy(float fluidDensity = 999.8f)
	{
		return Physics.gravity.magnitude * _Volume * _BuoyancyIntensity * fluidDensity / _RigidBody.mass;
	}

	private void Awake()
	{
		_LocalCollider = GetComponent<Collider>();
		_RigidBody = GetComponent<Rigidbody>();
		_RayUp = new Ray(Vector3.zero, Vector3.up);
		_RayDown = new Ray(Vector3.zero, Vector3.down);
		if (!_LocalCollider.IsNullReference(this) && !_RigidBody.IsNullReference(this))
		{
			Vector3 position = transform.position;
			_LastPositionX = position.x;
			_LastPositionZ = position.z;
			OnValidate();
			PrecomputeSamples();
			if (_UseImprovedDragAndFlowForces)
			{
				PrecomputeImprovedDrag();
			}
		}
	}

	private void OnEnable()
	{
		if (_WaterOverride == null)
		{
			CreateWaterProbe();
		}
	}

	private void OnDisable()
	{
		DestroyWaterProbe();
		OnWaterLeave();
	}

	private void OnValidate()
	{
		_NumSamplesInv = 1f / (float)_SampleCount;
		if (_LocalCollider != null)
		{
			_Volume = _LocalCollider.ComputeVolume();
			_Area = _LocalCollider.ComputeArea();
			if (_TotalArea == 0f)
			{
				UpdateTotalArea();
			}
			if (_UseImprovedDragAndFlowForces && !(_LocalCollider is MeshCollider))
			{
				_UseImprovedDragAndFlowForces = false;
				Debug.LogErrorFormat("Improved drag force won't work colliders other than mesh colliders. '{0}' collider has a wrong type.", name);
			}
			if (_UseImprovedDragAndFlowForces && ((MeshCollider)_LocalCollider).sharedMesh.vertexCount > 3000)
			{
				_UseImprovedDragAndFlowForces = false;
				Mesh sharedMesh = ((MeshCollider)_LocalCollider).sharedMesh;
				Debug.LogErrorFormat("Improved drag force won't work with meshes that have more than 3000 vertices. '{0}' has {1} vertices.", sharedMesh.name, sharedMesh.vertexCount);
			}
		}
		_FlowIntensity = Mathf.Max(_FlowIntensity, 0f);
		_BuoyancyIntensity = Mathf.Max(_BuoyancyIntensity, 0f);
		if (AffectingWater != null)
		{
			PrecomputeBuoyancy();
			PrecomputeDrag();
			PrecomputeFlow();
		}
	}

	private void FixedUpdate()
	{
		if (_UseImprovedDragAndFlowForces)
		{
			ImprovedFixedUpdate();
		}
		else
		{
			SimpleFixedUpdate();
		}
	}

	private void Reset()
	{
		Rigidbody component = GetComponent<Rigidbody>();
		Collider component2 = GetComponent<Collider>();
		if (component == null && component2 != null)
		{
			gameObject.AddComponent<Rigidbody>().mass = GetEquilibriumMass();
		}
	}

	private void SimpleFixedUpdate()
	{
		Water affectingWater = AffectingWater;
		if ((object)affectingWater == null || _RigidBody.isKinematic)
		{
			return;
		}
		Bounds bounds = _LocalCollider.bounds;
		float y = bounds.min.y;
		float y2 = bounds.max.y;
		Vector3 position = transform.position;
		float maxDistance = y2 - y + 80f;
		float fixedDeltaTime = Time.fixedDeltaTime;
		float num = fixedDeltaTime * (1f - _RigidBody.drag * fixedDeltaTime) / _RigidBody.mass;
		float time = affectingWater.Time;
		_AverageWaterElevation = 0f;
		Vector3 vector4 = default;
		for (int i = 0; i < _SampleCount; i++)
		{
			Vector3 vector = transform.TransformPoint(_CachedSamplePositions[_CachedSampleIndex]);
			_Samples[i].GetAndResetFast(vector.x, vector.z, time, out Vector3 result, out Vector3 forces);
			result.x += position.x - _LastPositionX;
			result.z += position.z - _LastPositionZ;
			float y3 = result.y;
			result.y = y - 20f;
			_RayUp.origin = result;
			_AverageWaterElevation += y3;
			if (_LocalCollider.Raycast(_RayUp, out var hitInfo, maxDistance))
			{
				float y4 = hitInfo.point.y;
				Vector3 normal = hitInfo.normal;
				result.y = y2 + 20f;
				_RayDown.origin = result;
				_LocalCollider.Raycast(_RayDown, out hitInfo, maxDistance);
				float y5 = hitInfo.point.y;
				float num2 = (y3 - y4) / (y5 - y4);
				if (!(num2 > 0f))
				{
					continue;
				}
				if (num2 > 1f)
				{
					num2 = 1f;
				}
				Vector3 vector2 = _BuoyancyPart * num2;
				float num3 = num2 * 0.5f;
				result.y = y4 * (1f - num3) + y5 * num3;
				if (_UseCheapDrag)
				{
					Vector3 pointVelocity = _RigidBody.GetPointVelocity(result);
					Vector3 vector3 = pointVelocity + vector2 * num;
					vector4.x = ((vector3.x > 0f) ? ((0f - vector3.x) * vector3.x) : (vector3.x * vector3.x));
					vector4.y = ((vector3.y > 0f) ? ((0f - vector3.y) * vector3.y) : (vector3.y * vector3.y));
					vector4.z = ((vector3.z > 0f) ? ((0f - vector3.z) * vector3.z) : (vector3.z * vector3.z));
					Vector3 vector5 = vector4 * _DragPart;
					float num4 = vector5.magnitude * num;
					float num5 = num4 * num4;
					float num6 = Vector3.Dot(pointVelocity, pointVelocity);
					if (num5 > num6)
					{
						num2 *= Mathf.Sqrt(num6) / num4;
					}
					vector2 += vector5 * num2;
				}
				_RigidBody.AddForceAtPosition(vector2, result, ForceMode.Force);
				if (_UseCheapFlow)
				{
					float num7 = Vector3.Dot(forces, forces);
					if (num7 != 0f)
					{
						num3 = -1f / num7;
						float num8 = Vector3.Dot(normal, forces) * num3 + 0.5f;
						if (num8 > 0f)
						{
							vector2 = forces * (num8 * _FlowPart);
							result.y = y4;
							_RigidBody.AddForceAtPosition(vector2, result, ForceMode.Force);
						}
					}
				}
			}
			if (++_CachedSampleIndex >= _CachedSampleCount)
			{
				_CachedSampleIndex = 0;
			}
		}
		_AverageWaterElevation *= _NumSamplesInv;
		_LastPositionX = position.x;
		_LastPositionZ = position.z;
	}

	private void ImprovedFixedUpdate()
	{
		Water affectingWater = AffectingWater;
		if ((object)affectingWater == null || _RigidBody.isKinematic)
		{
			return;
		}
		float time = affectingWater.Time;
		float improvedDragPart = _ImprovedDragPart;
		Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;
		Vector4 row = localToWorldMatrix.GetRow(1);
		Vector3 center = _LocalCollider.bounds.center;
		_AverageWaterElevation = 0f;
		int num = 0;
		for (int i = 0; i < _DragNormals.Length; i++)
		{
			Vector3 vector = localToWorldMatrix.MultiplyPoint3x4(_DragCenters[i]);
			Vector3 pointVelocity = _RigidBody.GetPointVelocity(vector);
			Vector3 vector2 = localToWorldMatrix.MultiplyVector(_DragNormals[i]);
			_ImprovedDragSamples[i].GetAndResetFast(vector.x, vector.z, time, out float result, out Vector3 forces);
			_AverageWaterElevation += result;
			float num2 = Vector3.Dot(vector2, pointVelocity);
			float num3 = Vector3.Dot(forces, vector2) * _ImprovedFlowPart;
			float num10;
			if (num2 > 0f || num3 > 0f)
			{
				float num4 = SingleComponentTransform(ref _DragVertices[num++], ref row);
				float num5 = SingleComponentTransform(ref _DragVertices[num++], ref row);
				float num6 = SingleComponentTransform(ref _DragVertices[num++], ref row);
				float num7 = result - num4;
				float num8 = result - num5;
				float num9 = result - num6;
				if (num7 > 0f)
				{
					if (num8 > 0f)
					{
						num10 = ((num9 >= 0f) ? 1f : ((num7 + num8) / (num7 + num8 - num9)));
					}
					else
					{
						num10 = ((num9 >= 0f) ? ((num7 + num9) / (num7 - num8 + num9)) : (num7 / (num7 - num8 - num9)));
					}
				}
				else if (num8 > 0f)
				{
					num10 = ((num9 >= 0f) ? ((num8 + num9) / (num8 + num9 - num7)) : (num8 / (num8 - num9 - num7)));
				}
				else
				{
					num10 = ((num9 >= 0f) ? (num9 / (num9 - num7 - num8)) : 0f);
				}
				if (!(num10 > 0f) || !(num10 <= 1.02f))
				{
					num10 = 0f;
				}
			}
			else
			{
				num10 = 0f;
				num += 3;
			}
			float num11 = _DragAreas[i] * num10;
			float num12 = ((num2 > 0f) ? (improvedDragPart * num2 * num2 * num11) : 0f);
			float magnitude = pointVelocity.magnitude;
			num12 = ((magnitude != 0f) ? (num12 / magnitude) : 0f);
			Vector3 force = pointVelocity * num12;
			if (center.y > vector.y)
			{
				if (result > center.y)
				{
					num10 = _PolygonVolumes[i];
					force.x += _ImprovedBuoyancyPart.x * num10;
					force.y += _ImprovedBuoyancyPart.y * num10;
					force.z += _ImprovedBuoyancyPart.z * num10;
				}
				else if (result > vector.y)
				{
					num10 = _PolygonVolumes[i] * (result - vector.y) / (center.y - vector.y);
					force.x += _ImprovedBuoyancyPart.x * num10;
					force.y += _ImprovedBuoyancyPart.y * num10;
					force.z += _ImprovedBuoyancyPart.z * num10;
				}
			}
			else if (result > vector.y)
			{
				num10 = _PolygonVolumes[i];
				force.x += _ImprovedBuoyancyPart.x * num10;
				force.y += _ImprovedBuoyancyPart.y * num10;
				force.z += _ImprovedBuoyancyPart.z * num10;
			}
			else if (result > center.y)
			{
				num10 = _PolygonVolumes[i] * (result - center.y) / (vector.y - center.y);
				force.x += _ImprovedBuoyancyPart.x * num10;
				force.y += _ImprovedBuoyancyPart.y * num10;
				force.z += _ImprovedBuoyancyPart.z * num10;
			}
			if (num3 > 0f)
			{
				magnitude = forces.magnitude;
				float num13 = ((magnitude != 0f) ? (num3 * num11 / magnitude) : 0f);
				force.x += forces.x * num13;
				force.y += forces.y * num13;
				force.z += forces.z * num13;
			}
			_RigidBody.AddForceAtPosition(force, vector, ForceMode.Force);
		}
		_AverageWaterElevation /= _DragNormals.Length;
	}

	private static float SingleComponentTransform(ref Vector3 point, ref Vector4 row)
	{
		return point.x * row.x + point.y * row.y + point.z * row.z + row.w;
	}

	private void CreateWaterProbe()
	{
		if (_WaterProbe == null)
		{
			_WaterProbe = WaterVolumeProbe.CreateProbe(_RigidBody.transform, _LocalCollider.bounds.extents.magnitude);
			_WaterProbe.Enter.AddListener(OnWaterEnter);
			_WaterProbe.Leave.AddListener(OnWaterLeave);
		}
	}

	private void DestroyWaterProbe()
	{
		if (_WaterProbe != null)
		{
			_WaterProbe.gameObject.Destroy();
			_WaterProbe = null;
		}
	}

	private void OnWaterEnter()
	{
		CreateWaterSamplers();
		AffectingWater.ProfilesManager.ValidateProfiles();
		PrecomputeBuoyancy();
		PrecomputeDrag();
		PrecomputeFlow();
	}

	private void OnWaterLeave()
	{
		if (_Samples != null)
		{
			for (int i = 0; i < _SampleCount; i++)
			{
				_Samples[i].Stop();
			}
			_Samples = null;
		}
	}

	private bool ValidateForEditor()
	{
		if (_LocalCollider == null)
		{
			_LocalCollider = GetComponent<Collider>();
			_RigidBody = GetComponentInParent<Rigidbody>();
			OnValidate();
		}
		if (_LocalCollider != null)
		{
			return _RigidBody != null;
		}
		return false;
	}

	private void PrecomputeSamples()
	{
		List<Vector3> list = new List<Vector3>();
		float num = 0.5f;
		float num2 = 1f;
		int num3 = _SampleCount * 18;
		Transform transform = base.transform;
		ColliderExtensions.GetLocalMinMax(_LocalCollider, out var min, out var max);
		for (int i = 0; i < 4; i++)
		{
			if (list.Count >= num3)
			{
				break;
			}
			for (float num4 = num; num4 <= 1f; num4 += num2)
			{
				for (float num5 = num; num5 <= 1f; num5 += num2)
				{
					for (float num6 = num; num6 <= 1f; num6 += num2)
					{
						Vector3 vector = new Vector3(Mathf.Lerp(min.x, max.x, num4), Mathf.Lerp(min.y, max.y, num5), Mathf.Lerp(min.z, max.z, num6));
						if (_LocalCollider.IsPointInside(transform.TransformPoint(vector)))
						{
							list.Add(vector);
						}
					}
				}
			}
			num2 = num;
			num *= 0.5f;
		}
		_CachedSamplePositions = list.ToArray();
		_CachedSampleCount = _CachedSamplePositions.Length;
		Shuffle(_CachedSamplePositions);
	}

	private void PrecomputeImprovedDrag()
	{
		Mesh sharedMesh = ((MeshCollider)_LocalCollider).sharedMesh;
		Vector3[] vertices = sharedMesh.vertices;
		Vector3[] normals = sharedMesh.normals;
		int[] indices = sharedMesh.GetIndices(0);
		int num = indices.Length / 3;
		_DragNormals = new Vector3[num];
		_DragVertices = new Vector3[num * 3];
		_DragCenters = new Vector3[num];
		_DragAreas = new float[num];
		_PolygonVolumes = new float[num];
		Vector3 vector = _LocalCollider.transform.InverseTransformPoint(_LocalCollider.bounds.center);
		int num2 = 0;
		int num3 = 0;
		while (num3 < indices.Length)
		{
			Vector3 vector2 = vertices[indices[num3]];
			Vector3 vector3 = vertices[indices[num3 + 1]];
			Vector3 vector4 = vertices[indices[num3 + 2]];
			_DragVertices[num3] = vector2;
			_DragVertices[num3 + 1] = vector3;
			_DragVertices[num3 + 2] = vector4;
			_DragAreas[num2] = Vector3.Cross(vector3 - vector2, vector4 - vector2).magnitude * 0.5f;
			_DragCenters[num2] = (vector2 + vector3 + vector4) * (1f / 3f);
			Vector3 vector5 = normals[indices[num3++]];
			Vector3 vector6 = normals[indices[num3++]];
			Vector3 vector7 = normals[indices[num3++]];
			_DragNormals[num2] = (vector5 + vector6 + vector7) * (1f / 3f);
			Vector3 p = vector2 - vector;
			Vector3 p2 = vector3 - vector;
			Vector3 p3 = vector4 - vector;
			_PolygonVolumes[num2++] = Mathf.Abs(ColliderExtensions.SignedVolumeOfTriangle(p, p2, p3));
		}
		_ImprovedDragSamples = new WaterSample[num];
	}

	private void UpdateTotalArea()
	{
		Rigidbody componentInParent = GetComponentInParent<Rigidbody>();
		WaterPhysics[] componentsInChildren = componentInParent.GetComponentsInChildren<WaterPhysics>();
		_TotalArea = 0f;
		foreach (WaterPhysics waterPhysics in componentsInChildren)
		{
			if (!(waterPhysics.GetComponentInParent<Rigidbody>() != componentInParent))
			{
				if (waterPhysics._Area == -1f && waterPhysics._LocalCollider != null)
				{
					waterPhysics._Area = waterPhysics._LocalCollider.ComputeArea();
				}
				_TotalArea += waterPhysics._Area;
			}
		}
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j]._TotalArea = _TotalArea;
		}
	}

	private void CreateWaterSamplers()
	{
		Water affectingWater = AffectingWater;
		if (_UseImprovedDragAndFlowForces)
		{
			for (int i = 0; i < _ImprovedDragSamples.Length; i++)
			{
				_ImprovedDragSamples[i] = new WaterSample(affectingWater, WaterSample.DisplacementMode.HeightAndForces, _Precision);
				_ImprovedDragSamples[i].Start(transform.TransformPoint(_DragCenters[i]));
			}
			return;
		}
		if (_Samples == null || _Samples.Length != _SampleCount)
		{
			_Samples = new WaterSample[_SampleCount];
		}
		for (int j = 0; j < _SampleCount; j++)
		{
			_Samples[j] = new WaterSample(affectingWater, WaterSample.DisplacementMode.HeightAndForces, _Precision);
			_Samples[j].Start(transform.TransformPoint(_CachedSamplePositions[_CachedSampleIndex]));
			if (++_CachedSampleIndex >= _CachedSampleCount)
			{
				_CachedSampleIndex = 0;
			}
		}
	}

	private void PrecomputeBuoyancy()
	{
		_BuoyancyPart = -Physics.gravity * (_NumSamplesInv * _Volume * _BuoyancyIntensity * AffectingWater.Density);
		_ImprovedBuoyancyPart = -Physics.gravity * (_BuoyancyIntensity * AffectingWater.Density);
	}

	private void PrecomputeDrag()
	{
		_UseCheapDrag = _DragCoefficient > 0f && !_UseImprovedDragAndFlowForces;
		_DragPart = 0.5f * _DragCoefficient * _Area * _NumSamplesInv * AffectingWater.Density;
		_ImprovedDragPart = -0.5f * _DragCoefficient * AffectingWater.Density;
	}

	private void PrecomputeFlow()
	{
		_UseCheapFlow = _FlowIntensity > 0f && !_UseImprovedDragAndFlowForces;
		_FlowPart = _FlowIntensity * _DragCoefficient * _Area * _NumSamplesInv * 100f;
		_ImprovedFlowPart = _FlowIntensity * _DragCoefficient * -100f;
	}

	private static void Shuffle<T>(IList<T> array)
	{
		int count = array.Count;
		while (count > 1)
		{
			int index = Random.Range(0, count--);
			T value = array[count];
			array[count] = array[index];
			array[index] = value;
		}
	}
}
