using System.Collections.Generic;
using UnityEngine;

namespace UltimateWater.Internal;

public class UtilityShaderVariants
{
	private readonly Dictionary<int, Material> _Materials;

	private static UtilityShaderVariants _Instance;

	public static UtilityShaderVariants Instance
	{
		get
		{
			if (_Instance == null)
			{
				return _Instance = new UtilityShaderVariants();
			}
			return _Instance;
		}
	}

	public Material GetVariant(Shader shader, string keywords)
	{
		int key = shader.GetInstanceID() ^ keywords.GetHashCode();
		if (!_Materials.TryGetValue(key, out var value))
		{
			value = new Material(shader)
			{
				hideFlags = HideFlags.DontSave,
				shaderKeywords = keywords.Split(' ')
			};
			_Materials[key] = value;
		}
		return value;
	}

	private UtilityShaderVariants()
	{
		_Materials = new Dictionary<int, Material>();
	}
}
