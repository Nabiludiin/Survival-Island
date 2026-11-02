using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class CharacterListConnection : IConnectionTypeDeclaration
{
	public const string ID = "CharacterList";

	public string Identifier => "CharacterList";

	public Type Type => typeof(List<Character>);

	public Color Color => new Color(1f, 125f / 255f, 75f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
