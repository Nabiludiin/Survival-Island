using UnityEngine;

public class FollowTransform : MonoBehaviour
{
	[SerializeField]
	private Transform target;

	[SerializeField]
	private Vector3 offsetFromTarget = Vector3.zero;

	[SerializeField]
	private bool followPosition = true;

	[SerializeField]
	private bool followPositionY = true;

	[SerializeField]
	private bool followRotation;

	private void Update()
	{
		if (target == null)
		{
			return;
		}
		if (followPosition)
		{
			Vector3 position = target.position;
			if (!followPositionY)
			{
				position.y = transform.position.y;
			}
			Vector3 vector = position + offsetFromTarget;
			if (transform.position != vector)
			{
				SetPosition(vector);
			}
		}
		if (followRotation && transform.rotation != target.rotation)
		{
			transform.rotation = target.rotation;
		}
	}

	protected virtual void SetPosition(Vector3 position)
	{
		transform.position = position;
	}
}
