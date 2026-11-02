using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[NodeCanvasType("Event Editor/System Event", "Create new System Event", "New System Event", "Create new System Event")]
[CreateAssetMenu(menuName = "60 Parsecs!/Event Editor/New System Event")]
public class SystemEvent : ParsecsEvent
{
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
		ExecuteNode("EE_OnReportNode");
	}

	public override void ResetData()
	{
		CurrentPhase = EParsecsEventPhase.REPORT;
	}
}
