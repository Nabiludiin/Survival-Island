using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class ItemListConnection : IConnectionTypeDeclaration
{
	public const string ID = "ItemList";

	public string Identifier => "ItemList";

	public Type Type => typeof(List<Character>);

	public Color Color => new Color(50f / 255f, 50f / 255f, 200f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
