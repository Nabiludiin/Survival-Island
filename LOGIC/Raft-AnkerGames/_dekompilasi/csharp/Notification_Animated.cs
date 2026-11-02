using UnityEngine;

public class Notification_Animated : Notification
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private float timeOnScreen = 1f;

	public override void Show()
	{
		base.Show();
		animator.SetBool("Show", value: true);
		Invoke("Hide", timeOnScreen);
	}

	public override void Hide()
	{
		base.Hide();
		animator.SetBool("Show", value: false);
	}
}
