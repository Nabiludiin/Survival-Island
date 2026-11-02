using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class SoundSlotConnection : IConnectionTypeDeclaration
{
	public const string ID = "SoundSlot";

	public string Identifier => "SoundSlot";

	public Type Type => typeof(SoundSlot);

	public Color Color => new Color(1f, 180f / 255f, 25f / 255f);

	public string InKnobTex => "Textures/In_Knob.png";

	public string OutKnobTex => "Textures/Out_Knob.png";

	public string InKnobFilledTex => "Textures/In_Knob_Filled.png";

	public string OutKnobFilledTex => "Textures/In_Knob_Filled.png";
}
