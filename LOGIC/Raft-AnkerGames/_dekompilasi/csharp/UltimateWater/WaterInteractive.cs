using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater;

[RequireComponent(typeof(Renderer))]
[AddComponentMenu("Ultimate Water/Dynamic/Water Interactive")]
public sealed class WaterInteractive : MonoBehaviour, IWavesInteractive, IDynamicWaterEffects
{
	[Tooltip("How much velocity modifies wave amplitude")]
	public float Multiplier = 1f;

	private Matrix4x4 _Previous;

	private Renderer _Renderer;

	private Material _Material;

	public void Render(CommandBuffer commandBuffer)
	{
		if (_Renderer.enabled)
		{
			commandBuffer.DrawRenderer(_Renderer, _Material);
		}
	}

	public void Enable()
	{
	}

	public void Disable()
	{
	}

	private void Awake()
	{
		_Material = ShaderUtility.Instance.CreateMaterial(ShaderList.Velocity);
		if (!_Material.IsNullReference(this))
		{
			_Renderer = GetComponent<Renderer>();
			if (!_Renderer.IsNullReference(this))
			{
				_Previous = transform.localToWorldMatrix;
			}
		}
	}

	private void OnEnable()
	{
		DynamicWater.AddRenderer(this);
	}

	private void OnDisable()
	{
		DynamicWater.RemoveRenderer(this);
	}

	private void FixedUpdate()
	{
		Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;
		_Material.SetMatrix("_PreviousWorld", _Previous);
		_Material.SetMatrix("_CurrentWorld", localToWorldMatrix);
		_Material.SetFloat("_Data", Multiplier / Time.fixedDeltaTime);
		_Previous = localToWorldMatrix;
	}
}
