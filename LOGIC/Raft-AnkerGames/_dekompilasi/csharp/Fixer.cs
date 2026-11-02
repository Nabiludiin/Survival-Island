using System.Collections.Generic;
using UnityEngine;

public abstract class Fixer : MonoBehaviour
{
	private List<Fixer_Component_ABS> all_fixers = new List<Fixer_Component_ABS>();

	protected abstract List<Fixer_Component_ABS> Fixers { get; }

	protected virtual void Awake()
	{
	}

	public virtual void DoFixes()
	{
	}

	public virtual bool DoChecks()
	{
		return true;
	}
}
