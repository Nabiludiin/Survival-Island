using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
	public static bool IsLoadingScene;

	public static bool IsLoadingLobbyScene;

	public static bool IsGameSceneLoaded;

	[SerializeField]
	private FadePanel fadePanel;

	[SerializeField]
	private GameObject loadPanel;

	[SerializeField]
	private GameObject waitPanel;

	[SerializeField]
	private GameObject leaveOptionPanel;

	[SerializeField]
	private float loadingTipInterval = 10f;

	[SerializeField]
	private int loadingTipCount = 10;

	[SerializeField]
	private Text loadingText;

	[SerializeField]
	private Text loadingTipText;

	[SerializeField]
	private Slider loadingbarSlider;

	[SerializeField]
	private GameObject loadingCircle;

	[SerializeField]
	private float tryingToConnectPanelDelay = 5f;

	[SerializeField]
	private float leaveButtonDelay = 10f;

	private Coroutine coroutineFade;

	private int currentLoadingTipIndex = -1;

	private float loadingTipTimestamp = -1f;

	private float showTryingToConnectPanelTime = -1f;

	private float showLeaveButtonTime = -1f;

	private void Awake()
	{
		IsLoadingScene = false;
		IsLoadingLobbyScene = false;
		IsGameSceneLoaded = false;
		fadePanel.gameObject.SetActive(value: false);
		waitPanel.SetActive(value: false);
	}

	public void LoadScene(string sceneName, bool loadAdditively)
	{
		leaveOptionPanel.SetActive(value: false);
		showLeaveButtonTime = -1f;
		showTryingToConnectPanelTime = -1f;
		StartCoroutine(Load(sceneName, loadAdditively));
	}

	public void LeaveGameInLoadingScreen()
	{
		if (!GameManager.IsLeavingGame)
		{
			ComponentManager<Raft_Network>.Value.LeaveGame(DisconnectReason.SelfDisconnected, SceneName.Lobby, saveGame: false, force: true);
		}
		ComponentManager<Raft_Network>.Value.CleanupState();
	}

	private void TryToDisplayNewLoadingTip()
	{
		if (!(loadingTipText == null) && (loadingTipTimestamp == -1f || Time.realtimeSinceStartup - loadingTipTimestamp > loadingTipInterval))
		{
			loadingTipTimestamp = Time.realtimeSinceStartup;
			currentLoadingTipIndex = GetRandomLoadingTipIndex();
			loadingTipText.text = Helper.GetTerm("LoadMenu/Tip" + currentLoadingTipIndex);
		}
	}

	private int GetRandomLoadingTipIndex()
	{
		if (loadingTipCount == 0)
		{
			return -1;
		}
		if (loadingTipCount == 1)
		{
			return 0;
		}
		int num = -1;
		int num2 = 20;
		while (num == -1 || num == currentLoadingTipIndex)
		{
			num = Random.Range(0, loadingTipCount);
			num2--;
			if (num2 <= 0)
			{
				Debug.Log("Could not find unique index when trying to get a random loading tip. Safety breaking");
				break;
			}
		}
		return Mathf.Clamp(num, 0, loadingTipCount - 1);
	}

	private IEnumerator Load(string sceneName, bool loadAdditively)
	{
		ButtonSound.Mute = true;
		IsLoadingScene = true;
		SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.DisableInput();
		float progressAmount_mainscene = 0.2f;
		float progressAmount_landmarks = 0.8f;
		fadePanel.gameObject.SetActive(value: true);
		fadePanel.SetAlpha(0f);
		loadPanel.SetActive(value: false);
		if (coroutineFade != null)
		{
			StopCoroutine(coroutineFade);
			coroutineFade = null;
		}
		yield return StartCoroutine(fadePanel.FadeToAlpha(1f, 2f));
		loadPanel.SetActive(value: true);
		if (sceneName == Raft_Network.MenuSceneName)
		{
			IsLoadingLobbyScene = true;
		}
		loadingTipText?.gameObject.SetActiveSafe(!IsLoadingLobbyScene);
		loadingCircle.SetActiveSafe(!IsLoadingLobbyScene);
		loadingbarSlider.gameObject.SetActiveSafe(!IsLoadingLobbyScene);
		loadingbarSlider.value = 0f;
		TryToDisplayNewLoadingTip();
		if (sceneName == Raft_Network.GameSceneName)
		{
			loadingText.text = Helper.GetTerm("LoadMenu/EnteringWorld");
		}
		else if (sceneName == Raft_Network.MenuSceneName)
		{
			loadingText.text = Helper.GetTerm("LoadMenu/LoadingMenu");
		}
		Helper.LogBuild("Waiting for items to be loaded");
		while (!ItemManager.ItemsLoaded)
		{
			TryToDisplayNewLoadingTip();
			yield return null;
		}
		Helper.LogBuild("Loading scene " + sceneName + "...");
		float startTime = Time.realtimeSinceStartup;
		AsyncOperation operation = ((!loadAdditively) ? SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single) : SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));
		while (!operation.isDone)
		{
			yield return null;
		}
		loadingbarSlider.value += progressAmount_mainscene;
		Helper.LogBuild("Scene " + sceneName + " loaded in " + (Time.realtimeSinceStartup - startTime) + " seconds");
		if (sceneName == Raft_Network.GameSceneName)
		{
			if (GameManager.GameMode != GameMode.Creative)
			{
				Helper.LogBuild("Loading assets...");
				loadingText.text = Helper.GetTerm("LoadMenu/InitingAssets");
				startTime = Time.realtimeSinceStartup;
				SceneLoader sceneLoader = ComponentManager<SceneLoader>.Value;
				sceneLoader.LoadScenes();
				float prevProgress = loadingbarSlider.value;
				while (!Raft_Network.IsAllLandmarksLoaded)
				{
					loadingbarSlider.value = prevProgress + progressAmount_landmarks * sceneLoader.LoadSceneProgressNormalized;
					TryToDisplayNewLoadingTip();
					yield return null;
				}
				Helper.LogBuild("Assets loaded in " + (Time.realtimeSinceStartup - startTime) + " seconds");
			}
			else
			{
				Raft_Network.IsAllLandmarksLoaded = true;
				Helper.LogBuild("Skipped loading assets because gamemode was Creative");
			}
		}
		else if (sceneName == Raft_Network.MenuSceneName)
		{
			Raft_Network.IsAllLandmarksLoaded = false;
		}
		while (!Raft_Network.WorldHasBeenRecieved && !GameManager.IsLeavingGame)
		{
			if (showTryingToConnectPanelTime == -1f)
			{
				showTryingToConnectPanelTime = Time.time + tryingToConnectPanelDelay;
			}
			else if (showTryingToConnectPanelTime < Time.time)
			{
				waitPanel.SetActive(value: true);
			}
			if (showLeaveButtonTime == -1f)
			{
				showLeaveButtonTime = Time.time + leaveButtonDelay;
			}
			else if (showLeaveButtonTime < Time.time)
			{
				Helper.SetCursorVisibleAndLockState(state: true, CursorLockMode.None);
				SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.EnableInput();
				leaveOptionPanel.SetActive(value: true);
				leaveOptionPanel.GetComponentInChildren<Button>().Select();
			}
			yield return null;
		}
		loadPanel.SetActive(value: false);
		waitPanel.SetActive(value: false);
		leaveOptionPanel.SetActive(value: false);
		coroutineFade = StartCoroutine(fadePanel.FadeToAlpha(0f, 2f));
		yield return coroutineFade;
		coroutineFade = null;
		fadePanel.gameObject.SetActive(value: false);
		IsLoadingLobbyScene = false;
		IsGameSceneLoaded = sceneName == Raft_Network.GameSceneName;
		if (!IsGameSceneLoaded)
		{
			SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.SwitchCurrentActionMap("UI");
		}
		SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.EnableInput();
		IsLoadingScene = false;
		ButtonSound.Mute = false;
		if (!IsGameSceneLoaded)
		{
			GameManager.IsLeavingGame = false;
		}
	}
}
