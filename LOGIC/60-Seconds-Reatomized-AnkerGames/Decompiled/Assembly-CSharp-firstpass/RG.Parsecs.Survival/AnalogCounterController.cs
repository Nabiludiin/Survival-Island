using TMPro;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class AnalogCounterController : MonoBehaviour
{
	[SerializeField]
	[Tooltip("First digit of counter counting from right.")]
	private TextMeshProUGUI _firstDigit;

	[SerializeField]
	[Tooltip("Second digit of counter counting from right.")]
	private TextMeshProUGUI _secondDigit;

	[SerializeField]
	[Tooltip("Third digit of counter counting from right.")]
	private TextMeshProUGUI _thirdDigit;

	public void SetCounterValue(int value)
	{
		if (value > 999 || value < 0)
		{
			Debug.LogWarning("This digit can't display values bigger than 999 or negative values correctly.");
		}
		int num = value % 10;
		int num2 = (value - num) % 100 / 10;
		int num3 = (value - num2) % 1000 / 100;
		_firstDigit.text = num.ToString();
		_secondDigit.text = num2.ToString();
		_thirdDigit.text = num3.ToString();
	}
}
