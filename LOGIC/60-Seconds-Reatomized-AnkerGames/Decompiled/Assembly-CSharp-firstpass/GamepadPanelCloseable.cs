using RG.Parsecs.Common;
using RG.Parsecs.Survival;
using UnityEngine;
using UnityEngine.UI;

public class GamepadPanelCloseable : GamepadPanel
{
	[SerializeField]
	private GamepadPanel masterPanel;

	[SerializeField]
	private Button closeButton;

	[SerializeField]
	private bool ShowOnEnable = true;

	private WindowView wv;

	protected override void Awake()
	{
		base.Awake();
		wv = GetComponent<WindowView>();
		if (wv != null)
		{
			wv.onWindowViewShown += OnWindowViewShown;
			wv.onWindowViewHidden += OnWindowViewHidden;
		}
	}

	private void OnDestroy()
	{
		if (wv != null)
		{
			wv.onWindowViewShown -= OnWindowViewShown;
			wv.onWindowViewHidden -= OnWindowViewHidden;
		}
		if (Singleton<HandCursor>.Instance != null)
		{
			Singleton<HandCursor>.Instance.UnRegisterGamepadPanel(this);
		}
	}

	protected override void OnEnable()
	{
		if (ShowOnEnable)
		{
			if (wv == null)
			{
				OnWindowViewShown();
			}
		}
		else
		{
			Interactable = false;
		}
	}

	public void Show()
	{
		OnWindowViewShown();
	}

	public override void Hide()
	{
		OnWindowViewHidden();
	}

	private void OnDisable()
	{
		if (wv == null)
		{
			OnWindowViewHidden();
		}
	}

	private void OnWindowViewHidden()
	{
		if (masterPanel != null)
		{
			masterPanel.Interactable = true;
			GamepadPanel.CurrentPanel = masterPanel;
		}
		Interactable = false;
		Singleton<HandCursor>.Instance.UnRegisterGamepadPanel(this);
	}

	private void OnWindowViewShown()
	{
		if (masterPanel != null)
		{
			masterPanel.Interactable = false;
		}
		Interactable = true;
		GamepadPanel.CurrentPanel = this;
		Singleton<HandCursor>.Instance.RegisterGamepadPanel(this);
	}

	public override void Cancel()
	{
		if (_interactable)
		{
			if (closeButton != null && closeButton.gameObject.activeInHierarchy && closeButton.interactable)
			{
				closeButton.onClick.Invoke();
			}
			else if (wv != null)
			{
				wv.Hide();
			}
		}
	}
}
