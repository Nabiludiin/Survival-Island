using RG.Core.SaveSystem;
using RG.Parsecs.Menu;
using RG.Parsecs.Scavenge;
using Twitter;
using UnityEngine;

public class TwitterManager : MonoBehaviour
{
	private enum TwitterErrorCodes
	{
		CouldNotAuthenticate = 32,
		UserNotFound = 50,
		UserSuspended = 63,
		UserSuspendedAndNotPermitted = 64,
		ClientNotPermittedThisAction = 87,
		InvalidOrExpiredToken = 89,
		UnableToVerifyCredentials = 99,
		BadAuthenticationData = 215,
		RateLimit = 88,
		TwitterIsOverCapacity = 130,
		PostingLimitationReached = 185,
		StatusIsDuplicate = 187
	}

	public SettingsSO SettingsSO;

	public GenericEvent OnShowPINFieldRequested = new GenericEvent();

	public GenericEvent<bool> OnAccessTokenResponse = new GenericEvent<bool>();

	public string CONSUMER_KEY = "ZTIGErlLad0vDNU3FywuuAUh0";

	public string CONSUMER_SECRET = "jhrOOy3p3X9PaL1Ocuqo1I1D3xOousXwa5hyiudIYa2vleL51r ";

	private const string PLAYER_PREFS_TWITTER_USER_ID = "TwitterUserID";

	private const string PLAYER_PREFS_TWITTER_USER_SCREEN_NAME = "TwitterUserScreenName";

	private const string PLAYER_PREFS_TWITTER_USER_TOKEN = "TwitterUserToken";

	private const string PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET = "TwitterUserTokenSecret";

	private RequestTokenResponse m_RequestTokenResponse;

	private AccessTokenResponse m_AccessTokenResponse;

	public bool IsLoggedIn { get; protected set; }

	private void Start()
	{
		LoadTwitterUserInfo();
	}

	public void LogIn()
	{
		StartCoroutine(API.GetRequestToken(CONSUMER_KEY, CONSUMER_SECRET, OnRequestTokenCallback));
	}

	public void EnterPin(string pin)
	{
		StartCoroutine(API.GetAccessToken(CONSUMER_KEY, CONSUMER_SECRET, m_RequestTokenResponse.Token, pin, OnAccessTokenCallback));
	}

	public void PostTweet(string text)
	{
		StartCoroutine(API.PostTweet(text, CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse, OnPostTweet));
	}

	public void PostTweetGIF(string text, string path)
	{
		StartCoroutine(API.PostGIFTweet(path, text, CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse, OnPostTweet));
	}

	public API.GIFTweet PrepareTweetGIF(string path)
	{
		return API.PrepareGIFTweet(path, CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse, OnPostTweet);
	}

	private void LoadTwitterUserInfo()
	{
		m_AccessTokenResponse = new AccessTokenResponse();
		m_AccessTokenResponse.UserId = SettingsSO.runtimeData.TWITTER_USER_ID;
		m_AccessTokenResponse.ScreenName = SettingsSO.runtimeData.TWITTER_USER_SCREEN_NAME;
		m_AccessTokenResponse.Token = SettingsSO.runtimeData.TWITTER_USER_TOKEN;
		m_AccessTokenResponse.TokenSecret = SettingsSO.runtimeData.TWITTER_USER_TOKEN_SECRET;
		if (!string.IsNullOrEmpty(m_AccessTokenResponse.Token) && !string.IsNullOrEmpty(m_AccessTokenResponse.ScreenName) && !string.IsNullOrEmpty(m_AccessTokenResponse.Token) && !string.IsNullOrEmpty(m_AccessTokenResponse.TokenSecret))
		{
			MonoBehaviour.print(string.Concat(string.Concat(string.Concat("LoadTwitterUserInfo - succeeded" + "\n    UserId : " + m_AccessTokenResponse.UserId, "\n    ScreenName : ", m_AccessTokenResponse.ScreenName), "\n    Token : ", m_AccessTokenResponse.Token), "\n    TokenSecret : ", m_AccessTokenResponse.TokenSecret));
			IsLoggedIn = true;
		}
	}

	private void OnRequestTokenCallback(bool success, RequestTokenResponse response)
	{
		if (success)
		{
			MonoBehaviour.print(string.Concat("OnRequestTokenCallback - succeeded" + "\n    Token : " + response.Token, "\n    TokenSecret : ", response.TokenSecret));
			m_RequestTokenResponse = response;
			OnShowPINFieldRequested.Invoke();
			API.OpenAuthorizationPage(response.Token);
		}
		else
		{
			MonoBehaviour.print("OnRequestTokenCallback - failed.");
		}
	}

	private void OnAccessTokenCallback(bool success, AccessTokenResponse response)
	{
		if (success)
		{
			MonoBehaviour.print(string.Concat(string.Concat(string.Concat("OnAccessTokenCallback - succeeded" + "\n    UserId : " + response.UserId, "\n    ScreenName : ", response.ScreenName), "\n    Token : ", response.Token), "\n    TokenSecret : ", response.TokenSecret));
			m_AccessTokenResponse = response;
			SettingsSO.runtimeData.TWITTER_USER_ID = response.UserId;
			SettingsSO.runtimeData.TWITTER_USER_SCREEN_NAME = response.ScreenName;
			SettingsSO.runtimeData.TWITTER_USER_TOKEN = response.Token;
			SettingsSO.runtimeData.TWITTER_USER_TOKEN_SECRET = response.TokenSecret;
			if (StorageDataManager.TheInstance != null)
			{
				StorageDataManager.TheInstance.Save("settings", () =>
				{
					Debug.Log("done");
				}, null);
			}
			IsLoggedIn = true;
		}
		else
		{
			MonoBehaviour.print("OnAccessTokenCallback - failed.");
		}
		OnAccessTokenResponse.Invoke(success);
	}

	private void OnPostTweet(bool success, int code)
	{
		if (!success)
		{
			switch ((TwitterErrorCodes)code)
			{
			case TwitterErrorCodes.CouldNotAuthenticate:
			case TwitterErrorCodes.UserNotFound:
			case TwitterErrorCodes.UserSuspended:
			case TwitterErrorCodes.UserSuspendedAndNotPermitted:
			case TwitterErrorCodes.ClientNotPermittedThisAction:
			case TwitterErrorCodes.InvalidOrExpiredToken:
			case TwitterErrorCodes.UnableToVerifyCredentials:
			case TwitterErrorCodes.BadAuthenticationData:
				UnlinkTwitter();
				break;
			}
		}
		MonoBehaviour.print("OnPostTweet - " + (success ? "succedded." : "failed."));
	}

	private void UnlinkTwitter()
	{
		IsLoggedIn = false;
		m_AccessTokenResponse = new AccessTokenResponse();
		SettingsSO.runtimeData.TWITTER_USER_ID = string.Empty;
		SettingsSO.runtimeData.TWITTER_USER_SCREEN_NAME = string.Empty;
		SettingsSO.runtimeData.TWITTER_USER_TOKEN = string.Empty;
		SettingsSO.runtimeData.TWITTER_USER_TOKEN_SECRET = string.Empty;
		if (StorageDataManager.TheInstance != null)
		{
			StorageDataManager.TheInstance.Save("settings", () =>
			{
				Debug.Log("done");
			}, null);
		}
	}
}
