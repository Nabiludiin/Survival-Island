using I2.Loc;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class DestinationPanelController : MonoBehaviour, IRefreshInteractable
{
	[SerializeField]
	[Header("Data")]
	[Tooltip("Expedition Data scriptable object holding informations about expedition system.")]
	private ExpeditionData _expeditionData;

	[SerializeField]
	[Tooltip("Term with \"UNLOCKED EXPEDITIONS\" text.")]
	private LocalizedString _unlockedExpeditionsTerm;

	[SerializeField]
	[Header("References")]
	[Tooltip("Reference to repaired expedition panel controller which is the parent of this object.")]
	private RepairedExpeditionPanelController _repairedExpeditionPanelController;

	[SerializeField]
	[Tooltip("A parent transform for spawned map.")]
	private Transform _mapHolder;

	[SerializeField]
	[Tooltip("Field for displaying text with amount of unlocked expeditions on current planet.")]
	private TextMeshProUGUI _unlockedExpeditionsText;

	[SerializeField]
	[Tooltip("Animation controller handle.")]
	private ExpeditionSummaryAnimationController _animationController;

	[SerializeField]
	private RectTransform _locationTooltipRectTransform;

	private bool _initialized;

	private MapController _mapController;

	public ExpeditionData ExpeditionData => _expeditionData;

	public void Initialize()
	{
		CreateMap();
		_repairedExpeditionPanelController.SetAcceptButtonInteractable(_expeditionData.RuntimeData.PlannedExpeditionData.ChosenDestination != null);
		if (_mapController != null)
		{
			_mapController.RefreshMapPoints();
		}
		SetUnlockedExpeditionText();
		_initialized = true;
	}

	public void ResetDestination()
	{
		if (!(_mapController == null) && _mapController.MapPoints != null)
		{
			for (int i = 0; i < _mapController.MapPoints.Count; i++)
			{
				_mapController.MapPoints[i].SetToggleWithoutInvokingValueChange(value: false);
			}
		}
	}

	private void CreateMap()
	{
		if (_expeditionData.RuntimeData.CurrentPlanetDeck == null)
		{
			Debug.LogWarning("There is no Planet Deck.");
		}
		else if (!_initialized)
		{
			_mapController = Object.Instantiate(_expeditionData.RuntimeData.CurrentPlanetDeck.MapPrefab, _mapHolder, worldPositionStays: false).GetComponent<MapController>();
			_mapController.BindDestinationPanel(this);
			_mapController.SetTooltipToMapPoints(_locationTooltipRectTransform);
			_mapController.SetControllerToMapPoints();
		}
	}

	private void SetUnlockedExpeditionText()
	{
		if (_expeditionData.RuntimeData.ExpeditionDestinations.Count == 0)
		{
			Debug.LogWarning("There is no destinations in this Map.");
			return;
		}
		int num = 0;
		for (int i = 0; i < _expeditionData.RuntimeData.ExpeditionDestinations.Count; i++)
		{
			if (_expeditionData.RuntimeData.ExpeditionDestinations[i].Enabled && _expeditionData.RuntimeData.ExpeditionSystemLevel >= _expeditionData.RuntimeData.ExpeditionDestinations[i].RequiredSystemLevel)
			{
				num++;
			}
		}
		_unlockedExpeditionsText.text = $"{num}/{_expeditionData.RuntimeData.ExpeditionDestinations.Count} {_unlockedExpeditionsTerm}";
	}

	public void SetCurrentDestination(ExpeditionDestination destination)
	{
		_expeditionData.RuntimeData.PlannedExpeditionData.ChosenDestination = destination;
		_repairedExpeditionPanelController.SetAcceptButtonInteractable(destination != null);
		_animationController.SetSummaryState((!(destination == null)) ? ExpeditionSummaryAnimationController.ESummaryState.DESTINATION : ExpeditionSummaryAnimationController.ESummaryState.HIDDEN);
	}

	public void RefreshInteractable()
	{
		if (_mapController != null)
		{
			_mapController.RefreshMapPoints();
		}
	}
}
