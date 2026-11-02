using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class VisualsManager : MonoBehaviour
{
	private static VisualsManager _instance;

	private List<VisualsController> _controllers = new List<VisualsController>();

	[SerializeField]
	private EndOfDayListenerList _endOfDayListenerList;

	public static VisualsManager Instance
	{
		get
		{
			if (_instance == null)
			{
				FindInstanceInScene();
			}
			return _instance;
		}
	}

	private static void FindInstanceInScene()
	{
		_instance = Object.FindObjectOfType<VisualsManager>();
		if (_instance == null)
		{
			Debug.LogWarning("No VisualsManager in scene!");
		}
	}

	public void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else if (_instance != this)
		{
			Debug.LogFormat("Duplicate instance of TooltipTagManager detected! Removing {0}", gameObject.name);
			Object.Destroy(this);
		}
		if (_instance == this)
		{
			RefreshVisualsStateCoroutine();
			if (_endOfDayListenerList != null)
			{
				_endOfDayListenerList.RegisterOnEndOfDay(RefreshVisualsStateCoroutine, "Visuals", 8, this, forceOrder: true);
			}
		}
	}

	private void OnDestroy()
	{
		if (_instance == this && _endOfDayListenerList != null)
		{
			_endOfDayListenerList.UnregisterOnEndOfDay(RefreshVisualsStateCoroutine, "Visuals");
		}
	}

	public void RefreshVisualsStateCoroutine()
	{
		for (int i = 0; i < _controllers.Count; i++)
		{
			_controllers[i].RefreshVisualsState();
		}
	}

	public void RegisterVisualsController(VisualsController controller)
	{
		if (controller != null && !_controllers.Contains(controller))
		{
			_controllers.Add(controller);
		}
	}

	public void UnregisterVisualsController(VisualsController controller)
	{
		if (controller != null && _controllers.Contains(controller))
		{
			_controllers.Remove(controller);
		}
	}

	public void HideAllVisuals()
	{
		for (int i = 0; i < _controllers.Count; i++)
		{
			_controllers[i].HideVisuals();
		}
	}

	public void ResetAllVisuals()
	{
		for (int i = 0; i < _controllers.Count; i++)
		{
			_controllers[i].ResetVisualsData();
		}
	}

	public void UpdateAllVisuals()
	{
		for (int i = 0; i < _controllers.Count; i++)
		{
			_controllers[i].RefreshVisualsState();
		}
	}
}
