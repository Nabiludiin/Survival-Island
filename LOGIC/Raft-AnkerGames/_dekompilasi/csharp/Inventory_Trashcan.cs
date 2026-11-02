using FMODUnity;

public class Inventory_Trashcan : Inventory
{
	public EventReference er_useTrashCan;

	public void Button_EmptyTrash()
	{
		bool flag = false;
		foreach (Slot allSlot in allSlots)
		{
			if (allSlot != null && allSlot.HasValidItemInstance())
			{
				if (!flag && allSlot.HasValidItemInstance())
				{
					flag = true;
				}
				allSlot.SetItem(null);
			}
		}
		if (flag)
		{
			RuntimeManager.PlayOneShot(er_useTrashCan);
		}
	}
}
