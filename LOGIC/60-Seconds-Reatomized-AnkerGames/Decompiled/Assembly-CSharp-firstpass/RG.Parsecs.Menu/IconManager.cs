using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RG.Parsecs.Menu;

public class IconManager : MonoBehaviour
{
	[SerializeField]
	protected bool isInteractable = true;

	[SerializeField]
	private TextMeshProUGUI TextLabel;

	[SerializeField]
	private Color32 NotInteractableTextColor;

	[SerializeField]
	private GameObject[] OptionalGOToDisable;

	[SerializeField]
	private Selectable[] Selectables;

	[SerializeField]
	protected UnityEvent OnSelect;

	[SerializeField]
	protected UnityEvent OnDeselect;

	[SerializeField]
	protected UnityEvent OnClick;

	private bool _isSelected;

	private void Start()
	{
		if (!isInteractable)
		{
			SetNotInteractable();
		}
	}

	public void SetNotInteractable()
	{
		isInteractable = false;
		if (isInteractable)
		{
			return;
		}
		if (TextLabel != null)
		{
			TextLabel.color = NotInteractableTextColor;
		}
		if (OptionalGOToDisable != null)
		{
			GameObject[] optionalGOToDisable = OptionalGOToDisable;
			for (int i = 0; i < optionalGOToDisable.Length; i++)
			{
				optionalGOToDisable[i].SetActive(value: false);
			}
		}
		Selectable[] selectables = Selectables;
		for (int i = 0; i < selectables.Length; i++)
		{
			selectables[i].interactable = false;
		}
	}

	public void Select()
	{
		if (isInteractable && !_isSelected)
		{
			_isSelected = true;
			OnSelect.Invoke();
		}
	}

	public void Deselect()
	{
		if (isInteractable && _isSelected)
		{
			_isSelected = false;
			OnDeselect.Invoke();
		}
	}

	public virtual void Click()
	{
		if (isInteractable)
		{
			OnClick.Invoke();
		}
	}
}
