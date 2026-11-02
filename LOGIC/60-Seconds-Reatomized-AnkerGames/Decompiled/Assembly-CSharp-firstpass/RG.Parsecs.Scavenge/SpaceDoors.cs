using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class SpaceDoors : MonoBehaviour
{
	public DoorOperationalState doorOperational;

	[SerializeField]
	private List<DoorTypeMaterial> materials;

	private Animator animator;

	public MeshRenderer doorA;

	public MeshRenderer doorB;

	public string DoorOpeningSoundPath = "event:/Scavenge/SFX/Rooms/Door";

	private string _doorOpeningClipName;

	private string _doorClosingClipName;

	private bool _areOpen;

	public float closedTime = 3f;

	public float openedTime = 3f;

	public SpriteRenderer[] lightsA;

	public SpriteRenderer[] lightsB;

	private List<GameObject> playerInRange = new List<GameObject>();

	public List<GameObject> blockersInDoors = new List<GameObject>();

	private void Start()
	{
		animator = GetComponent<Animator>();
		SetDoorState(doorOperational);
	}

	public void SetDoorState(DoorOperationalState state)
	{
		doorOperational = state;
		StopAllCoroutines();
		foreach (DoorTypeMaterial material in materials)
		{
			if (material.state == doorOperational)
			{
				doorA.material = material.materialA;
				doorB.material = material.materialB;
				_doorClosingClipName = material.DoorClosingClipName;
				_doorOpeningClipName = material.DoorOpeningClipName;
				SpriteRenderer[] array = lightsA;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].sprite = material.lights;
				}
				array = lightsB;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].sprite = material.lights;
				}
				break;
			}
		}
		if (state == DoorOperationalState.Damaged)
		{
			StartCoroutine(CycleDoorState());
		}
	}

	[ContextMenu("Change door to cycle")]
	private void ChangeDoorToCycle()
	{
		SetDoorState(DoorOperationalState.Damaged);
	}

	[ContextMenu("Change door to closed")]
	private void ChangeDoorToClose()
	{
		SetDoorState(DoorOperationalState.NonWorking);
	}

	[ContextMenu("Change door to open")]
	private void ChangeDoorToOpen()
	{
		SetDoorState(DoorOperationalState.Working);
	}

	private IEnumerator CycleDoorState()
	{
		while (true)
		{
			float singleLightTime = closedTime / (float)lightsA.Length;
			float timer = 0f;
			int lightsUp;
			for (lightsUp = 0; lightsUp < lightsA.Length; lightsUp++)
			{
				while (timer < singleLightTime)
				{
					timer += Time.deltaTime;
					yield return null;
				}
				lightsA[lightsUp].enabled = true;
				lightsB[lightsUp].enabled = true;
				timer = 0f;
			}
			SetDoorState(!_areOpen);
			singleLightTime = openedTime / (float)lightsA.Length;
			timer = 0f;
			lightsUp = lightsA.Length;
			while (lightsUp > 0)
			{
				while (timer < singleLightTime)
				{
					timer += Time.deltaTime;
					yield return null;
				}
				lightsUp--;
				lightsA[lightsUp].enabled = false;
				lightsB[lightsUp].enabled = false;
				timer = 0f;
			}
			while (blockersInDoors.Count != 0)
			{
				yield return null;
			}
			SetDoorState(!_areOpen);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (doorOperational == DoorOperationalState.Working)
		{
			playerInRange.Add(other.gameObject);
			if (!_areOpen)
			{
				SetDoorState(open: true);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (doorOperational == DoorOperationalState.Working)
		{
			playerInRange.Remove(other.gameObject);
			if (playerInRange.Count == 0 && blockersInDoors.Count == 0)
			{
				SetDoorState(open: false);
			}
		}
	}

	private void SetDoorState(bool open)
	{
		if (open)
		{
			if (!_areOpen && !string.IsNullOrEmpty(DoorOpeningSoundPath))
			{
				RuntimeManager.PlayOneShot(DoorOpeningSoundPath, transform.position);
			}
			animator.Play(_doorOpeningClipName);
		}
		else
		{
			animator.Play(_doorClosingClipName);
		}
		_areOpen = open;
	}
}
