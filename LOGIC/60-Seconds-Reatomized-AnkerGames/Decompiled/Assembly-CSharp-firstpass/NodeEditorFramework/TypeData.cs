using System;
using NodeEditorFramework.Utilities;
using UnityEngine;

namespace NodeEditorFramework;

public class TypeData
{
	private IConnectionTypeDeclaration declaration;

	public string Identifier { get; private set; }

	public Type Type { get; private set; }

	public Color Color { get; private set; }

	public Texture2D InKnobTex { get; private set; }

	public Texture2D OutKnobTex { get; private set; }

	public Texture2D InKnobFilledTex { get; private set; }

	public Texture2D OutKnobFilledTex { get; private set; }

	internal TypeData(IConnectionTypeDeclaration typeDecl)
	{
		Identifier = typeDecl.Identifier;
		declaration = typeDecl;
		Type = declaration.Type;
		Color = declaration.Color;
		InKnobTex = ResourceManager.GetTintedTexture(declaration.InKnobTex, Color);
		OutKnobTex = ResourceManager.GetTintedTexture(declaration.OutKnobTex, Color);
		InKnobFilledTex = ResourceManager.GetTintedTexture(declaration.InKnobFilledTex, Color);
		OutKnobFilledTex = ResourceManager.GetTintedTexture(declaration.OutKnobFilledTex, Color);
		if (!isValid())
		{
			throw new DataMisalignedException("Type Declaration " + typeDecl.Identifier + " contains invalid data!");
		}
	}

	public TypeData(Type type)
	{
		Identifier = type.Name;
		declaration = null;
		Type = type;
		Color = Color.white;
		byte[] bytes = BitConverter.GetBytes(type.GetHashCode());
		Color = new Color(Mathf.Pow((float)(int)bytes[0] / 255f, 0.5f), Mathf.Pow((float)(int)bytes[1] / 255f, 0.5f), Mathf.Pow((float)(int)bytes[2] / 255f, 0.5f));
		InKnobTex = ResourceManager.GetTintedTexture("Textures/In_Knob.png", Color);
		OutKnobTex = ResourceManager.GetTintedTexture("Textures/Out_Knob.png", Color);
		InKnobFilledTex = ResourceManager.GetTintedTexture("Textures/In_Knob_Filled.png", Color);
		OutKnobFilledTex = ResourceManager.GetTintedTexture("Textures/In_Knob_Filled.png", Color);
	}

	public bool isValid()
	{
		if (Type != null && InKnobTex != null)
		{
			return OutKnobTex != null;
		}
		return false;
	}
}
