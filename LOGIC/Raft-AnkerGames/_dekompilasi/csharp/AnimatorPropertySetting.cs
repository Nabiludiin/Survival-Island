using System;

[Serializable]
public class AnimatorPropertySetting
{
	public AnimatorPropertyType propertyType;

	public AIStateEvent eventType;

	public string parameterName;

	public int intValue;

	public float floatValue;

	public bool lerp;

	public float lerpSpeed;

	public bool boolValue;

	public bool IsFloatType()
	{
		return propertyType == AnimatorPropertyType.Float;
	}

	public bool IsBoolType()
	{
		return propertyType == AnimatorPropertyType.Bool;
	}

	public bool IsIntType()
	{
		return propertyType == AnimatorPropertyType.Int;
	}
}
