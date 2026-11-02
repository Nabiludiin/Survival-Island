using RG.Parsecs.Survival;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionSlotController : MonoBehaviour
{
	[SerializeField]
	private GameObject _lockedMission;

	[SerializeField]
	private Image _missionImage;

	[SerializeField]
	private Image _medalImage;

	[SerializeField]
	private TextMeshProUGUI _medalCount;

	public Mission Mission { get; private set; }

	public void SetMissionData(Mission mission)
	{
		Mission = mission;
		_missionImage.sprite = mission.Image;
		if (!mission.IsUnlocked(MissionManager.Instance.GetMedalsStats()))
		{
			_lockedMission.SetActive(value: true);
			_medalImage.sprite = mission.Requirements[0].Medal.IconWhite;
			_medalCount.text = "x " + GetRequiredAmountOfMedalsToUnlockMission(mission);
		}
		else
		{
			_lockedMission.SetActive(value: false);
		}
	}

	private int GetRequiredAmountOfMedalsToUnlockMission(Mission mission)
	{
		return mission.Requirements[0].Number;
	}
}
