using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class TooltipTagConnection : IConnectionTypeDeclaration
{
	public const string ID = "TooltipTag";

	public string Identifier => "TooltipTag";

	public Type Type => typeof(TooltipTag);

	public Color Color => new Color(125f / 255f, 35f / 255f, 125f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
