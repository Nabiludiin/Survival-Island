using UnityEngine;

public class BuildingUI : MonoBehaviour
{
	public CostCollection cost;

	[SerializeField]
	private bool bilboard = true;

	private static Transform player;

	private bool isVisible;

	public bool IsVisible
	{
		get
		{
			return isVisible;
		}
		private set
		{
			isVisible = value;
		}
	}

	protected virtual void Start()
	{
		if (player == null)
		{
			player = Object.FindObjectOfType<Player>().transform;
		}
	}

	protected virtual void Update()
	{
		if (IsVisible && bilboard)
		{
			transform.rotation = Quaternion.LookRotation(transform.position - player.position);
		}
	}

	public virtual void Show()
	{
		if (CanvasHelper.ActiveMenu == MenuType.None)
		{
			gameObject.SetActive(value: true);
			IsVisible = true;
		}
	}

	public void Hide()
	{
		gameObject.SetActive(value: false);
		IsVisible = false;
	}
}
