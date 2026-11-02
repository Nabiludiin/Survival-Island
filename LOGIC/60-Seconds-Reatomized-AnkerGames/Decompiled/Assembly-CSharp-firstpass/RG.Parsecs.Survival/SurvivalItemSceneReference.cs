using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class SurvivalItemSceneReference
{
	public GameObject Normal;

	public GameObject Damaged;

	public void SetActive(bool active, bool damaged)
	{
		if ((bool)Normal)
		{
			Normal.SetActive(!damaged & active);
		}
		if ((bool)Damaged)
		{
			Damaged.SetActive(damaged & active);
		}
	}
}
