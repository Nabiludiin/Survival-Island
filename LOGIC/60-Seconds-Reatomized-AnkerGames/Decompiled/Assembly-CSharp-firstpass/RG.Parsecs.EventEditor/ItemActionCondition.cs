using System.Collections.Generic;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[CreateAssetMenu(fileName = "New Item Action Condition", menuName = "60 Parsecs!/Event Editor/Action Conditions/New Item Action Condition")]
public class ItemActionCondition : BaseActionCondition
{
	[SerializeField]
	private List<IItem> _items;

	public override bool CanSelect()
	{
		bool flag = true;
		for (int i = 0; i < _items.Count; i++)
		{
			flag &= _items[i].IsLockable() && _items[i].BaseRuntimeData.IsAvailable;
		}
		return flag;
	}

	public override void OnSelect()
	{
		for (int i = 0; i < _items.Count; i++)
		{
			_items[i].Lock();
		}
	}

	public override void OnDeselect()
	{
		for (int i = 0; i < _items.Count; i++)
		{
			_items[i].Unlock();
		}
	}
}
