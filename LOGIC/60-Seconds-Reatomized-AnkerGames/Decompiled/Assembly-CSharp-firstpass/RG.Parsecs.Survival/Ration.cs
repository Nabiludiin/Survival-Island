using System;

namespace RG.Parsecs.Survival;

[Serializable]
public class Ration
{
	public int CharacterIndex;

	public IItem RationedItem;

	public override bool Equals(object obj)
	{
		if (obj is Ration)
		{
			Ration ration = (Ration)obj;
			if (ration.RationedItem == RationedItem)
			{
				return ration.CharacterIndex == CharacterIndex;
			}
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
