using System;
using UnityEngine;

public class WaterFloatSemih_Rigidbody : WaterFloatOxygenBounds
{
	public Action OnHitWater;

	private bool _isVelocityZero;

	[SerializeField]
	private bool active = true;

	[SerializeField]
	private Rigidbody body;

	[SerializeField]
	private float waterDrag = 3f;

	[HideInInspector]
	public float originalLerpSpeed;

	private bool cameFromAir;

	public bool HitWater { get; private set; }

	public bool IsVelocityZero
	{
		get
		{
			if (body == null)
			{
				return _isVelocityZero;
			}
			return body.velocity.sqrMagnitude < 0.1f;
		}
		set
		{
			if (body == null)
			{
				_isVelocityZero = value;
			}
		}
	}

	protected virtual bool CanBeSetToSurface => true;

	protected override void Awake()
	{
		base.Awake();
		originalLerpSpeed = lerpSpeed;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (active)
		{
			Activate();
		}
	}

	protected override void LateUpdate()
	{
		if (!active)
		{
			SetToSurface();
			return;
		}
		if (setToSurface && lerpSpeed != originalLerpSpeed)
		{
			float num = ((!cameFromAir) ? 0.1f : 2f);
			lerpSpeed = Mathf.MoveTowards(lerpSpeed, originalLerpSpeed, Time.deltaTime * num);
		}
		base.LateUpdate();
		if ((!(body == null) && body.isKinematic) || !(transform.position.y < PrevYPos - 0.1f))
		{
			return;
		}
		if (transform.parent != null)
		{
			transform.SetParent(null);
		}
		if (body != null)
		{
			if (body.useGravity)
			{
				body.useGravity = false;
				if (CanBeSetToSurface && OnHitWater != null)
				{
					OnHitWater();
				}
			}
			body.velocity = Vector3.Lerp(body.velocity, Vector3.zero, Time.deltaTime * waterDrag);
			if (body.velocity.sqrMagnitude < 0.1f && CanBeSetToSurface)
			{
				body.velocity = Vector3.zero;
				body.isKinematic = true;
				setToSurface = true;
			}
			return;
		}
		if (!HitWater)
		{
			HitWater = true;
			if (OnHitWater != null)
			{
				OnHitWater();
			}
		}
		if (IsVelocityZero && CanBeSetToSurface)
		{
			setToSurface = true;
		}
	}

	public bool IsAboveWaterLevel()
	{
		return transform.position.y > PrevYPos;
	}

	public bool IsActive()
	{
		return active;
	}

	public void Activate()
	{
		active = true;
		if (body != null)
		{
			body.isKinematic = false;
			body.useGravity = true;
		}
		setToSurface = false;
		cameFromAir = transform.position.y > -0.2f;
		HitWater = !cameFromAir;
		lerpSpeed = 0f;
	}

	public void DeActivate()
	{
		active = false;
		Reset();
	}

	public void Reset()
	{
		if (body != null)
		{
			body.isKinematic = true;
			body.useGravity = false;
		}
		setToSurface = false;
		cameFromAir = false;
		HitWater = false;
		IsVelocityZero = false;
		lerpSpeed = 0f;
	}

	public void SetBody(Rigidbody body)
	{
		this.body = body;
	}
}
