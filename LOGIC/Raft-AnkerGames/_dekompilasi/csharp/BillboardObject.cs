using UnityEngine;

public class BillboardObject : MonoBehaviour
{
	[Range(0f, 100f)]
	[SerializeField]
	private float hideDistanceFromCamera;

	[SerializeField]
	private GameObject bilboardObject;

	[SerializeField]
	private Transform targetToLookAt;

	private void Update()
	{
		if (transform == null || targetToLookAt == null)
		{
			return;
		}
		transform.rotation = Quaternion.LookRotation(transform.position - targetToLookAt.position);
		if (hideDistanceFromCamera > 0f)
		{
			float num = Vector3.Distance(transform.position, targetToLookAt.position);
			if (bilboardObject.activeInHierarchy && num >= hideDistanceFromCamera)
			{
				bilboardObject.SetActive(value: false);
			}
			else if (!bilboardObject.activeInHierarchy && num < hideDistanceFromCamera)
			{
				bilboardObject.SetActive(value: true);
			}
		}
	}

	public void SetTarget(Transform targetToLookAt)
	{
		this.targetToLookAt = targetToLookAt;
	}
}
