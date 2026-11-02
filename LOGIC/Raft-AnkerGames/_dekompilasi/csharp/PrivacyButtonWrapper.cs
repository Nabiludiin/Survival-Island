using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class PrivacyButtonWrapper : Button
{
	private bool urlOpened;

	private PrivacyButtonWrapper()
	{
		onClick.AddListener(OpenDataPrivacyUrl);
	}

	private void OnFailure(string reason)
	{
		interactable = true;
		Debug.LogWarning($"Failed to get data privacy url: {reason}");
	}

	private void OpenUrl(string url)
	{
		interactable = true;
		urlOpened = true;
		Application.OpenURL(url);
	}

	private void OpenDataPrivacyUrl()
	{
		interactable = false;
		DataPrivacy.FetchPrivacyUrl(OpenUrl, OnFailure);
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus && urlOpened)
		{
			urlOpened = false;
			RemoteSettings.ForceUpdate();
		}
	}
}
