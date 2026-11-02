namespace RG.Parsecs.Scavenge;

public enum ActorEvents
{
	None,
	PickUpTry,
	PickUpSuccess,
	PickUpFail_NoItemAvailable,
	PickUpFail_NotEnoughHands,
	DropItemsToPod,
	AboutToDropItemsToPod,
	RescurePodEntrance_OnTriggerEnter,
	RescurePodEntrance_OnTriggerExit,
	StartedTheHunt,
	HandFull,
	FurnitureCollision,
	Crew_NoSpaceLeft
}
