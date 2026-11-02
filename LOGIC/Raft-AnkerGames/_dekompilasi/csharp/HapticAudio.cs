using System;
using System.IO;
using UnityEngine;

[Serializable]
public class HapticAudio
{
	public string filepath;

	public bool loop;

	public float rangeSqr;

	public float scale = 1f;

	[HideInInspector]
	public float volume = 1f;

	private float[] samples;

	private float frequencey;

	private float startTime;

	private bool isPlaying;

	private static float[] Convert16BitByteArrayToAudioClipData(byte[] source, int headerOffset, int dataSize)
	{
		int num = BitConverter.ToInt32(source, headerOffset);
		headerOffset += 4;
		int num2 = 2;
		int num3 = num / num2;
		float[] array = new float[num3];
		short num4 = short.MaxValue;
		int num5 = 0;
		for (int i = 0; i < num3; i++)
		{
			num5 = i * num2 + headerOffset;
			array[i] = (float)BitConverter.ToInt16(source, num5) / (float)num4;
		}
		return array;
	}

	public void CreateAudioSources()
	{
		if (filepath != string.Empty)
		{
			byte[] array = File.ReadAllBytes(Application.streamingAssetsPath + "/Haptics/" + filepath + ".wav");
			int num = BitConverter.ToInt32(array, 16);
			BitConverter.ToUInt16(array, 20);
			BitConverter.ToUInt16(array, 22);
			int num2 = BitConverter.ToInt32(array, 24);
			BitConverter.ToUInt16(array, 34);
			int num3 = 20 + num + 4;
			int dataSize = BitConverter.ToInt32(array, num3);
			samples = Convert16BitByteArrayToAudioClipData(array, num3, dataSize);
			frequencey = num2;
		}
	}

	public bool IsPlaying()
	{
		return isPlaying;
	}

	public void Start()
	{
		startTime = 0f;
		isPlaying = true;
	}

	public void Stop()
	{
		isPlaying = false;
	}

	public bool Update(out float intensity)
	{
		if (isPlaying)
		{
			if (Time.timeScale == 0f)
			{
				intensity = 0f;
			}
			else
			{
				int num = Mathf.FloorToInt(startTime * frequencey);
				if (num >= 0 && num < samples.Length / 2)
				{
					int num2 = num * 2;
					int num3 = num2 + 1;
					float num4 = Mathf.Clamp01(Mathf.Abs(samples[num2])) * 0.25f * scale * volume;
					Mathf.Clamp01(Mathf.Abs(samples[num3]));
					_ = scale;
					_ = volume;
					intensity = num4;
					startTime += 1f / 60f;
				}
				else
				{
					intensity = 0f;
					if (!loop)
					{
						return false;
					}
					startTime = 0f;
				}
			}
			return true;
		}
		intensity = 0f;
		return false;
	}
}
