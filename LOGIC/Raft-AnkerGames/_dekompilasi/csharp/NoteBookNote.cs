using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoteBookNote : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public ChunkPointType landmarkType;

	public int noteIndex;

	public bool isUnlocked;

	public bool isFrequencyThumbnail;

	public ChunkPointType thumbNailLandmarkType;

	[SerializeField]
	private SO_VoiceActor voiceActor;

	[SerializeField]
	private SO_VoiceData voiceData;

	public Image playButtonImage;

	[SerializeField]
	private Sprite playSprite;

	[SerializeField]
	private Sprite stopSprite;

	private static NoteBook notebook;

	private bool pointerIsHovering;

	public bool IsUsingPlayVoiceSystem
	{
		get
		{
			if (playButtonImage != null)
			{
				return voiceData != null;
			}
			return false;
		}
	}

	public void FixName()
	{
		string[] array = transform.name.Split('_');
		string text = string.Empty;
		if (array.Length >= 2)
		{
			text = array[^1];
		}
		transform.name = "NoteBookNote_Index" + noteIndex + "_" + landmarkType.ToString() + "_" + text;
	}

	private void Awake()
	{
		if (notebook == null)
		{
			notebook = ComponentManager<NoteBook>.Value;
		}
		if (IsUsingPlayVoiceSystem)
		{
			Dialogue dialogue = notebook.dialogue;
			dialogue.OnVoiceDataCancelled = (Action<SO_VoiceData.Instance_VoiceData>)Delegate.Combine(dialogue.OnVoiceDataCancelled, new Action<SO_VoiceData.Instance_VoiceData>(OnVoiceDataCancelled));
		}
		SetPlayButtonState(state: false);
	}

	private void OnDestroy()
	{
		if (notebook != null && notebook.dialogue != null && IsUsingPlayVoiceSystem)
		{
			Dialogue dialogue = notebook.dialogue;
			dialogue.OnVoiceDataCancelled = (Action<SO_VoiceData.Instance_VoiceData>)Delegate.Remove(dialogue.OnVoiceDataCancelled, new Action<SO_VoiceData.Instance_VoiceData>(OnVoiceDataCancelled));
		}
	}

	public void Button_PlayAndStop()
	{
		if (notebook.IsPlayingVoiceData(voiceData))
		{
			notebook.StopDialogue();
			SetPlayAndStopSpriteState(showPlaySprite: true);
		}
		else if (!Dialogue.IsPlaying3DDialogue())
		{
			notebook.PlayDialogue(voiceActor, voiceData);
			SetPlayAndStopSpriteState(showPlaySprite: false);
		}
	}

	private void OnVoiceDataCancelled(SO_VoiceData.Instance_VoiceData instance)
	{
		if (instance != null && instance.voiceData == voiceData)
		{
			if (!pointerIsHovering)
			{
				playButtonImage.gameObject.SetActiveSafe(value: false);
			}
			SetPlayAndStopSpriteState(showPlaySprite: true);
		}
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		pointerIsHovering = true;
		SetPlayButtonState(state: true);
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		pointerIsHovering = false;
		if (!notebook.IsPlayingVoiceData(voiceData))
		{
			SetPlayButtonState(state: false);
		}
	}

	public void SetPlayButtonState(bool state)
	{
		if (IsUsingPlayVoiceSystem)
		{
			bool flag = notebook.IsPlayingVoiceData(voiceData);
			if (!flag)
			{
				playButtonImage.gameObject.SetActiveSafe(state);
			}
			SetPlayAndStopSpriteState(!flag);
		}
	}

	public void ToggleNoteGameObject(bool value)
	{
		gameObject.SetActiveSafe(value);
	}

	private void SetPlayAndStopSpriteState(bool showPlaySprite)
	{
		playButtonImage.sprite = ((!showPlaySprite) ? stopSprite : playSprite);
	}
}
