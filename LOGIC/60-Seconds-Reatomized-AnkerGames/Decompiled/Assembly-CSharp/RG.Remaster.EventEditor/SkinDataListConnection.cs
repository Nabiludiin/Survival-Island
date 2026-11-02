using System;
using NodeEditorFramework;
using RG.Remaster.Survival;
using UnityEngine;

namespace RG.Remaster.EventEditor;

public class SkinDataListConnection : IConnectionTypeDeclaration
{
	public const string ID = "SkinDataList";

	public string Identifier => "SkinDataList";

	public Type Type => typeof(SkinDataList);

	public Color Color => new Color(45f / 255f, 89f / 180f, 120f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
