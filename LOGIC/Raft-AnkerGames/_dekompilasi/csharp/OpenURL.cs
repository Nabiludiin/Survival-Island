using UnityEngine;

public class OpenURL : MonoBehaviour
{
	public string url = string.Empty;

	public void OnClick()
	{
		if (url != string.Empty)
		{
			Application.OpenURL(url);
		}
	}
}
