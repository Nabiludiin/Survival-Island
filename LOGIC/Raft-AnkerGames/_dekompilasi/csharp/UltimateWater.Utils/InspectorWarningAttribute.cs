using System;
using UnityEngine;

namespace UltimateWater.Utils;

[AttributeUsage(AttributeTargets.Field)]
public class InspectorWarningAttribute : PropertyAttribute
{
	public enum InfoType
	{
		Note,
		Warning,
		Error
	}

	public readonly string MethodName;

	public readonly InfoType Type;

	public InspectorWarningAttribute(string methodName, InfoType type)
	{
		MethodName = methodName;
		Type = type;
	}
}
