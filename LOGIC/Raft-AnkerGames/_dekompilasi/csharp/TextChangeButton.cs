using UnityEngine;
using UnityEngine.UI;

public class TextChangeButton : MonoBehaviour
{
	private Button button;

	private Text text;

	public Color normalColor = new Color(0.09f, 0.05f, 0.02f);

	public Color selectedColor = new Color(0.05f, 0.08f, 0.01f);

	public Color disabled = new Color(0.08f, 0.01f, 0f);

	private void Start()
	{
		button = GetComponent<Button>();
		text = GetComponentInChildren<Text>();
	}

	private void Update()
	{
		if (!button.interactable)
		{
			text.color = disabled;
		}
		else
		{
			text.color = normalColor;
		}
	}
}
