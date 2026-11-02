using UnityEngine;

namespace RG.Parsecs.Survival;

public class EndGameRaycastCatcher : MonoBehaviour
{
	public static bool Visible;

	private void Awake()
	{
		Visible = true;
	}

	private void OnDestroy()
	{
		Visible = false;
	}
}
