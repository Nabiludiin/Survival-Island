using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteBookNote_Enhancer : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[SerializeField]
	private float enhanceScale = 1.5f;

	[SerializeField]
	private Vector3 enhanceOffset = Vector3.zero;

	public static NoteBookNote_Enhancer currentEnhancedNote;

	private static Transform enhancePoint;

	private bool isEnhanced;

	private Vector3 noteStartLocalPos;

	private Vector3 noteStartLocalScale;

	private Transform noteStartParent;

	private Canvas subCanvas;

	private int noteStartSiblingIndex;

	private bool canBeEnhanced = true;

	private Animator animator;

	public static bool IsEnhacedOnNote => currentEnhancedNote != null;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		canBeEnhanced = true;
		noteStartLocalPos = transform.localPosition;
		noteStartLocalScale = transform.localScale;
		noteStartParent = transform.parent;
		noteStartSiblingIndex = transform.GetSiblingIndex();
	}

	private void Start()
	{
		if (enhancePoint == null)
		{
			NoteBookUI noteBookUI = ComponentManager<Network_Player>.Value.NoteBookUI;
			if (noteBookUI != null)
			{
				enhancePoint = noteBookUI.enhancePoint;
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!(currentEnhancedNote != null) || !(currentEnhancedNote != this))
		{
			ToggleEnhancement();
		}
	}

	public void ResetToNormal()
	{
		StopAllCoroutines();
		isEnhanced = false;
		canBeEnhanced = true;
		transform.SetParent(noteStartParent);
		transform.SetSiblingIndex(noteStartSiblingIndex);
		transform.localPosition = noteStartLocalPos;
		transform.localScale = noteStartLocalScale;
		if (currentEnhancedNote == this)
		{
			currentEnhancedNote = null;
		}
	}

	public void ToggleEnhancement()
	{
		if (canBeEnhanced)
		{
			isEnhanced = !isEnhanced;
			if (isEnhanced)
			{
				animator.enabled = false;
				currentEnhancedNote = this;
			}
			else
			{
				canBeEnhanced = false;
				Invoke("ResetEnhanceFlag", 0.25f);
			}
			StopAllCoroutines();
			StartCoroutine(RescaleNote());
		}
	}

	private void ResetEnhanceFlag()
	{
		canBeEnhanced = true;
		animator.enabled = true;
	}

	private IEnumerator RescaleNote()
	{
		float totalEnhanceTime = 0.1f;
		float startTime = Time.time;
		transform.SetParentSafe(isEnhanced ? enhancePoint : noteStartParent);
		Vector3 startScale = transform.localScale;
		Vector3 targetScale = (isEnhanced ? new Vector3(enhanceScale, enhanceScale, enhanceScale) : noteStartLocalScale);
		Vector3 startPosition = transform.localPosition;
		Vector3 targetPosition = (isEnhanced ? (Vector3.zero + enhanceOffset) : noteStartLocalPos);
		GetComponent<Animator>().enabled = !isEnhanced;
		while (true)
		{
			float num = (Time.time - startTime) / totalEnhanceTime;
			transform.localPosition = Vector3.Lerp(startPosition, targetPosition, num);
			transform.localScale = Vector3.Lerp(startScale, targetScale, num);
			if (num >= 1f)
			{
				break;
			}
			yield return null;
		}
		if (!isEnhanced && currentEnhancedNote == this)
		{
			currentEnhancedNote = null;
		}
		if (!isEnhanced)
		{
			transform.SetSiblingIndex(noteStartSiblingIndex);
		}
	}
}
