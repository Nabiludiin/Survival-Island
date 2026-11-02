using System;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Storage_Small : Block, IRaycastable
{
	[Serializable]
	public class StorageFill
	{
		public MeshFilter storageFillFilter;

		public Mesh[] storageFillMeshes;
	}

	public Inventory inventoryPrefab;

	public Animator anim;

	[SerializeField]
	private StorageFill[] storageFillers;

	[SerializeField]
	private AnimatorMessageForwarder animatorMessageForwarder;

	public EventReference er_open;

	public EventReference er_close;

	public EventReference er_close_squeal;

	[SerializeField]
	private bool playCloseSoundEndOfAnimation;

	private Inventory inventoryReference;

	private CanvasHelper canvas;

	private bool isOpen;

	private bool canCloseWithUsebutton;

	private StorageManager storageManager;

	private Raft_Network network;

	private Network_Player currentPlayer;

	public bool IsOpen
	{
		get
		{
			return isOpen;
		}
		private set
		{
			isOpen = value;
		}
	}

	public bool CanCloseWithUseButton
	{
		get
		{
			return canCloseWithUsebutton;
		}
		private set
		{
			canCloseWithUsebutton = value;
		}
	}

	public override void OnFinishedPlacement()
	{
		base.OnFinishedPlacement();
		StorageManager.allStorages.Add(this);
		canvas = ComponentManager<CanvasHelper>.Value;
		network = ComponentManager<Raft_Network>.Value;
		inventoryReference = UnityEngine.Object.Instantiate(inventoryPrefab, Inventory.Parent);
		inventoryReference.transform.localScale = inventoryPrefab.transform.localScale;
		inventoryReference.transform.localPosition = inventoryPrefab.transform.localPosition;
		if (this.animatorMessageForwarder != null)
		{
			AnimatorMessageForwarder animatorMessageForwarder = this.animatorMessageForwarder;
			animatorMessageForwarder.OnAnimatorMessage = (Action<string>)Delegate.Combine(animatorMessageForwarder.OnAnimatorMessage, new Action<string>(OnAnimatorMessage));
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (!GameManager.IsQuittingGame)
		{
			if (currentPlayer != null)
			{
				currentPlayer.StorageManager.CloseStorage(this);
			}
			if (StorageManager.allStorages.Contains(this))
			{
				StorageManager.allStorages.Remove(this);
			}
			if (inventoryReference != null)
			{
				UnityEngine.Object.Destroy(inventoryReference.gameObject);
			}
			if (canvas != null)
			{
				canvas.displayTextManager.HideDisplayTexts(0);
			}
		}
	}

	private void OnAnimatorMessage(string message)
	{
		if (message == "Close")
		{
			UpdateStorageFillRenderers();
			if (playCloseSoundEndOfAnimation && inventoryReference != null)
			{
				RuntimeManager.PlayOneShot(er_close, transform.position);
			}
		}
	}

	public Inventory GetInventoryReference()
	{
		return inventoryReference;
	}

	public void Open(Network_Player player)
	{
		IsOpen = true;
		currentPlayer = player;
		RuntimeManager.PlayOneShot(er_open, transform.position);
		if (anim != null)
		{
			anim.SetBool("IsOpen", IsOpen);
		}
		if (player != null && player.IsLocalPlayer)
		{
			inventoryReference.Show();
			canvas.OpenMenuCloseOther(MenuType.Inventory);
			PlayerItemManager.IsBusy = true;
			canCloseWithUsebutton = false;
			CancelInvoke("AllowCloseWithUseButton");
			Invoke("AllowCloseWithUseButton", 0.15f);
		}
		UpdateStorageFillRenderers();
	}

	public void Close(Network_Player player)
	{
		if (player != null && player.IsLocalPlayer)
		{
			PlayerItemManager.IsBusy = false;
			canvas.CloseMenu(MenuType.Inventory);
		}
		isOpen = false;
		canCloseWithUsebutton = false;
		RuntimeManager.PlayOneShot(er_close_squeal, transform.position);
		if (!playCloseSoundEndOfAnimation)
		{
			RuntimeManager.PlayOneShot(er_close, transform.position);
		}
		if (anim != null)
		{
			anim.SetBool("IsOpen", isOpen);
		}
		currentPlayer = null;
		if (player != null && player.IsLocalPlayer)
		{
			inventoryReference.Hide();
		}
	}

	private void UpdateStorageFillRenderers()
	{
		if (storageFillers != null && storageFillers.Length != 0)
		{
			if (inventoryReference == null)
			{
				SetStorageFillers(-1f);
				return;
			}
			if (!isOpen)
			{
				SetStorageFillers(-1f);
				return;
			}
			float normalizedFullAmount = inventoryReference.GetNormalizedFullAmount();
			SetStorageFillers(normalizedFullAmount);
		}
	}

	private void SetStorageFillers(float normalizedFullInventory)
	{
		StorageFill[] array = storageFillers;
		foreach (StorageFill storageFill in array)
		{
			float num = 1f / (float)(storageFill.storageFillMeshes.Length - 1);
			int num2 = Mathf.FloorToInt(normalizedFullInventory / num);
			if (normalizedFullInventory <= 0f)
			{
				num2 = -1;
			}
			if (num2 >= 0)
			{
				if (num2 >= storageFill.storageFillMeshes.Length)
				{
					num2 = storageFill.storageFillMeshes.Length - 1;
				}
				storageFill.storageFillFilter.sharedMesh = storageFill.storageFillMeshes[num2];
				storageFill.storageFillFilter.gameObject.SetActiveSafe(value: true);
			}
			else
			{
				storageFill.storageFillFilter.gameObject.SetActiveSafe(value: false);
			}
		}
	}

	private void AllowCloseWithUseButton()
	{
		canCloseWithUsebutton = true;
	}

	public void OnIsRayed()
	{
		if (CanvasHelper.ActiveMenu != MenuType.None)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
		else if (storageManager == null)
		{
			Network_Player value = ComponentManager<Network_Player>.Value;
			if (value != null)
			{
				storageManager = value.StorageManager;
			}
		}
		else if (!IsOpen && !PlayerItemManager.IsBusy && canvas.CanOpenMenu && Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance + 0.5f))
		{
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
			canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Open"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				canvas.displayTextManager.HideDisplayTexts();
				if (Raft_Network.IsHost)
				{
					network.RPC(new Message_Storage(Messages.StorageManager_Open, storageManager, this), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					storageManager.OpenStorage(this);
				}
				else
				{
					network.SendP2P(network.HostID, new Message_Storage(Messages.StorageManager_Open, storageManager, this), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
		}
		else
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (canvas != null && canvas.displayTextManager != null)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	public override RGD Serialize_Save()
	{
		if (!IsValidSerialization())
		{
			return null;
		}
		return new RGD_Storage(RGDType.Block_Storage, this, ObjectIndex, inventoryReference, IsOpen);
	}
}
