using System;
using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class StringConnection : IConnectionTypeDeclaration
{
	public const string ID = "String";

	public string Identifier => "String";

	public Type Type => typeof(string);

	public Color Color => new Color(0f, 150f / 255f, 0f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/In_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
