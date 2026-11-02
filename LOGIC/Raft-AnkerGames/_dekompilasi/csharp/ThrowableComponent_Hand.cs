public class ThrowableComponent_Hand : ThrowableComponent
{
	protected override void CallStartChargeEvent()
	{
		base.CallStartChargeEvent();
		playerNetwork.Animator.anim.SetBool("HookThrow", value: true);
	}

	protected override void ReleaseButton()
	{
		base.ReleaseButton();
		playerNetwork.Animator.anim.SetBool("HookThrow", value: false);
	}

	public override void OnDeSelect()
	{
		base.OnDeSelect();
		playerNetwork.Animator.anim.SetBool("HookThrow", value: false);
	}
}
