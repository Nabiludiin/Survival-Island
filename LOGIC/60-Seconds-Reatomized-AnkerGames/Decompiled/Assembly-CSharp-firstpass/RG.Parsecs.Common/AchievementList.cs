using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Common;

[CreateAssetMenu(menuName = "60 Parsecs!/Goal Editor/Achievement List", fileName = "New Achievement List")]
public class AchievementList : ScriptableObject
{
	[SerializeField]
	private List<Achievement> _achievements = new List<Achievement>();

	public List<Achievement> Achievements => _achievements;
}
