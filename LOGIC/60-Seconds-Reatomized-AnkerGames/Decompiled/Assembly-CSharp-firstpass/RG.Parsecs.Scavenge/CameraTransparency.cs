using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[RequireComponent(typeof(Camera))]
public class CameraTransparency : MonoBehaviour
{
	[SerializeField]
	private Transform _raycastOrigin;

	private Transform _target;

	private bool _inited;

	[SerializeField]
	private float _raycastLength;

	private Vector3 _direction;

	private RaycastHit[] rays = new RaycastHit[64];

	private Dictionary<Transform, ICameraTransparent> _cache = new Dictionary<Transform, ICameraTransparent>();

	public void SetTarget(Transform target)
	{
		_target = target;
		_inited = true;
	}

	private void Update()
	{
		if (!_inited || !ShipGenerator.I.TransparentWalls || _target == null)
		{
			return;
		}
		_direction = _target.position - _raycastOrigin.position;
		int num = Physics.RaycastNonAlloc(_raycastOrigin.position, _direction, rays, _raycastLength, -1, QueryTriggerInteraction.Collide);
		Debug.DrawRay(_raycastOrigin.position, _direction, Color.red);
		for (int i = 0; i < num; i++)
		{
			Transform transform = rays[i].transform;
			if (transform.CompareTag("Walls") || transform.CompareTag("Enviroment"))
			{
				GetTransparent(transform).SetTransparent();
			}
		}
	}

	private ICameraTransparent GetTransparent(Transform t)
	{
		ICameraTransparent value = null;
		if (!_cache.TryGetValue(t, out value))
		{
			value = t.GetComponent<ICameraTransparent>();
			_cache.Add(t, value);
		}
		return value;
	}
}
