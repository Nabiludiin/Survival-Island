using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Menu;

public class KeyboardSchema : MonoBehaviour
{
	[HideInInspector]
	public Toggle toggle;

	public string map;

	public SettingsSO settings;

	public MenuManager menuManager;

	private void Awake()
	{
		toggle = GetComponent<Toggle>();
	}

	private void OnEnable()
	{
		if (settings.runtimeData.keyboardMap.Equals(map))
		{
			toggle.isOn = true;
		}
	}

	public void PlayAudo(string sound)
	{
		if (toggle.isOn)
		{
			menuManager.PlayAudio(sound);
		}
	}
}
