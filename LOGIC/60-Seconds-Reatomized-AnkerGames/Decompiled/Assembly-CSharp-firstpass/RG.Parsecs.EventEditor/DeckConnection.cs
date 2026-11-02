using System;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class DeckConnection : IConnectionTypeDeclaration
{
	public const string ID = "Deck";

	public string Identifier => "Deck";

	public Type Type => typeof(Deck);

	public Color Color => new Color(0f, 80f / 255f, 150f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/In_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
