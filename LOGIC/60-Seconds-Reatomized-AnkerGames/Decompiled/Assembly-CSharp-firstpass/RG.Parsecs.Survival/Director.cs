using RG.Core.Base;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class Director : RGMonoBehaviourSingleton<Director>
{
	[SerializeField]
	private GlobalIntVariable _currentMainStoryActVariable;

	[SerializeField]
	private GlobalIntVariable _currentMainStoryStageVariable;
}
