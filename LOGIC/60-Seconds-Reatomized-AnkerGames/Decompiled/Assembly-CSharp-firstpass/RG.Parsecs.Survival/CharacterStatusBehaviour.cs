using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class CharacterStatusBehaviour
{
	[Tooltip("The stats for which these behaviours are defined.")]
	[SerializeField]
	private CharacterStatus _status;

	[Tooltip("The event that will be run when the character gains this status.")]
	[SerializeField]
	private SystemStatusEvent _onStatusEnterEvent;

	[Tooltip("The event that will be run each day while the character remains in this status.")]
	[SerializeField]
	private SystemStatusEvent _onStatusPersistEvent;

	[Tooltip("The event that will be run when the character exits this status.")]
	[SerializeField]
	private SystemStatusEvent _onStatusExitEvent;

	public CharacterStatus Status => _status;

	public SystemStatusEvent OnStatusEnterEvent => _onStatusEnterEvent;

	public SystemStatusEvent OnStatusPersistEvent => _onStatusPersistEvent;

	public SystemStatusEvent OnStatusExitEvent => _onStatusExitEvent;
}
