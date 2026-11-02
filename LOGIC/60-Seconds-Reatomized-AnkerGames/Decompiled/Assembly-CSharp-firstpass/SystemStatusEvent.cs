using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using UnityEngine;

[NodeCanvasType("Event Editor/Status Event", "Create new Status Event", "New Status Event", "Create new Status Event")]
[CreateAssetMenu(menuName = "60 Parsecs!/Event Editor/New Status Event")]
public class SystemStatusEvent : ParsecsEvent
{
	public const string CHARACTER_STATUS_EVENT_PARAMETER_NAME = "StatusCharacter";

	protected override void SetInitialPhase()
	{
		CurrentPhase = EParsecsEventPhase.REPORT;
	}

	protected override void UpdateCurrentPhase()
	{
		CurrentPhase = EParsecsEventPhase.REPORT;
	}

	protected override void Execute()
	{
		ExecuteNode("EE_OnStatusNode");
	}

	public override void ResetData()
	{
		CurrentPhase = EParsecsEventPhase.REPORT;
	}
}
