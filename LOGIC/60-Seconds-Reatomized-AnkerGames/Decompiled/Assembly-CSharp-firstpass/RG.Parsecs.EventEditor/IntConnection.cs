using System;
using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class IntConnection : IConnectionTypeDeclaration
{
	public const string ID = "Int";

	public string Identifier => "Int";

	public Type Type => typeof(int);

	public Color Color => new Color(0f, 150f / 255f, 200f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/In_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
