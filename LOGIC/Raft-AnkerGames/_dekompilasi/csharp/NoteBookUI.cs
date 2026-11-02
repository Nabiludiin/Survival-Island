using System;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NoteBookUI : MonoBehaviour
{
	public Action<bool> OnBookActiveChanged;

	public Transform enhancePoint;

	[SerializeField]
	private Canvas noteBookCanvas;

	[SerializeField]
	private Renderer noteBookRenderer;

	[SerializeField]
	private Animator noteBookAnimator;

	[SerializeField]
	private GameObject leftPageFlipImage;

	[SerializeField]
	private GameObject rightPageFlipImage;

	[SerializeField]
	private GameObject leftPageFlipImageConsole;

	[SerializeField]
	private GameObject rightPageFlipImageConsole;

	[SerializeField]
	private SO_QuestItemColors questItemColors;

	[SerializeField]
	private float hideBookDelay;

	[SerializeField]
	private uint currentPageIndex;

	[SerializeField]
	private StudioEventEmitter soundemitter_FlipPage;

	[SerializeField]
	private StudioEventEmitter soundemitter_OpenBook;

	[SerializeField]
	private StudioEventEmitter soundemitter_CloseBook;

	[SerializeField]
	private Light lightSource;

	[HideInInspector]
	public bool isDisplayed;

	public List<NoteBookNote> radarTowerNotes;

	public List<NoteBookNote> vasagatanNotes;

	public List<NoteBookNote> balboaNotes;

	public List<NoteBookNote> caravanNotes;

	public List<NoteBookNote> tangaroaNotes;

	public List<NoteBookNote> varunaNotes;

	public List<NoteBookNote> temperanceNotes;

	private NoteBookPage leftPage;

	private NoteBookPage rightPage;

	private Raft_Network network;

	private NoteBook noteBook;

	private Network_Player player;

	private NoteBookPage[] pageObjs;

	private NoteBookNote[] notes;

	private NoteBook_QuestItem[] questItemUIs;

	private uint highestUnlockedPageIndex;

	[SerializeField]
	private InputActionReference notebookAction;

	[SerializeField]
	private InputActionReference cancelAction;

	[SerializeField]
	private InputActionReference clickAction;

	private void Awake()
	{
		player = transform.GetComponentInParent<Network_Player>();
		pageObjs = transform.GetComponentsInChildren<NoteBookPage>(includeInactive: true);
		notes = transform.GetComponentsInChildren<NoteBookNote>(includeInactive: true);
		questItemUIs = transform.GetComponentsInChildren<NoteBook_QuestItem>(includeInactive: true);
		leftPage = pageObjs[0];
		rightPage = pageObjs[1];
		PopulateLandmarNotesListsByNoteEnum();
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		noteBook = ComponentManager<NoteBook>.Value;
		currentPageIndex = 0u;
		InitializeNotesAndPages();
		UpdatePages();
		DoBookDisplayToggle(atStart: true);
		QuestItemManager.OnQuestItemsModified = (Action)Delegate.Combine(QuestItemManager.OnQuestItemsModified, new Action(OnQuestItemsModified));
		NoteBook.OnIndexUnlock = (Action<uint>)Delegate.Combine(NoteBook.OnIndexUnlock, new Action<uint>(OnIndexUnlock));
		if (!player.IsLocalPlayer)
		{
			leftPageFlipImage.gameObject.SetActiveSafe(value: false);
			rightPageFlipImage.gameObject.SetActiveSafe(value: false);
			leftPageFlipImageConsole.gameObject.SetActiveSafe(value: false);
			rightPageFlipImageConsole.gameObject.SetActiveSafe(value: false);
			noteBookCanvas.GetComponent<GraphicRaycaster>().enabled = false;
		}
	}

	private void OnDestroy()
	{
		QuestItemManager.OnQuestItemsModified = (Action)Delegate.Remove(QuestItemManager.OnQuestItemsModified, new Action(OnQuestItemsModified));
		NoteBook.OnIndexUnlock = (Action<uint>)Delegate.Remove(NoteBook.OnIndexUnlock, new Action<uint>(OnIndexUnlock));
	}

	private void Update()
	{
		if (!player.IsLocalPlayer || (CanvasHelper.ActiveMenu != MenuType.None && CanvasHelper.ActiveMenu != MenuType.Journal))
		{
			return;
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			leftPageFlipImageConsole.SetActive(currentPageIndex >= 2);
			rightPageFlipImageConsole.SetActive(highestUnlockedPageIndex - currentPageIndex >= 2);
			leftPageFlipImage.SetActive(value: false);
			rightPageFlipImage.SetActive(value: false);
		}
		else
		{
			leftPageFlipImage.SetActive(currentPageIndex >= 2);
			rightPageFlipImage.SetActive(highestUnlockedPageIndex - currentPageIndex >= 2);
			leftPageFlipImageConsole.SetActive(value: false);
			rightPageFlipImageConsole.SetActive(value: false);
		}
		if (!IsInvoking() && CanUseNoteBook())
		{
			bool flag = ((!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad) ? Input.GetKeyDown(KeyCode.Escape) : cancelAction.action.WasPressedThisFrame());
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(notebookAction, "NoteBookToggle") || (isDisplayed & flag))
			{
				if (NoteBookNote_Enhancer.IsEnhacedOnNote)
				{
					NoteBookNote_Enhancer.currentEnhancedNote.ResetToNormal();
				}
				ToggleBookActivity();
				SendBookActiveMessage();
			}
		}
		if (NoteBookNote_Enhancer.IsEnhacedOnNote && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(clickAction, "LMB") && !EventSystem.current.IsPointerOverGameObject())
		{
			NoteBookNote_Enhancer.currentEnhancedNote.ToggleEnhancement();
		}
	}

	private void OnQuestItemsModified()
	{
		UpdateQuestItemUI();
	}

	private void OnIndexUnlock(uint noteIndex)
	{
		NoteBookNote noteBookNote = FindNoteByIndex((int)noteIndex);
		noteBookNote.ToggleNoteGameObject(value: true);
		noteBookNote.isUnlocked = true;
		NoteBookPage noteBookPage = FindPageByNoteIndex(noteIndex);
		if (noteBookPage != null)
		{
			uint pageIndex = ((noteBookPage.pageIndex % 2 == 0) ? noteBookPage.pageIndex : (noteBookPage.pageIndex - 1));
			FlipToPageLocally(pageIndex);
			SendFlipPageMessage();
		}
		else
		{
			Debug.Log("Unable to flip to page on index unlock, page null with noteindex" + noteIndex);
		}
		AchievementHandler.EvalueateNoteBookAchievement(noteBookNote.landmarkType, this);
	}

	public NoteBookNote[] GetAllNotes()
	{
		return notes;
	}

	public NoteBookNote FindNoteByIndex(int noteIndex)
	{
		NoteBookNote[] array = notes;
		foreach (NoteBookNote noteBookNote in array)
		{
			if (noteBookNote.noteIndex == noteIndex)
			{
				return noteBookNote;
			}
		}
		return null;
	}

	public NoteBookPage FindNotePageByIndex(int pageIndex)
	{
		NoteBookPage[] array = pageObjs;
		foreach (NoteBookPage noteBookPage in array)
		{
			if (noteBookPage.pageIndex == pageIndex)
			{
				return noteBookPage;
			}
		}
		return null;
	}

	public NoteBookPage FindPageByNoteIndex(uint noteIndex)
	{
		for (int i = 0; i < pageObjs.Length; i++)
		{
			NoteBookNote[] array = pageObjs[i].GetNotes();
			if (array == null)
			{
				continue;
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].noteIndex == noteIndex)
				{
					return pageObjs[i];
				}
			}
		}
		return null;
	}

	public void SetPageIndex(uint index)
	{
		if (index < 0)
		{
			index = 0u;
		}
		else if (index >= pageObjs.Length)
		{
			index = (uint)(pageObjs.Length - 1);
		}
		currentPageIndex = index;
		UpdatePages();
	}

	public bool FlipToPageLocally(uint pageIndex)
	{
		if (currentPageIndex == pageIndex)
		{
			return false;
		}
		uint num = ((pageIndex % 2 == 0) ? (pageIndex + 1) : (pageIndex - 1));
		NoteBookPage[] pageFromIndex = GetPageFromIndex(pageIndex, num);
		bool flag = false;
		NoteBookPage[] array = pageFromIndex;
		foreach (NoteBookPage noteBookPage in array)
		{
			if (noteBookPage != null && noteBookPage.HasNotesUnlockedOnPage())
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return false;
		}
		if (NoteBookNote_Enhancer.IsEnhacedOnNote)
		{
			NoteBookNote_Enhancer.currentEnhancedNote.ResetToNormal();
		}
		currentPageIndex = pageIndex;
		UpdatePages();
		return true;
	}

	public bool FlipToPageNetwork(uint pageIndex)
	{
		if (FlipToPageLocally(pageIndex))
		{
			SendFlipPageMessage();
			return true;
		}
		return false;
	}

	public void FlipPageLeft()
	{
		if (!NoteBookNote_Enhancer.IsEnhacedOnNote)
		{
			if (currentPageIndex != 0)
			{
				currentPageIndex -= 2u;
				UpdatePages();
				SendFlipPageMessage();
			}
			PlayFlipPageSound();
		}
	}

	public void FlipPageRight()
	{
		if (!NoteBookNote_Enhancer.IsEnhacedOnNote)
		{
			int num = Mathf.FloorToInt((int)highestUnlockedPageIndex / 2) * 2;
			if (currentPageIndex + 2 <= num)
			{
				currentPageIndex += 2u;
				UpdatePages();
				SendFlipPageMessage();
			}
			PlayFlipPageSound();
		}
	}

	public void UpdateAllNotesVisibility()
	{
		NoteBookNote[] array = notes;
		foreach (NoteBookNote noteBookNote in array)
		{
			bool value = false;
			for (int j = 0; j < NoteBook.unlockedNoteBookIndexes.Count; j++)
			{
				if (noteBookNote.noteIndex == NoteBook.unlockedNoteBookIndexes[j])
				{
					value = true;
					noteBookNote.isUnlocked = true;
					break;
				}
			}
			if (noteBookNote.isUnlocked && noteBookNote.isFrequencyThumbnail)
			{
				NoteBook.UnlockFrequency(noteBookNote.thumbNailLandmarkType);
			}
			noteBookNote.ToggleNoteGameObject(value);
		}
	}

	public void ToggleBookActivity()
	{
		if (isDisplayed || !PlayerItemManager.IsBusy)
		{
			SetBookActive(!isDisplayed);
		}
	}

	public void SetBookActive(bool value, bool atStart = false)
	{
		isDisplayed = value;
		if (player.IsLocalPlayer)
		{
			if (isDisplayed)
			{
				SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.SwitchCurrentActionMap("UI");
			}
			else
			{
				SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.SwitchCurrentActionMap("Player");
			}
		}
		DoBookDisplayToggle(atStart);
	}

	public void DoBookDisplayToggle(bool atStart = false)
	{
		if (isDisplayed)
		{
			SetBookVisible(atStart);
		}
		else
		{
			SetBookHidden(atStart);
		}
	}

	public bool IsAllNotesUnlockedOfType(ChunkPointType landmarkEnum)
	{
		return landmarkEnum switch
		{
			ChunkPointType.Landmark_RadioTower => IsAllNotesUnlockedInList(radarTowerNotes), 
			ChunkPointType.Landmark_Vasagatan => IsAllNotesUnlockedInList(vasagatanNotes), 
			ChunkPointType.Landmark_Balboa => IsAllNotesUnlockedInList(balboaNotes), 
			ChunkPointType.Landmark_CaravanIsland => IsAllNotesUnlockedInList(caravanNotes), 
			ChunkPointType.Landmark_Tangaroa => IsAllNotesUnlockedInList(tangaroaNotes), 
			ChunkPointType.Landmark_VarunaPoint => IsAllNotesUnlockedInList(varunaNotes), 
			ChunkPointType.Landmark_Temperance => IsAllNotesUnlockedInList(temperanceNotes), 
			_ => false, 
		};
	}

	public uint GetCurrentPageIndex()
	{
		return currentPageIndex;
	}

	public NoteBookNote GetNoteFromIndex(int noteIndex)
	{
		if (notes == null)
		{
			return null;
		}
		for (int i = 0; i < notes.Length; i++)
		{
			NoteBookNote noteBookNote = notes[i];
			if (!(noteBookNote == null) && noteBookNote.noteIndex == noteIndex)
			{
				return noteBookNote;
			}
		}
		return null;
	}

	private bool IsAllNotesUnlockedInList(List<NoteBookNote> noteList)
	{
		foreach (NoteBookNote note in noteList)
		{
			if (!note.isUnlocked)
			{
				return false;
			}
		}
		return true;
	}

	private void PopulateLandmarNotesListsByNoteEnum()
	{
		radarTowerNotes = new List<NoteBookNote>();
		vasagatanNotes = new List<NoteBookNote>();
		balboaNotes = new List<NoteBookNote>();
		varunaNotes = new List<NoteBookNote>();
		temperanceNotes = new List<NoteBookNote>();
		NoteBookNote[] array = notes;
		foreach (NoteBookNote noteBookNote in array)
		{
			switch (noteBookNote.landmarkType)
			{
			case ChunkPointType.Landmark_RadioTower:
				radarTowerNotes.Add(noteBookNote);
				break;
			case ChunkPointType.Landmark_Vasagatan:
				vasagatanNotes.Add(noteBookNote);
				break;
			case ChunkPointType.Landmark_Balboa:
				balboaNotes.Add(noteBookNote);
				break;
			case ChunkPointType.Landmark_CaravanIsland:
				caravanNotes.Add(noteBookNote);
				break;
			case ChunkPointType.Landmark_Tangaroa:
				tangaroaNotes.Add(noteBookNote);
				break;
			case ChunkPointType.Landmark_VarunaPoint:
				varunaNotes.Add(noteBookNote);
				break;
			case ChunkPointType.Landmark_Temperance:
				temperanceNotes.Add(noteBookNote);
				break;
			}
		}
	}

	private void SetBookVisible(bool atStart)
	{
		noteBookRenderer.enabled = true;
		if (!atStart)
		{
			OpenBookSound();
		}
		UpdatePages();
		UpdateQuestItemUI();
		if (player != null)
		{
			if (player.PlayerItemManager != null)
			{
				player.PlayerItemManager.HideItemInHand();
			}
			if (player.IsLocalPlayer)
			{
				if (noteBookCanvas.worldCamera == null)
				{
					noteBookCanvas.worldCamera = player.HandCamera;
				}
				PlayerItemManager.IsBusy = true;
				CanvasHelper.ActiveMenu = MenuType.Journal;
				ComponentManager<CanvasHelper>.Value.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Notebook);
			}
			player.Animator.SetAnimation(PlayerAnimation.Index_29_Notebook);
			noteBookAnimator.SetBool("Open", value: true);
		}
		CancelInvoke();
		Invoke("SetBookVisibleLate", hideBookDelay);
	}

	private void SetBookVisibleLate()
	{
		noteBookCanvas.gameObject.SetActiveSafe(value: true);
		if (player.IsLocalPlayer)
		{
			Helper.SetCursorVisibleAndLockState(state: true, CursorLockMode.None);
			player.PlayerScript.SetMouseLookScripts(canLook: false);
			player.cameraBobAnimator.SetBool("Centered", value: true);
			if (lightSource != null)
			{
				lightSource.gameObject.SetActiveSafe(value: true);
			}
		}
		if (OnBookActiveChanged != null)
		{
			OnBookActiveChanged(obj: true);
		}
	}

	private void SetBookHidden(bool atStart = false)
	{
		noteBookCanvas.gameObject.SetActiveSafe(value: false);
		player.Animator.SetAnimation(PlayerAnimation.Trigger_ItemHitTrigger, triggering: true);
		noteBookAnimator.SetBool("Open", value: false);
		if (!atStart)
		{
			CloseBookSound();
		}
		if (player.IsLocalPlayer)
		{
			Helper.SetCursorVisibleAndLockState(state: false, CursorLockMode.Locked);
			player.PlayerScript.SetMouseLookScripts(canLook: true);
			player.cameraBobAnimator.SetBool("Centered", value: false);
			if (lightSource != null)
			{
				lightSource.gameObject.SetActiveSafe(value: false);
			}
		}
		CancelInvoke();
		Invoke("SetBookHiddenLate", hideBookDelay);
	}

	private void SetBookHiddenLate()
	{
		noteBookRenderer.enabled = false;
		if (player.IsLocalPlayer)
		{
			if (CanvasHelper.ActiveMenu == MenuType.Journal)
			{
				PlayerItemManager.IsBusy = false;
				CanvasHelper.ActiveMenu = MenuType.None;
				ComponentManager<CanvasHelper>.Value.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Inventory);
			}
			player.Inventory.hotbar.ReselectCurrentSlot();
		}
		if (OnBookActiveChanged != null)
		{
			OnBookActiveChanged(obj: false);
		}
	}

	private NoteBookPage[] GetPageFromIndex(params uint[] pageIndexes)
	{
		NoteBookPage[] array = new NoteBookPage[pageIndexes.Length];
		NoteBookPage[] array2 = pageObjs;
		foreach (NoteBookPage noteBookPage in array2)
		{
			for (int j = 0; j < pageIndexes.Length; j++)
			{
				if (noteBookPage.pageIndex == pageIndexes[j])
				{
					array[j] = noteBookPage;
					break;
				}
			}
		}
		return array;
	}

	private void PlayFlipPageSound()
	{
		if (soundemitter_FlipPage != null)
		{
			soundemitter_FlipPage.Play();
		}
	}

	private void OpenBookSound()
	{
		if (soundemitter_OpenBook != null)
		{
			soundemitter_OpenBook.Play();
		}
	}

	private void CloseBookSound()
	{
		if (soundemitter_CloseBook != null)
		{
			soundemitter_CloseBook.Play();
		}
	}

	private void InitializeNotesAndPages()
	{
		for (int i = 0; i < notes.Length; i++)
		{
			notes[i].gameObject.SetActiveSafe(notes[i].isUnlocked);
			if (notes[i].isUnlocked)
			{
				noteBook.UnlockSpecificNoteWithUniqueNoteIndex(notes[i].noteIndex, toUnlock: true, showNotification: false);
			}
		}
	}

	private void UpdatePages()
	{
		UpdateAllNotesVisibility();
		NoteBookPage[] array = pageObjs;
		foreach (NoteBookPage noteBookPage in array)
		{
			if (noteBookPage.pageIndex > highestUnlockedPageIndex && noteBookPage.HasNotesUnlockedOnPage())
			{
				highestUnlockedPageIndex = noteBookPage.pageIndex;
			}
			noteBookPage.SetPageActive(value: false);
		}
		leftPage.SetPageActive(value: false);
		if (rightPage != null)
		{
			rightPage.SetPageActive(value: false);
		}
		if (player.IsLocalPlayer)
		{
			bool value = highestUnlockedPageIndex - currentPageIndex >= 2;
			rightPageFlipImage.gameObject.SetActiveSafe(value);
			leftPageFlipImage.gameObject.SetActiveSafe(currentPageIndex >= 2);
		}
		leftPage = FindNotePageByIndex((int)currentPageIndex);
		rightPage = FindNotePageByIndex((int)(currentPageIndex + 1));
		if (rightPage != null)
		{
			rightPage.SetPageActive(value: true);
		}
		leftPage.SetPageActive(value: true);
	}

	private void UpdateQuestItemUI()
	{
		if (!questItemUIs.ContainsItems())
		{
			questItemUIs = transform.GetComponentsInChildren<NoteBook_QuestItem>(includeInactive: true);
		}
		if (questItemUIs == null)
		{
			return;
		}
		QuestItem[] questItems = QuestItemManager.QuestItems;
		int num = 0;
		foreach (QuestItem questItem in questItems)
		{
			if (questItem != null && questItem.itemCount > 0)
			{
				if (num < questItemUIs.Length)
				{
					NoteBook_QuestItem obj = questItemUIs[num++];
					obj.gameObject.SetActiveSafe(value: true);
					obj.SetItem(questItem, questItemColors.GetQuestItemColorFromDestination(questItem.SOQuestItem.destinationType));
				}
				else
				{
					Debug.LogError("Could not show " + questItem.SOQuestItem.DisplayName + " in notebook because all QuestItemUIs are in use");
				}
			}
		}
		for (int j = num; j < questItemUIs.Length; j++)
		{
			questItemUIs[j].gameObject.SetActiveSafe(value: false);
		}
	}

	private bool CanUseNoteBook()
	{
		if (ChatTextFieldController.IsChatWindowSelected)
		{
			return false;
		}
		if (player != null && player.currentModel.thirdPersonSettings.ThirdPersonModel)
		{
			return false;
		}
		if (player.CarryingComponent.IsCarrying)
		{
			return false;
		}
		if (player.RessurectComponent.BeingCarried || player.RessurectComponent.IsCarrying)
		{
			return false;
		}
		if (player.BedComponent.Sleeping)
		{
			return false;
		}
		if (player.PlayerNetworkManager.IsAttached)
		{
			return false;
		}
		if (player.PlayerScript.IsDead)
		{
			return false;
		}
		if (ClawChallengeConsole.InFocus)
		{
			return false;
		}
		return true;
	}

	private void SendBookActiveMessage()
	{
		Message_NoteBook_SetBookVisibleState message = new Message_NoteBook_SetBookVisibleState(Messages.NoteBook_SetBookVisibleState, network.NetworkIDManager, noteBook.ObjectIndex, isDisplayed, player.steamID);
		if (Raft_Network.IsHost)
		{
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void SendFlipPageMessage()
	{
		Message_NoteBook_ChangePage message = new Message_NoteBook_ChangePage(Messages.NoteBook_FlipPage, network.NetworkIDManager, noteBook.ObjectIndex, (int)currentPageIndex, player.steamID);
		if (Raft_Network.IsHost)
		{
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}
}
