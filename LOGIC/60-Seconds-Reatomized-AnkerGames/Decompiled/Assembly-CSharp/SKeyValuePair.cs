using System;

[Serializable]
public struct SKeyValuePair(string id, string val)
{
	public string Id = id;

	public string Val = val;
}
