using System;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Connector_Auto : Puzzle_Connector
{
	[SerializeField]
	private TimerEventer checkGizmoCollisionTimer;

	private List<Puzzle_Connector> surgeLine = new List<Puzzle_Connector>();

	private List<Puzzle_Connector> previousSurgeLine = new List<Puzzle_Connector>();

	protected override void Awake()
	{
		base.Awake();
		TimerEventer timerEventer = checkGizmoCollisionTimer;
		timerEventer.OnTimerReached = (Action)Delegate.Combine(timerEventer.OnTimerReached, new Action(StartSurge));
	}

	private void Update()
	{
		checkGizmoCollisionTimer.DoUpdate(Time.deltaTime, autoResetTimer: true);
	}

	public void StartSurge()
	{
		if (surgeLine != null && surgeLine.Count > 0)
		{
			foreach (Puzzle_Connector item in surgeLine)
			{
				previousSurgeLine.Add(item);
			}
			surgeLine.Clear();
		}
		Puzzle_ConnectionPoint overlappingPoint = a.GetOverlappingPoint();
		if (overlappingPoint != null)
		{
			if (a != null)
			{
				a.IsConnected = true;
			}
			if (b != null)
			{
				b.IsConnected = true;
			}
			overlappingPoint.connector.NewConnection(surgeLine, overlappingPoint);
		}
		else
		{
			if (a != null)
			{
				a.IsConnected = true;
			}
			if (b != null)
			{
				b.IsConnected = false;
			}
		}
		_isConnected = true;
		RefreshVisualState();
		foreach (Puzzle_Connector item2 in previousSurgeLine)
		{
			if (!surgeLine.Contains(item2))
			{
				item2.RemoteDisconnect();
			}
		}
		previousSurgeLine.Clear();
	}
}
