using FMODUnity;

public class QuestInteractableComponent_Sound : QuestInteractableComponentBase<SoundComponentData>
{
	protected override void FinishInteractionWithData(SoundComponentData data, Network_Player player, bool atStart, bool successfull)
	{
		base.FinishInteractionWithData(data, player, atStart, successfull);
		if (data.useOneShotSound)
		{
			if (data.soundPlayLocationTransform != null)
			{
				RuntimeManager.PlayOneShotAttached(data.er_sound, data.soundPlayLocationTransform.gameObject);
			}
			else
			{
				RuntimeManager.PlayOneShot(data.er_sound, transform.position);
			}
		}
		else
		{
			for (int i = 0; i < data.emitter.Length; i++)
			{
				if (!(data.emitter[i] == null))
				{
					data.emitter[i].PlaySafe();
				}
			}
		}
		if (data.hapticEvent > 0)
		{
			SimpleMonoBehaviourSingleton<HapticManager>.Instance?.PlayOneShot((HapticEvent)data.hapticEvent);
		}
	}
}
