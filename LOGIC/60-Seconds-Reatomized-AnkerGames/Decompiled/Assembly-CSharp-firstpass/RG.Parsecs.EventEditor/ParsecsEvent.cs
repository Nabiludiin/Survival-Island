using System.Collections.Generic;
using I2.Loc;
using NodeEditorFramework;
using RG.Core.SaveSystem;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public abstract class ParsecsEvent : NodeCanvas, ISaveable
{
	private const string EVENT_WAS_INVOKED_WHEN_FINISHED = "Event `{0}` was invoked but it's state was already finished. Are events cleaned up propely after they've finished their execution?";

	[SerializeField]
	protected SaveEvent _saveEvent;

	[SerializeField]
	[Tooltip("Displayable name of the event.")]
	private LocalizedString _name = string.Empty;

	[SerializeField]
	[Tooltip("Displayable description of the event.")]
	private LocalizedString _shortDescription = string.Empty;

	[SerializeField]
	[Tooltip("List of Character-Type parameters send when event is invoked.")]
	private List<CharacterParameter> _characterParameters;

	[SerializeField]
	[Tooltip("List of Item-Type parameters send when event is invoked.")]
	private List<ItemParameter> _itemParameters;

	private const string EVENT_PREFIX = "event";

	public SaveEvent SaveEvent
	{
		get
		{
			return _saveEvent;
		}
		set
		{
			_saveEvent = value;
		}
	}

	public EParsecsEventPhase CurrentPhase { get; protected set; }

	public LocalizedString Name => _name;

	public LocalizedString ShortDescription => _shortDescription;

	public List<CharacterParameter> CharacterParameters => _characterParameters;

	public List<ItemParameter> ItemParameters => _itemParameters;

	public string ID => Guid;

	public override string GuidPrefix => "event";

	public void OnEnable()
	{
		Register();
	}

	private void OnDestroy()
	{
		Unregister();
	}

	public void Run()
	{
		if (CurrentPhase == EParsecsEventPhase.FINISHED)
		{
			Debug.LogWarningFormat("Event `{0}` was invoked but it's state was already finished. Are events cleaned up propely after they've finished their execution?", name);
			return;
		}
		if (CurrentPhase == EParsecsEventPhase.READY)
		{
			SetInitialPhase();
			BeginEvent();
		}
		PreExecute();
		Execute();
		PostExecute();
		UpdateCurrentPhase();
		if (CurrentPhase == EParsecsEventPhase.FINISHED)
		{
			EndEvent();
		}
	}

	protected abstract void SetInitialPhase();

	protected abstract void UpdateCurrentPhase();

	protected virtual void BeginEvent()
	{
	}

	protected virtual void PreExecute()
	{
	}

	protected abstract void Execute();

	protected virtual void PostExecute()
	{
	}

	protected virtual void EndEvent()
	{
	}

	public void DrawInfoGUI()
	{
	}

	private void ParsecEventGUI()
	{
	}

	protected virtual void InfoGUI()
	{
	}

	protected void ExecuteNode(string nodeId)
	{
		EventNode eventNode = FindNodeOfId<EventNode>(nodeId);
		if (eventNode == null)
		{
			throw new NodeNotFoundException(nodeId, name);
		}
		eventNode.ExecuteWithErrorHandling(this);
	}

	public Character GetCharacterParameterByName(string parameterName)
	{
		for (int i = 0; i < _characterParameters.Count; i++)
		{
			if (_characterParameters[i].Name == parameterName)
			{
				return _characterParameters[i].Character;
			}
		}
		throw new ParsecsEventParameterNotFoundException(this, parameterName);
	}

	public void SetCharacterParameterByName(string parameterName, Character character)
	{
		bool flag = false;
		for (int i = 0; i < _characterParameters.Count; i++)
		{
			if (_characterParameters[i].Name == parameterName)
			{
				flag = true;
				_characterParameters[i].Character = character;
			}
		}
		if (!flag)
		{
			throw new ParsecsEventParameterNotFoundException(this, parameterName);
		}
	}

	public IItem GetItemParameterByName(string parameterName)
	{
		for (int i = 0; i < _itemParameters.Count; i++)
		{
			if (_itemParameters[i].Name == parameterName)
			{
				return _itemParameters[i].Item;
			}
		}
		throw new ParsecsEventParameterNotFoundException(this, parameterName);
	}

	public void SetItemParameterByName(string parameterName, IItem item)
	{
		bool flag = false;
		for (int i = 0; i < _itemParameters.Count; i++)
		{
			if (_itemParameters[i].Name == parameterName)
			{
				flag = true;
				_itemParameters[i].Item = item;
			}
		}
		if (!flag)
		{
			throw new ParsecsEventParameterNotFoundException(this, parameterName);
		}
	}

	public bool ContainsParameter(string parameterName)
	{
		for (int i = 0; i < _characterParameters.Count; i++)
		{
			if (_characterParameters[i].Name == parameterName)
			{
				return true;
			}
		}
		for (int j = 0; j < _itemParameters.Count; j++)
		{
			if (_itemParameters[j].Name == parameterName)
			{
				return true;
			}
		}
		return false;
	}

	public virtual string Serialize()
	{
		return JsonUtility.ToJson(new StandardEventStatusCurrentWrapper
		{
			CurrentPhase = CurrentPhase
		});
	}

	public virtual void Deserialize(string jsonData)
	{
		CurrentPhase = JsonUtility.FromJson<StandardEventStatusCurrentWrapper>(jsonData).CurrentPhase;
	}

	public void Register()
	{
		if ((bool)_saveEvent)
		{
			_saveEvent.RegisterListener(this);
		}
	}

	public void Unregister()
	{
		if ((bool)_saveEvent)
		{
			_saveEvent.UnregisterListener(this);
		}
	}

	public abstract void ResetData();
}
