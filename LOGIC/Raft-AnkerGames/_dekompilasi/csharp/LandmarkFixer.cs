using System.Collections.Generic;

public class LandmarkFixer : Fixer
{
	protected override List<Fixer_Component_ABS> Fixers => new List<Fixer_Component_ABS>();

	public override void DoFixes()
	{
		foreach (Fixer_Component_ABS fixer in Fixers)
		{
			fixer.DoFix(transform);
		}
	}

	public override bool DoChecks()
	{
		base.DoChecks();
		bool result = true;
		foreach (Fixer_Component_ABS fixer in Fixers)
		{
			if (!fixer.DoCheck(transform))
			{
				result = false;
			}
		}
		return result;
	}
}
