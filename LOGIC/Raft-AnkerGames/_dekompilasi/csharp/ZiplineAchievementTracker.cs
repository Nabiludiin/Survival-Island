using System;
using UnityEngine;

public class ZiplineAchievementTracker : MonoBehaviour
{
	public Network_Player player;

	private float ziplinedDistance;

	private int minDistanceToTrack = 1;

	private Vector3 calcPoint;

	private void Start()
	{
		if (player.IsLocalPlayer)
		{
			ZiplinePlayer ziplinePlayer = player.ZiplinePlayer;
			ziplinePlayer.OnZiplineAttach = (Action)Delegate.Combine(ziplinePlayer.OnZiplineAttach, new Action(OnZipAttach));
			ZiplinePlayer ziplinePlayer2 = player.ZiplinePlayer;
			ziplinePlayer2.OnZiplineDeattach = (Action)Delegate.Combine(ziplinePlayer2.OnZiplineDeattach, new Action(OnZipDeattach));
		}
	}

	private void OnDestroy()
	{
		player = null;
	}

	private void Update()
	{
		if (player.IsLocalPlayer && player.ZiplinePlayer.IsAttachedToZipline && Vector3.Distance(player.transform.position, calcPoint) > (float)minDistanceToTrack)
		{
			ziplinedDistance += minDistanceToTrack;
			calcPoint = player.transform.position;
		}
	}

	private void OnZipAttach()
	{
		ziplinedDistance = 0f;
		calcPoint = player.transform.position;
	}

	private void OnZipDeattach()
	{
		if (ziplinedDistance > 0f)
		{
			AchievementHandler.AddPlayerZiplineDistance(ziplinedDistance);
			ziplinedDistance = 0f;
		}
	}
}
