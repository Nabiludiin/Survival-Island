using System.Collections.Generic;
using UnityEngine;

public class AnimationEventCaller : MonoBehaviour
{
	[SerializeField]
	private List<AnimationConnection> connections = new List<AnimationConnection>();

	[SerializeField]
	private Animator anim;

	private void Update()
	{
		if (anim == null)
		{
			return;
		}
		foreach (AnimationConnection connection in connections)
		{
			int layerIndex = anim.GetLayerIndex(connection.layerName);
			AnimatorStateInfo currentAnimatorStateInfo = anim.GetCurrentAnimatorStateInfo(layerIndex);
			if (currentAnimatorStateInfo.IsName(connection.stateName))
			{
				if (!connection.objectReciever.activeInHierarchy)
				{
					continue;
				}
				if (!connection.calledEvent)
				{
					connection.active = true;
					if (currentAnimatorStateInfo.normalizedTime >= connection.timeOfEvent)
					{
						connection.objectReciever.SendMessage(connection.functionName, connection.functionParameter, SendMessageOptions.DontRequireReceiver);
						connection.calledEvent = true;
						connection.active = false;
					}
				}
				else if (!connection.active && currentAnimatorStateInfo.normalizedTime < connection.lastNormalizedTime)
				{
					connection.calledEvent = false;
					connection.active = false;
				}
				connection.lastNormalizedTime = currentAnimatorStateInfo.normalizedTime;
			}
			else
			{
				if (connection.active)
				{
					connection.objectReciever.SendMessage(connection.functionName, connection.functionParameter, SendMessageOptions.DontRequireReceiver);
					connection.calledEvent = true;
					connection.active = false;
				}
				if (connection.calledEvent && !connection.active)
				{
					connection.calledEvent = false;
				}
			}
		}
	}

	private void OnValidate()
	{
		foreach (AnimationConnection connection in connections)
		{
			if (string.IsNullOrEmpty(connection.functionName))
			{
				connection.name = "Set a function name";
			}
			else
			{
				connection.name = connection.stateName + " | " + connection.functionName;
			}
		}
	}
}
