using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerKeyPress : MonoBehaviour
{
	[Serializable]
	public class Connection
	{
		public KeyCode key;

		public AnimatorPropertySetting_Legit settings;
	}

	public Animator anim;

	public List<Connection> connections = new List<Connection>();

	private void Update()
	{
		foreach (Connection connection in connections)
		{
			if (!Input.GetKeyDown(connection.key))
			{
				continue;
			}
			switch (connection.settings.propertyType)
			{
			case AnimatorPropertyType.Float:
				if (connection.settings.lerp)
				{
					if (connection.settings.lerpRoutine != null)
					{
						StopCoroutine(connection.settings.lerpRoutine);
						connection.settings.lerpRoutine = null;
					}
					connection.settings.lerpRoutine = StartCoroutine(connection.settings.LerpFloat(anim, connection.settings.parameterName, connection.settings.floatValue, connection.settings.lerpSpeed));
				}
				else
				{
					anim.SetFloat(connection.settings.parameterName, connection.settings.floatValue);
				}
				break;
			case AnimatorPropertyType.Bool:
				anim.SetBool(connection.settings.parameterName, connection.settings.boolValue);
				break;
			case AnimatorPropertyType.Trigger:
				anim.SetTrigger(connection.settings.parameterName);
				break;
			case AnimatorPropertyType.Int:
				anim.SetInteger(connection.settings.parameterName, connection.settings.intValue);
				break;
			}
		}
	}
}
