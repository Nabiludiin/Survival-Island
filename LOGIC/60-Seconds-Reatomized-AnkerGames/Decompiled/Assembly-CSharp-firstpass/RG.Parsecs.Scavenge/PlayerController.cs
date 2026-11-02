using System;
using System.Collections;
using System.Collections.Generic;
using RG.Parsecs.Survival;
using Rewired;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class PlayerController : MonoBehaviour
{
	private static int MAX_CREW_SIZE = 4;

	public ActorBrain Actor;

	public Transform CameraTarget;

	public GenericEvent<ScavangeItemData, ItemSpawnPoint> OnPickUp = new GenericEvent<ScavangeItemData, ItemSpawnPoint>();

	public GenericEvent OnAirlockDrop = new GenericEvent();

	private Player _player;

	private List<ItemSpawnPoint> _currentInteractedSpawnPoint = new List<ItemSpawnPoint>();

	private ScavengeManager _scavenge;

	private HandsController _hands;

	[SerializeField]
	private CharacterAnimationController _animationController;

	private float _vertical;

	private float _horizontal;

	private Rigidbody _characterController;

	private float _currentWeight;

	public List<ScavangeItemController> _currentItems;

	public static PlayerController Instance;

	public bool _pause;

	private bool _pauseMovement;

	public bool isNearRescuePod;

	private bool _stopMovement;

	public float JumpDelay;

	public float JumpTweenDuration;

	private bool _jumped;

	private bool _isDropping;

	public bool HasEverMoved { get; protected set; }

	internal void Init(Player player, ScavengeManager scavenge, HandsController hands)
	{
		_player = player;
		_scavenge = scavenge;
		_hands = hands;
		Actor.Player = player;
		hands.Init(scavenge, Actor);
	}

	private void Awake()
	{
		Instance = this;
		_characterController = GetComponent<Rigidbody>();
		_currentItems = new List<ScavangeItemController>();
		PauseCharacter();
	}

	public void UnpauseCharacter()
	{
		_pause = false;
	}

	public void PauseCharacter()
	{
		_pause = true;
	}

	public void StopMovement()
	{
		_stopMovement = true;
		_animationController.SetInput(0f, 0f);
	}

	public void Jump(Vector3 entrancePosition)
	{
		if (!_jumped)
		{
			_animationController.Jump();
			Actor.PlaySound(Actor.PlayerSounds.JumpToShuttle);
			entrancePosition.z -= 0.6f;
			StartCoroutine(_jump(entrancePosition));
			_jumped = true;
			_pauseMovement = true;
		}
	}

	private IEnumerator _jump(Vector3 entrancePosition)
	{
		yield return new WaitForSeconds(JumpDelay);
		Vector3 initialPosition = transform.position;
		float counter = 0f;
		float step = 1f / JumpTweenDuration;
		while (counter < 1f)
		{
			counter += Time.deltaTime * step;
			transform.position = Vector3.Lerp(initialPosition, entrancePosition, Mathf.SmoothStep(0f, 1f, counter));
			yield return null;
		}
		transform.position = entrancePosition;
	}

	private void Update()
	{
		if (_jumped)
		{
			return;
		}
		if (!_animationController.DoingCatch)
		{
			_pauseMovement = false;
		}
		if (_pause || _pauseMovement || _stopMovement)
		{
			return;
		}
		Vector2 vector = new Vector2(_player.GetAxis(2), _player.GetAxis(3));
		vector = Vector2.ClampMagnitude(vector, 1f);
		_vertical = vector.y;
		_horizontal = vector.x;
		_animationController.SetInput(_vertical, _horizontal);
		if (!HasEverMoved && (Mathf.Abs(_vertical) > 0.1f || Mathf.Abs(_horizontal) > 0.1f))
		{
			HasEverMoved = true;
		}
		if (Mathf.Abs(_vertical) > 0.1f || Mathf.Abs(_horizontal) > 0.1f)
		{
			Actor.PlaySound(Actor.PlayerSounds.Running);
		}
		if (_player.GetButtonDown(4))
		{
			if (_currentInteractedSpawnPoint.Count > 0 && !_hands.AreHandsFull)
			{
				TryCollect();
				return;
			}
			if (isNearRescuePod && (_scavenge.ScavengeState == ScavengeState.Hunt || _scavenge.ScavengeState == ScavengeState.Tutorial))
			{
				ClearHand();
				return;
			}
			if (_currentInteractedSpawnPoint.Count > 0 && _hands.AreHandsFull)
			{
				TryCollect();
				return;
			}
			Actor.SendEvent(ActorEvents.PickUpFail_NoItemAvailable);
			_hands.NoItemAvailable();
		}
	}

	private void FixedUpdate()
	{
		if (_pauseMovement || _stopMovement)
		{
			_characterController.velocity = new Vector3(_horizontal * 0f, _characterController.velocity.y, _vertical * 0f);
		}
		else
		{
			_characterController.velocity = new Vector3(_horizontal * Actor.Speed, _characterController.velocity.y, _vertical * Actor.Speed);
		}
	}

	private bool CheckIfCrewSpaceAvailable(IItem itm)
	{
		ScavengeCharacterItem scavengeCharacterItem = itm as ScavengeCharacterItem;
		if (scavengeCharacterItem != null && scavengeCharacterItem.characterList.CharactersInGame.Count >= MAX_CREW_SIZE)
		{
			return false;
		}
		return true;
	}

	public PickUpResult TryCollect()
	{
		Actor.SendEvent(ActorEvents.PickUpTry);
		ItemSpawnPoint itemSpawnPoint = _currentInteractedSpawnPoint.LastElement();
		if (itemSpawnPoint.IsItemAvailable)
		{
			if (_hands.AvailableHands >= itemSpawnPoint.Item.ScavengeItemData.Weight)
			{
				if (!CheckIfCrewSpaceAvailable(itemSpawnPoint.Item.ScavengeItemData.survivalItemRef))
				{
					Actor.SendEvent(ActorEvents.Crew_NoSpaceLeft);
					Actor.PlaySound(Actor.PlayerSounds.Nope);
					return PickUpResult.Crew_norSpaceAvailable;
				}
				Instance._pauseMovement = true;
				Instance._animationController.Catch = true;
				_hands.SetHands(itemSpawnPoint.Item.ScavengeItemData);
				ScavangeItemData scavengeItemData = itemSpawnPoint.Item.ScavengeItemData;
				itemSpawnPoint.Collect(_scavenge.GetPickUpDelay());
				Actor.SendEvent(ActorEvents.PickUpSuccess);
				OnPickUp.Invoke(scavengeItemData, itemSpawnPoint);
				if (_hands.AvailableHands == 0)
				{
					Actor.PlaySound(Actor.PlayerSounds.FullInventory);
					Actor.SendEvent(ActorEvents.HandFull);
				}
				return PickUpResult.Success;
			}
			Actor.PlaySound(Actor.PlayerSounds.Nope);
			Actor.SendEvent(ActorEvents.PickUpFail_NotEnoughHands);
			itemSpawnPoint.Item.SendEvent(ScavengeItemEvents.PickUpFail_NotEnoughHands);
			_hands.NotEnoughHands();
			return PickUpResult.NoHandsAvailable;
		}
		Actor.SendEvent(ActorEvents.PickUpFail_NoItemAvailable);
		_hands.NoItemAvailable();
		return PickUpResult.NoItemAvailable;
	}

	private void ClearHand()
	{
		if (!_isDropping && _hands.IsHoldingSomething())
		{
			Actor.PlaySound(Actor.PlayerSounds.DropToShuttle);
			_isDropping = true;
			Instance._pauseMovement = true;
			Instance._animationController.Catch = true;
			StartCoroutine(DelayedClearHand());
		}
		else if (_scavenge.CanActiveJump())
		{
			_scavenge.SkipToOutro();
		}
		else
		{
			Actor.SendEvent(ActorEvents.PickUpFail_NoItemAvailable);
			_hands.NoItemAvailable();
		}
	}

	public void ClearHandForced()
	{
		_hands.ClearHands();
	}

	private IEnumerator DelayedClearHand()
	{
		if (_hands.IsHoldingSomething())
		{
			OnAirlockDrop.Invoke();
			Actor.SendEvent(ActorEvents.AboutToDropItemsToPod);
			_scavenge.RescurePodEntrance.SendEvent(RescurePodEntranceEvents.AboutToClearHands);
			_hands.AbouToClearHands();
			yield return _scavenge.GetDropDelay();
			Actor.SendEvent(ActorEvents.DropItemsToPod);
			_scavenge.RescurePodEntrance.SendEvent(RescurePodEntranceEvents.ClearHands);
			_hands.ClearHands();
			_isDropping = false;
		}
	}

	public void AddSpawnPointInRange(ItemSpawnPoint isp)
	{
		if (isp == null || !isp.IsItemAvailable)
		{
			return;
		}
		if (_currentInteractedSpawnPoint.Count > 0)
		{
			ItemSpawnPoint itemSpawnPoint = _currentInteractedSpawnPoint.LastElement();
			if (itemSpawnPoint != null)
			{
				itemSpawnPoint.DeHighLight();
			}
		}
		_currentInteractedSpawnPoint.Add(isp);
		int freeHands = _hands.AvailableHands;
		if (!CheckIfCrewSpaceAvailable(isp.Item.ScavengeItemData.survivalItemRef))
		{
			freeHands = 0;
		}
		isp.HighLight(freeHands);
	}

	public void RemoveSpawnPointFromRange(ItemSpawnPoint isp)
	{
		try
		{
			if (isp == null)
			{
				return;
			}
			isp.DeHighLight();
			_currentInteractedSpawnPoint.Remove(isp);
			if (_currentInteractedSpawnPoint.Count <= 0)
			{
				return;
			}
			ItemSpawnPoint itemSpawnPoint = _currentInteractedSpawnPoint.LastElement();
			if (itemSpawnPoint != null)
			{
				int freeHands = _hands.AvailableHands;
				if (!CheckIfCrewSpaceAvailable(itemSpawnPoint.Item.ScavengeItemData.survivalItemRef))
				{
					freeHands = 0;
				}
				itemSpawnPoint.HighLight(freeHands);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
	}

	internal void RescuePodExit()
	{
		isNearRescuePod = false;
		Actor.SendEvent(ActorEvents.RescurePodEntrance_OnTriggerExit);
	}

	internal void RescuePodEnter()
	{
		isNearRescuePod = true;
		Actor.SendEvent(ActorEvents.RescurePodEntrance_OnTriggerEnter);
	}

	private void OnDestroy()
	{
		_scavenge = null;
		if (Instance == this)
		{
			Instance = null;
		}
	}
}
