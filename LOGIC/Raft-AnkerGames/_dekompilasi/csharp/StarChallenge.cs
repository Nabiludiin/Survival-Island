using FMODUnity;
using UnityEngine;

public class StarChallenge : MonoBehaviour_ID_Network
{
	[SerializeField]
	private Interval_Int horizontalIndexLimit;

	[SerializeField]
	private Interval_Int verticalIndexLimit;

	[SerializeField]
	private Vector2 positionIndex;

	[SerializeField]
	private Transform telescope;

	[SerializeField]
	private Transform dome;

	[SerializeField]
	private float telescopeRotationAmount;

	[SerializeField]
	private float domeRotationAmount;

	[SerializeField]
	private float rotateSpeed;

	[SerializeField]
	private RectTransform ui_background;

	[SerializeField]
	private Vector2 ui_MoveAmount;

	[SerializeField]
	private float moveSpeed;

	public EventReference er_tilt;

	public EventReference er_rotate;

	private InteractableButton_Network_Forwarder buttonForwarder;

	private void Awake()
	{
		buttonForwarder = new InteractableButton_Network_Forwarder(GetComponentsInChildren<InteractableButton_Networked>(includeInactive: true));
	}

	private void Update()
	{
		Vector2 vector = new Vector3((0f - positionIndex.x) * ui_MoveAmount.x, (0f - positionIndex.y) * ui_MoveAmount.y);
		ui_background.anchoredPosition = Vector3.Lerp(ui_background.anchoredPosition, vector, Time.deltaTime * moveSpeed);
		Vector3 localEulerAngles = telescope.localEulerAngles;
		localEulerAngles.y = Mathf.LerpAngle(localEulerAngles.y, telescopeRotationAmount * positionIndex.x, Time.deltaTime * rotateSpeed);
		telescope.localEulerAngles = localEulerAngles;
		localEulerAngles = dome.localEulerAngles;
		localEulerAngles.y = Mathf.LerpAngle(localEulerAngles.y, domeRotationAmount * positionIndex.x, Time.deltaTime * rotateSpeed);
		dome.localEulerAngles = localEulerAngles;
	}

	private bool ButtonPress(int buttonIndex)
	{
		if (buttonIndex < 0)
		{
			return false;
		}
		return buttonIndex switch
		{
			0 => Move(Vector2.up), 
			1 => Move(Vector2.right), 
			2 => Move(Vector2.down), 
			3 => Move(Vector2.left), 
			_ => false, 
		};
	}

	private bool Move(Vector2 direction)
	{
		Vector2 vector = positionIndex + direction;
		if (vector.x < (float)horizontalIndexLimit.minValue || vector.x > (float)horizontalIndexLimit.maxValue)
		{
			return false;
		}
		if (vector.y < (float)verticalIndexLimit.minValue || vector.y > (float)verticalIndexLimit.maxValue)
		{
			return false;
		}
		if (vector.y != positionIndex.y)
		{
			RuntimeManager.PlayOneShot(er_tilt, telescope.transform.position + Vector3.up * 5f);
		}
		if (vector.x != positionIndex.x)
		{
			RuntimeManager.PlayOneShot(er_rotate, telescope.transform.position + Vector3.up * 5f);
		}
		positionIndex = vector;
		return true;
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		Messages type = msg.Type;
		if (type == Messages.InteractableButtonPress || type == Messages.InteractableButtonPress_Int)
		{
			Message_InteractableButtonPress message_InteractableButtonPress = msg as Message_InteractableButtonPress;
			buttonForwarder.HandleMessage(message_InteractableButtonPress);
			if (message_InteractableButtonPress.success)
			{
				message_InteractableButtonPress.success = ButtonPress(message_InteractableButtonPress.buttonIndex);
			}
			return true;
		}
		return base.Deserialize(msg, remoteID);
	}

	public RGD_LandmarkItem_StarChallenge GetRGDSave()
	{
		return new RGD_LandmarkItem_StarChallenge(GetComponent<LandmarkItem>())
		{
			xIndex = (int)positionIndex.x,
			yIndex = (int)positionIndex.y
		};
	}

	public void RestoreRGDSave(RGD_LandmarkItem_StarChallenge rgd)
	{
		if (!(rgd == null))
		{
			positionIndex = new Vector2(rgd.xIndex, rgd.yIndex);
		}
	}
}
