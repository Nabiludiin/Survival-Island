using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeybindInterface : MonoBehaviour
{
	public static bool AssigningKey;

	[Tooltip("Used when saving and loading keybinds")]
	[SerializeField]
	private string identifier = string.Empty;

	[SerializeField]
	private KeyCode mainKeyDefault;

	[SerializeField]
	private KeyCode altKeyDefault;

	[SerializeField]
	private bool allowMouseKeybinds = true;

	[Space(20f)]
	[SerializeField]
	private KeyConnection mainKey;

	[SerializeField]
	private KeyConnection altKey;

	private Keybind keybind;

	private ColorBlock colorblock;

	public Keybind Keybind => keybind;

	public string Identifier => identifier;

	public void Initialize(ColorBlock colorblock)
	{
		this.colorblock = colorblock;
		if (mainKey.text != null)
		{
			mainKey.text.color = colorblock.normalColor;
		}
		if (altKey.text != null)
		{
			altKey.text.color = colorblock.normalColor;
		}
		if (mainKey.button != null)
		{
			EventTrigger component = mainKey.button.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			entry.callback.AddListener((BaseEventData eventData) =>
			{
				PointerEnter(mainKey);
			});
			component.triggers.Add(entry);
			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerExit;
			entry.callback.AddListener((BaseEventData eventData) =>
			{
				PointerExit(mainKey);
			});
			component.triggers.Add(entry);
		}
		if (altKey.button != null)
		{
			EventTrigger component2 = altKey.button.GetComponent<EventTrigger>();
			EventTrigger.Entry entry2 = new EventTrigger.Entry();
			entry2.eventID = EventTriggerType.PointerEnter;
			entry2.callback.AddListener((BaseEventData eventData) =>
			{
				PointerEnter(altKey);
			});
			component2.triggers.Add(entry2);
			entry2 = new EventTrigger.Entry();
			entry2.eventID = EventTriggerType.PointerExit;
			entry2.callback.AddListener((BaseEventData eventData) =>
			{
				PointerExit(altKey);
			});
			component2.triggers.Add(entry2);
		}
	}

	public void Set(Keybind keybind)
	{
		this.keybind = keybind;
		Refresh();
	}

	public void SetDefaultKeys()
	{
		keybind = new Keybind(Identifier, mainKeyDefault, altKeyDefault);
		Refresh();
	}

	public void StartAssignMainKey()
	{
		StartCoroutine(AssignKey(KeybindKey.MainKey));
	}

	public void StartAssignAltKey()
	{
		StartCoroutine(AssignKey(KeybindKey.AltKey));
	}

	public void PointerEnter(KeyConnection key)
	{
		if (!AssigningKey)
		{
			key.text.color = colorblock.highlightedColor;
		}
	}

	public void PointerExit(KeyConnection key)
	{
		if (!AssigningKey)
		{
			key.text.color = colorblock.normalColor;
		}
	}

	private IEnumerator AssignKey(KeybindKey key)
	{
		AssigningKey = true;
		Text text = ((key == KeybindKey.AltKey) ? altKey.text : mainKey.text);
		text.text = "-";
		text.color = colorblock.highlightedColor;
		while (true)
		{
			if (Input.anyKeyDown)
			{
				bool flag = false;
				foreach (KeyCode value in Enum.GetValues(typeof(KeyCode)))
				{
					if (!Input.GetKey(value))
					{
						continue;
					}
					if (!allowMouseKeybinds && value.ToString().Contains("Mouse", StringComparison.OrdinalIgnoreCase))
					{
						flag = false;
						continue;
					}
					flag = true;
					if (key == KeybindKey.MainKey)
					{
						keybind.MainKey = value;
					}
					else
					{
						keybind.AltKey = value;
					}
				}
				if (flag)
				{
					break;
				}
			}
			yield return null;
		}
		Refresh();
		text.color = colorblock.normalColor;
		AssigningKey = false;
	}

	private void Refresh()
	{
		if (mainKey.text != null)
		{
			mainKey.text.text = keybind.MainKey.ToShortString();
		}
		if (altKey.text != null)
		{
			altKey.text.text = keybind.AltKey.ToShortString();
		}
	}
}
