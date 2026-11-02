using System.Collections;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Menu;

public class SetMenuState : MonoBehaviour
{
	public BasePauseMenu.EGameState GameState;

	private IEnumerator Start()
	{
		if (GameState == BasePauseMenu.EGameState.GamePlay && Singleton<GameManager>.Instance != null && Singleton<GameManager>.Instance.LoadingManager != null)
		{
			while (Singleton<GameManager>.Instance.LoadingManager.CurrentlyLoading)
			{
				yield return null;
			}
		}
		if (BasePauseMenu.Instance != null)
		{
			BasePauseMenu.Instance.currentGameState = GameState;
		}
		else
		{
			Debug.LogError("Trying set menu state to " + GameState.ToString() + " but PauseMenu is null");
		}
		if (Singleton<GameManager>.Instance != null)
		{
			Singleton<GameManager>.Instance.RaycastCatcher.SetCatchingRaycasts(catchingRaycasts: false, null);
		}
		yield return null;
	}
}
