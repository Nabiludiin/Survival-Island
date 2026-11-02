using UnityEngine;
using UnityEngine.UI;

public class VoiceOverlay : MonoBehaviour
{
	public Text[] Texts;

	private int[] lastFrame = new int[4];

	public void PushVoiceOverlay(int index, string text)
	{
		if (index <= lastFrame.Length - 1)
		{
			Texts[index].text = text;
			lastFrame[index] = Time.frameCount;
		}
	}

	public void Update()
	{
		if (Time.frameCount <= 60)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			if (Time.frameCount - 60 < lastFrame[i])
			{
				Texts[i].transform.parent.gameObject.SetActive(value: true);
			}
			else
			{
				Texts[i].transform.parent.gameObject.SetActive(value: false);
			}
		}
	}
}
