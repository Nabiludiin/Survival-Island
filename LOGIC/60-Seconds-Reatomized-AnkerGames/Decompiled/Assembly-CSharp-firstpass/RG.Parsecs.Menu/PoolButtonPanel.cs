using Rewired;
using UnityEngine;

namespace RG.Parsecs.Menu;

public class PoolButtonPanel : MonoBehaviour
{
	private AssignableButtonPrefab assignableButton;

	public float timeout = 5f;

	private float currentTime;

	public void Show(AssignableButtonPrefab sender)
	{
		gameObject.SetActive(value: true);
		assignableButton = sender;
		ReInput.players.GetPlayer(0);
	}

	private void Update()
	{
		currentTime += Time.deltaTime;
		if (timeout < currentTime)
		{
			currentTime = 0f;
			gameObject.SetActive(value: false);
		}
	}

	private void OnGUI()
	{
		Event current = Event.current;
		if (current.isKey && !current.functionKey && current.keyCode != KeyCode.None)
		{
			Debug.Log("Detected key code: " + current.keyCode);
			assignableButton.Set(current.keyCode);
			currentTime = timeout;
		}
	}
}
