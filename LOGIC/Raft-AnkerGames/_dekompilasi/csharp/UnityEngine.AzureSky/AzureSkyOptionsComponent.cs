using System;

namespace UnityEngine.AzureSky;

[Serializable]
public class AzureSkyOptionsComponent
{
	public bool startAtCurrentTime;

	public bool startAtCurrentDate;

	public bool followMainCamera = true;

	public bool applyDayChangeTransition = true;

	public float dayChangeTransitionTime = 10f;

	public float planetRadius;

	public float lightSpeed = 50f;

	public float mieDepth;

	public float sunSize = 1.5f;

	public float moonSize = 10f;

	public Vector3 starfieldPosition;

	public Vector3 starfieldColor;

	public int particlesMode = 1;

	public bool keepWeatherUpdate = true;

	public int cloudMode = 1;

	public int repeatMode;

	public int sunsetColorMode;

	public int wavelengthMode;

	public int shaderMode;

	public int useReflectionProbe;

	public int reflectionProbeRefreshMode;

	public float reflectionProbeUpdateTime;

	public bool reflectionProbeUpdateAtFirstFrame = true;
}
