using Rewired;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ControllerDependentImage : MonoBehaviour
{
	[SerializeField]
	private Sprite _defaultControllerSprite;

	[SerializeField]
	private Sprite _xboxControllerSprite;

	[SerializeField]
	private Sprite _psControllerSprite;

	[SerializeField]
	private Sprite _steamControllerSprite;

	private Image _imageComponent;

	private Player _player;

	private Controller _currentController;

	private const string XBOX360_GUID = "d74a350e-fe8b-4e9e-bbcd-efff16d34115";

	private const string XBOX_ONE_GUID = "19002688-7406-4f4a-8340-8d25335406c8";

	private const string SONY_DUALSHOCK2_GUID = "c3ad3cad-c7cf-4ca8-8c2e-e3df8d9960bb";

	private const string SONY_DUALSHOCK3_GUID = "71dfe6c8-9e81-428f-a58e-c7e664b7fbed";

	private const string SONY_DUALSHOCK4_GUID = "cd9718bf-a87a-44bc-8716-60a0def28a9f";

	private const string STEAM_CONTROLLER_GUID = "2694f4b9-9d84-4f55-9ee8-78fbba744b7d";

	public void Awake()
	{
		_player = ReInput.players.GetPlayer(0);
	}

	public void Setup()
	{
		if (_imageComponent == null)
		{
			_imageComponent = GetComponent<Image>();
		}
		_currentController = _player.controllers.GetLastActiveController();
		if (!(_currentController is Joystick))
		{
			_imageComponent.sprite = _defaultControllerSprite;
		}
		else if (IsPSController(_currentController))
		{
			_imageComponent.sprite = _psControllerSprite;
		}
		else if (IsXboxController(_currentController))
		{
			_imageComponent.sprite = _xboxControllerSprite;
		}
		else if (IsSteamController(_currentController))
		{
			_imageComponent.sprite = _steamControllerSprite;
		}
		else
		{
			_imageComponent.sprite = _defaultControllerSprite;
		}
	}

	private void Update()
	{
		if (_player.controllers.GetLastActiveController() != _currentController)
		{
			Setup();
		}
	}

	private bool IsXboxController(Controller controller)
	{
		string text = controller.hardwareTypeGuid.ToString();
		if (!text.Equals("d74a350e-fe8b-4e9e-bbcd-efff16d34115"))
		{
			return text.Equals("19002688-7406-4f4a-8340-8d25335406c8");
		}
		return true;
	}

	private bool IsPSController(Controller controller)
	{
		string text = controller.hardwareTypeGuid.ToString();
		if (!text.Equals("c3ad3cad-c7cf-4ca8-8c2e-e3df8d9960bb") && !text.Equals("71dfe6c8-9e81-428f-a58e-c7e664b7fbed"))
		{
			return text.Equals("cd9718bf-a87a-44bc-8716-60a0def28a9f");
		}
		return true;
	}

	private bool IsSteamController(Controller controller)
	{
		return controller.hardwareTypeGuid.ToString().Equals("2694f4b9-9d84-4f55-9ee8-78fbba744b7d");
	}
}
