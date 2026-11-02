using UnityEngine;

namespace UltimateWater.Internal;

public abstract class GpuFFT
{
	private Texture2D _Butterfly;

	protected RenderTexturesCache _RenderTexturesSet;

	protected int _Resolution;

	protected int _NumButterflies;

	protected int _NumButterfliesPow2;

	protected bool _TwoChannels;

	private readonly bool _HighPrecision;

	private readonly bool _UsesUav;

	public Texture2D Butterfly => _Butterfly;

	public int Resolution => _Resolution;

	public abstract void SetupMaterials();

	public abstract void ComputeFFT(Texture tex, RenderTexture target);

	public virtual void Dispose()
	{
		if (_Butterfly != null)
		{
			_Butterfly.Destroy();
			_Butterfly = null;
		}
	}

	private void CreateTextures()
	{
		CreateButterflyTexture();
	}

	private void RetrieveRenderTexturesSet()
	{
		RenderTextureFormat format;
		if (_TwoChannels)
		{
			format = (_HighPrecision ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.ARGBHalf);
		}
		else
		{
			format = (_HighPrecision ? RenderTextureFormat.RGFloat : RenderTextureFormat.RGHalf);
		}
		_RenderTexturesSet = RenderTexturesCache.GetCache(_Resolution << 1, _Resolution << 1, 0, format, linear: true, _UsesUav);
	}

	protected virtual void FillButterflyTexture(Texture2D butterfly, int[][] indices, Vector2[][] weights)
	{
		float num = _Resolution << 1;
		Color color = default;
		for (int i = 0; i < _NumButterflies; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				int num2 = ((j != 0) ? _Resolution : 0);
				for (int k = 0; k < _Resolution; k++)
				{
					int num3 = _NumButterflies - i - 1;
					int num4 = k << 1;
					color.r = ((float)(indices[num3][num4] + num2) + 0.5f) / num;
					color.g = ((float)(indices[num3][num4 + 1] + num2) + 0.5f) / num;
					color.b = weights[i][k].x;
					color.a = weights[i][k].y;
					butterfly.SetPixel(num2 + k, i, color);
				}
			}
		}
	}

	private void CreateButterflyTexture()
	{
		_Butterfly = new Texture2D(_Resolution << 1, _NumButterfliesPow2, _HighPrecision ? TextureFormat.RGBAFloat : TextureFormat.RGBAHalf, mipChain: false, linear: true)
		{
			hideFlags = HideFlags.DontSave,
			filterMode = FilterMode.Point,
			wrapMode = TextureWrapMode.Clamp
		};
		ButterflyFFTUtility.ComputeButterfly(_Resolution, _NumButterflies, out int[][] indices, out Vector2[][] weights);
		FillButterflyTexture(_Butterfly, indices, weights);
		_Butterfly.Apply();
	}

	protected GpuFFT(int resolution, bool highPrecision, bool twoChannels, bool usesUAV)
	{
		_Resolution = resolution;
		_HighPrecision = highPrecision;
		_NumButterflies = (int)(Mathf.Log(resolution) / Mathf.Log(2f));
		_NumButterfliesPow2 = Mathf.NextPowerOfTwo(_NumButterflies);
		_TwoChannels = twoChannels;
		_UsesUav = usesUAV;
		RetrieveRenderTexturesSet();
		CreateTextures();
	}
}
