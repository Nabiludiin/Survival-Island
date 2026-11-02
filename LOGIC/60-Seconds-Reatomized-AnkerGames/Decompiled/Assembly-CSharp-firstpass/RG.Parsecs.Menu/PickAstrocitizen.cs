using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace RG.Parsecs.Menu;

public class PickAstrocitizen : MonoBehaviour
{
	public LocalizedString normal;

	public LocalizedString survival;

	public LocalizedString tutorial;

	public UnityEvent onStartSurvival;

	public UnityEvent onStartNewGame;

	private EGameModes _mode;

	public TextMeshProUGUI title;

	public void SetGameMode(int mode)
	{
		_mode = (EGameModes)mode;
		switch (_mode)
		{
		case EGameModes.Classic:
			title.text = normal;
			break;
		case EGameModes.Survival:
			title.text = survival;
			break;
		case EGameModes.Tutorial:
			title.text = tutorial;
			break;
		default:
			Debug.LogError("Wrong game mode selected");
			break;
		}
	}

	public void StartGame()
	{
		switch (_mode)
		{
		case EGameModes.Classic:
			onStartNewGame.Invoke();
			break;
		case EGameModes.Survival:
			onStartSurvival.Invoke();
			break;
		default:
			Debug.LogError("Wrong game mode selected");
			break;
		}
	}
}
