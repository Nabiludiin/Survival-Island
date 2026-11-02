using System;
using I2.Loc;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class ScavengeTutorialHint
{
	public float Time;

	public float Duration;

	public LocalizedString TextLoc;

	public Sprite Sprite;

	public Sprite ScreenSprite;

	public VerticalAlignmentOptions alignment = VerticalAlignmentOptions.Middle;
}
