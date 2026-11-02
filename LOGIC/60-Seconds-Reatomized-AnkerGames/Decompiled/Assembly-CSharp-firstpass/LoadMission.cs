using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.Survival;
using UnityEngine;
using UnityEngine.UI;

public class LoadMission : MonoBehaviour
{
	public Mission Mission;

	public SaveEvent _gameData;

	public CharacterList _characterList;

	public GlobalBoolVariable _onlySurvival;

	private void Start()
	{
		Button component = GetComponent<Button>();
		component.onClick.AddListener(LoadGameMission);
		component.GetComponentInChildren<Text>().text = Mission.Name;
	}

	private void LoadGameMission()
	{
		_gameData.ResetGame();
		MissionManager.Instance.SetActualMission(Mission);
		if (!Mission.SetCaptainExternal)
		{
			_characterList.AddCharToList(Mission.Captain);
		}
		_onlySurvival.Value = true;
		Singleton<GameManager>.Instance.StartSurvival();
	}
}
