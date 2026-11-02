using UnityEngine;

namespace RG.Parsecs.Survival;

public class SuitSystemView : SystemView
{
	[SerializeField]
	private SuitData _suitData;

	public override bool Visible => _suitData.RuntimeData.IsActive;

	public override int SystemLevel => _suitData.RuntimeData.SuitLevel;

	public override int MaxSystemLevel => _suitData.StaticData.MaxLevel;

	public override Lamps.ELampState GetLampState => Lamps.ELampState.NONE;
}
