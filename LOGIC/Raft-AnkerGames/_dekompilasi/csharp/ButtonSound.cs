using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerDownHandler, ISelectHandler, ISubmitHandler
{
	public static bool Mute;

	[SerializeField]
	private bool forceHighlightSound;

	[SerializeField]
	private bool skipClickSound;

	public EventReference er_uniqueClickSound;

	private static SoundManager soundmanager;

	private Button button;

	private void Start()
	{
		if (soundmanager == null)
		{
			soundmanager = ComponentManager<SoundManager>.Value;
		}
		button = GetComponent<Button>();
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		if (forceHighlightSound || (button != null && button.interactable))
		{
			PlayHighlightSound();
		}
	}

	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
		if (!skipClickSound)
		{
			if (button == null)
			{
				PlayClickSound();
			}
			else if (!button.interactable)
			{
				PlayClickFailSound();
			}
			else
			{
				PlayClickSound();
			}
		}
	}

	public void PlayClickSound()
	{
		if (!(soundmanager == null) && !Mute)
		{
			if (er_uniqueClickSound.IsNull)
			{
				soundmanager.PlayUI_Click();
			}
			else
			{
				RuntimeManager.PlayOneShot(er_uniqueClickSound);
			}
		}
	}

	public void PlayClickFailSound()
	{
		if (!Mute)
		{
			soundmanager?.PlayUI_Click_Fail();
		}
	}

	public void PlayHighlightSound()
	{
		if (!Mute)
		{
			soundmanager?.PlayUI_Highlight();
		}
	}

	private bool HasGamepadCursor()
	{
		CanvasHelper value = ComponentManager<CanvasHelper>.Value;
		if (value != null)
		{
			if (value.GetComponent<GamepadCursor>().enabled)
			{
				return value.GetComponent<GamepadCursor>().IsCursorEnabled();
			}
			return false;
		}
		return false;
	}

	public void OnSelect(BaseEventData eventData)
	{
		if (!HasGamepadCursor() && (forceHighlightSound || (button != null && button.interactable)))
		{
			PlayHighlightSound();
		}
	}

	public void OnSubmit(BaseEventData eventData)
	{
		if (!HasGamepadCursor() && !skipClickSound)
		{
			if (button == null)
			{
				PlayClickSound();
			}
			else if (!button.interactable)
			{
				PlayClickFailSound();
			}
			else
			{
				PlayClickSound();
			}
		}
	}
}
