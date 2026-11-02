using I2.Loc;
using RG.Core.Base;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public class BaseActionCondition : RGScriptableObject
{
	[SerializeField]
	private Sprite _sprite;

	[SerializeField]
	private LocalizedString _iconTerm;

	[SerializeField]
	private LocalizedString _selectableTerm;

	[SerializeField]
	private LocalizedString _notSelectableTerm;

	public LocalizedString SelectableTerm => _selectableTerm;

	public LocalizedString NotSelectableTerm => _notSelectableTerm;

	public Sprite Sprite => _sprite;

	public LocalizedString IconTerm => _iconTerm;

	public virtual bool CanSelect()
	{
		return true;
	}

	public virtual void OnSelect()
	{
	}

	public virtual void OnDeselect()
	{
	}
}
