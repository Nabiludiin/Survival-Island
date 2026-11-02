using UnityEngine;

public struct EnvironmentProbe_ColorWeight
{
	public EnvironmentProbe_ColorGroup colorGroup;

	public float weight;

	public Color SkyColorWeighted => colorGroup.skyColor * weight;

	public Color EquatorColorWeighted => colorGroup.equatorColor * weight;

	public Color GroundColorWeighted => colorGroup.groundColor * weight;

	public float ReflectionModifierWeighted => colorGroup.reflectionModifier * weight;

	public float ReflectionModifierNoneWeighted => colorGroup.reflectionModifierNone * weight;

	public EnvironmentProbe_ColorGroup ColorGroupWeighted => new EnvironmentProbe_ColorGroup(SkyColorWeighted, EquatorColorWeighted, GroundColorWeighted, ReflectionModifierWeighted, ReflectionModifierNoneWeighted);

	public EnvironmentProbe_ColorWeight(Color skyColor, Color equatorColor, Color groundColor, float reflectionModifier, float reflectionModifierNone, float weight)
	{
		colorGroup = new EnvironmentProbe_ColorGroup(skyColor, equatorColor, groundColor, reflectionModifier, reflectionModifierNone);
		this.weight = weight;
	}
}
