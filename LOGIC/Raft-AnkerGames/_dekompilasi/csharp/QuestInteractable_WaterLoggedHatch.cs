using UnityEngine;

public class QuestInteractable_WaterLoggedHatch : QuestInteractable
{
	public LandmarkItem_WaterLoggedHatch hatch;

	[SerializeField]
	private QuestType questToCompleteOnDone = QuestType.TangaroaFloodStarted;

	protected override void Start()
	{
		base.Start();
	}

	public override void Interact(Network_Player player, bool successFull)
	{
		base.Interact(player, successFull);
		if (successFull)
		{
			QuestProgressTracker.FinishQuestRPC(questToCompleteOnDone);
			hatch.HatchInteract();
		}
	}

	public override void ResetFromCheat()
	{
		base.ResetFromCheat();
		hatch.fillComponent.SetNormalizedValue(0f, instant: true);
		hatch.ladderCollider.transform.position = hatch.defaultLadderPos;
	}
}
