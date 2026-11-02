using System;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class NeedsConnection : IConnectionTypeDeclaration
{
	public string Identifier => "Needs";

	public Type Type => typeof(CharacterNeed);

	public Color Color => new Color(0f, 250f / 255f, 150f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
