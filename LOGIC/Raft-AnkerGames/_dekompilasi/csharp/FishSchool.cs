using System;
using UnityEngine;

public class FishSchool : MonoBehaviour
{
	[Serializable]
	public class FishCon
	{
		public Mesh mesh;

		public Material material;
	}

	[SerializeField]
	private Transform windzone;

	[SerializeField]
	private Interval_Float newPositionInterval;

	[SerializeField]
	private float radius = 1f;

	[SerializeField]
	private float movementSpeed = 1f;

	[SerializeField]
	private ParticleSystem particleParent;

	[SerializeField]
	private FishCon[] fishCons;

	[SerializeField]
	private bool clampY;

	[SerializeField]
	private float clampedYPos = -1f;

	private Vector3 targetPosition = Vector3.zero;

	private float timer;

	private float currentInterval;

	private bool initialized;

	public Vector3 Center
	{
		get
		{
			Vector3 pos = transform.position;
			Clamp(ref pos);
			return pos;
		}
	}

	private void Awake()
	{
		if (fishCons != null && fishCons.Length != 0)
		{
			ParticleSystemRenderer component = particleParent.GetComponent<ParticleSystemRenderer>();
			if (component != null)
			{
				FishCon fishCon = fishCons[UnityEngine.Random.Range(0, fishCons.Length)];
				component.mesh = fishCon.mesh;
				component.sharedMaterial = fishCon.material;
			}
		}
	}

	private void OnEnable()
	{
		Invoke("LateEnable", 2f);
	}

	private void LateEnable()
	{
		initialized = true;
		targetPosition = GetNewPosition();
		windzone.transform.position = targetPosition;
		currentInterval = newPositionInterval.GetRandomValue();
		timer = 0f;
		if (particleParent != null)
		{
			particleParent.Stop();
			particleParent.Play();
		}
	}

	private void OnDisable()
	{
		initialized = false;
	}

	private void Update()
	{
		if (initialized)
		{
			timer += Time.deltaTime;
			if (timer >= currentInterval)
			{
				timer = 0f;
				currentInterval = newPositionInterval.GetRandomValue();
				targetPosition = GetNewPosition();
			}
			windzone.position = Vector3.Lerp(windzone.position, targetPosition, Time.deltaTime * movementSpeed);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(Center, radius);
		if (Application.isPlaying)
		{
			Gizmos.DrawLine(Center, targetPosition);
		}
	}

	private Vector3 GetNewPosition()
	{
		Vector3 pos = Center + UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(0.1f, radius);
		Clamp(ref pos);
		return pos;
	}

	private void Clamp(ref Vector3 pos)
	{
		if (clampY && pos.y > clampedYPos)
		{
			pos.y = clampedYPos;
		}
	}
}
