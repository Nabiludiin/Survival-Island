using UnityEngine;

namespace RG.Parsecs.Survival;

[RequireComponent(typeof(ValueIconController))]
public class NeedController : MonoBehaviour
{
	[SerializeField]
	private CharacterNeed _characterNeed;

	[SerializeField]
	private RationingView _rationingView;

	[SerializeField]
	private ValueIconController _valueIconController;

	public void OnShow()
	{
		_valueIconController.SetValue(_rationingView.GetNeedValueForCharacter(_characterNeed));
	}
}
