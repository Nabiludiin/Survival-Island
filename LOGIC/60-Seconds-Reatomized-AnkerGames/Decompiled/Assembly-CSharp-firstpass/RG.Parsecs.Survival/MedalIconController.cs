using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class MedalIconController : MonoBehaviour
{
	[SerializeField]
	private int _medalIndex;

	[SerializeField]
	private Image _medalImage;

	[SerializeField]
	private Image _medalAchievedNow;

	[SerializeField]
	private TextMeshProUGUI _medalName;

	private void Start()
	{
		Mission currentMission = MissionManager.Instance.GetCurrentMission();
		if (currentMission == null || currentMission.Goals == null || currentMission.Goals.Count <= _medalIndex)
		{
			gameObject.SetActive(value: false);
			return;
		}
		GoalMedal goalMedal = currentMission.RuntimeData.Medals[_medalIndex];
		EnableCorrectImageRenderer(goalMedal).sprite = GetSpriteToSet(goalMedal, currentMission.Goals[_medalIndex].Medal);
		_medalName.text = currentMission.Goals[_medalIndex].Goal.Name;
	}

	private Image EnableCorrectImageRenderer(GoalMedal medal)
	{
		Image image = null;
		if (medal.IsAchieved && !medal.PreviouslyAchieved)
		{
			_medalImage.enabled = false;
			_medalAchievedNow.enabled = true;
			return _medalAchievedNow;
		}
		_medalImage.enabled = true;
		_medalAchievedNow.enabled = false;
		return _medalImage;
	}

	private Sprite GetSpriteToSet(GoalMedal goalMedal, Medal medal)
	{
		if (goalMedal.IsAchieved && !goalMedal.PreviouslyAchieved)
		{
			return medal.EndingIconAchievedNow;
		}
		if (goalMedal.PreviouslyAchieved)
		{
			return medal.IconWhite;
		}
		return medal.EndingIconNotAchieved;
	}
}
