using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GraphyRaftManager : MonoBehaviour
{
	public GameObject[] panelsToToggle;

	public GameObject backgroundImage;

	public Text text_Ping;

	public Text text_NetworkSent;

	public Text text_Drs;

	private Raft_Network network;

	private int panelIndex;

	private InputAction sprintAction;

	private InputAction crouchAction;

	private void Start()
	{
		DisablePanels();
	}

	private void Update()
	{
		if (PlayerInput.GetPlayerByIndex(0) == null)
		{
			return;
		}
		if (sprintAction == null)
		{
			sprintAction = PlayerInput.GetPlayerByIndex(0).actions["Sprint"];
		}
		if (crouchAction == null)
		{
			crouchAction = PlayerInput.GetPlayerByIndex(0).actions["Crouch"];
		}
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
			return;
		}
		bool flag = false;
		PlayerInput playerByIndex = PlayerInput.GetPlayerByIndex(0);
		flag = ((!(playerByIndex != null) || !(playerByIndex.currentControlScheme == "Gamepad")) ? MyInput.GetButtonDown("GameInfo") : (sprintAction.ReadValue<float>() == 1f && crouchAction.WasPressedThisFrame()));
		if ((!Raft_Network.InMenuScene & flag) && CanvasHelper.ActiveMenu == MenuType.None && !ChatTextFieldController.IsChatWindowSelected)
		{
			TogglePanels();
		}
		if (panelIndex >= 0)
		{
			text_Ping.text = network.Ping.ToString("F1");
			text_NetworkSent.text = network.TotalKiloBytesSentPerSecond.ToString();
		}
	}

	private void DisablePanels()
	{
		panelIndex = -1;
		for (int i = 0; i < panelsToToggle.Length; i++)
		{
			panelsToToggle[i].SetActiveSafe(value: false);
		}
	}

	private void TogglePanels()
	{
		panelIndex++;
		if (panelIndex >= panelsToToggle.Length)
		{
			panelIndex = -1;
		}
		for (int i = 0; i < panelsToToggle.Length; i++)
		{
			panelsToToggle[i].SetActiveSafe(i <= panelIndex);
		}
		backgroundImage.SetActiveSafe(panelIndex != -1);
	}
}
