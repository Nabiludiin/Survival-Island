using System;
using FMOD.Studio;
using FMODUnity;
using UltimateWater;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Throwable))]
public class Hook : MonoBehaviour
{
	public Collider hookObjectCollider;

	public ItemCollector hookItemCollector;

	public float pullSpeed = 3f;

	[HideInInspector]
	public Throwable throwable;

	[SerializeField]
	private Rope rope;

	[SerializeField]
	private GameObject leftHandRopeModel;

	[SerializeField]
	private GameObject hookMesh;

	[SerializeField]
	private GameObject ropeMesh;

	public float gatherTime = 5f;

	[SerializeField]
	private StudioEventEmitter eventEmitter_gather;

	private float gatherTimer;

	private CanvasHelper canvas;

	private PickupItem currentGatherItem;

	private float resetCooldown;

	protected PersonController personController;

	private Transform ropeConnectTransform;

	protected Network_Player playerNetwork;

	private ParticleSystem watersplash_hook_pulling;

	private Transform raftTransform;

	[SerializeField]
	private StudioEventEmitter eventEmitter_hookPull;

	[SerializeField]
	private InputActionReference inputCancel;

	private void Awake()
	{
		this.throwable = GetComponent<Throwable>();
		personController = GetComponentInParent<PersonController>();
		Throwable throwable = this.throwable;
		throwable.Action_OnThrow = (Action)Delegate.Combine(throwable.Action_OnThrow, new Action(OnTrow));
		Throwable throwable2 = this.throwable;
		throwable2.Action_OnWaterHit = (Action)Delegate.Combine(throwable2.Action_OnWaterHit, new Action(OnHitWater));
		hookObjectCollider.enabled = false;
	}

	protected virtual void Start()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
		canvas = ComponentManager<CanvasHelper>.Value;
		raftTransform = ComponentManager<Raft>.Value.transform;
		if (!playerNetwork.IsLocalPlayer)
		{
			throwable.throwableObject.gameObject.layer = playerNetwork.gameObject.layer;
		}
		ropeConnectTransform = playerNetwork.currentModel.ropeConnectTransform;
		PersonController.CameraSubmersionAction = (Action<SubmersionState>)Delegate.Combine(PersonController.CameraSubmersionAction, new Action<SubmersionState>(CameraSubmersionState));
	}

	protected virtual void Update()
	{
		if (!playerNetwork.IsLocalPlayer)
		{
			if (!throwable.InHand && throwable.InWater)
			{
				RotateHookTowardsPlayer();
			}
			if (throwable.InWater && throwable.networkPulling)
			{
				eventEmitter_hookPull.EventInstance.getPlaybackState(out var state);
				if (state == PLAYBACK_STATE.STOPPED)
				{
					eventEmitter_hookPull.Play();
				}
				if (watersplash_hook_pulling == null)
				{
					watersplash_hook_pulling = ParticleManager.PlaySystem("WaterSplash_Hook_Pulling", hookItemCollector.transform.position).GetComponent<ParticleSystem>();
				}
				if (!watersplash_hook_pulling.isPlaying)
				{
					watersplash_hook_pulling.Play();
				}
				else
				{
					Vector3 vector = throwable.GetPositon() - transform.position;
					vector.y = 0f;
					vector.Normalize();
					watersplash_hook_pulling.transform.position = hookItemCollector.transform.position + Vector3.up * 0.2f + vector * -1f;
				}
			}
			else
			{
				eventEmitter_hookPull.EventInstance.getPlaybackState(out var state2);
				if (state2 != PLAYBACK_STATE.STOPPED)
				{
					eventEmitter_hookPull.Stop();
				}
				if (watersplash_hook_pulling != null && watersplash_hook_pulling.isPlaying)
				{
					watersplash_hook_pulling.Stop();
				}
			}
		}
		else
		{
			PickupItem mineableItemAtCursor = playerNetwork.GetMineableItemAtCursor();
			bool flag = personController.SubmersionState != SubmersionState.None || personController.controllerType != ControllerType.Ground || mineableItemAtCursor != null;
			if (flag && !throwable.IsLocked)
			{
				throwable.ResetCanThrow();
			}
			if (throwable.IsLocked && (!flag || CanvasHelper.ActiveMenu != MenuType.None))
			{
				FinishGathering();
			}
			throwable.IsLocked = flag;
			if (CanvasHelper.ActiveMenu != MenuType.None)
			{
				return;
			}
			if (throwable.IsLocked)
			{
				HandleGathering(mineableItemAtCursor);
			}
			else
			{
				if (throwable.InWater)
				{
					RotateHookTowardsPlayer();
					throwable.throwableObject.SetParent((playerNetwork.transform.parent == null) ? null : raftTransform);
					if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(throwable.ThrowableAction, "LMB"))
					{
						playerNetwork.Animator.anim.SetBool("ItemHit", value: true);
						hookItemCollector.hapticEvent = true;
						if (watersplash_hook_pulling == null)
						{
							watersplash_hook_pulling = ParticleManager.PlaySystem("WaterSplash_Hook_Pulling", hookItemCollector.transform.position).GetComponent<ParticleSystem>();
						}
						if (!watersplash_hook_pulling.isPlaying)
						{
							watersplash_hook_pulling.Play();
						}
						if (hookItemCollector.collectedItems.Count > 0)
						{
							SimpleMonoBehaviourSingleton<HapticManager>.Instance?.ProgressProcedural(HapticEvent.Rumble, 0.01f);
						}
						eventEmitter_hookPull.EventInstance.getPlaybackState(out var state3);
						if (state3 == PLAYBACK_STATE.STOPPED)
						{
							eventEmitter_hookPull.Play();
						}
						Vector3 positon = throwable.GetPositon();
						Vector3 vector2 = positon - transform.position;
						vector2.y = 0f;
						vector2.Normalize();
						positon -= vector2 * Time.deltaTime * throwable.ThrowableAction.ReadValue<float>() * pullSpeed;
						throwable.SetPosition(positon);
						watersplash_hook_pulling.transform.position = hookItemCollector.transform.position + Vector3.up * 0.2f + vector2 * -1f;
						Vector3 position = transform.position;
						position.y = positon.y;
						float num = Vector3.Distance(positon, position);
						if (resetCooldown >= 60f && num <= 1f)
						{
							ResetHookToPlayer();
							int count = hookItemCollector.collectedItems.Count;
							hookItemCollector.AddCollectedItemsToPlayer(playerNetwork);
							if (count > 0)
							{
								playerNetwork.Inventory.RemoveDurabillityFromHotSlot();
							}
						}
					}
					else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(throwable.ThrowableAction, "LMB"))
					{
						if (watersplash_hook_pulling != null)
						{
							watersplash_hook_pulling.Stop();
						}
						eventEmitter_hookPull.Stop();
					}
					else if (resetCooldown >= 60f && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputCancel, "RMB") && hookItemCollector.collectedItems.Count <= 0)
					{
						throwable.ResetCanThrow();
						ResetHookToPlayer();
					}
					else
					{
						playerNetwork.Animator.anim.SetBool("ItemHit", value: false);
					}
				}
				else if (throwable.UsingPhysicsCollider && !throwable.InHand && resetCooldown >= 60f && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputCancel, "RMB"))
				{
					throwable.ResetCanThrow();
					ResetHookToPlayer();
					hookItemCollector.AddCollectedItemsToPlayer(playerNetwork);
				}
				if (throwable.InHand)
				{
					if (throwable.CanThrow && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(throwable.ThrowableAction, "LMB"))
					{
						playerNetwork.Animator.anim.SetBool("HookThrow", value: true);
					}
					if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(throwable.ThrowableAction, "LMB"))
					{
						throwable.ResetCanThrow();
					}
				}
				else
				{
					resetCooldown++;
				}
			}
		}
		if (!rope.gameObject.activeInHierarchy)
		{
			return;
		}
		if (playerNetwork.IsLocalPlayer)
		{
			int layer = playerNetwork.gameObject.layer;
			if (playerNetwork.IsLocalPlayer && !playerNetwork.currentModel.thirdPersonSettings.ThirdPersonModel && throwable.InHand)
			{
				layer = LayerMasks.MASK_HandCamera.value;
			}
			hookMesh.SetLayerSafe(layer);
			ropeMesh.SetLayerSafe(layer);
		}
		if (playerNetwork.IsLocalPlayer && !playerNetwork.currentModel.thirdPersonSettings.ThirdPersonModel)
		{
			rope.SetPosition(0, ropeConnectTransform, Helper.WorldPointToFOVPointLocal(ropeConnectTransform.position, ropeConnectTransform));
			rope.SetPosition(1, throwable.throwableObject);
		}
		else
		{
			rope.SetPosition(0, ropeConnectTransform);
			rope.SetPosition(1, throwable.throwableObject);
		}
	}

	private void OnDestroy()
	{
		PersonController.CameraSubmersionAction = (Action<SubmersionState>)Delegate.Remove(PersonController.CameraSubmersionAction, new Action<SubmersionState>(CameraSubmersionState));
	}

	public void OnSelect()
	{
		if (canvas == null)
		{
			canvas = ComponentManager<CanvasHelper>.Value;
		}
		rope.gameObject.SetActive(value: true);
		ResetGatherTimer();
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		playerNetwork.Animator.anim.SetBool("HookInHand", value: true);
		playerNetwork.Animator.anim.SetBool("HookThrow", value: false);
		playerNetwork.Animator.anim.SetBool("ItemHit", value: false);
		leftHandRopeModel.SetActive(value: true);
		resetCooldown = 0f;
		throwable.ResetLastKnownForce();
	}

	public void OnDeSelect()
	{
		rope.gameObject.SetActive(value: false);
		if (watersplash_hook_pulling != null)
		{
			watersplash_hook_pulling.Stop();
		}
		if (eventEmitter_hookPull != null)
		{
			eventEmitter_hookPull.Stop();
		}
		if (eventEmitter_gather != null)
		{
			eventEmitter_gather.Stop();
		}
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		playerNetwork.Animator.anim.SetBool("HookInHand", value: false);
		playerNetwork.Animator.anim.SetBool("HookThrow", value: false);
		playerNetwork.Animator.anim.SetBool("ItemHit", value: false);
		leftHandRopeModel.SetActive(value: false);
		if (playerNetwork.IsLocalPlayer && canvas != null)
		{
			canvas.SetAimSprite(AimSprite.Default);
		}
		throwable.ResetLastKnownForce();
	}

	private void CameraSubmersionState(SubmersionState state)
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (!(playerNetwork != null) || !playerNetwork.IsLocalPlayer)
		{
			return;
		}
		switch (state)
		{
		case SubmersionState.Full:
			if (!throwable.InHand)
			{
				int count = hookItemCollector.collectedItems.Count;
				hookItemCollector.AddCollectedItemsToPlayer(playerNetwork);
				throwable.ResetCanThrow();
				ResetHookToPlayer();
				if (count > 0)
				{
					playerNetwork.Inventory.RemoveDurabillityFromHotSlot();
				}
			}
			playerNetwork.Animator.anim.SetBool("HookInHand", value: true);
			playerNetwork.Animator.anim.SetBool("HookThrow", value: false);
			playerNetwork.Animator.anim.SetBool("ItemHit", value: false);
			break;
		case SubmersionState.None:
			playerNetwork.Animator.anim.SetBool("ItemHit", value: false);
			break;
		}
	}

	private void OnTrow()
	{
		if (playerNetwork.IsLocalPlayer && playerNetwork.Animator.anim.GetBool("HookThrow"))
		{
			playerNetwork.Animator.anim.SetBool("HookInHand", value: false);
			playerNetwork.Animator.anim.SetBool("HookThrow", value: false);
		}
	}

	private void OnHitWater()
	{
		ParticleManager.PlaySystem("WaterSplash_Hook", hookItemCollector.transform.position + Vector3.up * 0.2f);
		if (playerNetwork.IsLocalPlayer)
		{
			hookObjectCollider.enabled = true;
		}
	}

	protected void ResetHookToPlayer()
	{
		if (watersplash_hook_pulling != null)
		{
			PoolManager.ReturnObject(watersplash_hook_pulling.gameObject);
			watersplash_hook_pulling = null;
		}
		eventEmitter_hookPull.Stop();
		throwable.PickupThrowable();
		throwable.throwablePhysicsCollider.enabled = false;
		hookObjectCollider.enabled = false;
		playerNetwork.Animator.anim.SetBool("HookInHand", value: true);
		playerNetwork.Animator.anim.SetBool("HookThrow", value: false);
		playerNetwork.Animator.anim.SetBool("ItemHit", value: false);
		resetCooldown = 0f;
	}

	private void RotateHookTowardsPlayer()
	{
		Vector3 position = transform.position;
		Vector3 position2 = hookItemCollector.transform.position;
		position2.y = position.y;
		Vector3 up = position2 - position;
		up.Normalize();
		hookItemCollector.transform.up = up;
		hookItemCollector.transform.eulerAngles = new Vector3(-270f, hookItemCollector.transform.eulerAngles.y, hookItemCollector.transform.eulerAngles.z);
	}

	private void HandleGathering(PickupItem item)
	{
		if (!playerNetwork.IsLocalPlayer)
		{
			return;
		}
		if (personController.SubmersionState != SubmersionState.None || personController.controllerType == ControllerType.Water || item != null)
		{
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(throwable.ThrowableAction, "LMB"))
			{
				canvas.SetAimSprite(AimSprite.Default);
				playerNetwork.Animator.anim.SetBool("ItemHit", value: true);
				if (item != null)
				{
					if (item != currentGatherItem)
					{
						StartCollecting(item);
					}
					gatherTimer += Time.deltaTime * throwable.ThrowableAction.ReadValue<float>();
					float num = gatherTimer / gatherTime;
					if (num >= 1f)
					{
						playerNetwork.PickupScript.PickupItem(item, forcePickup: true);
						StopCollecting();
						playerNetwork.Inventory.RemoveDurabillityFromHotSlot();
						if (eventEmitter_gather != null)
						{
							eventEmitter_gather.EventInstance.keyOff();
						}
					}
					else
					{
						canvas.SetLoadCircle(num);
					}
				}
				else
				{
					StopCollecting();
					if (eventEmitter_gather != null)
					{
						eventEmitter_gather.Stop();
					}
				}
			}
			else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(throwable.ThrowableAction, "LMB"))
			{
				FinishGathering();
			}
		}
		else
		{
			FinishGathering();
		}
	}

	private void FinishGathering()
	{
		ResetGatherTimer();
		playerNetwork.Animator.anim.SetBool("ItemHit", value: false);
		if (eventEmitter_gather != null)
		{
			eventEmitter_gather.Stop();
		}
	}

	private void StartCollecting(PickupItem item)
	{
		currentGatherItem = item;
		ResetGatherTimer();
	}

	private void StopCollecting()
	{
		if (currentGatherItem != null)
		{
			currentGatherItem = null;
			ResetGatherTimer();
		}
	}

	private void ResetGatherTimer()
	{
		gatherTimer = 0f;
		canvas.SetLoadCircle(state: false);
	}

	public void OnHookGather()
	{
		if (currentGatherItem != null)
		{
			if (eventEmitter_gather != null)
			{
				eventEmitter_gather.Play();
			}
			ParticleManager.PlaySystem("ReefPick", currentGatherItem.transform.position);
			SimpleMonoBehaviourSingleton<HapticManager>.Instance.PlayOneShot(HapticEvent.Hit);
		}
	}
}
