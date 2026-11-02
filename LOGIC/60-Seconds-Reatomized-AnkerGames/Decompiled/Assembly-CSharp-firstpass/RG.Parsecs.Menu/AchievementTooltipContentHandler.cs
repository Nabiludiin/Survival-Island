using RG.Parsecs.Common;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Menu;

public class AchievementTooltipContentHandler : TooltipContentHandler
{
	[SerializeField]
	private TextMeshProUGUI _text;

	public override void HandleContent(TooltipContent content)
	{
		AchievementTooltipContent achievementTooltipContent = content as AchievementTooltipContent;
		if (achievementTooltipContent != null)
		{
			string text = $"<color=#FFFF00>{achievementTooltipContent.achievement.title}</color>\n{achievementTooltipContent.achievement.description}";
			_text.text = text;
		}
	}
}
