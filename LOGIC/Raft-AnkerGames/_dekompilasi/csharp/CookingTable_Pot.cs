public class CookingTable_Pot : CookingTable
{
	private FuelNetwork fuelInsertion;

	public Fuel Fuel => fuelInsertion.Fuel;

	protected override void Update()
	{
		if (hasBeenPlaced)
		{
			Fuel.UpdateFuel();
			base.Update();
			HandleAnimations();
		}
	}

	protected override void OnBlockPlaced()
	{
		base.OnBlockPlaced();
		fuelInsertion = GetComponentInChildren<FuelNetwork>();
		Fuel.StopBurning();
	}

	protected override bool IsCooking()
	{
		if (base.IsCooking() && Fuel.IsBurning())
		{
			return Fuel.GetFuelCount() > 0;
		}
		return false;
	}

	protected override bool StartCooking(SO_CookingTable_Recipe recipe)
	{
		bool flag = base.StartCooking(recipe);
		if (flag)
		{
			Fuel.StartBurning();
		}
		return flag;
	}

	protected override void FinishCooking()
	{
		base.FinishCooking();
		Fuel.StopBurning();
	}

	public override bool CanStartCooking()
	{
		if (base.CanStartCooking() && !Fuel.IsBurning())
		{
			return Fuel.GetFuelCount() > 0;
		}
		return false;
	}

	private void HandleAnimations()
	{
		if (!Fuel.IsBurning() && Fuel.GetFuelCount() > 0 && CurrentRecipe != null && Portions == 0)
		{
			Fuel.StartBurning();
			anim.SetBool("Cooking", value: true);
		}
		else if (!Fuel.IsBurning() && CurrentRecipe != null && Portions == 0)
		{
			anim.SetBool("Stalling", value: true);
		}
		else if (Fuel.IsBurning())
		{
			anim.SetBool("Stalling", value: false);
		}
	}

	public override RGD Serialize_Save()
	{
		if (hasBeenPlaced)
		{
			return new RGD_Block_CookingPot(GetComponent<Block>(), new RGD_CookingTable_Pot(this));
		}
		return base.Serialize_Save();
	}

	public override void RestoreCookingPot(RGD_CookingPot rgdCookingPot)
	{
		if (rgdCookingPot == null)
		{
			return;
		}
		base.RestoreCookingPot(rgdCookingPot);
		RGD_CookingTable_Pot rGD_CookingTable_Pot = rgdCookingPot as RGD_CookingTable_Pot;
		if (rGD_CookingTable_Pot != null)
		{
			if (rGD_CookingTable_Pot.rgdFuel != null)
			{
				rGD_CookingTable_Pot.rgdFuel.RestoreFuel(Fuel);
			}
			if (CurrentRecipe != null && Portions == 0)
			{
				Fuel.StartBurning();
			}
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.Fuel_Increment)
		{
			return fuelInsertion.OnFuelMessage(msg as Message_Fuel);
		}
		return base.Deserialize(msg, remoteID);
	}
}
