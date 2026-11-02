using System;
using UnityEngine;

namespace UltimateWater;

[Serializable]
public class WaterRipplesData
{
	[Serializable]
	public enum ShaderModes
	{
		ComputeShader,
		PixelShader
	}

	[Tooltip("Static objects that interact with water (terrain, pillars, rocks)")]
	[SerializeField]
	private LayerMask _StaticDepthMask;

	[Tooltip("How many simulation steps are performed per frame, the more iterations, the faster the water waves are, but it's expensive")]
	[SerializeField]
	private int _Iterations = 1;

	[Header("Texture formats")]
	[Tooltip("Simulation data (only R channel is used)")]
	[SerializeField]
	private RenderTextureFormat _SimulationFormat = RenderTextureFormat.RHalf;

	[Tooltip("Screen space simulation data gather (only R channel is used)")]
	[SerializeField]
	private RenderTextureFormat _GatherFormat = RenderTextureFormat.RGHalf;

	[SerializeField]
	[Tooltip("Depth data (only R channel is used)")]
	private RenderTextureFormat _DepthFormat = RenderTextureFormat.RHalf;

	[SerializeField]
	[Header("Shaders")]
	private ShaderModes _ShaderMode = ShaderModes.PixelShader;

	public int Iterations => _Iterations;

	public ShaderModes ShaderMode => _ShaderMode;

	public LayerMask StaticDepthMask => _StaticDepthMask;

	public RenderTextureFormat SimulationFormat => _SimulationFormat;

	public RenderTextureFormat GatherFormat => _GatherFormat;

	public RenderTextureFormat DepthFormat => _DepthFormat;
}
