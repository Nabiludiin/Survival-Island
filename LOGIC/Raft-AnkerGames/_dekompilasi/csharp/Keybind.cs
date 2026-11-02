using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class Keybind : SerializableBehaviour
{
	[SerializeField]
	private string identifier;

	[SerializeField]
	private KeyCode mainKey;

	[SerializeField]
	private KeyCode altKey;

	public KeyCode MainKey
	{
		get
		{
			return mainKey;
		}
		set
		{
			mainKey = value;
		}
	}

	public KeyCode AltKey
	{
		get
		{
			return altKey;
		}
		set
		{
			altKey = value;
		}
	}

	public string Identifier => identifier;

	public Keybind(string identifier, KeyCode mainKey, KeyCode altKey = KeyCode.None)
	{
		this.identifier = identifier;
		this.mainKey = mainKey;
		this.altKey = altKey;
	}

	public Keybind(Keybind other)
	{
		identifier = other.identifier;
		mainKey = other.MainKey;
		altKey = other.AltKey;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Identifier", identifier);
		info.AddValue("MainKey", mainKey);
		info.AddValue("AltKey", altKey);
	}

	public Keybind(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "Identifier":
				identifier = (string)item.Value;
				break;
			case "MainKey":
				mainKey = (KeyCode)item.Value;
				break;
			case "AltKey":
				altKey = (KeyCode)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		identifier = "Default settings";
		mainKey = KeyCode.None;
		altKey = KeyCode.None;
	}
}
