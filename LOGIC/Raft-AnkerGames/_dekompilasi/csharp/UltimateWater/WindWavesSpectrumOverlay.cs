using System;
using UnityEngine;

namespace UltimateWater;

public class WindWavesSpectrumOverlay
{
	private Vector2[][] _SpectrumData;

	private Texture2D _Texture;

	private bool _TextureDirty = true;

	private readonly WindWaves _WindWaves;

	public Texture2D Texture
	{
		get
		{
			if (_TextureDirty)
			{
				ValidateTexture();
			}
			return _Texture;
		}
	}

	public event Action Cleared;

	public WindWavesSpectrumOverlay(WindWaves windWaves)
	{
		_WindWaves = windWaves;
		_SpectrumData = new Vector2[4][];
		for (int i = 0; i < 4; i++)
		{
			_SpectrumData[i] = new Vector2[windWaves.FinalResolution * windWaves.FinalResolution];
		}
	}

	public void Destroy()
	{
		_SpectrumData = null;
		Cleared = null;
		if (_Texture != null)
		{
			UnityEngine.Object.Destroy(_Texture);
			_Texture = null;
		}
	}

	public Vector2[] GetSpectrumDataDirect(int tileIndex)
	{
		return _SpectrumData[tileIndex];
	}

	public void Refresh()
	{
		int finalResolution = _WindWaves.FinalResolution;
		int num = finalResolution * finalResolution;
		for (int i = 0; i < 4; i++)
		{
			Vector2[] array = _SpectrumData[i];
			if (array.Length == num)
			{
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = new Vector2(0f, 0f);
				}
			}
			else
			{
				_SpectrumData[i] = new Vector2[num];
			}
		}
		_TextureDirty = true;
		if (Cleared != null)
		{
			Cleared();
		}
	}

	private void ValidateTexture()
	{
		_TextureDirty = false;
		int finalResolution = _WindWaves.FinalResolution;
		int num = finalResolution << 1;
		if (_Texture != null && _Texture.width != num)
		{
			UnityEngine.Object.Destroy(_Texture);
			_Texture = null;
		}
		if (_Texture == null)
		{
			_Texture = new Texture2D(num, num, TextureFormat.RGHalf, mipChain: false, linear: true)
			{
				filterMode = FilterMode.Point
			};
		}
		for (int i = 0; i < 4; i++)
		{
			Vector2[] array = _SpectrumData[i];
			int num2 = ((i == 1 || i == 3) ? finalResolution : 0);
			int num3 = ((i == 2 || i == 3) ? finalResolution : 0);
			for (int num4 = finalResolution - 1; num4 >= 0; num4--)
			{
				for (int num5 = finalResolution - 1; num5 >= 0; num5--)
				{
					Vector2 vector = array[num4 * finalResolution + num5];
					_Texture.SetPixel(num2 + num4, num3 + num5, new Color(vector.x, vector.y, 0f, 0f));
				}
			}
		}
		_Texture.Apply(updateMipmaps: false, makeNoLongerReadable: false);
	}
}
