using System;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class RemediumConnection : IConnectionTypeDeclaration
{
	public const string ID = "Remedium";

	public string Identifier => "Remedium";

	public Type Type => typeof(Remedium);

	public Color Color => new Color(1f / 17f, 1f / 17f, 200f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
