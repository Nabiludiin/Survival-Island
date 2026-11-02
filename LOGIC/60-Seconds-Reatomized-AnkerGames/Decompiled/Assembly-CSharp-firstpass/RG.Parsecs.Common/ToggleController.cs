using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Common;

public abstract class ToggleController : MonoBehaviour
{
	[SerializeField]
	protected Toggle _toggle;

	protected bool _settingToggleWithoutValueChange;

	public Toggle Toggle => _toggle;

	public void OnValueChange(bool value)
	{
		if (!_settingToggleWithoutValueChange)
		{
			OnToggleValueChangedAction(value);
		}
	}

	public abstract void OnToggleValueChangedAction(bool toggleValue);

	public void SetToggleWithoutInvokingValueChange(bool value)
	{
		if (_toggle == null)
		{
			_toggle = GetComponent<Toggle>();
		}
		_settingToggleWithoutValueChange = true;
		_toggle.isOn = value;
		_settingToggleWithoutValueChange = false;
	}
}
