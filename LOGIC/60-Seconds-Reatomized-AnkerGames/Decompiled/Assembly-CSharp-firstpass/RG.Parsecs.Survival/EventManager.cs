using System.Collections.Generic;
using I2.Loc;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

public abstract class EventManager : MonoBehaviour
{
	protected static EventManager eventManager;

	[SerializeField]
	[Header("Data")]
	[Tooltip("Reference to listener list that holds active decks.")]
	protected ActiveDecksListenerList _listenerList;

	[SerializeField]
	[Tooltip("Reference to End Of Day Listener List")]
	protected EndOfDayListenerList _endOfDayListenerList;

	[SerializeField]
	[Tooltip("Reference to survival data scriptable object.")]
	protected SurvivalData _survivalData;

	[SerializeField]
	[Tooltip("Reference to expedition data scriptable object.")]
	protected ExpeditionData _expeditionData;

	[SerializeField]
	protected ParsecsEvent _runtimeEvent;

	public ParsecsEvent RuntimeEvent
	{
		get
		{
			if (_runtimeEvent.CurrentPhase == EParsecsEventPhase.READY)
			{
				_runtimeEvent.Run();
			}
			return _runtimeEvent;
		}
	}

	public static List<Deck> ActiveDecks => eventManager._listenerList.ActiveDecks;

	public static EventManager Instance => eventManager;

	public static void RegisterDeckInListenerList(Deck deck)
	{
		if (deck is PlanetDeck)
		{
			if (eventManager._expeditionData.RuntimeData.CurrentPlanetDeck == null)
			{
				eventManager._expeditionData.RuntimeData.CurrentPlanetDeck = deck as PlanetDeck;
			}
			else
			{
				Debug.LogWarning("We're already on the planet. We can't land on another one.");
			}
		}
		eventManager._listenerList.RegisterDeckInListenerList(deck);
	}

	public static void UnregisterDeckFromListenerList(Deck deck)
	{
		if (deck is PlanetDeck)
		{
			if (eventManager._expeditionData.RuntimeData.CurrentPlanetDeck != null)
			{
				if (eventManager._expeditionData.RuntimeData.CurrentPlanetDeck == deck)
				{
					eventManager._expeditionData.RuntimeData.CurrentPlanetDeck = null;
				}
				else
				{
					Debug.LogWarning("We're trying to remove other planet deck than the planet we're on right now. This is not expected behaviour and should be debugged.");
				}
			}
			else
			{
				Debug.LogWarning("We're already on the planet. We can't land on another one.");
			}
		}
		eventManager._listenerList.UnregisterDeckListenerList(deck);
	}

	public static void UpdateEventInDecks(SurvivalEvent survivalEvent)
	{
		eventManager._listenerList.UpdateEventInDecks(survivalEvent);
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

	public static void DisplayGoalsUpdate(LocalizedString text, LocalizedString goalName)
	{
		eventManager.ShowGoalsUpdate(text, goalName);
	}

	public static ChoiceCardController GetPlayerChoice()
	{
		return eventManager.GetChoice();
	}

	protected abstract ChoiceCardController GetChoice();

	protected abstract void ShowGoalsUpdate(LocalizedString term, LocalizedString goalName);
}
