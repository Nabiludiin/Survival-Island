using System;
using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class EventTagConnection : IConnectionTypeDeclaration
{
	public const string ID = "EventTagConection";

	public string Identifier => "EventTagConection";

	public Type Type => typeof(EventTag);

	public Color Color => new Color(1f, 105f / 255f, 180f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
