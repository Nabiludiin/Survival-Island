using Rewired;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class CustomMouseController : MonoBehaviour
{
	private Vector2 mouseDelta = Vector2.zero;

	public int AxisEase = 100;

	private void Start()
	{
	}

	private void Update()
	{
		if (ReInput.controllers.CustomControllers.Count != 0)
		{
			CustomController customController = ReInput.controllers.CustomControllers[0];
			_ = ReInput.controllers.Mouse;
			Player player = ReInput.players.GetPlayer(0);
			mouseDelta.x += player.GetAxis("MouseHorizontal");
			mouseDelta.y += player.GetAxis("MouseVertical");
			if (player.GetButtonDown(13))
			{
				mouseDelta = Vector2.zero;
			}
			if (player.GetButton(13))
			{
				Vector2 vector = mouseDelta / AxisEase;
				customController.SetAxisValue(0, vector.x);
				customController.SetAxisValue(1, vector.y);
			}
			else
			{
				customController.SetAxisValue(0, 0f);
				customController.SetAxisValue(1, 0f);
			}
		}
	}
}
