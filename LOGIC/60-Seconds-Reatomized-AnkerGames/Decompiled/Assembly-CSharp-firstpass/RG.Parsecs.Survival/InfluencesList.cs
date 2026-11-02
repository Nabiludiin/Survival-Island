using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class InfluencesList
{
	[SerializeField]
	private List<Influence> _influences;

	public List<Influence> Influences => _influences;
}
