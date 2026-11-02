using System.Collections;
using System.Collections.Generic;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/End Of Day Listener List")]
public class EndOfDayListenerList : ScriptableObject
{
	public delegate void OnEndOfDay();

	public delegate void OnDayStarted();

	public const string END_GAME_LAYER = "EndGame";

	public const string PRE_EVENTS_SYSTEM_LAYER = "PreEventSystems";

	public const string REPORT_EVENTS_LAYER = "ReportEvents";

	public const string UPDATE_LAYER = "Update";

	public const string SYSTEM_EVENTS_LAYER = "SystemEvents";

	public const string PREPARE_SYSTEM_LAYER = "PrepareSystem";

	public const string ACTION_EVENTS_LAYER = "ActionEvents";

	public const string GOAL_SYSTEM_LAYER = "Goals";

	public const string VISUALS_LAYER = "Visuals";

	public const string RESET_LAYER = "Reset";

	public const string CHANGE_FINISHED = "ChangeFinished";

	public SaveEvent SaveEvent;

	private readonly List<string> _layersOrder = new List<string> { "EndGame", "PreEventSystems", "ReportEvents", "Update", "SystemEvents", "Goals", "PrepareSystem", "ActionEvents", "Visuals", "Reset" };

	private readonly List<string> _dayStartedLayersOrder = new List<string> { "ChangeFinished" };

	private Dictionary<string, List<EndOfDayListener>> _eodListeners = new Dictionary<string, List<EndOfDayListener>>();

	private Dictionary<string, List<DayStartedListener>> _dayStartedListeners = new Dictionary<string, List<DayStartedListener>>();

	public void UpdateVisualLayer()
	{
		List<EndOfDayListener> list = _eodListeners["Visuals"];
		for (int i = 0; i < list.Count; i++)
		{
			list[i].Listener();
		}
	}

	private void OnEnable()
	{
		if (_eodListeners.Count == 0)
		{
			CreateListenerLayers();
		}
	}

	private void CreateListenerLayers()
	{
		for (int i = 0; i < _layersOrder.Count; i++)
		{
			_eodListeners.Add(_layersOrder[i], new List<EndOfDayListener>());
		}
		for (int j = 0; j < _dayStartedLayersOrder.Count; j++)
		{
			_dayStartedListeners.Add(_dayStartedLayersOrder[j], new List<DayStartedListener>());
		}
	}

	public IEnumerator NotifyEndOfDay()
	{
		for (int i = 0; i < _layersOrder.Count; i++)
		{
			List<EndOfDayListener> list = _eodListeners[_layersOrder[i]];
			for (int j = 0; j < list.Count; j++)
			{
				list[j].Listener();
			}
		}
		yield break;
	}

	public void NotifyDayStarted()
	{
		for (int i = 0; i < _dayStartedLayersOrder.Count; i++)
		{
			List<DayStartedListener> list = _dayStartedListeners[_dayStartedLayersOrder[i]];
			for (int j = 0; j < list.Count; j++)
			{
				list[j].Listener();
			}
		}
	}

	public void RegisterOnEndOfDay(OnEndOfDay method, string layer, int order, Object system, bool forceOrder = false)
	{
		if (_eodListeners.Count == 0)
		{
			CreateListenerLayers();
		}
		if (forceOrder || !IsMethodAlreadyRegistered(method, layer, order))
		{
			EndOfDayListener item = new EndOfDayListener
			{
				Listener = method,
				Order = order,
				SystemName = system.ToString()
			};
			_eodListeners[layer].Add(item);
			_eodListeners[layer].Sort();
		}
	}

	private bool IsMethodAlreadyRegistered(OnEndOfDay method, string layer, int order)
	{
		for (int i = 0; i < _eodListeners[layer].Count; i++)
		{
			EndOfDayListener endOfDayListener = _eodListeners[layer][i];
			if (endOfDayListener.Order == order)
			{
				Debug.LogErrorFormat("RegisterEndOfDay: There already is a method: {0} on layer {1} with order {2} but you're trying to register method {3} in the same order. This method will not be registered.", endOfDayListener.Listener, layer, order, method);
				return true;
			}
		}
		return false;
	}

	public void UnregisterOnEndOfDay(OnEndOfDay method, string layer)
	{
		_eodListeners[layer].RemoveAll((EndOfDayListener lis) => lis.Listener.Equals(method));
	}

	public void RegisterOnDayStarted(OnDayStarted method, string layer, int order)
	{
		if (_dayStartedListeners.Count == 0)
		{
			CreateListenerLayers();
		}
		DayStartedListener item = new DayStartedListener
		{
			Listener = method,
			Order = order
		};
		_dayStartedListeners[layer].Add(item);
		_dayStartedListeners[layer].Sort();
	}

	public void UnregisterOnDayStarted(OnDayStarted method, string layer)
	{
		_dayStartedListeners[layer].RemoveAll((DayStartedListener lis) => lis.Listener.Equals(method));
	}
}
