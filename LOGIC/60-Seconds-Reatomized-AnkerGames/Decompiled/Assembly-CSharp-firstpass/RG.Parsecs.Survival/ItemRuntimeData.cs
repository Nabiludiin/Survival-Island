using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class ItemRuntimeData : IItemRuntimeData
{
	[SerializeField]
	private int _durability;

	[SerializeField]
	private bool _isDamaged;

	public int Durability
	{
		get
		{
			return _durability;
		}
		set
		{
			_durability = value;
			if (value <= 0)
			{
				if (_isDamaged)
				{
					_isDamaged = false;
					_isAvailable = false;
				}
				else
				{
					_isDamaged = true;
					_durability = 0;
				}
			}
			else if (_isDamaged)
			{
				_isDamaged = false;
			}
		}
	}

	public bool IsDamaged
	{
		get
		{
			return _isDamaged;
		}
		set
		{
			_isDamaged = value;
		}
	}
}
