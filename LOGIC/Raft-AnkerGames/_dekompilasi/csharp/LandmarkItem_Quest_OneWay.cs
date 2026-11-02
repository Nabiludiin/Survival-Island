public class LandmarkItem_Quest_OneWay : LandmarkItem_Quest
{
	public QuestInteractable_OneWay oneWayInteractableComponent;

	public override RGD_LandmarkItem GetSave()
	{
		return new RGD_LandmarkItem_QuestEvent_OneWay(this);
	}
}
