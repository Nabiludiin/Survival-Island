using System.Collections.Generic;
using RG.Core.Base;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class CharacterManager : RGMonoBehaviour
{
	private static CharacterManager _instance;

	[SerializeField]
	private CharacterList _characterList;

	[SerializeField]
	private VariableReferenceList _variableList;

	[SerializeField]
	private CharacterStatusList _characterStatusList;

	public static CharacterManager Instance
	{
		get
		{
			if (_instance == null)
			{
				FindInstanceInScene();
			}
			return _instance;
		}
	}

	private static void FindInstanceInScene()
	{
		_instance = Object.FindObjectOfType<CharacterManager>();
		if (_instance == null)
		{
			Debug.LogError("No CharacterManager in scene!");
		}
	}

	protected override void CustomAwake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else if (_instance != this)
		{
			Debug.LogWarning("Duplicate instance of CharacterManager detected! Removing " + gameObject.name);
			Object.Destroy(this);
		}
		_characterList.RemoveForbiddenNeedsFromCaptain();
	}

	public void OnEnable()
	{
		RegisterDevToolsCommands();
		_characterList.RegisterInI2Loc();
	}

	public void OnDisable()
	{
		UnregisterDevToolsCommands();
	}

	protected override void CustomStart()
	{
		List<Character> liveCharacters = _characterList.GetLiveCharacters();
		if (liveCharacters.Count == 0)
		{
			throw new UnityException("No live characters in CharacterList in survival scene!");
		}
		for (int i = 0; i < liveCharacters.Count; i++)
		{
			liveCharacters[i].RuntimeData.IsCaptain = false;
		}
		liveCharacters[0].RuntimeData.IsCaptain = true;
		_characterList.SetStartingData();
	}

	public CharacterList GetCharacterList()
	{
		return _characterList;
	}

	public CharacterStatusList GetCharacterStatusList()
	{
		return _characterStatusList;
	}

	private void RegisterDevToolsCommands()
	{
	}

	private void UnregisterDevToolsCommands()
	{
	}
}
