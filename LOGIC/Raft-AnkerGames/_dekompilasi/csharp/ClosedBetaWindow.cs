using UnityEngine;
using UnityEngine.UI;

public class ClosedBetaWindow : MonoBehaviour
{
	public bool useSteamKeyValidityWindow;

	public SteamKeyValidity steamKeyValidity;

	public InputField inputField_SteamKey;

	public Button button_acceptSteam;

	public Button button_acceptNDA;

	public Scrollbar ndaScrollbar;

	public Text errorText;

	public GameObject window_NDA;

	public GameObject window_steamKey;

	public GameObject window_background;

	private void Start()
	{
		string sTEAMKEY = (inputField_SteamKey.text = PlayerPrefs.GetString("BetaKeyInput", string.Empty));
		SteamKeyJumper.STEAMKEY = sTEAMKEY;
	}

	private void Update()
	{
		if (window_background.activeInHierarchy)
		{
			SteamKeyResult steamKeyResult = steamKeyValidity.CheckKey(inputField_SteamKey.text);
			if (inputField_SteamKey.text.Length > 0)
			{
				switch (steamKeyResult)
				{
				case SteamKeyResult.NoMatch:
					errorText.text = "No matching key found";
					break;
				case SteamKeyResult.NoKeysAvailable:
					errorText.text = "No keys available";
					break;
				case SteamKeyResult.WrongFormat:
					errorText.text = "Wrong format, use: XXXXX-XXXXX-XXXXX";
					break;
				case SteamKeyResult.Error:
					errorText.text = "Unknown error";
					break;
				default:
					errorText.text = string.Empty;
					break;
				}
			}
			else
			{
				errorText.text = string.Empty;
			}
			button_acceptSteam.interactable = steamKeyResult == SteamKeyResult.Valid;
		}
		if (window_NDA.activeInHierarchy)
		{
			button_acceptNDA.interactable = ndaScrollbar.value <= 0.01f;
		}
	}

	public void Button_AcceptSteamKey()
	{
		SteamKeyJumper.STEAMKEY = inputField_SteamKey.text;
		PlayerPrefs.SetString("BetaKeyInput", SteamKeyJumper.STEAMKEY);
		window_NDA.gameObject.SetActive(value: false);
		window_steamKey.gameObject.SetActive(value: false);
		window_background.gameObject.SetActive(value: false);
	}

	public void Button_AcceptND()
	{
		window_NDA.gameObject.SetActive(value: false);
		if (useSteamKeyValidityWindow)
		{
			window_steamKey.gameObject.SetActive(value: true);
		}
		else
		{
			window_background.gameObject.SetActive(value: false);
		}
	}
}
