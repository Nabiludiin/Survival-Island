using System;
using TMPro;
using UnityEngine;

public class Calendar : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro textMesh;

	private bool placed;

	private void Start()
	{
		if (textMesh.text != null)
		{
			textMesh.text = WorldManager.DayCounter.ToString();
		}
	}

	public void OnBlockPlaced()
	{
		placed = true;
		WorldManager.DayCounterChange = (Action<int>)Delegate.Combine(WorldManager.DayCounterChange, new Action<int>(OnDayChange));
	}

	private void OnDestroy()
	{
		if (placed)
		{
			WorldManager.DayCounterChange = (Action<int>)Delegate.Remove(WorldManager.DayCounterChange, new Action<int>(OnDayChange));
		}
	}

	public void OnDayChange(int dayCount)
	{
		if (textMesh != null)
		{
			textMesh.text = dayCount.ToString();
		}
	}
}
