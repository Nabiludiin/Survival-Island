using System;
using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class ActionConditionConnection : IConnectionTypeDeclaration
{
	public const string ID = "ActionCondition";

	public string Identifier => "ActionCondition";

	public Type Type => typeof(BaseActionCondition);

	public Color Color => new Color(180f / 255f, 130f / 255f, 1f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
