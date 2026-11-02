using UnityEngine;

namespace RG.Parsecs.EventEditor;

[CreateAssetMenu(fileName = "New Function Action Condition", menuName = "60 Parsecs!/Event Editor/Action Conditions/New Function Action Condition")]
public class FunctionActionCondition : BaseActionCondition
{
	[SerializeField]
	private NodeFunction _canSelect;

	[SerializeField]
	private NodeFunction _onSelect;

	[SerializeField]
	private NodeFunction _onDeselect;

	public override bool CanSelect()
	{
		if (_canSelect == null)
		{
			return true;
		}
		for (int i = 0; i < _canSelect.Outputs.Length; i++)
		{
			if (_canSelect.Outputs[i].ConnectionTypeId.Equals("Bool"))
			{
				_canSelect.Execute(null);
				return (bool)_canSelect.Outputs[i].RuntimeValue;
			}
		}
		throw new UnityException($"There is no bool output in {_canSelect.name} function used by {name} Action Condition");
	}

	public override void OnSelect()
	{
		if (_onSelect != null)
		{
			_onSelect.Execute(null);
		}
	}

	public override void OnDeselect()
	{
		if (_onDeselect != null)
		{
			_onDeselect.Execute(null);
		}
	}
}
