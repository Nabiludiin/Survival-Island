using UnityEngine;
using UnityEngine.Events;

public class UiMagnet : MonoBehaviour
{
	public float Radius = 100f;

	public GamepadCursorLayer Layer;

	public RectTransform Viewport;

	public bool AllowDPad = true;

	public bool AllowStick = true;

	public int UserInt;

	public uint Mask = 1u;

	public UnityEvent<UiMagnet> OnCancel;

	public UnityEvent<UiMagnet> OnEnter;

	public UnityEvent<UiMagnet> OnExit;

	private void Awake()
	{
		if (Mask == 0)
		{
			Mask = 1u;
		}
	}

	private void OnEnable()
	{
		ComponentManager<CanvasHelper>.Value.GetComponent<GamepadCursor>().AddMagnet(this);
	}

	private void OnDisable()
	{
		ComponentManager<CanvasHelper>.Value.GetComponent<GamepadCursor>().RemoveMagnet(this);
	}

	public Vector2 GetCenterPosition()
	{
		RectTransform component = GetComponent<RectTransform>();
		return component.TransformPoint(component.rect.center);
	}

	public void Enter()
	{
		OnEnter?.Invoke(this);
	}

	public void Exit()
	{
		OnExit?.Invoke(this);
	}

	public void Cancel()
	{
		OnCancel?.Invoke(this);
	}
}
