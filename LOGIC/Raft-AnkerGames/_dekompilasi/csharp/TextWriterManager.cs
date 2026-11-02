using System;
using System.Text;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TextWriterManager : MonoBehaviour
{
	[SerializeField]
	private InputField inputField;

	private TextWriterObject currentTextWriterObj;

	private CanvasHelper canvas;

	private Network_Player localPlayer;

	protected Callback<GamepadTextInputDismissed_t> cbGamepadTextDissmissed;

	public bool IsWriting
	{
		get
		{
			if (currentTextWriterObj != null)
			{
				return CanvasHelper.ActiveMenu == MenuType.TextWriter;
			}
			return false;
		}
	}

	private void Awake()
	{
		ComponentManager<TextWriterManager>.Value = this;
	}

	private void Start()
	{
		canvas = ComponentManager<CanvasHelper>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
		inputField.onEndEdit.AddListener(OnEndEdit);
		cbGamepadTextDissmissed = Callback<GamepadTextInputDismissed_t>.Create(GamepadTextDismissed);
	}

	private void GamepadTextDismissed(GamepadTextInputDismissed_t data)
	{
		if (currentTextWriterObj == null)
		{
			return;
		}
		if (data.m_bSubmitted)
		{
			Debug.LogError("GamepadTextInputDismissed_t!");
			uint enteredGamepadTextLength = SteamUtils.GetEnteredGamepadTextLength();
			if (SteamUtils.GetEnteredGamepadTextInput(out var pchText, enteredGamepadTextLength))
			{
				inputField.text = InsertNewlines(pchText, currentTextWriterObj.maxLineCharacterCount, currentTextWriterObj.maxLineCount);
			}
			Button_Finished();
		}
		else
		{
			AbortTyping();
		}
	}

	private void OnDestroy()
	{
		canvas = null;
		localPlayer = null;
		cbGamepadTextDissmissed.Dispose();
	}

	private void OnEndEdit(string s)
	{
		if (currentTextWriterObj != null)
		{
			Button_Finished();
		}
	}

	private string InsertNewlines(string input, int charactersPerLine, int maxLineBreaks)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		int num2 = 0;
		foreach (char value in input)
		{
			stringBuilder.Append(value);
			num++;
			if (num == charactersPerLine && num2 < maxLineBreaks)
			{
				stringBuilder.Append("\n");
				num = 0;
				num2++;
			}
		}
		return stringBuilder.ToString();
	}

	private string RemoveNewlines(string input)
	{
		return input;
	}

	private string TrimText(string s)
	{
		string[] array = inputField.text.Split('\n');
		int num = array.Length;
		inputField.lineType = ((num >= currentTextWriterObj.maxLineCount) ? InputField.LineType.MultiLineSubmit : InputField.LineType.MultiLineNewline);
		string text = string.Empty;
		bool flag = num > currentTextWriterObj.maxLineCount;
		for (int i = 0; i < Math.Min(num, currentTextWriterObj.maxLineCount); i++)
		{
			string text2 = array[i];
			if (text2.Length > currentTextWriterObj.maxLineCharacterCount)
			{
				text2 = text2.Substring(0, currentTextWriterObj.maxLineCharacterCount);
				flag = true;
			}
			if (i != 0)
			{
				text += "\n";
			}
			text += text2;
		}
		if (flag)
		{
			return text;
		}
		return inputField.text;
	}

	private void Update()
	{
		if (IsWriting)
		{
			if (!inputField.isFocused)
			{
				inputField.ActivateInputField();
			}
			currentTextWriterObj.SetText(TrimText(inputField.text), playSound: false);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(PlayerInput.GetPlayerByIndex(0).actions["Cancel"], "Cancel"))
			{
				AbortTyping();
			}
			else if (Input.GetKeyDown(KeyCode.Return) && !Input.GetKey(KeyCode.LeftShift))
			{
				Button_Finished();
			}
		}
	}

	public void AbortTyping()
	{
		if (IsWriting)
		{
			if (!localPlayer.CarryingComponent.IsCarrying)
			{
				PlayerItemManager.IsBusy = false;
			}
			currentTextWriterObj.SetIsBeingEdited(p_value: false);
			localPlayer.PersonController.IsMovementFree = true;
			Message_NetworkBehaviour_ID message = new Message_NetworkBehaviour_ID(Messages.Sign_EndEdit, localPlayer.Network.NetworkIDManager, currentTextWriterObj.ObjectIndex);
			if (Raft_Network.IsHost)
			{
				localPlayer.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				localPlayer.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			currentTextWriterObj = null;
			canvas.CloseMenu(MenuType.TextWriter);
			canvas.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Inventory);
		}
	}

	public void Button_Finished()
	{
		if (!localPlayer.CarryingComponent.IsCarrying)
		{
			PlayerItemManager.IsBusy = false;
		}
		currentTextWriterObj.SetTextFiltered(TrimText(inputField.text));
		canvas.CloseMenu(MenuType.TextWriter);
		canvas.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Inventory);
		localPlayer.PersonController.IsMovementFree = true;
		Message_OnEditText message_OnEditText = new Message_OnEditText(Messages.Sign_OnEdit, localPlayer.Network.NetworkIDManager, currentTextWriterObj);
		if (Raft_Network.IsHost)
		{
			localPlayer.Network.RPC(message_OnEditText, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			currentTextWriterObj.OnEditText(message_OnEditText.newText);
		}
		else
		{
			localPlayer.SendP2P(message_OnEditText, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		currentTextWriterObj = null;
	}

	public void StartWriting(TextWriterObject textWriterObj)
	{
		if (!IsWriting)
		{
			if (localPlayer == null)
			{
				localPlayer = ComponentManager<Network_Player>.Value;
			}
			PlayerItemManager.IsBusy = true;
			currentTextWriterObj = textWriterObj;
			localPlayer.PersonController.IsMovementFree = false;
			inputField.text = textWriterObj.GetText();
			inputField.lineType = InputField.LineType.MultiLineSubmit;
			inputField.ActivateInputField();
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad && SteamUtils.IsSteamInBigPictureMode())
			{
				SteamUtils.ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeMultipleLines, "Text", 33u, inputField.text);
			}
		}
	}
}
