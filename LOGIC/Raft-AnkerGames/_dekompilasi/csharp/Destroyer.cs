using UnityEngine;

public class Destroyer : MonoBehaviour
{
	public float destroyTime = 1f;

	public MonoBehaviour_ID_Network behaviourID;

	public NetworkIDTag networkIDTag;

	private float timer;

	private void OnDestroy()
	{
		if (behaviourID != null)
		{
			NetworkIDManager.RemoveNetworkID(behaviourID, behaviourID.GetType(), networkIDTag);
		}
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= destroyTime)
		{
			Object.Destroy(gameObject);
		}
	}

	private bool HasBehaviourID()
	{
		return behaviourID != null;
	}
}
