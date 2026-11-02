using RG.Parsecs.Common;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Menu;

public class AchievementTooltipContent : TooltipContent
{
	public Achievement achievement;

	[SerializeField]
	private Image icon;

	[SerializeField]
	private Selectable selectable;

	private void Start()
	{
		if (achievement.IsAchieved)
		{
			icon.enabled = true;
			icon.sprite = achievement.icon;
			selectable.enabled = true;
		}
		else
		{
			icon.enabled = false;
			selectable.enabled = false;
		}
	}

	public override bool IsValid()
	{
		if (achievement != null)
		{
			return achievement.IsAchieved;
		}
		return false;
	}
}
