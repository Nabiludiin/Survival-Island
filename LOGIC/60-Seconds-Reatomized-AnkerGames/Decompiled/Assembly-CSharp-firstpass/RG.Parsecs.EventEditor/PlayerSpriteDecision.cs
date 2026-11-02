using System;

namespace RG.Parsecs.EventEditor;

[Serializable]
public class PlayerSpriteDecision : PlayerDecision
{
	public BaseActionCondition Result;

	public override string ToString()
	{
		return Result.Sprite.ToString();
	}
}
