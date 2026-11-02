using System;
using UnityEngine;
using UnityEngine.UI;

public class PianoUI : SimpleMonoBehaviourSingleton<PianoUI>
{
	[Serializable]
	private class Note
	{
		public Sprite sprite;

		public int index;
	}

	[SerializeField]
	private RectTransform ApressedTransform;

	[SerializeField]
	private RectTransform LTpressedTransform;

	[SerializeField]
	private RectTransform RTpressedTransform;

	[SerializeField]
	private Image image;

	[SerializeField]
	private Note[] notes;

	[SerializeField]
	private Sprite defaultSprite;

	[SerializeField]
	private Canvas canvas;

	public void SetNote(int index)
	{
		Note[] array = notes;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].index == index)
			{
				image.sprite = notes[index].sprite;
			}
		}
	}

	private void Update()
	{
		canvas.enabled = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad;
	}

	public void ResetNote()
	{
		image.sprite = defaultSprite;
	}

	public void SetAIsPressed(bool isPressed)
	{
		ApressedTransform.localScale = (isPressed ? new Vector3(0.75f, 0.75f) : new Vector3(1f, 1f));
	}

	public void SetLTIsPressed(bool isPressed)
	{
		LTpressedTransform.localScale = (isPressed ? new Vector3(0.75f, 0.75f) : new Vector3(1f, 1f));
	}

	public void SetRTIsPressed(bool isPressed)
	{
		RTpressedTransform.localScale = (isPressed ? new Vector3(0.75f, 0.75f) : new Vector3(1f, 1f));
	}
}
