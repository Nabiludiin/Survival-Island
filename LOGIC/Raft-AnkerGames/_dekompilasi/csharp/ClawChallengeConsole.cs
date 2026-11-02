using UnityEngine;
using UnityEngine.InputSystem;

public class ClawChallengeConsole : MonoBehaviour, IRaycastable
{
	public static bool InFocus;

	[SerializeField]
	private InteractableButton_Networked_ClawChallenge btnClaw;

	[SerializeField]
	private InteractableButton_Networked_ClawChallenge btnDown;

	[SerializeField]
	private InteractableButton_Networked_ClawChallenge btnLeft;

	[SerializeField]
	private InteractableButton_Networked_ClawChallenge btnRight;

	[SerializeField]
	private InteractableButton_Networked_ClawChallenge btnUp;

	[SerializeField]
	private InputActionReference inputClaw;

	[SerializeField]
	private InputActionReference inputDown;

	[SerializeField]
	private InputActionReference inputLeft;

	[SerializeField]
	private InputActionReference inputRight;

	[SerializeField]
	private InputActionReference inputUp;

	private CanvasHelper canvasHelper;

	private BoxCollider consoleIntercept;

	private void Start()
	{
		canvasHelper = ComponentManager<CanvasHelper>.Value;
		consoleIntercept = GetComponent<BoxCollider>();
	}

	public void Update()
	{
		consoleIntercept.enabled = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad;
	}

	private void OnDestroy()
	{
		canvasHelper = null;
		InFocus = false;
	}

	public void OnIsRayed()
	{
		if (InFocus && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			canvasHelper.displayTextManager.ShowTextConsole(Helper.GetTerm("Game/MoveCrane", applyParameters: true), "D-Pad", 1, 1, clearAllTexts: false);
			canvasHelper.displayTextManager.ShowTextConsole(Helper.GetTerm("Game/UseCrane", applyParameters: true), inputClaw.action.GetBindingDisplayString(), 2, 1, clearAllTexts: false);
			if (inputClaw.action.WasPressedThisFrame())
			{
				btnClaw.ButtonPress_Fake(hideDisplayText: false);
			}
			else if (inputDown.action.WasPressedThisFrame())
			{
				btnDown.ButtonPress_Fake(hideDisplayText: false);
			}
			else if (inputLeft.action.WasPressedThisFrame())
			{
				btnLeft.ButtonPress_Fake(hideDisplayText: false);
			}
			else if (inputRight.action.WasPressedThisFrame())
			{
				btnRight.ButtonPress_Fake(hideDisplayText: false);
			}
			else if (inputUp.action.WasPressedThisFrame())
			{
				btnUp.ButtonPress_Fake(hideDisplayText: false);
			}
		}
	}

	public void OnRayEnter()
	{
		InFocus = true;
	}

	public void OnRayExit()
	{
		InFocus = false;
		canvasHelper.displayTextManager.HideDisplayTexts();
	}
}
