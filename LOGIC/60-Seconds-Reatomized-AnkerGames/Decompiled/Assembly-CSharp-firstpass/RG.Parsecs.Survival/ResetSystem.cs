using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class ResetSystem : MonoBehaviour
{
	[SerializeField]
	private SaveEvent _resetSaveEvent;

	private void Start()
	{
		_resetSaveEvent.ResetGame();
	}
}
