using UnityEngine.UI;

public class CreditsBox : MenuBox
{
	public ScrollRect scrollRect;

	public void Start()
	{
		scrollRect.verticalNormalizedPosition = 1f;
	}

	public override void Update()
	{
		base.Update();
	}
}
