using System;
using I2.Loc;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class ExpeditionDestinationStaticData
{
	[SerializeField]
	[Tooltip("Unique id.")]
	private string _id;

	[SerializeField]
	[Tooltip("Name of the destination.")]
	private LocalizedString _name;

	[SerializeField]
	[Tooltip("Description of the destination.")]
	private LocalizedString _description;

	[SerializeField]
	[Tooltip("Items that have a chance to be found during this expedition.")]
	private IItem[] _itemsToFind;

	[SerializeField]
	[Tooltip("Resources that have a chance to be found during this expedition.")]
	private Resource[] _resourcesToFind;

	[SerializeField]
	[Tooltip("Threats that have a chance to be found during this expedition.")]
	private ExpeditionThreat[] _threats;

	[SerializeField]
	[Tooltip("How many clocks should be displayed when this point is chosen as a destination.")]
	private int _amountOfClocks;

	[SerializeField]
	[Tooltip("This Sprite will be used for displaying image in UI for this destination in summary part.")]
	private Sprite _destinationSprite;

	[SerializeField]
	[Tooltip("Event that will be invoked when Player sends the Expedition to this point.")]
	private ExpeditionEvent _event;

	[SerializeField]
	[Tooltip("Should this point be enabled on start. (Overrides RequiredSystemLevel)")]
	private bool _enabled;

	[SerializeField]
	[Tooltip("Minimum level of the Expedition System that is required to have this destination enabled.")]
	private int _requiredSystemLevel;

	[SerializeField]
	[Tooltip("Was this point already visited?")]
	private bool _visited;

	public string Id => _id;

	public LocalizedString Name => _name;

	public LocalizedString Description => _description;

	public bool Enabled => _enabled;

	public ExpeditionEvent Event => _event;

	public int RequiredSystemLevel => _requiredSystemLevel;

	public Sprite DestinationSprite => _destinationSprite;

	public Resource[] ResourcesToFind => _resourcesToFind;

	public IItem[] ItemsToFind => _itemsToFind;

	public ExpeditionThreat[] Threats => _threats;

	public int AmountOfClocks => _amountOfClocks;

	public bool Visited => _visited;
}
