using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class SetGoalView : MonoBehaviour
{
	public enum EGoalStatus
	{
		FINISHED,
		IN_PROGRESS,
		FAILED
	}

	[SerializeField]
	private Image _imgae;

	[SerializeField]
	private TextMeshProUGUI _text;

	[SerializeField]
	private GameObject _done;

	[SerializeField]
	private GameObject _fail;

	public void SetGoalViewData(EGoalStatus status, string goalName)
	{
		switch (status)
		{
		case EGoalStatus.FINISHED:
			_done.SetActive(value: true);
			_imgae.color = Color.white;
			break;
		case EGoalStatus.FAILED:
			_fail.SetActive(value: true);
			_imgae.color = Color.white;
			break;
		case EGoalStatus.IN_PROGRESS:
			_fail.SetActive(value: false);
			_done.SetActive(value: false);
			break;
		}
		_text.text = goalName;
	}
}
