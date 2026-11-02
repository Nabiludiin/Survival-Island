using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.AzureSky;

[ExecuteInEditMode]
[AddComponentMenu("Azure[Sky]/Sky Controller")]
public class AzureSkyController : MonoBehaviour
{
	public static class Uniforms
	{
		internal static readonly int _CullMode = Shader.PropertyToID("_Azure_CullMode");

		internal static readonly int _SunTexture = Shader.PropertyToID("_Azure_SunTexture");

		internal static readonly int _MoonTexture = Shader.PropertyToID("_Azure_MoonTexture");

		internal static readonly int _CloudNoise = Shader.PropertyToID("_Azure_CloudNoise");

		internal static readonly int _StarfieldTexture = Shader.PropertyToID("_Azure_StarfieldTexture");

		internal static readonly int _StarNoiseTexture = Shader.PropertyToID("_Azure_StarNoiseTexture");

		internal static readonly int _Kr = Shader.PropertyToID("_Azure_Kr");

		internal static readonly int _Km = Shader.PropertyToID("_Azure_Km");

		internal static readonly int _Scattering = Shader.PropertyToID("_Azure_Scattering");

		internal static readonly int _NightIntensity = Shader.PropertyToID("_Azure_NightIntensity");

		internal static readonly int _RayleighColor = Shader.PropertyToID("_Azure_RayleighColor");

		internal static readonly int _MieColor = Shader.PropertyToID("_Azure_MieColor");

		internal static readonly int _SunDiskIntensity = Shader.PropertyToID("_Azure_SunDiskIntensity");

		internal static readonly int _SunDiskColor = Shader.PropertyToID("_Azure_SunDiskColor");

		internal static readonly int _Exposure = Shader.PropertyToID("_Azure_Exposure");

		internal static readonly int _Pi316 = Shader.PropertyToID("_Azure_Pi316");

		internal static readonly int _Pi14 = Shader.PropertyToID("_Azure_Pi14");

		internal static readonly int _Pi = Shader.PropertyToID("_Azure_Pi");

		internal static readonly int _LightSpeed = Shader.PropertyToID("_Azure_LightSpeed");

		internal static readonly int _MieDepth = Shader.PropertyToID("_Azure_MieDepth");

		internal static readonly int _SunSize = Shader.PropertyToID("_Azure_SunSize");

		internal static readonly int _MoonSize = Shader.PropertyToID("_Azure_MoonSize");

		internal static readonly int _SunDirection = Shader.PropertyToID("_Azure_SunDirection");

		internal static readonly int _MoonDirection = Shader.PropertyToID("_Azure_MoonDirection");

		internal static readonly int _UpMatrix = Shader.PropertyToID("_Azure_UpMatrix");

		internal static readonly int _MoonMatrix = Shader.PropertyToID("_Azure_MoonMatrix");

		internal static readonly int _SunMatrix = Shader.PropertyToID("_Azure_SunMatrix");

		internal static readonly int _RelativeSunMatrix = Shader.PropertyToID("_Azure_RelativeSunMatrix");

		internal static readonly int _StarMatrix = Shader.PropertyToID("_Azure_StarMatrix");

		internal static readonly int _NoiseMatrix = Shader.PropertyToID("_Azure_NoiseMatrix");

		internal static readonly int _Br = Shader.PropertyToID("_Azure_Br");

		internal static readonly int _Bm = Shader.PropertyToID("_Azure_Bm");

		internal static readonly int _MieG = Shader.PropertyToID("_Azure_MieG");

		internal static readonly int _SunsetColorMode = Shader.PropertyToID("_Azure_SunsetColorMode");

		internal static readonly int _MoonColor = Shader.PropertyToID("_Azure_MoonColor");

		internal static readonly int _MoonBrightColor = Shader.PropertyToID("_Azure_MoonBrightColor");

		internal static readonly int _MoonBrightRange = Shader.PropertyToID("_Azure_MoonBrightRange");

		internal static readonly int _MoonEmission = Shader.PropertyToID("_Azure_MoonEmission");

		internal static readonly int _StarfieldColorBalance = Shader.PropertyToID("_Azure_StarfieldColorBalance");

		internal static readonly int _StarfieldIntensity = Shader.PropertyToID("_Azure_StarfieldIntensity");

		internal static readonly int _MilkyWayIntensity = Shader.PropertyToID("_Azure_MilkyWayIntensity");

		internal static readonly int _FogScale = Shader.PropertyToID("_Azure_FogScale");

		internal static readonly int _FogBlend = Shader.PropertyToID("_Azure_FogBlend");

		internal static readonly int _FogDensity = Shader.PropertyToID("_Azure_FogDensity");

		internal static readonly int _FogDistance = Shader.PropertyToID("_Azure_FogDistance");

		internal static readonly int _HeightFogBlend = Shader.PropertyToID("_Azure_HeightFogBlend");

		internal static readonly int _HeightFogDensity = Shader.PropertyToID("_Azure_HeightFogDensity");

		internal static readonly int _HeightFogDistance = Shader.PropertyToID("_Azure_HeightFogDistance");

		internal static readonly int _HeightFogStart = Shader.PropertyToID("_Azure_HeightFogStart");

		internal static readonly int _HeightFogEnd = Shader.PropertyToID("_Azure_HeightFogEnd");

		internal static readonly int _DynamicCloudLayer1Altitude = Shader.PropertyToID("_Azure_DynamicCloudLayer1Altitude");

		internal static readonly int _DynamicCloudLayer1Direction = Shader.PropertyToID("_Azure_DynamicCloudLayer1Direction");

		internal static readonly int _DynamicCloudLayer1Speed = Shader.PropertyToID("_Azure_DynamicCloudLayer1Speed");

		internal static readonly int _DynamicCloudLayer1Density = Shader.PropertyToID("_Azure_DynamicCloudLayer1Density");

		internal static readonly int _DynamicCloudLayer1Color1 = Shader.PropertyToID("_Azure_DynamicCloudLayer1Color1");

		internal static readonly int _DynamicCloudLayer1Color2 = Shader.PropertyToID("_Azure_DynamicCloudLayer1Color2");

		internal static readonly int _StaticCloudTextureSource = Shader.PropertyToID("_Azure_StaticCloudTextureSource");

		internal static readonly int _StaticCloudTextureDestination = Shader.PropertyToID("_Azure_StaticCloudTextureDestination");

		internal static readonly int _StaticCloudColor = Shader.PropertyToID("_Azure_StaticCloudColor");

		internal static readonly int _StaticCloudScattering = Shader.PropertyToID("_Azure_StaticCloudScattering");

		internal static readonly int _StaticCloudExtinction = Shader.PropertyToID("_Azure_StaticCloudExtinction");

		internal static readonly int _StaticCloudPower = Shader.PropertyToID("_Azure_StaticCloudPower");

		internal static readonly int _StaticCloudIntensity = Shader.PropertyToID("_Azure_StaticCloudIntensity");

		internal static readonly int _StaticCloudRotationSpeed = Shader.PropertyToID("_Azure_StaticCloudRotationSpeed");

		internal static readonly int _WeatherTransitionTime = Shader.PropertyToID("_Azure_WeatherTransitionTime");

		internal static readonly int _RainColor = Shader.PropertyToID("_TintColor");

		internal static readonly int _SnowColor = Shader.PropertyToID("_TintColor");
	}

	public Transform sunTransform;

	public Transform moonTransform;

	public Transform lightTransform;

	public Light thunderDirectionalLight;

	public Texture2D moonTexture;

	public Texture2D sunTexture;

	public Texture2D cloudNoise;

	public Cubemap starfieldTexture;

	public Cubemap starNoiseTexture;

	public Material skyMaterial;

	public Material rainMaterial;

	public Material snowMaterial;

	public ParticleSystem rainParticle;

	public ParticleSystem snowParticle;

	public WindZone windZone;

	public AzureSkySoundFX soundFX;

	public Transform skydome;

	public ReflectionProbe reflectionProbe;

	private Camera mainCamera;

	private DateTime m_date;

	private int m_dayOfYear = 1;

	private AzureSkyProfile m_getCalendarProfile;

	private Matrix4x4 m_starfieldMatrix;

	private Matrix4x4 m_noiseMatrix;

	private float m_noiseMatrixRotation;

	private Quaternion m_noiseRotation;

	private float m_timeSinceLastProbeUpdate;

	private float m_timelineCurveTime;

	private float m_timelineGradientTime;

	private float m_sunCurveTime;

	private float m_sunGradientTime;

	private float m_moonCurveTime;

	private float m_moonGradientTime;

	private float m_timeProgression;

	private Vector3 m_sunRealisticPosition = Vector3.one;

	private Vector3 m_moonRealisticPosition = Vector3.one;

	private Quaternion m_sunSimplePosition;

	private Vector3 m_sunLocalDirection;

	private Vector3 m_moonLocalDirection;

	public bool isBlendingSpecialWeather;

	public int weatherNumber;

	public bool isBlendingWeathers;

	public float weatherTransitionTime;

	private float m_startTime;

	private float m_transitionTime = 30f;

	public List<AzureSkyThunder> thunderList = new List<AzureSkyThunder>();

	public float thunderLightIntensity = 0.25f;

	public float thunderCloudIntensity = 0.5f;

	private float m_thunderLightingTime;

	private float m_thunderLightingSpeed;

	private float m_thunderLightingIntensity;

	private int m_thunderIndex;

	private bool m_thunderIsLighting;

	public AzureSkyProfile[] calendarProfileList = new AzureSkyProfile[366];

	public List<AzureSkyProfile> standardProfileList = new List<AzureSkyProfile>();

	public List<WeatherProfile> weatherProfileList = new List<WeatherProfile>();

	public AzureSkyProfile calendarDayProfile;

	private AzureSkyProfile nextCalendarDayProfile;

	public AzureSkyProfile currentDayProfile;

	public AzureSkyProfile sourceWeatherProfile;

	private AzureSkyProfile m_targetWeatherProfile;

	private AzureSkyProfile m_targetSpecialWeatherProfile;

	private WeightedDayProfile[] m_weightedProfiles;

	public AzureSkyTimeOfDayComponent timeOfDay = new AzureSkyTimeOfDayComponent();

	public AzureSkyOptionsComponent options = new AzureSkyOptionsComponent();

	public float kr = 8.4f;

	public float km = 1.25f;

	public float rayleigh = 1f;

	public float mie = 1f;

	public float nightIntensity = 2.5f;

	public float scattering = 0.25f;

	public float sunDiskIntensity = 3f;

	public float exposure = 1f;

	public Color mieColor = Color.white;

	public Color rayleighColor = Color.white;

	public Color sunDiskColor = Color.white;

	private Vector3 m_lambda = new Vector3(680f, 550f, 440f);

	public float N = 2.545f;

	private const float m_pi = MathF.PI;

	private const float m_n = 1.00029f;

	private const float m_pn = 0.035f;

	private Vector3 m_K = new Vector3(686f, 678f, 666f);

	public Color moonColor = Color.white;

	public Color moonBrightColor = Color.white;

	public float moonBrightRange = 50f;

	public float moonEmission = 0.5f;

	public float starsScintillation = 5f;

	public float starfieldIntensity;

	public float milkyWayIntensity;

	public float fogScale = 1f;

	public float fogBlend = 0.25f;

	public float fogDensity = 1f;

	public float fogDistance = 5000f;

	public float heightFogBlend = 1f;

	public float heightFogDensity = 0.5f;

	public float heightFogDistance = 500f;

	public float heightFogStart;

	public float heightFogEnd = 100f;

	public float dynamicCloudLayer1Altitude = 10f;

	public float dynamicCloudLayer1Direction = 1f;

	public float dynamicCloudLayer1Speed = 0.1f;

	public Color dynamicCloudLayer1Color1 = Color.white;

	public Color dynamicCloudLayer1Color2 = Color.white;

	public float dynamicCloudLayer1Density = 0.7f;

	private Texture2D staticCloudTextureSource;

	private Texture2D staticCloudTextureDestination;

	public Color staticCloudColor = Color.white;

	public float staticCloudScattering = 1f;

	public float staticCloudExtinction = 0.25f;

	public float staticCloudPower = 2.2f;

	public float staticCloudIntensity = 1f;

	public float staticCloudRotationSpeed;

	private float m_cloudRotationSpeed;

	public float lightIntensity = 1f;

	public Color lightColor = Color.black;

	public float ambientIntensity = 1f;

	public Color ambientSkyColor = Color.black;

	public Color ambientEquatorColor = Color.black;

	public Color ambientGroundColor = Color.black;

	public float reflectionIntensity = 1f;

	[HideInInspector]
	public Light m_lightComponent;

	private float m_sunElevation;

	public Color rainColor = Color.white;

	public Color snowColor = Color.white;

	public float rainIntensity;

	public float snowIntensity;

	public float windSpeed;

	public float windDirection;

	public float rainLightVolume;

	public float rainMediumVolume;

	public float rainHeavyVolume;

	public float windLightVolume;

	public float windMediumVolume;

	public float windHeavyVolume;

	public float wetness;

	public float snowAmount;

	public float covarage;

	public Color outputColor1 = Color.white;

	public Color outputColor2 = Color.white;

	public Color outputColor3 = Color.white;

	private ParticleSystem.EmissionModule m_rainEmission;

	private ParticleSystem.EmissionModule m_snowEmission;

	private float m_maxRainEmission = 2000f;

	private float m_maxSnowEmission = 750f;

	private Vector3 m_windRotation = Vector3.zero;

	private float m_maxWindSpeed = 10f;

	public List<CurveOutput> curveOuputList = new List<CurveOutput>();

	public List<GradientOutput> gradientOuputList = new List<GradientOutput>();

	private bool m_isDaytime;

	public float TimelineGradientTime => m_timelineGradientTime;

	public bool IsDaytime => m_isDaytime;

	private void Start()
	{
		weatherNumber = 0;
		if (rainParticle != null)
		{
			m_rainEmission = rainParticle.emission;
		}
		if (snowParticle != null)
		{
			m_snowEmission = snowParticle.emission;
		}
		m_lightComponent = lightTransform.GetComponent<Light>();
		ConfigureShaders();
		InitializeShaderUniforms();
		UpdateShaderUniforms();
		SetParticlesActive(options.particlesMode);
		UpdateProfiles();
		if (options.startAtCurrentDate)
		{
			timeOfDay.ApplySystemDate();
		}
		if (options.startAtCurrentTime)
		{
			timeOfDay.ApplySystemTime();
		}
		m_timeProgression = timeOfDay.GetDayLength();
		if (options.useReflectionProbe == 1 && options.reflectionProbeRefreshMode == 2 && options.reflectionProbeUpdateAtFirstFrame)
		{
			reflectionProbe.RenderProbe();
		}
	}

	private void Reset()
	{
		timeOfDay.UpdateCalendar(timeOfDay.selectableDayList);
	}

	private void Update()
	{
		UpdateShaderUniforms();
		if (mainCamera == null)
		{
			mainCamera = Helper.MainCamera;
		}
		if (options.followMainCamera && (bool)mainCamera)
		{
			transform.position = mainCamera.transform.position;
		}
		m_timelineGradientTime = timeOfDay.hour / 24f;
		m_timelineCurveTime = timeOfDay.hour;
		if (timeOfDay.setTimeByCurve)
		{
			timeOfDay.CalculateTimeByCurve();
			m_timelineGradientTime = timeOfDay.hourByCurve / 24f;
			m_timelineCurveTime = timeOfDay.hourByCurve;
		}
		m_sunCurveTime = Vector3.Dot(-sunTransform.transform.forward, transform.up);
		m_sunGradientTime = Mathf.InverseLerp(-1f, 1f, m_sunCurveTime);
		m_moonCurveTime = Vector3.Dot(-moonTransform.transform.forward, transform.up);
		m_moonGradientTime = Mathf.InverseLerp(-1f, 1f, m_moonCurveTime);
		if (!isBlendingWeathers && !isBlendingSpecialWeather)
		{
			N = currentDayProfile.scattering.N;
			kr = currentDayProfile.scattering.kr;
			km = currentDayProfile.scattering.km;
			m_lambda = currentDayProfile.scattering.lambda;
			rayleigh = GetCurveValue(currentDayProfile.scattering.rayleighCurve, currentDayProfile.scattering.rayleighCurveIndex);
			mie = GetCurveValue(currentDayProfile.scattering.mieCurve, currentDayProfile.scattering.mieCurveIndex);
			scattering = GetCurveValue(currentDayProfile.scattering.scatteringCurve, currentDayProfile.scattering.scatteringCurveIndex);
			nightIntensity = GetCurveValue(currentDayProfile.scattering.nightIntensityCurve, currentDayProfile.scattering.nightIntensityCurveIndex);
			sunDiskIntensity = GetCurveValue(currentDayProfile.scattering.sunDiskIntensityCurve, currentDayProfile.scattering.sunDiskIntensityCurveIndex);
			exposure = GetCurveValue(currentDayProfile.scattering.exposureCurve, currentDayProfile.scattering.exposureCurveIndex);
			rayleighColor = GetGradientValue(currentDayProfile.scattering.rayleighGradientColor, currentDayProfile.scattering.rayleighGradientIndex);
			mieColor = GetGradientValue(currentDayProfile.scattering.mieGradientColor, currentDayProfile.scattering.mieGradientIndex);
			sunDiskColor = GetGradientValue(currentDayProfile.scattering.sunDiskGradientColor, currentDayProfile.scattering.sunDiskGradientIndex);
			moonColor = GetGradientValue(currentDayProfile.deepSpace.moonColorGradientColor, currentDayProfile.deepSpace.moonColorGradientIndex);
			moonBrightColor = GetGradientValue(currentDayProfile.deepSpace.moonBrightColorGradientColor, currentDayProfile.deepSpace.moonBrightColorGradientIndex);
			moonBrightRange = GetCurveValue(currentDayProfile.deepSpace.moonBrightRangeCurve, currentDayProfile.deepSpace.moonBrightRangeCurveIndex);
			moonEmission = GetCurveValue(currentDayProfile.deepSpace.moonEmissionCurve, currentDayProfile.deepSpace.moonEmissionCurveIndex);
			starfieldIntensity = GetCurveValue(currentDayProfile.deepSpace.starfieldIntensityCurve, currentDayProfile.deepSpace.starfieldIntensityCurveIndex);
			milkyWayIntensity = GetCurveValue(currentDayProfile.deepSpace.milkyWayIntensityCurve, currentDayProfile.deepSpace.milkyWayIntensityCurveIndex);
			starsScintillation = currentDayProfile.deepSpace.starsScintillation;
			fogScale = currentDayProfile.fogScattering.fogScale;
			fogBlend = GetCurveValue(currentDayProfile.fogScattering.fogBlendCurve, currentDayProfile.fogScattering.fogBlendCurveIndex);
			fogDensity = GetCurveValue(currentDayProfile.fogScattering.fogDensityCurve, currentDayProfile.fogScattering.fogDensityCurveIndex);
			fogDistance = GetCurveValue(currentDayProfile.fogScattering.fogDistanceCurve, currentDayProfile.fogScattering.fogDistanceCurveIndex);
			heightFogBlend = GetCurveValue(currentDayProfile.fogScattering.heightFogBlendCurve, currentDayProfile.fogScattering.heightFogBlendCurveIndex);
			heightFogDensity = GetCurveValue(currentDayProfile.fogScattering.heightFogDensityCurve, currentDayProfile.fogScattering.heightFogDensityCurveIndex);
			heightFogDistance = GetCurveValue(currentDayProfile.fogScattering.heightFogDistanceCurve, currentDayProfile.fogScattering.heightFogDistanceCurveIndex);
			heightFogStart = GetCurveValue(currentDayProfile.fogScattering.heightFogStartCurve, currentDayProfile.fogScattering.heightFogStartCurveIndex);
			heightFogEnd = GetCurveValue(currentDayProfile.fogScattering.heightFogEndCurve, currentDayProfile.fogScattering.heightFogEndCurveIndex);
			switch (options.cloudMode)
			{
			case 1:
				dynamicCloudLayer1Altitude = currentDayProfile.clouds.dynamicCloudLayer1Altitude;
				dynamicCloudLayer1Direction = currentDayProfile.clouds.dynamicCloudLayer1Direction;
				dynamicCloudLayer1Speed = currentDayProfile.clouds.dynamicCloudLayer1Speed;
				dynamicCloudLayer1Density = GetCurveValue(currentDayProfile.clouds.dynamicCloudLayer1DensityCurve, currentDayProfile.clouds.dynamicCloudLayer1DensityCurveIndex);
				dynamicCloudLayer1Color1 = GetGradientValue(currentDayProfile.clouds.dynamicCloudLayer1GradientColor1, currentDayProfile.clouds.dynamicCloudLayer1GradientColor1Index);
				dynamicCloudLayer1Color2 = GetGradientValue(currentDayProfile.clouds.dynamicCloudLayer1GradientColor2, currentDayProfile.clouds.dynamicCloudLayer1GradientColor2Index);
				break;
			case 2:
				staticCloudColor = GetGradientValue(currentDayProfile.clouds.staticCloudColor, currentDayProfile.clouds.staticCloudColorIndex);
				staticCloudScattering = GetCurveValue(currentDayProfile.clouds.staticCloudScatteringCurve, currentDayProfile.clouds.staticCloudScatteringCurveIndex);
				staticCloudExtinction = GetCurveValue(currentDayProfile.clouds.staticCloudExtinctionCurve, currentDayProfile.clouds.staticCloudExtinctionCurveIndex);
				staticCloudPower = GetCurveValue(currentDayProfile.clouds.staticCloudPowerCurve, currentDayProfile.clouds.staticCloudPowerCurveIndex);
				staticCloudIntensity = GetCurveValue(currentDayProfile.clouds.staticCloudIntensityCurve, currentDayProfile.clouds.staticCloudIntensityCurveIndex);
				staticCloudRotationSpeed = currentDayProfile.clouds.staticCloudRotationSpeed;
				break;
			}
			lightIntensity = GetCurveValue(currentDayProfile.lighting.directionalLightIntensityCurve, currentDayProfile.lighting.directionalLightIntensityCurveIndex);
			lightColor = GetGradientValue(currentDayProfile.lighting.directionalLightGradientColor, currentDayProfile.lighting.directionalLightGradientColorIndex);
			ambientIntensity = GetCurveValue(currentDayProfile.lighting.ambientIntensityCurve, currentDayProfile.lighting.ambientIntensityCurveIndex);
			ambientSkyColor = GetGradientValue(currentDayProfile.lighting.ambientSkyGradientColor, currentDayProfile.lighting.ambientSkyGradientColorIndex);
			ambientEquatorColor = GetGradientValue(currentDayProfile.lighting.equatorGradientColor, currentDayProfile.lighting.equatorGradientColorIndex);
			ambientGroundColor = GetGradientValue(currentDayProfile.lighting.groundGradientColor, currentDayProfile.lighting.groundGradientColorIndex);
			reflectionIntensity = GetCurveValue(currentDayProfile.lighting.reflectionIntensityCurve, currentDayProfile.lighting.reflectionIntensityCurveIndex);
			if (options.particlesMode == 1 || options.keepWeatherUpdate)
			{
				rainColor = GetGradientValue(currentDayProfile.weather.weatherRainGradientColor, currentDayProfile.weather.weatherRainGradientColorIndex);
				snowColor = GetGradientValue(currentDayProfile.weather.weatherSnowGradientColor, currentDayProfile.weather.weatherSnowGradientColorIndex);
				rainIntensity = GetCurveValue(currentDayProfile.weather.weatherRainIntensityCurve, currentDayProfile.weather.weatherRainIntensityCurveIndex);
				snowIntensity = GetCurveValue(currentDayProfile.weather.weatherSnowIntensityCurve, currentDayProfile.weather.weatherSnowIntensityCurveIndex);
				windSpeed = GetCurveValue(currentDayProfile.weather.weatherWindSpeedCurve, currentDayProfile.weather.weatherWindSpeedCurveIndex);
				windDirection = GetCurveValue(currentDayProfile.weather.weatherWindDirectionCurve, currentDayProfile.weather.weatherWindDirectionCurveIndex);
				thunderLightIntensity = GetCurveValue(currentDayProfile.weather.weatherThunderLightIntensityCurve, currentDayProfile.weather.weatherThunderLightIntensityCurveIndex);
				thunderCloudIntensity = GetCurveValue(currentDayProfile.weather.weatherThunderCloudIntensityCurve, currentDayProfile.weather.weatherThunderCloudIntensityCurveIndex);
				rainLightVolume = currentDayProfile.weather.weatherRainLightVolume;
				rainMediumVolume = currentDayProfile.weather.weatherRainMediumVolume;
				rainHeavyVolume = currentDayProfile.weather.weatherRainHeavyVolume;
				windLightVolume = currentDayProfile.weather.weatherWindLightVolume;
				windMediumVolume = currentDayProfile.weather.weatherWindMediumVolume;
				windHeavyVolume = currentDayProfile.weather.weatherWindHeavyVolume;
			}
			if (options.keepWeatherUpdate)
			{
				wetness = GetCurveValue(currentDayProfile.weather.weatherWetnessCurve, currentDayProfile.weather.weatherWetnessCurveIndex);
				snowAmount = GetCurveValue(currentDayProfile.weather.weatherSnowAmountCurve, currentDayProfile.weather.weatherSnowAmountCurveIndex);
				covarage = GetCurveValue(currentDayProfile.weather.weatherCovarageCurve, currentDayProfile.weather.weatherCovarageCurveIndex);
				outputColor1 = GetGradientValue(currentDayProfile.weather.weatherOutputColor1GradientColor, currentDayProfile.weather.weatherOutputColor1Index);
				outputColor2 = GetGradientValue(currentDayProfile.weather.weatherOutputColor2GradientColor, currentDayProfile.weather.weatherOutputColor2Index);
				outputColor3 = GetGradientValue(currentDayProfile.weather.weatherOutputColor3GradientColor, currentDayProfile.weather.weatherOutputColor3Index);
			}
		}
		m_sunLocalDirection = transform.InverseTransformDirection(sunTransform.transform.forward);
		m_moonLocalDirection = transform.InverseTransformDirection(moonTransform.transform.forward);
		if (Application.isPlaying)
		{
			timeOfDay.hour += m_timeProgression * Time.deltaTime;
			if (timeOfDay.hour >= 24f)
			{
				timeOfDay.StartNextDay(options.repeatMode);
				if (options.applyDayChangeTransition && weatherNumber == 0 && calendarDayProfile != nextCalendarDayProfile)
				{
					SetNewWeatherProfile(-1);
				}
				UpdateProfiles();
			}
			if (isBlendingWeathers)
			{
				BlendingWeathers();
			}
			if (isBlendingSpecialWeather)
			{
				BlendingWeathersSpecial();
			}
			if (m_thunderIsLighting)
			{
				m_thunderLightingTime += m_thunderLightingSpeed * Time.deltaTime;
				m_thunderLightingIntensity = thunderList[m_thunderIndex].lightingCurve.Evaluate(Mathf.Clamp01(m_thunderLightingTime));
				thunderDirectionalLight.intensity = m_thunderLightingIntensity * thunderLightIntensity;
				if (m_thunderLightingTime >= 1f)
				{
					m_thunderIsLighting = false;
					thunderDirectionalLight.enabled = false;
					m_thunderLightingTime = 0f;
				}
			}
			if (soundFX != null)
			{
				if (soundFX.rainLight != null)
				{
					SoundPlayController(rainLightVolume, soundFX.rainLight);
				}
				if (soundFX.rainMedium != null)
				{
					SoundPlayController(rainMediumVolume, soundFX.rainMedium);
				}
				if (soundFX.rainHeavy != null)
				{
					SoundPlayController(rainHeavyVolume, soundFX.rainHeavy);
				}
				if (soundFX.windLight != null)
				{
					SoundPlayController(windLightVolume, soundFX.windLight);
				}
				if (soundFX.windMedium != null)
				{
					SoundPlayController(windMediumVolume, soundFX.windMedium);
				}
				if (soundFX.windHeavy != null)
				{
					SoundPlayController(windHeavyVolume, soundFX.windHeavy);
				}
			}
			if (staticCloudRotationSpeed != 0f)
			{
				m_cloudRotationSpeed += staticCloudRotationSpeed * Time.deltaTime;
				if (m_cloudRotationSpeed >= 1f)
				{
					m_cloudRotationSpeed--;
				}
			}
		}
		switch (timeOfDay.timeMode)
		{
		case 0:
			m_sunSimplePosition = Quaternion.Euler(0f, timeOfDay.longitude, timeOfDay.latitude);
			m_sunSimplePosition *= Quaternion.Euler(timeOfDay.SetSimpleSunPosition(), 180f, 0f);
			sunTransform.transform.localRotation = m_sunSimplePosition;
			moonTransform.transform.rotation = timeOfDay.SetSimpleMoonPosition(sunTransform);
			skydome.transform.localRotation = Quaternion.Inverse(transform.rotation);
			Shader.SetGlobalMatrix(Uniforms._UpMatrix, transform.worldToLocalMatrix);
			Shader.SetGlobalMatrix(Uniforms._SunMatrix, sunTransform.transform.worldToLocalMatrix);
			Shader.SetGlobalMatrix(Uniforms._RelativeSunMatrix, sunTransform.transform.worldToLocalMatrix * Matrix4x4.TRS(Vector3.zero, Quaternion.Inverse(transform.rotation), Vector3.one).inverse);
			Shader.SetGlobalMatrix(Uniforms._StarMatrix, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(options.starfieldPosition), Vector3.one).inverse);
			break;
		case 1:
		{
			m_sunRealisticPosition = timeOfDay.SetRealisticSunPosition();
			sunTransform.transform.forward = transform.TransformDirection(m_sunRealisticPosition);
			m_moonRealisticPosition = timeOfDay.SetRealisticMoonPosition();
			moonTransform.transform.forward = transform.TransformDirection(m_moonRealisticPosition);
			skydome.transform.localRotation = Quaternion.Inverse(transform.rotation);
			Shader.SetGlobalMatrix(Uniforms._UpMatrix, transform.worldToLocalMatrix);
			Shader.SetGlobalMatrix(Uniforms._SunMatrix, sunTransform.transform.worldToLocalMatrix);
			Shader.SetGlobalMatrix(Uniforms._RelativeSunMatrix, Matrix4x4.identity);
			Quaternion quaternion = Quaternion.Euler(90f - timeOfDay.latitude, 0f, 0f) * Quaternion.Euler(0f, timeOfDay.longitude, 0f) * Quaternion.Euler(0f, timeOfDay.lst * 57.29578f, 0f);
			m_starfieldMatrix = Matrix4x4.TRS(Vector3.zero, quaternion * Quaternion.Euler(options.starfieldPosition), Vector3.one);
			Shader.SetGlobalMatrix(Uniforms._StarMatrix, m_starfieldMatrix.inverse);
			break;
		}
		}
		m_sunElevation = Vector3.Dot(-sunTransform.transform.forward, transform.up);
		if (m_sunElevation >= 0f)
		{
			lightTransform.transform.localRotation = Quaternion.LookRotation(m_sunLocalDirection);
			m_isDaytime = true;
		}
		else
		{
			lightTransform.transform.localRotation = Quaternion.LookRotation(m_moonLocalDirection);
			m_isDaytime = false;
		}
		m_lightComponent.intensity = lightIntensity;
		m_lightComponent.color = lightColor;
		RenderSettings.ambientIntensity = ambientIntensity;
		if (options.useReflectionProbe == 1)
		{
			reflectionProbe.intensity = reflectionIntensity;
			m_timeSinceLastProbeUpdate += Time.deltaTime;
			if (options.reflectionProbeRefreshMode == 2 && m_timeSinceLastProbeUpdate >= options.reflectionProbeUpdateTime)
			{
				reflectionProbe.RenderProbe();
				m_timeSinceLastProbeUpdate = 0f;
			}
		}
		if (options.particlesMode == 1)
		{
			if (rainIntensity > 0f)
			{
				if (rainParticle.isStopped)
				{
					rainParticle.Play();
					m_rainEmission = rainParticle.emission;
				}
				m_rainEmission.rateOverTime = rainIntensity * m_maxRainEmission;
			}
			else if (rainParticle.isPlaying)
			{
				rainParticle.Stop();
			}
			if (snowIntensity > 0f)
			{
				if (snowParticle.isStopped)
				{
					snowParticle.Play();
					m_snowEmission = snowParticle.emission;
				}
				m_snowEmission.rateOverTime = snowIntensity * m_maxSnowEmission;
			}
			else if (snowParticle.isPlaying)
			{
				snowParticle.Stop();
			}
		}
		m_windRotation.y = windDirection * 360f;
		if (windZone != null)
		{
			windZone.transform.eulerAngles = m_windRotation;
			windZone.windMain = windSpeed * m_maxWindSpeed;
		}
		if (starsScintillation > 0f)
		{
			m_noiseMatrixRotation += starsScintillation * Time.deltaTime;
			m_noiseRotation = Quaternion.Euler(m_noiseMatrixRotation, m_noiseMatrixRotation, m_noiseMatrixRotation);
			m_noiseMatrix = Matrix4x4.TRS(Vector3.zero, m_noiseRotation, new Vector3(1f, 1f, 1f));
		}
	}

	private Vector3 GetBetaRay()
	{
		Vector3 vector = m_lambda * 1E-09f;
		float num = N * 1E+25f;
		Vector3 result = default;
		result.x = 8f * Mathf.Pow(MathF.PI, 3f) * Mathf.Pow(Mathf.Pow(1.00029f, 2f) - 1f, 2f) / (3f * num * Mathf.Pow(vector.x, 4f)) * 1000f;
		result.y = 8f * Mathf.Pow(MathF.PI, 3f) * Mathf.Pow(Mathf.Pow(1.00029f, 2f) - 1f, 2f) / (3f * num * Mathf.Pow(vector.y, 4f)) * 1000f;
		result.z = 8f * Mathf.Pow(MathF.PI, 3f) * Mathf.Pow(Mathf.Pow(1.00029f, 2f) - 1f, 2f) / (3f * num * Mathf.Pow(vector.z, 4f)) * 1000f;
		return result;
	}

	private Vector3 GetBetaMie()
	{
		float num = 4f;
		Vector3 result = default;
		result.x = 434f * num * MathF.PI * Mathf.Pow(MathF.PI * 2f / m_lambda.x, 2f) * m_K.x / 3f;
		result.y = 434f * num * MathF.PI * Mathf.Pow(MathF.PI * 2f / m_lambda.y, 2f) * m_K.y / 3f;
		result.z = 434f * num * MathF.PI * Mathf.Pow(MathF.PI * 2f / m_lambda.z, 2f) * m_K.z / 3f;
		result.x = Mathf.Pow(result.x, -1f);
		result.y = Mathf.Pow(result.y, -1f);
		result.z = Mathf.Pow(result.z, -1f);
		return result;
	}

	private float GetCurveValue(AnimationCurve[] curveTarget, int curveMode)
	{
		return curveMode switch
		{
			0 => curveTarget[0].Evaluate(m_timelineCurveTime), 
			1 => curveTarget[1].Evaluate(m_sunCurveTime), 
			2 => curveTarget[2].Evaluate(m_moonCurveTime), 
			_ => 0f, 
		};
	}

	public float GetCurveOutput(int element)
	{
		return curveOuputList[element].curveIndex switch
		{
			0 => curveOuputList[element].curveOutput[0].Evaluate(m_timelineCurveTime), 
			1 => curveOuputList[element].curveOutput[1].Evaluate(m_sunCurveTime), 
			2 => curveOuputList[element].curveOutput[2].Evaluate(m_moonCurveTime), 
			_ => 0f, 
		};
	}

	public Color GetGradientValue(Gradient[] gradientTarget, int curveMode)
	{
		return curveMode switch
		{
			0 => gradientTarget[0].Evaluate(m_timelineGradientTime), 
			1 => gradientTarget[1].Evaluate(m_sunGradientTime), 
			2 => gradientTarget[2].Evaluate(m_moonGradientTime), 
			_ => Color.white, 
		};
	}

	public Color GetGradientOutput(int element)
	{
		return gradientOuputList[element].gradientIndex switch
		{
			0 => gradientOuputList[element].gradientOutput[0].Evaluate(m_timelineGradientTime), 
			1 => gradientOuputList[element].gradientOutput[1].Evaluate(m_sunGradientTime), 
			2 => gradientOuputList[element].gradientOutput[2].Evaluate(m_moonGradientTime), 
			_ => Color.white, 
		};
	}

	public AzureSkyProfile GetCalendarDayProfile()
	{
		m_dayOfYear = timeOfDay.GetDayOfYear();
		if ((bool)calendarProfileList[m_dayOfYear])
		{
			m_getCalendarProfile = calendarProfileList[m_dayOfYear];
		}
		else
		{
			m_getCalendarProfile = standardProfileList[Random.Range(0, standardProfileList.Count)];
		}
		return m_getCalendarProfile;
	}

	public AzureSkyProfile GetNextCalendarDayProfile()
	{
		m_dayOfYear = timeOfDay.GetDayOfYear();
		if (m_dayOfYear < 365)
		{
			if ((bool)calendarProfileList[m_dayOfYear + 1])
			{
				m_getCalendarProfile = calendarProfileList[m_dayOfYear + 1];
			}
			else
			{
				m_getCalendarProfile = standardProfileList[Random.Range(0, standardProfileList.Count)];
			}
		}
		else if ((bool)calendarProfileList[0])
		{
			m_getCalendarProfile = calendarProfileList[0];
		}
		else
		{
			m_getCalendarProfile = standardProfileList[Random.Range(0, standardProfileList.Count)];
		}
		return m_getCalendarProfile;
	}

	public void SetNewWeatherProfile(int target)
	{
		if (!isBlendingWeathers)
		{
			switch (target)
			{
			case -1:
				m_transitionTime = options.dayChangeTransitionTime;
				m_targetWeatherProfile = nextCalendarDayProfile;
				break;
			case 0:
				m_transitionTime = weatherProfileList[target].TransitionTime;
				m_targetWeatherProfile = calendarDayProfile;
				break;
			default:
				m_transitionTime = weatherProfileList[target].TransitionTime;
				m_targetWeatherProfile = weatherProfileList[target].Profile;
				break;
			}
			weatherTransitionTime = 0f;
			m_startTime = Time.time;
			isBlendingWeathers = true;
			weatherNumber = target;
			if (weatherNumber < 0)
			{
				weatherNumber = 0;
			}
			SetStaticCloudTexture(sourceWeatherProfile.clouds.staticCloudTexture, m_targetWeatherProfile.clouds.staticCloudTexture);
		}
		else
		{
			isBlendingWeathers = false;
			weatherTransitionTime = 0f;
			m_startTime = 0f;
			sourceWeatherProfile = m_targetWeatherProfile;
			currentDayProfile = m_targetWeatherProfile;
			SetStaticCloudTexture(currentDayProfile.clouds.staticCloudTexture, currentDayProfile.clouds.staticCloudTexture);
		}
	}

	public void SetNewWeatherProfile(AzureSkyProfile profile, float transitionTime)
	{
		for (int i = 0; i < weatherProfileList.Count; i++)
		{
			WeatherProfile weatherProfile = weatherProfileList[i];
			if (weatherProfile.Profile == profile)
			{
				weatherProfile.TransitionTime = transitionTime;
				SetNewWeatherProfile(i);
				break;
			}
		}
	}

	public void SetNewWeatherProfileSpecial(int target)
	{
		if (target > 0)
		{
			m_targetSpecialWeatherProfile = weatherProfileList[target].Profile;
			isBlendingSpecialWeather = true;
			if (sourceWeatherProfile != null && m_targetSpecialWeatherProfile != null)
			{
				SetStaticCloudTexture(sourceWeatherProfile.clouds.staticCloudTexture, m_targetSpecialWeatherProfile.clouds.staticCloudTexture);
			}
		}
	}

	public void StopWeatherProfileSpecial()
	{
		if (!isBlendingSpecialWeather)
		{
			Debug.Log("Can't stop blending special weather because we were not blending any special weather");
		}
		else
		{
			isBlendingSpecialWeather = false;
		}
	}

	private void BlendingWeathers()
	{
		if (weatherNumber == 0 && !isBlendingWeathers)
		{
			sourceWeatherProfile = calendarDayProfile;
		}
		weatherTransitionTime = Mathf.Clamp01((Time.time - m_startTime) / m_transitionTime);
		if (!isBlendingSpecialWeather)
		{
			SetWeightedProfiles(new WeightedDayProfile(sourceWeatherProfile, 1f - weatherTransitionTime), new WeightedDayProfile(m_targetWeatherProfile, weatherTransitionTime));
			BlendWeatherProfiles(m_weightedProfiles);
			Shader.SetGlobalFloat(Uniforms._WeatherTransitionTime, weatherTransitionTime);
		}
		if (weatherTransitionTime == 1f)
		{
			isBlendingWeathers = false;
			weatherTransitionTime = 0f;
			m_startTime = 0f;
			sourceWeatherProfile = m_targetWeatherProfile;
			currentDayProfile = m_targetWeatherProfile;
			SetStaticCloudTexture(currentDayProfile.clouds.staticCloudTexture, currentDayProfile.clouds.staticCloudTexture);
		}
	}

	private void BlendingWeathersSpecial()
	{
		Debug.Log("Blend weather special Update");
		float num = Mathf.Clamp01(SkyManager.normalizedDistanceToZone);
		List<WeightedDayProfile> list = new List<WeightedDayProfile>();
		if (isBlendingWeathers)
		{
			WeightedDayProfile item = new WeightedDayProfile(sourceWeatherProfile, 1f - weatherTransitionTime);
			WeightedDayProfile item2 = new WeightedDayProfile(m_targetWeatherProfile, weatherTransitionTime);
			float num2 = 1f - num;
			item.weight = num2 * item.weight;
			item2.weight = num2 * item2.weight;
			list.Add(item);
			list.Add(item2);
		}
		else
		{
			list.Add(new WeightedDayProfile(currentDayProfile, 1f - num));
		}
		list.Add(new WeightedDayProfile(m_targetSpecialWeatherProfile, num));
		SetWeightedProfiles(list.ToArray());
		BlendWeatherProfiles(m_weightedProfiles);
	}

	private void SetWeightedProfiles(params WeightedDayProfile[] profiles)
	{
		m_weightedProfiles = profiles;
	}

	private void BlendWeatherProfiles(WeightedDayProfile[] profiles)
	{
		N = 0f;
		kr = 0f;
		km = 0f;
		m_lambda = Vector3.zero;
		rayleigh = 0f;
		mie = 0f;
		scattering = 0f;
		nightIntensity = 0f;
		sunDiskIntensity = 0f;
		exposure = 0f;
		rayleighColor = Color.black;
		mieColor = Color.black;
		sunDiskColor = Color.black;
		moonColor = Color.black;
		moonBrightColor = Color.black;
		moonBrightRange = 0f;
		moonEmission = 0f;
		starsScintillation = 0f;
		starfieldIntensity = 0f;
		milkyWayIntensity = 0f;
		fogScale = 0f;
		fogBlend = 0f;
		fogDensity = 0f;
		fogDistance = 0f;
		heightFogBlend = 0f;
		heightFogDensity = 0f;
		heightFogDistance = 0f;
		heightFogStart = 0f;
		heightFogEnd = 0f;
		dynamicCloudLayer1Altitude = 0f;
		dynamicCloudLayer1Direction = 0f;
		dynamicCloudLayer1Speed = 0f;
		dynamicCloudLayer1Color1 = Color.black;
		dynamicCloudLayer1Color2 = Color.black;
		dynamicCloudLayer1Density = 0f;
		staticCloudColor = Color.black;
		staticCloudScattering = 0f;
		staticCloudExtinction = 0f;
		staticCloudPower = 0f;
		staticCloudIntensity = 0f;
		staticCloudRotationSpeed = 0f;
		lightIntensity = 0f;
		lightColor = Color.black;
		ambientIntensity = 0f;
		ambientSkyColor = Color.black;
		ambientEquatorColor = Color.black;
		ambientGroundColor = Color.black;
		reflectionIntensity = 0f;
		rainColor = Color.black;
		snowColor = Color.black;
		rainIntensity = 0f;
		snowIntensity = 0f;
		windSpeed = 0f;
		windDirection = 0f;
		thunderLightIntensity = 0f;
		thunderCloudIntensity = 0f;
		rainLightVolume = 0f;
		rainMediumVolume = 0f;
		rainHeavyVolume = 0f;
		windLightVolume = 0f;
		windMediumVolume = 0f;
		windHeavyVolume = 0f;
		wetness = 0f;
		snowAmount = 0f;
		covarage = 0f;
		outputColor1 = Color.black;
		outputColor2 = Color.black;
		outputColor3 = Color.black;
		for (int i = 0; i < profiles.Length; i++)
		{
			AzureSkyProfile profile = profiles[i].profile;
			float weight = profiles[i].weight;
			N += profile.scattering.N * weight;
			kr += profile.scattering.kr * weight;
			km += profile.scattering.km * weight;
			m_lambda += profile.scattering.lambda * weight;
			rayleigh += GetCurveValue(profile.scattering.rayleighCurve, profile.scattering.rayleighCurveIndex) * weight;
			mie += GetCurveValue(profile.scattering.mieCurve, profile.scattering.mieCurveIndex) * weight;
			scattering += GetCurveValue(profile.scattering.scatteringCurve, profile.scattering.scatteringCurveIndex) * weight;
			nightIntensity += GetCurveValue(profile.scattering.nightIntensityCurve, profile.scattering.nightIntensityCurveIndex) * weight;
			sunDiskIntensity += GetCurveValue(profile.scattering.sunDiskIntensityCurve, profile.scattering.sunDiskIntensityCurveIndex) * weight;
			exposure += GetCurveValue(profile.scattering.exposureCurve, profile.scattering.exposureCurveIndex) * weight;
			rayleighColor += GetGradientValue(profile.scattering.rayleighGradientColor, profile.scattering.rayleighGradientIndex) * weight;
			mieColor += GetGradientValue(profile.scattering.mieGradientColor, profile.scattering.mieGradientIndex) * weight;
			sunDiskColor += GetGradientValue(profile.scattering.sunDiskGradientColor, profile.scattering.sunDiskGradientIndex) * weight;
			moonColor += GetGradientValue(profile.deepSpace.moonColorGradientColor, profile.deepSpace.moonColorGradientIndex) * weight;
			moonBrightColor += GetGradientValue(profile.deepSpace.moonBrightColorGradientColor, profile.deepSpace.moonBrightColorGradientIndex) * weight;
			moonBrightRange += GetCurveValue(profile.deepSpace.moonBrightRangeCurve, profile.deepSpace.moonBrightRangeCurveIndex) * weight;
			moonEmission += GetCurveValue(profile.deepSpace.moonEmissionCurve, profile.deepSpace.moonEmissionCurveIndex) * weight;
			starsScintillation += profile.deepSpace.starsScintillation * weight;
			starfieldIntensity += GetCurveValue(profile.deepSpace.starfieldIntensityCurve, profile.deepSpace.starfieldIntensityCurveIndex) * weight;
			milkyWayIntensity += GetCurveValue(profile.deepSpace.milkyWayIntensityCurve, profile.deepSpace.milkyWayIntensityCurveIndex) * weight;
			fogScale += profile.fogScattering.fogScale * weight;
			fogBlend += GetCurveValue(profile.fogScattering.fogBlendCurve, profile.fogScattering.fogBlendCurveIndex) * weight;
			fogDensity += GetCurveValue(profile.fogScattering.fogDensityCurve, profile.fogScattering.fogDensityCurveIndex) * weight;
			fogDistance += GetCurveValue(profile.fogScattering.fogDistanceCurve, profile.fogScattering.fogDistanceCurveIndex) * weight;
			heightFogBlend += GetCurveValue(profile.fogScattering.heightFogBlendCurve, profile.fogScattering.heightFogBlendCurveIndex) * weight;
			heightFogDensity += GetCurveValue(profile.fogScattering.heightFogDensityCurve, profile.fogScattering.heightFogDensityCurveIndex) * weight;
			heightFogDistance += GetCurveValue(profile.fogScattering.heightFogDistanceCurve, profile.fogScattering.heightFogDistanceCurveIndex) * weight;
			heightFogStart += GetCurveValue(profile.fogScattering.heightFogStartCurve, profile.fogScattering.heightFogStartCurveIndex) * weight;
			heightFogEnd += GetCurveValue(profile.fogScattering.heightFogEndCurve, profile.fogScattering.heightFogEndCurveIndex) * weight;
			switch (options.cloudMode)
			{
			case 1:
				dynamicCloudLayer1Altitude += profile.clouds.dynamicCloudLayer1Altitude * weight;
				dynamicCloudLayer1Direction += profile.clouds.dynamicCloudLayer1Direction * weight;
				dynamicCloudLayer1Speed += profile.clouds.dynamicCloudLayer1Speed * weight;
				dynamicCloudLayer1Density += GetCurveValue(profile.clouds.dynamicCloudLayer1DensityCurve, profile.clouds.dynamicCloudLayer1DensityCurveIndex) * weight;
				dynamicCloudLayer1Color1 += GetGradientValue(profile.clouds.dynamicCloudLayer1GradientColor1, profile.clouds.dynamicCloudLayer1GradientColor1Index) * weight;
				dynamicCloudLayer1Color2 += GetGradientValue(profile.clouds.dynamicCloudLayer1GradientColor2, profile.clouds.dynamicCloudLayer1GradientColor2Index) * weight;
				break;
			case 2:
				staticCloudColor += GetGradientValue(profile.clouds.staticCloudColor, profile.clouds.staticCloudColorIndex) * weight;
				staticCloudScattering += GetCurveValue(profile.clouds.staticCloudScatteringCurve, profile.clouds.staticCloudScatteringCurveIndex) * weight;
				staticCloudExtinction += GetCurveValue(profile.clouds.staticCloudExtinctionCurve, profile.clouds.staticCloudExtinctionCurveIndex) * weight;
				staticCloudPower += GetCurveValue(profile.clouds.staticCloudPowerCurve, profile.clouds.staticCloudPowerCurveIndex) * weight;
				staticCloudIntensity += GetCurveValue(profile.clouds.staticCloudIntensityCurve, profile.clouds.staticCloudIntensityCurveIndex) * weight;
				staticCloudRotationSpeed += profile.clouds.staticCloudRotationSpeed * weight;
				break;
			}
			lightIntensity += GetCurveValue(profile.lighting.directionalLightIntensityCurve, profile.lighting.directionalLightIntensityCurveIndex) * weight;
			lightColor += GetGradientValue(profile.lighting.directionalLightGradientColor, profile.lighting.directionalLightGradientColorIndex) * weight;
			ambientIntensity += GetCurveValue(profile.lighting.ambientIntensityCurve, profile.lighting.ambientIntensityCurveIndex) * weight;
			ambientSkyColor += GetGradientValue(profile.lighting.ambientSkyGradientColor, profile.lighting.ambientSkyGradientColorIndex) * weight;
			ambientEquatorColor += GetGradientValue(profile.lighting.equatorGradientColor, profile.lighting.equatorGradientColorIndex) * weight;
			ambientGroundColor += GetGradientValue(profile.lighting.groundGradientColor, profile.lighting.groundGradientColorIndex) * weight;
			reflectionIntensity += GetCurveValue(profile.lighting.reflectionIntensityCurve, profile.lighting.reflectionIntensityCurveIndex) * weight;
			if (options.particlesMode == 1 || options.keepWeatherUpdate)
			{
				rainColor += GetGradientValue(profile.weather.weatherRainGradientColor, profile.weather.weatherRainGradientColorIndex) * weight;
				snowColor += GetGradientValue(profile.weather.weatherSnowGradientColor, profile.weather.weatherSnowGradientColorIndex) * weight;
				rainIntensity += GetCurveValue(profile.weather.weatherRainIntensityCurve, profile.weather.weatherRainIntensityCurveIndex) * weight;
				snowIntensity += GetCurveValue(profile.weather.weatherSnowIntensityCurve, profile.weather.weatherSnowIntensityCurveIndex) * weight;
				windSpeed += GetCurveValue(profile.weather.weatherWindSpeedCurve, profile.weather.weatherWindSpeedCurveIndex) * weight;
				windDirection += GetCurveValue(profile.weather.weatherWindDirectionCurve, profile.weather.weatherWindDirectionCurveIndex) * weight;
				thunderLightIntensity += GetCurveValue(profile.weather.weatherThunderLightIntensityCurve, profile.weather.weatherThunderLightIntensityCurveIndex) * weight;
				thunderCloudIntensity += GetCurveValue(profile.weather.weatherThunderCloudIntensityCurve, profile.weather.weatherThunderCloudIntensityCurveIndex) * weight;
				rainLightVolume += profile.weather.weatherRainLightVolume * weight;
				rainMediumVolume += profile.weather.weatherRainMediumVolume * weight;
				rainHeavyVolume += profile.weather.weatherRainHeavyVolume * weight;
				windLightVolume += profile.weather.weatherWindLightVolume * weight;
				windMediumVolume += profile.weather.weatherWindMediumVolume * weight;
				windHeavyVolume += profile.weather.weatherWindHeavyVolume * weight;
			}
			if (options.keepWeatherUpdate)
			{
				wetness += GetCurveValue(profile.weather.weatherWetnessCurve, profile.weather.weatherWetnessCurveIndex) * weight;
				snowAmount += GetCurveValue(profile.weather.weatherSnowAmountCurve, profile.weather.weatherSnowAmountCurveIndex) * weight;
				covarage += GetCurveValue(profile.weather.weatherCovarageCurve, profile.weather.weatherCovarageCurveIndex) * weight;
				outputColor1 += GetGradientValue(profile.weather.weatherOutputColor1GradientColor, profile.weather.weatherOutputColor1Index) * weight;
				outputColor2 += GetGradientValue(profile.weather.weatherOutputColor2GradientColor, profile.weather.weatherOutputColor2Index) * weight;
				outputColor3 += GetGradientValue(profile.weather.weatherOutputColor3GradientColor, profile.weather.weatherOutputColor3Index) * weight;
			}
		}
	}

	public void UpdateProfiles()
	{
		timeOfDay.UpdateCalendar(timeOfDay.selectableDayList);
		calendarDayProfile = GetCalendarDayProfile();
		nextCalendarDayProfile = GetNextCalendarDayProfile();
		if (weatherNumber == 0)
		{
			currentDayProfile = calendarDayProfile;
			if (!isBlendingWeathers && !isBlendingSpecialWeather)
			{
				sourceWeatherProfile = calendarDayProfile;
			}
		}
	}

	private void SoundPlayController(float volume, AudioSource sound)
	{
		sound.volume = volume;
		if (volume > 0f)
		{
			if (!sound.isPlaying)
			{
				sound.Play();
			}
		}
		else if (sound.isPlaying)
		{
			sound.Stop();
		}
	}

	private IEnumerator PlayThunderAudioClip(int element, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		if (soundFX.thunder != null)
		{
			soundFX.thunder.clip = thunderList[element].audioClip;
			soundFX.thunder.Play();
		}
	}

	public void PlayThunderEffect(int element)
	{
		if (soundFX.thunder != null && !soundFX.thunder.isPlaying)
		{
			if (thunderList[element].audioDelay == 0f)
			{
				soundFX.thunder.clip = thunderList[element].audioClip;
				soundFX.thunder.Play();
			}
			else
			{
				StartCoroutine(PlayThunderAudioClip(element, thunderList[element].audioDelay));
			}
			if (thunderList[element].lightingSpeed > 0f && !m_thunderIsLighting)
			{
				m_thunderIsLighting = true;
				thunderDirectionalLight.enabled = true;
				m_thunderIndex = element;
				m_thunderLightingSpeed = thunderList[element].lightingSpeed;
			}
		}
		else
		{
			Debug.Log("The Thunder Audio Source is currently in use, wait until the current clip finishes and try again.");
		}
	}

	public void PlayThunderEffectRandom()
	{
		if (!soundFX.thunder.isPlaying)
		{
			int num = Random.Range(0, thunderList.Count);
			if (thunderList[num].audioDelay == 0f)
			{
				soundFX.thunder.clip = thunderList[num].audioClip;
				soundFX.thunder.Play();
			}
			else
			{
				StartCoroutine(PlayThunderAudioClip(num, thunderList[num].audioDelay));
			}
			if (thunderList[num].lightingSpeed > 0f && !m_thunderIsLighting)
			{
				m_thunderIsLighting = true;
				thunderDirectionalLight.enabled = true;
				m_thunderIndex = num;
				m_thunderLightingSpeed = thunderList[num].lightingSpeed;
			}
		}
		else
		{
			Debug.Log("The Thunder Audio Source is currently in use, wait until the current clip finishes and try again.");
		}
	}

	private void InitializeShaderUniforms()
	{
		Shader.SetGlobalTexture(Uniforms._SunTexture, sunTexture);
		Shader.SetGlobalTexture(Uniforms._MoonTexture, moonTexture);
		Shader.SetGlobalTexture(Uniforms._CloudNoise, cloudNoise);
		if (!isBlendingWeathers)
		{
			SetStaticCloudTexture(currentDayProfile.clouds.staticCloudTexture, currentDayProfile.clouds.staticCloudTexture);
		}
		Shader.SetGlobalTexture(Uniforms._StarfieldTexture, starfieldTexture);
		Shader.SetGlobalTexture(Uniforms._StarNoiseTexture, starNoiseTexture);
		Shader.SetGlobalVector(Uniforms._MieG, new Vector3(0.4375f, 1.5625f, 1.5f));
		Shader.SetGlobalFloat(Uniforms._Pi316, 0.0596831f);
		Shader.SetGlobalFloat(Uniforms._Pi14, 1f / (4f * MathF.PI));
		Shader.SetGlobalFloat(Uniforms._Pi, MathF.PI);
		Shader.SetGlobalFloat(Uniforms._LightSpeed, options.lightSpeed);
		Shader.SetGlobalFloat(Uniforms._MieDepth, options.mieDepth);
		Shader.SetGlobalFloat(Uniforms._SunSize, options.sunSize);
		Shader.SetGlobalFloat(Uniforms._MoonSize, options.moonSize);
		Shader.SetGlobalInt(Uniforms._SunsetColorMode, options.sunsetColorMode);
	}

	private void UpdateShaderUniforms()
	{
		Shader.SetGlobalFloat(Uniforms._Kr, kr);
		Shader.SetGlobalFloat(Uniforms._Km, km);
		Shader.SetGlobalFloat(Uniforms._Scattering, scattering * 60f);
		Shader.SetGlobalFloat(Uniforms._NightIntensity, nightIntensity);
		Shader.SetGlobalColor(Uniforms._RayleighColor, rayleighColor);
		Shader.SetGlobalColor(Uniforms._MieColor, mieColor);
		Shader.SetGlobalColor(Uniforms._SunDiskColor, sunDiskColor);
		Shader.SetGlobalFloat(Uniforms._SunDiskIntensity, sunDiskIntensity);
		Shader.SetGlobalFloat(Uniforms._Exposure, exposure);
		Shader.SetGlobalVector(Uniforms._SunDirection, transform.InverseTransformDirection(-sunTransform.transform.forward));
		Shader.SetGlobalVector(Uniforms._MoonDirection, transform.InverseTransformDirection(-moonTransform.transform.forward));
		switch (options.wavelengthMode)
		{
		case 0:
			Shader.SetGlobalVector(Uniforms._Br, new Vector3(0.00519673f, 0.0121427f, 0.0296453f) * rayleigh);
			Shader.SetGlobalVector(Uniforms._Bm, new Vector3(0.005721017f, 0.004451339f, 0.003146905f) * mie);
			break;
		case 1:
			Shader.SetGlobalVector(Uniforms._Br, GetBetaRay() * rayleigh);
			Shader.SetGlobalVector(Uniforms._Bm, GetBetaMie() * mie);
			break;
		}
		Shader.SetGlobalColor(Uniforms._MoonColor, moonColor);
		Shader.SetGlobalColor(Uniforms._MoonBrightColor, moonBrightColor);
		Shader.SetGlobalFloat(Uniforms._MoonBrightRange, moonBrightRange);
		Shader.SetGlobalFloat(Uniforms._MoonEmission, moonEmission);
		Shader.SetGlobalMatrix(Uniforms._MoonMatrix, moonTransform.transform.worldToLocalMatrix);
		Shader.SetGlobalVector(Uniforms._StarfieldColorBalance, options.starfieldColor);
		Shader.SetGlobalFloat(Uniforms._StarfieldIntensity, starfieldIntensity);
		Shader.SetGlobalFloat(Uniforms._MilkyWayIntensity, milkyWayIntensity);
		Shader.SetGlobalMatrix(Uniforms._NoiseMatrix, m_noiseMatrix);
		Shader.SetGlobalFloat(Uniforms._FogScale, fogScale);
		Shader.SetGlobalFloat(Uniforms._FogBlend, fogBlend);
		Shader.SetGlobalFloat(Uniforms._FogDensity, fogDensity);
		Shader.SetGlobalFloat(Uniforms._FogDistance, fogDistance);
		Shader.SetGlobalFloat(Uniforms._HeightFogBlend, heightFogBlend);
		Shader.SetGlobalFloat(Uniforms._HeightFogDensity, heightFogDensity);
		Shader.SetGlobalFloat(Uniforms._HeightFogDistance, heightFogDistance);
		Shader.SetGlobalFloat(Uniforms._HeightFogStart, options.planetRadius + heightFogStart);
		Shader.SetGlobalFloat(Uniforms._HeightFogEnd, options.planetRadius + heightFogEnd);
		switch (options.cloudMode)
		{
		case 1:
			Shader.SetGlobalFloat(Uniforms._DynamicCloudLayer1Altitude, dynamicCloudLayer1Altitude);
			Shader.SetGlobalFloat(Uniforms._DynamicCloudLayer1Direction, dynamicCloudLayer1Direction);
			Shader.SetGlobalFloat(Uniforms._DynamicCloudLayer1Speed, dynamicCloudLayer1Speed);
			Shader.SetGlobalFloat(Uniforms._DynamicCloudLayer1Density, Mathf.Lerp(25f, 0f, dynamicCloudLayer1Density));
			Shader.SetGlobalColor(Uniforms._DynamicCloudLayer1Color1, dynamicCloudLayer1Color1);
			Shader.SetGlobalColor(Uniforms._DynamicCloudLayer1Color2, dynamicCloudLayer1Color2 * (1f + m_thunderLightingIntensity * thunderCloudIntensity));
			break;
		case 2:
			Shader.SetGlobalColor(Uniforms._StaticCloudColor, staticCloudColor);
			Shader.SetGlobalFloat(Uniforms._StaticCloudScattering, staticCloudScattering);
			Shader.SetGlobalFloat(Uniforms._StaticCloudExtinction, staticCloudExtinction);
			Shader.SetGlobalFloat(Uniforms._StaticCloudPower, staticCloudPower);
			Shader.SetGlobalFloat(Uniforms._StaticCloudIntensity, staticCloudIntensity);
			Shader.SetGlobalFloat(Uniforms._StaticCloudRotationSpeed, m_cloudRotationSpeed);
			break;
		}
		if (options.particlesMode == 1)
		{
			rainColor.a = 0.5f;
			snowColor.a = 0.5f;
			rainMaterial.SetColor(Uniforms._RainColor, rainColor);
			snowMaterial.SetColor(Uniforms._SnowColor, snowColor);
		}
	}

	private void SetStaticCloudTexture(Texture2D source, Texture2D destination)
	{
		Shader.SetGlobalTexture(Uniforms._StaticCloudTextureSource, source);
		Shader.SetGlobalTexture(Uniforms._StaticCloudTextureDestination, destination);
	}

	public void ConfigureShaders()
	{
		switch (options.shaderMode)
		{
		case 0:
			skydome.gameObject.SetActive(value: false);
			RenderSettings.skybox = skyMaterial;
			Shader.SetGlobalInt(Uniforms._CullMode, 2);
			switch (options.cloudMode)
			{
			case 0:
				skyMaterial.shader = Shader.Find("Azure[Sky]/Pixel Sky");
				break;
			case 1:
				skyMaterial.shader = Shader.Find("Azure[Sky]/Pixel Dynamic Cloud");
				break;
			case 2:
				skyMaterial.shader = Shader.Find("Azure[Sky]/Pixel Static Cloud");
				break;
			}
			break;
		case 1:
			skydome.gameObject.SetActive(value: true);
			RenderSettings.skybox = null;
			Shader.SetGlobalInt(Uniforms._CullMode, 1);
			switch (options.cloudMode)
			{
			case 0:
				skyMaterial.shader = Shader.Find("Azure[Sky]/Vertex Sky");
				break;
			case 1:
				skyMaterial.shader = Shader.Find("Azure[Sky]/Vertex Dynamic Cloud");
				break;
			case 2:
				skyMaterial.shader = Shader.Find("Azure[Sky]/Vertex Static Cloud");
				break;
			}
			break;
		}
	}

	public void SetParticlesActive(bool value)
	{
		if ((bool)rainParticle)
		{
			rainParticle.gameObject.SetActive(value);
		}
		if ((bool)snowParticle)
		{
			snowParticle.gameObject.SetActive(value);
		}
	}

	public void SetParticlesActive(int value)
	{
		bool active = value != 0;
		if ((bool)rainParticle)
		{
			rainParticle.gameObject.SetActive(active);
		}
		if ((bool)snowParticle)
		{
			snowParticle.gameObject.SetActive(active);
		}
	}
}
