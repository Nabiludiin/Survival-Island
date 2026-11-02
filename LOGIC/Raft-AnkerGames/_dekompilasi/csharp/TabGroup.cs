using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TabGroup : MonoBehaviour
{
	public Action OnNewTabOpen;

	[SerializeField]
	private InputActionReference inputTabRight;

	[SerializeField]
	private InputActionReference inputTabLeft;

	[SerializeField]
	private bool setTabAtStart;

	[SerializeField]
	private int startingTabIndex;

	[HideInInspector]
	public TabButton[] tabButtons;

	private TabButton selectedTabButton;

	[HideInInspector]
	public TabButton hightlightedTabButton;

	public TabButton SelectedTabButton
	{
		get
		{
			return selectedTabButton;
		}
		set
		{
			OpenTab(value.Tab);
		}
	}

	private void Awake()
	{
		tabButtons = GetComponentsInChildren<TabButton>();
		if (setTabAtStart && startingTabIndex >= 0 && startingTabIndex < tabButtons.Length)
		{
			SelectTab(startingTabIndex);
		}
	}

	private void OnEnable()
	{
		ButtonSound.Mute = true;
		if (selectedTabButton != null)
		{
			OpenTab(selectedTabButton.Tab, forceOpen: true);
		}
		ButtonSound.Mute = false;
	}

	public void Update()
	{
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			return;
		}
		if ((bool)inputTabRight && inputTabRight.action.WasPressedThisFrame())
		{
			int num = selectedTabButton.tabIndex + 1;
			if (num < tabButtons.Length && tabButtons[num].IsInteractable)
			{
				selectedTabButton.OnPointerExit(forceExit: true);
				SelectTab(num);
			}
		}
		else if ((bool)inputTabLeft && inputTabLeft.action.WasPressedThisFrame())
		{
			int num2 = selectedTabButton.tabIndex - 1;
			if (num2 >= 0 && tabButtons[num2].IsInteractable)
			{
				selectedTabButton.OnPointerExit(forceExit: true);
				SelectTab(num2);
			}
		}
	}

	public void OpenTab(GameObject tab, bool forceOpen = false)
	{
		if (!forceOpen && SelectedTabButton != null && SelectedTabButton.Tab == tab)
		{
			return;
		}
		TabButton[] array = tabButtons;
		foreach (TabButton tabButton in array)
		{
			if (tabButton.Tab == tab)
			{
				if (tabButton.Tab != null)
				{
					tabButton.Tab.SetActive(value: true);
				}
				tabButton.OnPointerEnter();
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
				{
					tabButton.GetComponent<ButtonSound>()?.OnSubmit(null);
				}
				selectedTabButton = tabButton;
				OnNewTabOpen?.Invoke();
			}
		}
		array = tabButtons;
		foreach (TabButton tabButton2 in array)
		{
			if (tabButton2.Tab != tab && tabButton2.Tab != null)
			{
				tabButton2.Tab.SetActive(value: false);
			}
		}
	}

	public void SelectTab(int tabButtonIndex)
	{
		for (int i = 0; i < tabButtons.Length; i++)
		{
			if (tabButtons[i].tabIndex == tabButtonIndex)
			{
				OpenTab(tabButtons[i].Tab);
				break;
			}
		}
	}
}
