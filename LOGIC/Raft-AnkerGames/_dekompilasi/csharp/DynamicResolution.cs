using UnityEngine;

public class DynamicResolution : MonoBehaviour
{
	private float maxResolutionWidthScale = 1f;

	private float maxResolutionHeightScale = 1f;

	private float minResolutionWidthScale = 0.5f;

	private float minResolutionHeightScale = 0.5f;

	public static float m_widthScale = 1f;

	public static float m_heightScale = 1f;

	private static double targetFps = 60.0;

	private static double targetFrameTime = 1000.0 / targetFps;

	private uint m_frameCount;

	private const uint kNumFrameTimings = 1u;

	private FrameTiming[] frameTimings = new FrameTiming[1];

	private double m_gpuFrameTime;

	private double m_gpuFrameTimeDelta;

	public static uint m_scaleRaiseCounter = 0u;

	public static bool Enabled = true;

	private void OnDestroy()
	{
		ScalableBufferManager.ResizeBuffers(1f, 1f);
	}

	private void Update()
	{
		float widthScale = m_widthScale;
		float heightScale = m_heightScale;
		if (Enabled)
		{
			DetermineResolution();
		}
		else
		{
			m_widthScale = 1f;
			m_heightScale = 1f;
		}
		if (m_widthScale != widthScale || m_heightScale != heightScale)
		{
			ScalableBufferManager.ResizeBuffers(m_widthScale, m_heightScale);
		}
	}

	private void DecreaseScale(float value)
	{
		m_heightScale = Mathf.Max(minResolutionHeightScale, m_heightScale + value);
		m_widthScale = Mathf.Max(minResolutionWidthScale, m_widthScale + value);
	}

	private void IncrementScale(float value)
	{
		m_heightScale = Mathf.Min(maxResolutionHeightScale, m_heightScale + value);
		m_widthScale = Mathf.Min(maxResolutionWidthScale, m_widthScale + value);
	}

	private void DetermineResolution()
	{
		if (m_frameCount < 1)
		{
			m_frameCount++;
			return;
		}
		FrameTimingManager.CaptureFrameTimings();
		FrameTimingManager.GetLatestTimings(1u, frameTimings);
		if ((long)frameTimings.Length < 1L || frameTimings[0].cpuTimeFrameComplete < frameTimings[0].cpuTimePresentCalled)
		{
			return;
		}
		if (m_gpuFrameTime != 0.0)
		{
			m_gpuFrameTimeDelta = frameTimings[0].gpuFrameTime - m_gpuFrameTime;
		}
		m_gpuFrameTime = frameTimings[0].gpuFrameTime;
		double num = targetFrameTime - m_gpuFrameTime;
		if (num < 0.0)
		{
			m_scaleRaiseCounter = 0u;
			float value = (float)(num / targetFrameTime);
			DecreaseScale(value);
			return;
		}
		if (m_gpuFrameTimeDelta < 0.0)
		{
			m_scaleRaiseCounter += 10u;
		}
		else
		{
			double num2 = targetFrameTime * 0.05999999865889549;
			double num3 = targetFrameTime * 0.03500000014901161;
			if (num > num2 && m_gpuFrameTimeDelta < num3)
			{
				m_scaleRaiseCounter += 3u;
			}
		}
		if (m_scaleRaiseCounter >= 360)
		{
			m_scaleRaiseCounter = 0u;
			float t = (Mathf.Clamp((float)(num / targetFrameTime), 0.1f, 0.5f) - 0.1f) / 0.4f;
			float value2 = 0.06f * Mathf.Lerp(0.25f, 1f, t);
			IncrementScale(value2);
		}
	}
}
