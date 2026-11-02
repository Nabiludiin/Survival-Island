using System;
using RG.Core.Base;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RG.Parsecs.Survival;

public class RationsView : RGMonoBehaviour
{
	[SerializeField]
	private CharacterList _characterList;

	[SerializeField]
	private RationingListenerList _rationingListenerList;

	[SerializeField]
	private RationingData _rationingData;

	[SerializeField]
	private int _characterIndex;

	[SerializeField]
	private RationingView _rationingView;

	[SerializeField]
	private RationVisual[] _rationVisuals;

	[SerializeField]
	private EndOfDayListenerList _endOfDayListenerList;

	private void OnMouseUpAsButton()
	{
		if (!EventSystem.current.IsPointerOverGameObject() && !_characterList.CharactersInGame[_characterIndex].RuntimeData.HasStatusPreventingRationing())
		{
			_rationingView.Show();
		}
	}

	protected override void CustomStart()
	{
		_endOfDayListenerList.RegisterOnEndOfDay(OnEndOfDay, "Visuals", 6, this);
		_rationingListenerList.Register(RefreshOnSceneDisplay);
	}

	private void OnDestroy()
	{
		_endOfDayListenerList.UnregisterOnEndOfDay(OnEndOfDay, "Visuals");
		_rationingListenerList.Unregister(RefreshOnSceneDisplay);
	}

	public void OnEndOfDay()
	{
		RefreshOnSceneDisplay();
	}

	public void RefreshOnSceneDisplay()
	{
		for (int i = 0; i < _rationVisuals.Length; i++)
		{
			_rationVisuals[i].OnShipSprite.SetActive(value: false);
		}
		for (int j = 0; j < _rationingData.Rations.Count; j++)
		{
			if (_rationingData.Rations[j].CharacterIndex == _characterIndex && HasVisualForRemedy(_rationingData.Rations[j].RationedItem.BaseStaticData.ItemId))
			{
				GetOnShipSpriteForRemedy(_rationingData.Rations[j].RationedItem.BaseStaticData.ItemId).SetActive(value: true);
			}
		}
	}

	private bool HasVisualForRemedy(string remedyItemID)
	{
		for (int i = 0; i < _rationVisuals.Length; i++)
		{
			if (_rationVisuals[i].Remedy.BaseStaticData.ItemId == remedyItemID)
			{
				return true;
			}
		}
		return false;
	}

	private GameObject GetOnShipSpriteForRemedy(string remedyItemID)
	{
		for (int i = 0; i < _rationVisuals.Length; i++)
		{
			if (_rationVisuals[i].Remedy.BaseStaticData.ItemId == remedyItemID)
			{
				return _rationVisuals[i].OnShipSprite;
			}
		}
		throw new NullReferenceException($"No visual for {remedyItemID} defined in RationingView {gameObject.name}!");
	}
}
