using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
	private static Network_Player localPlayer;

	private void Update()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (!(localPlayer == null) && !(localPlayer.Camera == null))
		{
			transform.LookAt(localPlayer.Camera.transform.position);
		}
	}
}
