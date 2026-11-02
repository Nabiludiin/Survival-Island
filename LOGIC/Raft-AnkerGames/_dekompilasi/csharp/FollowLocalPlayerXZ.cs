using UnityEngine;

public class FollowLocalPlayerXZ : MonoBehaviour
{
	[SerializeField]
	private AreaZone areaBounds;

	private static Network_Player localPlayer;

	private void Update()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (localPlayer != null)
		{
			if (areaBounds == null)
			{
				Vector3 position = transform.position;
				Vector3 position2 = localPlayer.transform.position;
				position.x = position2.x;
				position.z = position2.z;
				transform.position = position;
			}
			else
			{
				Vector3 vector = areaBounds.transform.InverseTransformPoint(localPlayer.transform.position);
				Vector3 position3 = new Vector3(position3.x = Mathf.Clamp01(vector.x), position3.y = Mathf.Clamp01(vector.y), position3.z = Mathf.Clamp01(vector.z));
				position3 = areaBounds.transform.TransformPoint(position3);
				transform.position = position3;
			}
		}
	}
}
