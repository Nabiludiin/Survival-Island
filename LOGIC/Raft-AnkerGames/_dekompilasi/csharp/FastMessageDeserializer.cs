using System;
using Unity.Netcode;
using UnityEngine;

public class FastMessageDeserializer
{
	public static RGD_Block CreateBlock(RGDType type)
	{
		return type switch
		{
			RGDType.Block => new RGD_Block(), 
			RGDType.Block_Door => new RGD_Door(), 
			RGDType.Block_Cropplot => new RGD_Cropplot(), 
			RGDType.Block_CookingStand => new RGD_CookingStand(), 
			RGDType.Block_Storage => new RGD_Storage(), 
			RGDType.Block_AnchorStationary => new RGD_AnchorStationary(), 
			RGDType.Block_AnchorThrowable => new RGD_AnchorThrowable(), 
			RGDType.Block_ItemCollector => new RGD_ItemCollector(), 
			RGDType.Block_Scarecrow => new RGD_Scarecrow(), 
			RGDType.Block_Brick => new RGD_Brick(), 
			RGDType.Block_BirdsNest => new RGD_Birdsnest(), 
			RGDType.Block_ResearchTable => new RGD_ResearchTable(), 
			RGDType.Block_Sail => new RGD_Sail(), 
			RGDType.Block_Sharkbait => new RGD_Sharkbait(), 
			RGDType.Block_Reciever => new RGD_Reciever(), 
			RGDType.Block_CookingPot => new RGD_Block_CookingPot(), 
			RGDType.Block_Sprinkler => new RGD_Block_Sprinkler(), 
			RGDType.Block_Radio => new RGD_Block_Radio(), 
			RGDType.Block_SteeringWheel => new RGD_SteeringWheel(), 
			RGDType.Block_Motor_Wheel => new RGD_MotorWheel(), 
			RGDType.Block_BioFuelCreator => new RGD_Block_BiofuelExtractor(), 
			RGDType.Block_FuelTank => new RGD_Block_FuelTank(), 
			RGDType.Block_BeeHive => new RGD_Block_BeeHive(), 
			RGDType.Block_BatteryCharger => new RGD_Block_BatteryCharger(), 
			RGDType.Block_ZiplineBase => new RGD_Block(), 
			RGDType.Block_Foundation => new RGD_Block_Foundation(), 
			RGDType.Block_Firework => new RGD_Block_Firework(), 
			RGDType.Block_Electric_Purifier => new RGD_Block_ElectricPurifier(), 
			RGDType.Block_WindTurbine => new RGD_Block_WindTurbine(), 
			RGDType.Block_Recycler => new RGD_Block_Recycler(), 
			RGDType.Block_EngineControls => new RGD_Block(), 
			RGDType.Block_Fence => new RGD_Block_Fence(), 
			RGDType.Block_Interactable => new RGD_Block_Interactable(), 
			RGDType.Block_TicTacToe => new RGD_Block_TicTacToe(), 
			RGDType.Block_ShortcutPlank => new RGD_Block_DetailPlank(), 
			RGDType.Block_Electric_CookingStand => new RGD_Cookingstand_Smelter_Electric(), 
			RGDType.ID_TrophyHolder => new RGDTrophyHolder(), 
			_ => throw new ArgumentOutOfRangeException("type", type, null), 
		};
	}

	public static T DeserializeMessage<T>(FastBufferReader reader) where T : Message
	{
		reader.ReadValue(out short value, default(FastBufferWriter.ForPrimitives));
		Message message;
		switch ((Messages)value)
		{
		case Messages.Compound:
			message = new Message_Compound();
			break;
		case Messages.InitiateSession:
			message = new Message_InitiateConnection();
			break;
		case Messages.InitiateResultSteam:
			message = new Message_InitiateConnectionResultSteam();
			break;
		case Messages.InitiateResult:
			message = new Message_InitiateConnectionResult();
			break;
		case Messages.NetworkVersion:
			message = new Message_NetworkVersion();
			break;
		case Messages.Ping:
			message = new Message_PingPong();
			break;
		case Messages.Pong:
			message = new Message_PingPong();
			break;
		case Messages.RequestWorld:
			message = new Message();
			break;
		case Messages.WorldGuid:
			message = new Message_WorldInfo();
			break;
		case Messages.PlayerJoined:
			message = new Message_PlayerJoined();
			break;
		case Messages.PlayerKilled:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.Update:
			message = new Message_Player_Update();
			break;
		case Messages.Update_Throwable:
			message = new Message_ThrowableUpdate();
			break;
		case Messages.Raft_Create:
			message = new Message_Raft();
			break;
		case Messages.Raft_Update:
			message = new Message_Raft();
			break;
		case Messages.CreateHostNetwork:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.CreatePlayer:
			message = new Message_Player_Create();
			break;
		case Messages.CreateHostNetworkEntities:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.ObjectSpawner_Create:
			message = new Message_SpawnerManager_Create();
			break;
		case Messages.ObjectSpawner_SpawnItem:
			message = new Message_ObjectSpawner_SpawnItem();
			break;
		case Messages.Shark_Update:
			message = new Message_Shark();
			break;
		case Messages.Shark_Bait:
			message = new Message_SharkBait();
			break;
		case Messages.BlockCreator_Create:
			message = new Message_BlockCreator_Create();
			break;
		case Messages.BlockCreator_CreateEndMessage:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.BlockCreator_PlaceBlock:
			message = new Message_BlockCreator_PlaceBlock();
			break;
		case Messages.BlockCreator_PlaceBlockDetailPlank:
			message = new Message_BlockCreator_PlaceBlock_DetailPlank();
			break;
		case Messages.BlockCreator_UpgradeBlock:
			message = new Message_BlockCreator_UpgradeBlock();
			break;
		case Messages.BlockCreator_RemoveBlock:
			message = new Message_BlockCreator_RemoveBlock();
			break;
		case Messages.BlockCreator_UpdateHealth:
			message = new Message_BlockCreator_UpdateHealth();
			break;
		case Messages.BlockCreator_SetReinforced:
			message = new Message_BlockCreator_SetReinforced();
			break;
		case Messages.ItemManager_Create:
			message = new Message_SelectItem();
			break;
		case Messages.ItemManager_SelectItem:
			message = new Message_SelectItem();
			break;
		case Messages.RemoveNetworkBehaviour:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.SetIDBehaviourDead:
			message = new Message_SetIDBehaviourDead();
			break;
		case Messages.DamageEntity:
			message = new Message_NetworkEntity_Damage();
			break;
		case Messages.ItemCollectorCollect:
			message = new Message_ItemCollectorCollect();
			break;
		case Messages.ItemCollectorClear:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.ItemCollectorCreate:
			message = new Message_ItemCollectorCreate();
			break;
		case Messages.RemoveItem:
			message = new Message_PickupObjectManager_RemoveItem();
			break;
		case Messages.PlantManagerCreate:
			message = new Message_PlantManagerCreate();
			break;
		case Messages.PlantManager_PlantSeed:
			message = new Message_PlantSeed();
			break;
		case Messages.PlantManager_HarvestPlant:
			message = new Message_HarvestPlant();
			break;
		case Messages.PlantManager_WaterPlant:
			message = new Message_WaterCrop();
			break;
		case Messages.PlantManager_PlantComplete:
			message = new Message_Plant_Complete();
			break;
		case Messages.ConnectThrowableAnchorStand:
			message = new Message_NetworkBehaviour_SteamID();
			break;
		case Messages.DisconnectThrowableAnchorStand:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.ThrowableThrow:
			message = new Message_Throw();
			break;
		case Messages.ThrowableStandCreate:
			message = new Message_ThrowableStandCreate();
			break;
		case Messages.StationaryAnchorUse:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.StationaryAnchorCreate:
			message = new Message_StationaryAnchorCreate();
			break;
		case Messages.Door_Open:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.Door_Close:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.CreateNetworkPickupManager:
			message = new Message_PickupManager_Create();
			break;
		case Messages.CookingStand_InsertItem:
			message = new Message_CookingStand_Insert();
			break;
		case Messages.CookingStand_CollectItem:
			message = new Message_CookingStand_Collect();
			break;
		case Messages.CookingStand_IncrementFuel:
			message = new Message_CookingStand_IncrementFuel();
			break;
		case Messages.CookingStand_SlotComplete:
			message = new Message_CookingStand_SlotComplete();
			break;
		case Messages.StorageManager_Create:
			message = new Message_Storage_Create();
			break;
		case Messages.StorageManager_Open:
			message = new Message_Storage();
			break;
		case Messages.StorageManager_Close:
			message = new Message_Storage_Close();
			break;
		case Messages.DropItem:
			message = new Message_DropItem();
			break;
		case Messages.Brick_Pickup:
			message = new Message_Brick();
			break;
		case Messages.Brick_Complete:
			message = new Message_Brick();
			break;
		case Messages.PlayerNetworkManager_Create:
			message = new Message_PlayerNetworkManager_Create();
			break;
		case Messages.Respawn_Start:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.Respawn_Complete:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.CreateBedManager:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.Bed_Enter:
			message = new Message_Bed_Enter();
			break;
		case Messages.Bed_Exit:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.SlumberTime:
			message = new Message_BedManager_Slumber();
			break;
		case Messages.WorldInfo_Create:
			message = new Message_WorldInfo_Create();
			break;
		case Messages.WorldInfo_Update:
			message = new Message_WorldInfo_Update();
			break;
		case Messages.RessurectComponent_StartCarry:
			message = new Message_NetworkBehaviour_SteamID();
			break;
		case Messages.RessurectComponent_StopCarry:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.RessurectComponent_PlaceInBed:
			message = new Message_Bed_Enter();
			break;
		case Messages.Seagull_Create:
			message = new Message_Seagull_Create();
			break;
		case Messages.Seagull_Update:
			message = new Message_Seagull_Update();
			break;
		case Messages.Scarecrow_Create:
			message = new Message_Scarecrow_Create();
			break;
		case Messages.Scarecrow_Damage:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.BirdsNest_Create:
			message = new Message_Birdnest_Create();
			break;
		case Messages.BirdsNest_LayEgg:
			message = new Message_Birdnest_Lay();
			break;
		case Messages.BirdsNest_PickupEgg:
			message = new Message_NetworkBehaviour_SteamID();
			break;
		case Messages.BirdsNest_LayFeather:
			message = new Message_Birdnest_Lay();
			break;
		case Messages.BirdsNest_PickupFeather:
			message = new Message_NetworkBehaviour_SteamID();
			break;
		case Messages.WorldManager_ResetWorldToCenter:
			message = new Message_WorldShiftManager_WorldShift();
			break;
		case Messages.WorldManager_CreateLandmarks:
			message = new Message_WorldManager_CreateLandmarks();
			break;
		case Messages.Paddle:
			message = new Message_Paddle();
			break;
		case Messages.PaintBlock:
			message = new Message_PaintBlock();
			break;
		case Messages.ResearchTable_Create:
			message = new Message_ResearchTable_Create();
			break;
		case Messages.ResearchTable_Open:
			message = new Message_NetworkBehaviour_SteamID();
			break;
		case Messages.ResearchTable_Close:
			message = new Message_ResearchTable_Close();
			break;
		case Messages.ResearchTable_Research:
			message = new Message_ResearchTable_ResearchOrLearn();
			break;
		case Messages.ResearchTable_Learn:
			message = new Message_ResearchTable_ResearchOrLearn();
			break;
		case Messages.ResearchBlueprint:
			message = new Message_ResearchTable_ResearchOrLearn();
			break;
		case Messages.Consume:
			message = new Message_Consume();
			break;
		case Messages.AxeHit:
			message = new Message_AxeHit();
			break;
		case Messages.Axe_RemoveBlock:
			message = new Message_BlockCreator_RemoveBlock();
			break;
		case Messages.Sail_Open:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.Sail_Close:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.Sail_Rotate:
			message = new Message_Sail_Rotate();
			break;
		case Messages.Sail_Create:
			message = new Message_Sail_Create();
			break;
		case Messages.RaftLandmark_Update:
			message = new Message_RaftLandmark_Update();
			break;
		case Messages.RaftLandmark_StartSinking:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.WeatherManager_SetWeather:
			message = new Message_WeatherManager_SetWeather();
			break;
		case Messages.Equipment_Create:
			message = new Message_Equipment_Create();
			break;
		case Messages.Equipment_Equip:
			message = new Message_Equipment();
			break;
		case Messages.Equipment_UnEquip:
			message = new Message_Equipment();
			break;
		case Messages.Battery_Insert:
			message = new Message_Battery();
			break;
		case Messages.Battery_Take:
			message = new Message_Battery();
			break;
		case Messages.Battery_Update:
			message = new Message_Battery();
			break;
		case Messages.Battery_OnOff:
			message = new Message_Battery_OnOff();
			break;
		case Messages.Reciever_Create:
			message = new Message_Reciever_Create();
			break;
		case Messages.Reciever_Antenna_Connect:
			message = new Message_Reciever_Antenna();
			break;
		case Messages.Reciever_FrequencyUpdate:
			message = new Message_Reciever_FrequencyUpdate();
			break;
		case Messages.Blueprint_Switch:
			message = new Message_Item();
			break;
		case Messages.StoneBird_DropStone:
			message = new Message_NetworkBehaviour_ID_Vector3();
			break;
		case Messages.CreateAINetworkBehaviour:
			message = new Message_CreateAINetworkBehaviour();
			break;
		case Messages.Save:
			message = new Message();
			break;
		case Messages.PufferFish_Explode:
			message = new Message_NetworkBehaviour_ID_Vector3();
			break;
		case Messages.Ingame_Chat_Message:
			message = new Message_IngameChat();
			break;
		case Messages.Create_Chat_Manager:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.CookingPot_Pickup:
			message = new Message_CookingPot_Slot();
			break;
		case Messages.CookingPot_Insert:
			message = new Message_CookingPot_Slot();
			break;
		case Messages.CookingPot_PickupFood:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.CookingPot_Start:
			message = new Message_CookingPot_Recipe();
			break;
		case Messages.CookingPot_Finish:
			message = new Message_CookingPot_Recipe();
			break;
		case Messages.CreateNetworkIDManager:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.TrophyHolder_Create:
			message = new Message_TrophyBoardCreate();
			break;
		case Messages.TrophyHolder_Pickup:
			message = new Message_PickupTrophyOnBoard();
			break;
		case Messages.TrophyHolder_Place:
			message = new Message_PlaceTrophyOnBoard();
			break;
		case Messages.Sign_Create:
			message = new Message_Sign_Create();
			break;
		case Messages.Sign_StartEdit:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.Sign_EndEdit:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.Sign_OnEdit:
			message = new Message_OnEditText();
			break;
		case Messages.Fuel_Increment:
			message = new Message_Fuel();
			break;
		case Messages.ThrowableComponent_Throw:
			message = new Message_ThrowableComponent_Throw();
			break;
		case Messages.ThrowableComponent_Collision:
			message = new Message_ThrowableComponent_Collision();
			break;
		case Messages.Shovel_Use:
			message = new Message_UseShovel();
			break;
		case Messages.ResourceRegenerativeChange:
			message = new Message_OnResourceChange();
			break;
		case Messages.AnimalCaptured:
			message = new Message_AnimalCaptured();
			break;
		case Messages.Boar_Impale_Player:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.Boar_Depale_Player:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.DamageBox_Hit:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.StartCarryAIBehaviour:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.StopCarryAIBehaviour:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.StartCarryItem:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.StopCarryItem:
			message = new Message_StopCarryPosition();
			break;
		case Messages.StopCarryItemInterest:
			message = new Message_StopCarryObjectInterest();
			break;
		case Messages.StopCarryStack:
			message = new Message_StopCarryStack();
			break;
		case Messages.CreateDomestic:
			message = new Message_Domestic_Create();
			break;
		case Messages.CreateDomesticResource:
			message = new Message_Domestic_Resource_Create();
			break;
		case Messages.Chicken_LayEgg:
			message = new Message_Chicken_LayEgg();
			break;
		case Messages.Chicken_CreateEggs:
			message = new Message_ChickenEggs();
			break;
		case Messages.RareMaterial:
			message = new Message_RareMaterial();
			break;
		case Messages.QuickTameAnimal:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.Tank:
			message = new Message_Tank();
			break;
		case Messages.Tank_SetValue:
			message = new Message_Tank();
			break;
		case Messages.Sprinkler_Create:
			message = new Message_Sprinkler_Create();
			break;
		case Messages.Sprinkler_Water:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.ChunkManager_Create:
			message = new Message_ChunkManager_ChunkPoints();
			break;
		case Messages.ChunkManager_RefreshChunkPoints:
			message = new Message_ChunkManager_ChunkPoints();
			break;
		case Messages.Radio_Create:
			message = new Message_RadioPlayer_Create();
			break;
		case Messages.Radio_Toggle:
			message = new Message_RadioPlayer_Toggle();
			break;
		case Messages.SteeringWheelCreate:
			message = new Message_SteeringWheel_Create();
			break;
		case Messages.SteeringWheelRotate:
			message = new Message_SteeringWheel_Rotate();
			break;
		case Messages.ExtractorProcessTanks:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.ExtractorHarvestOutput:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.MotorWheelCreate:
			message = new Message_MotorWheelCreate();
			break;
		case Messages.MotorWheel_Inverse:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.MotorWheel_PowerButton:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.NoteBook_Create:
			message = new Message_NoteBook_Create();
			break;
		case Messages.NoteBook_SetNoteUnlockState:
			message = new Message_NoteBook_SetNoteState();
			break;
		case Messages.NoteBook_FlipPage:
			message = new Message_NoteBook_ChangePage();
			break;
		case Messages.NoteBook_SetBookVisibleState:
			message = new Message_NoteBook_SetBookVisibleState();
			break;
		case Messages.InteractableButtonPress:
			message = new Message_InteractableButtonPress();
			break;
		case Messages.QuestCreate:
			message = new Message_Quest_Create();
			break;
		case Messages.QuestRestore:
			message = new Message_RestoreQuests();
			break;
		case Messages.QuestFinish:
			message = new Message_FinishQuest();
			break;
		case Messages.QuestProgress:
			message = new Message_ProgressQuest();
			break;
		case Messages.QuestItemModifyCount:
			message = new Message_Quest_ModifyItemCount();
			break;
		case Messages.QuestEventInteract:
			message = new Message_QuestEventInteract();
			break;
		case Messages.WorldReceived:
			message = new Message();
			break;
		case Messages.AINetworkBehaviourUpdate_Domestic:
			message = new Message_Domestic_Update();
			break;
		case Messages.AINetworkBehaviourUpdate_Animal:
			message = new Message_Animal_Update();
			break;
		case Messages.AINetworkBehaviourUpdate_NPC:
			message = new Message_NPC_Update();
			break;
		case Messages.AINetworkBehaviourUpdate_PufferFish:
			message = new Message_PufferFish_Update();
			break;
		case Messages.AINetworkBehaviourUpdate_Shark:
			message = new Message_NewShark_Update();
			break;
		case Messages.AINetworkBehaviourUpdate_StoneBird:
			message = new Message_StoneBird_Update();
			break;
		case Messages.AINetworkBehaviourCreate:
			message = new Message_AINetworkBehaviours_Create();
			break;
		case Messages.AINetworkAnimatorUpdate:
			message = new Message_Animal_AnimTriggers();
			break;
		case Messages.PickupChannel:
			message = new Message_PickupChannel();
			break;
		case Messages.Balboa_BicyclePling:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.StartSinkingJumpObstacle:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.Teleport:
			message = new Message_Teleport();
			break;
		case Messages.BatteryCharger_Create:
			message = new Message_BatteryCharger_Create();
			break;
		case Messages.TreasurePointManager_Spawn:
			message = new Message_TreasurePointManager_Spawn();
			break;
		case Messages.TreasurePointManager_Create:
			message = new Message_TreasurePoint_Create();
			break;
		case Messages.TreasurePoint_Excevate:
			message = new Message_TreasurePoint_Excevate();
			break;
		case Messages.TreasurePoint_Reveal:
			message = new Message_TreasurePoint_Reveal();
			break;
		case Messages.TreasurePoint_Move:
			message = new Message_TreasurePoint_Move();
			break;
		case Messages.BeeHiveCreate:
			message = new Message_BeeHive_Create();
			break;
		case Messages.BeeHiveHarvestOutput:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.BeeHiveFillYield:
			message = new Message_FillYield();
			break;
		case Messages.RotationComponentRotate:
			message = new Message_RotationComponentRotate();
			break;
		case Messages.RotationComponentSetRotation:
			message = new Message_RotationComponentSetRotation();
			break;
		case Messages.RotationComponentRequestRotation:
			message = new Message_RotationComponentSetRotation();
			break;
		case Messages.Increment:
			message = new Message_Network_Increment();
			break;
		case Messages.ZiplineAttach:
			message = new Message_Zipline_Attach();
			break;
		case Messages.ZiplineDetach:
			message = new Message_NetworkBehaviour();
			break;
		case Messages.Tangaroa_CockpitLaunch:
			message = new Message_TangaroaCockpitLaunch();
			break;
		case Messages.FireworkHand_Launch:
			message = new Message_Firework_Launch();
			break;
		case Messages.FireworkBlock_LightFuse:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.Firework_Explode:
			message = new Message_Firework_Explode();
			break;
		case Messages.TikiPoleCompleted:
			message = new TikiPole_Head.Message_TikiPoleComplete();
			break;
		case Messages.SoundManager_Create:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.SoundManager_PlayOneShot:
			message = new Message_SoundManager_PlayOneShot();
			break;
		case Messages.SoundManager_PlayOneShotParam:
			message = new Message_SoundManager_PlayOneShotParam();
			break;
		case Messages.Instrument_KeyModified:
			message = new Message_InstrumentKeyModified();
			break;
		case Messages.Container_SpawnObject:
			message = new Message_Container_ObjectIndex();
			break;
		case Messages.Container_DestroyObject:
			message = new Message_Container_ObjectIndex();
			break;
		case Messages.Charge_Hit:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.HazmatSuit:
			message = new Message_HazmatSuit();
			break;
		case Messages.ComputerChallenge_StartTimer:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.ComputerChallenge_ResetTimer:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.ComputerChallenge_Complete:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.Extractor_Create:
			message = new Message_Extractor_Create();
			break;
		case Messages.Toggle_Held:
			message = new Fight_Challenge_Selene.Message_ToggleHeld();
			break;
		case Messages.MeshPath_Create:
			message = new Message_MeshPath();
			break;
		case Messages.MeshPath_Remove:
			message = new Message_MeshPath();
			break;
		case Messages.MeshPath_CreateAll:
			message = new Message_MeshPath_CreateAll();
			break;
		case Messages.TradingPost_Purchase:
			message = new Message_TradingPost();
			break;
		case Messages.TradingPost_Reputation:
			message = new Message_TradingPost();
			break;
		case Messages.Snowmobile_TakeSeat:
			message = new Message_Snowmobile_TakeSeat();
			break;
		case Messages.Snowmobile_LeaveSeat:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.Snowmobile_Update:
			message = new Message_Snowmobile_Update();
			break;
		case Messages.Snowmobile_Impact:
			message = new Message_NetworkBehaviour_ID_Float();
			break;
		case Messages.SnowmobileShed_Spawn:
			message = new Message_Snowmobile_Spawn();
			break;
		case Messages.SnowmobileShed_Reset:
			message = new Message_NetworkBehaviour_ID_Int();
			break;
		case Messages.AI_Protect:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.PlayerSeat_Take:
			message = new Message_PlayerTakeSeat();
			break;
		case Messages.PlayerSeat_Leave:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.WorldEventToggle_On:
			message = new Message_WorldEventToggle_On();
			break;
		case Messages.WorldEventToggle_Off:
			message = new Message_WorldEventToggle_Off();
			break;
		case Messages.WorldEvent_Create:
			message = new Message_WorldEvent_Create();
			break;
		case Messages.Whale_ShootWater:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.Dialogue:
			message = new Message_Dialogue();
			break;
		case Messages.ThrowExplosive:
			message = new ExplosiveThrower.Message_ThrowExplosive();
			break;
		case Messages.AttachPlayerMeleeAttack:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.OlofThrowMeat:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.HyenaBoss_StartPin:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.HyenaBoss_StopPin:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.HyenaBoss_SpawnStone:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.HyenaBoss_ThrowStone:
			message = new Message_NetworkBehaviour_ID_Vector3();
			break;
		case Messages.HyenaBoss_DigStoneMultiplier:
			message = new Message_NetworkBehaviour_ID_Float();
			break;
		case Messages.HyenaBoss_LandJump:
			message = new Message_NetworkBehaviour_ID_Vector3();
			break;
		case Messages.HyenaBoss_StartCharge:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.Character_ChangeOutfit:
			message = new Message_NetworkBehaviour_Int();
			break;
		case Messages.Wardrobe_Open:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.Wardrobe_Close:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.RemoteAnchorsDown:
			message = new Message_NetworkBehaviour_ID_Int();
			break;
		case Messages.CharacterUnlock_Interact:
			message = new Message_NetworkBehaviour_ID_SteamID();
			break;
		case Messages.FinalRoomOlofFall:
			message = new Message_NetworkBehaviour_ID();
			break;
		case Messages.InteractableButtonPress_Int:
			message = new Message_InteractableButtonPress_Integer();
			break;
		case Messages.KickUser:
			message = new Message();
			break;
		default:
			Debug.LogError($"Received an invalid message type: {value}");
			return null;
		}
		message.t = value;
		message.DeserializeFast(reader);
		return (T)message;
	}
}
