using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SouperScavengerPromo : MonoBehaviour
{
	[SerializeField]
	private Button _wishlistButton;

	[SerializeField]
	private SpriteRenderer _wishlistSpriteRenderer;

	[SerializeField]
	private Sprite _wishlistSprite;

	[SerializeField]
	private Sprite _souperScavengerBanner;

	[SerializeField]
	private TextMeshProUGUI _souperScavengerLabel;

	[SerializeField]
	private Color _souperScavengerWishlistColor;

	private const string _souperScavengerUrl = "https://store.steampowered.com/app/3197360/60_Seconds_Souper_Scavenger/";

	private void Start()
	{
		BindEvents();
	}

	private void OnDestroy()
	{
		UnBindEvents();
	}

	private void BindEvents()
	{
		_wishlistButton.onClick.AddListener(HandleWishlistButtonPressed);
	}

	private void UnBindEvents()
	{
		_wishlistButton.onClick.RemoveListener(HandleWishlistButtonPressed);
	}

	private void OnMouseEnter()
	{
		HandleMouseEntered();
	}

	private void OnMouseExit()
	{
		HandleMouseExited();
	}

	public void HandleMouseEntered()
	{
		_wishlistSpriteRenderer.sprite = _wishlistSprite;
		_souperScavengerLabel.color = _souperScavengerWishlistColor;
	}

	public void HandleMouseExited()
	{
		_wishlistSpriteRenderer.sprite = _souperScavengerBanner;
		_souperScavengerLabel.color = new Color(0f, 0f, 0f, 0f);
	}

	public void HandleWishlistButtonPressed()
	{
		Application.OpenURL("https://store.steampowered.com/app/3197360/60_Seconds_Souper_Scavenger/");
	}
}
