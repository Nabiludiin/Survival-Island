using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Puzzle_Connector : MonoBehaviour
{
	public enum Direction
	{
		NONE,
		ATOB,
		BTOA
	}

	private Direction _direction;

	protected bool _isConnected;

	[FormerlySerializedAs("from")]
	public Puzzle_ConnectionPoint a;

	[FormerlySerializedAs("to")]
	public Puzzle_ConnectionPoint b;

	[SerializeField]
	private Renderer renderer;

	[SerializeField]
	private float vertexOffsetMultiplier;

	[SerializeField]
	private bool reverse;

	private MaterialPropertyBlock mpb;

	public Direction _Direction => _direction;

	public bool IsConnected => _isConnected;

	protected virtual void Awake()
	{
		if (a != null)
		{
			a.connector = this;
		}
		if (b != null)
		{
			b.connector = this;
		}
		if (renderer != null)
		{
			if (mpb == null)
			{
				mpb = new MaterialPropertyBlock();
			}
			renderer.GetPropertyBlock(mpb);
			mpb.SetFloat("_VertexOffsetMultiplier", 0f);
			renderer.SetPropertyBlock(mpb);
		}
	}

	public void OnDrawGizmos()
	{
	}

	private void Button_FindFeedbackRenderer()
	{
		foreach (Transform item in transform)
		{
			Renderer component = item.GetComponent<Renderer>();
			if (component != null)
			{
				renderer = component;
				break;
			}
		}
	}

	public virtual void NewConnection(List<Puzzle_Connector> surgeLine, Puzzle_ConnectionPoint connectedPoint)
	{
		_isConnected = true;
		surgeLine.Add(this);
		if (connectedPoint == a)
		{
			_direction = Direction.ATOB;
		}
		else
		{
			_direction = Direction.BTOA;
		}
		if (a != null && b != null)
		{
			Puzzle_ConnectionPoint puzzle_ConnectionPoint = ((connectedPoint == a) ? b : a);
			Puzzle_ConnectionPoint overlappingPoint = puzzle_ConnectionPoint.GetOverlappingPoint();
			connectedPoint.IsConnected = true;
			if (overlappingPoint != null)
			{
				overlappingPoint.connector.NewConnection(surgeLine, overlappingPoint);
				puzzle_ConnectionPoint.IsConnected = true;
			}
			else
			{
				puzzle_ConnectionPoint.IsConnected = false;
			}
			RefreshVisualState();
		}
	}

	public virtual void RemoteDisconnect()
	{
		_direction = Direction.NONE;
		_isConnected = false;
		if (a != null)
		{
			a.IsConnected = false;
		}
		if (b != null)
		{
			b.IsConnected = false;
		}
		RefreshVisualState();
	}

	protected void RefreshVisualState()
	{
		if (a != null)
		{
			a.RefreshVisualState();
		}
		if (b != null)
		{
			b.RefreshVisualState();
		}
		if (renderer != null)
		{
			if (mpb == null)
			{
				mpb = new MaterialPropertyBlock();
			}
			renderer.GetPropertyBlock(mpb);
			float num = ((_direction == Direction.ATOB) ? 1f : 0f);
			if (reverse)
			{
				num = ((num == 0f) ? 1f : 0f);
			}
			mpb.SetFloat("_WaveDirection", num);
			mpb.SetFloat("_VertexOffsetMultiplier", IsConnected ? vertexOffsetMultiplier : 0f);
			renderer.SetPropertyBlock(mpb);
		}
	}
}
