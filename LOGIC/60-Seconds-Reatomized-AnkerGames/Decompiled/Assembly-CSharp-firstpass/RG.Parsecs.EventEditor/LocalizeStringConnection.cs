using System;
using I2.Loc;
using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class LocalizeStringConnection : IConnectionTypeDeclaration
{
	public const string ID = "LocalizedString";

	public string Identifier => "LocalizedString";

	public Type Type => typeof(LocalizedString);

	public Color Color => new Color(0f, 0f, 150f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/In_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
