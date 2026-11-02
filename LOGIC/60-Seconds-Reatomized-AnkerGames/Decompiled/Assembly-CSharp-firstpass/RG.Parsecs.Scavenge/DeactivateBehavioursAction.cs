using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RG.Parsecs.Scavenge;

public class DeactivateBehavioursAction : EventAction
{
	public List<MonoBehaviour> Objects = new List<MonoBehaviour>();

	public List<Collider> Colliders = new List<Collider>();

	protected override void Act(EventActionArgs args = null)
	{
		for (int i = 0; i < Objects.Count; i++)
		{
			MonoBehaviour monoBehaviour = Objects[i];
			if (monoBehaviour == null)
			{
				Debug.LogErrorFormat("DeactvateGameObjectsAction has a null object to deactivate in scene: {0} in {1}.", SceneManager.GetActiveScene().name, gameObject.name);
			}
			else
			{
				monoBehaviour.enabled = false;
			}
		}
		for (int j = 0; j < Colliders.Count; j++)
		{
			Collider collider = Colliders[j];
			if (collider == null)
			{
				Debug.LogErrorFormat("DeactvateGameObjectsAction has a null collider to deactivate in scene: {0} in {1}.", SceneManager.GetActiveScene().name, gameObject.name);
			}
			else
			{
				collider.enabled = false;
			}
		}
	}
}
