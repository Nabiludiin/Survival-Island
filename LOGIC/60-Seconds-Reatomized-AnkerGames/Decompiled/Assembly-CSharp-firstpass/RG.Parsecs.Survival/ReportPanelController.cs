using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class ReportPanelController : StarlogPanelController
{
	[SerializeField]
	[Tooltip("Transform that will be used as parent for all content generated from events.")]
	private Transform _contentHolder;

	[SerializeField]
	[Tooltip("Transform that will be used as parent for all content generated from events.")]
	private GameObject _dayPrefab;

	[SerializeField]
	[Tooltip("Prefab of object that will be set as parent of text icon contents.")]
	private GameObject _textIconGroup;

	[SerializeField]
	[Tooltip("Transform that will be used as parent for all content generated from events.")]
	private DayInfoController _dayInfoController;

	[SerializeField]
	[Tooltip("Reference to Survival Data scriptable object.")]
	private SurvivalData _survivalData;

	[SerializeField]
	private TopTextContent _topTextContent;

	private List<EventContentList> _currentContent = new List<EventContentList>();

	private Transform _currentTextIconGroup;

	private const string TEXT_ICON_GROUP_TAG = "TextIconGroup";

	public void AddContent(EventContent content)
	{
		int pageIndex = GetPageIndex(_dayInfoController.DayToDisplay);
		if (pageIndex == -1)
		{
			GameObject gameObject = Object.Instantiate(_dayPrefab);
			gameObject.name = "Day " + _dayInfoController.DayToDisplay;
			gameObject.transform.SetParent(_contentHolder, worldPositionStays: false);
			EventContentList eventContentList = new EventContentList(_dayInfoController.DayToDisplay, gameObject.transform);
			if (content is TopTextContent)
			{
				eventContentList.Add(content);
			}
			else
			{
				TopTextContent content2 = Object.Instantiate(_topTextContent, gameObject.transform);
				eventContentList.Add(content2);
			}
			if (content is TextIconContent)
			{
				eventContentList.Add(null);
			}
			eventContentList.Add(content);
			_currentContent.Add(eventContentList);
			SetContentParent(content, gameObject.transform, _currentContent.Count - 1);
			return;
		}
		TextIconContent textIconContent = _currentContent[pageIndex].GetLastContent() as TextIconContent;
		TextIconContent textIconContent2 = content as TextIconContent;
		TopTextContent topTextContent = content as TopTextContent;
		TopTextContent topTextContent2 = _currentContent[pageIndex].GetLastContent() as TopTextContent;
		if (topTextContent != null)
		{
			TopTextContent topTextContent3 = _currentContent[pageIndex].EventContents[0] as TopTextContent;
			if (topTextContent3 != null)
			{
				topTextContent3.AppendText(topTextContent.CurrentText);
			}
			Object.Destroy(content.gameObject);
		}
		else
		{
			if (textIconContent != null && textIconContent2 != null && SumDisplayAmountOfTextIconContents(textIconContent, textIconContent2))
			{
				return;
			}
			if (textIconContent2 != null && topTextContent2 != null && _currentContent[pageIndex].EventContents.Count > 2)
			{
				textIconContent = _currentContent[pageIndex].GetSecondContentFromEnd() as TextIconContent;
				if (textIconContent != null && SumDisplayAmountOfTextIconContents(textIconContent, textIconContent2))
				{
					return;
				}
			}
			SetContentParent(content, _currentContent[pageIndex].Holder, pageIndex);
			if (content is TextContent && _currentContent[pageIndex].Holder.transform.childCount == 3)
			{
				Transform child = _currentContent[pageIndex].Holder.transform.GetChild(1);
				if (child.CompareTag("TextIconGroup"))
				{
					child.SetAsLastSibling();
					_currentContent[pageIndex].Set(1, content);
				}
			}
			_currentContent[pageIndex].Add(content);
		}
	}

	private bool SumDisplayAmountOfTextIconContents(TextIconContent lastContent, TextIconContent currentTextIconContent)
	{
		if (lastContent.Type != currentTextIconContent.Type)
		{
			return false;
		}
		bool result = false;
		if (currentTextIconContent.Item != null)
		{
			if (lastContent.Item == currentTextIconContent.Item)
			{
				float amount = lastContent.Amount + currentTextIconContent.Amount;
				lastContent.SetTextAndIcon(lastContent.Type, amount, lastContent.Item);
				result = true;
			}
		}
		else if (currentTextIconContent.Resource != null && lastContent.Resource == currentTextIconContent.Resource)
		{
			float amount2 = lastContent.Amount + currentTextIconContent.Amount;
			lastContent.SetTextAndIcon(lastContent.Type, amount2, lastContent.Resource);
			result = true;
		}
		return result;
	}

	private void SetContentParent(EventContent content, Transform parent, int dayIndex)
	{
		if (content is TextIconContent)
		{
			if (_currentTextIconGroup == null)
			{
				_currentTextIconGroup = Object.Instantiate(_textIconGroup, parent).transform;
			}
			content.SetParent(_currentTextIconGroup);
		}
		else if (content is TopTextContent && _currentContent[dayIndex].GetLastContent() is TextIconContent)
		{
			content.SetParent(parent);
		}
		else
		{
			if ((bool)_currentTextIconGroup)
			{
				_currentTextIconGroup = null;
			}
			content.SetParent(parent);
		}
	}

	public void DisplayPage(int dayToDisplay, int dayToHide)
	{
		int pageIndex = GetPageIndex(dayToDisplay);
		int pageIndex2 = GetPageIndex(dayToHide);
		if (pageIndex2 >= 0)
		{
			HidePage(pageIndex2);
		}
		if (pageIndex < 0)
		{
			ParsecsEventManager.CreateStarlogPage(dayToDisplay);
		}
		else
		{
			ShowPage(pageIndex);
		}
	}

	private int GetPageIndex(int day)
	{
		for (int i = 0; i < _currentContent.Count; i++)
		{
			if (_currentContent[i].Day == day)
			{
				return i;
			}
		}
		return -1;
	}

	private void HidePage(int pageIndex)
	{
		_currentContent[pageIndex].Holder.gameObject.SetActive(value: false);
	}

	private void ShowPage(int pageIndex)
	{
		_currentContent[pageIndex].Holder.gameObject.SetActive(value: true);
	}

	public void ClearContents()
	{
		for (int i = 0; i < _currentContent.Count; i++)
		{
			Object.Destroy(_currentContent[i].Holder.gameObject);
		}
		_currentContent.Clear();
	}

	public void SetDataAfterLoad()
	{
		_dayInfoController.UpdateDayCounter();
		_dayInfoController.SetDayTextToCurrentDay();
	}
}
