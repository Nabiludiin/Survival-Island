using RG.Parsecs.Common;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

[RequireComponent(typeof(Toggle))]
public class MapPointController : ToggleController
{
	[SerializeField]
	[Tooltip("Destination that will be used when choosing location for expedition.")]
	private ExpeditionDestination _expeditionDestination;

	[SerializeField]
	private TooltipController _tooltipController;

	[SerializeField]
	private GameObject _unvisitedMark;

	[SerializeField]
	private Image _unvisitedMarkRenderer;

	private MapController _mapController;

	public ExpeditionDestination Destination => _expeditionDestination;

	public bool IsOn
	{
		get
		{
			return Toggle.isOn;
		}
		set
		{
			Toggle.isOn = value;
		}
	}

	public bool Interactable
	{
		get
		{
			return Toggle.interactable;
		}
		set
		{
			Toggle.interactable = value;
		}
	}

	private new Toggle Toggle
	{
		get
		{
			if (_toggle == null)
			{
				_toggle = GetComponent<Toggle>();
			}
			return _toggle;
		}
	}

	public void SetUnvisitedMark(bool visible)
	{
		_unvisitedMark.SetActive(visible);
	}

	public void RefreshMapPoint(bool expeditionSystemHasRequiredLevel)
	{
		bool flag = expeditionSystemHasRequiredLevel && _expeditionDestination.Enabled;
		gameObject.SetActive(flag);
		Interactable = flag;
		SetUnvisitedMark(!_expeditionDestination.DynamicData.Visited);
	}

	public void SetTooltip(RectTransform tooltip)
	{
		_tooltipController.SetTooltipRect(tooltip);
	}

	public void ShowUnvisitedAnimation()
	{
		_unvisitedMarkRenderer.enabled = true;
	}

	public void HideUnvisitedAnimation()
	{
		_unvisitedMarkRenderer.enabled = false;
	}

	public void SetMapController(MapController mapController)
	{
		_mapController = mapController;
	}

	public override void OnToggleValueChangedAction(bool toggleValue)
	{
		_mapController.SetDestinationPoint(this);
	}
}
