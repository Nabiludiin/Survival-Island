using System.Collections.Generic;
using RG.Parsecs.EventEditor;
using TMPro;
using UnityEngine;

namespace RG.SecondsRemaster.Menu;

public class ResolutionSettingController : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _valueField;

	[SerializeField]
	private GlobalIntVariable _widthVariable;

	[SerializeField]
	private GlobalIntVariable _heightVariable;

	[SerializeField]
	private GlobalBoolVariable _isFullScreen;

	[SerializeField]
	private bool _applyInstantly = true;

	[SerializeField]
	private int _minWidth;

	[SerializeField]
	private int _minHeight;

	private List<Resolution> _resolutions;

	private int _currentIndex;

	private const int RESOLUTION_NOT_FOUND = -1;

	private void Awake()
	{
		_resolutions = new List<Resolution>(Screen.resolutions);
	}

	private void OnEnable()
	{
		_currentIndex = GetCurrentResolutionIndex(_widthVariable.Value, _heightVariable.Value);
		if (_currentIndex == -1)
		{
			_currentIndex = Screen.resolutions.Length - 1;
		}
		SetCurrentResolutionValues();
	}

	private int GetCurrentResolutionIndex(int width, int height)
	{
		Resolution[] resolutions = Screen.resolutions;
		int result = -1;
		for (int num = resolutions.Length - 1; num >= 0; num--)
		{
			if (resolutions[num].width == width && resolutions[num].height == height)
			{
				result = num;
				break;
			}
		}
		return result;
	}

	public void SetNext()
	{
		bool flag = false;
		if (_currentIndex + 1 < _resolutions.Count)
		{
			_currentIndex++;
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		Resolution resolution = _resolutions[_currentIndex];
		if (resolution.width == _widthVariable.Value && resolution.height == _heightVariable.Value)
		{
			SetNext();
			return;
		}
		SetCurrentResolutionValues();
		if (_applyInstantly)
		{
			int num = ((resolution.height < _minHeight) ? _minHeight : resolution.height);
			int num2 = ((resolution.width < _minWidth) ? _minWidth : resolution.width);
			_widthVariable.Value = num2;
			_heightVariable.Value = num;
			Screen.SetResolution(num2, num, _isFullScreen.Value, resolution.refreshRate);
		}
	}

	public void SetPrevious()
	{
		bool flag = false;
		if (_currentIndex - 1 >= 0)
		{
			_currentIndex--;
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		Resolution resolution = _resolutions[_currentIndex];
		if (resolution.width == _widthVariable.Value && resolution.height == _heightVariable.Value)
		{
			SetPrevious();
			return;
		}
		SetCurrentResolutionValues();
		if (_applyInstantly)
		{
			int num = ((resolution.height < _minHeight) ? _minHeight : resolution.height);
			int num2 = ((resolution.width < _minWidth) ? _minWidth : resolution.width);
			_widthVariable.Value = num2;
			_heightVariable.Value = num;
			Screen.SetResolution(num2, num, _isFullScreen.Value, resolution.refreshRate);
		}
	}

	private void SetCurrentResolutionValues()
	{
		_valueField.text = $"{_resolutions[_currentIndex].width} x {_resolutions[_currentIndex].height}";
		int value = ((_resolutions[_currentIndex].height < _minHeight) ? _minHeight : _resolutions[_currentIndex].height);
		int value2 = ((_resolutions[_currentIndex].width < _minWidth) ? _minWidth : _resolutions[_currentIndex].width);
		_widthVariable.Value = value2;
		_heightVariable.Value = value;
	}
}
