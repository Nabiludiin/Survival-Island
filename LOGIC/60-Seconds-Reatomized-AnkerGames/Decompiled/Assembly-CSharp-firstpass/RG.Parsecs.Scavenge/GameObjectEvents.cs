namespace RG.Parsecs.Scavenge;

public enum GameObjectEvents
{
	None = 0,
	Started = 1,
	Enabled = 2,
	Disabled = 4,
	Destroyed = 8,
	Awaken = 0x10,
	StartedOrEnabled = 0x20
}
