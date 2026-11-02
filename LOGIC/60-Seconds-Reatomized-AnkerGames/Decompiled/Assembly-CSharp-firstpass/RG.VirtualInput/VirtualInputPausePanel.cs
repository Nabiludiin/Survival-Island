using RG.Parsecs.Common;

namespace RG.VirtualInput;

public class VirtualInputPausePanel : VirtualInputPanel
{
	private VirtualInputPanel _previousPanel;

	private bool wasInputBlocked;

	protected override void OnEnable()
	{
		Interactable = false;
	}

	public void Update()
	{
		if (Interactable && Singleton<VirtualInputManager>.Instance != null && Singleton<VirtualInputManager>.Instance.IsGamepadInputBlocked)
		{
			wasInputBlocked = true;
			Singleton<VirtualInputManager>.Instance.IsGamepadInputBlocked = false;
		}
	}

	public void Hide()
	{
		Interactable = false;
		if (_previousPanel != null)
		{
			_previousPanel.Interactable = true;
			VirtualInputPanel.CurrentPanel = _previousPanel;
		}
		if (wasInputBlocked)
		{
			Singleton<VirtualInputManager>.Instance.IsGamepadInputBlocked = true;
		}
		wasInputBlocked = false;
	}

	public void Show()
	{
		_previousPanel = VirtualInputPanel.CurrentPanel;
		if (_previousPanel != null)
		{
			_previousPanel.Interactable = false;
		}
		VirtualInputPanel.CurrentPanel = this;
		Interactable = true;
		if (Singleton<VirtualInputManager>.Instance != null && Singleton<VirtualInputManager>.Instance.IsGamepadInputBlocked)
		{
			wasInputBlocked = true;
			Singleton<VirtualInputManager>.Instance.IsGamepadInputBlocked = false;
		}
		Singleton<VirtualInputManager>.Instance.VisualManager.MouseCursorVisible = true;
	}

	public void GamepadInputUnlocked()
	{
		if (wasInputBlocked && Interactable)
		{
			wasInputBlocked = false;
		}
	}
}
