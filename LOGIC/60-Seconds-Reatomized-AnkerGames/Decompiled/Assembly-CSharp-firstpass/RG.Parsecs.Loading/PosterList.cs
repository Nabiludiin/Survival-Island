using UnityEngine;

namespace RG.Parsecs.Loading;

[CreateAssetMenu(menuName = "60 Parsecs!/Missions/New Poster List")]
public class PosterList : ScriptableObject
{
	[SerializeField]
	private Poster[] _posters;

	public Poster[] Posters => _posters;
}
