using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
[CreateAssetMenu(fileName = "New Planet Deck", menuName = "60 Parsecs!/Event Editor/Planets/New Planet Deck")]
public class PlanetDeck : Deck, ILocalizationParamsManager
{
	private const string PLANET_NAME_I2LOC_PARAM = "PLANET_NAME";

	[SerializeField]
	[Tooltip("Unique id.")]
	private string _id;

	[SerializeField]
	[Tooltip("Index used for landing animation")]
	private int _planetIndex;

	[SerializeField]
	private float _landingFmodParameter;

	[SerializeField]
	[Tooltip("Name of the planet.")]
	private LocalizedString _planetName;

	[SerializeField]
	[Tooltip("Description of the planet.")]
	private LocalizedString _planetDescription;

	[SerializeField]
	[Tooltip("Prefab that will be spawned when we'll land on the planet.")]
	private MapController _mapPrefab;

	public LocalizedString PlanetName => _planetName;

	public string Id => _id;

	public LocalizedString PlanetDescription => _planetDescription;

	public MapController MapPrefab => _mapPrefab;

	public float LandingFmodParameter => _landingFmodParameter;

	public List<ExpeditionDestination> Destinations
	{
		get
		{
			List<ExpeditionDestination> list = new List<ExpeditionDestination>();
			for (int i = 0; i < _mapPrefab.MapPoints.Count; i++)
			{
				list.Add(_mapPrefab.MapPoints[i].Destination);
			}
			return list;
		}
	}

	public int PlanetIndex => _planetIndex;

	public void RegisterInI2Loc()
	{
		if (!LocalizationManager.ParamManagers.Contains(this))
		{
			LocalizationManager.ParamManagers.Add(this);
			LocalizationManager.LocalizeAll(Force: true);
		}
	}

	public void UnregisterFromI2Loc()
	{
		LocalizationManager.ParamManagers.Remove(this);
	}

	public string GetParameterValue(string param)
	{
		if (param == "PLANET_NAME")
		{
			return _planetName;
		}
		return null;
	}

	public override void SetActive(bool activate)
	{
		if (activate && CurrentState != DeckState.ACTIVATED)
		{
			RegisterInI2Loc();
		}
		else if (!activate && CurrentState != DeckState.DEACTIVATED)
		{
			UnregisterFromI2Loc();
		}
		base.SetActive(activate);
	}
}
