using RG.Core.Base;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

[RequireComponent(typeof(Scrollbar))]
public class SetScrollbarHandleSize : RGMonoBehaviour
{
	[SerializeField]
	[Tooltip("Desired size of the Scrollbar handle.")]
	private float _scrollbarHandleSize;

	private Scrollbar _scrollbar;

	private void Awake()
	{
		_scrollbar = GetComponent<Scrollbar>();
		SetSizeDelayed();
	}

	private void OnEnable()
	{
		SetSizeDelayed();
	}

	public void ResetScrollbarDelayed(float delay)
	{
		Invoke("ResetScrollbar", delay);
	}

	public void ResetScrollbar()
	{
		SetSize();
		_scrollbar.value = 1f;
	}

	public void SetSizeDelayed()
	{
		Invoke("SetSize", 0.05f);
	}

	public void SetSize()
	{
		_scrollbar.size = _scrollbarHandleSize;
	}
}
