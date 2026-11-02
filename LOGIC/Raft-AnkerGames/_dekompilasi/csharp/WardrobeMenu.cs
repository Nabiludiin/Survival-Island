using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class WardrobeMenu : MonoBehaviour
{
	public static Block_Wardrobe wardrobeOpened;

	[SerializeField]
	private GameObject gamepadBack;

	[SerializeField]
	private RectTransform selectedHighlight;

	[SerializeField]
	private Image wholeOutfitImage;

	[SerializeField]
	private Button[] outfitButtons;

	public float maxDistanceToWardrobe = 5f;

	public EventReference er_click;

	private Network_Player localPlayer;

	private CanvasHelper canvas;

	private void Start()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		canvas = ComponentManager<CanvasHelper>.Value;
		for (int i = 0; i < outfitButtons.Length; i++)
		{
			int index = i;
			Button button = outfitButtons[i];
			button.onClick.AddListener(() =>
			{
				OutfitButtonPressed(button, index);
			});
		}
	}

	private void OnDisable()
	{
		wardrobeOpened = null;
	}

	public void MenuOpen()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		for (int i = 0; i < outfitButtons.Length; i++)
		{
			if (i < localPlayer.currentModel.outfits.Length)
			{
				outfitButtons[i].GetComponent<Image>().sprite = localPlayer.currentModel.outfits[i].sprite;
			}
			else
			{
				Debug.Log("Can't set sprite to " + outfitButtons[i].name);
			}
		}
		Invoke("MenuOpenLate", 0.1f);
	}

	private void MenuOpenLate()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		SelectOutfitUI(localPlayer.characterSettings.OutfitIndex, focus: true);
	}

	public void OutfitButtonPressed(Button button, int outfitIndex)
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (!er_click.IsNull)
		{
			RuntimeManager.PlayOneShot(er_click);
		}
		SelectOutfitUI(outfitIndex, focus: false);
		if (localPlayer.characterSettings.OutfitIndex == outfitIndex)
		{
			return;
		}
		Message_NetworkBehaviour_Int message = new Message_NetworkBehaviour_Int(Messages.Character_ChangeOutfit, localPlayer, outfitIndex);
		if (Raft_Network.IsHost)
		{
			if (localPlayer.ApplyOutfit(outfitIndex))
			{
				localPlayer.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			localPlayer.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void SelectOutfitUI(int outfitIndex, bool focus)
	{
		if (outfitIndex < 0 || outfitIndex >= outfitButtons.Length)
		{
			return;
		}
		Button button = outfitButtons[outfitIndex];
		if (!(button == null))
		{
			selectedHighlight.position = button.GetComponent<RectTransform>().position;
			if (focus)
			{
				canvas.GetComponent<GamepadCursor>().FocusMagnet(button.GetComponent<UiMagnet>());
			}
			wholeOutfitImage.sprite = localPlayer.currentModel.outfits[outfitIndex].fullBodySprite;
		}
	}

	private void Update()
	{
		gamepadBack.SetActive(SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad);
		if (!(wardrobeOpened == null))
		{
			if (localPlayer == null)
			{
				localPlayer = ComponentManager<Network_Player>.Value;
			}
			if (localPlayer == null || Vector3.Distance(localPlayer.transform.position, wardrobeOpened.transform.position) > maxDistanceToWardrobe || localPlayer.Stats.IsDead)
			{
				wardrobeOpened.Close();
				wardrobeOpened = null;
			}
		}
	}
}
