using System.Collections;
using UnityEngine;
using UnityEngine.AzureSky;

public class CutsceneManager : MonoBehaviour
{
	[SerializeField]
	private Animator cutsceneAnimator;

	[SerializeField]
	private Dialogue dialogue;

	[SerializeField]
	private Helper_PlayCutsceneMusic cutsceneMusic;

	private SO_Cutscene[] cutscenes;

	private Coroutine slideRoutine;

	private CanvasHelper canvas;

	private Network_Player localPlayer;

	private void Awake()
	{
		ComponentManager<CutsceneManager>.Value = this;
	}

	private void Start()
	{
		localPlayer = ComponentManager<Network_Player>.Value;
		canvas = ComponentManager<CanvasHelper>.Value;
		cutscenes = Resources.LoadAll<SO_Cutscene>("SO_Cutscene");
	}

	public void PlayCutscene(SO_Cutscene cutscene)
	{
		if (cutscene != null)
		{
			if (cutsceneMusic != null)
			{
				cutsceneMusic.ApplyMusicSettings();
			}
			StopSlideShow();
			slideRoutine = CoroutineManager.Singleton.AddCoroutine(ShowSlides(cutscene));
		}
	}

	public SO_Cutscene GetCutsceneFromType(CutsceneType type)
	{
		if (cutscenes == null)
		{
			return null;
		}
		SO_Cutscene[] array = cutscenes;
		foreach (SO_Cutscene sO_Cutscene in array)
		{
			if (!(sO_Cutscene == null) && sO_Cutscene.type == type)
			{
				return sO_Cutscene;
			}
		}
		return null;
	}

	private void StopSlideShow()
	{
		if (slideRoutine != null)
		{
			CoroutineManager.Singleton.StopCoroutine(slideRoutine);
			slideRoutine = null;
		}
		dialogue.CancelVoice();
		canvas.CloseMenu(MenuType.Cutscene);
	}

	private void LockPlayer(Network_Player player, bool value)
	{
		player.PlayerScript.SetMouseLookScripts(!value);
		player.PersonController.controller.enabled = !value;
		player.PersonController.IsMovementFree = !value;
		if (value)
		{
			player.PlayerItemManager.HideItemInHand();
		}
		else
		{
			player.Inventory.hotbar.ReselectCurrentSlot();
		}
		PlayerItemManager.IsBusy = value;
	}

	private IEnumerator ShowSlides(SO_Cutscene cutscene)
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		canvas.OpenMenuCloseOther(MenuType.Cutscene);
		canvas.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Cutscene);
		canvas.SetUIState(state: false);
		cutsceneAnimator.SetTrigger("Play");
		yield return new WaitForSeconds(cutscene.startVoiceDelay);
		ForceDayTime();
		LockPlayer(localPlayer, value: true);
		dialogue.PlayVoice(cutscene.voiceData, null);
		yield return new WaitForSeconds(cutscene.lockPlayerDuration);
		LockPlayer(localPlayer, value: false);
		yield return new WaitForSeconds(Mathf.Clamp(cutscene.cutsceneDuration - cutscene.lockPlayerDuration, 0f, float.MaxValue));
		canvas.CloseMenu(MenuType.Cutscene);
		canvas.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Inventory);
		canvas.SetUIState(state: true);
		AchievementHandler.UnlockAchievement(AchievementType.ach_finalCutscene);
	}

	private void ForceDayTime()
	{
		AzureSkyController value = ComponentManager<AzureSkyController>.Value;
		if (value != null)
		{
			value.timeOfDay.hour = 12f;
		}
	}
}
