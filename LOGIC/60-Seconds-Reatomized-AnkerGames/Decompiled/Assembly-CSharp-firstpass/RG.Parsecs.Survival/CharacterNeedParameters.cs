using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class CharacterNeedParameters
{
	public enum ECharacterNeedStatus
	{
		MIN,
		LOW,
		NORMAL,
		HIGH,
		MAX
	}

	[SerializeField]
	[Tooltip("This field decides whether the parameters can be changed in code (in runtime).\nIf you don't know what to set it to, set it to false.")]
	private bool _isMutable;

	[Tooltip("The need for which these parameters are defined.")]
	[SerializeField]
	private CharacterNeed _need;

	[SerializeField]
	[Tooltip("This field holds info about the status of the need.The status represents whether the value is min, low, normal, high or max.Because of the assymetry of the thresholds for entering various states,this value needs to be stored.When setting up the character, make sure that the status you setmatches the value and status thresholds you set for the need.")]
	private ECharacterNeedStatus _needStatus = ECharacterNeedStatus.NORMAL;

	[Tooltip("The value of the need.")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _value = 0.5f;

	[Tooltip("If the value of the need falls below this threshold, the character will enter an appropriate low status.")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _lowValueThreshold = 0.25f;

	[Tooltip("If the value of the need rises above this threshold, the character will enter an appropriate high status.")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _highValueThreshold = 0.75f;

	[Tooltip("If the value of the need rises above this threshold, the character will lose its low value status.")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _riseToNormalValueThreshold = 0.5f;

	[Tooltip("If the value of the need rises above this threshold, the character will lose its min value status.")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _riseToLowValueThreshold = 0.25f;

	[Tooltip("If the value of the need falls below this threshold, the character will lose its max value status.")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _fallToNormalValueThreshold = 0.5f;

	[Tooltip("If the value of the need falls below this threshold, the character will lose its max value status.")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _fallToHighValueThreshold = 0.75f;

	[Tooltip("The amount by which the value of the need changes each day by default.\nThis may be positive, negative or zero.")]
	[Range(-1f, 1f)]
	[SerializeField]
	private float _dailyValueChange;

	[Tooltip("If set to true, the rising need value thresholdswon't be checked")]
	[SerializeField]
	private bool _minValueStatusIrreversible;

	[Tooltip("If set to true, the falling need value thresholdswon't be checked")]
	[SerializeField]
	private bool _maxValueStatusIrreversible;

	[Tooltip("The status the character will enter when the need value reaches max. Can be null.")]
	[SerializeField]
	private CharacterStatus _maxValueStatus;

	[Tooltip("The status the character will enter when the need value reaches high threshold. Can be null.")]
	[SerializeField]
	private CharacterStatus _highValueStatus;

	[Tooltip("The status the character will enter when the need value reaches low threshold. Can be null.")]
	[SerializeField]
	private CharacterStatus _lowValueStatus;

	[Tooltip("The status the character will enter when the need value reaches min. Can be null.")]
	[SerializeField]
	private CharacterStatus _minValueStatus;

	[SerializeField]
	private bool _keepPrevStatusOnEnterMin;

	[SerializeField]
	private bool _keepPrevStatusOnEnterMax;

	private CharacterStatus _cachedStatus;

	private const string IMMUTABILITY_VIOLATION_ERROR_TEXT = "Attempting to change an immutable instance of CharacterNeedParameters.";

	public CharacterNeed Need => _need;

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
				ECharacterNeedStatus needStatus = _needStatus;
				_value = value;
				if (_value < 0f || Mathf.Approximately(_value, 0f))
				{
					_value = 0f;
				}
				if (_value > 1f || Mathf.Approximately(_value, 1f))
				{
					_value = 1f;
				}
				if (needStatus == ECharacterNeedStatus.MAX && !_maxValueStatusIrreversible)
				{
					CheckFallToHighStatus();
					CheckFallToNormalStatus();
					CheckFallToLowStatus();
					CheckFallToMinStatus();
				}
				if (needStatus == ECharacterNeedStatus.HIGH)
				{
					CheckRiseToMaxStatus();
					CheckFallToNormalStatus();
					CheckFallToLowStatus();
					CheckFallToMinStatus();
				}
				if (needStatus == ECharacterNeedStatus.NORMAL)
				{
					CheckRiseToMaxStatus();
					CheckRiseToHighStatus();
					CheckFallToLowStatus();
					CheckFallToMinStatus();
				}
				if (needStatus == ECharacterNeedStatus.LOW)
				{
					CheckRiseToMaxStatus();
					CheckRiseToHighStatus();
					CheckRiseToNormalStatus();
					CheckFallToMinStatus();
				}
				if (needStatus == ECharacterNeedStatus.MIN && !_minValueStatusIrreversible)
				{
					CheckRiseToMaxStatus();
					CheckRiseToHighStatus();
					CheckRiseToNormalStatus();
					CheckRiseToLowStatus();
				}
				return;
			}
			throw new AccessViolationException("Attempting to change an immutable instance of CharacterNeedParameters.");
		}
	}

	public float LowValueThreshold
	{
		get
		{
			return _lowValueThreshold;
		}
		set
		{
			if (_isMutable)
			{
				_lowValueThreshold = value;
				return;
			}
			throw new AccessViolationException("Attempting to change an immutable instance of CharacterNeedParameters.");
		}
	}

	public float HighValueThreshold
	{
		get
		{
			return _highValueThreshold;
		}
		set
		{
			if (_isMutable)
			{
				_highValueThreshold = value;
				return;
			}
			throw new AccessViolationException("Attempting to change an immutable instance of CharacterNeedParameters.");
		}
	}

	public float FallToNormalValueThreshold
	{
		get
		{
			return _fallToNormalValueThreshold;
		}
		set
		{
			if (_isMutable)
			{
				_fallToNormalValueThreshold = value;
				return;
			}
			throw new AccessViolationException("Attempting to change an immutable instance of CharacterNeedParameters.");
		}
	}

	public float FallToHighValueThreshold
	{
		get
		{
			return _fallToHighValueThreshold;
		}
		set
		{
			if (_isMutable)
			{
				_fallToHighValueThreshold = value;
				return;
			}
			throw new AccessViolationException("Attempting to change an immutable instance of CharacterNeedParameters.");
		}
	}

	public float RiseToNormalValueThreshold
	{
		get
		{
			return _riseToNormalValueThreshold;
		}
		set
		{
			if (_isMutable)
			{
				_riseToNormalValueThreshold = value;
				return;
			}
			throw new AccessViolationException("Attempting to change an immutable instance of CharacterNeedParameters.");
		}
	}

	public float RiseToLowValueThreshold
	{
		get
		{
			return _riseToLowValueThreshold;
		}
		set
		{
			if (_isMutable)
			{
				_riseToNormalValueThreshold = value;
				return;
			}
			throw new AccessViolationException("Attempting to change an immutable instance of CharacterNeedParameters.");
		}
	}

	public float DailyValueChange
	{
		get
		{
			return _dailyValueChange;
		}
		set
		{
			if (_isMutable)
			{
				_dailyValueChange = value;
				return;
			}
			throw new AccessViolationException("Attempting to change an immutable instance of CharacterNeedParameters.");
		}
	}

	public CharacterStatus MaxValueStatus
	{
		get
		{
			return _maxValueStatus;
		}
		set
		{
			if (_isMutable)
			{
				_maxValueStatus = value;
				return;
			}
			throw new AccessViolationException("Attempting to change an immutable instance of CharacterNeedParameters.");
		}
	}

	public CharacterStatus HighValueStatus
	{
		get
		{
			return _highValueStatus;
		}
		set
		{
			if (_isMutable)
			{
				_highValueStatus = value;
				return;
			}
			throw new AccessViolationException("Attempting to change an immutable instance of CharacterNeedParameters.");
		}
	}

	public CharacterStatus LowValueStatus
	{
		get
		{
			return _lowValueStatus;
		}
		set
		{
			if (_isMutable)
			{
				_lowValueStatus = value;
				return;
			}
			throw new AccessViolationException("Attempting to change an immutable instance of CharacterNeedParameters.");
		}
	}

	public CharacterStatus MinValueStatus
	{
		get
		{
			return _minValueStatus;
		}
		set
		{
			if (_isMutable)
			{
				_minValueStatus = value;
				return;
			}
			throw new AccessViolationException("Attempting to change an immutable instance of CharacterNeedParameters.");
		}
	}

	public bool KeepPrevStatusOnEnterMin => _keepPrevStatusOnEnterMin;

	public bool KeepPrevStatusOnEnterMax => _keepPrevStatusOnEnterMax;

	public CharacterStatus CachedStatus
	{
		get
		{
			return _cachedStatus;
		}
		set
		{
			_cachedStatus = value;
		}
	}

	public void SetNeedStatus(ECharacterNeedStatus status)
	{
		_needStatus = status;
	}

	public ECharacterNeedStatus GetNeedStatus()
	{
		return _needStatus;
	}

	public List<ECharacterNeedStatus> GetCharacterNeedStatusesPreventingReturningFromExpedition()
	{
		List<ECharacterNeedStatus> list = new List<ECharacterNeedStatus>();
		if (MaxValueStatus != null && MaxValueStatus.PreventsReturnFromExpedition)
		{
			list.Add(ECharacterNeedStatus.MAX);
		}
		if (HighValueStatus != null && HighValueStatus.PreventsReturnFromExpedition)
		{
			list.Add(ECharacterNeedStatus.HIGH);
		}
		if (LowValueStatus != null && LowValueStatus.PreventsReturnFromExpedition)
		{
			list.Add(ECharacterNeedStatus.LOW);
		}
		if (MinValueStatus != null && MinValueStatus.PreventsReturnFromExpedition)
		{
			list.Add(ECharacterNeedStatus.MIN);
		}
		return list;
	}

	public CharacterStatus GetCurrentCharacterStatusForNeedStatus()
	{
		return GetCharacterStatusForNeedStatus(_needStatus);
	}

	public CharacterStatus GetCharacterStatusForNeedStatus(ECharacterNeedStatus needStatus)
	{
		return needStatus switch
		{
			ECharacterNeedStatus.HIGH => HighValueStatus, 
			ECharacterNeedStatus.MAX => MaxValueStatus, 
			ECharacterNeedStatus.LOW => LowValueStatus, 
			ECharacterNeedStatus.MIN => MinValueStatus, 
			_ => null, 
		};
	}

	public CharacterNeedParameters GetMutableCopy()
	{
		return new CharacterNeedParameters
		{
			_isMutable = true,
			_value = _value,
			_dailyValueChange = _dailyValueChange,
			_fallToNormalValueThreshold = _fallToNormalValueThreshold,
			_fallToHighValueThreshold = _fallToHighValueThreshold,
			_highValueStatus = _highValueStatus,
			_highValueThreshold = _highValueThreshold,
			_lowValueStatus = _lowValueStatus,
			_lowValueThreshold = _lowValueThreshold,
			_maxValueStatus = _maxValueStatus,
			_minValueStatus = _minValueStatus,
			_need = _need,
			_riseToNormalValueThreshold = _riseToNormalValueThreshold,
			_riseToLowValueThreshold = _riseToLowValueThreshold,
			_needStatus = _needStatus,
			_minValueStatusIrreversible = _minValueStatusIrreversible,
			_maxValueStatusIrreversible = _maxValueStatusIrreversible,
			_keepPrevStatusOnEnterMin = _keepPrevStatusOnEnterMin,
			_keepPrevStatusOnEnterMax = _keepPrevStatusOnEnterMax
		};
	}

	private void CheckRiseToMaxStatus()
	{
		if (Mathf.Approximately(_value, 1f))
		{
			_needStatus = ECharacterNeedStatus.MAX;
		}
	}

	private void CheckFallToMinStatus()
	{
		if (Mathf.Approximately(_value, 0f))
		{
			_needStatus = ECharacterNeedStatus.MIN;
		}
	}

	private void CheckFallToHighStatus()
	{
		if (_value <= FallToHighValueThreshold && _value > FallToNormalValueThreshold)
		{
			_needStatus = ECharacterNeedStatus.HIGH;
		}
	}

	private void CheckRiseToHighStatus()
	{
		if (_value >= HighValueThreshold && _value < 1f)
		{
			_needStatus = ECharacterNeedStatus.HIGH;
		}
	}

	private void CheckFallToNormalStatus()
	{
		if (_value <= FallToNormalValueThreshold && _value > LowValueThreshold)
		{
			_needStatus = ECharacterNeedStatus.NORMAL;
		}
	}

	private void CheckRiseToNormalStatus()
	{
		if (_value >= RiseToNormalValueThreshold && _value < HighValueThreshold)
		{
			_needStatus = ECharacterNeedStatus.NORMAL;
		}
	}

	private void CheckFallToLowStatus()
	{
		if (_value <= LowValueThreshold && _value > 0f)
		{
			_needStatus = ECharacterNeedStatus.LOW;
		}
	}

	private void CheckRiseToLowStatus()
	{
		if (_value >= RiseToLowValueThreshold && _value < RiseToNormalValueThreshold)
		{
			_needStatus = ECharacterNeedStatus.LOW;
		}
	}

	public bool AllowStatusRemoval(ECharacterNeedStatus prevNeedStatus)
	{
		if (prevNeedStatus != _needStatus)
		{
			if (_needStatus == ECharacterNeedStatus.MIN && _keepPrevStatusOnEnterMin)
			{
				return false;
			}
			if (_needStatus == ECharacterNeedStatus.MAX && _keepPrevStatusOnEnterMax)
			{
				return false;
			}
		}
		return true;
	}
}
