using UnityEngine;

public class ObjectOfInterestTracker : MonoBehaviour
{
	public ObjectOfInterestType trackType;

	public float maxYDiffToInteract;

	private void Awake()
	{
		if (trackType != ObjectOfInterestType.None && ObjectManager.GetObjectOfInterest(trackType) == null)
		{
			ObjectManager.AddObjectOfInterest(trackType, transform);
		}
	}

	private void OnDestroy()
	{
		ObjectManager.RemoveObjectOfInterest(trackType);
	}
}
