using System;
using FMODUnity;
using UltimateWater;
using UnityEngine;

public class ThrowableComponent_NetGun : ThrowableComponent
{
	[SerializeField]
	private Animator gunAnimator;

	[SerializeField]
	private float originShotCooldown = 1f;

	private float currentShotCooldown;

	private bool isCooldownsDeclining = true;

	[SerializeField]
	private float reloadDuration = 1.5f;

	[SerializeField]
	private bool isReloaded;

	[SerializeField]
	private Throwable_Object canisterShellObject;

	[SerializeField]
	private ParticleSystem netGunFireParticleEffect;

	[SerializeField]
	private GameObject canisterShellModel;

	[SerializeField]
	private StudioEventEmitter eventEmitter_Reload;

	[SerializeField]
	private StudioEventEmitter eventEmitter_Shoot;

	[SerializeField]
	private float reloadSoundDelay;

	private bool reloadStarted;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CancelInvoke();
	}

	protected override void Start()
	{
		base.Start();
		if (!playerNetwork.IsLocalPlayer)
		{
			PlayerAnimator animator = playerNetwork.Animator;
			animator.Action_OnDeSerializeNetworkProperties = (Action<Message_Player_Update>)Delegate.Combine(animator.Action_OnDeSerializeNetworkProperties, new Action<Message_Player_Update>(OnNetworkProperties));
		}
	}

	public void AnimationReload()
	{
		Invoke("ReloadSoundDelayed", reloadSoundDelay);
	}

	public override void OnSelect()
	{
		canisterShellModel.gameObject.SetActive(value: true);
		base.OnSelect();
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (playerNetwork != null && playerNetwork.IsLocalPlayer)
		{
			if (HasAmmo())
			{
				StartReload();
			}
			else
			{
				ResetThrowable();
			}
		}
	}

	public override void OnDeSelect()
	{
		if (playerNetwork != null)
		{
			canisterShellModel.gameObject.SetActive(value: false);
			if (playerNetwork.IsLocalPlayer)
			{
				isReloaded = false;
				base.OnDeSelect();
			}
		}
	}

	public void SpawnNetParticleEffect()
	{
		if (netGunFireParticleEffect != null)
		{
			netGunFireParticleEffect.Play();
		}
	}

	public void StopParticleEffect()
	{
		if (netGunFireParticleEffect != null)
		{
			netGunFireParticleEffect.StopSafe();
		}
	}

	public override void HandleShoot()
	{
		if (isReloaded && currentShotCooldown <= 0f)
		{
			if (gunAnimator != null)
			{
				gunAnimator.SetTrigger("ItemHitTrigger");
			}
			if (playerNetwork.Animator != null)
			{
				playerNetwork.Animator.SetAnimation(PlayerAnimation.Trigger_ItemHitTrigger);
			}
			base.HandleShoot();
			if (playerNetwork.IsLocalPlayer)
			{
				ResetShotCooldown();
				SetCooldownDecreasingFalse();
				Invoke("SetCooldownDecreasingTrue", throwDelay);
				SetIsNotReloaded();
				Invoke("LateAmmoCheck", throwDelay + 0.1f);
			}
		}
	}

	public void LateAmmoCheck()
	{
		bool flag = HasAmmo();
		if (playerNetwork.Animator != null)
		{
			playerNetwork.Animator.anim.SetBool("HasAmmo", flag);
		}
		playerNetwork.Inventory.RemoveDurabillityFromHotSlot();
		if (flag)
		{
			StartReload();
		}
	}

	public override void HandleReleaseButton(bool releaseButtonPressed, bool fireWithoutReleaseButton)
	{
		if (isReloaded && currentShotCooldown <= 0f)
		{
			base.HandleReleaseButton(releaseButtonPressed, fireWithoutReleaseButton: true);
		}
	}

	protected override void HandleLocalClient()
	{
		if (!isReloaded)
		{
			if (HasAmmo())
			{
				if (!reloadStarted)
				{
					StartReload();
				}
				if (isTextShown)
				{
					textManager.HideDisplayTexts(0);
					isTextShown = false;
				}
			}
			else if (CanvasHelper.ActiveMenu == MenuType.None || CanvasHelper.ActiveMenu == MenuType.ChatField)
			{
				textManager.ShowText(Helper.GetTerm("Game/NoAmmo"), 0, clearAllTexts: false);
				isTextShown = true;
			}
		}
		else
		{
			if (isCooldownsDeclining)
			{
				currentShotCooldown -= Time.deltaTime;
			}
			if (playerNetwork.PersonController.SubmersionState != SubmersionState.Full)
			{
				base.HandleLocalClient();
			}
			else
			{
				ResetThrowable();
			}
		}
	}

	protected override void CallThrowEvent(Vector3 position, Vector3 force)
	{
		base.CallThrowEvent(position, force);
		if (canisterShellObject != null)
		{
			SpawnShell(++uniqueThrowableIndex, 0u, position, force);
		}
		SpawnNetParticleEffect();
		if (eventEmitter_Shoot != null)
		{
			eventEmitter_Shoot.PlaySafe();
		}
	}

	private void StartReload()
	{
		reloadStarted = true;
		canisterShellModel.gameObject.SetActive(value: true);
		Invoke("SetIsReloaded", reloadDuration);
	}

	private void ReloadSoundDelayed()
	{
		if (eventEmitter_Reload != null)
		{
			eventEmitter_Reload.PlaySafe();
		}
	}

	private void SetIsReloaded()
	{
		isReloaded = true;
		isCooldownsDeclining = true;
		reloadStarted = false;
	}

	private void SetIsNotReloaded()
	{
		isReloaded = false;
	}

	private void SpawnShell(uint throwableIndex, uint throwablePickupIndex, Vector3 pos, Vector3 force)
	{
		if (force != Vector3.zero)
		{
			force /= 4f;
		}
		UnityEngine.Object.Instantiate(canisterShellObject, pos, Quaternion.LookRotation(shootPoint.forward), null).Initialize(this, throwableIndex, throwablePickupIndex, force, playerNetwork.IsLocalPlayer);
	}

	private void ResetShotCooldown()
	{
		currentShotCooldown = originShotCooldown;
	}

	private void SetCooldownDecreasingTrue()
	{
		isCooldownsDeclining = true;
	}

	private void SetCooldownDecreasingFalse()
	{
		isCooldownsDeclining = false;
	}

	private void OnNetworkProperties(Message_Player_Update msg)
	{
		if (!gameObject.activeInHierarchy || playerNetwork.IsLocalPlayer)
		{
			return;
		}
		bool flag = false;
		if (gunAnimator != null)
		{
			flag = gunAnimator.GetBool("ItemHit");
			gunAnimator.SetBool("ItemHit", msg.Anim_ItemHit);
		}
		if (playerNetwork.Animator != null)
		{
			playerNetwork.Animator.anim.SetBool("ItemHit", msg.Anim_ItemHit);
		}
		if (!flag && msg.Anim_ItemHit && !eventEmitter_Reload.IsPlaying())
		{
			eventEmitter_Reload.Play();
		}
		if (!(gunAnimator != null))
		{
			return;
		}
		string[] anim_triggers = msg.anim_triggers;
		for (int i = 0; i < anim_triggers.Length; i++)
		{
			if (anim_triggers[i] == "ItemHitTrigger")
			{
				gunAnimator.SetTrigger("ItemHitTrigger");
				break;
			}
		}
	}
}
