using UnityEngine;
using UnityEngine.Playables;

public class TimeDilationMixerBehaviour : PlayableBehaviour
{
	private float m_OldTimeScale = 1f;

	public override void OnGraphStart(Playable playable)
	{
		m_OldTimeScale = Time.timeScale;
	}

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		int inputCount = playable.GetInputCount();
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < inputCount; i++)
		{
			float inputWeight = playable.GetInputWeight(i);
			num2 += inputWeight;
			TimeDilationBehaviour behaviour = ((ScriptPlayable<TimeDilationBehaviour>)playable.GetInput(i)).GetBehaviour();
			num += inputWeight * behaviour.timeScale;
		}
		MCTime.BulletTimeScale = num + m_OldTimeScale * (1f - num2);
	}

	public override void OnGraphStop(Playable playable)
	{
		MCTime.BulletTimeScale = m_OldTimeScale;
	}
}
