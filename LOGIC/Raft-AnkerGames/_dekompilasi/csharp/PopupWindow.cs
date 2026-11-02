using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour
{
	[SerializeField]
	private Selectable selectable;

	[SerializeField]
	private Selectable selectableOnClose;

	[SerializeField]
	private InputActionReference actionToListen;

	[SerializeField]
	private Text text;

	[SerializeField]
	private GameObject consoleText;

	[SerializeField]
	private Button closeButton;

	public Action OnClose;

	public void OnActionOnperformed(InputAction.CallbackContext context)
	{
		Close();
	}

	public void Close()
	{
		gameObject.SetActive(value: false);
		if ((bool)selectableOnClose)
		{
			selectableOnClose.Select();
		}
		else
		{
			ComponentManager<CanvasHelper>.Value.GetComponent<GamepadCursor>().EnableCursor(_useCursor: true);
		}
	}

	public void OnEnable()
	{
		if ((bool)selectable)
		{
			selectable.Select();
		}
		else
		{
			ComponentManager<CanvasHelper>.Value.GetComponent<GamepadCursor>().EnableCursor(_useCursor: false);
		}
		CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
		instance.OnControlsChangedEvent = (Action<bool>)Delegate.Combine(instance.OnControlsChangedEvent, new Action<bool>(ControlsChangedEvent));
		ControlsChangedEvent(SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad);
	}

	private void ControlsChangedEvent(bool gamepad)
	{
		consoleText.SetActive(gamepad);
		closeButton.gameObject.SetActive(!gamepad);
		if (gamepad)
		{
			actionToListen.action.performed += OnActionOnperformed;
		}
		else
		{
			actionToListen.action.performed -= OnActionOnperformed;
		}
	}

	public void OnDisable()
	{
		if ((bool)SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance)
		{
			CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
			instance.OnControlsChangedEvent = (Action<bool>)Delegate.Remove(instance.OnControlsChangedEvent, new Action<bool>(ControlsChangedEvent));
		}
		actionToListen.action.performed -= OnActionOnperformed;
		if (OnClose != null)
		{
			OnClose();
		}
	}

	public void SetMessage(string msg)
	{
		text.text = msg;
	}
}
