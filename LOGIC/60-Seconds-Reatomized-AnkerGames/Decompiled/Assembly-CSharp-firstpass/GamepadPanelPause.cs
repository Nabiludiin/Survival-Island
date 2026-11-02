using RG.Parsecs.Common;
using RG.Parsecs.Menu;
using RG.Parsecs.Survival;

public class GamepadPanelPause : GamepadPanel
{
	private GamepadPanel _previousPanel;

	protected override void OnEnable()
	{
		_interactable = false;
	}

	public new void Hide()
	{
		Interactable = false;
		if (_previousPanel != null)
		{
			_previousPanel.Interactable = true;
			GamepadPanel.CurrentPanel = _previousPanel;
			Singleton<HandCursor>.Instance.RegisterGamepadPanel(_previousPanel);
		}
		Singleton<HandCursor>.Instance.UnRegisterGamepadPanel(this);
	}

	public void Show()
	{
		_previousPanel = GamepadPanel.CurrentPanel;
		if (_previousPanel != null)
		{
			_previousPanel.Interactable = false;
			Singleton<HandCursor>.Instance.UnRegisterGamepadPanel(_previousPanel);
		}
		GamepadPanel.CurrentPanel = this;
		Interactable = true;
		Singleton<HandCursor>.Instance.RegisterGamepadPanel(this);
	}

	public override void Cancel()
	{
		if (GamepadPanel.CurrentPanel == this)
		{
			BasePauseMenu.Instance.TogglePauseGame();
		}
	}
}
