using System.Collections.Generic;
using System.Linq;
using RG.VirtualInput;
using UnityEngine;
using UnityEngine.UI;

public class NavigationOverrideButton : Button, INavigationMappable
{
	[SerializeField]
	private InputMappingTag _mappingTag;

	[HideInInspector]
	public List<Selectable> Down;

	[HideInInspector]
	public List<Selectable> Left;

	[HideInInspector]
	public List<Selectable> Up;

	[HideInInspector]
	public List<Selectable> Right;

	public InputMappingTag MappingTag => _mappingTag;

	private Selectable _getFirstAvailableFromList(List<Selectable> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			Selectable selectable = list[i];
			if (selectable == null || !selectable.enabled || !selectable.interactable || !selectable.gameObject.activeInHierarchy || !selectable.IsInteractable())
			{
				continue;
			}
			VirtualInputButton component = selectable.GetComponent<VirtualInputButton>();
			if (!(component != null) || component.IsInteractable)
			{
				NavigationMappingTemplate component2 = selectable.GetComponent<NavigationMappingTemplate>();
				if (!(component2 != null))
				{
					return selectable;
				}
				if (component2.CurrentlyMappedTarget != null)
				{
					return component2.CurrentlyMappedTarget;
				}
			}
		}
		return null;
	}

	public override Selectable FindSelectableOnDown()
	{
		return _getFirstAvailableFromList(Down) ?? base.FindSelectableOnDown();
	}

	public override Selectable FindSelectableOnLeft()
	{
		return _getFirstAvailableFromList(Left) ?? base.FindSelectableOnLeft();
	}

	public override Selectable FindSelectableOnUp()
	{
		return _getFirstAvailableFromList(Up) ?? base.FindSelectableOnUp();
	}

	public override Selectable FindSelectableOnRight()
	{
		return _getFirstAvailableFromList(Right) ?? base.FindSelectableOnRight();
	}

	public void AddNavigationMapping(List<Selectable> down, List<Selectable> left, List<Selectable> up, List<Selectable> right)
	{
		if (down != null)
		{
			Down.AddRange(down);
		}
		if (left != null)
		{
			Left.AddRange(left);
		}
		if (up != null)
		{
			Up.AddRange(up);
		}
		if (right != null)
		{
			Right.AddRange(right);
		}
	}

	[ContextMenu("Find selectables on left")]
	public void FindSelectablesOnLeft()
	{
		Left.Clear();
		List<Selectable> list = new List<Selectable>();
		Selectable[] array = Object.FindObjectsOfType<Selectable>();
		foreach (Selectable selectable in array)
		{
			if (!(selectable == this) && selectable.transform.position.x < transform.position.x)
			{
				list.Add(selectable);
			}
		}
		list = list.OrderBy((Selectable s) => Vector3.Distance(s.transform.position, transform.position)).ToList();
		Left.AddRange(list);
	}

	[ContextMenu("Find selectables on right")]
	public void FindSelectablesOnRight()
	{
		Right.Clear();
		List<Selectable> list = new List<Selectable>();
		Selectable[] array = Object.FindObjectsOfType<Selectable>();
		foreach (Selectable selectable in array)
		{
			if (!(selectable == this) && selectable.transform.position.x > transform.position.x)
			{
				list.Add(selectable);
			}
		}
		list = list.OrderBy((Selectable s) => Vector3.Distance(s.transform.position, transform.position)).ToList();
		Right.AddRange(list);
	}

	[ContextMenu("Find selectables on up")]
	public void FindSelectablesOnUp()
	{
		Up.Clear();
		List<Selectable> list = new List<Selectable>();
		Selectable[] array = Object.FindObjectsOfType<Selectable>();
		foreach (Selectable selectable in array)
		{
			if (!(selectable == this) && selectable.transform.position.y > transform.position.y)
			{
				list.Add(selectable);
			}
		}
		list = list.OrderBy((Selectable s) => Vector3.Distance(s.transform.position, transform.position)).ToList();
		Up.AddRange(list);
	}

	[ContextMenu("Find selectables on down")]
	public void FindSelectablesOnDown()
	{
		Down.Clear();
		List<Selectable> list = new List<Selectable>();
		Selectable[] array = Object.FindObjectsOfType<Selectable>();
		foreach (Selectable selectable in array)
		{
			if (!(selectable == this) && selectable.transform.position.y < transform.position.y)
			{
				list.Add(selectable);
			}
		}
		list = list.OrderBy((Selectable s) => Vector3.Distance(s.transform.position, transform.position)).ToList();
		Down.AddRange(list);
	}

	[ContextMenu("Add Journal Ribbon To Left")]
	public void AddJournalRibbonToLeft()
	{
		GameObject gameObject = GameObject.Find("Journal_Button_Hide");
		Left.Add(gameObject.GetComponent<Selectable>());
	}

	[ContextMenu("Add Journal Ribbon To Up")]
	public void AddJournalRibbonToUp()
	{
		GameObject gameObject = GameObject.Find("Journal_Button_Hide");
		Up.Add(gameObject.GetComponent<Selectable>());
	}

	[ContextMenu("Add Journal Open To Right")]
	public void AddJournalOpenToRight()
	{
		GameObject gameObject = GameObject.Find("ShowJournal_Button");
		Right.Add(gameObject.GetComponent<Selectable>());
	}

	[ContextMenu("Do All")]
	public void DoAll()
	{
		FindSelectablesOnDown();
		FindSelectablesOnRight();
		FindSelectablesOnUp();
		FindSelectablesOnLeft();
		AddJournalRibbonToLeft();
		AddJournalRibbonToUp();
		AddJournalOpenToRight();
	}
}
