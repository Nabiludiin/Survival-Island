using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class CharacterAttributeValue
{
	[SerializeField]
	[Tooltip("This field decides whether the value of the attribute can be changed in code (in runtime).\nIf you don't know what to set it to, set it to false.")]
	private bool _isMutable;

	[SerializeField]
	private CharacterAttribute _attribute;

	[SerializeField]
	[Range(0f, 1f)]
	private float _value = 0.5f;

	public CharacterAttribute Attribute => _attribute;

	public float Value
	{
		get
		{
			return _value;
		}
		set
		{
			if (_isMutable)
			{
				_value = Mathf.Min(value, 1f);
				_value = Mathf.Max(_value, 0f);
				return;
			}
			throw new AccessViolationException("Attempting to change the value in CharacterAttributeValue instance marked as immutable");
		}
	}

	public CharacterAttributeValue GetMutableCopy()
	{
		return new CharacterAttributeValue
		{
			_isMutable = true,
			_attribute = _attribute,
			_value = _value
		};
	}
}
