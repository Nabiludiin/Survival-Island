using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectableScrollList : Selectable, ISubmitHandler, IEventSystemHandler
{
	private ScrollRect m_scrollRect;

	[SerializeField]
	private UnityEvent<ScrollRect> m_onSelected;

	private new void Start()
	{
		m_scrollRect = GetComponent<ScrollRect>();
	}

	public void OnSubmit(BaseEventData eventData)
	{
		m_onSelected?.Invoke(m_scrollRect);
	}
}
