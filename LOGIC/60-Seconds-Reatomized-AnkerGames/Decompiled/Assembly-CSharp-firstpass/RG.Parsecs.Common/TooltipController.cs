using System.Collections.Generic;
using RG.Parsecs.Survival;
using RG.VirtualInput;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RG.Parsecs.Common;

public class TooltipController : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	private enum TooltipFlow
	{
		ON_CLICK,
		HOVER,
		HOVER_KEEP_POSITION
	}

	[SerializeField]
	private TooltipFlow _tooltipFlow = TooltipFlow.HOVER;

	private const int TOOLTIP_OFFSET = 5;

	[Tooltip("The tags that this tooltip has")]
	[SerializeField]
	private List<TooltipTag> _tooltipTags;

	[Tooltip("Tag data set up in the TooltipTagManager")]
	[SerializeField]
	private TooltipTagData _tagData;

	[SerializeField]
	private RectTransform _tooltipRectTransform;

	[SerializeField]
	private TooltipPivotOverride _gamepadTooltipPivotOverride;

	[SerializeField]
	[Tooltip("Time the user must hold the cursor over the object to see the tooltip, in seconds")]
	private float _timeToDisplayTooltip = 0.5f;

	[SerializeField]
	private bool _useCustomPivot;

	[SerializeField]
	private Vector2 _cusomPivot;

	[SerializeField]
	private bool _useStaticPosition;

	[SerializeField]
	private Transform _staticPosition;

	[SerializeField]
	private bool _updateStaticPosition;

	[SerializeField]
	private bool _useOffset;

	[SerializeField]
	private Vector2 _offset;

	private TooltipContent _content;

	private float _mouseOverTime;

	private bool _tooltipShouldBeDisplayed;

	private bool _tooltipHasFilteredTag;

	private Vector3 _tooltipOffset;

	private bool _isMouseOver;

	private bool _isGamepadOver;

	private bool _hasRectTransform;

	private RaycastCatcher _raycastCatcher;

	private static float _lastTimeFromMouseDown;

	[SerializeField]
	private float _tooltipShowTime = 3f;

	private RaycastCatcher RaycastCatcher
	{
		get
		{
			if (_raycastCatcher == null)
			{
				_raycastCatcher = Singleton<GameManager>.Instance.RaycastCatcher;
			}
			return _raycastCatcher;
		}
	}

	private void Awake()
	{
		_hasRectTransform = gameObject.transform is RectTransform;
		_content = GetComponent<TooltipContent>();
		if (_tagData != null)
		{
			_tagData.Register(FilteredTagsListUpdated);
			FilteredTagsListUpdated();
		}
	}

	private void OnDestroy()
	{
		if (_tagData != null)
		{
			_tagData.Unregister(FilteredTagsListUpdated);
		}
	}

	public void SetTooltipRect(RectTransform tooltip)
	{
		_tooltipRectTransform = tooltip;
	}

	private void Update()
	{
		MouseOverUI();
		if (!(_tooltipRectTransform != null) || !_tooltipShouldBeDisplayed || _tooltipHasFilteredTag || !_content.IsValid())
		{
			return;
		}
		if (_tooltipFlow == TooltipFlow.ON_CLICK)
		{
			if (Time.time - _lastTimeFromMouseDown > _tooltipShowTime)
			{
				_tooltipRectTransform.gameObject.SetActive(value: false);
				_tooltipShouldBeDisplayed = false;
			}
		}
		else if (_tooltipFlow == TooltipFlow.HOVER)
		{
			_tooltipRectTransform.gameObject.SetActive(value: true);
			SetTooltipPosition();
		}
		else if (_tooltipFlow == TooltipFlow.HOVER_KEEP_POSITION)
		{
			_tooltipRectTransform.gameObject.SetActive(value: true);
			if (_updateStaticPosition)
			{
				SetTooltipPosition();
			}
		}
	}

	private void SetTooltipPosition()
	{
		if (!(_tooltipRectTransform != null))
		{
			return;
		}
		Vector3 mousePosition = Singleton<VirtualInputManager>.Instance.GetMousePosition();
		if (!_useCustomPivot)
		{
			int num;
			int num2;
			if (!_gamepadTooltipPivotOverride.Use || Singleton<VirtualInputManager>.Instance.IsPointerMode())
			{
				num = ((!(mousePosition.x < (float)Screen.width / 2f)) ? 1 : 0);
				num2 = ((!(mousePosition.y < (float)Screen.height / 2f)) ? 1 : 0);
			}
			else
			{
				num = _gamepadTooltipPivotOverride.PivotX;
				num2 = _gamepadTooltipPivotOverride.PivotY;
			}
			if (num2 == 1 && num == 0)
			{
				_tooltipOffset = Vector3.zero;
				_tooltipOffset = new Vector3(_tooltipOffset.x + Singleton<VirtualInputManager>.Instance.VisualManager.SizeOnScreen.x, _tooltipOffset.y, _tooltipOffset.z);
			}
			else
			{
				_tooltipOffset = new Vector3((num == 0) ? 1 : (-1), (num2 == 0) ? 1 : (-1), 0f) * 5f;
			}
			_tooltipRectTransform.pivot = new Vector2(num, num2);
			HorizontalOrVerticalLayoutGroup component = _tooltipRectTransform.GetComponent<HorizontalOrVerticalLayoutGroup>();
			ContentSizeFitter component2 = _tooltipRectTransform.GetComponent<ContentSizeFitter>();
			if (component != null && component2 == null)
			{
				if (num == 0 && num2 == 0)
				{
					component.childAlignment = TextAnchor.LowerLeft;
				}
				else if (num == 0 && num2 == 1)
				{
					component.childAlignment = TextAnchor.UpperLeft;
				}
				else if (num == 1 && num2 == 0)
				{
					component.childAlignment = TextAnchor.LowerRight;
				}
				else if (num == 1 && num2 == 1)
				{
					component.childAlignment = TextAnchor.UpperRight;
				}
			}
		}
		else
		{
			_tooltipRectTransform.pivot = _cusomPivot;
		}
		if (_useOffset)
		{
			_tooltipOffset = _offset;
		}
		if (_useStaticPosition)
		{
			_tooltipRectTransform.position = _staticPosition.position;
		}
		else
		{
			_tooltipRectTransform.position = mousePosition + _tooltipOffset;
		}
	}

	private void SendTooltipContentToContentHandler()
	{
		if (_tooltipRectTransform != null)
		{
			TooltipContentHandler component = _tooltipRectTransform.GetComponent<TooltipContentHandler>();
			if (_content != null && component != null)
			{
				component.HandleContent(_content);
			}
		}
	}

	public void FilteredTagsListUpdated()
	{
		_tooltipHasFilteredTag = false;
		for (int i = 0; i < _tooltipTags.Count; i++)
		{
			if (_tagData != null && _tagData.CurrentFilteredTags.Contains(_tooltipTags[i]))
			{
				_tooltipHasFilteredTag = true;
			}
		}
	}

	private void OnDisable()
	{
		_isMouseOver = false;
		_isGamepadOver = false;
		MouseExit();
	}

	private void MouseEnter()
	{
		_mouseOverTime = Time.time;
	}

	private void OnMouseEnter()
	{
		if (!Singleton<VirtualInputManager>.Instance.IsPointerOverGameObject())
		{
			MouseEnter();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_isMouseOver = true;
		MouseEnter();
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		if (_tooltipFlow == TooltipFlow.ON_CLICK && !Singleton<VirtualInputManager>.Instance.IsPointerOverGameObject())
		{
			ShowOnClickTooltip();
		}
	}

	public void GamepadEnter()
	{
		_isMouseOver = true;
		_isGamepadOver = true;
		MouseEnter();
	}

	public void GamepadExit()
	{
		_isMouseOver = false;
		_isGamepadOver = false;
		MouseExit();
	}

	private void MouseOver()
	{
		if ((_tooltipFlow == TooltipFlow.HOVER || _tooltipFlow == TooltipFlow.HOVER_KEEP_POSITION) && Time.time > _mouseOverTime + _timeToDisplayTooltip)
		{
			if (!_tooltipShouldBeDisplayed)
			{
				SetTooltipPosition();
			}
			_tooltipShouldBeDisplayed = true;
			SendTooltipContentToContentHandler();
		}
	}

	private void ShowOnClickTooltip()
	{
		_lastTimeFromMouseDown = Time.time;
		_tooltipShouldBeDisplayed = true;
		SetTooltipPosition();
		_tooltipRectTransform.gameObject.SetActive(value: true);
		SendTooltipContentToContentHandler();
	}

	private void OnMouseUpAsButton()
	{
		if (_tooltipFlow == TooltipFlow.ON_CLICK && !Singleton<VirtualInputManager>.Instance.IsPointerOverGameObject())
		{
			ShowOnClickTooltip();
		}
	}

	private void OnMouseDown()
	{
	}

	private void OnMouseUp()
	{
	}

	private void OnMouseOver()
	{
		if (Singleton<VirtualInputManager>.Instance.IsPointerOverGameObject())
		{
			return;
		}
		if (RaycastCatcher.IsBlockingRaycasts || EndGameRaycastCatcher.Visible)
		{
			if (_tooltipShouldBeDisplayed)
			{
				MouseExit();
			}
		}
		else if (Singleton<VirtualInputManager>.Instance.IsSelectablesMode())
		{
			if (_tooltipShouldBeDisplayed)
			{
				MouseExit();
			}
		}
		else if (Singleton<VirtualInputManager>.Instance.IsPointerMode())
		{
			if (Singleton<VirtualInputManager>.Instance.OverMe(gameObject))
			{
				MouseOver();
			}
			else if (_tooltipShouldBeDisplayed)
			{
				MouseExit();
			}
		}
		else
		{
			MouseOver();
		}
	}

	private void MouseOverUI()
	{
		if (_isMouseOver && (_hasRectTransform || _isGamepadOver))
		{
			MouseOver();
		}
	}

	public void MouseExit()
	{
		if (_tooltipFlow == TooltipFlow.HOVER || _tooltipFlow == TooltipFlow.HOVER_KEEP_POSITION)
		{
			_tooltipShouldBeDisplayed = false;
			if (_tooltipRectTransform != null)
			{
				_tooltipRectTransform.gameObject.SetActive(value: false);
			}
		}
	}

	private void OnMouseExit()
	{
		MouseExit();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_isMouseOver = false;
		_isGamepadOver = false;
		MouseExit();
	}
}
