using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RG.Parsecs.Scavenge;

public class ShipGenerator : MonoBehaviour
{
	public BaseSeed Seed;

	private BaseSeed RuntimeSeed;

	private int _runtimeItemSeed;

	private int _runtimeLeftModuleSeed;

	private int _runtimeRightModuleSeed;

	public bool TransparentWalls = true;

	public SceneField TutorialAddon;

	public List<RoomType> roomsWithCharacters = new List<RoomType>();

	public bool IsUnloading;

	public ShipModulesDatabase ShipModulesDatabase;

	public RoomsDatabase RoomsDatabase;

	public RoomMaterialsDatabase RoomMaterialsDatabase;

	public RoomMaterialsDatabase RoomTransparentMaterialsDatabase;

	public RoomMaterialsDatabase RoomFloorMaterialsDatabase;

	public Material OutsideMaterial;

	public Vector3 Pivot;

	private List<ModuleLoader> stillLoading = new List<ModuleLoader>();

	private List<Scene> Scenes = new List<Scene>();

	private List<ItemSpawnPoint> _spawnPoints = new List<ItemSpawnPoint>();

	private List<DropSoup> _soupMachines;

	public ItemsDistribute itemsDistribute;

	private StringBuilder b;

	private List<ShipModule> shipModules = new List<ShipModule>();

	private ScavengeTutorialDriver _tutorialDriver;

	private RescurePodEntrance _rescurePodEntrance;

	private Dictionary<Room, RoomType> _roomList = new Dictionary<Room, RoomType>();

	private List<BottomCorridorWall> _bottomCorridorWalls = new List<BottomCorridorWall>();

	public static ShipGenerator I { get; protected set; }

	public object StringBuilder { get; private set; }

	private void Awake()
	{
		I = this;
		itemsDistribute.Reset();
		_soupMachines = new List<DropSoup>();
	}

	private void OnDestroy()
	{
		I = null;
		_bottomCorridorWalls.Clear();
		_tutorialDriver = null;
	}

	private string RoomTypesListPrint(List<RoomType> list)
	{
		if (b == null)
		{
			b = new StringBuilder();
		}
		b.Length = 0;
		foreach (RoomType item in list)
		{
			b.Append(item.ToString());
			b.Append(" : ");
		}
		return b.ToString();
	}

	public void AddSoupMachine(DropSoup soupMachine)
	{
		_soupMachines.Add(soupMachine);
	}

	internal void RestoreItems(List<TutorialPickUp> pickUps)
	{
		for (int i = 0; i < _soupMachines.Count; i++)
		{
			_soupMachines[i].ResetDroppedSoups();
		}
		foreach (TutorialPickUp pickUp in pickUps)
		{
			pickUp.SpawnPoint.Restore(pickUp.Item);
		}
	}

	private string RoomShapeListPrint(List<RoomShape> list)
	{
		if (b == null)
		{
			b = new StringBuilder();
		}
		b.Length = 0;
		foreach (RoomShape item in list)
		{
			b.Append(item.ToString());
			b.Append(" : ");
		}
		return b.ToString();
	}

	[ContextMenu("PrintShips")]
	public void PrintShips()
	{
		StringBuilder stringBuilder = new StringBuilder();
		List<RoomShape> list = new List<RoomShape>
		{
			RoomShape.RectangleLeft,
			RoomShape.CutSmallLeft,
			RoomShape.CutLargeLeft,
			RoomShape.VerticalCorridor,
			RoomShape.RectangleRight,
			RoomShape.CutSmallRight,
			RoomShape.CutLargeRight
		};
		stringBuilder.AppendFormat("Shapes {0} \n", RoomShapeListPrint(list));
		List<RoomType> list2 = new List<RoomType>
		{
			RoomType.Bathroom,
			RoomType.Cafeteria,
			RoomType.CommandCentre,
			RoomType.EngineeringRoom,
			RoomType.ScienceRoom,
			RoomType.SleepingQuaters
		};
		RoomShape item = list.Find((RoomShape m) => m == RoomShape.VerticalCorridor);
		list.Remove(item);
		int num = factorial(list2.Count);
		int num2 = 0;
		for (int num3 = 0; num3 < num; num3++)
		{
			num2++;
			List<RoomType> list3 = new List<RoomType>(list2);
			int num4 = num3;
			int count = list3.Count;
			list3 = perm(list3, num4, count);
			if (!RoomsDatabase.CanGenerate(list, list3))
			{
				stringBuilder.AppendFormat("Seed {0} : {1} (Cant do) \n", num4, RoomTypesListPrint(list3));
			}
			else
			{
				stringBuilder.AppendFormat("Seed {0} : {1} \n", num4, RoomTypesListPrint(list3));
			}
			if (num2 > 100)
			{
				num2 = 0;
				UnityEngine.Debug.Log(stringBuilder.ToString());
				stringBuilder.Length = 0;
			}
		}
		UnityEngine.Debug.Log(stringBuilder.ToString());
	}

	[ContextMenu("Generate")]
	public void EditorGenerateShip()
	{
		Unload();
		StartCoroutine(GenerateShip());
		stillLoading.Clear();
	}

	[ContextMenu("Unload")]
	public void Unload()
	{
	}

	public IEnumerator GenerateShip()
	{
		roomsWithCharacters.Clear();
		Stopwatch sw = new Stopwatch();
		sw.Start();
		RuntimeSeed = Seed;
		List<ShipModule> modules = new List<ShipModule>();
		yield return StartCoroutine(LoadModules(modules));
		shipModules = new List<ShipModule>(modules);
		yield return StartCoroutine(LoadRooms(modules));
		yield return null;
		foreach (ShipModule shipModule in shipModules)
		{
			shipModule.SetOutsideMaterial(OutsideMaterial);
		}
		sw.Stop();
		UnityEngine.Debug.LogFormat("Ship generation: {0} ms", sw.ElapsedMilliseconds);
	}

	public IEnumerator GenerateTutorialShip()
	{
		Stopwatch sw = new Stopwatch();
		sw.Start();
		yield return StartCoroutine(LoadTutorial());
		RuntimeSeed = _tutorialDriver.BaseSeed;
		List<ShipModule> modules = new List<ShipModule>();
		yield return StartCoroutine(LoadModules(modules));
		shipModules = new List<ShipModule>(modules);
		yield return StartCoroutine(LoadRooms(modules));
		yield return null;
		foreach (ShipModule shipModule in shipModules)
		{
			shipModule.SetOutsideMaterial(OutsideMaterial);
		}
		sw.Stop();
		UnityEngine.Debug.LogFormat("Ship generation: {0} ms", sw.ElapsedMilliseconds);
	}

	internal ScavengeTutorialDriver GetTutorialDriver()
	{
		return _tutorialDriver;
	}

	[ContextMenu("Set door state as functional")]
	public void DoorsSetToFunctional()
	{
		foreach (ShipModule shipModule in shipModules)
		{
			shipModule.ResetDoors();
		}
	}

	internal RescurePodEntrance GetRescurePodEntrance()
	{
		return _rescurePodEntrance;
	}

	[ContextMenu("Randomly close doors")]
	public void CloseDors()
	{
		foreach (ShipModule shipModule in shipModules)
		{
			shipModule.RandomlyCloseDoors();
		}
	}

	private IEnumerator LoadTutorial()
	{
		ModuleLoader loader = new ModuleLoader(TutorialAddon.SceneName);
		StartCoroutine(loader.Load(Pivot, null));
		while (!loader.IsLoaded)
		{
			yield return null;
		}
		Scenes.Add(loader.Scene);
		_tutorialDriver = (ScavengeTutorialDriver)loader.Module;
	}

	private IEnumerator LoadModules(List<ShipModule> output)
	{
		SceneField sceneField = ShipModulesDatabase.Left.RandomElement(RuntimeSeed.LeftModuleSeed, out _runtimeLeftModuleSeed);
		SceneField sceneField2 = ShipModulesDatabase.Right.RandomElement(RuntimeSeed.RightModuleSeed, out _runtimeRightModuleSeed);
		ModuleLoader leftLoader = new ModuleLoader(sceneField.SceneName);
		StartCoroutine(leftLoader.Load(Pivot, null));
		ModuleLoader rightLoader = new ModuleLoader(sceneField2.SceneName);
		StartCoroutine(rightLoader.Load(Pivot, null));
		while (!leftLoader.IsLoaded || !rightLoader.IsLoaded)
		{
			yield return null;
		}
		Scenes.Add(leftLoader.Scene);
		Scenes.Add(rightLoader.Scene);
		output.Add((ShipModule)leftLoader.Module);
		output.Add((ShipModule)rightLoader.Module);
		RescurePodEntrance componentInChildren = leftLoader.Module.GetComponentInChildren<RescurePodEntrance>();
		if (componentInChildren == null)
		{
			componentInChildren = rightLoader.Module.GetComponentInChildren<RescurePodEntrance>();
		}
		_rescurePodEntrance = componentInChildren;
	}

	public List<RoomType> perm(List<RoomType> sequence, int index, int b)
	{
		sequence = new List<RoomType>(sequence);
		List<RoomType> list = new List<RoomType>();
		for (int i = 0; i < b; i++)
		{
			int index2 = index % sequence.Count;
			index /= sequence.Count;
			list.Add(sequence[index2]);
			sequence[index2] = sequence[sequence.Count - 1];
			sequence.RemoveAt(sequence.Count - 1);
		}
		if (list.Count < b)
		{
			throw new NotImplementedException();
		}
		return list;
	}

	private int factorial(int n)
	{
		if (n == 0)
		{
			return 1;
		}
		return n * factorial(n - 1);
	}

	private IEnumerator LoadRooms(List<ShipModule> input)
	{
		List<RoomSlot> list = new List<RoomSlot>();
		List<RoomType> list2 = new List<RoomType>
		{
			RoomType.Bathroom,
			RoomType.Cafeteria,
			RoomType.CommandCentre,
			RoomType.EngineeringRoom,
			RoomType.ScienceRoom,
			RoomType.SleepingQuaters
		};
		foreach (ShipModule item3 in input)
		{
			list.AddRange(item3.RoomSlots);
		}
		RoomSlot roomSlot = list.Find((RoomSlot m) => m.RoomShape == RoomShape.VerticalCorridor);
		list.Remove(roomSlot);
		_ = RuntimeSeed.RoomSeed;
		if (RuntimeSeed.RoomSeed >= 0)
		{
			int count = list2.Count;
			list2 = perm(list2, RuntimeSeed.RoomSeed, count);
			if (!RoomsDatabase.CanGenerate(list, list2))
			{
				UnityEngine.Debug.LogErrorFormat("Can not generate ship for this seed. Seed {0} : Rooms {1} RoomTypes : {2}", RuntimeSeed.RoomSeed, RoomTypesListPrint(list2), count);
				yield break;
			}
			UnityEngine.Debug.LogFormat("Generating Seed {0} : Rooms {1} RoomTypes : {2}", RuntimeSeed.RoomSeed, RoomTypesListPrint(list2), count);
		}
		else
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			int num = factorial(list2.Count);
			List<int> list3 = new List<int>(num);
			for (int num2 = 0; num2 < num; num2++)
			{
				list3.Add(num2);
			}
			int index = UnityEngine.Random.Range(0, list3.Count);
			int num3 = list3[index];
			list3.RemoveAt(index);
			int count2 = list2.Count;
			List<RoomType> list4 = perm(list2, num3, count2);
			bool flag = RoomsDatabase.CanGenerate(list, list4);
			int num4 = 1;
			while (!flag && list3.Count > 0)
			{
				num4++;
				index = UnityEngine.Random.Range(0, list3.Count);
				num3 = list3[index];
				list3.RemoveAt(index);
				list4 = perm(list2, num3, count2);
				flag = RoomsDatabase.CanGenerate(list, list4);
			}
			if (!flag)
			{
				UnityEngine.Debug.LogFormat("No legal seed found in {0} tries", num);
				yield break;
			}
			list2 = list4;
			stopwatch.Stop();
			UnityEngine.Debug.LogFormat("Seed: {0}. Tries: {1}. Elapsed: {2}ms", num3, num4, stopwatch.ElapsedMilliseconds);
		}
		_roomList = new Dictionary<Room, RoomType>();
		RandomRoomData room = RoomsDatabase.GetRoom(roomSlot.RoomShape, RoomType.Corridor, verbose: true, RuntimeSeed.CorridorSeed);
		ModuleLoader item = LoadRoom(room.RoomData, roomSlot, RoomType.Corridor, OnModuleLoaded);
		stillLoading.Add(item);
		foreach (RoomType item4 in list2)
		{
			RoomSlot roomSlot2 = list[0];
			RandomRoomData randomRoomData;
			if (_tutorialDriver != null)
			{
				switch (item4)
				{
				case RoomType.SleepingQuaters:
					randomRoomData = new RandomRoomData();
					randomRoomData.RoomData = _tutorialDriver.TutorialSleepingQuater;
					break;
				case RoomType.Bathroom:
					randomRoomData = new RandomRoomData();
					randomRoomData.RoomData = _tutorialDriver.TutorialBathroom;
					break;
				case RoomType.EngineeringRoom:
					randomRoomData = new RandomRoomData();
					randomRoomData.RoomData = _tutorialDriver.TutorialEngineering;
					break;
				default:
					randomRoomData = RoomsDatabase.GetRoom(roomSlot2.RoomShape, item4);
					break;
				}
			}
			else
			{
				randomRoomData = RoomsDatabase.GetRoom(roomSlot2.RoomShape, item4);
			}
			UnityEngine.Debug.LogFormat("Loading Shape {0} : Rooms {1}", roomSlot2.RoomShape, item4);
			list.Remove(roomSlot2);
			ModuleLoader item2 = LoadRoom(randomRoomData.RoomData, roomSlot2, item4, OnModuleLoaded);
			stillLoading.Add(item2);
		}
		yield return null;
		while (stillLoading.Count > 0)
		{
			yield return null;
		}
		for (int i = 0; i < 10; i++)
		{
			yield return null;
		}
		foreach (KeyValuePair<Room, RoomType> room2 in _roomList)
		{
			foreach (ItemSpawnPoint spawnPoint in room2.Key.SpawnPoints)
			{
				spawnPoint.SetRoomType(room2.Value);
				_spawnPoints.Add(spawnPoint);
			}
		}
		itemsDistribute.AddSpawnPoints(_spawnPoints);
		if (RuntimeSeed.ItemSeed == -1)
		{
			_runtimeItemSeed = UnityEngine.Random.Range(0, 100000);
		}
		else
		{
			_runtimeItemSeed = RuntimeSeed.ItemSeed;
		}
		yield return StartCoroutine(itemsDistribute.SpawnItems(_runtimeItemSeed, _tutorialDriver));
		if (_tutorialDriver != null)
		{
			foreach (PriorityItemSpawnPoint spawnPoint2 in _tutorialDriver.SpawnPoints)
			{
				_spawnPoints.Add(spawnPoint2);
			}
		}
		yield return null;
	}

	private void itemStateSetter(int state)
	{
		_runtimeItemSeed = state;
	}

	private void OnModuleLoaded(ModuleLoader loader, Module module, RoomType type, RoomSlot slot)
	{
		Scenes.Add(loader.Scene);
		stillLoading.Remove(loader);
		Room room = module as Room;
		if (room != null)
		{
			_roomList.Add(room, type);
		}
		if (type != RoomType.Corridor)
		{
			slot.SetInsideMaterial(RoomMaterialsDatabase.GetMaterial(type), RoomTransparentMaterialsDatabase.GetMaterial(type));
			slot.SetFloorMaterial(RoomFloorMaterialsDatabase.GetMaterial(type));
		}
	}

	private ModuleLoader LoadRoom(RoomData roomData, RoomSlot slot, RoomType type, Action<ModuleLoader, Module, RoomType, RoomSlot> callback)
	{
		ModuleLoader moduleLoader = new ModuleLoader(roomData.Scene.SceneName);
		StartCoroutine(moduleLoader.Load(slot.transform.position, (ModuleLoader ml, Module m) =>
		{
			if (callback != null)
			{
				callback(ml, m, type, slot);
			}
		}));
		return moduleLoader;
	}

	public List<Scene> GetScavangeScenes()
	{
		return new List<Scene>(Scenes) { gameObject.scene };
	}

	internal void DisableItems()
	{
		foreach (ItemSpawnPoint spawnPoint in _spawnPoints)
		{
			ScavengeItem item = spawnPoint.Item;
			if (!(item == null))
			{
				item.CanBeCollected = false;
				PlayerController playerController = ScavengePlayersManager.Instance.PlayerController;
				if (playerController != null)
				{
					playerController.RemoveSpawnPointFromRange(spawnPoint);
				}
			}
		}
	}

	internal void EnableItems(List<ScavangeItemData> items)
	{
		foreach (ItemSpawnPoint spawnPoint in _spawnPoints)
		{
			ScavengeItem item = spawnPoint.Item;
			if (item == null)
			{
				continue;
			}
			if (items.Contains(item.ScavengeItemData))
			{
				item.CanBeCollected = true;
				spawnPoint.CheckIfInPlayerRange();
				continue;
			}
			item.CanBeCollected = false;
			PlayerController playerController = ScavengePlayersManager.Instance.PlayerController;
			if (playerController != null)
			{
				playerController.RemoveSpawnPointFromRange(spawnPoint);
			}
		}
	}

	internal void EnableItems()
	{
		foreach (ItemSpawnPoint spawnPoint in _spawnPoints)
		{
			ScavengeItem item = spawnPoint.Item;
			if (!(item == null))
			{
				item.CanBeCollected = true;
				spawnPoint.CheckIfInPlayerRange();
			}
		}
	}

	internal void RegisterCorridorWall(BottomCorridorWall bottomCorridorWall)
	{
		_bottomCorridorWalls.Add(bottomCorridorWall);
		bottomCorridorWall.OnTransparent.Add(OnBottomWallTransparent);
		bottomCorridorWall.RemoveSides();
	}

	private void OnBottomWallTransparent(Wall wall)
	{
		foreach (BottomCorridorWall bottomCorridorWall in _bottomCorridorWalls)
		{
			if (bottomCorridorWall != wall && bottomCorridorWall.gameObject.activeInHierarchy)
			{
				bottomCorridorWall.Transparency.SetTransparent(events: false);
			}
		}
	}
}
