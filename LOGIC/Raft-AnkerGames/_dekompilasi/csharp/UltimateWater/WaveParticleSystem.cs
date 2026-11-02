using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

[RequireComponent(typeof(DynamicWater))]
[AddComponentMenu("Water/Waves Particle System", 1)]
public sealed class WaveParticleSystem : MonoBehaviour, IOverlaysRenderer
{
	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("waterWavesParticlesShader")]
	private Shader _WaterWavesParticlesShader;

	[SerializeField]
	[FormerlySerializedAs("maxParticles")]
	private int _MaxParticles = 50000;

	[SerializeField]
	[FormerlySerializedAs("maxParticlesPerTile")]
	private int _MaxParticlesPerTile = 2000;

	[SerializeField]
	[FormerlySerializedAs("prewarmTime")]
	private float _PrewarmTime = 40f;

	[Tooltip("Allowed execution time per frame.")]
	[SerializeField]
	[FormerlySerializedAs("timePerFrame")]
	private float _TimePerFrame = 0.8f;

	private WaveParticlesQuadtree _Particles;

	private Water _Water;

	private Material _WaterWavesParticlesMaterial;

	private float _SimulationTime;

	private float _TimePerFrameExp;

	private bool _Prewarmed;

	private readonly List<IWavesParticleSystemPlugin> _Plugins;

	public int ParticleCount => _Particles.Count;

	public float SimulationTime => _SimulationTime;

	public WaveParticleSystem()
	{
		_Plugins = new List<IWavesParticleSystemPlugin>();
	}

	public bool Spawn(WaveParticle particle, int clones, float waveShapeIrregularity, float centerElevation = 2f, float edgesElevation = 0.35f)
	{
		if (particle == null || _Particles.FreeSpace < clones * 2 + 1)
		{
			return false;
		}
		particle.Group = new WaveParticlesGroup(_SimulationTime);
		particle.BaseAmplitude *= _Water.UniformWaterScale;
		particle.BaseFrequency /= _Water.UniformWaterScale;
		WaveParticle waveParticle = null;
		float minInclusive = 1f / waveShapeIrregularity;
		for (int i = -clones; i <= clones; i++)
		{
			WaveParticle waveParticle2 = particle.Clone(particle.Position + new Vector2(particle.Direction.y, 0f - particle.Direction.x) * ((float)i * 1.48f / particle.BaseFrequency));
			if (waveParticle2 == null)
			{
				continue;
			}
			waveParticle2.AmplitudeModifiers2 = UnityEngine.Random.Range(minInclusive, 1f) * (edgesElevation + (0.5f + Mathf.Cos(MathF.PI * (float)i / (float)clones) * 0.5f) * (centerElevation - edgesElevation));
			waveParticle2.LeftNeighbour = waveParticle;
			if (waveParticle != null)
			{
				waveParticle.RightNeighbour = waveParticle2;
				if (i == clones)
				{
					waveParticle2.DisallowSubdivision = true;
				}
			}
			else
			{
				waveParticle2.Group.LeftParticle = waveParticle2;
				waveParticle2.DisallowSubdivision = true;
			}
			if (!_Particles.AddElement(waveParticle2))
			{
				return waveParticle != null;
			}
			waveParticle = waveParticle2;
		}
		return true;
	}

	public void RenderOverlays(DynamicWaterCameraData overlays)
	{
	}

	public void RenderFoam(DynamicWaterCameraData overlays)
	{
		if (enabled)
		{
			RenderParticles(overlays);
		}
	}

	public void RegisterPlugin(IWavesParticleSystemPlugin plugin)
	{
		if (!_Plugins.Contains(plugin))
		{
			_Plugins.Add(plugin);
		}
	}

	public void UnregisterPlugin(IWavesParticleSystemPlugin plugin)
	{
		_Plugins.Remove(plugin);
	}

	public bool AddParticle(WaveParticle particle)
	{
		if (particle != null)
		{
			if (particle.Group == null)
			{
				throw new ArgumentException("Particle has no group");
			}
			return _Particles.AddElement(particle);
		}
		return false;
	}

	private void LateUpdate()
	{
		if (!_Prewarmed)
		{
			Prewarm();
		}
		UpdateSimulation(Time.deltaTime);
	}

	private void OnValidate()
	{
		_TimePerFrameExp = Mathf.Exp(_TimePerFrame * 0.5f);
		if (_WaterWavesParticlesShader == null)
		{
			_WaterWavesParticlesShader = Shader.Find("UltimateWater/Particles/Particles");
		}
		if (_Particles != null)
		{
			_Particles.DebugMode = _Water.ShaderSet.LocalEffectsDebug;
		}
	}

	private void Awake()
	{
		_Water = GetComponent<Water>();
		OnValidate();
	}

	private void OnEnable()
	{
		CheckResources();
	}

	private void OnDisable()
	{
		FreeResources();
	}

	private void Prewarm()
	{
		_Prewarmed = true;
		while (_SimulationTime < _PrewarmTime)
		{
			UpdateSimulationWithoutFrameBudget(0.1f);
		}
	}

	private void UpdateSimulation(float deltaTime)
	{
		_SimulationTime += deltaTime;
		UpdatePlugins(deltaTime);
		_Particles.UpdateSimulation(_SimulationTime, _TimePerFrameExp);
	}

	private void UpdateSimulationWithoutFrameBudget(float deltaTime)
	{
		_SimulationTime += deltaTime;
		UpdatePlugins(deltaTime);
		_Particles.UpdateSimulation(_SimulationTime);
	}

	private void UpdatePlugins(float deltaTime)
	{
		int count = _Plugins.Count;
		for (int i = 0; i < count; i++)
		{
			_Plugins[i].UpdateParticles(_SimulationTime, deltaTime);
		}
	}

	private void RenderParticles(DynamicWaterCameraData overlays)
	{
		Spray component = GetComponent<Spray>();
		if (component != null && component.ParticlesBuffer != null)
		{
			Graphics.SetRandomWriteTarget(3, component.ParticlesBuffer);
		}
		if (!_Water.ShaderSet.LocalEffectsDebug)
		{
			Graphics.SetRenderTarget(new RenderBuffer[2]
			{
				overlays.DynamicDisplacementMap.colorBuffer,
				overlays.NormalMap.colorBuffer
			}, overlays.DynamicDisplacementMap.depthBuffer);
		}
		else
		{
			Graphics.SetRenderTarget(new RenderBuffer[3]
			{
				overlays.DynamicDisplacementMap.colorBuffer,
				overlays.NormalMap.colorBuffer,
				overlays.GetDebugMap(createIfNotExists: true).colorBuffer
			}, overlays.DynamicDisplacementMap.depthBuffer);
		}
		Shader.SetGlobalMatrix("_ParticlesVP", GL.GetGPUProjectionMatrix(overlays.Camera.PlaneProjectorCamera.projectionMatrix, renderIntoTexture: true) * overlays.Camera.PlaneProjectorCamera.worldToCameraMatrix);
		Vector4 localMapsShaderCoords = overlays.Camera.LocalMapsShaderCoords;
		float uniformWaterScale = GetComponent<Water>().UniformWaterScale;
		_WaterWavesParticlesMaterial.SetFloat("_WaterScale", uniformWaterScale);
		_WaterWavesParticlesMaterial.SetVector("_LocalMapsCoords", localMapsShaderCoords);
		_WaterWavesParticlesMaterial.SetPass(_Water.ShaderSet.LocalEffectsDebug ? 1 : 0);
		_Particles.Render(overlays.Camera.LocalMapsRect);
		Graphics.ClearRandomWriteTargets();
	}

	private void CheckResources()
	{
		if (_WaterWavesParticlesMaterial == null)
		{
			_WaterWavesParticlesMaterial = new Material(_WaterWavesParticlesShader)
			{
				hideFlags = HideFlags.DontSave
			};
		}
		if (_Particles == null)
		{
			_Particles = new WaveParticlesQuadtree(new Rect(-1000f, -1000f, 2000f, 2000f), _MaxParticlesPerTile, _MaxParticles)
			{
				DebugMode = _Water.ShaderSet.LocalEffectsDebug
			};
		}
	}

	private void FreeResources()
	{
		if (_WaterWavesParticlesMaterial != null)
		{
			_WaterWavesParticlesMaterial.Destroy();
			_WaterWavesParticlesMaterial = null;
		}
	}
}
