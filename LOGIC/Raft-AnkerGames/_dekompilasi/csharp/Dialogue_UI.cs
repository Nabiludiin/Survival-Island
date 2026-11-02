using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dialogue_UI : MonoBehaviour
{
	public static bool IsShowingDialogue;

	[SerializeField]
	private TextMeshProUGUI subtitleText;

	[SerializeField]
	private DisplayText displayText;

	[SerializeField]
	private GameObject backgroundImage;

	[SerializeField]
	private InputActionReference inputCancel;

	private void Awake()
	{
		subtitleText = GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);
	}

	private void Update()
	{
		UpdateSubtitles();
	}

	private void UpdateSubtitles()
	{
		if (Dialogue.ActiveTrackedDialogues.Count > 0)
		{
			SO_VoiceData.Instance_VoiceData localPlayersPrioritizedDialogue = Dialogue.GetLocalPlayersPrioritizedDialogue();
			if (localPlayersPrioritizedDialogue != null)
			{
				SetDialogueText(localPlayersPrioritizedDialogue);
				SetUIEnable(value: true);
			}
			else
			{
				SetUIEnable(value: false);
			}
		}
		else
		{
			SetUIEnable(value: false);
		}
	}

	private void SetUIEnable(bool value)
	{
		IsShowingDialogue = value;
		backgroundImage.SetActiveSafe(value);
		if (!value)
		{
			displayText.HardHide();
		}
	}

	private void SetDialogueText(SO_VoiceData.Instance_VoiceData voiceDataInstance)
	{
		subtitleText.text = voiceDataInstance.GetCurrentSubtitleText();
		if (voiceDataInstance.voiceData.canBeCanceled && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.PlayerInput.currentActionMap.name == "Player")
		{
			if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
			{
				displayText.Show(string.Empty, MyInput.Keybinds["NoteBookStop"].MainKey);
			}
			else
			{
				displayText.ShowConsoleBinding(string.Empty, inputCancel.action.GetBindingDisplayString());
			}
		}
		else
		{
			displayText.HardHide();
		}
	}
}
