using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationMappingTemplate : MonoBehaviour
{
	[Serializable]
	public class NavigationMappingEntry
	{
		[SerializeField]
		public InputMappingTag Tag;

		[HideInInspector]
		public List<Selectable> Down;

		[HideInInspector]
		public List<Selectable> Left;

		[HideInInspector]
		public List<Selectable> Up;

		[HideInInspector]
		public List<Selectable> Right;
	}

	private Selectable _currentlyMappedTarget;

	public int MappingCount;

	[HideInInspector]
	public List<NavigationMappingEntry> Mappings = new List<NavigationMappingEntry>();

	public Selectable CurrentlyMappedTarget
	{
		get
		{
			return _currentlyMappedTarget;
		}
		set
		{
			_currentlyMappedTarget = value;
		}
	}

	private void OnValidate()
	{
		if (Mappings.Count == MappingCount)
		{
			return;
		}
		if (MappingCount > Mappings.Count)
		{
			int num = MappingCount - Mappings.Count;
			for (int i = 0; i < num; i++)
			{
				Mappings.Add(new NavigationMappingEntry());
			}
		}
		else
		{
			if (MappingCount >= Mappings.Count)
			{
				return;
			}
			int num2 = Mappings.Count - MappingCount;
			for (int j = 0; j < num2; j++)
			{
				if (Mappings.Count <= 0)
				{
					break;
				}
				Mappings.RemoveAt(Mappings.Count - 1);
			}
		}
	}

	public NavigationMappingEntry GetNavigationMappingEntry(InputMappingTag tag)
	{
		if (tag != null && Mappings != null && Mappings.Count > 0)
		{
			for (int i = 0; i < Mappings.Count; i++)
			{
				if (Mappings[i].Tag == tag)
				{
					return Mappings[i];
				}
			}
		}
		return null;
	}
}
