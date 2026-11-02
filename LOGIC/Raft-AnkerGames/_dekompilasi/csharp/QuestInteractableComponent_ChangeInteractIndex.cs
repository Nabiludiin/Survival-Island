public class QuestInteractableComponent_ChangeInteractIndex : QuestInteractableComponentBase<QuestInteractable_ComponentData_ChangeInteractIndex>
{
	protected override void Start()
	{
		base.Start();
	}

	protected override void FinishInteractionWithData(QuestInteractable_ComponentData_ChangeInteractIndex data, Network_Player player, bool atStart, bool successfull)
	{
		base.FinishInteractionWithData(data, player, atStart, successfull);
		if (data.target != null)
		{
			data.target.SetNewObjectStateIndex(data.newInteractIndex);
		}
		else
		{
			interactable.SetNewObjectStateIndex(data.newInteractIndex);
		}
	}
}
