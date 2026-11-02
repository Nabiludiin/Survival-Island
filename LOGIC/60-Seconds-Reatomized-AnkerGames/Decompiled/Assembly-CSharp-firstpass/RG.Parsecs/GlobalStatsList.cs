using System.Collections.Generic;
using RG.Core.Base;
using UnityEngine;

namespace RG.Parsecs;

[CreateAssetMenu(fileName = "New Global Variables", menuName = "60 Parsecs!/Stats/New Global Stats")]
public class GlobalStatsList : RGScriptableObject
{
	[SerializeField]
	private List<string> _variables;

	[SerializeField]
	private List<string> _list;

	public List<string> Variables => _variables;

	public List<string> List => _list;
}
