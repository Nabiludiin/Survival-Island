public class QuestInteractable_LaserChallenge_Lever : QuestInteractable
{
	public ReflectiveRaycast lineCaster;

	protected override void Start()
	{
		base.Start();
		if (currentObjectStateIndex == 1)
		{
			lineCaster.SetCastingActive(value: true);
		}
	}

	public override void Interact(Network_Player player, bool successFull)
	{
		base.Interact(player, successFull);
		if (successFull)
		{
			lineCaster.SetCastingActive(value: true);
		}
	}
}
