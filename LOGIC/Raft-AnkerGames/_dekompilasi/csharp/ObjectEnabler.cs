using UnityEngine;
using UnityEngine.Serialization;

public class ObjectEnabler : MonoBehaviour
{
	[FormerlySerializedAs("particlesObjects")]
	public GameObject[] objectsToToggle;

	[HideInInspector]
	public int currentActiveIndex = -1;

	public void ActivateIndex(int index, bool deactivateOthers)
	{
		if (deactivateOthers)
		{
			DeactivateAllObject(index);
		}
		if (index >= 0 && index <= objectsToToggle.Length)
		{
			objectsToToggle[index].SetActiveSafe(value: true);
			currentActiveIndex = index;
		}
	}

	public void DeactivateAllObject(int exceptIndex = -999)
	{
		currentActiveIndex = -1;
		for (int i = 0; i < objectsToToggle.Length; i++)
		{
			if (i != exceptIndex)
			{
				objectsToToggle[i].SetActiveSafe(value: false);
			}
			else
			{
				currentActiveIndex = i;
			}
		}
	}
}
