using System;
using System.Collections;
using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

public class ListConnection : IConnectionTypeDeclaration
{
	public static string ID = "CNE_List";

	public string Identifier => ID;

	public Type Type => typeof(IList);

	public Color Color => new Color(120f / 255f, 180f / 255f, 250f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
