using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

public class GoalConnectionType : IConnectionTypeDeclaration
{
	public const string ID = "CNE_Goal";

	public string Identifier => "CNE_Goal";

	public Type Type => typeof(Goal);

	public Color Color => new Color(1f, 225f / 255f, 0f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
