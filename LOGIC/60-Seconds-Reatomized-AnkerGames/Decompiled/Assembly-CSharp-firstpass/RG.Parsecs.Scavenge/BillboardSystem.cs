using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class BillboardSystem : MonoBehaviour
{
	private Camera _camera;

	private Transform _cameraTransform;

	private HashSet<Billboard> _billboards = new HashSet<Billboard>();

	public static bool Exist;

	public static BillboardSystem I { get; protected set; }

	protected void Awake()
	{
		if (I != null)
		{
			Debug.LogError("More than one BillboardSystem in scene.");
			Object.Destroy(this);
			return;
		}
		I = this;
		Exist = true;
		_camera = Camera.main;
		_cameraTransform = _camera.transform;
	}

	protected void OnDestroy()
	{
		if (I == this)
		{
			Exist = false;
			I = null;
		}
		_billboards.Clear();
	}

	public void RegisterBillboard(Billboard b)
	{
		_billboards.Add(b);
	}

	public void UnregiterBillboard(Billboard b)
	{
		_billboards.Remove(b);
	}

	private void Update()
	{
		foreach (Billboard billboard in _billboards)
		{
			if (billboard.amActive)
			{
				Transform containerTransform = billboard.ContainerTransform;
				containerTransform.LookAt(containerTransform.position + _cameraTransform.rotation * -Vector3.back, _cameraTransform.rotation * Vector3.up);
			}
			Vector3 eulerAngles = billboard.Transform.rotation.eulerAngles;
			billboard.Transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, billboard.TopTransform.rotation.eulerAngles.z);
		}
	}
}
