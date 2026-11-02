using UnityEngine;

public class Notification : MonoBehaviour
{
	public string identifier;

	public bool active;

	public virtual void Show()
	{
		active = true;
	}

	public virtual void Hide()
	{
		active = false;
	}
}
