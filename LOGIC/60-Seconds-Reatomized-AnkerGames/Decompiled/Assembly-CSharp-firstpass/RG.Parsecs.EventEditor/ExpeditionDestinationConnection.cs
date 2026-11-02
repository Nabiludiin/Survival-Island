using System;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class ExpeditionDestinationConnection : IConnectionTypeDeclaration
{
	public const string ID = "ExpeditionDestination";

	public string Identifier => "ExpeditionDestination";

	public Type Type => typeof(ExpeditionDestination);

	public Color Color => new Color(60f / 255f, 200f / 255f, 100f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
