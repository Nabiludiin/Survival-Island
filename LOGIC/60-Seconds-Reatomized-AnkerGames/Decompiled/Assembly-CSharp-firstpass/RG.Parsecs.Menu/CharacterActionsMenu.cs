using System;
using RG.Parsecs.Survival;
using UnityEngine.Events;

namespace RG.Parsecs.Menu;

[Serializable]
internal class CharacterActionsMenu
{
	public Character character;

	public UnityEvent action;
}
