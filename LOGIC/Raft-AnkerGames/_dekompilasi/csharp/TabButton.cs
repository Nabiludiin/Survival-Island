using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
	public int tabIndex;

	[Header("Button settings")]
	public Button button;

	[SerializeField]
	private Color buttonHighlightColor = Color.white;

	[SerializeField]
	private Color buttonNormalColor = Color.white;

	[SerializeField]
	private Image highlight;

	[SerializeField]
	private Image[] extraImages;

	[SerializeField]
	private bool dimNonInteractive;

	[Header("Text settings")]
	[SerializeField]
	public Text text;

	[SerializeField]
	private Color textHighlightColor = Color.white;

	[SerializeField]
	private Color textNormalColor = Color.white;

	[Header("Tab to enable")]
	[SerializeField]
	protected GameObject tab;

	[SerializeField]
	protected GameObject tabConsole;

	protected TabGroup tabGroup;

	private bool isInteractable = true;

	public GameObject Tab => tab;

	public bool IsInteractable => isInteractable;

	private void Start()
	{
		tabGroup = GetComponentInParent<TabGroup>();
		EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener((BaseEventData eventData) =>
		{
			OnPointerEnter();
		});
		eventTrigger.triggers.Add(entry);
		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerExit;
		entry.callback.AddListener((BaseEventData eventData) =>
		{
			OnPointerExit();
		});
		eventTrigger.triggers.Add(entry);
		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener((BaseEventData eventData) =>
		{
			OnPointerClick();
		});
		eventTrigger.triggers.Add(entry);
	}

	public virtual void OnPointerClick()
	{
		if (tabGroup == null)
		{
			tabGroup = GetComponentInParent<TabGroup>();
		}
		if (IsInteractable)
		{
			if (tabGroup.SelectedTabButton == null)
			{
				tabGroup.SelectedTabButton = this;
			}
			else if (tabGroup.SelectedTabButton != this)
			{
				tabGroup.SelectedTabButton.OnPointerExit(forceExit: true);
				tabGroup.SelectedTabButton = this;
			}
		}
	}

	public virtual void OnPointerEnter()
	{
		if (tabGroup == null)
		{
			tabGroup = GetComponentInParent<TabGroup>();
		}
		if (button.interactable)
		{
			Color color = buttonHighlightColor;
			Color color2 = textHighlightColor;
			if (!isInteractable && dimNonInteractive)
			{
				color.a = 0.5f;
				color2.a = 0.5f;
			}
			button.targetGraphic.color = color;
			if (text != null)
			{
				text.color = color2;
			}
			SetColorToImages(extraImages, color2);
			if (highlight != null)
			{
				highlight.gameObject.SetActiveSafe(value: true);
			}
			tabGroup.hightlightedTabButton = this;
		}
	}

	public virtual void OnChange()
	{
		if (tabGroup.hightlightedTabButton != null && tabGroup.hightlightedTabButton != this && tabGroup.hightlightedTabButton != tabGroup.SelectedTabButton)
		{
			tabGroup.hightlightedTabButton.OnPointerExit(forceExit: true);
		}
		OnPointerEnter();
	}

	public virtual void OnPointerExit(bool forceExit = false)
	{
		if (forceExit || tabGroup.SelectedTabButton != this)
		{
			Color color = buttonNormalColor;
			Color color2 = textNormalColor;
			if (!isInteractable && dimNonInteractive)
			{
				color.a = 0.5f;
				color2.a = 0.5f;
			}
			button.targetGraphic.color = color;
			if (text != null)
			{
				text.color = color2;
			}
			SetColorToImages(extraImages, color2);
			if (highlight != null)
			{
				highlight.gameObject.SetActiveSafe(value: false);
			}
			if (tabGroup != null)
			{
				tabGroup.hightlightedTabButton = null;
			}
		}
	}

	public virtual void SetInteractability(bool interactable)
	{
		isInteractable = interactable;
	}

	private void SetColorToImages(Image[] images, Color color)
	{
		if (images == null || images.Length == 0)
		{
			return;
		}
		foreach (Image image in images)
		{
			if (!(image == null))
			{
				image.color = color;
			}
		}
	}
}
