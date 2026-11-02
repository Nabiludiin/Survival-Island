using System;
using System.Runtime.Serialization;

[Serializable]
public class RGD_Settings_General : SerializableBehaviour
{
	public bool quickBuild;

	public bool showNameTag;

	public bool autoSave;

	public bool motionSickness;

	public bool chatVisible;

	public bool helpfulUIVisible;

	public bool cursorInvert;

	public bool autoPlayNotes;

	public RGD_Settings_General()
	{
		SetDefaults(default);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("QuickBuild", quickBuild);
		info.AddValue("ShowNameTag", showNameTag);
		info.AddValue("AutoSave", autoSave);
		info.AddValue("MotionSickness", motionSickness);
		info.AddValue("ChatVisible", chatVisible);
		info.AddValue("HelpfulUIVisible", helpfulUIVisible);
		info.AddValue("CursorInvert", cursorInvert);
		info.AddValue("AutoPlayNotes", autoPlayNotes);
	}

	public RGD_Settings_General(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "QuickBuild":
				quickBuild = Helper.ObjToValue<bool>(item.Value);
				break;
			case "ShowNameTag":
				showNameTag = Helper.ObjToValue<bool>(item.Value);
				break;
			case "AutoSave":
				autoSave = Helper.ObjToValue<bool>(item.Value);
				break;
			case "MotionSickness":
				motionSickness = Helper.ObjToValue<bool>(item.Value);
				break;
			case "ChatVisible":
				chatVisible = Helper.ObjToValue<bool>(item.Value);
				break;
			case "HelpfulUIVisible":
				helpfulUIVisible = Helper.ObjToValue<bool>(item.Value);
				break;
			case "CursorInvert":
				cursorInvert = Helper.ObjToValue<bool>(item.Value);
				break;
			case "AutoPlayNotes":
				autoPlayNotes = Helper.ObjToValue<bool>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		quickBuild = true;
		showNameTag = true;
		autoSave = true;
		motionSickness = false;
		chatVisible = true;
		helpfulUIVisible = true;
		cursorInvert = false;
		autoPlayNotes = true;
	}
}
