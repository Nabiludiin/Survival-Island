using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateWater;

public class ShaderVariant
{
	private readonly Dictionary<string, bool> _UnityKeywords;

	private readonly Dictionary<string, bool> _WaterKeywords;

	private readonly Dictionary<string, string> _SurfaceShaderParts;

	private readonly Dictionary<string, string> _VolumeShaderParts;

	public ShaderVariant()
	{
		_UnityKeywords = new Dictionary<string, bool>();
		_WaterKeywords = new Dictionary<string, bool>();
		_SurfaceShaderParts = new Dictionary<string, string>();
		_VolumeShaderParts = new Dictionary<string, string>();
	}

	public void SetUnityKeyword(string keyword, bool value)
	{
		if (value)
		{
			_UnityKeywords[keyword] = true;
		}
		else
		{
			_UnityKeywords.Remove(keyword);
		}
	}

	public void SetWaterKeyword(string keyword, bool value)
	{
		if (value)
		{
			_WaterKeywords[keyword] = true;
		}
		else
		{
			_WaterKeywords.Remove(keyword);
		}
	}

	public void SetAdditionalSurfaceCode(string keyword, string code)
	{
		if (code != null)
		{
			_SurfaceShaderParts[keyword] = code;
		}
		else
		{
			_SurfaceShaderParts.Remove(keyword);
		}
	}

	public void SetAdditionalVolumeCode(string keyword, string code)
	{
		if (code != null)
		{
			_VolumeShaderParts[keyword] = code;
		}
		else
		{
			_VolumeShaderParts.Remove(keyword);
		}
	}

	public bool IsUnityKeywordEnabled(string keyword)
	{
		if (_UnityKeywords.TryGetValue(keyword, out var _))
		{
			return true;
		}
		return false;
	}

	public bool IsWaterKeywordEnabled(string keyword)
	{
		if (_WaterKeywords.TryGetValue(keyword, out var _))
		{
			return true;
		}
		return false;
	}

	public string GetAdditionalSurfaceCode()
	{
		StringBuilder stringBuilder = new StringBuilder(512);
		foreach (string value in _SurfaceShaderParts.Values)
		{
			stringBuilder.Append(value);
		}
		return stringBuilder.ToString();
	}

	public string GetAdditionalVolumeCode()
	{
		StringBuilder stringBuilder = new StringBuilder(512);
		foreach (string value in _VolumeShaderParts.Values)
		{
			stringBuilder.Append(value);
		}
		return stringBuilder.ToString();
	}

	public string[] GetUnityKeywords()
	{
		string[] array = new string[_UnityKeywords.Count];
		int num = 0;
		foreach (string key in _UnityKeywords.Keys)
		{
			array[num++] = key;
		}
		return array;
	}

	public string[] GetWaterKeywords()
	{
		string[] array = new string[_WaterKeywords.Count];
		int num = 0;
		foreach (string key in _WaterKeywords.Keys)
		{
			array[num++] = key;
		}
		return array;
	}

	public string GetKeywordsString()
	{
		StringBuilder stringBuilder = new StringBuilder(512);
		bool flag = false;
		foreach (string item in _WaterKeywords.Keys.OrderBy((string k) => k))
		{
			if (flag)
			{
				stringBuilder.Append(' ');
			}
			else
			{
				flag = true;
			}
			stringBuilder.Append(item);
		}
		foreach (string item2 in _UnityKeywords.Keys.OrderBy((string k) => k))
		{
			if (flag)
			{
				stringBuilder.Append(' ');
			}
			else
			{
				flag = true;
			}
			stringBuilder.Append(item2);
		}
		foreach (string item3 in _SurfaceShaderParts.Keys.OrderBy((string k) => k))
		{
			if (flag)
			{
				stringBuilder.Append(' ');
			}
			else
			{
				flag = true;
			}
			stringBuilder.Append(item3);
		}
		return stringBuilder.ToString();
	}
}
