public class OptionsMenuBox : MenuBox
{
	protected Settings settings;

	private void OnEnable()
	{
		SetFocus(SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad);
	}

	public virtual BoxVisibility GetBoxVisibility()
	{
		return BoxVisibility.Both;
	}

	public virtual void SetFocus(bool gamepad)
	{
	}

	public override void Close()
	{
		if (settings == null)
		{
			settings = GetComponentInParent<Settings>(includeInactive: true);
		}
		settings.Close();
	}

	public virtual void Save(RGD_Settings settings)
	{
	}

	public virtual void Load(RGD_Settings settings)
	{
	}
}
