using UnityEngine;

public static class GameModeValueManager
{
	private static SO_GameModeValue currentDifficulty;

	private static bool isSetup;

	[SerializeField]
	private static SO_GameModeValue creative;

	[SerializeField]
	private static SO_GameModeValue peaceful;

	[SerializeField]
	private static SO_GameModeValue normal;

	[SerializeField]
	private static SO_GameModeValue hard;

	private static SO_GameModeValue[] gameModeValues;

	public static bool IsPlayerInvurnerable
	{
		get
		{
			if (GetCurrentGameModeValue() != null)
			{
				if (!GetCurrentGameModeValue().playerSpecificVariables.alwaysHealthy)
				{
					return Cheat.UseGodMode;
				}
				return true;
			}
			return false;
		}
	}

	public static void InitializeGameModes()
	{
		if (!isSetup)
		{
			isSetup = true;
			gameModeValues = Resources.LoadAll<SO_GameModeValue>("GameModeSO");
			currentDifficulty = gameModeValues[1];
		}
	}

	public static void SelectCurrentGameMode(GameMode p_wantedGameMode)
	{
		GameManager.GameMode = p_wantedGameMode;
		if (!isSetup)
		{
			InitializeGameModes();
		}
		if (currentDifficulty.gameMode == p_wantedGameMode)
		{
			return;
		}
		for (int i = 0; i < gameModeValues.Length; i++)
		{
			if (gameModeValues[i].gameMode == p_wantedGameMode)
			{
				currentDifficulty = gameModeValues[i];
			}
		}
	}

	public static SO_GameModeValue GetCurrentGameModeValue()
	{
		if (currentDifficulty == null)
		{
			SelectCurrentGameMode(GameManager.GameMode);
			if (currentDifficulty == null)
			{
				Debug.LogError("seems like the current game mode was null, after recheck falling back to default procedure");
				currentDifficulty = gameModeValues[1];
			}
		}
		return currentDifficulty;
	}

	public static bool IsCreatureTameInCurrentGameMode(AI_NetworkBehaviourType type)
	{
		switch (type)
		{
		case AI_NetworkBehaviourType.StoneBird:
			return currentDifficulty.stonebirdVariables.isTame;
		case AI_NetworkBehaviourType.PufferFish:
			return currentDifficulty.pufferfishVariables.isTame;
		case AI_NetworkBehaviourType.Llama:
			return true;
		case AI_NetworkBehaviourType.Goat:
			return true;
		case AI_NetworkBehaviourType.Chicken:
			return true;
		case AI_NetworkBehaviourType.Boar:
			return currentDifficulty.boarVariables.isTame;
		case AI_NetworkBehaviourType.Rat:
			return currentDifficulty.ratVariables.isTame;
		case AI_NetworkBehaviourType.Shark:
			return currentDifficulty.sharkVariables.isTame;
		case AI_NetworkBehaviourType.Bear:
		case AI_NetworkBehaviourType.PolarBear:
			return currentDifficulty.bearVariables.isTame;
		case AI_NetworkBehaviourType.MamaBear:
			return currentDifficulty.bearVariables.isTame;
		case AI_NetworkBehaviourType.ButlerBot:
			return currentDifficulty.butlerBotVariables.isTame;
		default:
			return false;
		}
	}

	public static EntityVariables GetDifficultyEntityVariableFromAINetworkBehaviourType(AI_NetworkBehaviourType type)
	{
		switch (type)
		{
		case AI_NetworkBehaviourType.StoneBird:
		case AI_NetworkBehaviourType.StoneBird_Caravan:
			return currentDifficulty.stonebirdVariables;
		case AI_NetworkBehaviourType.Bear:
		case AI_NetworkBehaviourType.MamaBear:
		case AI_NetworkBehaviourType.PolarBear:
			return currentDifficulty.bearVariables;
		case AI_NetworkBehaviourType.PufferFish:
			return currentDifficulty.pufferfishVariables;
		case AI_NetworkBehaviourType.Boar:
			return currentDifficulty.boarVariables;
		case AI_NetworkBehaviourType.Rat:
		case AI_NetworkBehaviourType.Rat_Tangaroa:
			return currentDifficulty.ratVariables;
		case AI_NetworkBehaviourType.Shark:
			return currentDifficulty.sharkVariables;
		case AI_NetworkBehaviourType.Pig:
			return currentDifficulty.pigVariables;
		case AI_NetworkBehaviourType.ButlerBot:
			return currentDifficulty.butlerBotVariables;
		case AI_NetworkBehaviourType.AnglerFish:
			return currentDifficulty.anglerFishVariables;
		case AI_NetworkBehaviourType.Hyena:
			return currentDifficulty.hyenaVariables;
		case AI_NetworkBehaviourType.Boss_Varuna:
			return currentDifficulty.varunaBossVariables;
		case AI_NetworkBehaviourType.HyenaBoss:
			return currentDifficulty.utopiaBossVariables;
		default:
			return null;
		}
	}
}
