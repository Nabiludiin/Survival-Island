using System;
using RG.Core.Base;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
[CreateAssetMenu(menuName = "60 Parsecs!/EOD System Registration Data", fileName = "RegistrationData")]
public class CustomEndOfDayProcessingRegistrationData : RGScriptableObject
{
	public enum EndOfDayProcessingSlot
	{
		Update,
		SystemEvents,
		Reset,
		PrepareSystemLayer,
		GoalLayer
	}

	[Tooltip("This allows the designers to run chosen ReportEvents (without displaying text, obviously) during different moments of the end of day processing.\nEvents marked as Update will be run after most report events but before system events. Thats when rationing happened in Parsecs.\nEvents marked as System are happening when SystemEvents are run.\nEvents marked as Reset are run last in the processing, as cleanup. During that time the rationing decisions of the player from the previous day are reset in Parsecs.\n\nCAUTION! The end of day processing order is complex and fragile, use with care and test thoroughly.")]
	public EndOfDayProcessingSlot ProcessingSlot;

	public int Priority;

	public ReportEvent EventToRun;

	public bool IsValid()
	{
		if (Priority >= 0)
		{
			return EventToRun != null;
		}
		return false;
	}

	public void RegisterInEndOfDayListenerList(EndOfDayListenerList list)
	{
		switch (ProcessingSlot)
		{
		case EndOfDayProcessingSlot.Update:
			list.RegisterOnEndOfDay(EndOfDayCustomProcessing, "Update", Priority, this);
			break;
		case EndOfDayProcessingSlot.SystemEvents:
			list.RegisterOnEndOfDay(EndOfDayCustomProcessing, "SystemEvents", Priority, this);
			break;
		case EndOfDayProcessingSlot.Reset:
			list.RegisterOnEndOfDay(EndOfDayCustomProcessing, "Reset", Priority, this);
			break;
		case EndOfDayProcessingSlot.PrepareSystemLayer:
			list.RegisterOnEndOfDay(EndOfDayCustomProcessing, "PrepareSystem", Priority, this);
			break;
		case EndOfDayProcessingSlot.GoalLayer:
			list.RegisterOnEndOfDay(EndOfDayCustomProcessing, "Goals", Priority, this);
			break;
		default:
			Debug.LogError("Illegal ProcessingSlot set in CustomEndOfDayProcessingRegistrationData " + name);
			break;
		}
	}

	public void UnregisterFromEndOfDayListenerList(EndOfDayListenerList list)
	{
		switch (ProcessingSlot)
		{
		case EndOfDayProcessingSlot.Update:
			list.UnregisterOnEndOfDay(EndOfDayCustomProcessing, "Update");
			break;
		case EndOfDayProcessingSlot.SystemEvents:
			list.UnregisterOnEndOfDay(EndOfDayCustomProcessing, "SystemEvents");
			break;
		case EndOfDayProcessingSlot.Reset:
			list.UnregisterOnEndOfDay(EndOfDayCustomProcessing, "Reset");
			break;
		case EndOfDayProcessingSlot.PrepareSystemLayer:
			list.UnregisterOnEndOfDay(EndOfDayCustomProcessing, "PrepareSystem");
			break;
		case EndOfDayProcessingSlot.GoalLayer:
			list.UnregisterOnEndOfDay(EndOfDayCustomProcessing, "Goals");
			break;
		default:
			Debug.LogError("Illegal ProcessingSlot set in CustomEndOfDayProcessingRegistrationData " + name);
			break;
		}
	}

	public void EndOfDayCustomProcessing()
	{
		if (EventToRun != null)
		{
			EventToRun.Run();
		}
	}
}
