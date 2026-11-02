using I2.Loc;
using RG.Parsecs.Survival;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionGoalController : MonoBehaviour
{
	[SerializeField]
	private Image _medalImageMain;

	[SerializeField]
	private Image _medalImageSecondary;

	[SerializeField]
	private GameObject _medalImageMainGameObject;

	[SerializeField]
	private GameObject _medalImageSecondaryGameObject;

	[SerializeField]
	private TextMeshProUGUI _goalName;

	private readonly Color TRANSPARENT_COLOR = new Color(1f, 1f, 1f, 0.33f);

	public void SetGoal(MedalReward medalReward, bool isAchieved, LocalizedString mainObjectiveTerm, LocalizedString challangeObjectiveTerm)
	{
		_goalName.text = medalReward.Goal.Name;
		_goalName.fontStyle = (medalReward.DisplayTextBold ? FontStyles.Bold : FontStyles.Normal);
		Image image = (medalReward.DisplayTextBold ? _medalImageMain : _medalImageSecondary);
		image.sprite = medalReward.Medal.MenuIconNormal;
		image.color = (isAchieved ? Color.white : TRANSPARENT_COLOR);
		if (medalReward.DisplayTextBold)
		{
			_medalImageSecondaryGameObject.SetActive(value: false);
			_medalImageMainGameObject.SetActive(value: true);
		}
		else
		{
			_medalImageMainGameObject.SetActive(value: false);
			_medalImageSecondaryGameObject.SetActive(value: true);
		}
	}
}
