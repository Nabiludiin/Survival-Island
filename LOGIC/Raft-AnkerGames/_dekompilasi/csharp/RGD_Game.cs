using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public class RGD_Game : RGD
{
	public string name;

	public long lastPlayedDateTicks;

	public GameMode mode;

	public RGD_World world;

	public List<RGD> behaviours = new List<RGD>();

	public List<RGD_FloatingItem> floatingItems = new List<RGD_FloatingItem>();

	public List<RGD_Landmark> landmarks = new List<RGD_Landmark>();

	public List<RGD_QuestItem> landmarkQuestItems = new List<RGD_QuestItem>();

	public List<RGD_Quest> quests = new List<RGD_Quest>();

	public List<RGD_WorldEvent> worldEvents = new List<RGD_WorldEvent>();

	public RGD_ResearchTableWorld researchTableWorld;

	public RGD_NoteBook noteBook;

	public int biggestObjectIndex;

	public float timePlayedSeconds;

	public uint versionNumber;

	public int traderReputation;

	public RGD_Game()
		: base(RGDType.World, RGDSortingOrder.First)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("TimePlayedSeconds", timePlayedSeconds);
		info.AddValue("WorldName", name);
		info.AddValue("LastPlayed", lastPlayedDateTicks);
		info.AddValue("Mode", mode);
		info.AddValue("World", world);
		info.AddValue("Behaviours", behaviours);
		info.AddValue("FloatingItems", floatingItems);
		info.AddValue("Landmarks", landmarks);
		info.AddValue("ResearchTableWorld", researchTableWorld);
		info.AddValue("BiggestObjectIndex", biggestObjectIndex);
		info.AddValue("VersionNumber", versionNumber);
		info.AddValue("LandmarkQuestItems", landmarkQuestItems);
		info.AddValue("Quests", quests);
		info.AddValue("NoteBook", noteBook);
		info.AddValue("WorldEvents", worldEvents);
		info.AddValue("TraderReputation", traderReputation);
	}

	public RGD_Game(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "WorldName":
				name = (string)item.Value;
				break;
			case "LastPlayed":
				lastPlayedDateTicks = Helper.ObjToValue<long>(item.Value);
				break;
			case "Mode":
				mode = (GameMode)item.Value;
				break;
			case "World":
				world = (RGD_World)item.Value;
				break;
			case "Behaviours":
				behaviours = (List<RGD>)item.Value;
				break;
			case "FloatingItems":
				floatingItems = (List<RGD_FloatingItem>)item.Value;
				break;
			case "Landmarks":
				landmarks = (List<RGD_Landmark>)item.Value;
				break;
			case "ResearchTableWorld":
				researchTableWorld = (RGD_ResearchTableWorld)item.Value;
				break;
			case "BiggestObjectIndex":
				biggestObjectIndex = (int)item.Value;
				break;
			case "TimePlayedSeconds":
				timePlayedSeconds = (float)item.Value;
				break;
			case "VersionNumber":
				versionNumber = (uint)item.Value;
				break;
			case "LandmarkQuestItems":
				landmarkQuestItems = (List<RGD_QuestItem>)item.Value;
				break;
			case "Quests":
				quests = (List<RGD_Quest>)item.Value;
				break;
			case "NoteBook":
				noteBook = (RGD_NoteBook)item.Value;
				break;
			case "WorldEvents":
				worldEvents = (List<RGD_WorldEvent>)item.Value;
				break;
			case "TraderReputation":
				traderReputation = (int)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		name = "Default name";
		lastPlayedDateTicks = 0L;
		world = null;
		behaviours = null;
		floatingItems = null;
		landmarks = null;
		researchTableWorld = null;
		biggestObjectIndex = -1;
		mode = GameMode.Normal;
		timePlayedSeconds = 0f;
		versionNumber = 0u;
		landmarkQuestItems = null;
		quests = null;
		noteBook = null;
		traderReputation = 0;
	}
}
