using UnityEngine;

public class NoteBookNotePickup : PickupItem
{
	public int noteIndex;

	[HideInInspector]
	public NoteBook noteBook;

	public override void Start()
	{
		base.Start();
		noteBook = ComponentManager<NoteBook>.Value;
	}
}
