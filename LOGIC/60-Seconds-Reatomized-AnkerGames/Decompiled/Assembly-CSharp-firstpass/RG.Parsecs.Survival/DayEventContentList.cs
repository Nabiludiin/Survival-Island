using System;
using System.Collections.Generic;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class DayEventContentList
{
	[SerializeField]
	private List<EventContentData> _dayEventContentList = new List<EventContentData>();

	[SerializeField]
	private int _playerChoice;

	public List<EventContentData> EventContentList => _dayEventContentList;

	public int PlayerChoice => _playerChoice;

	public void Add(EventContentData eventContentData)
	{
		_dayEventContentList.Add(eventContentData);
	}

	public void SetPlayerChoice(int choice)
	{
		_playerChoice = choice;
	}

	public DayEventContentList()
	{
	}

	public DayEventContentList(DayEventContentListWrapper wrapper, SaveEvent saveEvent)
	{
		_playerChoice = wrapper.PlayerChoice;
		_dayEventContentList = new List<EventContentData>();
		for (int i = 0; i < wrapper.DayList.Count; i++)
		{
			_dayEventContentList.Add(new EventContentData(wrapper.DayList[i], saveEvent));
		}
	}

	public DayEventContentListWrapper Save()
	{
		DayEventContentListWrapper result = new DayEventContentListWrapper
		{
			PlayerChoice = _playerChoice,
			DayList = new List<EventContentData.EventContentDataWrapper>()
		};
		for (int i = 0; i < _dayEventContentList.Count; i++)
		{
			result.DayList.Add(_dayEventContentList[i].Save());
		}
		return result;
	}
}
