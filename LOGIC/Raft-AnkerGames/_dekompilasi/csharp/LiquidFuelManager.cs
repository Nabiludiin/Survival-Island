using System.Collections.Generic;
using UnityEngine;

public class LiquidFuelManager : MonoBehaviour
{
	private static Dictionary<FuelValue_FiltrationType, List<SO_FuelValue>> fuelValueDictionary = new Dictionary<FuelValue_FiltrationType, List<SO_FuelValue>>();

	private static bool initialized = false;

	private void Awake()
	{
		if (initialized)
		{
			Object.Destroy(gameObject);
			return;
		}
		initialized = true;
		DontDestroyOnLoadManager.AddGameObject(gameObject);
		SO_FuelValue[] array = Resources.LoadAll<SO_FuelValue>("FuelValueAssets");
		if (array == null)
		{
			return;
		}
		foreach (SO_FuelValue sO_FuelValue in array)
		{
			if (!(sO_FuelValue == null))
			{
				if (fuelValueDictionary.ContainsKey(sO_FuelValue.fuelFiltrationType))
				{
					fuelValueDictionary[sO_FuelValue.fuelFiltrationType].Add(sO_FuelValue);
					continue;
				}
				fuelValueDictionary.Add(sO_FuelValue.fuelFiltrationType, new List<SO_FuelValue> { sO_FuelValue });
			}
		}
	}

	public static SO_FuelValue GetFilteredFuelValueSoFromItem(Item_Base item, FuelValue_FiltrationType fVFilterType)
	{
		List<SO_FuelValue> list = fuelValueDictionary[fVFilterType];
		if (list != null)
		{
			foreach (SO_FuelValue item2 in list)
			{
				if (!(item2 == null) && item2.itemType == item)
				{
					return item2;
				}
			}
		}
		return null;
	}
}
