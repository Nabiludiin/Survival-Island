using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ChargeMeter))]
public class ThrowableComponent : MonoBehaviour_ID_Network
{
	public Action OnStartCharge;

	public Action<Vector3, Vector3> OnThrow;

	public FiringType firingType;

	[SerializeField]
	private ItemObjectEnabler throwablePrefabs;

	[SerializeField]
	protected ItemObjectEnabler throwableModelEnabler;

	[SerializeField]
	protected Transform shootPoint;

	[SerializeField]
	protected Vector3 throwForce = Vector3.zero;

	[SerializeField]
	protected float throwDelay;

	[SerializeField]
	private bool hideDuringCooldown;

	[SerializeField]
	private float hideDelay = 1f;

	[SerializeField]
	private string button_Charge = string.Empty;

	[SerializeField]
	private string button_Release = string.Empty;

	[SerializeField]
	private string button_Abort = string.Empty;

	[HideInInspector]
	public Network_Player playerNetwork;

	private List<Throwable_Object> thrownObjects = new List<Throwable_Object>();

	protected ChargeMeter chargeMeter;

	protected uint uniqueThrowableIndex;

	protected bool canThrow = true;

	private Item_Base currentAmmo;

	protected DisplayTextManager textManager;

	protected bool isTextShown;

	private bool hasAborted;

	private bool rumbling;

	protected virtual void Awake()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
		chargeMeter = GetComponent<ChargeMeter>();
	}

	protected virtual void Start()
	{
		textManager = ComponentManager<DisplayTextManager>.Value;
	}

	protected virtual void Update()
	{
		if (playerNetwork.IsLocalPlayer)
		{
			HandleLocalClient();
		}
	}

	protected virtual void HandleLocalClient()
	{
		currentAmmo = GetAmmo();
		if (firingType == FiringType.TYPE_CHARGE && currentAmmo != null && this is ThrowableComponent_Bow && chargeMeter.ChargeNormal > 0.45f)
		{
			SimpleMonoBehaviourSingleton<HapticManager>.Instance?.ProgressProcedural(HapticEvent.Rumble, 0.1f);
		}
		if (CanvasHelper.ActiveMenu != MenuType.None)
		{
			chargeMeter.Reset();
			return;
		}
		if (playerNetwork.Animator != null)
		{
			playerNetwork.Animator.anim.SetBool("HasAmmo", currentAmmo != null);
		}
		if (currentAmmo == null)
		{
			if (throwableModelEnabler != null)
			{
				throwableModelEnabler.DeActivateAllModels();
			}
			if (CanvasHelper.ActiveMenu == MenuType.None || CanvasHelper.ActiveMenu == MenuType.ChatField)
			{
				textManager.ShowText(Helper.GetTerm("Game/NoAmmo"), 0, clearAllTexts: false);
				isTextShown = true;
			}
			return;
		}
		if (isTextShown)
		{
			textManager.HideDisplayTexts(0);
			isTextShown = false;
		}
		if (canThrow && throwableModelEnabler != null)
		{
			throwableModelEnabler.ActivateModel(currentAmmo);
		}
		InputAction inputAction = PlayerInput.GetPlayerByIndex(0).actions["Fire"];
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Context"];
		bool releaseButtonPressed = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(inputAction, button_Release);
		bool flag = !string.IsNullOrEmpty(button_Abort);
		bool flag2 = flag && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "RMB");
		if (PlayerItemManager.IsBusy && !(chargeMeter.ChargeNormal > 0f))
		{
			return;
		}
		switch (firingType)
		{
		case FiringType.TYPE_CHARGE:
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputAction, "LMB") && canThrow)
			{
				if (chargeMeter.ChargeNormal == 0f)
				{
					CallStartChargeEvent();
				}
				PlayerItemManager.IsBusy = true;
				chargeMeter.Charge(SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad ? inputAction.ReadValue<float>() : 1f);
				if (flag)
				{
					textManager.ShowText(Helper.GetTerm("Controls/Cancel"), action.GetBindingDisplayString(), MyInput.Keybinds["RMB"].MainKey, 1, 0, clearAllTexts: false);
					isTextShown = true;
					if (flag2)
					{
						textManager.HideDisplayTexts(1);
						isTextShown = false;
						hasAborted = true;
						ReleaseHand();
						break;
					}
				}
			}
			HandleReleaseButton(releaseButtonPressed, fireWithoutReleaseButton: false);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(inputAction, button_Charge))
			{
				if (flag)
				{
					canThrow = true;
					textManager.HideDisplayTexts(1);
				}
				if (chargeMeter.ChargeNormal > 0f)
				{
					ResetThrowable();
				}
			}
			break;
		case FiringType.TYPE_CLICK:
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputAction, button_Charge) && canThrow)
			{
				HandleShoot();
			}
			break;
		}
	}

	public void OnThrowableCollision(Throwable_Object throwable, Collision collision, Network_Entity entity)
	{
		if (!(throwable == null) && collision != null)
		{
			Vector3 normal = ((collision.contacts != null && collision.contacts.Length != 0) ? collision.contacts[0].normal : Vector3.up);
			ParentType parentType = ParentType.None;
			parentType = ((!Helper.ObjectIsOnLayer(collision.gameObject, LayerMasks.MASK_ProjectileObstruction)) ? ParentType.LockedPivot : ParentType.None);
			Message_ThrowableComponent_Collision message = new Message_ThrowableComponent_Collision(Messages.ThrowableComponent_Collision, playerNetwork.Network.NetworkIDManager, ObjectIndex, throwable.throwableIndex, throwable.transform.position, normal, parentType, (entity == null) ? (-1) : ((int)entity.ObjectIndex));
			if (Raft_Network.IsHost)
			{
				playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				HandleCollision(throwable, throwable.transform.position, normal, parentType, entity);
			}
			else
			{
				playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void RemoveThrownObject(Throwable_Object thrownObject)
	{
		if (thrownObjects.Contains(thrownObject))
		{
			thrownObjects.Remove(thrownObject);
		}
	}

	public virtual bool HasAmmo()
	{
		if (GetAmmo() != null)
		{
			return true;
		}
		return false;
	}

	public virtual void HandleShoot()
	{
		if (!PlayerItemManager.IsBusy)
		{
			CallStartChargeEvent();
		}
		PlayerItemManager.IsBusy = true;
		if (GetThrowableFromAmmo(currentAmmo) != null)
		{
			StartCoroutine(StartThrow(throwDelay, chargeMeter.ChargeNormal));
		}
	}

	public virtual void HandleReleaseButton(bool releaseButtonPressed, bool fireWithoutReleaseButton)
	{
		if (!(chargeMeter.ChargeNormal <= 0f) && (fireWithoutReleaseButton || releaseButtonPressed) && GetThrowableFromAmmo(currentAmmo) != null)
		{
			StartCoroutine(StartThrow(throwDelay, chargeMeter.ChargeNormal));
			ReleaseButton();
		}
	}

	public virtual void OnSelect()
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		currentAmmo = GetAmmo();
		if (playerNetwork.Animator != null)
		{
			playerNetwork.Animator.anim.SetBool("HasAmmo", currentAmmo != null);
		}
		if (playerNetwork != null)
		{
			if (playerNetwork.IsLocalPlayer)
			{
				canThrow = true;
				hasAborted = false;
				CancelInvoke();
			}
			else if (throwableModelEnabler != null)
			{
				throwableModelEnabler.ActivateModel(0);
			}
		}
	}

	public virtual void OnDeSelect()
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (playerNetwork != null && playerNetwork.IsLocalPlayer)
		{
			PlayerItemManager.IsBusy = false;
			chargeMeter.Reset();
			CancelInvoke();
			if (textManager != null && isTextShown)
			{
				textManager.HideDisplayTexts();
				isTextShown = false;
			}
		}
	}

	private void HandleCollision(Throwable_Object throwable, Vector3 collisionPosition, Vector3 normal, ParentType parentType, Network_Entity entity)
	{
		if (throwable == null)
		{
			return;
		}
		if (entity != null)
		{
			if (entity.IsInvurnerable)
			{
				throwable.ReflectVelocity(normal);
			}
			else
			{
				throwable.transform.SetParent(entity.transform);
			}
		}
		else
		{
			Helper.SetParentFromParentType(throwable.transform, parentType);
		}
		throwable.HandleCollision(collisionPosition, normal, parentType, entity);
	}

	private IEnumerator StartThrow(float throwDelay, float chargeNormal)
	{
		yield return new WaitForSeconds(throwDelay);
		Vector3 force = Vector3.zero;
		switch (firingType)
		{
		case FiringType.TYPE_CHARGE:
			force = playerNetwork.Camera.transform.forward * (chargeNormal * throwForce.z) + playerNetwork.Camera.transform.right * (chargeNormal * throwForce.x) + Vector3.up * (chargeNormal * throwForce.y);
			break;
		case FiringType.TYPE_CLICK:
			force = playerNetwork.Camera.transform.forward * throwForce.z + playerNetwork.Camera.transform.right * throwForce.x + Vector3.up * throwForce.y;
			break;
		}
		Throwable_Object throwableFromAmmo = GetThrowableFromAmmo(currentAmmo);
		Message_ThrowableComponent_Throw message_ThrowableComponent_Throw = new Message_ThrowableComponent_Throw(Messages.ThrowableComponent_Throw, playerNetwork.Network.NetworkIDManager, ObjectIndex, shootPoint.position, force, currentAmmo.UniqueIndex);
		if (Raft_Network.IsHost)
		{
			if (throwableFromAmmo.usePickup)
			{
				message_ThrowableComponent_Throw.throwablePickupObjectIndex = (throwableFromAmmo.usePickup ? SaveAndLoad.GetUniqueObjectIndex() : 0u);
			}
			message_ThrowableComponent_Throw.throwableIndex = ++uniqueThrowableIndex;
			playerNetwork.Network.RPC(message_ThrowableComponent_Throw, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			Vector3 position = ((shootPoint == null) ? transform.position : shootPoint.position);
			Throw(message_ThrowableComponent_Throw.throwableIndex, message_ThrowableComponent_Throw.throwablePickupObjectIndex, position, force, throwableFromAmmo);
		}
		else
		{
			playerNetwork.SendP2P(message_ThrowableComponent_Throw, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		ReleaseHand();
	}

	private void Throw(uint throwableIndex, uint throwablePickupIndex, Vector3 position, Vector3 force, Throwable_Object throwablePrefab)
	{
		if (throwablePrefab == null)
		{
			ResetCooldown();
			return;
		}
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		CallThrowEvent(position, force);
		if (playerNetwork.IsLocalPlayer)
		{
			playerNetwork.Inventory.RemoveItem(currentAmmo.UniqueName, 1);
			CancelInvoke("ResetCooldown");
			Invoke("ResetCooldown", currentAmmo.settings_usable.UseButtonCooldown);
		}
		Throwable_Object throwable_Object = UnityEngine.Object.Instantiate(throwablePrefab, position, Quaternion.LookRotation(force.normalized), null);
		throwable_Object.Initialize(this, throwableIndex, throwablePickupIndex, force, playerNetwork.IsLocalPlayer);
		if (playerNetwork.PersonController.HasRaftAsParent)
		{
			throwable_Object.transform.SetParent(SingletonGeneric<GameManager>.Singleton.raftTransform);
		}
		thrownObjects.Add(throwable_Object);
	}

	private Throwable_Object GetThrowableFromIndex(uint throwableIndex)
	{
		foreach (Throwable_Object thrownObject in thrownObjects)
		{
			if (thrownObject.throwableIndex == throwableIndex)
			{
				return thrownObject;
			}
		}
		return null;
	}

	private Throwable_Object GetThrowableFromAmmo(Item_Base ammo)
	{
		GameObject modelFromItem = throwablePrefabs.GetModelFromItem(ammo);
		if (modelFromItem == null)
		{
			return null;
		}
		return modelFromItem.GetComponent<Throwable_Object>();
	}

	protected Item_Base GetAmmo()
	{
		ItemModelConnection[] objectConnections = throwablePrefabs.GetObjectConnections();
		foreach (ItemModelConnection itemModelConnection in objectConnections)
		{
			if (playerNetwork.Inventory.GetItemCount(itemModelConnection.item.UniqueName) > 0)
			{
				return itemModelConnection.item;
			}
		}
		return null;
	}

	protected virtual void ReleaseButton()
	{
		chargeMeter.Reset();
	}

	protected virtual void ReleaseHand()
	{
		canThrow = false;
		ResetThrowable();
		if (hideDuringCooldown)
		{
			if (throwableModelEnabler != null)
			{
				throwableModelEnabler.DeActivateAllModels();
			}
			CancelInvoke("HideModelDuringCooldown");
			Invoke("HideModelDuringCooldown", hideDelay);
		}
	}

	protected virtual void ResetThrowable()
	{
		PlayerItemManager.IsBusy = false;
		if (chargeMeter != null)
		{
			chargeMeter.Reset();
		}
	}

	private void ResetCooldown()
	{
		canThrow = true;
	}

	private void HideModelDuringCooldown()
	{
		if (throwableModelEnabler != null)
		{
			if (playerNetwork.IsLocalPlayer)
			{
				throwableModelEnabler.ActivateModel(GetAmmo());
			}
			else
			{
				throwableModelEnabler.ActivateModel(0);
			}
		}
	}

	protected virtual void CallStartChargeEvent()
	{
		if (OnStartCharge != null)
		{
			OnStartCharge();
		}
	}

	protected virtual void CallThrowEvent(Vector3 position, Vector3 force)
	{
		if (OnThrow != null)
		{
			OnThrow(position, force);
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.ThrowableComponent_Throw:
		{
			Message_ThrowableComponent_Throw message_ThrowableComponent_Throw = msg as Message_ThrowableComponent_Throw;
			Throwable_Object throwableFromAmmo = GetThrowableFromAmmo(ItemManager.GetItemByIndex(message_ThrowableComponent_Throw.ammoItemIndex));
			if (throwableFromAmmo == null)
			{
				return false;
			}
			if (Raft_Network.IsHost)
			{
				message_ThrowableComponent_Throw.throwablePickupObjectIndex = (throwableFromAmmo.usePickup ? SaveAndLoad.GetUniqueObjectIndex() : 0u);
				message_ThrowableComponent_Throw.throwableIndex = ++uniqueThrowableIndex;
			}
			Throw(message_ThrowableComponent_Throw.throwableIndex, message_ThrowableComponent_Throw.throwablePickupObjectIndex, message_ThrowableComponent_Throw.position.Value, message_ThrowableComponent_Throw.force.Value, throwableFromAmmo);
			ReleaseHand();
			return true;
		}
		case Messages.ThrowableComponent_Collision:
		{
			Message_ThrowableComponent_Collision message_ThrowableComponent_Collision = msg as Message_ThrowableComponent_Collision;
			Network_Entity entity = null;
			if (message_ThrowableComponent_Collision.hitEntityIndex >= 0)
			{
				entity = NetworkIDManager.GetNetworkIDFromObjectIndex<Network_Entity>((uint)message_ThrowableComponent_Collision.hitEntityIndex);
			}
			HandleCollision(GetThrowableFromIndex(message_ThrowableComponent_Collision.throwableIndex), message_ThrowableComponent_Collision.position.Value, message_ThrowableComponent_Collision.normal.Value, message_ThrowableComponent_Collision.parentType, entity);
			return true;
		}
		default:
			return false;
		}
	}
}
