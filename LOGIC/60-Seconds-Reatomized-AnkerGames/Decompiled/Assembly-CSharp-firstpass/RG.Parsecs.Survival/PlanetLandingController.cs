using System.Collections;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class PlanetLandingController : MonoBehaviour
{
	[SerializeField]
	private Animator _landingAnimator;

	[SerializeField]
	private Animator _planetAnimator;

	[SerializeField]
	private MusicManager _musicManager;

	public IEnumerator LandOnPlanet(PlanetDeck planet)
	{
		if (Singleton<HandCursor>.Instance != null)
		{
			Singleton<HandCursor>.Instance.RemoveSelection();
		}
		_planetAnimator.SetInteger("PlanetIndex", planet.PlanetIndex);
		_landingAnimator.SetTrigger("LandOnPlanet");
		_musicManager.SetMusicParameter(planet.LandingFmodParameter);
		yield return new WaitForSeconds(0.1f);
		yield return new WaitWhile(() => !IsAnimatorInState("NoLandingAnim"));
		yield return null;
	}

	private bool IsAnimatorInState(string state)
	{
		return _landingAnimator.GetCurrentAnimatorStateInfo(0).IsName(state);
	}
}
