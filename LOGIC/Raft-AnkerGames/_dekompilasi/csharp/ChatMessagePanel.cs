using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessagePanel : MonoBehaviour
{
	public TextMeshProUGUI chatText;

	public Text usernameText;

	public Image uiImage;

	private RectTransform chatRect;

	private RectTransform usernameRect;

	private RectTransform imageRect;

	private RectTransform rect;

	private void Awake()
	{
		rect = GetComponent<RectTransform>();
		imageRect = uiImage.GetComponent<RectTransform>();
		usernameRect = usernameText.GetComponent<RectTransform>();
		chatRect = chatText.GetComponent<RectTransform>();
	}

	public void SetText(string text)
	{
		chatText.text = text;
		Invoke("SetHeight", 0.1f);
	}

	private void SetHeight()
	{
		chatRect.sizeDelta = new Vector2(rect.sizeDelta.x - imageRect.sizeDelta.x - usernameRect.sizeDelta.x - 20f, chatRect.sizeDelta.y);
		rect.sizeDelta = new Vector2(rect.sizeDelta.x, Mathf.Clamp(chatText.renderedHeight + 10f, imageRect.sizeDelta.y, float.MaxValue));
	}
}
