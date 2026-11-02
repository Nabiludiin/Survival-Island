using UnityEngine;
using UnityEngine.UI;

public class OfflineModePopup : MonoBehaviour
{
	public Button[] ButtonsToDisable;

	public GameObject WarningPopup;

	private void Start()
	{
		RetryOfflineMode();
	}

	public void RetryOfflineMode()
	{
	}

	public void Ignore()
	{
		WarningPopup.SetActive(value: false);
		for (int i = 0; i < ButtonsToDisable.Length; i++)
		{
			ButtonsToDisable[i].interactable = true;
		}
	}
}
