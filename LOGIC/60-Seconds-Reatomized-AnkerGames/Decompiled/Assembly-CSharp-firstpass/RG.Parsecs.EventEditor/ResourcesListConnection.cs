using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class ResourcesListConnection : IConnectionTypeDeclaration
{
	public const string ID = "ResourcesList";

	public string Identifier => "ResourcesList";

	public Type Type => typeof(List<Resource>);

	public Color Color => new Color(200f / 255f, 160f / 255f, 50f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
