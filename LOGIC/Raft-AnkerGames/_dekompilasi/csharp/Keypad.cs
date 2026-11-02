using System;
using System.Collections;
using FMODUnity;
using TMPro;
using UnityEngine;

public class Keypad : MonoBehaviour_ID_Network
{
	public Action OnCodeComplete;

	[SerializeField]
	private string correctString = string.Empty;

	[SerializeField]
	protected TextMeshPro codeTextMesh;

	[SerializeField]
	private float displayFeedbackTime = 1f;

	[SerializeField]
	protected Color normalColor = Color.white;

	[SerializeField]
	protected Color successColor = Color.white;

	[SerializeField]
	protected Color errorColor = Color.white;

	[SerializeField]
	protected GameObject sucessLamp;

	[SerializeField]
	protected GameObject failureLamp;

	[SerializeField]
	private string[] buttonIndexToString;

	[SerializeField]
	protected string successTerm = string.Empty;

	[SerializeField]
	protected string errorTerm = string.Empty;

	public EventReference er_success;

	public EventReference er_failure;

	private Raft_Network network;

	private InteractableButton_Network_Forwarder buttonForwarder;

	protected Coroutine displayFeedbackRoutine;

	protected bool canInteractWithKeyPad = true;

	protected bool displayingFeedback;

	public string CurrentCode => codeTextMesh.text;

	protected virtual void Awake()
	{
		normalColor = codeTextMesh.color;
		codeTextMesh.text = string.Empty;
		buttonForwarder = new InteractableButton_Network_Forwarder(GetComponentsInChildren<InteractableButton_Networked>(includeInactive: true));
	}

	protected virtual void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	public void SetCode(string code)
	{
		codeTextMesh.text = code;
		if (codeTextMesh.text.Length >= correctString.Length)
		{
			bool flag = IsKeypadCodeComplete(codeTextMesh.text);
			if (flag)
			{
				RuntimeManager.PlayOneShot(er_success, transform.position);
			}
			else
			{
				RuntimeManager.PlayOneShot(er_failure, transform.position);
			}
			StartDisplayingFeedback(flag);
			if (flag)
			{
				OnCodeComplete?.Invoke();
			}
		}
	}

	private bool ButtonPress(int buttonIndex)
	{
		if (Raft_Network.IsHost)
		{
			if (buttonIndex < 0)
			{
				return false;
			}
			if (displayingFeedback)
			{
				return false;
			}
			if (!canInteractWithKeyPad)
			{
				return false;
			}
		}
		string stringFromButtonIndex = GetStringFromButtonIndex(buttonIndex);
		SetCode(CurrentCode + stringFromButtonIndex);
		return true;
	}

	private bool IsKeypadCodeComplete(string currentCode)
	{
		return currentCode == correctString;
	}

	private void StartDisplayingFeedback(bool codeComplete)
	{
		if (displayFeedbackRoutine != null)
		{
			StopCoroutine(displayFeedbackRoutine);
		}
		displayFeedbackRoutine = StartCoroutine(DisplayFeedback(codeComplete));
	}

	private IEnumerator DisplayFeedback(bool codeComplete)
	{
		string text = (codeComplete ? Helper.GetTerm(successTerm) : Helper.GetTerm(errorTerm));
		if (string.IsNullOrEmpty(text))
		{
			text = (codeComplete ? successTerm : errorTerm);
		}
		displayingFeedback = true;
		codeTextMesh.text = text;
		codeTextMesh.color = (codeComplete ? successColor : errorColor);
		if (codeComplete)
		{
			if (sucessLamp != null)
			{
				sucessLamp.SetActiveSafe(value: true);
			}
			if (failureLamp != null)
			{
				failureLamp.SetActiveSafe(value: false);
			}
		}
		yield return new WaitForSeconds(displayFeedbackTime);
		codeTextMesh.text = string.Empty;
		codeTextMesh.color = normalColor;
		displayingFeedback = false;
	}

	private string GetStringFromButtonIndex(int buttonIndex)
	{
		if (buttonIndexToString != null && buttonIndex >= 0 && buttonIndex < buttonIndexToString.Length)
		{
			return buttonIndexToString[buttonIndex];
		}
		return buttonIndex.ToString();
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		Messages type = msg.Type;
		if (type == Messages.InteractableButtonPress || type == Messages.InteractableButtonPress_Int)
		{
			Message_InteractableButtonPress message_InteractableButtonPress = msg as Message_InteractableButtonPress;
			buttonForwarder.HandleMessage(message_InteractableButtonPress);
			if (message_InteractableButtonPress.success)
			{
				message_InteractableButtonPress.success = ButtonPress(message_InteractableButtonPress.buttonIndex);
			}
			return true;
		}
		return base.Deserialize(msg, remoteID);
	}
}
