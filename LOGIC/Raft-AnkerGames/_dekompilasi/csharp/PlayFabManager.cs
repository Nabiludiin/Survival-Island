using System;
using System.Collections;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Party;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

public class PlayFabManager : SimpleMonoBehaviourSingleton<PlayFabManager>
{
	public UnityAction OnPlayFabSignedIn;

	public UnityAction<string, string> OnJoinWorld;

	private string m_xboxToken;

	private PlayFabAuthenticationContext m_authenticationContext;

	private string m_titleAccountId;

	private string m_masterAccountId;

	private string m_sessionTicket;

	private bool m_failedToLogin;

	private EntityTokenResponse m_tokenResponse;

	private Coroutine m_checkExpirationRoutine;

	private Coroutine m_retryLoginRoutine;

	private HAuthTicket currentTicket = HAuthTicket.Invalid;

	private Callback<GetAuthSessionTicketResponse_t> m_authTicketResponseCallback;

	private string m_pendingSteamTicket;

	private bool m_waitingForTicketResponse;

	public string XboxToken => m_xboxToken;

	public PlayFabAuthenticationContext AuthenticationContext => m_authenticationContext;

	public string TitleAccountId => m_titleAccountId;

	public string MasterAccountId => m_masterAccountId;

	public string SessionTicket => m_sessionTicket;

	public static void DisplayPlayFabError(PlayFabError error)
	{
		Debug.LogError(error.GenerateErrorReport());
	}

	public string GetSteamAuthTicket()
	{
		byte[] array = new byte[1024];
		SteamNetworkingIdentity pSteamNetworkingIdentity = default;
		pSteamNetworkingIdentity.m_eType = ESteamNetworkingIdentityType.k_ESteamNetworkingIdentityType_SteamID;
		pSteamNetworkingIdentity.SetSteamID(SteamUser.GetSteamID());
		currentTicket = SteamUser.GetAuthSessionTicket(array, array.Length, out var pcbTicket, ref pSteamNetworkingIdentity);
		Array.Resize(ref array, (int)pcbTicket);
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			stringBuilder.AppendFormat("{0:x2}", b);
		}
		return stringBuilder.ToString();
	}

	public void Start()
	{
		m_authTicketResponseCallback = Callback<GetAuthSessionTicketResponse_t>.Create(OnGetAuthSessionTicketResponse);
		if (SteamManager.Initialized && SteamUser.BLoggedOn())
		{
			RequestSteamLogin();
		}
		else
		{
			Debug.LogError("Called to quick or not logged on");
		}
	}

	private void RequestSteamLogin()
	{
		if (currentTicket != HAuthTicket.Invalid)
		{
			SteamUser.CancelAuthTicket(currentTicket);
			currentTicket = HAuthTicket.Invalid;
		}
		m_pendingSteamTicket = GetSteamAuthTicket();
		m_waitingForTicketResponse = true;
	}

	private void OnGetAuthSessionTicketResponse(GetAuthSessionTicketResponse_t response)
	{
		if (!m_waitingForTicketResponse || response.m_hAuthTicket != currentTicket)
		{
			return;
		}
		m_waitingForTicketResponse = false;
		if (response.m_eResult == EResult.k_EResultOK)
		{
			Debug.Log("Steam auth ticket validated, logging into PlayFab...");
			PlayFabClientAPI.LoginWithSteam(new LoginWithSteamRequest
			{
				SteamTicket = m_pendingSteamTicket,
				CreateAccount = true,
				TicketIsServiceSpecific = false
			}, OnSteamLoginSuccess, OnSteamLoginFailure);
			return;
		}
		Debug.LogError($"Steam auth ticket failed with result: {response.m_eResult}");
		SteamUser.CancelAuthTicket(currentTicket);
		currentTicket = HAuthTicket.Invalid;
		if (m_retryLoginRoutine == null)
		{
			m_retryLoginRoutine = StartCoroutine(RetryLogin());
		}
	}

	private void LoginWithSteam()
	{
		RequestSteamLogin();
	}

	public void Update()
	{
		if (m_failedToLogin)
		{
			LoginWithSteam();
			m_failedToLogin = false;
		}
	}

	private void OnSteamLoginFailure(PlayFabError error)
	{
		SteamUser.CancelAuthTicket(currentTicket);
		if (m_retryLoginRoutine == null)
		{
			m_retryLoginRoutine = StartCoroutine(RetryLogin());
		}
		Debug.LogError(error.GenerateErrorReport());
	}

	private void OnSteamLoginSuccess(LoginResult result)
	{
		SteamUser.CancelAuthTicket(currentTicket);
		Debug.Log("Congratulations, you made your first successful API call!");
		UserSignedIn(result);
	}

	private void UserSignedIn(LoginResult result)
	{
		m_retryLoginRoutine = null;
		m_failedToLogin = false;
		m_authenticationContext = result.AuthenticationContext;
		m_tokenResponse = result.EntityToken;
		m_titleAccountId = m_tokenResponse.Entity.Id;
		m_masterAccountId = result.PlayFabId;
		m_sessionTicket = result.SessionTicket;
		PlayFabMultiplayerManager.Get().UpdateEntityToken(m_tokenResponse.EntityToken);
		if (result.EntityToken.TokenExpiration.HasValue)
		{
			DateTime timeWhenExpire = DateTime.UtcNow + (result.EntityToken.TokenExpiration.Value - DateTime.UtcNow) / 2.0;
			if (m_checkExpirationRoutine == null)
			{
				m_checkExpirationRoutine = StartCoroutine(CheckTokenExpiration(timeWhenExpire));
			}
		}
		OnPlayFabSignedIn?.Invoke();
	}

	public void JoinWorld(string worldCode)
	{
		OnJoinWorld?.Invoke(worldCode, GameManager.Password);
	}

	private IEnumerator RetryLogin()
	{
		yield return new WaitForSecondsRealtime(10f);
		m_failedToLogin = true;
		m_retryLoginRoutine = null;
	}

	private IEnumerator CheckTokenExpiration(DateTime timeWhenExpire)
	{
		while (timeWhenExpire > DateTime.UtcNow)
		{
			yield return new WaitForSecondsRealtime(500f);
		}
		Debug.Log("Reauthorize");
		if (SteamManager.Initialized && SteamUser.BLoggedOn())
		{
			LoginWithSteam();
		}
		m_checkExpirationRoutine = null;
	}
}
