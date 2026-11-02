using System.Collections.Generic;
using Cinemachine;
using Rewired;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ScavengePlayersManager : MonoBehaviour
{
	public Transform[] SpawnPoints;

	public CameraTransparency CameraTransparency;

	public CameraTransparency[] AdditionalCameraTransparency;

	public CinemachineVirtualCamera VirtualCamera;

	public CinemachineVirtualCamera[] AdditionalVirtualCameras;

	public HandsController HandsController;

	private List<PlayerController> _players = new List<PlayerController>();

	public static ScavengePlayersManager Instance;

	public List<PlayerController> availableCharacters;

	private ScavengeManager _scavenge;

	public PlayerController PlayerController
	{
		get
		{
			if (_players.Count == 0)
			{
				return null;
			}
			return _players[0];
		}
	}

	public ScavengeManager ScavengeManager => _scavenge;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Debug.LogError("ScavangePlayersManager is not null");
		}
		Instance = this;
	}

	public float GetExplorationTime(float baseTime)
	{
		return baseTime + (float)_players[0].Actor.Intelligence;
	}

	public void Init(ScavengeManager scavenge)
	{
		PlayerController playerController = Object.Instantiate(SetCaptain(), SpawnPoints[0]);
		playerController.Actor.InitAttributes();
		_scavenge = scavenge;
		_players.Add(playerController);
		playerController.Init(ReInput.players.Players[0], scavenge, HandsController);
		CameraTransparency.SetTarget(playerController.transform);
		CameraTransparency[] additionalCameraTransparency = AdditionalCameraTransparency;
		for (int i = 0; i < additionalCameraTransparency.Length; i++)
		{
			additionalCameraTransparency[i].SetTarget(playerController.transform);
		}
		VirtualCamera.Follow = playerController.CameraTarget;
		VirtualCamera.LookAt = playerController.CameraTarget;
		CinemachineVirtualCamera[] additionalVirtualCameras = AdditionalVirtualCameras;
		foreach (CinemachineVirtualCamera obj in additionalVirtualCameras)
		{
			obj.Follow = PlayerController.CameraTarget;
			obj.LookAt = playerController.CameraTarget;
		}
	}

	public bool IsNearRescuePod()
	{
		foreach (PlayerController player in _players)
		{
			if (player.isNearRescuePod)
			{
				return true;
			}
		}
		return false;
	}

	public void Jump(Vector3 position)
	{
		foreach (PlayerController player in _players)
		{
			player.Jump(position);
		}
	}

	internal void UnpauseGameplay()
	{
		foreach (PlayerController player in _players)
		{
			player.UnpauseCharacter();
		}
	}

	internal void StopMovement()
	{
		foreach (PlayerController player in _players)
		{
			player.StopMovement();
		}
	}

	internal void PauseGameplay()
	{
		foreach (PlayerController player in _players)
		{
			player.PauseCharacter();
		}
	}

	private PlayerController SetCaptain()
	{
		foreach (PlayerController availableCharacter in availableCharacters)
		{
			if (availableCharacter.Actor.Character.RuntimeData.IsCaptain)
			{
				Debug.Log("Current captain is " + availableCharacter.Actor.Character.StaticData.Surname);
				return availableCharacter;
			}
		}
		Debug.LogError("Captain not found");
		return null;
	}

	private void OnDestroy()
	{
		_scavenge = null;
	}

	internal void StartTheHunt()
	{
		HandsController.Show();
		foreach (PlayerController player in _players)
		{
			player.Actor.SendEvent(ActorEvents.StartedTheHunt);
		}
	}
}
