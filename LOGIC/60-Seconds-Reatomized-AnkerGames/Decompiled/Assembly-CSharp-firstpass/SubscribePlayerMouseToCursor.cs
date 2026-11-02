using RG.Parsecs.Common;
using RG.Parsecs.Survival;
using Rewired.Components;
using UnityEngine;

public class SubscribePlayerMouseToCursor : MonoBehaviour
{
	[SerializeField]
	private PlayerMouse PlayerMouse;

	public void Start()
	{
		if (Singleton<HandCursor>.Instance != null)
		{
			Singleton<HandCursor>.Instance.SubscribePlayerMouse(PlayerMouse);
		}
	}
}
