using System;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class VisualIdConnection : IConnectionTypeDeclaration
{
	public const string ID = "VisualId";

	public string Identifier => "VisualId";

	public Type Type => typeof(VisualId);

	public Color Color => new Color(1f, 0f, 0f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
