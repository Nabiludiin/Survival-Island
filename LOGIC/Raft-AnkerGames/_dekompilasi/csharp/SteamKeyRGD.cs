using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public class SteamKeyRGD : SerializableBehaviour
{
	public List<string> allKeys = new List<string>();

	public SteamKeyRGD()
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Keys", allKeys);
	}

	public SteamKeyRGD(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "Keys")
			{
				allKeys = (List<string>)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		allKeys = null;
	}
}
