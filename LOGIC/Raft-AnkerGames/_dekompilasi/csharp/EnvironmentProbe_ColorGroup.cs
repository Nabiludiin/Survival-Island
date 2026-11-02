using System;
using UnityEngine;

[Serializable]
public class EnvironmentProbe_ColorGroup
{
	public Color skyColor;

	public Color equatorColor;

	public Color groundColor;

	public float reflectionModifier;

	public float reflectionModifierNone;

	public EnvironmentProbe_ColorGroup()
	{
	}

	public EnvironmentProbe_ColorGroup(Color skyColor, Color equatorColor, Color groundColor, float reflectionModifier, float reflectionModifierNone)
	{
		this.skyColor = skyColor;
		this.equatorColor = equatorColor;
		this.groundColor = groundColor;
		this.reflectionModifier = reflectionModifier;
		this.reflectionModifierNone = reflectionModifierNone;
	}
}
