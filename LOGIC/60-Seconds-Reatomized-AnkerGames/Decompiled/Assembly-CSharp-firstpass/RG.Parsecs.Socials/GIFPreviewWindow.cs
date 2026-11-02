using System.Collections.Generic;
using Moments.Encoder;
using RG.VirtualInput;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Socials;

public class GIFPreviewWindow : MonoBehaviour, IRefreshInteractable
{
	public GameObject TwitterTextMenu;

	public GameObject TwitterPinMenu;

	public Canvas Canvas;

	public Slider Progress;

	public RawImage RawImage;

	public Slider FacebookSlider;

	public Slider TwitterSlider;

	public Slider SaveSlider;

	public Button FacebookButton;

	public Button TwitterButton;

	public Button SaveButton;

	public Button ExitButton;

	public Button TwitterPinMenuOk;

	public Button TwitterPinMenuExit;

	public Button TwitterTextMenuOk;

	public Button TwitterTextMenuExit;

	public VirtualInputClosablePanel GamepadPanelCloseable;

	[SerializeField]
	private bool _setPreviewHeight = true;

	[SerializeField]
	private bool _setPreviewWidth;

	private List<GifFrame> _frames;

	private bool _showGif;

	private float _frameCounter = 0.5f;

	private float _frameTime = 0.5f;

	private int _frameIndex;

	private float _progress;

	public TMP_InputField TwitterPin;

	public TMP_InputField TweetText;

	private bool _facebookButtonInteractable;

	private bool _twitterButtonInteractable;

	private bool _saveButtonInteractable;

	public bool FacebookButtonInteractable
	{
		get
		{
			return _facebookButtonInteractable;
		}
		set
		{
			if (Canvas != null && Canvas.enabled && FacebookButton != null)
			{
				FacebookButton.interactable = value;
			}
			_facebookButtonInteractable = value;
		}
	}

	public bool TwitterButtonInteractable
	{
		get
		{
			return _twitterButtonInteractable;
		}
		set
		{
			if (Canvas != null && Canvas.enabled && TwitterButton != null)
			{
				TwitterButton.interactable = value;
			}
			_twitterButtonInteractable = value;
		}
	}

	public bool SaveButtonInteractable
	{
		get
		{
			return _saveButtonInteractable;
		}
		set
		{
			if (Canvas != null && Canvas.enabled && SaveButton != null)
			{
				SaveButton.interactable = value;
			}
			_saveButtonInteractable = value;
		}
	}

	public void SetFrames(List<GifFrame> frames)
	{
		if (frames != null && frames.Count > 0 && frames[0].Texture != null)
		{
			_frames = frames;
			int width = _frames[0].Texture.width;
			int height = _frames[0].Texture.height;
			if (_setPreviewHeight)
			{
				RawImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
			}
			if (_setPreviewWidth)
			{
				RawImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
			}
			SetFrame(0);
		}
		Progress.gameObject.SetActive(value: false);
	}

	public void Show()
	{
		Canvas.enabled = true;
		_showGif = true;
		if (GamepadPanelCloseable != null)
		{
			GamepadPanelCloseable.Show();
		}
	}

	public void Hide()
	{
		Canvas.enabled = false;
		_showGif = false;
		_frameIndex = 0;
		SetFrame(0);
		if (GamepadPanelCloseable != null)
		{
			GamepadPanelCloseable.Hide();
		}
	}

	public void SetProgress(float p)
	{
		_progress = p;
	}

	private void SetFrame(int index)
	{
		if (_frames != null)
		{
			Texture2D texture = _frames[index].Texture;
			if (texture != null)
			{
				RawImage.color = Color.white;
			}
			else
			{
				RawImage.color = Color.black;
			}
			RawImage.texture = texture;
		}
	}

	public void Start()
	{
		if (SteamManager.IsRunningOnSteamDeck())
		{
			FacebookButton.gameObject.SetActive(value: false);
			TwitterButton.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (_showGif)
		{
			if (_frames == null || _frames.Count == 0)
			{
				return;
			}
			_frameCounter -= Time.deltaTime;
			if (!(_frameCounter > 0f))
			{
				_frameCounter = _frameTime;
				_frameIndex++;
				if (_frameIndex >= _frames.Count)
				{
					_frameIndex = 0;
				}
				SetFrame(_frameIndex);
			}
		}
		else
		{
			Progress.value = _progress;
		}
	}

	public void ShowTwitterTextMenu()
	{
		TwitterTextMenu.gameObject.SetActive(value: true);
	}

	public void ShowTwitterPinMenu()
	{
		TwitterPinMenu.gameObject.SetActive(value: true);
	}

	public void HideTwitterTextMenu()
	{
		TwitterTextMenu.gameObject.SetActive(value: false);
	}

	public void HideTwitterPinMenu()
	{
		TwitterPinMenu.gameObject.SetActive(value: false);
	}

	public void RefreshInteractable()
	{
		FacebookButton.interactable = FacebookButtonInteractable;
		TwitterButton.interactable = TwitterButtonInteractable;
		SaveButton.interactable = SaveButtonInteractable;
		ExitButton.interactable = true;
		TwitterPinMenuOk.interactable = true;
		TwitterPinMenuExit.interactable = true;
		TwitterTextMenuOk.interactable = true;
		TwitterTextMenuExit.interactable = true;
	}
}
