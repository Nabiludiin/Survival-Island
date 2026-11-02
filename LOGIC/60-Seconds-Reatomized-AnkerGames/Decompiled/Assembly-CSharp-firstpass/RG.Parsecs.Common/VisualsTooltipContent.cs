using UnityEngine.Serialization;

namespace RG.Parsecs.Common;

public class VisualsTooltipContent : TooltipContent
{
	[FormerlySerializedAs("Term")]
	public TooltipTerm TooltipTerm;

	public override bool IsValid()
	{
		if (TooltipTerm != null && (string)TooltipTerm.Term != null)
		{
			return !TooltipTerm.Term.IsNullOrEmpty();
		}
		return false;
	}
}
