using System.Collections.Generic;
using UnityEngine;

public class Puzzle_ConnectionPoint : MonoBehaviour
{
	private bool _isConnected;

	[HideInInspector]
	public Puzzle_Connector connector;

	[SerializeField]
	private GizmoBox gizmoBox;

	[SerializeField]
	private ParticleSystem ps_leaking;

	[SerializeField]
	private LayerMask connectionLayer;

	[SerializeField]
	private StudioEventEmitterSustain emitter_waterLeak;

	public bool IsConnected
	{
		get
		{
			return _isConnected;
		}
		set
		{
			_isConnected = value;
		}
	}

	public void OnValidate()
	{
		if (gizmoBox == null)
		{
			gizmoBox = GetComponent<GizmoBox>();
		}
	}

	public Puzzle_ConnectionPoint GetOverlappingPoint()
	{
		List<Puzzle_ConnectionPoint> overlaps = gizmoBox.GetOverlaps<Puzzle_ConnectionPoint>(connectionLayer, QueryTriggerInteraction.Collide);
		if (overlaps != null && overlaps.Count > 0)
		{
			foreach (Puzzle_ConnectionPoint item in overlaps)
			{
				if (item != this)
				{
					return item;
				}
			}
		}
		return null;
	}

	public void RefreshVisualState()
	{
		if (ps_leaking == null)
		{
			return;
		}
		bool flag = false;
		Puzzle_ConnectionPoint puzzle_ConnectionPoint = ((this == connector.a) ? connector.b : connector.a);
		if (this != null && puzzle_ConnectionPoint != null && !IsConnected && puzzle_ConnectionPoint.IsConnected)
		{
			flag = true;
		}
		if (flag)
		{
			ps_leaking.Play();
		}
		else
		{
			ps_leaking.Stop();
		}
		if (emitter_waterLeak != null && emitter_waterLeak.eventEmitter != null)
		{
			if (IsConnected || !puzzle_ConnectionPoint.IsConnected)
			{
				emitter_waterLeak.Stop();
			}
			else
			{
				emitter_waterLeak.PlaySafe();
			}
		}
	}
}
