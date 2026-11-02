using System;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class SetScrollRectPosition : MonoBehaviour
{
	private enum EScrollbarBehaviour
	{
		PERMANANENT_VISIBILITY,
		AUTO_HIDE,
		AUTO_DISABLE
	}

	[SerializeField]
	private ScrollRect _scrollRect;

	[SerializeField]
	private Slider _slider;

	[SerializeField]
	private bool _horizontalSlider;

	[SerializeField]
	private EScrollbarBehaviour _scrollbarBehaviour;

	private bool _updateScrollRect = true;

	private RectTransform _viewRect;

	private RectTransform _contentTransform;

	private readonly Vector3[] _corners = new Vector3[4];

	private Bounds _contentBounds;

	private Bounds _viewBounds;

	private const float UNITY_INTERNAL_DISASSEMBLED_CONST = 0.01f;

	private bool HScrollingNeeded
	{
		get
		{
			if (Application.isPlaying)
			{
				return _contentBounds.size.x > _viewBounds.size.x + 0.01f;
			}
			return true;
		}
	}

	private bool VScrollingNeeded
	{
		get
		{
			if (Application.isPlaying)
			{
				return _contentBounds.size.y > _viewBounds.size.y + 0.01f;
			}
			return true;
		}
	}

	private void Awake()
	{
		_contentTransform = _scrollRect.content;
		_viewRect = _scrollRect.viewport;
	}

	private void Update()
	{
		RefreshScrollBarVisibility();
	}

	private void OnEnable()
	{
		RefreshScrollBarVisibility();
	}

	public void SetScrollRectPos(Vector2 pos)
	{
		int num = 0;
		int num2 = 0;
		if (_slider.direction == Slider.Direction.TopToBottom)
		{
			num2 = 1;
		}
		if (_slider.direction == Slider.Direction.LeftToRight)
		{
			num = 1;
		}
		if (_horizontalSlider)
		{
			_scrollRect.horizontalNormalizedPosition = Mathf.Clamp((float)num - pos.x, 0f, 1f);
		}
		else
		{
			_scrollRect.verticalNormalizedPosition = Mathf.Clamp((float)num2 - pos.y, 0f, 1f);
		}
	}

	public void ChangeScrollRectPos()
	{
		if (_updateScrollRect)
		{
			int num = 0;
			int num2 = 0;
			if (_slider.direction == Slider.Direction.TopToBottom)
			{
				num2 = 1;
			}
			if (_slider.direction == Slider.Direction.LeftToRight)
			{
				num = 1;
			}
			if (_horizontalSlider)
			{
				_scrollRect.horizontalNormalizedPosition = Mathf.Clamp((float)num - _slider.value, 0f, 1f);
			}
			else
			{
				_scrollRect.verticalNormalizedPosition = Mathf.Clamp((float)num2 - _slider.value, 0f, 1f);
			}
		}
	}

	public void SetSliderValueWithoutUpdatingScrollRect(Vector2 value)
	{
		_updateScrollRect = false;
		if (!_slider.interactable)
		{
			_slider.value = 0f;
		}
		else
		{
			int num = 0;
			int num2 = 0;
			if (_slider.direction == Slider.Direction.TopToBottom)
			{
				num2 = 1;
			}
			if (_slider.direction == Slider.Direction.LeftToRight)
			{
				num = 1;
			}
			_slider.value = (_horizontalSlider ? Mathf.Clamp((float)num - value.x, 0f, 1f) : Mathf.Clamp((float)num2 - value.y, 0f, 1f));
		}
		_updateScrollRect = true;
	}

	private void ShouldReactivate()
	{
		switch (_scrollbarBehaviour)
		{
		case EScrollbarBehaviour.PERMANANENT_VISIBILITY:
			if (!_slider.gameObject.activeSelf)
			{
				_slider.gameObject.SetActive(value: true);
			}
			break;
		case EScrollbarBehaviour.AUTO_HIDE:
			if (_horizontalSlider)
			{
				_slider.gameObject.SetActive(HScrollingNeeded);
			}
			else
			{
				_slider.gameObject.SetActive(VScrollingNeeded);
			}
			break;
		case EScrollbarBehaviour.AUTO_DISABLE:
			if (_horizontalSlider)
			{
				_slider.interactable = HScrollingNeeded;
			}
			else
			{
				_slider.interactable = VScrollingNeeded;
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public void RefreshScrollBarVisibility()
	{
		_contentBounds = GetBounds();
		_viewBounds = new Bounds(_viewRect.rect.center, _viewRect.rect.size);
		ShouldReactivate();
	}

	private Bounds GetBounds()
	{
		if (_contentTransform == null)
		{
			return default;
		}
		_contentTransform.GetWorldCorners(_corners);
		Matrix4x4 viewWorldToLocalMatrix = _viewRect.worldToLocalMatrix;
		return InternalGetBounds(_corners, ref viewWorldToLocalMatrix);
	}

	internal static Bounds InternalGetBounds(Vector3[] corners, ref Matrix4x4 viewWorldToLocalMatrix)
	{
		Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		for (int i = 0; i < 4; i++)
		{
			Vector3 lhs = viewWorldToLocalMatrix.MultiplyPoint3x4(corners[i]);
			vector = Vector3.Min(lhs, vector);
			vector2 = Vector3.Max(lhs, vector2);
		}
		Bounds result = new Bounds(vector, Vector3.zero);
		result.Encapsulate(vector2);
		return result;
	}
}
