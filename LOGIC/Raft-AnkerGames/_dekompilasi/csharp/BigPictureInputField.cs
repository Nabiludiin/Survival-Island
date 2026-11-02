using Steamworks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class BigPictureInputField : MonoBehaviour, ISubmitHandler, IEventSystemHandler
{
	private InputField _inputField;

	private Callback<GamepadTextInputDismissed_t> cbGamepadTextDissmissed;

	[SerializeField]
	private string _label;

	private bool open;

	private void Awake()
	{
		_inputField = GetComponent<InputField>();
	}

	private void GamepadTextDismissed(GamepadTextInputDismissed_t data)
	{
		if (!open)
		{
			return;
		}
		if (data.m_bSubmitted)
		{
			uint enteredGamepadTextLength = SteamUtils.GetEnteredGamepadTextLength();
			if (SteamUtils.GetEnteredGamepadTextInput(out var pchText, enteredGamepadTextLength))
			{
				_inputField.text = pchText;
			}
		}
		_inputField.DeactivateInputField();
		open = false;
	}

	private void OnEnable()
	{
		cbGamepadTextDissmissed = Callback<GamepadTextInputDismissed_t>.Create(GamepadTextDismissed);
	}

	private void OnDisable()
	{
		cbGamepadTextDissmissed.Dispose();
	}

	private void Update()
	{
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad && _inputField.isFocused && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.PlayerInput.actions["Cancel"].WasPressedThisFrame())
		{
			_inputField.DeactivateInputField();
			open = false;
		}
	}

	public void OnSubmit(BaseEventData eventData)
	{
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad && SteamUtils.IsSteamInBigPictureMode())
		{
			open = SteamUtils.ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, _label, (uint)_inputField.characterLimit, _inputField.text);
		}
	}
}
