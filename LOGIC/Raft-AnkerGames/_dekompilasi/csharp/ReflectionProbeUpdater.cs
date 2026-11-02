using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(ReflectionProbe))]
public class ReflectionProbeUpdater : MonoBehaviour
{
	[SerializeField]
	private float timerInterval = 1f;

	[SerializeField]
	private float lerpCubeTime = 1f;

	[SerializeField]
	private Color solidColor;

	[SerializeField]
	private Cubemap cubemapSimple;

	public GameObject reflectionSphere;

	[SerializeField]
	private Cubemap cubemapCurrent;

	[SerializeField]
	private RenderTexture renderTexture0;

	[SerializeField]
	private RenderTexture renderTexture1;

	[SerializeField]
	private RenderTexture renderTextureTMP;

	[SerializeField]
	private RenderTexture renderTextureCombined;

	private ReflectionProbe probe;

	private Coroutine lerpCoroutine;

	private int renderTextureIndex;

	private int probeRenderID = -1;

	private float timer;

	private void Awake()
	{
		probe = GetComponent<ReflectionProbe>();
		SetResolution(SkyManager.DesiredReflectionResolution);
	}

	private void OnDisable()
	{
		StopLerpCoroutine();
	}

	private void Update()
	{
		if (Cheat.ForceReflectionsOff)
		{
			if (lerpCoroutine != null)
			{
				StopCoroutine(lerpCoroutine);
				lerpCoroutine = null;
			}
		}
		else if (SkyManager.ReflectionMode != ReflectionType.None)
		{
			int desiredReflectionResolution = SkyManager.DesiredReflectionResolution;
			if (desiredReflectionResolution >= 8 && probe.resolution != desiredReflectionResolution)
			{
				SetResolution(desiredReflectionResolution);
			}
			if (SkyManager.UsingCustomEnvironmentLight)
			{
				if (probe.clearFlags != ReflectionProbeClearFlags.SolidColor)
				{
					probe.clearFlags = ReflectionProbeClearFlags.SolidColor;
				}
				probe.backgroundColor = solidColor;
			}
			else if (probe.clearFlags != ReflectionProbeClearFlags.Skybox)
			{
				probe.clearFlags = ReflectionProbeClearFlags.Skybox;
			}
			if (!probe.enabled)
			{
				return;
			}
			bool flag = probe.IsFinishedRendering(probeRenderID);
			if (probeRenderID == -1)
			{
				renderTextureIndex = 0;
				probeRenderID = probe.RenderProbe(renderTextureTMP);
				return;
			}
			timer += Time.deltaTime;
			if (timer >= timerInterval)
			{
				timer = 0f;
				if (flag && probeRenderID != 0)
				{
					probeRenderID = 0;
					RenderTexture dst = ((renderTextureIndex == 0) ? renderTexture0 : renderTexture1);
					Graphics.CopyTexture(renderTextureTMP, dst);
					RenderCubeMap();
					renderTextureIndex.IncrementCycle(1, 0, 1);
					probeRenderID = probe.RenderProbe(renderTextureTMP);
				}
			}
		}
		else
		{
			probeRenderID = -1;
			if (lerpCoroutine != null)
			{
				StopCoroutine(lerpCoroutine);
				lerpCoroutine = null;
			}
			if (RenderSettings.customReflection != cubemapSimple)
			{
				RenderSettings.customReflection = cubemapSimple;
			}
		}
	}

	private void SetResolution(int resolution)
	{
		if (resolution < 8)
		{
			resolution = 8;
		}
		renderTexture0 = new RenderTexture(resolution, resolution, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		renderTexture0.dimension = TextureDimension.Cube;
		renderTexture0.antiAliasing = 1;
		renderTexture0.useMipMap = true;
		renderTexture0.autoGenerateMips = true;
		renderTexture0.useDynamicScale = false;
		renderTexture0.wrapMode = TextureWrapMode.Clamp;
		renderTexture0.filterMode = FilterMode.Bilinear;
		renderTexture0.anisoLevel = 0;
		renderTexture1 = new RenderTexture(resolution, resolution, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		renderTexture1.dimension = TextureDimension.Cube;
		renderTexture1.antiAliasing = 1;
		renderTexture1.useMipMap = true;
		renderTexture1.autoGenerateMips = true;
		renderTexture1.useDynamicScale = false;
		renderTexture1.wrapMode = TextureWrapMode.Clamp;
		renderTexture1.filterMode = FilterMode.Bilinear;
		renderTexture1.anisoLevel = 0;
		renderTextureTMP = new RenderTexture(resolution, resolution, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		renderTextureTMP.dimension = TextureDimension.Cube;
		renderTextureTMP.antiAliasing = 1;
		renderTextureTMP.useMipMap = true;
		renderTextureTMP.autoGenerateMips = true;
		renderTextureTMP.useDynamicScale = false;
		renderTextureTMP.wrapMode = TextureWrapMode.Clamp;
		renderTextureTMP.filterMode = FilterMode.Bilinear;
		renderTextureTMP.anisoLevel = 0;
		renderTextureCombined = new RenderTexture(resolution, resolution, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		renderTextureCombined.dimension = TextureDimension.Cube;
		renderTextureCombined.antiAliasing = 1;
		renderTextureCombined.useMipMap = true;
		renderTextureCombined.autoGenerateMips = true;
		renderTextureCombined.useDynamicScale = false;
		renderTextureCombined.wrapMode = TextureWrapMode.Clamp;
		renderTextureCombined.filterMode = FilterMode.Bilinear;
		renderTextureCombined.anisoLevel = 0;
		probe.resolution = resolution;
		cubemapCurrent = new Cubemap(resolution, TextureFormat.RGBA32, mipChain: true);
	}

	private void RenderCubeMap()
	{
		StopLerpCoroutine();
		lerpCoroutine = StartCoroutine(LerpCubeMap());
	}

	private IEnumerator LerpCubeMap()
	{
		float startTime = Time.time;
		RenderTexture startTexture = ((renderTextureIndex == 1) ? renderTexture0 : renderTexture1);
		RenderTexture endTexture = ((renderTextureIndex == 0) ? renderTexture0 : renderTexture1);
		float normalizedTime = 0f;
		while (normalizedTime <= 1f)
		{
			normalizedTime = Mathf.Clamp01((Time.time - startTime) / lerpCubeTime);
			ReflectionProbe.BlendCubemap(startTexture, endTexture, normalizedTime, renderTextureCombined);
			Graphics.CopyTexture(renderTextureCombined, cubemapCurrent);
			RenderSettings.customReflection = cubemapCurrent;
			if (normalizedTime != 1f)
			{
				yield return null;
				continue;
			}
			break;
		}
	}

	private void StopLerpCoroutine()
	{
		if (lerpCoroutine != null)
		{
			StopCoroutine(lerpCoroutine);
			lerpCoroutine = null;
		}
	}
}
