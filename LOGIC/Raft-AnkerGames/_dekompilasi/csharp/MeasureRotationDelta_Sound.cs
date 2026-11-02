using FMODUnity;
using UnityEngine;

public class MeasureRotationDelta_Sound : MeasureRotationDelta
{
	[SerializeField]
	private StudioEventEmitter emitter_rotate;

	[SerializeField]
	[Range(0f, 1f)]
	private float minRequiredRotation = 0.1f;

	[SerializeField]
	private bool xRotation;

	[SerializeField]
	private bool yRotation;

	[SerializeField]
	private bool zRotation;

	[SerializeField]
	private bool stopAutomatically = true;

	[SerializeField]
	private string emitterParameter = string.Empty;

	[SerializeField]
	private float emitterLerpSpeed = 1f;

	protected override void LateUpdate()
	{
		base.LateUpdate();
		if (!(emitter_rotate != null))
		{
			return;
		}
		float f = 0f;
		if (xRotation)
		{
			f = NormalizedRotationDelta.x;
		}
		else if (yRotation)
		{
			f = NormalizedRotationDelta.y;
		}
		else if (zRotation)
		{
			f = NormalizedRotationDelta.z;
		}
		f = Mathf.Abs(f);
		if (emitter_rotate.IsPlaying())
		{
			if (stopAutomatically && f < minRequiredRotation)
			{
				emitter_rotate.Stop();
			}
		}
		else if (f >= minRequiredRotation)
		{
			emitter_rotate.Play();
		}
		if (emitterParameter != string.Empty)
		{
			emitter_rotate.EventInstance.getParameterByName(emitterParameter, out var value);
			value = Mathf.Lerp(value, f, Time.deltaTime * emitterLerpSpeed);
			emitter_rotate.SetParameter(emitterParameter, value);
		}
	}
}
