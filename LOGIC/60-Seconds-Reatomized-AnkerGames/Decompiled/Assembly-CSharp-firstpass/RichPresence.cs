using System;
using UnityEngine;

[Serializable]
internal class RichPresence
{
	public RichPresenceStatus richPresenceId;

	public string defaultPresence;

	[SerializeField]
	public RichPresenceLocales[] locales;
}
