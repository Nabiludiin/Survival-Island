using System;
using System.Runtime.Serialization;

[Serializable]
public class RGD_ZiplinePath : SerializableBehaviour
{
	public uint baseAObjectIndex;

	public uint baseBObjectIndex;

	public float slack;

	public RGD_ZiplinePath(MeshPathConnection ziplineConnection)
	{
		baseAObjectIndex = ziplineConnection.baseA.ObjectIndex;
		baseBObjectIndex = ziplineConnection.baseB.ObjectIndex;
		slack = ziplineConnection.path.currentSlack;
	}

	public RGD_ZiplinePath(ZiplineBase baseA, ZiplineBase baseB, float slack)
	{
		baseAObjectIndex = baseA.ObjectIndex;
		baseBObjectIndex = baseB.ObjectIndex;
		this.slack = slack;
	}

	public bool Restore(bool playSound)
	{
		if (baseAObjectIndex != 0 && baseBObjectIndex != 0)
		{
			Block blockByObjectIndex = BlockCreator.GetBlockByObjectIndex(baseAObjectIndex);
			if (blockByObjectIndex != null)
			{
				MeshPathBase component = blockByObjectIndex.GetComponent<MeshPathBase>();
				if (component != null)
				{
					Block blockByObjectIndex2 = BlockCreator.GetBlockByObjectIndex(baseBObjectIndex);
					if (blockByObjectIndex2 != null)
					{
						MeshPathBase component2 = blockByObjectIndex2.GetComponent<MeshPathBase>();
						if (component2 != null)
						{
							return MeshPath.CreateMeshPath(component, component2, slack, replicating: true);
						}
					}
				}
			}
		}
		return false;
	}

	public MeshPathConnection GetZiplineBlockConnection()
	{
		Block blockByObjectIndex = BlockCreator.GetBlockByObjectIndex(baseAObjectIndex);
		if (blockByObjectIndex != null)
		{
			MeshPathBase component = blockByObjectIndex.GetComponent<MeshPathBase>();
			if (component != null)
			{
				Block blockByObjectIndex2 = BlockCreator.GetBlockByObjectIndex(baseBObjectIndex);
				if (blockByObjectIndex2 != null)
				{
					MeshPathBase component2 = blockByObjectIndex2.GetComponent<MeshPathBase>();
					return MeshPath.GetConnection(component, component2);
				}
			}
		}
		return null;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("ZiplineBaseAObjectIndex", baseAObjectIndex);
		info.AddValue("ZiplineBaseBObjectIndex", baseBObjectIndex);
		info.AddValue("ZiplineSlack", slack);
	}

	public RGD_ZiplinePath(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "ZiplineBaseAObjectIndex":
				baseAObjectIndex = Helper.ObjToValue<uint>(item.Value);
				break;
			case "ZiplineBaseBObjectIndex":
				baseBObjectIndex = Helper.ObjToValue<uint>(item.Value);
				break;
			case "ZiplineSlack":
				slack = Helper.ObjToValue<float>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		baseAObjectIndex = (baseBObjectIndex = 0u);
		slack = -0.5f;
	}
}
