using System.Collections;
using UnityEngine;

public class ChatVisibilityController : MonoBehaviour
{
	public CanvasGroup chatCanvas;

	[SerializeField]
	private bool isChatVisible;

	[SerializeField]
	private bool autoHideChat;

	[SerializeField]
	private float chatAutoHideDelay = 5f;

	private float latestRecievedMessageTimestamp;

	private Coroutine chatActivity;

	private ChatTextFieldController chatTextFieldController;

	private void OnEnable()
	{
		ChatManager.OnRecieveChatMessage += UpdateChatVisiblity;
	}

	private void OnDisable()
	{
		ChatManager.OnRecieveChatMessage -= UpdateChatVisiblity;
	}

	private void Awake()
	{
		chatCanvas = GetComponent<CanvasGroup>();
	}

	private void Start()
	{
		chatTextFieldController = ComponentManager<ChatTextFieldController>.Value;
	}

	public void UpdateChatVisiblity()
	{
		latestRecievedMessageTimestamp = Time.time;
		if (GeneralSettingsBox.ChatVisible)
		{
			SetChatVisible(p_value: true);
			StartChatActivityCoroutine1();
		}
	}

	public void SetChatVisible(bool p_value)
	{
		isChatVisible = p_value;
		ShowChat(isChatVisible);
	}

	private void ShowChat(bool value)
	{
		chatCanvas.blocksRaycasts = value;
		chatCanvas.alpha = (value ? 1 : 0);
		chatCanvas.interactable = value;
	}

	public void StartChatActivityCoroutine1()
	{
		if (chatActivity != null)
		{
			StopCoroutine(chatActivity);
		}
		chatActivity = StartCoroutine(CheckChatActivity());
	}

	private IEnumerator CheckChatActivity()
	{
		float timer = 0f;
		while (timer < chatAutoHideDelay)
		{
			timer += Time.deltaTime;
			if (!GeneralSettingsBox.ChatVisible)
			{
				break;
			}
			yield return null;
		}
		CheckAutoHideChat();
	}

	private void CheckAutoHideChat()
	{
		if (autoHideChat && !chatTextFieldController.chatInputField.isFocused)
		{
			SetChatVisible(p_value: false);
		}
	}

	public void ResetAutoHideTimer()
	{
		latestRecievedMessageTimestamp = Time.time;
	}
}
