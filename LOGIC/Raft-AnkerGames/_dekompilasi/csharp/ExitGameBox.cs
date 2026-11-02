public class ExitGameBox : MenuBox
{
	private StartMenuScreen menuScreen;

	private void Awake()
	{
		menuScreen = GetComponentInParent<StartMenuScreen>();
	}

	public void Button_ExitYes()
	{
		menuScreen.ExitApplication();
	}

	public void Button_ExitNo()
	{
		Close();
	}
}
