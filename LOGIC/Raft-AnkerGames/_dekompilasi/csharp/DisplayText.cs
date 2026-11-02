using UnityEngine;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour
{
	[SerializeField]
	private Text textComponent;

	[SerializeField]
	private Text buttonText;

	[SerializeField]
	private Image buttonBackground;

	[SerializeField]
	private Vector2 buttonBackgroundMargin = Vector2.zero;

	[SerializeField]
	private Image controllerImage;

	[SerializeField]
	private Image controllerImage1;

	[SerializeField]
	private Image controllerImage2;

	[SerializeField]
	private SO_ControllerActions xboxControllerActions;

	[SerializeField]
	private SO_ControllerActions ps5ControllerActions;

	private RectTransform buttonRect;

	private RectTransform buttonTextRect;

	private int currentPriority;

	private int defaultPriority = -1;

	public bool hide;

	public void Show(string text, int priority = 0)
	{
		gameObject.SetActiveSafe(value: true);
		hide = false;
		if (priority >= currentPriority)
		{
			currentPriority = priority;
			buttonBackground.gameObject.SetActiveSafe(value: false);
			buttonText.gameObject.SetActiveSafe(value: false);
			controllerImage.gameObject.SetActiveSafe(value: false);
			controllerImage1.transform.parent.gameObject.SetActive(value: false);
			if (textComponent != null && textComponent.text != text)
			{
				textComponent.text = text;
			}
		}
	}

	public void Show(string text, KeyCode key, int priority = 0)
	{
		gameObject.SetActiveSafe(value: true);
		hide = false;
		if (priority >= currentPriority)
		{
			currentPriority = priority;
			buttonBackground.gameObject.SetActiveSafe(value: true);
			buttonText.gameObject.SetActiveSafe(value: true);
			controllerImage.gameObject.SetActiveSafe(value: false);
			controllerImage1.transform.parent.gameObject.SetActive(value: false);
			if (buttonText.text != key.ToShortString())
			{
				buttonText.text = key.ToShortString();
			}
			if (textComponent != null && textComponent.text != text)
			{
				textComponent.text = text;
			}
			if (buttonRect == null || buttonTextRect == null)
			{
				FindRectTranforms();
			}
			Vector2 vector = buttonTextRect.sizeDelta + buttonBackgroundMargin;
			if (buttonRect.sizeDelta != vector)
			{
				buttonRect.sizeDelta = vector;
			}
		}
	}

	public void ShowConsoleBinding(string text, string key, int priority = 0)
	{
		gameObject.SetActiveSafe(value: true);
		hide = false;
		if (priority >= currentPriority)
		{
			currentPriority = priority;
			if (textComponent != null && textComponent.text != text)
			{
				textComponent.text = text;
				LayoutRebuilder.ForceRebuildLayoutImmediate(textComponent.rectTransform);
			}
			buttonBackground.gameObject.SetActiveSafe(value: false);
			buttonText.gameObject.SetActiveSafe(value: false);
			controllerImage1.transform.parent.gameObject.SetActiveSafe(value: false);
			controllerImage.gameObject.SetActiveSafe(value: true);
			if (xboxControllerActions.GetSprite(key, out var sprite))
			{
				controllerImage.sprite = sprite;
			}
		}
	}

	public void ShowConsoleBinding2(string text, string action1, string action2, int priority = 0)
	{
		gameObject.SetActiveSafe(value: true);
		hide = false;
		if (priority >= currentPriority)
		{
			currentPriority = priority;
			if (textComponent != null && textComponent.text != text)
			{
				textComponent.text = text;
				LayoutRebuilder.ForceRebuildLayoutImmediate(textComponent.rectTransform);
			}
			buttonBackground.gameObject.SetActiveSafe(value: false);
			buttonText.gameObject.SetActiveSafe(value: false);
			controllerImage.gameObject.SetActiveSafe(value: false);
			controllerImage1.transform.parent.gameObject.SetActive(value: true);
			if (xboxControllerActions.GetSprite(action1, out var sprite))
			{
				controllerImage1.sprite = sprite;
			}
			if (xboxControllerActions.GetSprite(action2, out var sprite2))
			{
				controllerImage2.sprite = sprite2;
			}
		}
	}

	public void Hide()
	{
		hide = true;
		currentPriority = defaultPriority;
	}

	public void HardHide()
	{
		if (gameObject != null)
		{
			gameObject.SetActiveSafe(value: false);
		}
	}

	private void FindRectTranforms()
	{
		buttonRect = buttonBackground.GetComponent<RectTransform>();
		buttonTextRect = buttonText.GetComponent<RectTransform>();
	}
}
