using UnityEngine;
using UnityEngine.UI;

namespace RG.VirtualInput;

public class VirtualMouseVisualManager : MonoBehaviour
{
	[SerializeField]
	private bool _isMouseCursorVisible;

	[SerializeField]
	private Image _handCursorImage;

	[SerializeField]
	private RectTransform _handRectTransform;

	private Vector2 _sizeOnScreen = Vector2.zero;

	public bool MouseCursorVisible
	{
		get
		{
			return _isMouseCursorVisible;
		}
		set
		{
			_isMouseCursorVisible = value;
			_handCursorImage.enabled = value;
			if (value)
			{
				Cursor.lockState = CursorLockMode.None;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
	}

	public Vector2 SizeOnScreen
	{
		get
		{
			if (_sizeOnScreen == Vector2.zero)
			{
				Vector3[] array = new Vector3[4];
				_handRectTransform.GetWorldCorners(array);
				_sizeOnScreen = new Vector2(Vector3.Distance(array[1], array[2]), Vector3.Distance(array[0], array[1]));
			}
			return _sizeOnScreen;
		}
	}
}
