using UnityEngine;

public class Block_CookingStand_Electric : Block_CookingStand
{
	public Battery battery;

	public int batteryUsesPerMin = 1;

	[SerializeField]
	private MeshRenderer rend;

	[SerializeField]
	private float emissionLerpSpeed = 1f;

	private MaterialPropertyBlock materialPropertyBlock;

	private float timer;

	protected override void Start()
	{
		base.Start();
		if (materialPropertyBlock == null)
		{
			materialPropertyBlock = new MaterialPropertyBlock();
		}
		if (rend != null)
		{
			rend.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetFloat("_EmissionSwitch", 0f);
			rend.SetPropertyBlock(materialPropertyBlock);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (Raft_Network.IsHost && IsUsingEnergySource())
		{
			timer += Time.deltaTime;
			if (timer >= 60f)
			{
				timer = 0f;
				battery.RemoveBatteryUsesNetworked(batteryUsesPerMin);
			}
		}
		if (materialPropertyBlock != null && rend != null)
		{
			float num = ((battery != null && battery.On) ? 1f : 0f);
			rend.GetPropertyBlock(materialPropertyBlock);
			float num2 = materialPropertyBlock.GetFloat("_EmissionSwitch");
			if (num2 != num)
			{
				float num3 = ((num == 0f) ? (emissionLerpSpeed * 3f) : emissionLerpSpeed);
				materialPropertyBlock.SetFloat("_EmissionSwitch", Mathf.MoveTowards(num2, num, Time.deltaTime * num3));
				rend.SetPropertyBlock(materialPropertyBlock);
			}
		}
	}

	protected override bool CanStartEnergySource()
	{
		if (battery != null && !battery.On && battery.BatteryUses > 0)
		{
			return true;
		}
		return false;
	}

	protected override bool IsUsingEnergySource()
	{
		if (battery != null && battery.On)
		{
			return true;
		}
		return false;
	}

	protected override void StartEnergySource()
	{
		if (battery != null)
		{
			battery.On = true;
		}
	}

	protected override void StopEnergySource()
	{
		if (battery != null)
		{
			battery.On = false;
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Cookingstand_Smelter_Electric(RGDType.Block_Electric_CookingStand, this);
	}

	public override RGD_Block GetBlockCreationData()
	{
		return new RGD_Cookingstand_Smelter_Electric(RGDType.Block_Electric_CookingStand, this);
	}
}
