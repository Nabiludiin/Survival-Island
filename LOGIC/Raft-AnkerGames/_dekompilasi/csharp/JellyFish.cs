using System;
using UnityEngine;

public class JellyFish : MonoBehaviour
{
	public bool scared;

	[SerializeField]
	private float pointPositionNormalized;

	[SerializeField]
	private Transform pointA;

	[SerializeField]
	private Transform pointB;

	[SerializeField]
	private Transform pointScared;

	[SerializeField]
	private float movementSpeed = 5f;

	[SerializeField]
	private float pingpongSpeed = 5f;

	[SerializeField]
	private ParticleSystem particles_Attack;

	[SerializeField]
	private DamageBox damageBox;

	private float randomOffset;

	private void Awake()
	{
		randomOffset = UnityEngine.Random.Range(0f, 50f);
		movementSpeed *= UnityEngine.Random.Range(0.9f, 1.1f);
		DamageBox damageBox = this.damageBox;
		damageBox.OnDamage = (Action<Network_Player>)Delegate.Combine(damageBox.OnDamage, new Action<Network_Player>(OnDamageBoxDamage));
	}

	private void Update()
	{
		Vector3 target = pointA.position;
		pointPositionNormalized = Helper.MapValue(-1f, 1f, Mathf.Sin((Time.time + randomOffset) * pingpongSpeed), 0f, 1f);
		if (scared)
		{
			target = pointScared.position + Vector3.up * pointPositionNormalized;
		}
		else
		{
			Vector3.Lerp(pointA.position, pointB.position, pointPositionNormalized);
		}
		transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * movementSpeed);
	}

	private void OnDrawGizmos()
	{
		if (pointA != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(pointA.position, 0.1f);
		}
		if (pointB != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(pointB.position, 0.1f);
		}
		if (pointScared != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(pointScared.position, 0.1f);
			if (pointA != null)
			{
				Gizmos.DrawLine(pointA.position, pointScared.position);
			}
			if (pointB != null)
			{
				Gizmos.DrawLine(pointB.position, pointScared.position);
			}
		}
		if (pointA != null && pointB != null)
		{
			Gizmos.color = Color.white;
			Gizmos.DrawLine(pointA.position, pointB.position);
			Gizmos.DrawSphere(Vector3.Lerp(pointA.position, pointB.position, pointPositionNormalized), 0.1f);
		}
	}

	private void OnDamageBoxDamage(Network_Player player)
	{
		if (particles_Attack != null)
		{
			particles_Attack.Play();
		}
	}
}
