using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class LocalizeDropdownSemih : MonoBehaviour
{
	public string[] terms;

	private Dropdown dropdown;

	public void Start()
	{
		LocalizationManager.OnLocalizeEvent += OnLocalize;
		OnLocalize();
	}

	public void OnDestroy()
	{
		LocalizationManager.OnLocalizeEvent -= OnLocalize;
	}

	private void OnEnable()
	{
		OnLocalize();
	}

	public void OnLocalize()
	{
		if (enabled && !(gameObject == null) && gameObject.activeInHierarchy && !string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
		{
			UpdateLocalization();
		}
	}

	public void UpdateLocalization()
	{
		if (dropdown == null)
		{
			dropdown = GetComponent<Dropdown>();
		}
		dropdown.options.Clear();
		for (int i = 0; i < terms.Length; i++)
		{
			string translation = LocalizationManager.GetTranslation(terms[i]);
			dropdown.options.Add(new Dropdown.OptionData(translation));
		}
		dropdown.RefreshShownValue();
	}
}
