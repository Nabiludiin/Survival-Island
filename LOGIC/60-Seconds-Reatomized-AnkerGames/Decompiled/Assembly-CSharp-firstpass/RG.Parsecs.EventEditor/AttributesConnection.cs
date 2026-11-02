using System;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class AttributesConnection : IConnectionTypeDeclaration
{
	public const string ID = "Attributes";

	public string Identifier => "Attributes";

	public Type Type => typeof(CharacterAttribute);

	public Color Color => new Color(0f, 150f / 255f, 150f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
