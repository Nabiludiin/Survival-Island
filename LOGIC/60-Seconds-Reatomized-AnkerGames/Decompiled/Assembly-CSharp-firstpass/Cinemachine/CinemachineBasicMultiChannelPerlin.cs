using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine;

[DocumentationSorting(8f, DocumentationSortingAttribute.Level.UserRef)]
[AddComponentMenu("")]
[RequireComponent(typeof(CinemachinePipeline))]
[SaveDuringPlay]
public class CinemachineBasicMultiChannelPerlin : CinemachineComponentBase
{
	[HideInInspector]
	[Tooltip("The asset containing the Noise Profile.  Define the frequencies and amplitudes there to make a characteristic noise profile.  Make your own or just use one of the many presets.")]
	[FormerlySerializedAs("m_Definition")]
	public NoiseSettings m_NoiseProfile;

	[Tooltip("Gain to apply to the amplitudes defined in the NoiseSettings asset.  1 is normal.  Setting this to 0 completely mutes the noise.")]
	public float m_AmplitudeGain = 1f;

	[Tooltip("Scale factor to apply to the frequencies defined in the NoiseSettings asset.  1 is normal.  Larger magnitudes will make the noise shake more rapidly.")]
	public float m_FrequencyGain = 1f;

	private bool mInitialized;

	private float mNoiseTime;

	private Vector3 mNoiseOffsets = Vector3.zero;

	public override bool IsValid
	{
		get
		{
			if (enabled)
			{
				return m_NoiseProfile != null;
			}
			return false;
		}
	}

	public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Noise;

	public override void MutateCameraState(ref CameraState curState, float deltaTime)
	{
		if (IsValid && !(deltaTime < 0f))
		{
			if (!mInitialized)
			{
				Initialize();
			}
			mNoiseTime += deltaTime * m_FrequencyGain;
			curState.PositionCorrection += curState.CorrectedOrientation * GetCombinedFilterResults(m_NoiseProfile.PositionNoise, mNoiseTime, mNoiseOffsets) * m_AmplitudeGain;
			Quaternion quaternion = Quaternion.Euler(GetCombinedFilterResults(m_NoiseProfile.OrientationNoise, mNoiseTime, mNoiseOffsets) * m_AmplitudeGain);
			curState.OrientationCorrection *= quaternion;
		}
	}

	private void Initialize()
	{
		mInitialized = true;
		mNoiseTime = 0f;
		mNoiseOffsets = new Vector3(Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
	}

	private static Vector3 GetCombinedFilterResults(NoiseSettings.TransformNoiseParams[] noiseParams, float time, Vector3 noiseOffsets)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		if (noiseParams != null)
		{
			for (int i = 0; i < noiseParams.Length; i++)
			{
				NoiseSettings.TransformNoiseParams transformNoiseParams = noiseParams[i];
				Vector3 vector = new Vector3(transformNoiseParams.X.Frequency, transformNoiseParams.Y.Frequency, transformNoiseParams.Z.Frequency) * time;
				vector += noiseOffsets;
				Vector3 vector2 = new Vector3(Mathf.PerlinNoise(vector.x, 0f) - 0.5f, Mathf.PerlinNoise(vector.y, 0f) - 0.5f, Mathf.PerlinNoise(vector.z, 0f) - 0.5f);
				num += vector2.x * transformNoiseParams.X.Amplitude;
				num2 += vector2.y * transformNoiseParams.Y.Amplitude;
				num3 += vector2.z * transformNoiseParams.Z.Amplitude;
			}
		}
		return new Vector3(num, num2, num3);
	}
}
