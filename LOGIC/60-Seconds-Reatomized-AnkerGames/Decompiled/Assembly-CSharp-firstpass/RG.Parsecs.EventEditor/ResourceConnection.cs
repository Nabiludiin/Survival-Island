using System;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class ResourceConnection : IConnectionTypeDeclaration
{
	public const string ID = "Resources";

	public string Identifier => "Resources";

	public Type Type => typeof(Resource);

	public Color Color => new Color(200f / 255f, 130f / 255f, 50f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
