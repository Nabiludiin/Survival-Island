using System;
using UnityEngine;

public class AnimatorMessageForwarder : MonoBehaviour
{
	public Action<string> OnAnimatorMessage;

	public virtual void AnimatorMessage(string message)
	{
		OnAnimatorMessage?.Invoke(message);
	}
}
