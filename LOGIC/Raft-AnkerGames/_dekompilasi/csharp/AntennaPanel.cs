using UnityEngine;

public class AntennaPanel : MonoBehaviour
{
	[SerializeField]
	private GameObject error_CloseToReciever;

	[SerializeField]
	private GameObject error_FarAwayFromReciever;

	[SerializeField]
	private GameObject error_CloseToOtherAntenna;

	[SerializeField]
	private GameObject error_WrongAltitude;

	[SerializeField]
	private GameObject error_notConnected;

	[SerializeField]
	private GameObject errorImage;

	[SerializeField]
	private GameObject checkboxImage;

	public void Hide()
	{
		gameObject.SetActive(value: false);
	}

	public void RefreshUI(Reciever_Antenna antenna)
	{
		if (antenna == null)
		{
			HideStandardErrors();
			error_notConnected.SetActive(value: true);
			checkboxImage.SetActive(value: false);
			errorImage.SetActive(value: true);
		}
		else
		{
			error_notConnected.SetActive(value: false);
			error_CloseToReciever.SetActive(antenna.toCloseToReciever);
			error_FarAwayFromReciever.SetActive(antenna.toFarAwayFromReciever);
			error_CloseToOtherAntenna.SetActive(antenna.toCloseToOtherAntenna);
			error_WrongAltitude.SetActive(!antenna.onSameAltitudeAsReciever);
			checkboxImage.SetActive(antenna.HasValidConnection);
			errorImage.SetActive(!checkboxImage.activeSelf);
		}
		gameObject.SetActive(value: false);
		gameObject.SetActive(value: true);
	}

	private void HideStandardErrors()
	{
		error_CloseToReciever.SetActive(value: false);
		error_FarAwayFromReciever.SetActive(value: false);
		error_CloseToOtherAntenna.SetActive(value: false);
		error_WrongAltitude.SetActive(value: false);
	}
}
