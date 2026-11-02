using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater.Internal;

[RequireComponent(typeof(Renderer))]
public sealed class WaterSimpleMask : MonoBehaviour
{
	[SerializeField]
	[FormerlySerializedAs("water")]
	private Water _Water;

	[SerializeField]
	[FormerlySerializedAs("renderQueuePriority")]
	private int _RenderQueuePriority;

	private const string _WaterIdName = "_WaterId";

	public Renderer Renderer { get; private set; }

	public int RenderQueuePriority => _RenderQueuePriority;

	public Water Water
	{
		get
		{
			return _Water;
		}
		set
		{
			if (!(_Water == value))
			{
				enabled = false;
				_Water = value;
				enabled = true;
			}
		}
	}

	private void OnValidate()
	{
		SetObjectLayer();
	}

	private void OnEnable()
	{
		Renderer = GetComponent<Renderer>();
		Renderer.enabled = false;
		Renderer.material.SetFloat("_WaterId", 1 << _Water.WaterId);
		SetObjectLayer();
		if (Renderer == null)
		{
			throw new InvalidOperationException("WaterSimpleMask is attached to an object without any renderer.");
		}
		_Water.Renderer.AddMask(this);
		_Water.WaterIdChanged += OnWaterIdChanged;
	}

	private void OnDisable()
	{
		_Water.WaterIdChanged -= OnWaterIdChanged;
		_Water.Renderer.RemoveMask(this);
	}

	private void SetObjectLayer()
	{
		if (gameObject.layer != WaterProjectSettings.Instance.WaterTempLayer)
		{
			gameObject.layer = WaterProjectSettings.Instance.WaterTempLayer;
		}
	}

	private void OnWaterIdChanged()
	{
		GetComponent<Renderer>().material.SetFloat("_WaterId", 1 << _Water.WaterId);
	}
}
