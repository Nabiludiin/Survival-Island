using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class DamagedCraftingPanelController : MonoBehaviour, IRefreshInteractable
{
	[SerializeField]
	[Header("Data")]
	private CraftingData _craftingData;

	[SerializeField]
	private PlayerResources _playerResources;

	[SerializeField]
	private LocalizedString _insufficientSupplies;

	[SerializeField]
	private LocalizedString _sufficientSupplies;

	[SerializeField]
	private GameObject _itemPrefab;

	[SerializeField]
	private GameObject _resourcePrefab;

	[SerializeField]
	[Header("References")]
	private ResourcesDisplayController _resourcesDisplayController;

	[SerializeField]
	private Button _acceptButton;

	[SerializeField]
	private Toggle _reActivateButton;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	private TextMeshProUGUI _hasEnoughSuppliesText;

	[SerializeField]
	private Transform _requiredSuppliesHolder;

	[SerializeField]
	private GameObject _craftingPaper;

	public void Show()
	{
		_resourcesDisplayController.Show();
		SetRequiredSupplies();
		SetButtonState();
	}

	private void SetRequiredSupplies()
	{
		foreach (Transform item in _requiredSuppliesHolder)
		{
			Object.Destroy(item.gameObject);
		}
		for (int i = 0; i < _craftingData.StaticData.RequiredItems.Length; i++)
		{
			GameObject gameObject = Object.Instantiate(_itemPrefab);
			gameObject.GetComponentInChildren<Image>().sprite = _craftingData.StaticData.RequiredItems[i].BaseStaticData.ButtonIcon;
			gameObject.transform.SetParent(_requiredSuppliesHolder, worldPositionStays: false);
		}
		for (int j = 0; j < _craftingData.StaticData.RequiredResources.Length; j++)
		{
			GameObject gameObject2 = Object.Instantiate(_resourcePrefab);
			gameObject2.GetComponentInChildren<Image>().sprite = _craftingData.StaticData.RequiredResources[j].Resource.Icon;
			gameObject2.GetComponentInChildren<TextMeshProUGUI>().text = _craftingData.StaticData.RequiredResources[j].Amount.ToString();
			gameObject2.transform.SetParent(_requiredSuppliesHolder, worldPositionStays: false);
		}
	}

	private void SetButtonState()
	{
		bool flag = CanActivate() || _reActivateButton.isOn;
		_acceptButton.interactable = flag;
		_reActivateButton.interactable = flag;
		_hasEnoughSuppliesText.text = (flag ? _sufficientSupplies : _insufficientSupplies);
	}

	private bool CanActivate()
	{
		Debug.LogWarning("Warning: As activating system is no longer used this method may not be working anymore!");
		for (int i = 0; i < _playerResources.RuntimeResources.Count; i++)
		{
			for (int j = 0; j < _craftingData.StaticData.RequiredResources.Length; j++)
			{
				if (_playerResources.RuntimeResources[i].Resource.Equals(_craftingData.StaticData.RequiredResources[j].Resource) && (_playerResources.RuntimeResources[i] < _craftingData.StaticData.RequiredResources[j] || !_playerResources.RuntimeResources[i].CanLock()))
				{
					return false;
				}
			}
		}
		for (int k = 0; k < _craftingData.StaticData.RequiredItems.Length; k++)
		{
			if (!_craftingData.StaticData.RequiredItems[k].BaseRuntimeData.IsAvailable || !_craftingData.StaticData.RequiredItems[k].IsLockable())
			{
				return false;
			}
		}
		return true;
	}

	public void SetExpeditionReactivated(bool reactivate)
	{
		Debug.LogWarning("Warning: As activating system is no longer used this method may not be working anymore!");
		_craftingPaper.SetActive(reactivate);
		if (reactivate)
		{
			_craftingData.RuntimeData.ShouldReactivate = true;
			for (int i = 0; i < _playerResources.RuntimeResources.Count; i++)
			{
				for (int j = 0; j < _craftingData.StaticData.RequiredResources.Length; j++)
				{
					if (_playerResources.RuntimeResources[i].Resource.Equals(_craftingData.StaticData.RequiredResources[j].Resource))
					{
						_playerResources.RuntimeResources[i].Lock();
					}
				}
			}
			for (int k = 0; k < _craftingData.StaticData.RequiredItems.Length; k++)
			{
				_craftingData.StaticData.RequiredItems[k].Lock();
			}
			return;
		}
		_craftingData.RuntimeData.ShouldReactivate = false;
		for (int l = 0; l < _playerResources.RuntimeResources.Count; l++)
		{
			for (int m = 0; m < _craftingData.StaticData.RequiredResources.Length; m++)
			{
				if (_playerResources.RuntimeResources[l].Resource.Equals(_craftingData.StaticData.RequiredResources[m].Resource))
				{
					_playerResources.RuntimeResources[l].Unlock();
				}
			}
		}
		for (int n = 0; n < _craftingData.StaticData.RequiredItems.Length; n++)
		{
			_craftingData.StaticData.RequiredItems[n].Unlock();
		}
	}

	public void Repair()
	{
		Debug.LogWarning("Warning: As activating system is no longer used this method may not be working anymore!");
		for (int i = 0; i < _playerResources.RuntimeResources.Count; i++)
		{
			for (int j = 0; j < _craftingData.StaticData.RequiredResources.Length; j++)
			{
				if (_playerResources.RuntimeResources[i].Resource.Equals(_craftingData.StaticData.RequiredResources[j].Resource))
				{
					_playerResources.RuntimeResources[i].Unlock();
					_playerResources.RuntimeResources[i] -= _craftingData.StaticData.RequiredResources[j];
				}
			}
		}
		for (int k = 0; k < _craftingData.StaticData.RequiredItems.Length; k++)
		{
			_craftingData.StaticData.RequiredItems[k].Remove();
			_craftingData.StaticData.RequiredItems[k].Unlock();
		}
		_craftingData.RuntimeData.IsActive = true;
		_craftingData.RuntimeData.IsRepaired = true;
	}

	public void RefreshInteractable()
	{
		_closeButton.interactable = true;
	}
}
