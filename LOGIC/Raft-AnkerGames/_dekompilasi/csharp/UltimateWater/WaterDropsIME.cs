using System;
using UltimateWater.Internal;
using UltimateWater.Utils;
using UnityEngine;

namespace UltimateWater;

public class WaterDropsIME : MonoBehaviour, IWaterImageEffect
{
	public enum Type
	{
		NormalMap,
		Blur
	}

	public interface IWaterDropsModule
	{
		void Initialize(WaterDropsIME ime);

		void Validate();

		void Advance();

		void UpdateMask(RenderTexture mask);

		void RenderImage(RenderTexture source, RenderTexture destination);
	}

	[Serializable]
	public struct BlurModule : IWaterDropsModule
	{
		public float FadeSpeed;

		private WaterDropsIME _Reference;

		private WaterCamera _Camera;

		private float _Intensity;

		[HideInInspector]
		[SerializeField]
		private Blur _Blur;

		public void Initialize(WaterDropsIME effect)
		{
			_Reference = effect;
			_Camera = effect.GetComponent<WaterCamera>();
		}

		public void Validate()
		{
			_Blur.Validate("UltimateWater/Utilities/Blur (VisionBlur)");
		}

		public void Advance()
		{
			if (Application.isPlaying)
			{
				_Intensity += Mathf.Max(0f, _Camera.WaterLevel - _Reference.transform.position.y);
				_Intensity *= 1f - Time.deltaTime * FadeSpeed;
				_Intensity = Mathf.Clamp01(_Intensity);
			}
		}

		public void RenderImage(RenderTexture source, RenderTexture destination)
		{
			float size = _Blur.Size;
			_Blur.Size *= _Intensity;
			_Blur.Apply(source);
			_Blur.Size = size;
			Graphics.Blit(source, destination);
		}

		public void UpdateMask(RenderTexture mask)
		{
		}
	}

	[Serializable]
	public struct NormalModule : IWaterDropsModule
	{
		public Texture NormalMap;

		public float Intensity;

		public bool Preview;

		private Material _Material;

		public void Initialize(WaterDropsIME effect)
		{
			_Material = ShaderUtility.Instance.CreateMaterial(ShaderList.WaterdropsNormal);
			_Material.SetTexture("_NormalMap", NormalMap);
		}

		public void UpdateMask(RenderTexture mask)
		{
			if (Preview)
			{
				Graphics.Blit(DefaultTextures.Get(Color.white), mask);
			}
			_Material.SetTexture("_Mask", mask);
		}

		public void RenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!Application.isPlaying && !Preview)
			{
				Graphics.Blit(source, destination);
				return;
			}
			_Material.SetFloat("_Intensity", Intensity);
			Graphics.Blit(source, destination, _Material);
		}

		public void Validate()
		{
			ShaderUtility.Instance.Use(ShaderList.WaterdropsNormal);
			if (_Material == null)
			{
				_Material = ShaderUtility.Instance.CreateMaterial(ShaderList.WaterdropsNormal);
			}
			_Material.SetTexture("_NormalMap", NormalMap);
		}

		public void Advance()
		{
		}
	}

	private struct MaskingModule
	{
		internal Material _Material;

		internal RenderTexture _MaskA;

		internal RenderTexture _MaskB;

		internal void Blit()
		{
			Graphics.Blit(_MaskA, _MaskB, _Material, 0);
		}

		internal void Swap()
		{
			RenderTexture maskA = _MaskA;
			_MaskA = _MaskB;
			_MaskB = maskA;
		}

		internal void Release()
		{
			TextureUtility.Release(ref _MaskA);
			TextureUtility.Release(ref _MaskB);
		}
	}

	[Range(0.95f, 1f)]
	[Tooltip("How slow the effects disappear")]
	public float Fade = 1f;

	public BlurModule Blur;

	public NormalModule Normal;

	private bool fullSubmersion;

	[SerializeField]
	private Type _Type;

	private MaskingModule _Masking;

	private IWaterDropsModule _SelectedModule;

	[InspectorWarning("Validation", InspectorWarningAttribute.InfoType.Warning)]
	[SerializeField]
	private string _Validation;

	public RenderTexture Mask => _Masking._MaskB;

	public void OnWaterCameraEnabled()
	{
		GetComponent<WaterCamera>().SubmersionStateChanged.AddListener(OnSubmersionStateChanged);
	}

	private void OnSubmersionStateChanged(WaterCamera waterCamera)
	{
		fullSubmersion = waterCamera.SubmersionState == SubmersionState.Full;
	}

	public void OnWaterCameraPreCull()
	{
	}

	private void Awake()
	{
		_Masking._Material = ShaderUtility.Instance.CreateMaterial(ShaderList.WaterdropsMask);
		AssignModule();
	}

	private void OnValidate()
	{
		ShaderUtility.Instance.Use(ShaderList.WaterdropsMask);
		AssignModule();
		_SelectedModule.Validate();
		if (Application.isPlaying && _Masking._Material != null)
		{
			_Masking._Material.SetFloat("_Fadeout", Fade * 0.98f);
		}
	}

	private void OnPreCull()
	{
		_SelectedModule.Advance();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (Application.isPlaying)
		{
			CheckResources();
			_Masking.Blit();
			_Masking.Swap();
			_SelectedModule.UpdateMask(_Masking._MaskB);
		}
		if (fullSubmersion)
		{
			Graphics.Blit(source, destination);
		}
		else
		{
			_SelectedModule.RenderImage(source, destination);
		}
	}

	private void Reset()
	{
		Normal.Intensity = 1f;
		Blur.FadeSpeed = 1f;
	}

	private void OnDestroy()
	{
		_Masking.Release();
	}

	private void AssignModule()
	{
		switch (_Type)
		{
		case Type.Blur:
			_SelectedModule = Blur;
			break;
		case Type.NormalMap:
			_SelectedModule = Normal;
			break;
		}
		_SelectedModule.Initialize(this);
	}

	private void CheckResources()
	{
		if (_Masking._MaskA == null || _Masking._MaskA.width != Screen.width >> 2 || _Masking._MaskA.height != Screen.height >> 2)
		{
			_Masking._MaskA = CreateMaskRt();
			_Masking._MaskB = CreateMaskRt();
			_Masking._MaskA.name = "[UWS] WaterDropsIME - Mask A";
			_Masking._MaskB.name = "[UWS] WaterDropsIME - Mask B";
		}
	}

	private static RenderTexture CreateMaskRt()
	{
		RenderTexture renderTexture = new RenderTexture(Screen.width >> 2, Screen.height >> 2, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear)
		{
			hideFlags = HideFlags.DontSave,
			filterMode = FilterMode.Bilinear,
			name = "[UWS] WaterDropsIME - CreateMaskRt"
		};
		Graphics.SetRenderTarget(renderTexture);
		GL.Clear(clearDepth: false, clearColor: true, Color.black);
		return renderTexture;
	}

	private string Validation()
	{
		string text = string.Empty;
		if (_Type == Type.NormalMap && Normal.NormalMap == null)
		{
			text += "warning: select normal map texture";
		}
		return text;
	}
}
