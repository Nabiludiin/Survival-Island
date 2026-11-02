using System;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[Serializable]
internal class DoorTypeMaterial
{
	public Material materialA;

	public Material materialB;

	public Sprite lights;

	public DoorOperationalState state;

	public string DoorOpeningClipName;

	public string DoorClosingClipName;
}
