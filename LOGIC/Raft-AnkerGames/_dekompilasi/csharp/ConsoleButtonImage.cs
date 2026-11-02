using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ConsoleButtonImage : MonoBehaviour
{
	public Sprite ps5Sprite;

	public Button btnToPress;

	public InputActionReference actionToListen;

	private Image img;

	public void Awake()
	{
	}

	public void OnEnable()
	{
		if (actionToListen != null)
		{
			actionToListen.action.performed += OnPerformed;
		}
	}

	public void OnDisable()
	{
		if (actionToListen != null)
		{
			actionToListen.action.performed -= OnPerformed;
		}
	}

	private void OnPerformed(InputAction.CallbackContext ctx)
	{
		if (btnToPress != null && btnToPress.interactable)
		{
			btnToPress.onClick.Invoke();
		}
	}
}
