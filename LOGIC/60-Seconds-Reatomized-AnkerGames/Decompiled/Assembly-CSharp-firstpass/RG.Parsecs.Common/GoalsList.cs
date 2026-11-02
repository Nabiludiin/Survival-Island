using System.Collections.Generic;
using RG.Core.Base;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.Common;

[CreateAssetMenu(fileName = "New Goals List", menuName = "60 Parsecs!/Goal Editor/Goals List")]
public class GoalsList : RGScriptableObject
{
	[Tooltip("Start goals")]
	[SerializeField]
	private List<Goal> _goals;

	public List<Goal> Goals => _goals;
}
