using System;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class CharacterStatusConnection : IConnectionTypeDeclaration
{
	public const string ID = "CharacterStatusConnection";

	public string Identifier => "CharacterStatusConnection";

	public Type Type => typeof(CharacterStatus);

	public Color Color => new Color(1f, 0f, 0f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
