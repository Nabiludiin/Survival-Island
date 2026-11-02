using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

public sealed class SpritesheetAnimation : MonoBehaviour
{
	[SerializeField]
	[FormerlySerializedAs("horizontal")]
	private int _Horizontal = 2;

	[SerializeField]
	[FormerlySerializedAs("vertical")]
	private int _Vertical = 2;

	[SerializeField]
	[FormerlySerializedAs("timeStep")]
	private float _TimeStep = 0.06f;

	[SerializeField]
	[FormerlySerializedAs("loop")]
	private bool _Loop;

	[SerializeField]
	[FormerlySerializedAs("destroyGo")]
	private bool _DestroyGo;

	private Material _Material;

	private float _NextChangeTime;

	private int _X;

	private int _Y;

	private void Start()
	{
		Renderer component = GetComponent<Renderer>();
		_Material = component.material;
		_Material.mainTextureScale = new Vector2(1f / (float)_Horizontal, 1f / (float)_Vertical);
		_Material.mainTextureOffset = new Vector2(0f, 0f);
		_NextChangeTime = Time.time + _TimeStep;
	}

	private void Update()
	{
		if (!(Time.time >= _NextChangeTime))
		{
			return;
		}
		_NextChangeTime += _TimeStep;
		if (_X == _Horizontal - 1 && _Y == _Vertical - 1)
		{
			if (!_Loop)
			{
				if (_DestroyGo)
				{
					Object.Destroy(gameObject);
				}
				else
				{
					enabled = false;
				}
				return;
			}
			_X = 0;
			_Y = 0;
		}
		else
		{
			_X++;
			if (_X >= _Horizontal)
			{
				_X = 0;
				_Y++;
			}
		}
		_Material.mainTextureOffset = new Vector2((float)_X / (float)_Horizontal, 1f - (float)(_Y + 1) / (float)_Vertical);
	}

	private void OnDestroy()
	{
		if (_Material != null)
		{
			Object.Destroy(_Material);
			_Material = null;
		}
	}
}
