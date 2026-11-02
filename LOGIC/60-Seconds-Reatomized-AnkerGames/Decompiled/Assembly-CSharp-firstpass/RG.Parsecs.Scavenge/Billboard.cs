using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class Billboard : MonoBehaviour
{
	public bool amActive;

	public bool autoInit = true;

	private GameObject myContainer;

	[SerializeField]
	private Transform TopObject;

	public Transform Transform { get; set; }

	public Transform ContainerTransform { get; set; }

	public Transform TopTransform => TopObject;

	private void Awake()
	{
		if (autoInit)
		{
			amActive = true;
		}
		Transform = transform;
		myContainer = new GameObject();
		ContainerTransform = myContainer.transform;
		ContainerTransform.parent = Transform.parent;
		myContainer.name = "GRP_" + Transform.gameObject.name;
		ContainerTransform.position = Transform.position;
		Transform.parent = ContainerTransform;
	}

	private void OnBecameVisible()
	{
		if (BillboardSystem.Exist)
		{
			BillboardSystem.I.RegisterBillboard(this);
		}
	}

	private void OnBecameInvisible()
	{
		if (BillboardSystem.Exist)
		{
			BillboardSystem.I.UnregiterBillboard(this);
		}
	}
}
