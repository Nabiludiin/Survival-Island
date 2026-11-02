using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Common;

public abstract class TooltipContentHandler : MonoBehaviour
{
	private TooltipsUIManager _manager;

	public void SetManager(TooltipsUIManager manager)
	{
		_manager = manager;
	}

	private void OnEnable()
	{
		if (_manager != null)
		{
			_manager.Activate(this);
		}
	}

	public abstract void HandleContent(TooltipContent content);
}
