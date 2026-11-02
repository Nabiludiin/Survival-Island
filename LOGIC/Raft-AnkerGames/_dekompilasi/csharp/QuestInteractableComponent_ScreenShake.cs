public class QuestInteractableComponent_ScreenShake : QuestInteractableComponentBase<ScreenShakeComponentData>
{
	private ScreenEffectManager screenEffectManager;

	protected override void Start()
	{
		base.Start();
		screenEffectManager = ComponentManager<ScreenEffectManager>.Value;
	}

	protected override void FinishInteractionWithData(ScreenShakeComponentData data, Network_Player player, bool atStart, bool successfull)
	{
		base.FinishInteractionWithData(data, player, atStart, successfull);
		if ((data.shakeRadius <= 0f || Helper.LocalPlayerIsWithinDistance(transform.position, data.shakeRadius)) && screenEffectManager != null && data != null)
		{
			screenEffectManager.ShakeCamera(data.cameraShakeSettings);
		}
	}
}
