using System;
using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class ObjectConnection : IConnectionTypeDeclaration
{
	public static string ID = "Object";

	public string Identifier => ID;

	public Type Type => typeof(object);

	public Color Color => new Color(150f / 255f, 150f / 255f, 150f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
