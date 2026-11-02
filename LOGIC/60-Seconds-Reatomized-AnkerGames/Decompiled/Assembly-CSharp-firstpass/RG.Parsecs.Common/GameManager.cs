using System.Collections;
using System.Collections.Generic;
using RG.Parsecs.Survival;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RG.Parsecs.Common;

public class GameManager : Singleton<GameManager>
{
	public EndGameInfo EndInfo;

	[SerializeField]
	private HandCursor _cursor;

	[SerializeField]
	private string _survivalSceneName = "Survival";

	[SerializeField]
	private string _endSceneName = "EndScene";

	[SerializeField]
	private string _scavangeSceneName = "Scavenge_start";

	[SerializeField]
	private string _menuSceneName = "Menu";

	[SerializeField]
	private string _developerMenuSceneName = "DeveloperMenu";

	public LoadingManager LoadingManager { get; private set; }

	public StorageManager StorageManager { get; private set; }

	public ContentManager ContentManager { get; private set; }

	public RaycastCatcher RaycastCatcher { get; private set; }

	public bool IsLoadingObscurerVisible => LoadingManager.ObscurerController.IsObscurerVisible;

	public bool FirstMenuEnter { get; set; }

	public string ScavangeSceneName
	{
		get
		{
			return _scavangeSceneName;
		}
		set
		{
			_scavangeSceneName = value;
		}
	}

	private new void Awake()
	{
		base.Awake();
		LoadingManager = GetComponent<LoadingManager>();
		StorageManager = GetComponent<StorageManager>();
		ContentManager = GetComponent<ContentManager>();
		RaycastCatcher = GetComponentInChildren<RaycastCatcher>();
	}

	public void EndSurvivalGame()
	{
		LoadingManager.LoadScene(_endSceneName);
	}

	public void EndScavengeGame(GameResult result)
	{
		EndInfo = new EndGameInfo();
		EndInfo.Result = result;
		EndInfo.LeftFood = 0f;
		EndInfo.LeftFuel = 0f;
		EndInfo.SurvivedDays = 0;
		LoadingManager.LoadScene(_endSceneName);
	}

	public void StartSurvival()
	{
		LoadingManager.LoadScene(_survivalSceneName, ContentManager.LoadSurvivalAssetBundles, ContentManager.UnloadSurvivalAssetBundles);
	}

	public void StartSurvival(string saveEventToLoad)
	{
		LoadingManager.LoadScene(_survivalSceneName, saveEventToLoad, ContentManager.LoadSurvivalAssetBundles, ContentManager.UnloadSurvivalAssetBundles);
	}

	public void StartSurvival(List<Scene> scenesToUnload)
	{
		LoadingManager.LoadScene(_survivalSceneName, scenesToUnload, null, ContentManager.LoadSurvivalAssetBundles, ContentManager.UnloadSurvivalAssetBundles);
	}

	public void LoadSave()
	{
		LoadingManager.LoadSave(ContentManager.LoadSurvivalAssetBundles, ContentManager.UnloadSurvivalAssetBundles);
	}

	public void StartScavenge()
	{
		LoadingManager.LoadScene(_scavangeSceneName, ContentManager.LoadScavangeAssets, ContentManager.UnloadScavangeAssetBundles);
	}

	public void StartScavenge(List<Scene> scenesToUnload)
	{
		LoadingManager.LoadScene(_scavangeSceneName, scenesToUnload, null, ContentManager.LoadScavangeAssets, ContentManager.UnloadScavangeAssetBundles);
	}

	public void ResetGame()
	{
		LoadMenu();
	}

	public void LoadMenu()
	{
		LoadingManager.LoadScene(_menuSceneName, ContentManager.LoadMenuAssets, ContentManager.UnloadMenuAssetBundles);
	}

	public void LoadMenuWithOpening()
	{
		LoadingManager.LoadScene(_menuSceneName, ContentManager.LoadMenuAssets, ContentManager.UnloadMenuAssetBundles, "Opening");
	}

	public void LoadMenu(List<Scene> scenesToUnload)
	{
		LoadingManager.LoadScene(_menuSceneName, scenesToUnload, null, ContentManager.LoadMenuAssets, ContentManager.UnloadMenuAssetBundles);
	}

	public void LoadDeveloperMenu()
	{
		LoadingManager.LoadScene(_developerMenuSceneName);
	}

	public void CloseGame()
	{
		Application.Quit();
	}

	public void PlaySoundInvoke(IEnumerator soundPlayer)
	{
		StartCoroutine(soundPlayer);
	}
}
