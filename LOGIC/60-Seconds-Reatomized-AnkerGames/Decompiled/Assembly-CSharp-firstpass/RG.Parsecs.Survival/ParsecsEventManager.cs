using System;
using System.Collections.Generic;
using I2.Loc;
using NodeEditorFramework;
using RG.Core.SaveSystem;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class ParsecsEventManager : EventManager, ISaveable
{
	private static ParsecsEventManager _instance;

	private const int RESET_LISTENER_LIST_PRIORITY = 25;

	[SerializeField]
	[Header("Scene References")]
	[Tooltip("Refrence to Event View.")]
	private EventView _eventView;

	[SerializeField]
	[Tooltip("Reference to Starlog View")]
	private StarlogView _starlogView;

	[SerializeField]
	[Tooltip("Refrence to Report Panel Controller.")]
	private ReportPanelController _reportPanelController;

	[SerializeField]
	[Space]
	[Header("Content Prefabs")]
	[Tooltip("Reference to Text Content prefab that will be used for displaying text fields in Event View.")]
	private TextContent _textContentPrefab;

	[SerializeField]
	[Tooltip("Reference to Top Text Content prefab that will be used for displaying top text field in Event View.")]
	private TopTextContent _topTextContentPrefab;

	[SerializeField]
	[Tooltip("Reference to Top Text Content prefab that will be used for displaying top text field in Event View.")]
	private SpriteContent _spriteContentPrefab;

	[SerializeField]
	[Tooltip("Reference to Text Content prefab that will be used for displaying text fields in Event View.")]
	private TextIconContent _textIconContentPrefab;

	[SerializeField]
	[Tooltip("Reference to Choice Summary Event Content prefab that will be used for displaying text fields in Event View.")]
	private ChoiceSummaryEventContent _choiceSummaryEventContent;

	[SerializeField]
	[Space]
	[Header("Choice Icons")]
	[Tooltip("Reference to the sprite that will be used to display Yes choice.")]
	private Sprite _yesIcon;

	[SerializeField]
	[Tooltip("Reference to the sprite that will be used to display No choice.")]
	private Sprite _noIcon;

	[SerializeField]
	[Tooltip("Reference to the sprite that will be used to display No Choice choice.")]
	private Sprite _noChoiceIcon;

	[SerializeField]
	private SaveEvent _saveEvent;

	private const string SAVE_ID = "MB_EventManager";

	[SerializeField]
	private List<DayEventContentList> _dayEventContentLists = new List<DayEventContentList>();

	[SerializeField]
	private int _reportLastDayCounter;

	public string ID => "MB_EventManager";

	public static Sprite YesIcon => _instance._yesIcon;

	public static Sprite NoIcon => _instance._noIcon;

	public static Sprite NoChoiceIcon => _instance._noChoiceIcon;

	public ChoiceCardController PlayerChoice => _eventView.GetPlayerChoice();

	public string Serialize()
	{
		EventManagerWrapper eventManagerWrapper = new EventManagerWrapper
		{
			ReportCount = _reportLastDayCounter,
			List = new List<DayEventContentListWrapper>()
		};
		for (int i = 0; i < _dayEventContentLists.Count; i++)
		{
			eventManagerWrapper.List.Add(_dayEventContentLists[i].Save());
		}
		return JsonUtility.ToJson(eventManagerWrapper);
	}

	public void Deserialize(string jsonData)
	{
		EventManagerWrapper eventManagerWrapper = JsonUtility.FromJson<EventManagerWrapper>(jsonData);
		_reportLastDayCounter = eventManagerWrapper.ReportCount;
		_dayEventContentLists = new List<DayEventContentList>();
		for (int i = 0; i < eventManagerWrapper.List.Count; i++)
		{
			_dayEventContentLists.Add(new DayEventContentList(eventManagerWrapper.List[i], _saveEvent));
		}
		_reportPanelController.ClearContents();
		_reportPanelController.SetDataAfterLoad();
		CreateStarlogPage(_survivalData.CurrentDay);
	}

	public void Register()
	{
		_saveEvent.RegisterListener(this);
	}

	public void Unregister()
	{
		_saveEvent.UnregisterListener(this);
	}

	public void ResetData()
	{
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			EventManager.eventManager = this;
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
		_endOfDayListenerList.RegisterOnEndOfDay(ResetCounter, "PreEventSystems", 90, this);
		_endOfDayListenerList.RegisterOnEndOfDay(_eventView.PrepareEventView, "PreEventSystems", 2, this);
		_endOfDayListenerList.RegisterOnEndOfDay(_eventView.SetPlayerChoice, "PreEventSystems", 1, this);
		_endOfDayListenerList.RegisterOnEndOfDay(_starlogView.PrepareStarlogView, "PreEventSystems", 3, this);
		_endOfDayListenerList.RegisterOnEndOfDay(_starlogView.UpdateDayCounter, "PrepareSystem", 3, this);
		_endOfDayListenerList.RegisterOnEndOfDay(_starlogView.SetNewDayTexts, "PrepareSystem", 4, this);
		_endOfDayListenerList.RegisterOnEndOfDay(OnEndOfDay, "Reset", 25, this);
		_endOfDayListenerList.RegisterOnDayStarted(_starlogView.OnDayStarted, "ChangeFinished", 0);
		_endOfDayListenerList.RegisterOnDayStarted(_starlogView.UpdateStatsAndGoalsLaye, "ChangeFinished", 10);
		Register();
	}

	private void OnDestroy()
	{
		_endOfDayListenerList.UnregisterOnEndOfDay(ResetCounter, "PreEventSystems");
		_endOfDayListenerList.UnregisterOnEndOfDay(_eventView.PrepareEventView, "PreEventSystems");
		_endOfDayListenerList.UnregisterOnEndOfDay(_eventView.SetPlayerChoice, "PreEventSystems");
		_endOfDayListenerList.UnregisterOnEndOfDay(_starlogView.PrepareStarlogView, "PreEventSystems");
		_endOfDayListenerList.UnregisterOnEndOfDay(_starlogView.SetNewDayTexts, "PrepareSystem");
		_endOfDayListenerList.UnregisterOnEndOfDay(OnEndOfDay, "Reset");
		_endOfDayListenerList.UnregisterOnDayStarted(_starlogView.OnDayStarted, "ChangeFinished");
		_endOfDayListenerList.UnregisterOnDayStarted(_starlogView.UpdateStatsAndGoalsLaye, "ChangeFinished");
		_endOfDayListenerList.UnregisterOnEndOfDay(_starlogView.UpdateDayCounter, "PrepareSystem");
		Unregister();
	}

	private void ResetCounter()
	{
		_reportLastDayCounter = 0;
	}

	private void OnEndOfDay()
	{
		if (_dayEventContentLists.Count <= _survivalData.CurrentDay)
		{
			_dayEventContentLists.Add(new DayEventContentList());
		}
	}

	private static void AddEventContentDataToDay(int day, EventContentData eventContentData)
	{
		if (day > _instance._dayEventContentLists.Count || _instance._dayEventContentLists[day - 1] == null)
		{
			_instance._dayEventContentLists.Insert(day - 1, new DayEventContentList());
		}
		_instance._dayEventContentLists[day - 1].Add(eventContentData);
	}

	private static EParsecsEventPhase GetCanvasPhase(NodeCanvas parentCanvas)
	{
		if (parentCanvas is ParsecsEvent)
		{
			return (parentCanvas as ParsecsEvent).CurrentPhase;
		}
		if (parentCanvas is NodeFunction)
		{
			return (parentCanvas as NodeFunction).GetOriginalCanvasAs<ParsecsEvent>().CurrentPhase;
		}
		if (parentCanvas is Goal)
		{
			return EParsecsEventPhase.REPORT;
		}
		throw new UnityException("This canvas type can only be executed from canvases that derive from ParsecsEvent or NodeFunctions");
	}

	public static void DisplayTextContent(NodeCanvas parentCanvas, EventContentData data, bool recreate = false)
	{
		DisplayTextContent(GetCanvasPhase(parentCanvas), data, recreate);
	}

	public static void DisplayTopTextContent(NodeCanvas parentCanvas, EventContentData data, bool recreate = false)
	{
		DisplayTopTextContent(GetCanvasPhase(parentCanvas), data, recreate);
	}

	public static void DisplaySprite(NodeCanvas parentCanvas, EventContentData data, bool recreate = false)
	{
		DisplaySprite(GetCanvasPhase(parentCanvas), data, recreate);
	}

	public static void DisplayTextContent(EParsecsEventPhase eventPhase, EventContentData data, bool recreate = false)
	{
		TextContent content = CreateTextContent(data);
		AddContentToView(eventPhase, content);
		if (!recreate)
		{
			AddEventContentDataToDay(_instance._survivalData.CurrentDay, data);
		}
	}

	public static void DisplayTopTextContent(EParsecsEventPhase eventPhase, EventContentData data, bool recreate = false)
	{
		TopTextContent content = CreateTopTextContent(data);
		AddContentToView(eventPhase, content);
		if (!recreate)
		{
			AddEventContentDataToDay(_instance._survivalData.CurrentDay, data);
		}
	}

	public static void DisplaySprite(EParsecsEventPhase eventPhase, EventContentData data, bool recreate = false)
	{
		SpriteContent content = CreateSpriteContent(data);
		AddContentToView(eventPhase, content);
		if (!recreate)
		{
			AddEventContentDataToDay(_instance._survivalData.CurrentDay, data);
		}
	}

	private static TextContent CreateTextContent(EventContentData eventContent)
	{
		TextContent component = UnityEngine.Object.Instantiate(_instance._textContentPrefab).GetComponent<TextContent>();
		if (string.IsNullOrEmpty(eventContent.Text) && !string.IsNullOrEmpty(eventContent.PureText))
		{
			component.SetText(eventContent.PureText);
		}
		else
		{
			component.SetText(eventContent.GetTranslatedText());
		}
		return component;
	}

	private static TopTextContent CreateTopTextContent(EventContentData eventContent)
	{
		TopTextContent component = UnityEngine.Object.Instantiate(_instance._topTextContentPrefab).GetComponent<TopTextContent>();
		if (string.IsNullOrEmpty(eventContent.Text) && !string.IsNullOrEmpty(eventContent.PureText))
		{
			component.SetText(eventContent.PureText);
		}
		else
		{
			component.SetText(eventContent.GetTranslatedText());
		}
		return component;
	}

	private static SpriteContent CreateSpriteContent(EventContentData eventContent)
	{
		SpriteContent component = UnityEngine.Object.Instantiate(_instance._spriteContentPrefab).GetComponent<SpriteContent>();
		component.SetSprite(eventContent.Sprite, eventContent.SpriteAlign);
		return component;
	}

	public static void DisplayTextIconContent(NodeCanvas parentCanvas, EventContentData.ETextIconContentType type, float amount, Resource resource, bool recreate = false)
	{
		DisplayTextIconContent(GetCanvasPhase(parentCanvas), type, amount, resource, recreate);
	}

	public static void DisplayTextIconContent(NodeCanvas parentCanvas, EventContentData.ETextIconContentType type, float amount, IItem item, bool recreate = false)
	{
		DisplayTextIconContent(GetCanvasPhase(parentCanvas), type, amount, item, recreate);
	}

	public static void DisplayTextIconContent(NodeCanvas parentCanvas, bool damaged, IItem item, bool recreate = false)
	{
		DisplayTextIconContent(GetCanvasPhase(parentCanvas), damaged, item, recreate);
	}

	public static void DisplayTextIconContent(EParsecsEventPhase eventPhase, EventContentData.ETextIconContentType type, float amount, IItem item, bool recreate = false)
	{
		TextIconContent content = CreateTextIconContent(type, amount, item);
		AddContentToView(eventPhase, content);
		if (!recreate)
		{
			AddEventContentDataToDay(_instance._survivalData.CurrentDay, new EventContentData(EventContentData.EEventContentType.TEXT_ICON, type, amount, item));
		}
	}

	public static void DisplayTextIconContent(EParsecsEventPhase eventPhase, bool damaged, IItem item, bool recreate = false)
	{
		TextIconContent content = CreateTextIconContent(damaged, item);
		AddContentToView(eventPhase, content);
		if (!recreate)
		{
			AddEventContentDataToDay(_instance._survivalData.CurrentDay, new EventContentData(EventContentData.EEventContentType.TEXT_ICON, EventContentData.ETextIconContentType.NONE, damaged, item));
		}
	}

	public static void DisplayTextIconContent(EParsecsEventPhase eventPhase, EventContentData.ETextIconContentType type, float amount, Resource resource, bool recreate = false)
	{
		TextIconContent content = CreateTextIconContent(type, amount, resource);
		AddContentToView(eventPhase, content);
		if (!recreate)
		{
			AddEventContentDataToDay(_instance._survivalData.CurrentDay, new EventContentData(EventContentData.EEventContentType.TEXT_ICON, type, amount, resource));
		}
	}

	private static TextIconContent CreateTextIconContent(EventContentData.ETextIconContentType type, float amount, Resource resource)
	{
		TextIconContent component = UnityEngine.Object.Instantiate(_instance._textIconContentPrefab).GetComponent<TextIconContent>();
		component.SetTextAndIcon(type, amount, resource);
		return component;
	}

	private static TextIconContent CreateTextIconContent(EventContentData.ETextIconContentType type, float amount, IItem item)
	{
		TextIconContent component = UnityEngine.Object.Instantiate(_instance._textIconContentPrefab).GetComponent<TextIconContent>();
		component.SetTextAndIcon(type, amount, item);
		return component;
	}

	private static TextIconContent CreateTextIconContent(bool damaged, IItem item)
	{
		TextIconContent component = UnityEngine.Object.Instantiate(_instance._textIconContentPrefab).GetComponent<TextIconContent>();
		component.SetTextAndIcon(damaged, item);
		return component;
	}

	private static void AddContentToView(EParsecsEventPhase eventPhase, EventContent content)
	{
		switch (eventPhase)
		{
		case EParsecsEventPhase.ACTION:
			_instance._eventView.AddContent(content);
			break;
		case EParsecsEventPhase.REPORT:
			_instance._reportLastDayCounter++;
			_instance._reportPanelController.AddContent(content);
			break;
		case EParsecsEventPhase.FINISHED:
			break;
		}
	}

	public static void DisplayChoiceContent()
	{
		_instance._eventView.SetChoiceContent();
		AddEventContentDataToDay(_instance._survivalData.CurrentDay, new EventContentData(EventContentData.EEventContentType.CHOICE, new List<Sprite> { null, null, YesIcon, NoIcon }));
	}

	public static void DisplayChoiceContent(Character character, CharacterAttribute attribute1, CharacterAttribute attribute2 = null, CharacterAttribute attribute3 = null, CharacterAttribute attribute4 = null)
	{
		_instance._eventView.SetChoiceContent(character, attribute1, attribute2, attribute3, attribute4);
		AddEventContentDataToDay(_instance._survivalData.CurrentDay, new EventContentData(EventContentData.EEventContentType.CHOICE, new List<Sprite>
		{
			attribute1 ? attribute1.Icon : null,
			attribute2 ? attribute2.Icon : null,
			attribute3 ? attribute3.Icon : null,
			attribute4 ? attribute4.Icon : null
		}));
	}

	public static void DisplayChoiceContent(bool useResource, GameResource resource1, GameResource resource2 = null, GameResource resource3 = null)
	{
		_instance._eventView.SetChoiceContent(useResource, resource1, resource2, resource3);
		AddEventContentDataToDay(_instance._survivalData.CurrentDay, new EventContentData(EventContentData.EEventContentType.CHOICE, new List<Sprite>
		{
			resource1.Resource ? resource1.Resource.Icon : null,
			resource2.Resource ? resource2.Resource.Icon : null,
			resource3.Resource ? resource3.Resource.Icon : null,
			NoChoiceIcon
		}));
	}

	public static void DisplayChoiceContent(IItem item1, IItem item2 = null, IItem item3 = null, IItem item4 = null)
	{
		_instance._eventView.SetChoiceContent(item1, item2, item3, item4);
		AddEventContentDataToDay(_instance._survivalData.CurrentDay, new EventContentData(EventContentData.EEventContentType.CHOICE, new List<Sprite>
		{
			item1 ? item1.BaseStaticData.IconJournal : null,
			item2 ? item2.BaseStaticData.IconJournal : null,
			item3 ? item3.BaseStaticData.IconJournal : null,
			NoChoiceIcon
		}));
	}

	public static void DisplayChoiceContent(Character character1, Character character2 = null, Character character3 = null, Character character4 = null)
	{
		_instance._eventView.SetChoiceContent(character1, character2, character3, character4);
		AddEventContentDataToDay(_instance._survivalData.CurrentDay, new EventContentData(EventContentData.EEventContentType.CHOICE, new List<Sprite>
		{
			character1 ? character1.StaticData.Icons.JournalIcon : null,
			character2 ? character2.StaticData.Icons.JournalIcon : null,
			character3 ? character3.StaticData.Icons.JournalIcon : null,
			character4 ? character4.StaticData.Icons.JournalIcon : null
		}));
	}

	public static void DisplayChoiceContent(BaseActionCondition choice1, BaseActionCondition choice2 = null, BaseActionCondition choice3 = null, BaseActionCondition choice4 = null)
	{
		_instance._eventView.SetChoiceContent(choice1, choice2, choice3, choice4);
		AddEventContentDataToDay(_instance._survivalData.CurrentDay, new EventContentData(EventContentData.EEventContentType.CHOICE, new List<Sprite>
		{
			choice1 ? choice1.Sprite : null,
			choice2 ? choice2.Sprite : null,
			choice3 ? choice3.Sprite : null,
			choice4 ? choice4.Sprite : null
		}));
	}

	public static void SetDaysChoiceForStarlog(int choice)
	{
		if (!(_instance == null) && _instance._dayEventContentLists != null && _instance._dayEventContentLists.Count != 0)
		{
			_instance._dayEventContentLists[_instance._survivalData.CurrentDay - 1].SetPlayerChoice(choice);
		}
	}

	public static void CreateStarlogPage(int day)
	{
		int num = ((day == _instance._survivalData.CurrentDay) ? _instance._reportLastDayCounter : _instance._dayEventContentLists[day - 1].EventContentList.Count);
		for (int i = 0; i < num; i++)
		{
			EventContentData eventContentData = _instance._dayEventContentLists[day - 1].EventContentList[i];
			switch (_instance._dayEventContentLists[day - 1].EventContentList[i].ContentType)
			{
			case EventContentData.EEventContentType.TEXT:
				DisplayTextContent(EParsecsEventPhase.REPORT, _instance._dayEventContentLists[day - 1].EventContentList[i], recreate: true);
				break;
			case EventContentData.EEventContentType.TOP_TEXT:
				DisplayTopTextContent(EParsecsEventPhase.REPORT, _instance._dayEventContentLists[day - 1].EventContentList[i], recreate: true);
				break;
			case EventContentData.EEventContentType.TEXT_ICON:
				if (eventContentData.TextIconType == EventContentData.ETextIconContentType.NONE)
				{
					DisplayTextIconContent(EParsecsEventPhase.REPORT, eventContentData.Damaged, eventContentData.Item, recreate: true);
				}
				else if (eventContentData.Item != null)
				{
					DisplayTextIconContent(EParsecsEventPhase.REPORT, eventContentData.TextIconType, eventContentData.Amount, eventContentData.Item, recreate: true);
				}
				else if ((bool)eventContentData.Resource)
				{
					DisplayTextIconContent(EParsecsEventPhase.REPORT, eventContentData.TextIconType, eventContentData.Amount, eventContentData.Resource, recreate: true);
				}
				break;
			case EventContentData.EEventContentType.SPRITE:
				DisplaySprite(EParsecsEventPhase.REPORT, _instance._dayEventContentLists[day - 1].EventContentList[i], recreate: true);
				break;
			case EventContentData.EEventContentType.CHOICE:
				DisplayChoiceSummaryContent(_instance._dayEventContentLists[day - 1].EventContentList[i].ChoiceSprites, _instance._dayEventContentLists[day - 1].PlayerChoice);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}

	private static void DisplayChoiceSummaryContent(Sprite[] choiceSprites, int choice)
	{
		ChoiceSummaryEventContent content = CreateChoiceSummaryContent(choiceSprites, choice);
		AddContentToView(EParsecsEventPhase.REPORT, content);
	}

	private static ChoiceSummaryEventContent CreateChoiceSummaryContent(Sprite[] choiceSprites, int choice)
	{
		ChoiceSummaryEventContent component = UnityEngine.Object.Instantiate(_instance._choiceSummaryEventContent).GetComponent<ChoiceSummaryEventContent>();
		component.SetImages(choiceSprites, choice);
		return component;
	}

	protected override ChoiceCardController GetChoice()
	{
		return _instance._eventView.GetPlayerChoice();
	}

	protected override void ShowGoalsUpdate(LocalizedString term, LocalizedString goalName)
	{
		EventContentData eventContentData = null;
		eventContentData = (((string)goalName != null && !string.IsNullOrEmpty(goalName.mTerm)) ? new EventContentData(EventContentData.EEventContentType.TOP_TEXT, term, "{[GOAL_NAME]}", goalName) : new EventContentData(EventContentData.EEventContentType.TOP_TEXT, term));
		DisplayTopTextContent(EParsecsEventPhase.REPORT, eventContentData);
	}
}
