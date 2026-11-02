using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class ExpeditionDestinationDynamicData
{
	[SerializeField]
	private bool _enabled;

	[SerializeField]
	[Tooltip("Threats that have a chance to be found during this expedition.")]
	private List<ExpeditionThreat> _threats;

	[SerializeField]
	private List<IItem> _items;

	[SerializeField]
	private bool _visited;

	public bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			_enabled = value;
		}
	}

	public List<ExpeditionThreat> Threats
	{
		get
		{
			return _threats;
		}
		set
		{
			_threats = value;
		}
	}

	public bool Visited
	{
		get
		{
			return _visited;
		}
		set
		{
			_visited = value;
		}
	}

	public List<IItem> Items
	{
		get
		{
			return _items;
		}
		set
		{
			_items = value;
		}
	}
}
