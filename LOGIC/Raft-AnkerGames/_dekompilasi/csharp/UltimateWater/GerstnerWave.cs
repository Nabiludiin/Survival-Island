using UnityEngine;

namespace UltimateWater;

public class GerstnerWave
{
	public Vector2 Direction;

	public float Amplitude;

	public float Offset;

	public float Frequency;

	public float Speed;

	public GerstnerWave()
	{
		Direction = new Vector2(0f, 1f);
		Frequency = 1f;
	}

	public GerstnerWave(WaterWave wave, Vector2[] scaleOffsets)
	{
		float w = wave._W;
		float num = (scaleOffsets[wave._ScaleIndex].x * wave._Nkx + scaleOffsets[wave._ScaleIndex].y * wave._Nky) * wave.K;
		Direction = new Vector2(wave._Nkx, wave._Nky);
		Amplitude = wave._Amplitude;
		Offset = num + wave._Offset;
		Frequency = wave.K;
		Speed = w;
	}

	public GerstnerWave(Vector2 direction, float amplitude, float offset, float frequency, float speed)
	{
		Direction = direction;
		Amplitude = amplitude;
		Offset = offset;
		Frequency = frequency;
		Speed = speed;
	}
}
