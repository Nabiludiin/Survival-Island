using System;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class ItemConnection : IConnectionTypeDeclaration
{
	public const string ID = "Item";

	public string Identifier => "Item";

	public Type Type => typeof(IItem);

	public Color Color => new Color(0f, 0f, 1f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
