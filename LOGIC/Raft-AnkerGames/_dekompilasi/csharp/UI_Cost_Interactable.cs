using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Cost_Interactable : UI_Cost, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public Action<UI_Cost_Interactable> OnPointerEnter;

	public Action<UI_Cost_Interactable> OnPointerExit;

	public Action<UI_Cost_Interactable> OnPointerClick;

	[SerializeField]
	protected GameObject hoverImage;

	private static UI_Cost_Interactable currentActive;

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		if (isValidForInteract && !(this is UI_Cost_Interactable_FishingBait))
		{
			SetHoverImageActive();
			OnPointerEnter?.Invoke(this);
		}
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		if (isValidForInteract && !(this is UI_Cost_Interactable_FishingBait))
		{
			SetHoverImageInactive();
			OnPointerExit?.Invoke(this);
		}
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		if (isValidForInteract && !(this is UI_Cost_Interactable_FishingBait))
		{
			OnPointerClick?.Invoke(this);
		}
	}

	protected virtual void SetHoverImageActive()
	{
		if (hoverImage != null)
		{
			if ((bool)currentActive)
			{
				currentActive.SetHoverImageInactive();
			}
			hoverImage.SetActiveSafe(value: true);
			currentActive = this;
		}
	}

	protected virtual void SetHoverImageInactive()
	{
		if (hoverImage != null)
		{
			hoverImage.SetActiveSafe(value: false);
		}
	}
}
