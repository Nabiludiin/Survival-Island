using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ItemSpawnPoint : MonoBehaviour
{
	public ESpawnPointType SpawnType;

	public SphereCollider Collider;

	private RoomType roomType = RoomType.Bathroom;

	private ScavengeItem _currentItem;

	private ScavengeItem _exItem;

	private Collider[] _overlaps = new Collider[1];

	private static readonly int CharacterMask = 1024;

	public ScavengeItem Item => _currentItem;

	public bool IsItemAvailable
	{
		get
		{
			if (_currentItem != null)
			{
				return _currentItem.CanBeCollected;
			}
			return false;
		}
	}

	protected virtual IEnumerator Start()
	{
		yield return null;
		GetComponentInParent<Room>().RegisterSpawnPoint(this);
		SpriteRenderer component = GetComponent<SpriteRenderer>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	public void SetRoomType(RoomType roomType)
	{
		this.roomType = roomType;
	}

	public RoomType GetRoomType()
	{
		return roomType;
	}

	public void AssignItem(ScavangeItemData scavangeItem)
	{
		GameObject gameObject = Object.Instantiate(scavangeItem.GetItemRepresentation(SpawnType));
		if (SpawnType == ESpawnPointType.Standard)
		{
			gameObject.transform.SetParent(null);
		}
		else
		{
			gameObject.transform.SetParent(transform.parent);
		}
		gameObject.transform.position = transform.position;
		transform.SetParent(gameObject.transform);
		_currentItem = gameObject.GetComponent<ScavengeItem>();
		_exItem = _currentItem;
		_currentItem.ScavengeItemData = scavangeItem;
		if (scavangeItem.survivalItemRef is ScavengeCharacterItem)
		{
			ShipGenerator.I.roomsWithCharacters.Add(roomType);
		}
	}

	public void AssignItem(ScavangeItemData scavangeItem, Vector3 rotation)
	{
		GameObject itemRepresentation = scavangeItem.GetItemRepresentation(SpawnType);
		GameObject gameObject = ((SpawnType != ESpawnPointType.Standard) ? Object.Instantiate(itemRepresentation, transform.position, Quaternion.identity, transform.parent) : Object.Instantiate(itemRepresentation, transform.position, Quaternion.identity, null));
		gameObject.transform.Rotate(rotation);
		transform.SetParent(gameObject.transform);
		_currentItem = gameObject.GetComponent<ScavengeItem>();
		_exItem = _currentItem;
		_currentItem.ScavengeItemData = scavangeItem;
		if (scavangeItem.survivalItemRef is ScavengeCharacterItem)
		{
			ShipGenerator.I.roomsWithCharacters.Add(roomType);
		}
	}

	private void OnTriggerEnter(Collider player)
	{
		if (IsItemAvailable)
		{
			ScavengePlayersManager.Instance.PlayerController.AddSpawnPointInRange(this);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (IsItemAvailable)
		{
			ScavengePlayersManager.Instance.PlayerController.RemoveSpawnPointFromRange(this);
		}
	}

	public void HighLight(int freeHands)
	{
		if (_currentItem != null)
		{
			_currentItem.Highlight(freeHands);
		}
	}

	public void DeHighLight()
	{
		if (_currentItem != null)
		{
			_currentItem.DeHighLight();
		}
	}

	internal void Restore(ScavangeItemData item)
	{
		if (_exItem.ScavengeItemData != item)
		{
			Debug.LogErrorFormat("Restoring item into wrong spawn point {0} : {1} : {2}", _exItem.ScavengeItemData.itemName, item.itemName, gameObject.name);
		}
		else
		{
			_currentItem = _exItem;
			_currentItem.Restore();
			Item.ScavengeItemData.survivalItemRef.Remove();
		}
	}

	public void Collect(float pickUpDelay)
	{
		Item.ScavengeItemData.survivalItemRef.Add();
		ScavengePlayersManager.Instance.PlayerController.RemoveSpawnPointFromRange(this);
		Item.Collect(pickUpDelay);
		_currentItem = null;
	}

	public void OnDrawGizmos()
	{
	}

	internal void CheckIfInPlayerRange()
	{
		if (!IsItemAvailable)
		{
			return;
		}
		if (Collider == null)
		{
			Collider = GetComponent<SphereCollider>();
			if (Collider == null)
			{
				Debug.LogErrorFormat("ItemSpawnPoint SphereCollider is not assigned {0}", transform.name);
				return;
			}
		}
		if (Physics.OverlapSphereNonAlloc(Collider.center + transform.position, Collider.radius, _overlaps, CharacterMask) > 0)
		{
			ScavengePlayersManager.Instance.PlayerController.AddSpawnPointInRange(this);
		}
	}
}
