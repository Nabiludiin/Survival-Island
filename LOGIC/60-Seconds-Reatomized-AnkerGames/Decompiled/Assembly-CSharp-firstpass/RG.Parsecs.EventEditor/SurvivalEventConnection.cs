using System;
using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class SurvivalEventConnection : IConnectionTypeDeclaration
{
	public const string ID = "SurvivalEvent";

	public string Identifier => "SurvivalEvent";

	public Type Type => typeof(SurvivalEvent);

	public Color Color => new Color(200f / 255f, 100f / 255f, 200f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/In_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
