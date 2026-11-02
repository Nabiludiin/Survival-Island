using System.Collections;
using I2.Loc;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Startup;

public class StartupLogoManager : MonoBehaviour
{
	private Coroutine _cor;

	public void StartLoadingGame()
	{
		StartCoroutine(LoadGame());
	}

	private IEnumerator LoadGame()
	{
		LocalizationManager.InitializeIfNeeded();
		yield return Singleton<GameManager>.Instance.ContentManager.LoadCommonAsset();
		Singleton<GameManager>.Instance.LoadMenuWithOpening();
	}
}
