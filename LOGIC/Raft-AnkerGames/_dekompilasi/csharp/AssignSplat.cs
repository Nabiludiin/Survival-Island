using System.Collections.Generic;
using UnityEngine;

public class AssignSplat : MonoBehaviour
{
	[Range(0.05f, 0.95f)]
	public float fadeOverlap;

	public List<SplatTexture> splatTextures = new List<SplatTexture>();

	public int alphaTextureIndex;

	private void OnValidate()
	{
		for (int num = splatTextures.Count - 1; num >= 0; num--)
		{
			SplatTexture splatTexture = ((num - 1 >= 0) ? splatTextures[num - 1] : null);
			SplatTexture splatTexture2 = splatTextures[num];
			SplatTexture splatTexture3 = ((num + 1 < splatTextures.Count) ? splatTextures[num + 1] : null);
			if (splatTexture != null)
			{
				splatTexture2.interval.x = splatTexture.interval.y;
			}
			if (splatTexture3 != null)
			{
				splatTexture2.interval.y = splatTexture3.interval.x;
			}
			if (num == 0)
			{
				splatTexture2.interval.x = 0f;
			}
			else if (num == splatTextures.Count - 1)
			{
				splatTexture2.interval.y = 1f;
			}
		}
	}

	public void DoShit()
	{
		TerrainData terrainData = GetComponent<Terrain>().terrainData;
		float[,,] alphamaps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
		for (int i = 0; i < terrainData.alphamapHeight; i++)
		{
			for (int j = 0; j < terrainData.alphamapWidth; j++)
			{
				float num = (float)i / (float)terrainData.alphamapHeight;
				float num2 = (float)j / (float)terrainData.alphamapWidth;
				terrainData.GetHeight(Mathf.RoundToInt(num * (float)terrainData.heightmapResolution), Mathf.RoundToInt(num2 * (float)terrainData.heightmapResolution));
				Vector3 interpolatedNormal = terrainData.GetInterpolatedNormal(num, num2);
				float steepness = terrainData.GetSteepness(num, num2);
				float num3 = Mathf.Clamp01(steepness * steepness / ((float)terrainData.heightmapResolution / 5f));
				Mathf.Clamp01(1f - num3);
				float[] array = new float[terrainData.alphamapLayers];
				float num4 = 0f;
				for (int k = 0; k < splatTextures.Count; k++)
				{
					SplatTexture splatTexture = splatTextures[k];
					float amountFromNormal = splatTexture.GetAmountFromNormal(interpolatedNormal.y, fadeOverlap);
					array[splatTexture.textureIndex] = amountFromNormal;
					num4 += amountFromNormal;
				}
				float num5 = 0f;
				int num6 = 0;
				float num7 = 0f;
				for (int l = 0; l < array.Length; l++)
				{
					array[l] /= num4;
					num5 += array[l];
					if (array[l] > num7)
					{
						num7 = array[l];
						num6 = l;
					}
				}
				if (num5 != 1f)
				{
					float num8 = 1f - num5;
					array[num6] += num8;
					num5 += num8;
				}
				float num9 = num5 - array[splatTextures[0].textureIndex];
				float num10 = 1f - array[splatTextures[0].textureIndex];
				float num11 = 0f;
				for (int m = 0; m < array.Length; m++)
				{
					num11 += alphamaps[j, i, m];
				}
				float[] array2 = new float[terrainData.alphamapLayers];
				float num12 = 0f;
				for (int n = 0; n < array.Length; n++)
				{
					array2[n] = alphamaps[j, i, n] / num11;
					if (IsPaintTextureIndex(n))
					{
						array2[n] *= num10;
						num12 += array2[n];
					}
				}
				float num13 = 1f - num12 / num9;
				for (int num14 = 0; num14 < array.Length; num14++)
				{
					if (num14 != splatTextures[0].textureIndex)
					{
						array[num14] *= num13;
					}
				}
				for (int num15 = 0; num15 < array2.Length; num15++)
				{
					if (IsPaintTextureIndex(num15))
					{
						array[num15] = array2[num15];
					}
				}
				for (int num16 = 0; num16 < terrainData.alphamapLayers; num16++)
				{
					if (num16 != alphaTextureIndex)
					{
						alphamaps[j, i, num16] = array[num16];
					}
				}
			}
		}
		terrainData.SetAlphamaps(0, 0, alphamaps);
	}

	private bool IsPaintTextureIndex(int textureIndex)
	{
		for (int i = 0; i < splatTextures.Count; i++)
		{
			if (splatTextures[i].textureIndex == textureIndex)
			{
				return false;
			}
		}
		return true;
	}
}
