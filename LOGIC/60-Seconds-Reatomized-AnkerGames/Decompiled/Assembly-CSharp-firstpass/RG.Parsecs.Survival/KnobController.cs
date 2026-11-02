using FMOD.Studio;
using RG.Parsecs.Common;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class KnobController : MonoBehaviour, IRefreshInteractable
{
	[Header("Data")]
	[SerializeField]
	private EndOfDayListenerList _endOfDayListenerList;

	[Header("Scene References")]
	[SerializeField]
	private StarlogPanelController[] _starlogPanels;

	[SerializeField]
	private DayInfoController _dayInfoController;

	[SerializeField]
	private Toggle[] _toggles;

	[SerializeField]
	private Transform _knobButton;

	private EStarlogPanelType _currentPanelType;

	private int _currentPanelIndex;

	private const float TURN_ANGLE = 90f;

	private const int RESET_LISTENER_LIST_PRIORITY = 35;

	[Header("FMod emitter")]
	[SerializeField]
	private OnUIClickedSoundPlayer _soundEmitter;

	private PLAYBACK_STATE playbackState;

	private void Awake()
	{
		SwitchToPanel(EStarlogPanelType.REPORT);
		_endOfDayListenerList.RegisterOnEndOfDay(ResetStarlogPanelToReport, "Reset", 35, this);
	}

	private void OnDestroy()
	{
		_endOfDayListenerList.UnregisterOnEndOfDay(ResetStarlogPanelToReport, "Reset");
	}

	private void ResetStarlogPanelToReport()
	{
		SwitchToPanel(EStarlogPanelType.REPORT);
		_toggles[0].GetComponent<KnobToggle>().SetToggleWithoutInvokingValueChange(value: true);
		for (int i = 1; i < _toggles.Length; i++)
		{
			_toggles[i].GetComponent<KnobToggle>().SetToggleWithoutInvokingValueChange(value: false);
		}
		_soundEmitter.FModEvent.getPlaybackState(out playbackState);
		if (playbackState == PLAYBACK_STATE.STARTING)
		{
			_soundEmitter.FModEvent.stop(STOP_MODE.IMMEDIATE);
		}
	}

	public void OnStarlogKnobPressed()
	{
		SwitchToNextPanel();
	}

	public void OnStarlogButtonPressed(int intType)
	{
		_dayInfoController.SetKnobState((EStarlogPanelType)intType);
		_dayInfoController.RefreshInteractable();
		SwitchToPanel((EStarlogPanelType)intType);
	}

	private void DisablePanelOfType(EStarlogPanelType type)
	{
		for (int i = 0; i < _starlogPanels.Length; i++)
		{
			if (_starlogPanels[i].PanelType == type)
			{
				_starlogPanels[i].gameObject.SetActive(value: false);
				_toggles[i].GetComponent<GlowController>().SetEnabled(enable: false);
				break;
			}
		}
	}

	private void SwitchToPanel(EStarlogPanelType type)
	{
		DisablePanelOfType(_currentPanelType);
		_currentPanelType = type;
		_currentPanelIndex = EnablePanelOfType(type);
		UpdateKnobRotation();
	}

	private void SwitchToNextPanel()
	{
		_starlogPanels[_currentPanelIndex].gameObject.SetActive(value: false);
		_toggles[_currentPanelIndex].GetComponent<GlowController>().SetEnabled(enable: false);
		if (_currentPanelIndex + 1 >= _starlogPanels.Length)
		{
			_currentPanelIndex = 0;
		}
		else
		{
			_currentPanelIndex++;
		}
		_starlogPanels[_currentPanelIndex].gameObject.SetActive(value: true);
		_toggles[_currentPanelIndex].isOn = true;
		_currentPanelType = _starlogPanels[_currentPanelIndex].PanelType;
		UpdateKnobRotation();
	}

	private int EnablePanelOfType(EStarlogPanelType type)
	{
		for (int i = 0; i < _starlogPanels.Length; i++)
		{
			if (_starlogPanels[i].PanelType == _currentPanelType)
			{
				_starlogPanels[i].gameObject.SetActive(value: true);
				_toggles[i].GetComponent<GlowController>().SetEnabled(enable: true);
				return i;
			}
		}
		throw new UnityException($"You're trying to enable panel that is not existing! {type}");
	}

	private void UpdateKnobRotation()
	{
		_knobButton.localRotation = Quaternion.Euler(new Vector3(0f, 0f, (float)_currentPanelIndex * -90f));
	}

	public void RefreshInteractable()
	{
		Toggle[] toggles = _toggles;
		for (int i = 0; i < toggles.Length; i++)
		{
			toggles[i].interactable = true;
		}
	}
}
