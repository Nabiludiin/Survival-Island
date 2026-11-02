using RG.Parsecs.Survival;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MedalCounterController : MonoBehaviour
{
	[SerializeField]
	private Medal _medal;

	[SerializeField]
	private TextMeshProUGUI _currentAmount;

	[SerializeField]
	private TextMeshProUGUI _maxAmount;

	[SerializeField]
	private Image _medalImage;

	private void Start()
	{
		_medalImage.sprite = _medal.MenuIconNormal;
		_currentAmount.text = MissionManager.Instance.GetNumberOfMedal(_medal).ToString();
		_maxAmount.text = "/" + MissionManager.Instance.GetTotalMedalCount(_medal);
	}
}
