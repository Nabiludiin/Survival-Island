using UnityEngine;

public class LandmarkItem_CharacterUnlock : LandmarkItem
{
	[SerializeField]
	private CharacterUnlock characterUnlock;

	private bool unlocked;

	public override RGD_LandmarkItem GetSave()
	{
		return new RGD_LandmarkItem_CharacterUnlock(this, characterUnlock);
	}

	public void Restore(RGD_LandmarkItem_CharacterUnlock rgd)
	{
		characterUnlock.Restore(rgd);
	}
}
