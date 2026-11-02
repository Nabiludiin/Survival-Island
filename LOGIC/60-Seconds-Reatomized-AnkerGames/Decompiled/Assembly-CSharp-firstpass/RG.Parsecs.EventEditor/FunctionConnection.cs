using System;
using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class FunctionConnection : IConnectionTypeDeclaration
{
	public const string ID = "NodeFunction";

	public string Identifier => "NodeFunction";

	public Type Type => typeof(NodeFunction);

	public Color Color => new Color(30f / 255f, 30f / 255f, 30f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
