using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class ExpeditionThreatRuntimeData
{
	[SerializeField]
	[Tooltip("Is threat enabled?")]
	private bool _enabled = true;

	public bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			_enabled = value;
		}
	}
}
