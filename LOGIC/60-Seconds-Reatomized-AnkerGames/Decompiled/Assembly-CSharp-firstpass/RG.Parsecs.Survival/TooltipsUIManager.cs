using System.Collections.Generic;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class TooltipsUIManager : MonoBehaviour
{
	protected List<TooltipContentHandler> _handlers = new List<TooltipContentHandler>();

	private void Awake()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			TooltipContentHandler component = transform.GetChild(i).gameObject.GetComponent<TooltipContentHandler>();
			if (component != null)
			{
				_handlers.Add(component);
				component.SetManager(this);
			}
		}
	}

	public void Register(TooltipContentHandler handler)
	{
		_handlers.Add(handler);
	}

	public void Activate(TooltipContentHandler handler)
	{
		for (int i = 0; i < _handlers.Count; i++)
		{
			if (!_handlers[i].Equals(handler))
			{
				_handlers[i].gameObject.SetActive(value: false);
			}
		}
	}
}
