using Rewired.Dev;

namespace RewiredConsts;

public static class Action
{
	[ActionIdFieldInfo(categoryName = "Default", friendlyName = "InitPlayer")]
	public const int InitPlayer = 0;

	[ActionIdFieldInfo(categoryName = "Default", friendlyName = "DeveloperMenu")]
	public const int DeveloperMenu = 10;

	[ActionIdFieldInfo(categoryName = "Default", friendlyName = "PauseMenu")]
	public const int PauseMenu = 12;

	[ActionIdFieldInfo(categoryName = "Scavenge", friendlyName = "Horizontal")]
	public const int Horizontal = 2;

	[ActionIdFieldInfo(categoryName = "Scavenge", friendlyName = "Vertical")]
	public const int Vertical = 3;

	[ActionIdFieldInfo(categoryName = "Scavenge", friendlyName = "UI Controls Grab-Drop")]
	public const int PickUp = 4;

	[ActionIdFieldInfo(categoryName = "Scavenge", friendlyName = "MauseControllerStartPoint")]
	public const int MauseControllerStartPoint = 13;

	[ActionIdFieldInfo(categoryName = "Scavenge", friendlyName = "MouseHorizontal")]
	public const int MouseHorizontal = 14;

	[ActionIdFieldInfo(categoryName = "Scavenge", friendlyName = "MouseVertical")]
	public const int MouseVertical = 15;

	[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UIHorizontal")]
	public const int UIHorizontal = 6;

	[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UIVertical")]
	public const int UIVertical = 7;

	[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UISubmit")]
	public const int UISubmit = 8;

	[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UICancel")]
	public const int UICancel = 9;

	[ActionIdFieldInfo(categoryName = "UI", friendlyName = "MouseX")]
	public const int MouseX = 16;

	[ActionIdFieldInfo(categoryName = "UI", friendlyName = "MouseY")]
	public const int MouseY = 17;

	[ActionIdFieldInfo(categoryName = "UI", friendlyName = "GamepadMouseClick")]
	public const int GamepadMouseClick = 19;

	[ActionIdFieldInfo(categoryName = "UI", friendlyName = "LB")]
	public const int LB = 20;

	[ActionIdFieldInfo(categoryName = "UI", friendlyName = "RB")]
	public const int RB = 21;

	[ActionIdFieldInfo(categoryName = "UI", friendlyName = "LT")]
	public const int LT = 22;

	[ActionIdFieldInfo(categoryName = "UI", friendlyName = "RT")]
	public const int RT = 23;
}
