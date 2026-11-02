using System.Collections.Generic;
using RG.Core.Base;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Missions/New Missions List")]
public class MissionList : RGScriptableObject
{
	[SerializeField]
	private List<Mission> _missions;

	public List<Mission> Missions => _missions;
}
