using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PatternButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	public static PatternButton CurrentButton;

	[SerializeField]
	private Image patternImage;

	[SerializeField]
	private GameObject selectedImage;

	[HideInInspector]
	public SO_Pattern pattern;

	private void OnDisable()
	{
		if (CurrentButton != this)
		{
			SetSelectedState(state: false);
		}
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		OnClick();
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		if (CurrentButton != this)
		{
			SetSelectedState(state: false);
		}
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		SetSelectedState(state: true);
	}

	public void OnClick()
	{
		if (CurrentButton != null && CurrentButton != this)
		{
			CurrentButton.SetSelectedState(state: false);
		}
		CurrentButton = this;
		CurrentButton.SetSelectedState(state: true);
	}

	public void AttachPattern(SO_Pattern pattern)
	{
		this.pattern = pattern;
		patternImage.sprite = pattern.buttonSprite;
	}

	public void SetSelectedState(bool state)
	{
		selectedImage.SetActiveSafe(state);
	}
}
