using I2.Loc;
using UnityEngine;

namespace RG.Parsecs.Loading;

[CreateAssetMenu(menuName = "60 Parsecs!/Missions/New Poster")]
public class Poster : ScriptableObject
{
	[SerializeField]
	private Sprite _posterImage;

	[SerializeField]
	private LocalizedString _topText;

	[SerializeField]
	private LocalizedString _bottomText;

	public Sprite PosterImage => _posterImage;

	public LocalizedString TopText => _topText;

	public LocalizedString BottomText => _bottomText;
}
