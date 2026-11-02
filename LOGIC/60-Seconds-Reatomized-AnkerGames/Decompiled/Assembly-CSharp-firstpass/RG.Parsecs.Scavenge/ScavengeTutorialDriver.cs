using System.Collections;
using System.Collections.Generic;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ScavengeTutorialDriver : Module
{
	public List<ScavengeTutorialTask> Tasks = new List<ScavengeTutorialTask>();

	public List<ScavengeTutorialTask> FailFlowTasks = new List<ScavengeTutorialTask>();

	public BaseSeed BaseSeed;

	public Transform SpawnPoint;

	public RoomData TutorialSleepingQuater;

	public RoomData TutorialBathroom;

	public RoomData TutorialEngineering;

	public List<PriorityItemSpawnPoint> SpawnPoints;

	public float DelayAfterTasks = 3f;

	public HintAnnouncer HintAnnouncer;

	private Queue<ScavengeTutorialTask> _tasks;

	private ScavengeTutorialTask _currentTask;

	private bool _finished;

	private ScavengeManager _manager;

	[SerializeField]
	private PlayerResources _playerResources;

	private List<TutorialPickUp> _pickUps = new List<TutorialPickUp>();

	private void NextTask()
	{
		if (_tasks.Count == 0)
		{
			Finished();
			return;
		}
		_currentTask = _tasks.Dequeue();
		_currentTask.OnSuccess.AddOneTime(NextTask);
		_currentTask.TaskStart(_manager, HintAnnouncer);
	}

	private void Finished()
	{
		_finished = true;
	}

	public void InitTutorial(ScavengeManager manager)
	{
		_manager = manager;
	}

	public IEnumerator TutorialExploration()
	{
		_manager.ShipGenerator.DisableItems();
		_tasks = new Queue<ScavengeTutorialTask>(Tasks);
		NextTask();
		_finished = false;
		while (!_finished)
		{
			if (_currentTask != null)
			{
				_currentTask.TaskUpdate();
			}
			yield return null;
		}
		_manager.ShipGenerator.DisableItems();
		yield return new WaitForSeconds(DelayAfterTasks);
		_manager.ShipGenerator.EnableItems();
		_manager.ScavengePlayersManager.PlayerController.OnPickUp.Add(OnPickUp);
	}

	private void OnPickUp(ScavangeItemData data, ItemSpawnPoint spawnPoint)
	{
		_pickUps.Add(new TutorialPickUp
		{
			Item = data,
			SpawnPoint = spawnPoint
		});
	}

	public IEnumerator TutorialFailedExploration()
	{
		_manager.ShipGenerator.DisableItems();
		_tasks = new Queue<ScavengeTutorialTask>(FailFlowTasks);
		NextTask();
		_finished = false;
		while (!_finished)
		{
			if (_currentTask != null)
			{
				_currentTask.TaskUpdate();
			}
			yield return null;
		}
		_manager.ScavengePlayersManager.PlayerController.ClearHandForced();
		_manager.ShipGenerator.RestoreItems(_pickUps);
		_manager.CrewList.RefreshCrewList();
		_pickUps.Clear();
		_manager.ShipGenerator.DisableItems();
		yield return new WaitForSeconds(DelayAfterTasks);
		_manager.ShipGenerator.EnableItems();
		_playerResources.ClearResources();
	}

	public override void Load()
	{
	}

	public override void Unload()
	{
	}

	private void OnDestroy()
	{
		_manager = null;
	}
}
