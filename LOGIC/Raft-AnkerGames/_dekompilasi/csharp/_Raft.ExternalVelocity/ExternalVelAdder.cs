using UnityEngine;

namespace _Raft.ExternalVelocity;

public class ExternalVelAdder : MonoBehaviour
{
	protected enum VelocityMode
	{
		Set,
		Add
	}

	protected enum ForceFallOffMode
	{
		Fixed,
		DistanceToBounds,
		DistanceToCenter
	}

	protected enum ClampSetting
	{
		None,
		Clamped
	}

	public string entityTag = "Player";

	[SerializeField]
	protected VelocityMode velocityMode = VelocityMode.Add;

	[SerializeField]
	private ForceFallOffMode forceFallOffMode;

	[SerializeField]
	private Vector3 fixedVelocity;

	[SerializeField]
	private ClampSetting velocityClamping;

	[SerializeField]
	private float clampedLenght = 5f;

	[SerializeField]
	private Vector3 forceMultiplier = Vector3.one;

	protected void AddVelocity(Collider col, VelocityMode velMode)
	{
		if (!col.CompareTag(entityTag))
		{
			return;
		}
		Network_Player component = col.GetComponent<Network_Player>();
		if (!(component != null))
		{
			return;
		}
		switch (velMode)
		{
		case VelocityMode.Set:
			component.PersonController.SetExternalVelocity(transform.InverseTransformDirection(CalculateForce(col, forceFallOffMode)));
			break;
		case VelocityMode.Add:
		{
			Vector3 vector = transform.InverseTransformDirection(CalculateForce(col, forceFallOffMode));
			if (velocityClamping == ClampSetting.Clamped)
			{
				float num = Vector3.ClampMagnitude(component.PersonController.externalVelocity + vector, clampedLenght).magnitude - component.PersonController.externalVelocity.magnitude;
				vector = vector.normalized * num;
			}
			component.PersonController.AddExternalVelocity(vector);
			break;
		}
		}
	}

	protected Vector3 CalculateForce(Collider col, ForceFallOffMode forcefallOffMode)
	{
		switch (forceFallOffMode)
		{
		case ForceFallOffMode.Fixed:
			return fixedVelocity;
		case ForceFallOffMode.DistanceToBounds:
		{
			Vector3 vector = col.ClosestPointOnBounds(transform.position);
			return Vector3.Scale((vector - transform.position).normalized, forceMultiplier) * Vector3.Distance(transform.position, vector);
		}
		case ForceFallOffMode.DistanceToCenter:
			return Vector3.Scale((col.transform.position - transform.position).normalized, forceMultiplier) * Vector3.Distance(transform.position, col.transform.position);
		default:
			return fixedVelocity;
		}
	}

	private bool IsForceFixed()
	{
		return forceFallOffMode == ForceFallOffMode.Fixed;
	}

	private bool IsClamped()
	{
		return velocityClamping == ClampSetting.Clamped;
	}
}
