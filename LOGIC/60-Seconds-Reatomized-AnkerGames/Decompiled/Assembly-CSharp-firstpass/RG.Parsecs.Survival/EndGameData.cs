using RG.Core.Base;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/End Game/End Game Data")]
public class EndGameData : RGScriptableObject
{
	[SerializeField]
	private EndGameRuntimeData _runtimeData;

	public EndGameRuntimeData RuntimeData => _runtimeData;

	public void Reset()
	{
		RuntimeData.Reset();
	}
}
