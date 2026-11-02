using UnityEngine;

namespace RG.Parsecs.Survival;

public class CrewPanelController : StarlogPanelController
{
	[SerializeField]
	private StarlogCrewMemberController[] _crewMemberControllers;

	[SerializeField]
	private Transform[] _separators;

	[SerializeField]
	private CharacterList _characterList;

	private Transform[] _crewMemberTransforms = new Transform[4];

	private const int MAX_CHARACTER_COUNT = 4;

	private int _startDeadIndex;

	private void Awake()
	{
		UpdateCrewPanel();
	}

	public void UpdateCrewPanel()
	{
		_startDeadIndex = 3;
		for (int i = 0; i < _crewMemberControllers.Length; i++)
		{
			if (i < _characterList.GetCharacterCount())
			{
				_crewMemberControllers[i].gameObject.SetActive(value: true);
				_crewMemberControllers[i].OnShow();
			}
			else
			{
				_crewMemberControllers[i].gameObject.SetActive(value: false);
				_crewMemberTransforms[_startDeadIndex] = _crewMemberControllers[i].transform;
				_startDeadIndex--;
			}
		}
		SortByDead();
	}

	private void SortByDead()
	{
		int num = 0;
		int num2 = _startDeadIndex;
		for (int i = 0; i < _crewMemberControllers.Length; i++)
		{
			if (i < _characterList.GetCharacterCount() && _crewMemberControllers[i].IsCharacterAlive())
			{
				_crewMemberTransforms[num] = _crewMemberControllers[i].transform;
				num++;
			}
			else if (i < _characterList.GetCharacterCount())
			{
				_crewMemberTransforms[num2] = _crewMemberControllers[i].transform;
				num2--;
			}
		}
		int num3 = 0;
		int num4 = 0;
		for (int j = 0; j < _crewMemberTransforms.Length * 2 - 1; j++)
		{
			if (j % 2 == 0)
			{
				_crewMemberTransforms[num3].SetSiblingIndex(j);
				num3++;
			}
			else
			{
				_separators[num4].SetSiblingIndex(j);
				_separators[num4].gameObject.SetActive(num4 + 1 < _characterList.CharactersInGame.Count);
				num4++;
			}
		}
	}
}
