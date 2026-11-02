using System;
using NodeEditorFramework;
using RG.SecondsRemaster.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class GroupIdConnection : IConnectionTypeDeclaration
{
	public const string ID = "GroupId";

	public string Identifier => "GroupId";

	public Type Type => typeof(TextJournalGroupId);

	public Color Color => new Color(0f, 150f / 255f, 100f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/In_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
