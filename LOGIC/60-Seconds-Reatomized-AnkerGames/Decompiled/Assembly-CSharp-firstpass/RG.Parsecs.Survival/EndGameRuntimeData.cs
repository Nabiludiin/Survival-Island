using System;
using RG.Parsecs.EndGameEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class EndGameRuntimeData
{
	[SerializeField]
	[Tooltip("Should Game End?")]
	private bool _shouldEndGame;

	[SerializeField]
	[Tooltip("Does the game have an ending with slides?")]
	private bool _isGameWon;

	[SerializeField]
	[Tooltip("The EndGameCanvas/slide that will describe how the player dealt with the main plot problem.")]
	private EndGameCanvas _mainQuestSummary;

	[SerializeField]
	[Tooltip("The EndGameCanvas/slide that will describe the fate of the captain. If left null, the one set in the Character will be used.")]
	private EndGameCanvas _captainsFate;

	[SerializeField]
	[Tooltip("The EndGameCanvas/slide that will describe a general overview of the finished game that is influenced by playstyle and miscellaneous stats.")]
	private EndGameCanvas _sessionSummary;

	public EndGameCanvas MainQuestSummary
	{
		get
		{
			return _mainQuestSummary;
		}
		set
		{
			_mainQuestSummary = value;
		}
	}

	public EndGameCanvas CaptainsFate
	{
		get
		{
			return _captainsFate;
		}
		set
		{
			_captainsFate = value;
		}
	}

	public EndGameCanvas SessionSummary
	{
		get
		{
			return _sessionSummary;
		}
		set
		{
			_sessionSummary = value;
		}
	}

	public bool ShouldEndGame
	{
		get
		{
			return _shouldEndGame;
		}
		set
		{
			_shouldEndGame = value;
		}
	}

	public bool IsGameWon
	{
		get
		{
			return _isGameWon;
		}
		set
		{
			_isGameWon = value;
		}
	}

	public void Reset()
	{
		_shouldEndGame = false;
		_isGameWon = false;
		_mainQuestSummary = null;
		_captainsFate = null;
		_sessionSummary = null;
	}
}
