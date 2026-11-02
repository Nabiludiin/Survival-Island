using UnityEngine;

namespace RG.Parsecs.Survival;

public class EquipmentPanelController : MonoBehaviour
{
	[SerializeField]
	[Header("Data")]
	[Tooltip("Reference to Suit System data scriptable object.")]
	private SuitData _suitData;

	[SerializeField]
	[Header("References")]
	[Tooltip("Reference to select item slots.")]
	private ItemSlotController[] _itemSlotControllers;

	public void Initialize()
	{
		for (int i = 0; i < _itemSlotControllers.Length; i++)
		{
			_itemSlotControllers[i].SetLocked(_itemSlotControllers[i].RequiredSuitLevel > _suitData.SuitLevel);
		}
	}

	public void ResetEquipment()
	{
		for (int i = 0; i < _itemSlotControllers.Length; i++)
		{
			_itemSlotControllers[i].ResetSelectItem();
		}
	}
}
