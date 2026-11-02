using System;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class VisualsDataConnection : IConnectionTypeDeclaration
{
	public const string ID = "VisualsData";

	public string Identifier => "VisualsData";

	public Type Type => typeof(VisualsData);

	public Color Color => new Color(45f / 255f, 1f / 6f, 120f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
