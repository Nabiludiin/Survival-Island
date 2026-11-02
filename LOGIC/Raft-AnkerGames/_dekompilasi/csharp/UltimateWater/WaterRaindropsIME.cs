using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater;

[RequireComponent(typeof(Camera))]
public class WaterRaindropsIME : MonoBehaviour
{
	[Serializable]
	public class FadeModule
	{
		[Tooltip("1 - no fade, 0 - instant fade")]
		[Range(0.5f, 1f)]
		public float Intensity = 0.99f;

		[Tooltip("Additional texture based fade")]
		public Texture2D Texture;

		[Range(0f, 1f)]
		public float Multiplier = 0.1f;
	}

	[Serializable]
	public class DistortionModule
	{
		public float Multiplier = 1f;

		[Range(0.001f, 0.008f)]
		public float NormalSpread = 0.002f;
	}

	[Serializable]
	public class TwirlModule
	{
		public Texture2D Texture;

		[Range(0f, 1f)]
		public float Multiplier = 0.1f;
	}

	[Serializable]
	public class TrackingModule
	{
		[Header("Force multipliers")]
		[SerializeField]
		private float _Translation;

		[SerializeField]
		private float _Rotation;

		private WaterRaindropsIME _Reference;

		private Vector3 _PreviousPosition;

		private Vector3 _Velocity;

		public Vector3 Force => -_Velocity;

		internal void Initialize(WaterRaindropsIME reference)
		{
			_Reference = reference;
			_PreviousPosition = _Reference.transform.forward * _Rotation + _Reference.transform.position * _Translation;
		}

		internal void Advance()
		{
			Vector3 vector = _Reference.transform.forward * _Rotation + _Reference.transform.position * _Translation;
			_Velocity = _Reference.transform.worldToLocalMatrix * ((vector - _PreviousPosition) / Time.fixedDeltaTime);
			_PreviousPosition = vector;
		}
	}

	private struct Droplet
	{
		public Vector2 Position;

		public Vector2 Velocity;

		public float Volume;

		public float Life;
	}

	[SerializeField]
	[HideInInspector]
	private Cubemap _CubeMap;

	[Header("Settings")]
	public Vector3 Force = Vector3.down;

	public float VolumeLoss = 0.02f;

	[Range(0.1f, 1f)]
	public float Resolution = 0.5f;

	[Header("Friction")]
	[Range(0f, 1f)]
	[Tooltip("Air resistance causing raindrops to slow down")]
	public float AirFriction = 0.5f;

	[Range(0f, 10f)]
	public float WindowFrictionMultiplier = 0.5f;

	[Tooltip("Adds forces caused by lens imperfections")]
	public Texture2D WindowFriction;

	[Tooltip("How much the water bends light")]
	public DistortionModule Distortion;

	[Tooltip("Distorts water paths using custom texture")]
	public TwirlModule Twirl;

	[Tooltip("How much time is needed for raindrops to disappear")]
	public FadeModule Fade;

	[Tooltip("How much force is applied to raindrops from camera movement")]
	public TrackingModule Tracking;

	private RenderTexture _Target;

	private Vector2[,] _Friction;

	private CommandBuffer _Buffer;

	private Mesh _Mesh;

	private Matrix4x4[] _Matrices;

	private readonly List<Droplet> _Droplets = new List<Droplet>();

	private bool _Initialized;

	private Material _FadeMaterial;

	private Material _DropletMaterial;

	private Material _FinalMaterial;

	private Material _InvertMaterial;

	public Vector3 WorldForce => transform.worldToLocalMatrix * Force;

	public void Spawn(Vector3 velocity, float volume, float life, float x, float y)
	{
		Vector2 position = new Vector2(x, y);
		Droplet item = default;
		item.Position = position;
		item.Volume = volume;
		item.Velocity = transform.worldToLocalMatrix * -velocity;
		item.Velocity.x = 0f - item.Velocity.x;
		item.Life = life;
		_Droplets.Add(item);
	}

	private void Awake()
	{
		CreateMaterials();
		CreateSimulation();
		SetMaterialProperties();
	}

	private void Start()
	{
		Tracking.Initialize(this);
	}

	private void OnPreCull()
	{
		Render();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		FadeMaps();
		_FinalMaterial.SetTexture("unity_Spec", _CubeMap);
		if (source.texelSize.y < 0f)
		{
			if (_InvertMaterial == null)
			{
				_InvertMaterial = new Material(Shader.Find("Hidden/InvertY"));
			}
			RenderTexture renderTexture = source.CreateTemporary();
			Graphics.Blit(source, renderTexture);
			Graphics.Blit(renderTexture, source);
			Graphics.Blit(source, renderTexture, _FinalMaterial);
			Graphics.Blit(renderTexture, destination, _InvertMaterial);
			renderTexture.ReleaseTemporary();
		}
		else
		{
			Graphics.Blit(source, destination, _FinalMaterial);
		}
	}

	private void Update()
	{
		Tracking.Advance();
		Advance();
	}

	private void OnValidate()
	{
		if (Application.isPlaying && _Initialized)
		{
			SetMaterialProperties();
		}
	}

	private void OnDestroy()
	{
		TextureUtility.Release(ref _Target);
	}

	private static bool IsVisible(Droplet droplet)
	{
		return (byte)(1u & ((droplet.Position.x >= 0f && droplet.Position.x <= 1f) ? 1u : 0u) & ((droplet.Position.y >= 0f && droplet.Position.y <= 1f) ? 1u : 0u)) != 0;
	}

	private void OnDropletUpdate(ref Droplet droplet)
	{
		Vector2 position = droplet.Position;
		Vector2 velocity = droplet.Velocity;
		droplet.Life -= Time.fixedDeltaTime;
		if (!(droplet.Volume < 0.2f))
		{
			Vector4 vector = transform.worldToLocalMatrix * Force;
			Vector2 vector2 = new Vector2(vector.x, 0f - vector.y);
			Vector2 vector3 = new Vector2(Tracking.Force.x, Tracking.Force.y);
			Vector2 vector4 = Vector2.zero;
			if (_Friction != null)
			{
				vector4 = _Friction[(int)(position.x * (float)(_Target.width - 1)), (int)(position.y * (float)(_Target.height - 1))];
			}
			float magnitude = velocity.magnitude;
			Vector2 vector5 = -velocity.normalized * ((magnitude + 1f) * (magnitude + 1f)) * AirFriction;
			Vector2 vector6 = -vector4 * WindowFrictionMultiplier;
			Vector2 vector7 = (vector2 + vector3 + vector5 + vector6) / droplet.Volume;
			droplet.Velocity += vector7 * Time.deltaTime;
			droplet.Position += droplet.Velocity * Time.deltaTime;
			float num = Vector3.Distance(droplet.Position, position) * VolumeLoss + 0.001f;
			droplet.Volume -= num;
		}
	}

	private void Draw(int index, Vector3 position, float size, Vector2 velocity)
	{
		float angle = Mathf.Atan2(velocity.y, velocity.x) * 57.29578f;
		_Matrices[index] = Matrix4x4.TRS(position * 2f - new Vector3(1f, 1f, 0f), Quaternion.AngleAxis(angle, Vector3.forward), new Vector3(1f + velocity.magnitude * 10f, 1f, 1f) * size * 0.1f);
	}

	private void Advance()
	{
		_Droplets.RemoveAll((Droplet x) => !IsVisible(x) || x.Life <= 0f || x.Volume <= 0f);
		for (int num = 0; num < _Droplets.Count; num++)
		{
			Droplet droplet = _Droplets[num];
			OnDropletUpdate(ref droplet);
			if (!IsVisible(droplet))
			{
				_Droplets[num] = droplet;
				continue;
			}
			Draw(num, droplet.Position, droplet.Volume, droplet.Velocity);
			_Droplets[num] = droplet;
		}
	}

	private void FadeMaps()
	{
		RenderTexture temporary = RenderTexture.GetTemporary(_Target.width, _Target.height, _Target.depth, _Target.format);
		temporary.filterMode = _Target.filterMode;
		_FadeMaterial.SetVector("_Modulation_STX", Vector4.one * 10f);
		Graphics.Blit(_Target, temporary, _FadeMaterial);
		Graphics.CopyTexture(temporary, _Target);
		RenderTexture.ReleaseTemporary(temporary);
	}

	private void Render()
	{
		_Buffer.Clear();
		_Buffer.SetRenderTarget(_Target);
		for (int i = 0; i < _Droplets.Count; i++)
		{
			_Buffer.DrawMesh(_Mesh, _Matrices[i], _DropletMaterial);
		}
		Graphics.ExecuteCommandBuffer(_Buffer);
	}

	private void CreateSimulation()
	{
		_Initialized = true;
		CreateRenderTexture();
		_Matrices = new Matrix4x4[4096];
		_Buffer = new CommandBuffer();
		_Mesh = BuildQuad(1f, 1f);
		CreateFrictionMap();
	}

	private void CreateFrictionMap()
	{
		if (!(_Target == null) && !(WindowFriction == null))
		{
			_Friction = Sample(WindowFriction, _Target.width, _Target.height);
		}
	}

	private void CreateRenderTexture()
	{
		TextureUtility.RenderTextureDesc renderTextureDesc = new TextureUtility.RenderTextureDesc("[UWS] WaterRaindropsIME - Raindrops");
		renderTextureDesc.Height = (int)((float)Screen.height * Resolution);
		renderTextureDesc.Width = (int)((float)Screen.width * Resolution);
		renderTextureDesc.Format = RenderTextureFormat.RFloat;
		renderTextureDesc.Filter = FilterMode.Bilinear;
		TextureUtility.RenderTextureDesc desc = renderTextureDesc;
		_Target = desc.CreateRenderTexture();
		_Target.Clear();
	}

	private void CreateMaterials()
	{
		ShaderUtility instance = ShaderUtility.Instance;
		_FinalMaterial = instance.CreateMaterial(ShaderList.RaindropsFinal);
		_FadeMaterial = instance.CreateMaterial(ShaderList.RaindropsFade);
		_DropletMaterial = instance.CreateMaterial(ShaderList.RaindropsParticle);
	}

	private void SetMaterialProperties()
	{
		_FinalMaterial.SetTexture("_WaterDropsTex", _Target);
		_FinalMaterial.SetFloat("_NormalSpread", Distortion.NormalSpread);
		_FinalMaterial.SetFloat("_Distortion", Distortion.Multiplier);
		if (Twirl.Texture != null)
		{
			_FinalMaterial.SetTexture("_Twirl", Twirl.Texture);
			_FinalMaterial.SetFloat("_TwirlMultiplier", Twirl.Multiplier);
		}
		_FadeMaterial.SetFloat("_Value", Fade.Intensity);
		_FadeMaterial.SetFloat("_ModulationStrength", Fade.Multiplier);
		if (Fade.Texture != null)
		{
			_FadeMaterial.SetTexture("_Modulation", Fade.Texture);
		}
	}

	private static Mesh BuildQuad(float width, float height)
	{
		Mesh mesh = new Mesh();
		Vector3[] array = new Vector3[4];
		float num = height * 0.5f;
		float num2 = width * 0.5f;
		array[0] = new Vector3(0f - num2, 0f - num, 0f);
		array[1] = new Vector3(0f - num2, num, 0f);
		array[2] = new Vector3(num2, 0f - num, 0f);
		array[3] = new Vector3(num2, num, 0f);
		Vector2[] array2 = new Vector2[array.Length];
		array2[0] = new Vector2(0f, 0f);
		array2[1] = new Vector2(0f, 1f);
		array2[2] = new Vector2(1f, 0f);
		array2[3] = new Vector2(1f, 1f);
		int[] triangles = new int[6] { 0, 1, 2, 3, 2, 1 };
		Vector3[] array3 = new Vector3[array.Length];
		for (int i = 0; i < array3.Length; i++)
		{
			array3[i] = Vector3.forward;
		}
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.triangles = triangles;
		mesh.normals = array3;
		return mesh;
	}

	private Vector2[,] Sample(Texture texture, int width, int height, int step = 4)
	{
		Vector2[,] array = new Vector2[width, height];
		Color[] pixels = WindowFriction.GetPixels();
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				float num = (float)j / (float)width;
				float num2 = (float)i / (float)height;
				int num3 = (int)(num * (float)texture.width);
				int num4 = (int)(num2 * (float)texture.height);
				int num5 = num4 * width + num3 - step;
				int num6 = num4 * width + num3 + step;
				int num7 = (num4 + step) * width + num3;
				int num8 = (num4 - step) * width + num3;
				float x = 0f;
				float y = 0f;
				if (IsValidTextureIndex(num5, width, height) && IsValidTextureIndex(num6, width, height))
				{
					x = pixels[num6].r - pixels[num5].r;
				}
				if (IsValidTextureIndex(num7, width, height) && IsValidTextureIndex(num8, width, height))
				{
					y = pixels[num7].r - pixels[num8].r;
				}
				array[j, i] = new Vector2(x, y);
			}
		}
		return array;
	}

	private static bool IsValidTextureIndex(int index, int width, int height)
	{
		int num = width * height;
		if (index >= 0)
		{
			return index < num;
		}
		return false;
	}
}
