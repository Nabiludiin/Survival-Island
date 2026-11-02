using UnityEngine;

public class RagdollPart : MonoBehaviour
{
	public Rigidbody body;

	public Collider collider;

	public bool IsValid
	{
		get
		{
			if (body != null)
			{
				return collider != null;
			}
			return false;
		}
	}

	public void FindComponents()
	{
		if (body == null)
		{
			body = GetComponent<Rigidbody>();
		}
		if (collider == null)
		{
			collider = GetComponent<Collider>();
		}
		if (collider == null)
		{
			collider = GetComponentInChildren<Collider>();
		}
	}

	public void AddChildCollider()
	{
		GameObject gameObject = new GameObject(transform.name + "_Ragdoll");
		gameObject.transform.SetParent(transform);
		gameObject.transform.SetAsFirstSibling();
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localEulerAngles = Vector3.zero;
		BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
		boxCollider.size = Vector3.one / 4f;
		collider = boxCollider;
	}

	private void OnEnable()
	{
		if (body == null)
		{
			body = GetComponent<Rigidbody>();
		}
		if (collider == null)
		{
			collider = GetComponent<Collider>();
		}
	}

	public void SetRagdollState(bool state)
	{
		if (body != null)
		{
			body.isKinematic = !state;
		}
		if (collider != null)
		{
			collider.enabled = state;
		}
	}

	public void SetGravity(bool state)
	{
		if (body != null)
		{
			body.velocity = Vector3.zero;
		}
	}
}
