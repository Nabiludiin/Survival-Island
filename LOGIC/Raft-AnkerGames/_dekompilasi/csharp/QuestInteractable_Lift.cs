using FMODUnity;
using UnityEngine;

public class QuestInteractable_Lift : QuestInteractable
{
	[SerializeField]
	private Transform lift;

	[SerializeField]
	private Transform pointA;

	[SerializeField]
	private Transform pointB;

	[SerializeField]
	private Animator[] weightAnimators;

	[SerializeField]
	private float speed;

	[SerializeField]
	private StudioEventEmitter emitter_Mechanism1;

	[SerializeField]
	private StudioEventEmitter emitter_Mechanism2;

	private float maxDistance;

	private bool isTravelling;

	private bool wasTravelling;

	protected override void Start()
	{
		base.Start();
		maxDistance = Vector3.Distance(pointA.position, pointB.position);
	}

	private void Update()
	{
		wasTravelling = isTravelling;
		Vector3 vector = ((CurrentObjectStateIndex == 0) ? pointA.position : pointB.position);
		float num = Vector3.Distance(vector, lift.position);
		isTravelling = num > 0.4f;
		if (isTravelling != wasTravelling)
		{
			if (isTravelling)
			{
				if (emitter_Mechanism1 != null)
				{
					emitter_Mechanism1.StopSafe();
					emitter_Mechanism1.PlaySafe();
				}
				if (emitter_Mechanism2 != null)
				{
					emitter_Mechanism2.StopSafe();
					emitter_Mechanism2.PlaySafe();
				}
			}
			else
			{
				if (emitter_Mechanism1 != null)
				{
					emitter_Mechanism1.EventInstance.keyOff();
				}
				if (emitter_Mechanism2 != null)
				{
					emitter_Mechanism2.EventInstance.keyOff();
				}
			}
		}
		lift.position = Vector3.MoveTowards(lift.position, vector, speed * Time.deltaTime);
		float num2 = Vector3.Distance(pointA.position, lift.position);
		Animator[] array = weightAnimators;
		foreach (Animator animator in array)
		{
			if (animator != null)
			{
				animator.SetFloat("Blend", Mathf.Clamp01(num2 / maxDistance));
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (pointA != null && pointB != null)
		{
			float radius = 0.1f;
			Gizmos.DrawSphere(pointA.position, radius);
			Gizmos.DrawSphere(pointB.position, radius);
			Gizmos.DrawLine(pointA.position, pointB.position);
		}
	}

	private void SetToA()
	{
		if (lift != null && pointA != null)
		{
			lift.position = pointA.position;
		}
	}

	private void SetToB()
	{
		if (lift != null && pointB != null)
		{
			lift.position = pointB.position;
		}
	}

	private void SetToCenter()
	{
		if (lift != null && pointA != null && pointB != null)
		{
			lift.position = pointA.position + (pointB.position - pointA.position) * 0.5f;
		}
	}
}
