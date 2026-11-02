using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class GlowController : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[SerializeField]
	private bool _enabled;

	[SerializeField]
	private Image _image;

	private void Awake()
	{
		SetEnabled(_enabled);
	}

	public void SetEnabled(bool enable)
	{
		_enabled = enable;
		_image.enabled = enable;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!_enabled)
		{
			_image.enabled = true;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!_enabled)
		{
			_image.enabled = false;
		}
	}
}
