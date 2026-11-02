using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuBox : MonoBehaviour
{
	[SerializeField]
	private Selectable selectOnOpen;

	[SerializeField]
	private Selectable selectOnClose;

	[SerializeField]
	protected GameObject[] gamepadObjects;

	[SerializeField]
	private GameObject[] keyboardObjects;

	public virtual bool IsOpen => gameObject.activeInHierarchy;

	protected virtual void OnDestroy()
	{
		if ((bool)SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance)
		{
			CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
			instance.OnControlsChangedEvent = (Action<bool>)Delegate.Remove(instance.OnControlsChangedEvent, new Action<bool>(OnControlChanged));
		}
	}

	protected virtual void OnControlChanged(bool gamepad)
	{
		GameObject[] array = gamepadObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(gamepad);
		}
		array = keyboardObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(!gamepad);
		}
	}

	public virtual void SettingStart()
	{
	}

	public virtual void SettingsUpdate()
	{
	}

	public virtual void Update()
	{
		if (!KeybindInterface.AssigningKey && Input.GetKeyDown(KeyCode.Escape) && IsOpen)
		{
			Close();
		}
	}

	public virtual void Open()
	{
		gameObject.SetActive(value: true);
		CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
		instance.OnControlsChangedEvent = (Action<bool>)Delegate.Combine(instance.OnControlsChangedEvent, new Action<bool>(OnControlChanged));
		OnControlChanged(SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad);
		selectOnOpen?.Select();
		selectOnClose?.GetComponent<SelectableMenuItem>()?.KeepActive();
	}

	public virtual void Close()
	{
		if ((bool)SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance)
		{
			CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
			instance.OnControlsChangedEvent = (Action<bool>)Delegate.Remove(instance.OnControlsChangedEvent, new Action<bool>(OnControlChanged));
		}
		gameObject.SetActive(value: false);
		selectOnClose?.GetComponent<SelectableMenuItem>()?.ReleaseActive();
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			selectOnClose?.Select();
		}
		else
		{
			selectOnClose?.GetComponent<SelectableMenuItem>()?.OnDeselect(null);
		}
	}

	public virtual void Toggle()
	{
		if (gameObject.activeInHierarchy)
		{
			Close();
		}
		else
		{
			Open();
		}
	}
}
