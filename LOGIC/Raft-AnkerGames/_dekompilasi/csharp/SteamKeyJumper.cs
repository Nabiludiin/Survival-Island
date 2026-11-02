using System.Collections;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class SteamKeyJumper : MonoBehaviour
{
	public static string STEAMKEY;

	public bool useSteamName;

	public Text textComponent;

	public RectTransform rect;

	public float jumpInterval;

	public RectTransform[] jumpPositions;

	private void Start()
	{
		if (useSteamName)
		{
			textComponent.text = SteamFriends.GetPersonaName();
		}
		else
		{
			textComponent.text = STEAMKEY;
		}
		if (!string.IsNullOrEmpty(textComponent.text))
		{
			StartCoroutine(Jump());
		}
		else
		{
			gameObject.SetActive(value: false);
		}
	}

	private IEnumerator Jump()
	{
		while (true)
		{
			rect.position = jumpPositions[Random.Range(0, jumpPositions.Length)].position;
			yield return new WaitForSeconds(jumpInterval);
		}
	}
}
