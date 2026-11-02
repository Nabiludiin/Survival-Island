using UltimateWater.Internal;
using UnityEngine;

namespace UltimateWater;

public class FastWaterPhysics : MonoBehaviour
{
	[SerializeField]
	private Water _Water;

	[Tooltip("Adjust buoyancy proportionally, if your collider is bigger or smaller than the actual object. Lowering this may fix some weird behavior of objects with extremely low density like beach balls or baloons.")]
	[SerializeField]
	private float _BuoyancyIntensity = 1f;

	[Range(0f, 3f)]
	[Tooltip("Controls drag force. Determined experimentally in wind tunnels. Example values:\n https://en.wikipedia.org/wiki/Drag_coefficient#General")]
	[SerializeField]
	private float _DragCoefficient = 0.9f;

	[Tooltip("Horizontal flow force intensity.")]
	[SerializeField]
	private float _FlowIntensity = 1f;

	private Rigidbody _RigidBody;

	private Collider _LocalCollider;

	private WaterSample _Sample;

	private float _LastPositionX;

	private float _LastPositionZ;

	private Vector3 _BuoyancyPart;

	private float _DragPart;

	private float _FlowPart;

	private float _Volume;

	private float _Area;

	private bool _UseCheapDrag;

	private bool _UseCheapFlow;

	private static Ray _RayUp;

	private static Ray _RayDown;

	private void Awake()
	{
		_RayUp = new Ray(Vector3.zero, Vector3.up);
		_RayDown = new Ray(Vector3.zero, Vector3.down);
	}

	private void Start()
	{
		_RigidBody = GetComponentInParent<Rigidbody>();
		_LocalCollider = GetComponentInParent<Collider>();
		if (_Water == null)
		{
			_Water = ApplicationSingleton<WaterSystem>.Instance.BoundlessWaters[0];
		}
		OnValidate();
		Vector3 position = transform.position;
		_LastPositionX = position.x;
		_LastPositionZ = position.z;
		_Sample = new WaterSample(_Water, WaterSample.DisplacementMode.HeightAndForces);
		_Sample.Start(transform.position);
	}

	private void OnValidate()
	{
		if (_LocalCollider != null)
		{
			_Volume = _LocalCollider.ComputeVolume();
			_Area = _LocalCollider.ComputeArea();
		}
		if (_FlowIntensity < 0f)
		{
			_FlowIntensity = 0f;
		}
		if (_BuoyancyIntensity < 0f)
		{
			_BuoyancyIntensity = 0f;
		}
		if (_Water != null)
		{
			PrecomputeBuoyancy();
			PrecomputeDrag();
			PrecomputeFlow();
		}
	}

	private void FixedUpdate()
	{
		if (_RigidBody.isKinematic)
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
		float time = _Water.Time;
		Vector3 position2 = transform.position;
		_Sample.GetAndResetFast(position2.x, position2.z, time, out Vector3 result, out Vector3 forces);
		result.x += position.x - _LastPositionX;
		result.z += position.z - _LastPositionZ;
		float y3 = result.y;
		result.y = y - 20f;
		_RayUp.origin = result;
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
				return;
			}
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			Vector3 vector = _BuoyancyPart * num2;
			float num3 = num2 * 0.5f;
			result.y = y4 * (1f - num3) + y5 * num3;
			if (_UseCheapDrag)
			{
				Vector3 pointVelocity = _RigidBody.GetPointVelocity(result);
				Vector3 vector2 = pointVelocity + vector * num;
				Vector3 vector3 = default;
				vector3.x = ((vector2.x > 0f) ? ((0f - vector2.x) * vector2.x) : (vector2.x * vector2.x));
				vector3.y = ((vector2.y > 0f) ? ((0f - vector2.y) * vector2.y) : (vector2.y * vector2.y));
				vector3.z = ((vector2.z > 0f) ? ((0f - vector2.z) * vector2.z) : (vector2.z * vector2.z));
				Vector3 vector4 = vector3 * _DragPart;
				float num4 = vector4.magnitude * num;
				float num5 = num4 * num4;
				float num6 = Vector3.Dot(pointVelocity, pointVelocity);
				if (num5 > num6)
				{
					num2 *= Mathf.Sqrt(num6) / num4;
				}
				vector += vector4 * num2;
			}
			_RigidBody.AddForceAtPosition(vector, result, ForceMode.Force);
			if (_UseCheapFlow)
			{
				float num7 = Vector3.Dot(forces, forces);
				if (num7 != 0f)
				{
					num3 = -1f / num7;
					float num8 = Vector3.Dot(normal, forces) * num3 + 0.5f;
					if (num8 > 0f)
					{
						vector = forces * (num8 * _FlowPart);
						result.y = y4;
						_RigidBody.AddForceAtPosition(vector, result, ForceMode.Force);
					}
				}
			}
		}
		_LastPositionX = position.x;
		_LastPositionZ = position.z;
	}

	private void PrecomputeBuoyancy()
	{
		_BuoyancyPart = -Physics.gravity * (_Volume * _BuoyancyIntensity * _Water.Density);
	}

	private void PrecomputeDrag()
	{
		_UseCheapDrag = _DragCoefficient > 0f;
		_DragPart = 0.25f * _DragCoefficient * _Area * _Water.Density;
	}

	private void PrecomputeFlow()
	{
		_UseCheapFlow = _FlowIntensity > 0f;
		_FlowPart = _FlowIntensity * _DragCoefficient * _Area * 100f;
	}
}
