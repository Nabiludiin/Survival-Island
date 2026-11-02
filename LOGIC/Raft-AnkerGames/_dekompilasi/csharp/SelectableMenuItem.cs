using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableMenuItem : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
	[SerializeField]
	private GameObject selected;

	private bool keepActive;

	private void OnEnable()
	{
		if (selected == null)
		{
			selected = gameObject.transform.parent.Find("Selected").gameObject;
		}
		if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
		{
			selected.SetActive(value: true);
		}
	}

	private void OnDisable()
	{
		selected.SetActive(value: false);
	}

	public void OnSelect(BaseEventData eventData)
	{
		selected.SetActive(value: true);
	}

	public void OnDeselect(BaseEventData eventData)
	{
		if (!keepActive)
		{
			selected.SetActive(value: false);
		}
	}

	public void KeepActive()
	{
		selected.SetActive(value: true);
		keepActive = true;
	}

	public void ReleaseActive()
	{
		keepActive = false;
	}

	public bool IsKeepActive()
	{
		return keepActive;
	}
}
