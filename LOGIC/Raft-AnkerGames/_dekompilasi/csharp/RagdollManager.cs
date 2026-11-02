using System;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
	public Rigidbody mainRigidbody;

	public AI_NetworkBehaviour networkBehaviour;

	[SerializeField]
	private RagdollPart[] parts;

	private WaterFloatSemih_Rigidbody waterFloat;

	private Transform[] children;

	private Vector3[] _connectedAnchor;

	private Vector3[] _anchor;

	private void Start()
	{
		SetCorrectLayers();
	}

	private void OnDestroy()
	{
		if (waterFloat != null)
		{
			WaterFloatSemih_Rigidbody waterFloatSemih_Rigidbody = waterFloat;
			waterFloatSemih_Rigidbody.OnHitWater = (Action)Delegate.Remove(waterFloatSemih_Rigidbody.OnHitWater, new Action(OnHitWater));
		}
		if (networkBehaviour != null)
		{
			AI_NetworkBehaviour aI_NetworkBehaviour = networkBehaviour;
			aI_NetworkBehaviour.OnTransformScaleModified = (Action)Delegate.Remove(aI_NetworkBehaviour.OnTransformScaleModified, new Action(FixAnchors));
		}
	}

	private void OnValidate()
	{
		if (networkBehaviour == null)
		{
			networkBehaviour = GetComponent<AI_NetworkBehaviour>();
		}
	}

	private void Awake()
	{
		if (networkBehaviour == null)
		{
			return;
		}
		AI_NetworkBehaviour aI_NetworkBehaviour = networkBehaviour;
		aI_NetworkBehaviour.OnTransformScaleModified = (Action)Delegate.Combine(aI_NetworkBehaviour.OnTransformScaleModified, new Action(FixAnchors));
		children = transform.GetComponentsInChildren<Transform>();
		_connectedAnchor = new Vector3[children.Length];
		_anchor = new Vector3[children.Length];
		for (int i = 0; i < children.Length; i++)
		{
			if (children[i].GetComponent<Joint>() != null)
			{
				_connectedAnchor[i] = children[i].GetComponent<Joint>().connectedAnchor;
				_anchor[i] = children[i].GetComponent<Joint>().anchor;
			}
		}
	}

	private void OnHitWater()
	{
		RagdollPart[] array = parts;
		foreach (RagdollPart obj in array)
		{
			obj.body.velocity = Vector3.zero;
			obj.body.mass = 0.1f;
		}
		mainRigidbody.mass = 10f;
	}

	public void SetRagdollState(bool state)
	{
		if (mainRigidbody != null)
		{
			if (Raft_Network.IsHost)
			{
				mainRigidbody.isKinematic = !state;
			}
			else
			{
				mainRigidbody.isKinematic = true;
			}
		}
		if (parts == null)
		{
			return;
		}
		for (int i = 0; i < parts.Length; i++)
		{
			RagdollPart ragdollPart = parts[i];
			if (!(ragdollPart == null))
			{
				ragdollPart.SetRagdollState(state);
			}
		}
	}

	public void SetRagdollState(bool state, Vector3 force)
	{
		SetRagdollState(state);
		if (mainRigidbody != null)
		{
			mainRigidbody.AddForce(force, ForceMode.VelocityChange);
		}
	}

	public void AddForceToAllParts(float force)
	{
		RagdollPart[] array = parts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].body.AddForce(UnityEngine.Random.insideUnitSphere * force, ForceMode.VelocityChange);
		}
	}

	public void SetGamePlayReady()
	{
		if (mainRigidbody != null)
		{
			mainRigidbody.isKinematic = true;
		}
		if (parts != null)
		{
			for (int i = 0; i < parts.Length; i++)
			{
				RagdollPart ragdollPart = parts[i];
				if (!(ragdollPart == null))
				{
					if (ragdollPart.body != null)
					{
						ragdollPart.body.isKinematic = true;
						ragdollPart.body.useGravity = true;
					}
					if (ragdollPart.collider != null)
					{
						ragdollPart.collider.enabled = true;
					}
				}
			}
		}
		SetCorrectLayers();
	}

	private void SetCorrectLayers()
	{
		for (int i = 0; i < parts.Length; i++)
		{
			RagdollPart ragdollPart = parts[i];
			if (!(ragdollPart == null))
			{
				int layer = LayerMask.NameToLayer("Ragdoll");
				ragdollPart.gameObject.layer = layer;
				if (ragdollPart.collider != null)
				{
					ragdollPart.collider.gameObject.layer = layer;
				}
			}
		}
	}

	private void FixAnchors()
	{
		for (int i = 0; i < children.Length; i++)
		{
			if (children[i].GetComponent<Joint>() != null)
			{
				children[i].GetComponent<Joint>().connectedAnchor = _connectedAnchor[i];
				children[i].GetComponent<Joint>().anchor = _anchor[i];
			}
		}
	}

	public RagdollPart[] GetParts()
	{
		return parts;
	}

	public Rigidbody GetMainBody()
	{
		return mainRigidbody;
	}

	public void AttachWaterFloat(WaterFloatSemih_Rigidbody waterFloat)
	{
		this.waterFloat = waterFloat;
		WaterFloatSemih_Rigidbody waterFloatSemih_Rigidbody = this.waterFloat;
		waterFloatSemih_Rigidbody.OnHitWater = (Action)Delegate.Combine(waterFloatSemih_Rigidbody.OnHitWater, new Action(OnHitWater));
	}

	private void Button_FindParts()
	{
		parts = GetComponentsInChildren<RagdollPart>();
	}

	private void Button_ActivateRagdoll()
	{
		if (Application.isPlaying)
		{
			SetRagdollState(state: true);
		}
		else
		{
			Debug.Log("Can only activate ragdoll when game is running");
		}
	}

	private void Button_DisableRagdoll()
	{
		SetRagdollState(state: false);
	}

	private void Button_GamePlayReady()
	{
		SetGamePlayReady();
	}
}
