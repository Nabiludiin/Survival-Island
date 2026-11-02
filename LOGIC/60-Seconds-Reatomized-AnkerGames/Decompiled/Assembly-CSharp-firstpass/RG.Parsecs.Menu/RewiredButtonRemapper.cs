using System.Collections.Generic;
using Rewired;
using UnityEngine;

namespace RG.Parsecs.Menu;

public class RewiredButtonRemapper : MonoBehaviour
{
	public AssignableButtonPrefab buttonPrefab;

	public PoolButtonPanel poolButtonPanel;

	private void Start()
	{
		IEnumerable<ControllerMap> allMapsInCategory = ReInput.players.GetPlayer(0).controllers.maps.GetAllMapsInCategory(3);
		ControllerMap map = null;
		foreach (ControllerMap item in allMapsInCategory)
		{
			Debug.Log(item.id);
			if (item.enabled)
			{
				map = item;
			}
		}
		foreach (InputAction item2 in ReInput.mapping.UserAssignableActionsInCategory(1, sort: true))
		{
			if (item2.type == InputActionType.Button)
			{
				Object.Instantiate(buttonPrefab.gameObject, transform, worldPositionStays: false).GetComponent<AssignableButtonPrefab>().Init(item2, map, Pole.Positive, poolButtonPanel);
				continue;
			}
			Object.Instantiate(buttonPrefab.gameObject, transform, worldPositionStays: false).GetComponent<AssignableButtonPrefab>().Init(item2, map, Pole.Positive, poolButtonPanel);
			Object.Instantiate(buttonPrefab.gameObject, transform, worldPositionStays: false).GetComponent<AssignableButtonPrefab>().Init(item2, map, Pole.Negative, poolButtonPanel);
		}
	}
}
