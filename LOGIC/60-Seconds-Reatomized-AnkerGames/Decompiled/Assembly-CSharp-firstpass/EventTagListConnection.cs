using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using UnityEngine;

public class EventTagListConnection : IConnectionTypeDeclaration
{
	public const string ID = "EventTagListConnection";

	public string Identifier => "EventTagListConnection";

	public Type Type => typeof(EventTagList);

	public Color Color => new Color(1f, 105f / 255f, 0f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
