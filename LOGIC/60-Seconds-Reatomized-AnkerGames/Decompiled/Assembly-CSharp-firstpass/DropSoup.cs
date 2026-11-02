using System.Collections.Generic;
using RG.Parsecs.Scavenge;
using UnityEngine;

public class DropSoup : MonoBehaviour
{
	[SerializeField]
	private ItemSpawnPoint _itemSpawnPoint;

	[SerializeField]
	private ScavangeItemData _soup;

	[SerializeField]
	private int _maxSoupsToDrop;

	private int _soupDropped;

	private ItemSpawnPoint _insideSoup;

	[Tooltip("The offset for dropped soup regards to soup machine")]
	[SerializeField]
	private Vector3 _offset = new Vector3(0.225f, 0.75f, 0f);

	private List<GameObject> _droppedSoups;

	private void Start()
	{
		_insideSoup = gameObject.GetComponentInChildren<ItemSpawnPoint>();
		_droppedSoups = new List<GameObject>();
		ShipGenerator.I.AddSoupMachine(this);
	}

	public void ResetDroppedSoups()
	{
		for (int i = 0; i < _droppedSoups.Count; i++)
		{
			Object.Destroy(_droppedSoups[i].gameObject);
		}
		_droppedSoups.Clear();
		_soupDropped = 0;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.GetComponent<PlayerController>() != null && (!(_insideSoup != null) || !_insideSoup.IsItemAvailable) && _soupDropped < _maxSoupsToDrop)
		{
			ItemSpawnPoint itemSpawnPoint = Object.Instantiate(_itemSpawnPoint, transform.position + _offset, transform.rotation);
			itemSpawnPoint.AssignItem(_soup, new Vector3(0f, 0f, 90f));
			_soupDropped++;
			_droppedSoups.Add(itemSpawnPoint.transform.parent.gameObject);
		}
	}
}
