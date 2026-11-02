using RG.VirtualInput;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalButtonOverride : Button
{
	public Selectable SelectThisOnLeftInsteadOfDefault;

	public Selectable SelectThisOnRightInsteadOfDefault;

	public Selectable SelectThisOnUpInsteadOfDefault;

	public Selectable SelectThisOnDownInsteadOfDefault;

	public Selectable SelectThisEventuallyOnLeft;

	public Selectable SelectThisEventuallyOnRight;

	public Selectable SelectThisEventuallyOnUp;

	public Selectable SelectThisEventuallyOnDown;

	public override Selectable FindSelectableOnDown()
	{
		if (SelectThisOnDownInsteadOfDefault != null && SelectThisOnDownInsteadOfDefault.gameObject.activeInHierarchy && SelectThisOnDownInsteadOfDefault.interactable)
		{
			VirtualInputButton component = SelectThisOnDownInsteadOfDefault.GetComponent<VirtualInputButton>();
			if (component != null && component.IsInteractable)
			{
				return SelectThisOnDownInsteadOfDefault;
			}
		}
		Selectable selectable = base.FindSelectableOnDown();
		if (!CanSelectableBeSelected(selectable))
		{
			return SelectThisEventuallyOnDown;
		}
		return selectable;
	}

	public override Selectable FindSelectableOnLeft()
	{
		if (SelectThisOnLeftInsteadOfDefault != null && SelectThisOnLeftInsteadOfDefault.gameObject.activeInHierarchy && SelectThisOnLeftInsteadOfDefault.interactable)
		{
			VirtualInputButton component = SelectThisOnLeftInsteadOfDefault.GetComponent<VirtualInputButton>();
			if (component != null && component.IsInteractable)
			{
				return SelectThisOnLeftInsteadOfDefault;
			}
		}
		Selectable selectable = base.FindSelectableOnLeft();
		if (!CanSelectableBeSelected(selectable))
		{
			return SelectThisEventuallyOnLeft;
		}
		return selectable;
	}

	public override Selectable FindSelectableOnUp()
	{
		if (SelectThisOnUpInsteadOfDefault != null && SelectThisOnUpInsteadOfDefault.gameObject.activeInHierarchy && SelectThisOnUpInsteadOfDefault.interactable)
		{
			VirtualInputButton component = SelectThisOnUpInsteadOfDefault.GetComponent<VirtualInputButton>();
			if (component != null && component.IsInteractable)
			{
				return SelectThisOnUpInsteadOfDefault;
			}
		}
		Selectable selectable = base.FindSelectableOnUp();
		if (!CanSelectableBeSelected(selectable))
		{
			return SelectThisEventuallyOnUp;
		}
		return selectable;
	}

	public override Selectable FindSelectableOnRight()
	{
		if (SelectThisOnRightInsteadOfDefault != null && SelectThisOnRightInsteadOfDefault.gameObject.activeInHierarchy && SelectThisOnRightInsteadOfDefault.interactable)
		{
			VirtualInputButton component = SelectThisOnRightInsteadOfDefault.GetComponent<VirtualInputButton>();
			if (component != null && component.IsInteractable)
			{
				return SelectThisOnRightInsteadOfDefault;
			}
		}
		Selectable selectable = base.FindSelectableOnRight();
		if (!CanSelectableBeSelected(selectable))
		{
			return SelectThisEventuallyOnRight;
		}
		return selectable;
	}

	private bool CanSelectableBeSelected(Selectable selectable)
	{
		if (selectable == null)
		{
			return false;
		}
		VirtualInputButton component = selectable.GetComponent<VirtualInputButton>();
		if (component == null)
		{
			return false;
		}
		return component.IsInteractable;
	}

	[ContextMenu("Set Journal Ribbon To Left")]
	public void AddJournalRibbonToLeft()
	{
		GameObject gameObject = GameObject.Find("Journal_Button_Hide");
		SelectThisEventuallyOnLeft = gameObject.GetComponent<Selectable>();
	}

	[ContextMenu("Set Journal Ribbon To Up")]
	public void AddJournalRibbonToUp()
	{
		GameObject gameObject = GameObject.Find("Journal_Button_Hide");
		SelectThisEventuallyOnUp = gameObject.GetComponent<Selectable>();
	}

	[ContextMenu("Set Journal Open To Right")]
	public void AddJournalOpenToRight()
	{
		GameObject gameObject = GameObject.Find("ShowJournalButton");
		SelectThisEventuallyOnRight = gameObject.GetComponent<Selectable>();
	}

	[ContextMenu("Set Journal Open To Down")]
	public void AddJournalOpenToDown()
	{
		GameObject gameObject = GameObject.Find("ShowJournalButton");
		SelectThisEventuallyOnDown = gameObject.GetComponent<Selectable>();
	}
}
