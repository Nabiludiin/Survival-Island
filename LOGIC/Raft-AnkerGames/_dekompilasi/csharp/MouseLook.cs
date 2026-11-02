using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
	public enum RotationAxes
	{
		MouseXAndY,
		MouseX,
		MouseY
	}

	public RotationAxes axes;

	public bool clampX;

	public bool clampY;

	public bool isChild;

	public float minimumX = -360f;

	public float maximumX = 360f;

	public float minimumY = -60f;

	public float maximumY = 60f;

	public float lerpSpeed = 3f;

	public float fallbackSensitivity = 1f;

	private Settings settings;

	private float targetRotX;

	private float targetRotY;

	private bool rotationLocked;

	private Vector3 lockedEulerRoation;

	[SerializeField]
	private InputActionReference actionLook;

	public bool Lerp => lerpSpeed > 0f;

	public float Sensitivity
	{
		get
		{
			if (settings != null)
			{
				if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
				{
					return settings.controls.MouseSensitivity;
				}
				return settings.controls.LookSensitivity;
			}
			return fallbackSensitivity;
		}
	}

	public bool InvertMouseY
	{
		get
		{
			if (settings != null)
			{
				if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
				{
					return settings.controls.InvertMouseY;
				}
				return settings.controls.InvertLookY;
			}
			return false;
		}
	}

	public float rotX { get; private set; }

	public float rotY { get; private set; }

	private void Start()
	{
		settings = ComponentManager<Settings>.Value;
	}

	private void Update()
	{
		if (CanvasHelper.ActiveMenu != MenuType.None)
		{
			return;
		}
		if (rotationLocked)
		{
			transform.localEulerAngles = lockedEulerRoation;
			return;
		}
		Vector2 vector2;
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			Vector2 vector = actionLook.action.ReadValue<Vector2>();
			vector2 = new Vector2(vector.x, 0f - vector.y);
		}
		else
		{
			vector2 = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		}
		if (axes == RotationAxes.MouseX)
		{
			targetRotX += vector2.x * Sensitivity;
			if (clampX)
			{
				targetRotX = Mathf.Clamp(targetRotX, minimumX, maximumX);
			}
			if (Lerp)
			{
				rotX = Mathf.Lerp(rotX, targetRotX, Time.deltaTime * lerpSpeed);
			}
			else
			{
				rotX = targetRotX;
			}
			if (isChild)
			{
				transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotX, transform.localEulerAngles.z);
			}
			else
			{
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotX, transform.eulerAngles.z);
			}
		}
		else if (axes == RotationAxes.MouseY)
		{
			targetRotY += vector2.y * Sensitivity * (float)(InvertMouseY ? 1 : (-1));
			if (clampY)
			{
				targetRotY = Mathf.Clamp(targetRotY, minimumY, maximumY);
			}
			if (Lerp)
			{
				rotY = Mathf.Lerp(rotY, targetRotY, Time.deltaTime * lerpSpeed);
			}
			else
			{
				rotY = targetRotY;
			}
			if (isChild)
			{
				transform.localEulerAngles = new Vector3(rotY, transform.localEulerAngles.y, transform.localEulerAngles.z);
			}
			else
			{
				transform.eulerAngles = new Vector3(rotY, transform.eulerAngles.y, transform.eulerAngles.z);
			}
		}
		else if (axes == RotationAxes.MouseXAndY)
		{
			targetRotY += vector2.y * Sensitivity * (float)(InvertMouseY ? 1 : (-1));
			if (clampY)
			{
				targetRotY = Mathf.Clamp(targetRotY, minimumY, maximumY);
			}
			if (Lerp)
			{
				rotY = Mathf.Lerp(rotY, targetRotY, Time.deltaTime * lerpSpeed);
			}
			else
			{
				rotY = targetRotY;
			}
			targetRotX += vector2.x * Sensitivity;
			if (clampX)
			{
				targetRotX = Mathf.Clamp(targetRotX, minimumX, maximumX);
			}
			if (Lerp)
			{
				rotX = Mathf.Lerp(rotX, targetRotX, Time.deltaTime * lerpSpeed);
			}
			else
			{
				rotX = targetRotX;
			}
			if (isChild)
			{
				transform.localEulerAngles = new Vector3(rotY, rotX, transform.localEulerAngles.z);
			}
			else
			{
				transform.eulerAngles = new Vector3(rotY, rotX, transform.eulerAngles.z);
			}
		}
	}

	public void SetTargetRotToCurrentRotation()
	{
		SetTargetRotXToCurrentRotation();
		SetTargetRotYToCurrentRotation();
	}

	public void SetTargetRotYToCurrentRotation()
	{
		if (isChild)
		{
			targetRotY = Mathf.Clamp(transform.localEulerAngles.x, minimumY, maximumY);
		}
		else
		{
			targetRotY = Mathf.Clamp(transform.eulerAngles.x, minimumY, maximumY);
		}
	}

	public void SetTargetRotXToCurrentRotation()
	{
		if (isChild)
		{
			targetRotX = Mathf.Clamp(transform.localEulerAngles.y, minimumX, maximumX);
		}
		else
		{
			targetRotX = Mathf.Clamp(transform.eulerAngles.y, minimumX, maximumX);
		}
	}

	public void SetTargetRotX(float newTarget, bool applyInstantly = false)
	{
		targetRotX = Mathf.Clamp(newTarget, minimumX, maximumX);
		if (applyInstantly)
		{
			rotX = targetRotX;
			ApplyRotations();
		}
	}

	public void SetTargetRotY(float newTarget, bool applyInstantly = false)
	{
		targetRotY = Mathf.Clamp(newTarget, minimumX, maximumX);
		if (applyInstantly)
		{
			rotY = targetRotY;
			ApplyRotations();
		}
	}

	public void ResetRotations()
	{
		float num = (rotY = (targetRotX = (targetRotY = 0f)));
		rotX = num;
		ApplyRotations();
	}

	public void CopyRotationToOther(MouseLook other)
	{
		other.rotX = rotX;
		other.rotY = rotY;
		other.targetRotX = targetRotX;
		other.targetRotY = targetRotY;
		ApplyRotations();
	}

	public void CopyXRotationToOther(MouseLook other)
	{
		other.rotX = rotX;
		other.targetRotX = targetRotX;
		ApplyRotations();
	}

	public void CopyYRotationToOther(MouseLook other)
	{
		other.rotY = rotY;
		other.targetRotY = targetRotY;
		ApplyRotations();
	}

	public void ApplyRotations()
	{
		Vector3 vector = (isChild ? transform.localEulerAngles : transform.eulerAngles);
		switch (axes)
		{
		case RotationAxes.MouseX:
			vector.x = rotY;
			break;
		case RotationAxes.MouseY:
			vector.y = rotX;
			break;
		case RotationAxes.MouseXAndY:
			vector.x = rotY;
			vector.y = rotX;
			break;
		}
		if (isChild)
		{
			transform.localEulerAngles = vector;
		}
		else
		{
			transform.eulerAngles = vector;
		}
	}

	public void SetLocked(bool locked)
	{
		if (locked && !rotationLocked)
		{
			lockedEulerRoation = transform.localEulerAngles;
		}
		rotationLocked = locked;
	}
}
