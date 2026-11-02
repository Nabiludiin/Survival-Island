using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Menu;

public class EnableIfAchievementUnlocked : MonoBehaviour
{
	public Achievement achievement;

	private void OnEnable()
	{
		if (!achievement.IsAchieved)
		{
			gameObject.SetActive(value: false);
		}
	}
}
