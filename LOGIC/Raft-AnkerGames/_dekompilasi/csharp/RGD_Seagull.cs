using System;
using System.Runtime.Serialization;

[Serializable]
public class RGD_Seagull : RGD
{
	public SerializableVector3 Position;

	public uint objectIndex;

	public uint behaviourIndex;

	public uint entityObjectIndex;

	public int targetScarecrowObjectIndex;

	public int targetCropplotObjectIndex;

	public int targetNestObjectIndex;

	public int health;

	public float broodTimer;

	public SeagullState state;

	public bool anim_sit;

	public bool anim_flyingAltitude;

	public bool anim_nest;

	public bool anim_cropplot;

	public bool anim_scarecrow;

	public RGD_Seagull(RGDType type, Seagull seagull)
		: base(type, RGDSortingOrder.Entities)
	{
		objectIndex = seagull.ObjectIndex;
		behaviourIndex = seagull.BehaviourIndex;
		entityObjectIndex = seagull.entityComponent.ObjectIndex;
		Position = new SerializableVector3(seagull.transform.position);
		health = (int)seagull.entityComponent.stat_health.Value;
		state = seagull.currentState;
		broodTimer = seagull.broodTimer;
		anim_sit = seagull.anim.GetBool("Sit");
		anim_flyingAltitude = seagull.anim.GetBool("FlyingAltitude");
		anim_nest = seagull.anim.GetBool("Nest");
		anim_cropplot = seagull.anim.GetBool("Peck_Cropplot");
		anim_scarecrow = seagull.anim.GetBool("Peck_Scarecrow");
		targetCropplotObjectIndex = ((seagull.targetCropplot == null) ? (-1) : ((int)seagull.targetCropplot.ObjectIndex));
		targetScarecrowObjectIndex = ((seagull.targetScarecrow == null) ? (-1) : ((int)seagull.targetScarecrow.GetComponent<Block>().ObjectIndex));
		targetNestObjectIndex = ((seagull.currentNest == null) ? (-1) : ((int)seagull.currentNest.GetComponent<Block>().ObjectIndex));
	}

	public void RestoreSeagull(Seagull seagull)
	{
		seagull.transform.position = Position.Value;
		if (health != -1)
		{
			seagull.entityComponent.stat_health.Value = health;
		}
		if (broodTimer != -1f)
		{
			seagull.broodTimer = broodTimer;
		}
		seagull.anim.SetBool("Sit", anim_sit);
		seagull.anim.SetBool("FlyingAltitude", anim_flyingAltitude);
		seagull.anim.SetBool("Nest", anim_nest);
		seagull.anim.SetBool("Peck_Cropplot", anim_cropplot);
		seagull.anim.SetBool("Peck_Scarecrow", anim_scarecrow);
		if (state != SeagullState.Peck && state != SeagullState.Ruva)
		{
			return;
		}
		if (targetScarecrowObjectIndex >= 0)
		{
			Block blockByObjectIndex = BlockCreator.GetBlockByObjectIndex((uint)targetScarecrowObjectIndex);
			if (blockByObjectIndex != null)
			{
				seagull.targetScarecrow = blockByObjectIndex.GetComponent<Scarecrow>();
				seagull.targetPoint = seagull.GetBirdlandingPos(seagull.targetScarecrow.transform);
				seagull.SwitchState(SeagullState.Peck);
			}
		}
		else if (targetCropplotObjectIndex >= 0)
		{
			Block blockByObjectIndex2 = BlockCreator.GetBlockByObjectIndex((uint)targetCropplotObjectIndex);
			if (blockByObjectIndex2 != null)
			{
				seagull.targetCropplot = blockByObjectIndex2.GetComponent<Cropplot>();
				seagull.targetPoint = seagull.targetCropplot.transform;
				seagull.SwitchState(SeagullState.Peck);
			}
		}
		else if (targetNestObjectIndex >= 0)
		{
			Block blockByObjectIndex3 = BlockCreator.GetBlockByObjectIndex((uint)targetNestObjectIndex);
			if (blockByObjectIndex3 != null)
			{
				seagull.currentNest = blockByObjectIndex3.GetComponent<BirdsNest>();
				seagull.targetPoint = seagull.GetBirdlandingPos(seagull.currentNest.transform);
				seagull.SwitchState(SeagullState.Ruva);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Position", Position);
		info.AddValue("ObjectIndex", objectIndex);
		info.AddValue("BehaviourIndex", behaviourIndex);
		info.AddValue("EntityObjectIndex", entityObjectIndex);
		info.AddValue("ScarecrowObjectIndex", targetScarecrowObjectIndex);
		info.AddValue("CropplotObjectIndex", targetCropplotObjectIndex);
		info.AddValue("NestObjectIndex", targetNestObjectIndex);
		info.AddValue("Health", health);
		info.AddValue("BroodTimer", broodTimer);
		info.AddValue("State", state);
		info.AddValue("Anim_Sit", anim_sit);
		info.AddValue("Anim_FlyingAltitude", anim_flyingAltitude);
		info.AddValue("Anim_Nest", anim_nest);
		info.AddValue("Anim_Cropplot", anim_cropplot);
		info.AddValue("Anim_Scarecrow", anim_scarecrow);
	}

	public RGD_Seagull(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "Position":
				Position = (SerializableVector3)item.Value;
				break;
			case "ObjectIndex":
				objectIndex = (uint)item.Value;
				break;
			case "BehaviourIndex":
				behaviourIndex = (uint)item.Value;
				break;
			case "EntityObjectIndex":
				entityObjectIndex = (uint)item.Value;
				break;
			case "ScarecrowObjectIndex":
				targetScarecrowObjectIndex = (int)item.Value;
				break;
			case "CropplotObjectIndex":
				targetCropplotObjectIndex = (int)item.Value;
				break;
			case "NestObjectIndex":
				targetNestObjectIndex = (int)item.Value;
				break;
			case "Health":
				health = (int)item.Value;
				break;
			case "BroodTimer":
				broodTimer = (float)item.Value;
				break;
			case "State":
				state = (SeagullState)item.Value;
				break;
			case "Anim_Sit":
				anim_sit = (bool)item.Value;
				break;
			case "Anim_FlyingAltitude":
				anim_flyingAltitude = (bool)item.Value;
				break;
			case "Anim_Nest":
				anim_nest = (bool)item.Value;
				break;
			case "Anim_Cropplot":
				anim_cropplot = (bool)item.Value;
				break;
			case "Anim_Scarecrow":
				anim_scarecrow = (bool)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		Position = new SerializableVector3();
		objectIndex = 0u;
		behaviourIndex = 0u;
		entityObjectIndex = 0u;
		targetScarecrowObjectIndex = -1;
		targetCropplotObjectIndex = -1;
		targetNestObjectIndex = -1;
		health = -1;
		broodTimer = -1f;
		state = SeagullState.Hover;
		anim_sit = false;
		anim_flyingAltitude = false;
		anim_nest = false;
		anim_cropplot = false;
		anim_scarecrow = false;
	}
}
