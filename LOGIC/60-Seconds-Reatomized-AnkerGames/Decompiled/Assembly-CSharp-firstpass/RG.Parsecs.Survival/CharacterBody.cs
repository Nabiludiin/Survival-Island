using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class CharacterBody : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	public bool Refresh = true;

	[Tooltip("The base BodyPartType.\nOther Body Part Types should\nbe created as children of the objects\nwith this Body Part Type")]
	[SerializeField]
	private BodyPartType _baseBodyPartType;

	[SerializeField]
	private BodyPartType _blinkingBodyPartType;

	[Tooltip("Should not contain the base and blinking Body Part Types.")]
	[SerializeField]
	private BodyPartType[] _supportedBodyPartTypes;

	[Tooltip("If a character has one or more of these statuses, the base body part to be activated has to require at least one of them")]
	[SerializeField]
	private List<CharacterStatus> _priorityCharacterStatuses = new List<CharacterStatus>();

	private List<CharacterBodyPart> _baseBodyParts = new List<CharacterBodyPart>();

	private List<CharacterBodyPart> _activeParts = new List<CharacterBodyPart>();

	private CharacterBodyPart _activeBaseBodyPart;

	private List<CharacterBodyPart> _bodyPartList = new List<CharacterBodyPart>();

	private Dictionary<BodyPartType, List<CharacterBodyPart>> _bodyParts = new Dictionary<BodyPartType, List<CharacterBodyPart>>();

	[SerializeField]
	private float _minBlinkingInterval = 10f;

	[SerializeField]
	private float _maxBlinkingInterval = 60f;

	[SerializeField]
	private float _minBlinkDuration = 0.5f;

	[SerializeField]
	private float _maxBlinkDuration = 1.5f;

	private bool _areEyelidsVisible;

	private bool _isBodyVisible;

	[SerializeField]
	private bool _layerSet;

	[SerializeField]
	private string _layerName;

	private void Start()
	{
		UpdateDisplayedBody();
		StartCoroutine(Blink());
	}

	private void Update()
	{
		if (Refresh)
		{
			Refresh = false;
			UpdateDisplayedBody();
		}
	}

	public void UpdateDisplayedCharacter()
	{
		UpdateDisplayedBody();
	}

	private IEnumerator Blink()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(_minBlinkDuration, _maxBlinkDuration));
			HideAllBlinkingBodyparts();
			yield return new WaitForSeconds(Random.Range(_minBlinkingInterval, _maxBlinkingInterval));
			ShowBestMatchBlinkingBodypart();
		}
	}

	private void ShowBestMatchBlinkingBodypart()
	{
		if (_isBodyVisible)
		{
			_areEyelidsVisible = true;
			ShowBodyParts(_blinkingBodyPartType);
		}
	}

	private void HideAllBlinkingBodyparts()
	{
		_areEyelidsVisible = false;
		for (int i = 0; i < _activeParts.Count; i++)
		{
			if (_activeParts[i].BodyPartType == _blinkingBodyPartType)
			{
				HideBodyPart(_activeParts[i]);
			}
		}
	}

	public void SetBodySortingLayer(string layerName)
	{
		_layerSet = true;
		_layerName = layerName;
		for (int i = 0; i < _bodyPartList.Count; i++)
		{
			_bodyPartList[i].Handle.GetComponent<SpriteRenderer>().sortingLayerName = _layerName;
		}
	}

	public void AddBodyPart(CharacterBodyPart bodyPart)
	{
		if (_layerSet)
		{
			bodyPart.Handle.GetComponent<SpriteRenderer>().sortingLayerName = _layerName;
		}
		if (bodyPart.BodyPartType.Equals(_baseBodyPartType))
		{
			_baseBodyParts.Add(bodyPart);
		}
		else
		{
			if (!_bodyParts.ContainsKey(bodyPart.BodyPartType))
			{
				_bodyParts.Add(bodyPart.BodyPartType, new List<CharacterBodyPart>());
			}
			_bodyParts[bodyPart.BodyPartType].Add(bodyPart);
		}
		_bodyPartList.Add(bodyPart);
	}

	private void UpdateDisplayedBody()
	{
		HideBody();
		if (_character.RuntimeData.IsDrawnOnShip())
		{
			_isBodyVisible = true;
			ActivateBestMatchBaseBodyPart();
			for (int i = 0; i < _supportedBodyPartTypes.Length; i++)
			{
				ShowBodyParts(_supportedBodyPartTypes[i]);
			}
			if (_areEyelidsVisible)
			{
				ShowBestMatchBlinkingBodypart();
			}
		}
	}

	private void HideBody()
	{
		_isBodyVisible = false;
		DeactivateBaseBodyPart();
		for (int i = 0; i < _activeParts.Count; i++)
		{
			_activeParts[i].Handle.SetActive(value: false);
		}
		_activeParts.Clear();
	}

	private void DeactivateBaseBodyPart()
	{
		if (_activeBaseBodyPart != null)
		{
			_activeBaseBodyPart.Handle.SetActive(value: false);
			_activeBaseBodyPart = null;
		}
	}

	private void ActivateBestMatchBaseBodyPart()
	{
		for (int i = 0; i < _baseBodyParts.Count; i++)
		{
			if (!_character.RuntimeData.IsAlive())
			{
				for (int j = 0; j < _baseBodyParts[i].RequiredStatuses.Length; j++)
				{
					if (_baseBodyParts[i].RequiredStatuses[j].IsDead)
					{
						UpdateActiveBaseBodyPartIfBetterMatchThanCurrent(i);
					}
				}
			}
			else if (IsBaseBodyPartAllowedByPriorityStatuses(i) && IsBaseBodyPartRequirementListMet(i))
			{
				UpdateActiveBaseBodyPartIfBetterMatchThanCurrent(i);
			}
		}
		if (_activeBaseBodyPart != null)
		{
			_activeBaseBodyPart.Handle.SetActive(value: true);
		}
	}

	private bool IsBaseBodyPartRequirementListMet(int baseBodyPartIndex)
	{
		return !_baseBodyParts[baseBodyPartIndex].RequiresStatusNotInList(_character.RuntimeData.CurrentStatuses);
	}

	private bool IsBaseBodyPartAllowedByPriorityStatuses(int baseBodyPartIndex)
	{
		if (CharacterHasPriorityStatus())
		{
			return _baseBodyParts[baseBodyPartIndex].GetIntersectionCount(_priorityCharacterStatuses) > 0;
		}
		return true;
	}

	private void UpdateActiveBaseBodyPartIfBetterMatchThanCurrent(int baseBodyPartIndex)
	{
		if (_activeBaseBodyPart == null || _activeBaseBodyPart.GetIntersectionCount(_character.RuntimeData.CurrentStatuses) < _baseBodyParts[baseBodyPartIndex].GetIntersectionCount(_character.RuntimeData.CurrentStatuses))
		{
			_activeBaseBodyPart = _baseBodyParts[baseBodyPartIndex];
		}
	}

	private bool CharacterHasPriorityStatus()
	{
		for (int i = 0; i < _priorityCharacterStatuses.Count; i++)
		{
			if (_character.RuntimeData.HasStatus(_priorityCharacterStatuses[i].Id))
			{
				return true;
			}
		}
		return false;
	}

	private void ShowBodyParts(BodyPartType type)
	{
		CharacterBodyPart characterBodyPart = null;
		bool flag = false;
		if (!_bodyParts.ContainsKey(type))
		{
			return;
		}
		for (int i = 0; i < _bodyParts[type].Count; i++)
		{
			if (_bodyParts[type][i].Handle.transform.parent == _activeBaseBodyPart.Handle.transform && !_bodyParts[type][i].RequiresStatusNotInList(_character.RuntimeData.CurrentStatuses))
			{
				flag = true;
				if (type.MultipleActiveAllowed)
				{
					ShowBodyPart(_bodyParts[type][i]);
				}
				else if (characterBodyPart == null || characterBodyPart.GetIntersectionCount(_character.RuntimeData.CurrentStatuses) < _bodyParts[type][i].GetIntersectionCount(_character.RuntimeData.CurrentStatuses))
				{
					characterBodyPart = _bodyParts[type][i];
				}
			}
		}
		if (type.IsMandatory && !flag)
		{
			Debug.LogErrorFormat("No matching parts of mandatory bodypart type {0} found in {1}!", type, gameObject.name);
		}
		if (characterBodyPart != null)
		{
			ShowBodyPart(characterBodyPart);
		}
	}

	private void ShowBodyPart(CharacterBodyPart bodyPart)
	{
		if (bodyPart != null)
		{
			bodyPart.Handle.SetActive(value: true);
			_activeParts.Add(bodyPart);
		}
	}

	private void HideBodyPart(CharacterBodyPart bodyPart)
	{
		if (bodyPart != null)
		{
			bodyPart.Handle.SetActive(value: false);
			_activeParts.Remove(bodyPart);
		}
	}
}
