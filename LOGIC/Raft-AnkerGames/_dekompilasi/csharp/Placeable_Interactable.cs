using UnityEngine;

public class Placeable_Interactable : MonoBehaviour_ID_Network, IBlockPlaced
{
	[SerializeField]
	protected Block_Interactable blockInteractable;

	[SerializeField]
	protected InteractableButton_Networked button;

	[SerializeField]
	protected int currentIndex;

	[SerializeField]
	private int maxIndex = 1;

	[SerializeField]
	private bool loopIndex = true;

	[SerializeField]
	private float cooldown;

	private float cooldownTimer;

	public int CurrentIndex => currentIndex;

	public bool IsOnCooldown => cooldownTimer > 0f;

	protected virtual void Start()
	{
		if (button == null)
		{
			button = GetComponentInChildren<InteractableButton_Networked>(includeInactive: true);
		}
		if (blockInteractable == null)
		{
			blockInteractable = GetComponentInChildren<Block_Interactable>(includeInactive: true);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkIDManager.RemoveNetworkID(this, typeof(Placeable_Interactable));
	}

	private void Update()
	{
		if (cooldownTimer > 0f)
		{
			cooldownTimer -= Time.deltaTime;
		}
	}

	void IBlockPlaced.OnBlockPlacedInterface()
	{
		BlockPlaced();
	}

	protected virtual void OnInteract(bool restoring)
	{
	}

	protected virtual void BlockPlaced()
	{
		NetworkIDManager.AddNetworkID(this, typeof(Placeable_Interactable));
	}

	public bool ButtonPress()
	{
		if (currentIndex == maxIndex && !loopIndex)
		{
			return false;
		}
		if (IsOnCooldown)
		{
			return false;
		}
		currentIndex++;
		if (currentIndex > maxIndex)
		{
			currentIndex = 0;
		}
		if (cooldown > 0f)
		{
			cooldownTimer = cooldown;
		}
		OnInteract(restoring: false);
		return true;
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Block_Interactable(RGDType.Block_Interactable, blockInteractable, CurrentIndex);
	}

	public void RestoreIndex(int index)
	{
		currentIndex = index;
		OnInteract(restoring: true);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		Messages type = msg.Type;
		if (type == Messages.InteractableButtonPress || type == Messages.InteractableButtonPress_Int)
		{
			Message_InteractableButtonPress message_InteractableButtonPress = msg as Message_InteractableButtonPress;
			if (message_InteractableButtonPress.success)
			{
				message_InteractableButtonPress.success = ButtonPress();
			}
			button.PlayButtonFeedback(message_InteractableButtonPress.success);
			return true;
		}
		return base.Deserialize(msg, remoteID);
	}
}
