using UnityEngine;

public abstract class Fixer_Component_ABS
{
	public abstract bool DoFix(Transform target);

	public abstract bool DoCheck(Transform target);
}
