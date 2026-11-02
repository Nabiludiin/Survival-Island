using UnityEngine;

public abstract class dfTweenPlayableBase : MonoBehaviour
{
	public abstract string TweenName { get; set; }

	public abstract bool IsPlaying { get; }

	public abstract void Play();

	public abstract void Stop();

	public abstract void Reset();

	public void Enable()
	{
		enabled = true;
	}

	public void Disable()
	{
		enabled = false;
	}

	public override string ToString()
	{
		return TweenName + " - " + base.ToString();
	}
}
