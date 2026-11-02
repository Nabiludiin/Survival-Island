using UnityEngine;

namespace RG.Parsecs.Survival;

public class ExpeditionView : WindowView
{
	[SerializeField]
	[Header("Data")]
	[Tooltip("Expedition Data scriptable object holding informations about expedition system.")]
	private ExpeditionData _expeditionData;

	[SerializeField]
	[Header("References")]
	[Tooltip("Reference to damaged expedition panel.")]
	private DamagedExpeditionPanelController _damagedPanel;

	[SerializeField]
	[Tooltip("Reference to repaired expedition panel.")]
	private RepairedExpeditionPanelController _repairedPanel;

	[SerializeField]
	private GameObject _redLightExpedition;

	[SerializeField]
	private GameObject _greenLightExpedition;

	[SerializeField]
	private GameObject _yellowLightExpedition;

	[SerializeField]
	private GameObject _expeditionPaper;

	public override void OnShow()
	{
		if (!_expeditionData.RuntimeData.IsActive && !_expeditionData.RuntimeData.IsRepaired)
		{
			_damagedPanel.Initialize();
			_damagedPanel.gameObject.SetActive(value: true);
			_repairedPanel.gameObject.SetActive(value: false);
		}
		else
		{
			_repairedPanel.Initialize();
			_damagedPanel.gameObject.SetActive(value: false);
			_repairedPanel.gameObject.SetActive(value: true);
		}
	}

	public void ResetView()
	{
		if (_expeditionData.RuntimeData.IsActive)
		{
			_repairedPanel.ResetPanel();
		}
		else if (_expeditionData.RuntimeData.ShouldReactivate)
		{
			_damagedPanel.RepairExpedition();
		}
		if (_expeditionData.RuntimeData.IsOngoing)
		{
			_yellowLightExpedition.SetActive(value: true);
			_greenLightExpedition.SetActive(value: false);
			_redLightExpedition.SetActive(value: false);
		}
		else
		{
			_redLightExpedition.SetActive(value: true);
			_greenLightExpedition.SetActive(value: false);
			_yellowLightExpedition.SetActive(value: false);
			_expeditionPaper.SetActive(value: false);
		}
	}

	public void OnAcceptButtonPressed()
	{
		if (!_expeditionData.RuntimeData.IsActive)
		{
			Hide();
			return;
		}
		if (_repairedPanel.CanShowNextPanel)
		{
			_repairedPanel.ShowNextPanel();
			return;
		}
		_expeditionData.RuntimeData.IsPlanned = true;
		_repairedPanel.PrintLastSummary();
		Invoke("CloseWindow", 0.8f);
		_greenLightExpedition.SetActive(value: true);
		_redLightExpedition.SetActive(value: false);
		_yellowLightExpedition.SetActive(value: false);
		_expeditionPaper.SetActive(value: true);
	}

	public void CloseWindow()
	{
		Hide();
	}

	public void OnCloseWindowButtonPressed()
	{
		PrepareWindowClose();
		Hide();
	}

	private void PrepareWindowClose()
	{
		if ((_expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionCharacter == null || _expeditionData.RuntimeData.PlannedExpeditionData.ChosenDestination == null || !_expeditionData.RuntimeData.IsPlanned) && !_expeditionData.RuntimeData.IsOngoing)
		{
			_expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionCharacter = null;
			_expeditionData.RuntimeData.PlannedExpeditionData.ChosenDestination = null;
			_expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionItems.Clear();
			_expeditionData.RuntimeData.IsPlanned = false;
			_repairedPanel.ResetPanel();
			_redLightExpedition.SetActive(value: true);
			_greenLightExpedition.SetActive(value: false);
			_yellowLightExpedition.SetActive(value: false);
			_expeditionPaper.SetActive(value: false);
		}
	}

	protected override void OnClickAway()
	{
		PrepareWindowClose();
		Hide();
	}

	public void HideExpeditionPaper()
	{
		_expeditionPaper.SetActive(value: false);
	}
}
