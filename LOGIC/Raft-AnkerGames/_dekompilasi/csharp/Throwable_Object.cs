using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

public class Throwable_Object : MonoBehaviour
{
	public Action<Vector3, ParentType> OnCollision;

	public Action OnHitWater;

	public uint throwableIndex;

	[SerializeField]
	private Rigidbody body;

	[SerializeField]
	private Collider physicsCollider;

	[SerializeField]
	private bool disablePhysicsColliderOnCollision = true;

	[SerializeField]
	private bool disableRigidBodyOnCollision = true;

	[SerializeField]
	private bool rotateBodyWithVelocity = true;

	[SerializeField]
	private bool moveBodyWithTranslation = true;

	public bool hasLifeTime;

	public float lifeTime = 1f;

	public bool usePickup;

	public PickupItem_Networked pickupItem;

	public Collider pickupCollider;

	public bool useWaterCollision;

	[SerializeField]
	protected float waterDrag = 10f;

	[SerializeField]
	private WaterFloatSemih2 waterFloat;

	[SerializeField]
	private ParticleSystem underWaterParticles;

	public EventReference er_hitWaterSound;

	private IEnumerator lateWaterCoroutine;

	private bool hasBeenOverWater;

	protected bool underWater;

	private bool sink;

	public bool usesHitBoxOnClient;

	public EventReference er_hitSound;

	[SerializeField]
	private StudioEventEmitter eventEmitter_projectileFly;

	[SerializeField]
	private Interval_Float flyVelocityInterval;

	[SerializeField]
	private float flyVelocityMagnifier = 1f;

	protected ThrowableComponent throwableComponent;

	private float defaultDrag;

	protected bool hitSomething;

	public virtual void Initialize(ThrowableComponent throwableComponent, uint throwableIndex, uint throwablePickupIndex, Vector3 force, bool colliderEnabled)
	{
		if (!colliderEnabled)
		{
			colliderEnabled = usesHitBoxOnClient;
		}
		this.throwableComponent = throwableComponent;
		this.throwableIndex = throwableIndex;
		physicsCollider.enabled = colliderEnabled;
		defaultDrag = body.drag;
		body.AddForce(force);
		body.angularVelocity = new Vector3(UnityEngine.Random.Range(0f, 5f), UnityEngine.Random.Range(0f, 5f), UnityEngine.Random.Range(0f, 5f));
		if (hasLifeTime)
		{
			UnityEngine.Object.Destroy(gameObject, lifeTime);
		}
		if (usePickup)
		{
			pickupCollider.enabled = false;
			pickupItem.ObjectIndex = throwablePickupIndex;
			NetworkIDManager.AddNetworkID(pickupItem);
		}
	}

	protected virtual void Update()
	{
		HandleVelocitySound();
		if (useWaterCollision)
		{
			HandleWaterBehaviour();
		}
	}

	private void OnDestroy()
	{
		if (throwableComponent != null)
		{
			throwableComponent.RemoveThrownObject(this);
		}
		if (usePickup)
		{
			NetworkIDManager.RemoveNetworkID(pickupItem);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		Network_Entity_Redirect componentInParent = collision.collider.gameObject.GetComponentInParent<Network_Entity_Redirect>();
		Network_Entity network_Entity = null;
		if (componentInParent != null)
		{
			network_Entity = componentInParent.entity;
			if (network_Entity == null)
			{
				network_Entity = collision.collider.gameObject.GetComponentInParent<Network_Entity>();
			}
		}
		OnCollisionEvent(collision, network_Entity);
	}

	protected virtual void OnCollisionEvent(Collision collision, Network_Entity entity)
	{
		if (throwableComponent != null)
		{
			throwableComponent.OnThrowableCollision(this, collision, entity);
		}
	}

	public virtual void HandleCollision(Vector3 collisionPosition, Vector3 normal, ParentType parentType, Network_Entity entity)
	{
		if (!hitSomething)
		{
			hitSomething = true;
			if (disableRigidBodyOnCollision)
			{
				body.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
				body.isKinematic = true;
			}
			transform.position = collisionPosition;
			if (disablePhysicsColliderOnCollision)
			{
				physicsCollider.enabled = false;
			}
			if (usePickup && pickupCollider != null)
			{
				pickupCollider.enabled = true;
			}
			if (useWaterCollision && underWaterParticles != null)
			{
				underWaterParticles.StopSafe();
			}
			if (!underWater)
			{
				RuntimeManager.PlayOneShot(er_hitSound, collisionPosition);
			}
			if (OnCollision != null)
			{
				OnCollision(collisionPosition, parentType);
			}
		}
	}

	public void ReflectVelocity(Vector3 normalLenght)
	{
		body.velocity = Vector3.Reflect(body.velocity, normalLenght.normalized);
	}

	protected virtual void CallOnHitWater()
	{
		if (OnHitWater != null)
		{
			OnHitWater();
		}
	}

	private void HandleVelocitySound()
	{
		if (eventEmitter_projectileFly != null)
		{
			if (!body.useGravity || body.isKinematic)
			{
				eventEmitter_projectileFly.SetParameter("velocity", 0f);
			}
			else if (body.useGravity)
			{
				eventEmitter_projectileFly.SetParameter("velocity", Mathf.Clamp(body.velocity.magnitude * flyVelocityMagnifier, flyVelocityInterval.minValue, flyVelocityInterval.maxValue));
			}
		}
	}

	private void HandleWaterBehaviour()
	{
		if (disableRigidBodyOnCollision && hitSomething)
		{
			return;
		}
		if (sink)
		{
			SinkInWater();
		}
		else
		{
			if (!(body.velocity != Vector3.zero) || body.isKinematic)
			{
				return;
			}
			underWater = transform.position.y <= waterFloat.PrevYPos;
			if (underWater)
			{
				body.drag = waterDrag;
				if (moveBodyWithTranslation)
				{
					body.useGravity = false;
				}
				SinkInWater();
				if (lateWaterCoroutine == null)
				{
					lateWaterCoroutine = LaterWaterHit();
					StartCoroutine(lateWaterCoroutine);
					if (hasBeenOverWater)
					{
						if (OnHitWater != null)
						{
							OnHitWater();
						}
						ParticleManager.PlaySystem("WaterSplash_Hook", transform.position, Quaternion.LookRotation(Vector3.up));
						RuntimeManager.PlayOneShot(er_hitWaterSound, transform.position);
					}
				}
				transform.SetParentSafe(null);
				if (underWaterParticles != null)
				{
					underWaterParticles.PlaySafe();
				}
			}
			else
			{
				if (lateWaterCoroutine != null)
				{
					StopCoroutine(lateWaterCoroutine);
					lateWaterCoroutine = null;
				}
				body.drag = defaultDrag;
				if (moveBodyWithTranslation)
				{
					body.useGravity = true;
				}
				hasBeenOverWater = true;
				if (rotateBodyWithVelocity)
				{
					transform.rotation = Quaternion.LookRotation(body.velocity);
				}
				if (underWaterParticles != null)
				{
					underWaterParticles.StopSafe();
				}
			}
		}
	}

	private void SinkInWater()
	{
		if (moveBodyWithTranslation)
		{
			transform.position += Vector3.down * Time.deltaTime;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.down), Time.deltaTime * 0.3f);
		}
	}

	private IEnumerator LaterWaterHit()
	{
		yield return new WaitForSeconds(1f);
		body.isKinematic = true;
		if (usePickup && pickupCollider != null)
		{
			pickupCollider.enabled = true;
		}
		if (useWaterCollision)
		{
			sink = true;
			if (underWaterParticles != null)
			{
				underWaterParticles.StopSafe();
			}
		}
	}
}
