using System.Collections;
using UnityEngine;

public class Instrument_Animator
{
	public float playingAnimationDuration = 0.5f;

	private float latestPlayedTimeStamp;

	private Coroutine stopAnimRoutine;

	public bool ShouldAnimate => !(latestPlayedTimeStamp - Time.time > playingAnimationDuration);

	public Instrument_Animator(float playingAnimationDuration = 0.5f)
	{
		this.playingAnimationDuration = playingAnimationDuration;
	}

	public void UpdatePlayingAnimationLogic(Network_Player player)
	{
		latestPlayedTimeStamp = Time.time;
		player.Animator.anim.SetBool("PlayingInstrumentNote", value: true);
		if (stopAnimRoutine != null)
		{
			CoroutineManager.Singleton.StopCoroutine(stopAnimRoutine);
		}
		stopAnimRoutine = CoroutineManager.Singleton.StartCoroutine(StopAnimation(player));
	}

	public IEnumerator StopAnimation(Network_Player player)
	{
		yield return new WaitForSeconds(playingAnimationDuration);
		if (player != null && player.Animator != null && player.Animator.anim != null)
		{
			player.Animator.anim.SetBool("PlayingInstrumentNote", value: false);
		}
	}

	public void SetIsUsedAnimation(Network_Player player, bool value)
	{
		if (player != null)
		{
			player.Animator.SetAnimation(value ? PlayerFullBodyAnimation.UsingInstrument_3 : PlayerFullBodyAnimation.None_0);
		}
	}
}
