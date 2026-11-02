using UnityEngine;

namespace RG.Parsecs.Survival;

public class SpaceshipLeftShelfController : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Closed shelves objects.")]
	private GameObject[] _statesSprites;

	private void Awake()
	{
		SetInitialStateOfShelves();
	}

	private void SetInitialStateOfShelves()
	{
		SetShelf(0, closed: false);
		SetShelf(1, closed: false);
		SetShelf(2, closed: false);
		SetShelf(3, closed: false);
	}

	public void SetShelf(int id, bool closed)
	{
		_statesSprites[id].SetActive(closed);
	}
}
