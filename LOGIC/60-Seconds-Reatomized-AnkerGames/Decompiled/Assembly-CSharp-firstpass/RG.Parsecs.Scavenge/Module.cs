using UnityEngine;

namespace RG.Parsecs.Scavenge;

public abstract class Module : MonoBehaviour
{
	public abstract void Load();

	public abstract void Unload();
}
