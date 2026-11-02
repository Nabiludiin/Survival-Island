using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Steamworks;
using UltimateWater;
using UnityEngine;

public class SoundManager : MonoBehaviour_ID_Network, SceneEventInterface
{
	public EventReference er_Underwater;

	public EventReference er_Pause;

	public EventReference er_BlockBreak;

	public EventReference er_UI_Click;

	public EventReference er_UI_Click_Fail;

	public EventReference er_UI_Hightlight;

	public EventReference er_UI_MoveItem;

	public EventReference er_UI_Drop;

	public EventReference er_UI_OpenMenu;

	[SerializeField]
	private StudioEventEmitter eventEmitter_Ocean;

	[SerializeField]
	public StudioEventEmitter eventEmitter_Music;

	[SerializeField]
	public StudioEventEmitter eventEmitter_BossMusic;

	[SerializeField]
	public StudioEventEmitter eventEmitter_CutsceneMusic;

	[SerializeField]
	private StudioEventEmitter eventEmitter_PlayerUnderWater;

	[SerializeField]
	private StudioEventEmitterSustain_Ambient[] ambientSounds;

	[SerializeField]
	private SoundManagerSnaphshot_Ambient[] ambientSnapshots;

	private EventInstance eventInstance_snapshot_UnderWater;

	private EventInstance eventInstance_snapshot_Pause;

	private static bool initialized;

	private static EVENT_CALLBACK dialogueCallback;

	private bool underwaterFilter;

	private Network_Player player;

	private Raft_Network network;

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		switch (sceneEvent)
		{
		case SceneEvent.LeaveGame:
		{
			StudioEventEmitterSustain_Ambient[] array = ambientSounds;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].StopHard();
			}
			SoundManagerSnaphshot_Ambient[] array2 = ambientSnapshots;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].snapShot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			SetOceanRoughness(0f);
			SetOceanPlayerHeight(0f);
			eventInstance_snapshot_Pause.start();
			RegionManager.OnRegionUpdate = (Action)Delegate.Remove(RegionManager.OnRegionUpdate, new Action(OnRegionChanged));
			break;
		}
		case SceneEvent.JoinGame:
			eventEmitter_Ocean.Play();
			eventInstance_snapshot_Pause.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			RegionManager.OnRegionUpdate = (Action)Delegate.Combine(RegionManager.OnRegionUpdate, new Action(OnRegionChanged));
			break;
		}
	}

	private void OnRegionChanged()
	{
		List<Subregion> currentSubregions = RegionManager.CurrentSubregions;
		List<SubregionComponent_SoundType> list = new List<SubregionComponent_SoundType>();
		if (currentSubregions.ContainsItems())
		{
			foreach (Subregion currentSubregion in RegionManager.CurrentSubregions)
			{
				SubregionComponent_Sound subRegionComponent = currentSubregion.GetSubRegionComponent<SubregionComponent_Sound>();
				if (subRegionComponent != null)
				{
					list.AddRangeUniqueOnly(subRegionComponent.soundTypes);
				}
			}
		}
		SoundManagerSnaphshot_Ambient[] array = ambientSnapshots;
		foreach (SoundManagerSnaphshot_Ambient soundManagerSnaphshot_Ambient in array)
		{
			soundManagerSnaphshot_Ambient.snapShot.getPlaybackState(out var state);
			if (list.Contains(soundManagerSnaphshot_Ambient.soundType))
			{
				if (state == PLAYBACK_STATE.STOPPED)
				{
					soundManagerSnaphshot_Ambient.snapShot.start();
				}
			}
			else if (state == PLAYBACK_STATE.PLAYING)
			{
				soundManagerSnaphshot_Ambient.snapShot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}
		for (int j = 0; j < ambientSounds.Length; j++)
		{
			StudioEventEmitterSustain_Ambient studioEventEmitterSustain_Ambient = ambientSounds[j];
			if (list.Contains(studioEventEmitterSustain_Ambient.soundType))
			{
				studioEventEmitterSustain_Ambient.Play();
			}
			else
			{
				studioEventEmitterSustain_Ambient.Stop();
			}
			studioEventEmitterSustain_Ambient.eventEmitter.SetParameter("roughness", WaterRoughnessManager.Roughness);
		}
	}

	private void Awake()
	{
		if (initialized)
		{
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		initialized = true;
		DontDestroyOnLoadManager.AddGameObject(gameObject);
		ComponentManager<SoundManager>.Value = this;
		CanvasHelper.MenuOpenEvent += OnMenuOpen;
		CanvasHelper.MenuCloseEvent -= OnMenuClose;
		dialogueCallback = DialogueEventCallback;
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		eventInstance_snapshot_UnderWater = RuntimeManager.CreateInstance(er_Underwater);
		eventInstance_snapshot_Pause = RuntimeManager.CreateInstance(er_Pause);
		SoundManagerSnaphshot_Ambient[] array = ambientSnapshots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initialize();
		}
		if (eventEmitter_Music != null)
		{
			eventEmitter_Music.Play();
		}
		else
		{
			UnityEngine.Debug.Log("No music event emitter to run Play() on");
		}
		SO_VoiceData[] array2 = Resources.LoadAll<SO_VoiceData>("SO_Dialogue");
		if (array2 == null)
		{
			return;
		}
		foreach (SO_VoiceData sO_VoiceData in array2)
		{
			if (!(sO_VoiceData == null))
			{
				sO_VoiceData.RefreshVoicelineDurations();
			}
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		underwaterFilter = false;
		eventInstance_snapshot_UnderWater.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		eventInstance_snapshot_UnderWater.release();
		eventInstance_snapshot_UnderWater.clearHandle();
		eventInstance_snapshot_Pause.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		eventInstance_snapshot_Pause.release();
		eventInstance_snapshot_Pause.clearHandle();
		SoundManagerSnaphshot_Ambient[] array = ambientSnapshots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Destroy();
		}
		network = null;
		player = null;
	}

	private void OnMenuOpen(MenuType menu)
	{
		if (menu == MenuType.PauseMenu)
		{
			eventInstance_snapshot_Pause.start();
		}
	}

	private void OnMenuClose(MenuType menu)
	{
		if (menu == MenuType.PauseMenu)
		{
			eventInstance_snapshot_Pause.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
	}

	private void Update()
	{
		if (player == null)
		{
			player = ComponentManager<Network_Player>.Value;
			if (player == null)
			{
				return;
			}
		}
		eventInstance_snapshot_Pause.getPlaybackState(out var state);
		if ((CanvasHelper.ActiveMenu == MenuType.PauseMenu && Raft_Network.InSinglePlayerMode) || Raft_Network.InMenuScene)
		{
			if (state == PLAYBACK_STATE.STOPPED)
			{
				eventInstance_snapshot_Pause.start();
			}
		}
		else if (state == PLAYBACK_STATE.PLAYING)
		{
			eventInstance_snapshot_Pause.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		HandleUnderWaterFilter();
	}

	private void HandleUnderWaterFilter()
	{
		if (player.PersonController.SubmersionState == SubmersionState.Full)
		{
			if (!underwaterFilter)
			{
				underwaterFilter = true;
				eventInstance_snapshot_UnderWater.start();
				eventEmitter_PlayerUnderWater.Play();
			}
		}
		else if (underwaterFilter)
		{
			underwaterFilter = false;
			eventInstance_snapshot_UnderWater.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			eventEmitter_PlayerUnderWater.Stop();
		}
	}

	public static EventInstance PlayProgrammerSound(EventReference eventRef, string eventKey)
	{
		EventInstance result = RuntimeManager.CreateInstance(eventRef);
		GCHandle value = GCHandle.Alloc(eventKey, GCHandleType.Pinned);
		result.setUserData(GCHandle.ToIntPtr(value));
		result.setCallback(dialogueCallback);
		result.start();
		result.release();
		return result;
	}

	public static EventInstance PlayProgrammerSound(EventReference eventRef, string eventKey, Transform attachTransform)
	{
		EventInstance eventInstance = RuntimeManager.CreateInstance(eventRef);
		GCHandle value = GCHandle.Alloc(eventKey, GCHandleType.Pinned);
		eventInstance.setUserData(GCHandle.ToIntPtr(value));
		eventInstance.setCallback(dialogueCallback);
		eventInstance.start();
		eventInstance.release();
		RuntimeManager.AttachInstanceToGameObject(eventInstance, attachTransform);
		return eventInstance;
	}

	[MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
	private static RESULT DialogueEventCallback(EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
	{
		new EventInstance(instancePtr).getUserData(out var userdata);
		GCHandle gCHandle = GCHandle.FromIntPtr(userdata);
		string text = gCHandle.Target as string;
		switch (type)
		{
		case EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND:
		{
			MODE mODE = MODE.LOOP_NORMAL | MODE.CREATECOMPRESSEDSAMPLE | MODE.NONBLOCKING;
			PROGRAMMER_SOUND_PROPERTIES structure = (PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(PROGRAMMER_SOUND_PROPERTIES));
			if (text.Contains("."))
			{
				if (RuntimeManager.CoreSystem.createSound(Application.streamingAssetsPath + "/" + text, mODE, out var sound2) == RESULT.OK)
				{
					structure.sound = sound2.handle;
					structure.subsoundIndex = -1;
					Marshal.StructureToPtr(structure, parameterPtr, fDeleteOld: false);
				}
				break;
			}
			RESULT soundInfo = RuntimeManager.StudioSystem.getSoundInfo(text, out var info);
			Sound sound3;
			if (soundInfo != RESULT.OK)
			{
				UnityEngine.Debug.Log(soundInfo);
			}
			else if (RuntimeManager.CoreSystem.createSound(info.name_or_data, mODE | info.mode, ref info.exinfo, out sound3) == RESULT.OK)
			{
				structure.sound = sound3.handle;
				structure.subsoundIndex = info.subsoundindex;
				Marshal.StructureToPtr(structure, parameterPtr, fDeleteOld: false);
			}
			break;
		}
		case EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND:
		{
			PROGRAMMER_SOUND_PROPERTIES pROGRAMMER_SOUND_PROPERTIES = (PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(PROGRAMMER_SOUND_PROPERTIES));
			Sound sound = default;
			sound.handle = pROGRAMMER_SOUND_PROPERTIES.sound;
			sound.release();
			break;
		}
		case EVENT_CALLBACK_TYPE.DESTROYED:
			gCHandle.Free();
			break;
		}
		return RESULT.OK;
	}

	public void PlayBlockBreak(Vector3 position, bool withWaterSound)
	{
		AudioHelper.PlayOneShotWithParameters(er_BlockBreak, position, ("height", withWaterSound ? 0f : 3f));
	}

	public void PlayUI_Click()
	{
		RuntimeManager.PlayOneShot(er_UI_Click);
	}

	public void PlayUI_Click_Fail()
	{
		RuntimeManager.PlayOneShot(er_UI_Click_Fail);
	}

	public void PlayUI_Highlight()
	{
		RuntimeManager.PlayOneShot(er_UI_Hightlight);
	}

	public void PlayUI_Drop()
	{
		RuntimeManager.PlayOneShot(er_UI_Drop);
	}

	public void PlayUI_MoveItem()
	{
		RuntimeManager.PlayOneShot(er_UI_MoveItem);
	}

	public void PlayUI_OpenMenu()
	{
		RuntimeManager.PlayOneShot(er_UI_OpenMenu);
	}

	public void PlayOneShotNetworked(EventReference eventRef, Vector3 position)
	{
		Message_SoundManager_PlayOneShot message = new Message_SoundManager_PlayOneShot(Messages.SoundManager_PlayOneShot, network.NetworkIDManager, ObjectIndex, eventRef.Guid.ToString(), position);
		if (Raft_Network.IsHost)
		{
			if (PlayOneShot(eventRef, position))
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public void PlayOneShotNetworked(EventReference eventRef, Vector3 position, string paramName, float paramValue)
	{
		Message_SoundManager_PlayOneShotParam message = new Message_SoundManager_PlayOneShotParam(Messages.SoundManager_PlayOneShotParam, network.NetworkIDManager, ObjectIndex, eventRef.Guid.ToString(), position, paramName, paramValue);
		if (Raft_Network.IsHost)
		{
			if (PlayOneShot(eventRef, position, paramName, paramValue))
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private bool PlayOneShot(EventReference eventRef, Vector3 position)
	{
		if (eventRef.IsNull)
		{
			return false;
		}
		RuntimeManager.PlayOneShot(eventRef, position);
		return true;
	}

	private bool PlayOneShot(EventReference eventRef, Vector3 position, string paramName, float paramValue)
	{
		if (eventRef.IsNull)
		{
			return false;
		}
		AudioHelper.PlayOneShotWithParameters(eventRef, position, (paramName, paramValue));
		return true;
	}

	public void SetOceanRoughness(float roughness)
	{
		if (eventEmitter_Ocean != null)
		{
			eventEmitter_Ocean.SetParameter("roughness", Mathf.Clamp01(roughness));
		}
	}

	public float GetOceanRoughness()
	{
		float value = 0f;
		if (eventEmitter_Ocean != null)
		{
			eventEmitter_Ocean.EventInstance.getParameterByName("roughness", out value);
		}
		return value;
	}

	public void SetOceanPlayerHeight(float height)
	{
		if (eventEmitter_Ocean != null)
		{
			eventEmitter_Ocean.SetParameter("height", Mathf.Clamp(height, 0f, 15f));
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.SoundManager_PlayOneShot:
		{
			Message_SoundManager_PlayOneShot message_SoundManager_PlayOneShot = msg as Message_SoundManager_PlayOneShot;
			return PlayOneShot(new EventReference
			{
				Guid = GUID.Parse(message_SoundManager_PlayOneShot.EventRef)
			}, message_SoundManager_PlayOneShot.Position);
		}
		case Messages.SoundManager_PlayOneShotParam:
		{
			Message_SoundManager_PlayOneShotParam message_SoundManager_PlayOneShotParam = msg as Message_SoundManager_PlayOneShotParam;
			return PlayOneShot(new EventReference
			{
				Guid = GUID.Parse(message_SoundManager_PlayOneShotParam.EventRef)
			}, message_SoundManager_PlayOneShotParam.Position, message_SoundManager_PlayOneShotParam.ParamName, message_SoundManager_PlayOneShotParam.ParamValue);
		}
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
