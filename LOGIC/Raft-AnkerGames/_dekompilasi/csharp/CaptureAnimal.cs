using System;
using UnityEngine;

public class CaptureAnimal : MonoBehaviour
{
	private bool _isCapturable = true;

	private bool _isCaptured;

	public Action OnIsCaptured;

	public bool IsCapturable
	{
		get
		{
			if (_isCapturable)
			{
				return !IsCaptured;
			}
			return false;
		}
		set
		{
			_isCapturable = value;
		}
	}

	public bool IsCaptured
	{
		get
		{
			return _isCaptured;
		}
		set
		{
			_isCaptured = value;
			if (_isCaptured)
			{
				SendOnIsCaptured();
			}
		}
	}

	public void SendOnIsCaptured()
	{
		if (OnIsCaptured != null)
		{
			OnIsCaptured();
		}
	}
}
