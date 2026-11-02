using UnityEngine;

public class AnimatorRandomizeFloat : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private string parameter = string.Empty;

	[SerializeField]
	private TimerEventer switchTimer;

	[SerializeField]
	private Interval_Float interval;

	[SerializeField]
	private float lerpSpeed = 1f;

	private float target;

	private void Update()
	{
		if (switchTimer.DoUpdate(Time.deltaTime, autoResetTimer: true))
		{
			target = interval.GetRandomValue();
		}
		float a = animator.GetFloat(parameter);
		animator.SetFloat(parameter, Mathf.Lerp(a, target, Time.deltaTime * lerpSpeed));
	}
}
