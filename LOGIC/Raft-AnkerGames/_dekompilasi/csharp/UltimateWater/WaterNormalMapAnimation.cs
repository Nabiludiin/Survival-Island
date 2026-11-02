using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

[RequireComponent(typeof(Water))]
public sealed class WaterNormalMapAnimation : MonoBehaviour
{
	[FormerlySerializedAs("normalMapShader")]
	[HideInInspector]
	[SerializeField]
	private Shader _NormalMapShader;

	[SerializeField]
	[FormerlySerializedAs("resolution")]
	private int _Resolution = 512;

	[FormerlySerializedAs("period")]
	[SerializeField]
	private float _Period = 60f;

	[FormerlySerializedAs("animationSpeed")]
	[SerializeField]
	private float _AnimationSpeed = 0.015f;

	[SerializeField]
	[FormerlySerializedAs("intensity")]
	private float _Intensity = 2f;

	private Water _Water;

	private RenderTexture _NormalMap1;

	private Texture _SourceNormalMap;

	private Material _NormalMapMaterial;

	private void Start()
	{
		OnValidate();
		_NormalMap1 = new RenderTexture(_Resolution, _Resolution, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
		{
			name = "[UWS] WaterNormalMapAnimation - Normal Map 1",
			wrapMode = TextureWrapMode.Repeat
		};
		_NormalMapMaterial = new Material(_NormalMapShader)
		{
			hideFlags = HideFlags.DontSave
		};
		_Water = GetComponent<Water>();
		_SourceNormalMap = _Water.Materials.SurfaceMaterial.GetTexture("_BumpMap");
		_Water.Materials.SurfaceMaterial.SetTexture("_BumpMap", _NormalMap1);
	}

	private void OnValidate()
	{
		if (_NormalMapShader == null)
		{
			_NormalMapShader = Shader.Find("UltimateWater/Utilities/WaterNormalMap");
		}
	}

	private void Update()
	{
		_NormalMapMaterial.SetVector(ShaderVariables.Offset, new Vector4(0f, 0f, Time.time * _AnimationSpeed, 0f));
		_NormalMapMaterial.SetVector(ShaderVariables.Period, new Vector4(_Period, _Period, _Period, _Period));
		_NormalMapMaterial.SetFloat("_Param", _Intensity);
		Graphics.Blit(_SourceNormalMap, _NormalMap1, _NormalMapMaterial, 0);
	}
}
