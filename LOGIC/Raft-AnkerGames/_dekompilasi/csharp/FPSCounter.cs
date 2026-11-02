using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
	[SerializeField]
	private float updateRate;

	[SerializeField]
	private Text text_FPS;

	[SerializeField]
	private Text text_Ping;

	[SerializeField]
	private GameObject panel;

	private Raft_Network network;

	private float deltaTime;

	private float accumulator;

	private void Start()
	{
		panel.SetActive(value: false);
	}

	private void Update()
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		if (network == null || panel == null)
		{
			return;
		}
		if (!Raft_Network.InMenuScene && MyInput.GetButtonDown("GameInfo") && CanvasHelper.ActiveMenu == MenuType.None && !ChatTextFieldController.IsChatWindowSelected)
		{
			panel.SetActive(!panel.activeInHierarchy);
		}
		if (panel.activeInHierarchy)
		{
			deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
			_ = deltaTime;
			float num = 1f / deltaTime;
			float num2 = 1f / updateRate;
			accumulator += Time.deltaTime;
			if (accumulator >= num2)
			{
				accumulator -= num2;
				text_FPS.text = num.ToString("F0");
				text_Ping.text = network.Ping.ToString("F1");
			}
		}
	}
}
