using System;
using System.Collections.Generic;
using RG.VirtualInput;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class SpaceshipConsumableShelfController : MonoBehaviour
{
	[SerializeField]
	[Header("Data")]
	[Tooltip("RationingData Scriptable Object reference.")]
	private RationingData _rationingData;

	[SerializeField]
	[Tooltip("EndOfDayListenerList Scriptable Object reference.")]
	private EndOfDayListenerList _endOfDayListenerList;

	[SerializeField]
	[Tooltip("Consumable Remedium that will be used to control this shelf.")]
	private ConsumableRemedium _consumable;

	[SerializeField]
	[Header("References")]
	[Tooltip("Reference to left shelf controller.")]
	private SpaceshipLeftShelfController _leftShelfController;

	[SerializeField]
	[Tooltip("Reference to representation of consumable objects.")]
	private List<GameObject> _consumablesRepresentations = new List<GameObject>();

	[SerializeField]
	[Tooltip("Reference to representation of object that says we have more consumables that visible.")]
	private GameObject _overflowConsumableRepresentation;

	[SerializeField]
	[Tooltip("Reference to representation of object that shows we have one open consumable.")]
	private GameObject _openConsumableRepresentation;

	[SerializeField]
	[Tooltip("How much soup is worth the overflow consumable.")]
	private int _overflowConsumableWorth;

	[SerializeField]
	[Tooltip("Reference to rationing manager on scene.")]
	private RationingManager _rationingManager;

	private int _currentAmount;

	private List<GameObject> _enabledConsumables = new List<GameObject>();

	private List<GameObject> _disabledConsumables = new List<GameObject>();

	[SerializeField]
	private VirtualInputButton _virtualInputButton;

	private const int SHELF_ONE_INDEX = 0;

	private const int SHELF_TWO_INDEX = 1;

	private const int SHELF_THREE_INDEX = 2;

	private void Awake()
	{
		_endOfDayListenerList.RegisterOnEndOfDay(OnEndOfDay, "Visuals", 0, this, forceOrder: true);
		DisableAllRepresentations();
	}

	private void Start()
	{
		RefreshConsumablesRepresentations();
	}

	private void OnDestroy()
	{
		_endOfDayListenerList.UnregisterOnEndOfDay(OnEndOfDay, "Visuals");
	}

	private void OnEndOfDay()
	{
		RefreshConsumablesRepresentations();
	}

	public void DisableAllRepresentations()
	{
		for (int i = 0; i < _consumablesRepresentations.Count; i++)
		{
			_consumablesRepresentations[i].SetActive(value: false);
			if (!_disabledConsumables.Contains(_consumablesRepresentations[i]))
			{
				_disabledConsumables.Add(_consumablesRepresentations[i]);
			}
		}
		_enabledConsumables.Clear();
		_currentAmount = 0;
	}

	public void RefreshConsumablesRepresentations()
	{
		int num = (int)_consumable.RuntimeData.Amount;
		RefreshOpenConsumableRepresentation((double)Math.Abs(_consumable.RuntimeData.Amount - (float)num) > 0.001);
		RefreshConsumablesRepresentations(num);
	}

	public void RefreshOpenConsumableRepresentation(bool visible)
	{
		if (_openConsumableRepresentation != null)
		{
			_openConsumableRepresentation.SetActive(visible);
		}
	}

	public void RefreshConsumablesRepresentations(int amount)
	{
		if (amount == 0)
		{
			DisableAllRepresentations();
			SetOpenShelves();
		}
		bool visibilityOfOverflowConsumableRepresentation = false;
		if (amount > _consumablesRepresentations.Count)
		{
			visibilityOfOverflowConsumableRepresentation = true;
			amount -= _overflowConsumableWorth;
		}
		SetVisibilityOfOverflowConsumableRepresentation(visibilityOfOverflowConsumableRepresentation);
		if (amount < 0 || _currentAmount == amount)
		{
			return;
		}
		_currentAmount = amount;
		int count = _enabledConsumables.Count;
		if (amount < count)
		{
			for (int i = 0; i < count - amount; i++)
			{
				DisableRandomConsumable();
			}
		}
		else if (amount > _enabledConsumables.Count)
		{
			for (int j = 0; j < amount - count; j++)
			{
				EnableRandomConsumable();
			}
		}
		SetOpenShelves();
	}

	private void SetVisibilityOfOverflowConsumableRepresentation(bool visible)
	{
		if (_overflowConsumableRepresentation != null)
		{
			_overflowConsumableRepresentation.SetActive(visible);
		}
	}

	private void EnableRandomConsumable()
	{
		if (_disabledConsumables.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, _disabledConsumables.Count);
			_disabledConsumables[index].SetActive(value: true);
			if (!_enabledConsumables.Contains(_disabledConsumables[index]))
			{
				_enabledConsumables.Add(_disabledConsumables[index]);
			}
			_disabledConsumables.RemoveAt(index);
		}
	}

	private void DisableRandomConsumable()
	{
		if (_enabledConsumables.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, _enabledConsumables.Count);
			if (!_disabledConsumables.Contains(_enabledConsumables[index]))
			{
				_disabledConsumables.Add(_enabledConsumables[index]);
			}
			_enabledConsumables[index].SetActive(value: false);
			_enabledConsumables.RemoveAt(index);
		}
	}

	private void SetOpenShelves()
	{
		if (_leftShelfController == null)
		{
			return;
		}
		_leftShelfController.SetShelf(0, closed: true);
		_leftShelfController.SetShelf(1, closed: true);
		_leftShelfController.SetShelf(2, closed: true);
		for (int i = 0; i < _consumablesRepresentations.Count; i++)
		{
			if (i < 3 && _consumablesRepresentations[i].activeSelf)
			{
				_leftShelfController.SetShelf(2, closed: false);
				i = 2;
			}
			if (i > 2 && i < 6 && _consumablesRepresentations[i].activeSelf)
			{
				_leftShelfController.SetShelf(1, closed: false);
				i = 5;
			}
			if (i > 5 && i < 9 && _consumablesRepresentations[i].activeSelf)
			{
				_leftShelfController.SetShelf(0, closed: false);
				break;
			}
		}
	}
}
