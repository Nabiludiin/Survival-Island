using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using UnityEngine;

public class PlayerDecisionConnection : IConnectionTypeDeclaration
{
	public const string ID = "PlayerDecision";

	public string Identifier => "PlayerDecision";

	public Type Type => typeof(PlayerDecision);

	public Color Color => new Color(11f / 15f, 116f / 255f, 0f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/In_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
