using UnityEngine;

namespace RG.VirtualInput;

public class VirtualInputClosablePanel : VirtualInputPanel
{
	[SerializeField]
	private VirtualInputPanel _masterPanel;

	[SerializeField]
	private bool _showOnEnable = true;

	protected override void OnEnable()
	{
		if (_showOnEnable)
		{
			Show();
		}
	}

	private void OnDisable()
	{
		Hide();
	}

	public void Show()
	{
		if (_masterPanel != null)
		{
			_masterPanel.Interactable = false;
		}
		Interactable = true;
		VirtualInputPanel.CurrentPanel = this;
	}

	public void Hide()
	{
		if (_masterPanel != null)
		{
			_masterPanel.Interactable = true;
			VirtualInputPanel.CurrentPanel = _masterPanel;
		}
		Interactable = false;
	}
}
