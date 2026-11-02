using System;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class ExpeditionThreatConnection : IConnectionTypeDeclaration
{
	public const string ID = "ExpeditionThreat";

	public string Identifier => "ExpeditionThreat";

	public Type Type => typeof(ExpeditionThreat);

	public Color Color => new Color(100f / 255f, 50f / 255f, 50f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
