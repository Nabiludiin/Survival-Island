using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class MapController : MonoBehaviour
{
	[SerializeField]
	[Tooltip("List of map points that are added to this map.")]
	private List<MapPointController> _mapPoints;

	[SerializeField]
	private float _animationOffset;

	private DestinationPanelController _destinationPanelController;

	private Coroutine _animation;

	public List<MapPointController> MapPoints => _mapPoints;

	private void Awake()
	{
		_animation = StartCoroutine(AnimateUnvisitedMarks());
	}

	private void OnEnable()
	{
		if (_animation == null)
		{
			_animation = StartCoroutine(AnimateUnvisitedMarks());
		}
	}

	private void OnDisable()
	{
		StopAnimationCoroutine();
	}

	private void OnDestroy()
	{
		StopAnimationCoroutine();
	}

	private void StopAnimationCoroutine()
	{
		if (_animation != null)
		{
			StopCoroutine(_animation);
			_animation = null;
		}
	}

	private IEnumerator AnimateUnvisitedMarks()
	{
		bool visible = true;
		while (true)
		{
			for (int i = 0; i < _mapPoints.Count; i++)
			{
				if (visible)
				{
					_mapPoints[i].ShowUnvisitedAnimation();
				}
				else
				{
					_mapPoints[i].HideUnvisitedAnimation();
				}
			}
			visible = !visible;
			yield return new WaitForSeconds(_animationOffset);
		}
	}

	public void BindDestinationPanel(DestinationPanelController destinationPanelController)
	{
		_destinationPanelController = destinationPanelController;
	}

	public void SetDestinationPoint(MapPointController pointController)
	{
		if (!pointController.IsOn)
		{
			_destinationPanelController.SetCurrentDestination(null);
			pointController.SetUnvisitedMark(!pointController.Destination.DynamicData.Visited);
			return;
		}
		if (pointController.Destination == null)
		{
			throw new UnityException("Destination was not set.");
		}
		_destinationPanelController.SetCurrentDestination(pointController.Destination);
		pointController.SetUnvisitedMark(visible: false);
	}

	public void SetTooltipToMapPoints(RectTransform tooltip)
	{
		for (int i = 0; i < _mapPoints.Count; i++)
		{
			_mapPoints[i].SetTooltip(tooltip);
		}
	}

	public void SetControllerToMapPoints()
	{
		for (int i = 0; i < _mapPoints.Count; i++)
		{
			_mapPoints[i].SetMapController(this);
		}
	}

	public void RefreshMapPoints()
	{
		for (int i = 0; i < _mapPoints.Count; i++)
		{
			_mapPoints[i].RefreshMapPoint(_mapPoints[i].Destination.RequiredSystemLevel <= _destinationPanelController.ExpeditionData.RuntimeData.ExpeditionSystemLevel);
		}
	}
}
