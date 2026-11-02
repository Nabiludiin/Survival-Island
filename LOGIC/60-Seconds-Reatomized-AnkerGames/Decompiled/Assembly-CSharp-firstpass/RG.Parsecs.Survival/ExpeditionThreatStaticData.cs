using System;
using I2.Loc;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class ExpeditionThreatStaticData
{
	[SerializeField]
	private string _id;

	[SerializeField]
	[Tooltip("Icon of the threat.")]
	private Sprite _icon;

	[SerializeField]
	[Tooltip("Term for name of the threat.")]
	private LocalizedString _name;

	[SerializeField]
	[Tooltip("Term for description of the threat.")]
	private LocalizedString _description;

	[SerializeField]
	[Tooltip("Is threat enabled at the beggining.")]
	private bool _enabled = true;

	public Sprite Icon => _icon;

	public LocalizedString Name => _name;

	public LocalizedString Description => _description;

	public bool Enabled => _enabled;

	public string Id => _id;
}
