using System;

[Serializable]
public class RaftSpecificVariables
{
	public bool isRaftAlwaysAnchored;

	public bool usesBuoyancy = true;

	public bool prewarmFloatingItems = true;

	public bool handleRaftCollision = true;
}
