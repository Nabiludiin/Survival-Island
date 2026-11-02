using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class VisualsController : MonoBehaviour
{
	[SerializeField]
	private VisualsData _visualsData;

	[Tooltip("If set to false, the ResetVisualsData command won't work.")]
	[SerializeField]
	private bool _isAffectedByVisualsReset;

	private Visual _activeVisual;

	private List<Visual> _visuals = new List<Visual>();

	public bool IsVisualActive => _activeVisual != null;

	public void Awake()
	{
		GetVisualsList();
		RefreshVisualsState();
		VisualsManager.Instance.RegisterVisualsController(this);
	}

	public void OnDestroy()
	{
		if (VisualsManager.Instance != null)
		{
			VisualsManager.Instance.UnregisterVisualsController(this);
		}
	}

	private void GetVisualsList()
	{
		GetComponentsInChildren(includeInactive: true, _visuals);
	}

	public void RefreshVisualsState()
	{
		if (_visuals == null || !(_visualsData != null))
		{
			return;
		}
		for (int i = 0; i < _visuals.Count; i++)
		{
			if (_visuals[i].gameObject.activeSelf != ShouldVisualBeShown(i))
			{
				_visuals[i].gameObject.SetActive(ShouldVisualBeShown(i));
			}
		}
	}

	private bool ShouldVisualBeShown(int visualId)
	{
		return _visuals[visualId].Id.Equals(_visualsData.RuntimeData.VisualToDisplay);
	}

	public void ResetVisualsData()
	{
		if (_isAffectedByVisualsReset)
		{
			_visualsData.ResetToInitialState();
		}
	}

	public void HideVisuals()
	{
		if (_visuals != null)
		{
			for (int i = 0; i < _visuals.Count; i++)
			{
				_visuals[i].gameObject.SetActive(value: false);
			}
		}
	}
}
