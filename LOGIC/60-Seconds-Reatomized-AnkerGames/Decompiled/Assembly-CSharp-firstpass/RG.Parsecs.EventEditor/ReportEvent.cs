using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[NodeCanvasType("Event Editor/Report Event", "Create new Report Event", "New Report Event", "Create new Report Event")]
[CreateAssetMenu(menuName = "60 Parsecs!/Event Editor/New Report Event")]
public class ReportEvent : ParsecsEvent
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
