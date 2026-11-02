using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Menu;

public class ManagerSpawners : MonoBehaviour
{
	public GameObject PauseMenuPrefab;

	public GameObject StorageManagerPrefab;

	public GameObject NewGameManager;

	private void Awake()
	{
		if (BasePauseMenu.Instance == null)
		{
			Object.Instantiate(PauseMenuPrefab);
		}
		if (StorageDataManager.TheInstance == null)
		{
			Object.Instantiate(StorageManagerPrefab);
		}
		if (Singleton<GameManager>.Instance == null)
		{
			Object.Instantiate(NewGameManager);
		}
	}
}
