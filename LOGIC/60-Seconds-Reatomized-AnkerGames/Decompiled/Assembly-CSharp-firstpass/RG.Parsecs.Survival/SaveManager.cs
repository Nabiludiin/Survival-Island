using RG.Core.SaveSystem;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class SaveManager : MonoBehaviour
{
	[SerializeField]
	private SaveEvent _saveEvent;

	[SerializeField]
	private SaveEvent _globalData;

	[SerializeField]
	private EndOfDayListenerList ListenerList;

	[SerializeField]
	private EndGameData _endGameData;

	[SerializeField]
	private GlobalBoolVariable _isGameContinue;

	private StorageDataManager _instance;

	private void Start()
	{
		_saveEvent.FinishLoad();
	}

	private void OnEnable()
	{
		_instance = StorageDataManager.TheInstance;
		ListenerList.RegisterOnEndOfDay(ChangeSaveGameState, "PrepareSystem", 1000, this);
		ListenerList.RegisterOnEndOfDay(SetGameContinueVariable, "Goals", 6, this);
	}

	private void OnDestroy()
	{
		ListenerList.UnregisterOnEndOfDay(ChangeSaveGameState, "PrepareSystem");
		ListenerList.UnregisterOnEndOfDay(SetGameContinueVariable, "Goals");
	}

	private void SetGameContinueVariable()
	{
		_isGameContinue.Value = !_endGameData.RuntimeData.ShouldEndGame;
		if (!_isGameContinue.Value)
		{
			bool isOperationFinished = false;
			_instance.Save(_globalData.DataTag, () =>
			{
				isOperationFinished = true;
			}, () =>
			{
				isOperationFinished = true;
				Debug.LogError("Global data failed!!!");
			});
			while (!isOperationFinished)
			{
			}
		}
	}

	public void SaveGlobalGameData()
	{
		bool isOperationFinished = false;
		_instance.Save(_globalData.DataTag, () =>
		{
			isOperationFinished = true;
		}, () =>
		{
			isOperationFinished = true;
			Debug.LogError("Global game data save failed!!!");
		});
		while (!isOperationFinished)
		{
		}
	}

	public void ChangeSaveGameState()
	{
		bool isOperationFinished = false;
		_instance.Save(_saveEvent.DataTag, () =>
		{
			_instance.Save(_globalData.DataTag, () =>
			{
				isOperationFinished = true;
			}, () =>
			{
				isOperationFinished = true;
				Debug.LogError("Global data failed!!!");
			});
		}, () =>
		{
			isOperationFinished = true;
			Debug.LogError("Save game failed!!!");
		});
		while (!isOperationFinished)
		{
		}
	}
}
