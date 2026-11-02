using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NavigationOverrideToggle : Toggle, INavigationMappable
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

	public bool Togglable;

	public InputMappingTag MappingTag => _mappingTag;

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (Togglable)
		{
			base.OnPointerClick(eventData);
		}
	}

	public override void OnSubmit(BaseEventData eventData)
	{
		if (Togglable)
		{
			base.OnSubmit(eventData);
		}
	}

	private Selectable _getFirstAvailableFromList(List<Selectable> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			Selectable selectable = list[i];
			if (selectable.enabled && selectable.gameObject.activeInHierarchy && selectable.IsInteractable())
			{
				NavigationMappingTemplate component = selectable.GetComponent<NavigationMappingTemplate>();
				if (!(component != null))
				{
					return selectable;
				}
				if (component.CurrentlyMappedTarget != null)
				{
					return component.CurrentlyMappedTarget;
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
}
