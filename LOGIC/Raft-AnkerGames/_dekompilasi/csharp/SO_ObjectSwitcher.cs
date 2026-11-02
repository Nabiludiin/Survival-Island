using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSwitcher", menuName = "Raft/Miscellaneous/ObjectSwitcher")]
public class SO_ObjectSwitcher : ScriptableObject
{
	public List<ObjectSwitcherConnection> objectsToSwitch = new List<ObjectSwitcherConnection>();
}
