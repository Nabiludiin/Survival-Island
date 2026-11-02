using System;
using RG.Parsecs.Survival;

namespace RG.Parsecs.EventEditor;

[Serializable]
public class PlayerResourceDecision : PlayerDecision
{
	public GameResource Result;

	public bool UseResource;

	public override string ToString()
	{
		return Result.ToString();
	}
}
