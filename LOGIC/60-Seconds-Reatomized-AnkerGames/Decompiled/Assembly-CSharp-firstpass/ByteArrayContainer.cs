using System;

[Serializable]
public class ByteArrayContainer
{
	public string Base64;

	public byte[] Bytes
	{
		get
		{
			return Convert.FromBase64String(Base64);
		}
		set
		{
			Base64 = Convert.ToBase64String(value);
		}
	}
}
