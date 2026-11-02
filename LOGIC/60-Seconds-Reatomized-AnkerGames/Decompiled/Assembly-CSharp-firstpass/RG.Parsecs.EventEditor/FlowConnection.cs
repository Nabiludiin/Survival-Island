using System;
using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class FlowConnection : IConnectionTypeDeclaration
{
	public const string ID = "Flow";

	public string Identifier => "Flow";

	public Type Type => typeof(void);

	public Color Color => Color.white;

	public string InKnobTex => "Textures/Knob_Flow.png";

	public string OutKnobTex => "Textures/Knob_Flow.png";

	public string InKnobFilledTex => "Textures/Knob_Flow_Filled.png";

	public string OutKnobFilledTex => "Textures/Knob_Flow_Filled.png";
}
