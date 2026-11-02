using RG.Core.Base;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Missions/New Medal")]
public class Medal : RGScriptableObject
{
	[SerializeField]
	private Sprite _iconWhite;

	[SerializeField]
	private Sprite _menuIconNotAchieved;

	[SerializeField]
	private Sprite _menuIconNormal;

	[SerializeField]
	private Sprite _endingIconNotAchieved;

	[SerializeField]
	private Sprite _endingIconAchievedNow;

	public Sprite IconWhite => _iconWhite;

	public Sprite MenuIconNotAchieved => _menuIconNotAchieved;

	public Sprite MenuIconNormal => _menuIconNormal;

	public Sprite EndingIconNotAchieved => _endingIconNotAchieved;

	public Sprite EndingIconAchievedNow => _endingIconAchievedNow;
}
