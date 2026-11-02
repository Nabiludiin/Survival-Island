using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RG.Parsecs.Scavenge;

public class DeactivateGameObjectsAction : EventAction
{
	public bool ActOnTriggeringObject;

	public List<GameObject> Objects = new List<GameObject>();

	protected override void Start()
	{
		base.Start();
	}

	protected override void Act(EventActionArgs args = null)
	{
		for (int i = 0; i < Objects.Count; i++)
		{
			GameObject gameObject = Objects[i];
			if (gameObject == null)
			{
				Debug.LogErrorFormat("DeactvateGameObjectsAction has a null object to deactivate in scene: {0} in {1}.", SceneManager.GetActiveScene().name, base.gameObject.name);
			}
			else
			{
				gameObject.SetActive(value: false);
			}
		}
		if (ActOnTriggeringObject && args != null)
		{
			if (args.Actor != null)
			{
				args.Actor.GameObject.SetActive(value: false);
			}
			args.Object.SetActive(value: false);
		}
	}
}
