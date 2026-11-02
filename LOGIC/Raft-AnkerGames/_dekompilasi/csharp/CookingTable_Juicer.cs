using FMODUnity;
using UnityEngine;

public class CookingTable_Juicer : CookingTable
{
	public Battery battery;

	[SerializeField]
	private int batteryUsesPerMin = 1;

	[SerializeField]
	private StudioEventEmitter eventEmitter_juicerActive;

	private float timer;

	private float scuffedAntilaggSoundTimer;

	public void Start()
	{
		scuffedAntilaggSoundTimer = Random.Range(0f, 0.2f);
	}

	protected override void Update()
	{
		base.Update();
		if (Raft_Network.IsHost && IsCooking())
		{
			timer += Time.deltaTime;
			if (timer >= 60f)
			{
				timer = 0f;
				battery.RemoveBatteryUsesNetworked(batteryUsesPerMin);
			}
		}
		scuffedAntilaggSoundTimer += Time.deltaTime;
		if (scuffedAntilaggSoundTimer >= 0.3f)
		{
			HandleSound();
			scuffedAntilaggSoundTimer = 0f;
		}
		HandleAnimations();
	}

	private void HandleSound()
	{
		if (IsCooking() && !eventEmitter_juicerActive.IsPlaying())
		{
			eventEmitter_juicerActive.StopSafe();
			eventEmitter_juicerActive.PlaySafe();
		}
		else if (!IsCooking())
		{
			eventEmitter_juicerActive.StopSafe();
		}
	}

	private void HandleAnimations()
	{
		if (!battery.CanGiveElectricity && CurrentRecipe != null && Portions == 0)
		{
			anim.SetBool("Stalling", value: true);
		}
		else if (battery.CanGiveElectricity)
		{
			anim.SetBool("Stalling", value: false);
		}
	}

	protected override bool IsCooking()
	{
		if (base.IsCooking())
		{
			return battery.CanGiveElectricity;
		}
		return false;
	}

	public override bool CanStartCooking()
	{
		if (base.CanStartCooking())
		{
			return battery.CanGiveElectricity;
		}
		return false;
	}

	protected override bool StartCooking(SO_CookingTable_Recipe recipe)
	{
		bool flag = base.StartCooking(recipe);
		if (flag && eventEmitter_juicerActive != null)
		{
			eventEmitter_juicerActive.Stop();
			eventEmitter_juicerActive.Play();
		}
		return flag;
	}

	protected override void FinishCooking()
	{
		if (eventEmitter_juicerActive != null)
		{
			eventEmitter_juicerActive.EventInstance.keyOff();
		}
		base.FinishCooking();
	}

	public override RGD Serialize_Save()
	{
		if (hasBeenPlaced)
		{
			return new RGD_Block_CookingPot(GetComponent<Block>(), new RGD_CookingTable_Juicer(this));
		}
		return base.Serialize_Save();
	}

	public override void RestoreCookingPot(RGD_CookingPot rgdCookingPot)
	{
		if (rgdCookingPot != null)
		{
			base.RestoreCookingPot(rgdCookingPot);
			RGD_CookingTable_Juicer rGD_CookingTable_Juicer = rgdCookingPot as RGD_CookingTable_Juicer;
			if (rGD_CookingTable_Juicer != null && rGD_CookingTable_Juicer.rgdBattery != null)
			{
				rGD_CookingTable_Juicer.rgdBattery.RestoreBattery(battery);
			}
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		Messages type = msg.Type;
		if ((uint)(type - 116) <= 2u)
		{
			return battery.OnBatteryMessage(msg);
		}
		return base.Deserialize(msg, remoteID);
	}
}
