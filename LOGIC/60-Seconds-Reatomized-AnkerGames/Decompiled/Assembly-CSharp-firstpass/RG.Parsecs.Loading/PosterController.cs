using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Loading;

public class PosterController : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _topText;

	[SerializeField]
	private TextMeshProUGUI _bottomText;

	[SerializeField]
	private Image _posterImage;

	private const string NO_POSTER_SPRITE_ERROR = "There is no poster sprite to set! The posterImage attribute is null.";

	public void SetPoster(Poster poster)
	{
		if ((string)poster.TopText != null && !string.IsNullOrEmpty(poster.TopText.mTerm))
		{
			_topText.text = poster.TopText.ToString();
			_topText.gameObject.GetComponent<Localize>().OnLocalize();
		}
		else
		{
			_topText.text = string.Empty;
		}
		if ((string)poster.BottomText != null && !string.IsNullOrEmpty(poster.BottomText.mTerm))
		{
			_bottomText.text = poster.BottomText.ToString();
			_bottomText.gameObject.GetComponent<Localize>().OnLocalize();
		}
		else
		{
			_bottomText.text = string.Empty;
		}
		if (poster.PosterImage == null)
		{
			Debug.LogError("There is no poster sprite to set! The posterImage attribute is null.");
		}
		_posterImage.sprite = poster.PosterImage;
		gameObject.SetActive(value: true);
	}
}
