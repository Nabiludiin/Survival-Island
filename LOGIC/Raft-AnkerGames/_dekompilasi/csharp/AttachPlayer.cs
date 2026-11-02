using System;
using UnityEngine;

public class AttachPlayer : MonoBehaviour
{
	[Serializable]
	public class CameraSettings
	{
		public bool enabled;

		public bool isChild;

		public bool keepRotationOnEnter;

		public float localRotationOnEnter;

		public bool keepRotationOnExit;

		public float localRotationOnExit;
	}

	public Action<Network_Player> OnStartCarryPlayer;

	public Action<Network_Player> OnStopCarryPlayer;

	public Transform firstPersonParent;

	public Transform thirdPersonParent;

	public Transform stopCarryPlayerTransform;

	public Vector3 stopCarryPlayerOffset;

	public string animationBoolParameter = string.Empty;

	public PlayerFullBodyAnimation fullBodyAnimation;

	public bool autoUpdatePositionToParents = true;

	public bool disableMouseLook = true;

	public bool disablePlayerCollider;

	public bool disableCameraBob = true;

	public bool hideItemInHand;

	public bool setPlayerIsBusy = true;

	public bool dropPlayerCarriedObject;

	[SerializeField]
	private CameraSettings cameraX;

	[HideInInspector]
	public Network_Player carriedPlayer;

	public bool HasPlayerAttached => carriedPlayer != null;

	public void OnDestroy()
	{
		if (GameManager.IsQuittingGame)
		{
			return;
		}
		if (carriedPlayer != null)
		{
			if (!string.IsNullOrEmpty(animationBoolParameter))
			{
				carriedPlayer.Animator.anim.SetBool(animationBoolParameter, value: false);
			}
			if (fullBodyAnimation != PlayerFullBodyAnimation.None_0)
			{
				carriedPlayer.Animator.SetAnimation(PlayerFullBodyAnimation.None_0);
			}
		}
		StopCarryingPlayer(manipulatePosition: false);
	}

	public void Update()
	{
		if (!HasPlayerAttached)
		{
			return;
		}
		if (carriedPlayer.PlayerScript.IsDead)
		{
			StopCarryingPlayer(manipulatePosition: true);
			return;
		}
		if (autoUpdatePositionToParents)
		{
			Transform transform = firstPersonParent;
			if ((!carriedPlayer.IsLocalPlayer || carriedPlayer.currentModel.thirdPersonSettings.ThirdPersonModel) && thirdPersonParent != null)
			{
				transform = thirdPersonParent;
			}
			carriedPlayer.transform.SetParentSafe(transform);
			if (carriedPlayer.transform.parent != null)
			{
				carriedPlayer.transform.localPosition = Vector3.zero;
				carriedPlayer.transform.localEulerAngles = Vector3.zero;
				if (transform == thirdPersonParent)
				{
					carriedPlayer.playerPivot.localEulerAngles = Vector3.zero;
				}
			}
		}
		bool flag = !carriedPlayer.IsLocalPlayer || carriedPlayer.currentModel.thirdPersonSettings.ThirdPersonModel;
		if (!string.IsNullOrEmpty(animationBoolParameter) && carriedPlayer.Animator.anim.GetBool(animationBoolParameter) != flag)
		{
			carriedPlayer.Animator.anim.SetBool(animationBoolParameter, flag);
		}
		if (fullBodyAnimation != PlayerFullBodyAnimation.None_0)
		{
			carriedPlayer.Animator.SetAnimation(flag ? fullBodyAnimation : PlayerFullBodyAnimation.None_0);
		}
	}

	public bool StartCarryingPlayer(Network_Player playerToAttach, bool manipulatePosition = true)
	{
		if (carriedPlayer != null)
		{
			return false;
		}
		PlayerStats stats = playerToAttach.Stats;
		stats.OnDeath = (Action)Delegate.Combine(stats.OnDeath, new Action(HandleAttachedPlayerDeath));
		carriedPlayer = playerToAttach;
		if (!string.IsNullOrEmpty(animationBoolParameter))
		{
			carriedPlayer.Animator.anim.SetBool(animationBoolParameter, value: true);
		}
		if (fullBodyAnimation != PlayerFullBodyAnimation.None_0)
		{
			carriedPlayer.Animator.SetAnimation(fullBodyAnimation);
		}
		if (playerToAttach.IsLocalPlayer)
		{
			playerToAttach.PlayerNetworkManager.IsAttached = true;
			playerToAttach.PersonController.SwitchControllerType(ControllerType.Ground);
			playerToAttach.PersonController.ResetPlayerPivotToOriginal();
			if (disableMouseLook)
			{
				playerToAttach.PlayerScript.SetMouseLookScripts(canLook: false);
			}
			else if (cameraX.enabled && cameraX.isChild)
			{
				playerToAttach.PlayerScript.mouseLookXScript.isChild = true;
				if (!cameraX.keepRotationOnEnter)
				{
					playerToAttach.PlayerScript.mouseLookXScript.SetTargetRotX(cameraX.localRotationOnEnter);
				}
				else
				{
					playerToAttach.PlayerScript.mouseLookXScript.SetTargetRotXToCurrentRotation();
				}
			}
			if (disableCameraBob)
			{
				playerToAttach.cameraBobAnimator.SetBool("InWater", value: false);
				playerToAttach.cameraBobAnimator.SetFloat("Speed", 0f);
			}
			if (hideItemInHand)
			{
				playerToAttach.PlayerItemManager.SelectUsable(null);
			}
			if (dropPlayerCarriedObject && playerToAttach.CarryingComponent.IsCarrying)
			{
				playerToAttach.CarryingComponent.CarriedObject.OnStopCarry?.Invoke(playerToAttach, arg2: true);
			}
			PlayerItemManager.IsBusy = setPlayerIsBusy;
		}
		playerToAttach.PersonController.enabled = false;
		playerToAttach.PersonController.IsMovementFree = false;
		playerToAttach.PersonController.waterFloat.setToSurface = false;
		if (disablePlayerCollider)
		{
			playerToAttach.PersonController.controller.enabled = false;
		}
		playerToAttach.transform.SetParent(firstPersonParent);
		if (manipulatePosition)
		{
			playerToAttach.transform.localPosition = Vector3.zero;
		}
		playerToAttach.transform.localEulerAngles = Vector3.zero;
		playerToAttach.playerPivot.localEulerAngles = Vector3.zero;
		OnStartCarryPlayer?.Invoke(playerToAttach);
		return true;
	}

	public void StopCarryingPlayer(bool manipulatePosition, bool manipulateEuler = true)
	{
		if (!HasPlayerAttached)
		{
			return;
		}
		OnStopCarryPlayer?.Invoke(carriedPlayer);
		PlayerStats stats = carriedPlayer.Stats;
		stats.OnDeath = (Action)Delegate.Remove(stats.OnDeath, new Action(HandleAttachedPlayerDeath));
		if (!string.IsNullOrEmpty(animationBoolParameter))
		{
			carriedPlayer.Animator.anim.SetBool(animationBoolParameter, value: false);
		}
		if (fullBodyAnimation != PlayerFullBodyAnimation.None_0)
		{
			carriedPlayer.Animator.SetAnimation(PlayerFullBodyAnimation.None_0);
		}
		carriedPlayer.transform.SetParent(null);
		if (manipulatePosition)
		{
			if (stopCarryPlayerTransform != null)
			{
				carriedPlayer.transform.position = stopCarryPlayerTransform.position;
			}
			else if (stopCarryPlayerOffset != Vector3.zero)
			{
				carriedPlayer.transform.position = transform.position + stopCarryPlayerOffset;
			}
		}
		if (manipulateEuler)
		{
			carriedPlayer.transform.eulerAngles = Vector3.zero;
		}
		carriedPlayer.PersonController.SetNetworkPosition(carriedPlayer.transform.position);
		carriedPlayer.PersonController.IsMovementFree = true;
		carriedPlayer.PersonController.enabled = true;
		if (disablePlayerCollider)
		{
			carriedPlayer.PersonController.controller.enabled = true;
		}
		if (carriedPlayer.IsLocalPlayer)
		{
			carriedPlayer.PlayerNetworkManager.IsAttached = false;
			PlayerItemManager.IsBusy = PlayerItemManager.ShouldItBeBusy();
			if (disableMouseLook)
			{
				carriedPlayer.PlayerScript.SetMouseLookScripts(canLook: true);
			}
			else
			{
				if (cameraX.enabled && cameraX.isChild)
				{
					carriedPlayer.PlayerScript.mouseLookXScript.isChild = false;
				}
				if (cameraX.enabled)
				{
					if (cameraX.keepRotationOnExit)
					{
						carriedPlayer.PlayerScript.mouseLookXScript.SetTargetRotXToCurrentRotation();
					}
					else
					{
						carriedPlayer.PlayerScript.mouseLookXScript.SetTargetRotX(cameraX.localRotationOnExit);
					}
					carriedPlayer.PlayerScript.mouseLookXScript.ApplyRotations();
				}
			}
			if (hideItemInHand)
			{
				carriedPlayer.Inventory.hotbar.ReselectCurrentSlot();
			}
			carriedPlayer.PersonController.DetachFromLadders();
		}
		carriedPlayer = null;
	}

	private void HandleAttachedPlayerDeath()
	{
		if (HasPlayerAttached)
		{
			StopCarryingPlayer(manipulatePosition: true);
		}
	}
}
