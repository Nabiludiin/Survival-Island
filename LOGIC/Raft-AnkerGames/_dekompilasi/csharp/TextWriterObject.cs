using System;
using Steamworks;
using TMPro;
using UnityEngine;

public class TextWriterObject : MonoBehaviour_ID_Network
{
	[HideInInspector]
	public CanvasHelper canvas;

	[HideInInspector]
	public TextWriterManager textWriterMenu;

	[SerializeField]
	private TextMeshPro textMesh;

	public int maxLineCount = 1;

	public int maxLineCharacterCount = 10;

	[HideInInspector]
	public bool isBeingEdited;

	[HideInInspector]
	public bool hasBeenInitialized;

	[SerializeField]
	public bool modifyTextOnPlacement = true;

	protected void Awake()
	{
		SetText(string.Empty, playSound: false);
	}

	protected virtual void Start()
	{
		SetComponents();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkIDManager.RemoveNetworkID(this, typeof(TextWriterObject));
	}

	public void Initialize(MonoBehaviour_ID id)
	{
		ObjectIndex = id.ObjectIndex;
		NetworkIDManager.AddNetworkID(this, typeof(TextWriterObject));
	}

	public void SetComponents()
	{
		canvas = ComponentManager<CanvasHelper>.Value;
		textWriterMenu = ComponentManager<TextWriterManager>.Value;
		hasBeenInitialized = true;
	}

	public bool OnEditText(string p_value)
	{
		SetText(p_value);
		SetIsBeingEdited(p_value: false);
		return true;
	}

	public bool GetIsBeingEdited()
	{
		return isBeingEdited;
	}

	public void SetIsBeingEdited(bool p_value)
	{
		isBeingEdited = p_value;
	}

	public static string GetMessageFromText(string text)
	{
		int num = text.IndexOf("-#PF[[", StringComparison.Ordinal);
		if (num < 0)
		{
			return text;
		}
		return text.Substring(0, num);
	}

	public void SetText(string text, bool playSound = true)
	{
		textMesh.text = GetMessageFromText(text);
	}

	public void SetTextFiltered(string text)
	{
		textMesh.text = GetMessageFromText(text);
	}

	public void SetTextNetworked(string text)
	{
		Raft_Network value = ComponentManager<Raft_Network>.Value;
		Message_OnEditText message = new Message_OnEditText(Messages.Sign_OnEdit, ComponentManager<NetworkIDManager>.Value, text, ObjectIndex);
		if (Raft_Network.IsHost)
		{
			SetText(text);
			value.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			value.SendP2P(value.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public string GetText()
	{
		return textMesh.text;
	}

	public virtual void SendOnSignEdited(string p_newString)
	{
	}

	public override RGD Serialize_Save()
	{
		return new RGD_TextWriterObject(RGDType.ID_TextWriterObject, this);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.Sign_OnEdit)
		{
			Message_OnEditText message_OnEditText = msg as Message_OnEditText;
			SetTextFiltered(message_OnEditText.newText);
			SetIsBeingEdited(p_value: false);
			return true;
		}
		return false;
	}
}
