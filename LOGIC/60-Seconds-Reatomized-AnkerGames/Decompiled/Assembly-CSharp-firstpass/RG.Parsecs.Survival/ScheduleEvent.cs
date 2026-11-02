using System;
using System.Collections.Generic;
using RG.Core.SaveSystem;
using RG.Parsecs.EventEditor;

namespace RG.Parsecs.Survival;

[Serializable]
public class ScheduleEvent
{
	public ParsecsEvent Event;

	public List<CharacterParameter> Characters = new List<CharacterParameter>();

	public List<ItemParameter> Items = new List<ItemParameter>();

	public ScheduleEvent(ParsecsEvent e)
	{
		Event = e;
		Characters = new List<CharacterParameter>();
		Items = new List<ItemParameter>();
	}

	public ScheduleEvent(ScheduleEventWrapper eventWrapper, SaveEvent saveEvent)
	{
		if (!string.IsNullOrEmpty(eventWrapper.EventName))
		{
			Event = (ParsecsEvent)saveEvent.GetReferenceObjectByID(eventWrapper.EventName);
		}
		Characters = new List<CharacterParameter>();
		for (int i = 0; i < eventWrapper.CharacterParameter.Count; i++)
		{
			CharacterParameter item = new CharacterParameter
			{
				Character = (Character)saveEvent.GetReferenceObjectByID(eventWrapper.CharacterParameter[i].ObjectID),
				Name = eventWrapper.CharacterParameter[i].Name
			};
			Characters.Add(item);
		}
		Items = new List<ItemParameter>();
		for (int j = 0; j < eventWrapper.ItemParameter.Count; j++)
		{
			ItemParameter item2 = new ItemParameter
			{
				Item = (IItem)saveEvent.GetReferenceObjectByID(eventWrapper.ItemParameter[j].ObjectID),
				Name = eventWrapper.ItemParameter[j].Name
			};
			Items.Add(item2);
		}
	}

	public ScheduleEventWrapper Save()
	{
		ScheduleEventWrapper result = new ScheduleEventWrapper
		{
			EventName = ((Event != null) ? Event.ID : string.Empty),
			CharacterParameter = new List<EventParameterWrapper>()
		};
		if (Characters != null)
		{
			for (int i = 0; i < Characters.Count; i++)
			{
				result.CharacterParameter.Add(new EventParameterWrapper
				{
					Name = Characters[i].Name,
					ObjectID = Characters[i].Character.ID
				});
			}
		}
		result.ItemParameter = new List<EventParameterWrapper>();
		if (Items != null)
		{
			for (int j = 0; j < Items.Count; j++)
			{
				result.ItemParameter.Add(new EventParameterWrapper
				{
					Name = Items[j].Name,
					ObjectID = Items[j].Item.BaseStaticData.ItemId
				});
			}
		}
		return result;
	}

	public void AddCharacterParameter(CharacterParameter characterParameter)
	{
		if (Characters == null)
		{
			Characters = new List<CharacterParameter>();
		}
		Characters.Add(characterParameter);
	}

	public void AddItemParameter(ItemParameter itemParameter)
	{
		if (Items == null)
		{
			Items = new List<ItemParameter>();
		}
		Items.Add(itemParameter);
	}

	public void RunEventWithScheduledParameters()
	{
		if (Characters != null)
		{
			for (int i = 0; i < Characters.Count; i++)
			{
				Event.SetCharacterParameterByName(Characters[i].Name, Characters[i].Character);
			}
		}
		if (Items != null)
		{
			for (int j = 0; j < Items.Count; j++)
			{
				Event.SetItemParameterByName(Items[j].Name, Items[j].Item);
			}
		}
		if (Event != null)
		{
			Event.Run();
		}
	}
}
