using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class GamepadCursor : MonoBehaviour
{
	[Serializable]
	private class LayerData
	{
		public GamepadCursorLayer layer;

		public bool allowLeftStickToModifyCursor = true;

		public bool showCursor = true;
	}

	public const uint Default = 1u;

	public const uint Item = 2u;

	public const uint Equipment = 4u;

	public const uint All = uint.MaxValue;

	[SerializeField]
	private RectTransform cursorTransform;

	[SerializeField]
	private RectTransform canvasRectTransform;

	[SerializeField]
	private float stickEscapeFactor = 2000f;

	[SerializeField]
	private float magnetPullSpeed = 450f;

	[SerializeField]
	private float magnetCenterThreshold = 8f;

	[SerializeField]
	private float dPadDotThreshold = 0.5f;

	[SerializeField]
	private float leftStickAccelerationThreshold = 0.1f;

	[SerializeField]
	private float cursorVelocity = 100f;

	[SerializeField]
	private float cursorMaxVelocity = 300f;

	[SerializeField]
	private float cursorAcceleration = 200f;

	[SerializeField]
	private bool visualizeAcceleration;

	[SerializeField]
	private LayerData[] layerDatas;

	private float velocity;

	private bool prevState;

	private Mouse virtualMouse;

	private PlayerInput playerInput;

	private List<UiMagnet> magnetList = new List<UiMagnet>();

	private Dictionary<int, int> magnetDictionary = new Dictionary<int, int>();

	private UiMagnet? focusMagnet;

	private LayerData currentLayerData;

	private float dpadCooldown;

	private bool dpadFast;

	private UiMagnet magnetUnderCursor;

	private uint currentMask = uint.MaxValue;

	private bool enableCursor = true;

	[SerializeField]
	private Sprite cursorSprite;

	[SerializeField]
	private Sprite hooverSprite;

	public UiMagnet MagnetUnderCursor => magnetUnderCursor;

	public Vector3 CursorPos => virtualMouse.position.ReadValue();

	private void Start()
	{
		PairWithUser(PlayerInput.GetPlayerByIndex(0));
		SetLayer(GamepadCursorLayer.Inventory);
	}

	private void OnEnable()
	{
		if (virtualMouse == null)
		{
			virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
		}
		else if (!virtualMouse.added)
		{
			InputSystem.AddDevice(virtualMouse);
		}
		InputState.Change(virtualMouse.position, new Vector2(Screen.width / 2, Screen.height / 2));
		InputSystem.onAfterUpdate += AfterUpdate;
	}

	private void OnDisable()
	{
		if (virtualMouse != null)
		{
			InputSystem.RemoveDevice(virtualMouse);
		}
		InputSystem.onAfterUpdate -= AfterUpdate;
	}

	public void EnableCursor(bool _useCursor)
	{
		enableCursor = _useCursor;
	}

	public void RecenterCursor()
	{
		InputState.Change(virtualMouse.position, new Vector2(Screen.width / 2, Screen.height / 2));
	}

	public bool IsCursorEnabled()
	{
		return enableCursor;
	}

	private void Update()
	{
		if (virtualMouse != null && !(playerInput == null) && playerInput.enabled && playerInput.currentActionMap.name == "UI" && playerInput.currentControlScheme == "Gamepad" && Gamepad.current != null && enableCursor)
		{
			UiMagnet uiMagnet = GetMagnetUnderCursor();
			if (uiMagnet != null && Gamepad.current.bButton.wasPressedThisFrame)
			{
				uiMagnet.Cancel();
			}
			if (uiMagnet != magnetUnderCursor)
			{
				magnetUnderCursor?.Exit();
				uiMagnet?.Enter();
				magnetUnderCursor = uiMagnet;
			}
		}
	}

	public void PairWithUser(PlayerInput p_playerInput)
	{
		playerInput = p_playerInput;
		InputUser.PerformPairingWithDevice(virtualMouse, p_playerInput.user);
	}

	public void SetLayer(GamepadCursorLayer layer)
	{
		LayerData[] array = layerDatas;
		foreach (LayerData layerData in array)
		{
			if (layerData.layer == layer)
			{
				currentLayerData = layerData;
				break;
			}
		}
	}

	public void SetMask(uint mask)
	{
		currentMask = mask;
	}

	public void AddMagnet(UiMagnet magnet)
	{
		int count = magnetList.Count;
		magnetList.Add(magnet);
		magnetDictionary.Add(magnet.GetInstanceID(), count);
	}

	public void RemoveMagnet(UiMagnet magnet)
	{
		int num = magnetDictionary[magnet.GetInstanceID()];
		int index = magnetList.Count - 1;
		UiMagnet uiMagnet = magnetList[index];
		magnetList[num] = uiMagnet;
		magnetList.RemoveAt(index);
		magnetDictionary[uiMagnet.GetInstanceID()] = num;
		magnetDictionary.Remove(magnet.GetInstanceID());
	}

	public void FocusMagnet(UiMagnet uiMagnet)
	{
		focusMagnet = uiMagnet;
	}

	public UiMagnet GetMagnetUnderCursor()
	{
		if (currentLayerData == null)
		{
			return null;
		}
		foreach (UiMagnet magnet in magnetList)
		{
			if (magnet.Layer == currentLayerData.layer)
			{
				RectTransform component = magnet.GetComponent<RectTransform>();
				if ((!(magnet.Viewport != null) || RectTransformUtility.RectangleContainsScreenPoint(magnet.Viewport, component.position)) && RectTransformUtility.RectangleContainsScreenPoint(component, CursorPos))
				{
					return magnet;
				}
			}
		}
		return null;
	}

	public UiMagnet GetMagnetClosestToCursor()
	{
		Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
		UiMagnet result = null;
		foreach (UiMagnet magnet in magnetList)
		{
			if (magnet.Layer != currentLayerData.layer)
			{
				continue;
			}
			RectTransform component = magnet.GetComponent<RectTransform>();
			if (!(magnet.Viewport != null) || RectTransformUtility.RectangleContainsScreenPoint(magnet.Viewport, component.position))
			{
				Vector2 vector2 = magnet.GetCenterPosition() - new Vector2(CursorPos.x, CursorPos.y);
				if (vector2.sqrMagnitude < vector.sqrMagnitude)
				{
					vector = vector2;
					result = magnet;
				}
			}
		}
		return result;
	}

	private bool DPadNavigate(Vector2 currPos, Vector2 axis, out Vector2 newPos)
	{
		float num = float.MaxValue;
		UiMagnet uiMagnet = null;
		foreach (UiMagnet magnet in magnetList)
		{
			if (!magnet.AllowDPad || magnet.Layer != currentLayerData.layer || (magnet.Mask & currentMask) == 0)
			{
				continue;
			}
			RectTransform component = magnet.GetComponent<RectTransform>();
			if (!(magnet.Viewport != null) || RectTransformUtility.RectangleContainsScreenPoint(magnet.Viewport, component.position))
			{
				Vector2 vector = magnet.GetCenterPosition() - currPos;
				float num2 = Vector2.Dot(vector.normalized, axis);
				float num3 = 1f;
				if (num2 > dPadDotThreshold && vector.sqrMagnitude * num3 < num)
				{
					num = vector.sqrMagnitude * num3;
					uiMagnet = magnet;
				}
			}
		}
		if (uiMagnet != null)
		{
			newPos = uiMagnet.GetCenterPosition();
			return true;
		}
		newPos = Vector2.zero;
		return false;
	}

	private void ShowCursor()
	{
		Color white = Color.white;
		cursorTransform.gameObject.GetComponent<Image>().color = white;
	}

	private void HideCursor(bool fade)
	{
		Color color = cursorTransform.gameObject.GetComponent<Image>().color;
		if (fade)
		{
			float val = color.a - 1.2f * Time.deltaTime;
			color.a = Math.Max(val, 0f);
		}
		else
		{
			color.a = 0f;
		}
		cursorTransform.gameObject.GetComponent<Image>().color = color;
	}

	private void AfterUpdate()
	{
		if (virtualMouse == null || playerInput == null)
		{
			return;
		}
		if (playerInput.currentActionMap.name == "UI" && playerInput.currentControlScheme == "Gamepad" && Gamepad.current != null && enableCursor)
		{
			Settings value = ComponentManager<Settings>.Value;
			bool allowLeftStickToModifyCursor = currentLayerData.allowLeftStickToModifyCursor;
			bool showCursor = currentLayerData.showCursor;
			cursorTransform.gameObject.SetActive(value: true);
			cursorTransform.gameObject.GetComponent<Image>().enabled = showCursor;
			Vector2 vector = virtualMouse.position.ReadValue();
			Vector2 vector2 = vector;
			if (focusMagnet != null)
			{
				vector = focusMagnet.GetCenterPosition();
				focusMagnet = null;
			}
			if (Gamepad.current.dpad.right.wasReleasedThisFrame || Gamepad.current.leftStick.right.wasReleasedThisFrame || Gamepad.current.dpad.left.wasReleasedThisFrame || Gamepad.current.leftStick.left.wasReleasedThisFrame || Gamepad.current.dpad.up.wasReleasedThisFrame || Gamepad.current.leftStick.up.wasReleasedThisFrame || Gamepad.current.dpad.down.wasReleasedThisFrame || Gamepad.current.leftStick.down.wasReleasedThisFrame)
			{
				dpadCooldown = 0f;
				dpadFast = false;
			}
			if (dpadCooldown > 0f)
			{
				dpadCooldown -= Time.unscaledDeltaTime;
				if (!(dpadCooldown <= 0f))
				{
					return;
				}
				dpadCooldown = 0f;
			}
			HideCursor(fade: true);
			if (Gamepad.current.dpad.right.isPressed || (!allowLeftStickToModifyCursor && Gamepad.current.leftStick.right.isPressed))
			{
				if (DPadNavigate(vector, Vector2.right, out var newPos))
				{
					vector = newPos;
					dpadCooldown = ((!dpadFast) ? 0.4f : 0.125f);
					dpadFast = true;
				}
			}
			else if (Gamepad.current.dpad.left.isPressed || (!allowLeftStickToModifyCursor && Gamepad.current.leftStick.left.isPressed))
			{
				if (DPadNavigate(vector, Vector2.left, out var newPos2))
				{
					vector = newPos2;
					dpadCooldown = ((!dpadFast) ? 0.4f : 0.125f);
					dpadFast = true;
				}
			}
			else if (Gamepad.current.dpad.up.isPressed || (!allowLeftStickToModifyCursor && Gamepad.current.leftStick.up.isPressed))
			{
				if (DPadNavigate(vector, Vector2.up, out var newPos3))
				{
					vector = newPos3;
					dpadCooldown = ((!dpadFast) ? 0.4f : 0.125f);
					dpadFast = true;
				}
			}
			else if (Gamepad.current.dpad.down.isPressed || (!allowLeftStickToModifyCursor && Gamepad.current.leftStick.down.isPressed))
			{
				if (DPadNavigate(vector, Vector2.down, out var newPos4))
				{
					vector = newPos4;
					dpadCooldown = ((!dpadFast) ? 0.4f : 0.125f);
					dpadFast = true;
				}
			}
			else if (allowLeftStickToModifyCursor)
			{
				Vector2 vector3 = new Vector2(float.MaxValue, float.MaxValue);
				bool flag = false;
				foreach (UiMagnet magnet in magnetList)
				{
					if (!magnet.AllowStick || magnet.Layer != currentLayerData.layer || (magnet.Mask & currentMask) == 0)
					{
						continue;
					}
					RectTransform component = magnet.GetComponent<RectTransform>();
					if (magnet.Viewport != null && !RectTransformUtility.RectangleContainsScreenPoint(magnet.Viewport, component.position))
					{
						continue;
					}
					Vector2 centerPosition = magnet.GetCenterPosition();
					if (Vector3.Distance(vector, centerPosition) < magnet.Radius)
					{
						Vector2 vector4 = centerPosition - vector;
						if (vector4.sqrMagnitude < vector3.sqrMagnitude)
						{
							vector3 = vector4;
							flag = true;
						}
					}
				}
				Vector2 vector5 = Gamepad.current.leftStick.ReadValue();
				if (vector5.sqrMagnitude > 0.010000001f)
				{
					ShowCursor();
				}
				if (vector5.sqrMagnitude < 0.05f)
				{
					velocity = cursorVelocity;
				}
				if (flag && vector3.sqrMagnitude >= (vector5 * stickEscapeFactor).sqrMagnitude)
				{
					velocity = cursorVelocity;
					if (vector3.sqrMagnitude > magnetCenterThreshold * magnetCenterThreshold)
					{
						vector += vector3.normalized * magnetPullSpeed * Time.unscaledDeltaTime;
					}
					else
					{
						vector = vector3 + vector;
					}
				}
				else
				{
					float num = value.controls.CursorSensitivity * 2f;
					Vector2 vector6 = vector5;
					ShowCursor();
					if (vector5.magnitude > leftStickAccelerationThreshold)
					{
						velocity += cursorAcceleration * Time.unscaledDeltaTime;
						velocity = Math.Clamp(velocity, cursorVelocity, cursorMaxVelocity);
					}
					else
					{
						velocity = cursorVelocity;
					}
					if (Application.isPlaying && visualizeAcceleration)
					{
						cursorTransform.gameObject.GetComponent<Image>().material.color = new Color(velocity / cursorMaxVelocity, 1f, 1f, 1f);
					}
					vector += vector6 * num * velocity * Time.unscaledDeltaTime;
				}
			}
			vector.x = Mathf.Clamp(vector.x, 0f, Screen.width);
			vector.y = Mathf.Clamp(vector.y, 0f, Screen.height);
			InputState.Change(virtualMouse.position, vector);
			InputState.Change(virtualMouse.delta, vector - vector2);
			InputState.Change(virtualMouse.scroll, Gamepad.current.rightStick.ReadValue() * 750f * Time.unscaledDeltaTime);
			bool flag2 = Gamepad.current.aButton.IsPressed();
			if (prevState != flag2)
			{
				virtualMouse.CopyState<MouseState>(out var state);
				state.WithButton(MouseButton.Left, flag2);
				InputState.Change(virtualMouse, state);
				prevState = flag2;
			}
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, vector, null, out var localPoint);
			cursorTransform.anchoredPosition = localPoint;
		}
		else
		{
			cursorTransform.gameObject.SetActive(value: false);
		}
	}
}
