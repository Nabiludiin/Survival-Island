using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class SpinFree : MonoBehaviour
{
	[Tooltip("Spin: Yes or No")]
	public bool spin;

	[Tooltip("Spin the parent object instead of the object this script is attached to")]
	public bool spinParent;

	public float speed = 10f;

	[HideInInspector]
	public bool clockwise = true;

	[HideInInspector]
	public float direction = 1f;

	[HideInInspector]
	public float directionChangeSpeed = 2f;

	private void Update()
	{
		if (direction < 1f)
		{
			direction += Time.deltaTime / (directionChangeSpeed / 2f);
		}
		if (!spin)
		{
			return;
		}
		if (clockwise)
		{
			if (spinParent)
			{
				transform.parent.transform.Rotate(Vector3.up, speed * direction * Time.deltaTime);
			}
			else
			{
				transform.Rotate(Vector3.up, speed * direction * Time.deltaTime);
			}
		}
		else if (spinParent)
		{
			transform.parent.transform.Rotate(-Vector3.up, speed * direction * Time.deltaTime);
		}
		else
		{
			transform.Rotate(-Vector3.up, speed * direction * Time.deltaTime);
		}
	}
}
