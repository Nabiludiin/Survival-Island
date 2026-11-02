using System;

namespace RG.Parsecs.Survival;

[Serializable]
public class GameResource : ICloneable
{
	public static string CHEMICAL_ID = "resource_chemical";

	public static string MINERAL_ID = "resource_mineral";

	public static string POWER_ID = "resource_power";

	public Resource Resource;

	public int Amount;

	private int _plannedUse;

	public int CurrentAmount => Amount - _plannedUse;

	public GameResource(Resource resource, int amount)
	{
		Resource = resource;
		Amount = amount;
	}

	public GameResource(Resource resource)
	{
		Resource = resource;
	}

	public static GameResource operator +(GameResource a, GameResource b)
	{
		if (!a.Resource.Equals(b.Resource))
		{
			throw new Exception();
		}
		return new GameResource(a.Resource)
		{
			Amount = a.Amount + b.Amount
		};
	}

	public static GameResource operator -(GameResource a, GameResource b)
	{
		if (!a.Resource.Equals(b.Resource))
		{
			throw new Exception();
		}
		return new GameResource(a.Resource)
		{
			Amount = a.Amount - b.Amount
		};
	}

	public static bool operator >(GameResource a, GameResource b)
	{
		if (!a.Resource.Equals(b.Resource))
		{
			throw new Exception();
		}
		return a.Amount > b.Amount;
	}

	public static bool operator <(GameResource a, GameResource b)
	{
		if (!a.Resource.Equals(b.Resource))
		{
			throw new Exception();
		}
		return a.Amount < b.Amount;
	}

	public static bool operator <=(GameResource a, GameResource b)
	{
		if (!a.Resource.Equals(b.Resource))
		{
			throw new Exception();
		}
		return a.Amount <= b.Amount;
	}

	public static bool operator >=(GameResource a, GameResource b)
	{
		if (!a.Resource.Equals(b.Resource))
		{
			throw new Exception();
		}
		return a.Amount >= b.Amount;
	}

	public override string ToString()
	{
		return $"{Resource.Name} ({Amount})";
	}

	public object Clone()
	{
		return new GameResource(Resource, Amount);
	}

	public bool CanLock(int amount = 1)
	{
		return Amount - _plannedUse - amount >= 0;
	}

	public void Lock(int amount = 1)
	{
		if (Amount - _plannedUse - amount >= 0)
		{
			_plannedUse += amount;
		}
	}

	public void Unlock(int amount = 1)
	{
		if (_plannedUse - amount >= 0)
		{
			_plannedUse -= amount;
		}
	}
}
