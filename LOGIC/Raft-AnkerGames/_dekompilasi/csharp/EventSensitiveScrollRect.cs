using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventSensitiveScrollRect : MonoBehaviour
{
	[SerializeField]
	private int offset;

	[SerializeField]
	private int extraCount;

	private static float SCROLL_MARGIN = 0.3f;

	private ScrollRect sr;

	public void Awake()
	{
		sr = gameObject.GetComponent<ScrollRect>();
	}

	public void OnUpdateSelected(BaseEventData eventData)
	{
		Transform content = sr.content;
		Transform parent = eventData.selectedObject.transform;
		while (parent.parent != content && parent.parent != null)
		{
			parent = parent.parent;
		}
		int num = parent.GetSiblingIndex() - offset;
		float num2 = 1f - (float)num / (float)(sr.content.transform.childCount + extraCount - offset);
		if ((double)num2 < 0.4)
		{
			float num3 = 1f / (float)(sr.content.transform.childCount + extraCount - offset);
			num2 -= num3;
		}
		sr.verticalNormalizedPosition = num2;
	}
}
