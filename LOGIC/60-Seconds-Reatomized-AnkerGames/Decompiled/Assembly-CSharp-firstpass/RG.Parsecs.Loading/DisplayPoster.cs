using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Loading;

public class DisplayPoster : MonoBehaviour
{
	[SerializeField]
	private PosterList _posters;

	[SerializeField]
	private PosterController _poster;

	[SerializeField]
	private PosterController _missionPoster;

	[SerializeField]
	private RectTransform _transform;

	[SerializeField]
	private MissionManagerData _missionManagerData;

	private void Start()
	{
		if (MissionManager.Instance.IsMissionOngoing() && _missionManagerData.CurrentMission.Poster != null)
		{
			SetMissionPoster();
		}
		else
		{
			SetRandomPoster();
		}
		FlipBackgroundRandomly();
	}

	private void FlipBackgroundRandomly()
	{
		_transform.localScale = new Vector3((Random.Range(0, 2) > 0) ? 1 : (-1), 1f, 1f);
	}

	private void SetRandomPoster()
	{
		int num = Random.Range(0, _posters.Posters.Length);
		_missionPoster.gameObject.SetActive(value: false);
		_poster.gameObject.SetActive(value: true);
		_poster.SetPoster(_posters.Posters[num]);
	}

	private void SetMissionPoster()
	{
		_missionPoster.gameObject.SetActive(value: true);
		_poster.gameObject.SetActive(value: false);
		_missionPoster.SetPoster(_missionManagerData.CurrentMission.Poster);
	}
}
