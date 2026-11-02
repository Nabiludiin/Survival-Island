using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class DamagedExpeditionPanelController : MonoBehaviour, IRefreshInteractable
{
	[SerializeField]
	[Header("Data")]
	[Tooltip("Expedition Data scriptable object holding informations about expedition system.")]
	private ExpeditionData _expeditionData;

	[SerializeField]
	[Tooltip("Player Resources scriptable object holding informations about available resources.")]
	private PlayerResources _playerResources;

	[SerializeField]
	[Tooltip("Term with \"INSUFFICIENT SUPPLIES\" text.")]
	private LocalizedString _insufficientSupplies;

	[SerializeField]
	[Tooltip("Term with \"SUFFICIENT SUPPLIES\" text.")]
	private LocalizedString _sufficientSupplies;

	[SerializeField]
	[Tooltip("Prefab for item that will be used for displaying items needed to re-activate the system.")]
	private GameObject _itemPrefab;

	[SerializeField]
	[Tooltip("Prefab for resource that will be used for displaying resource needed to re-activate the system.")]
	private GameObject _resourcePrefab;

	[SerializeField]
	[Header("References")]
	[Tooltip("Holder where required items and resources will be displayed in the panel.")]
	private Transform _requiredSuppliesHolder;

	[SerializeField]
	[Tooltip("Holder where available resources will be displayed in the panel.")]
	private Transform _availableResourcesHolder;

	[SerializeField]
	[Tooltip("Reference to the toggle that will be enabled when there are sufficent supplies to re-activate the system.")]
	private Toggle _reActivateButton;

	[SerializeField]
	[Tooltip("Reference to the button that will be enabled if _reActivateToggle isOn")]
	private Button _acceptButton;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	[Tooltip("Reference to the field where text about sufficient supplies will be displayed.")]
	private TextMeshProUGUI _hasEnoughSuppliesText;

	public void Initialize()
	{
		SetRequiredSupplies();
		SetButtonState();
		SetAvailableResources();
	}

	private void SetRequiredSupplies()
	{
		foreach (Transform item in _requiredSuppliesHolder)
		{
			Object.Destroy(item.gameObject);
		}
		for (int i = 0; i < _expeditionData.StaticData.RequiredItems.Length; i++)
		{
			GameObject gameObject = Object.Instantiate(_itemPrefab);
			gameObject.GetComponentInChildren<Image>().sprite = _expeditionData.StaticData.RequiredItems[i].BaseStaticData.ButtonIcon;
			gameObject.transform.SetParent(_requiredSuppliesHolder, worldPositionStays: false);
		}
		for (int j = 0; j < _expeditionData.StaticData.RequiredResources.Length; j++)
		{
			GameObject gameObject2 = Object.Instantiate(_resourcePrefab);
			gameObject2.GetComponentInChildren<Image>().sprite = _expeditionData.StaticData.RequiredResources[j].Resource.Icon;
			gameObject2.GetComponentInChildren<TextMeshProUGUI>().text = _expeditionData.StaticData.RequiredResources[j].Amount.ToString();
			gameObject2.transform.SetParent(_requiredSuppliesHolder, worldPositionStays: false);
		}
	}

	private void SetAvailableResources()
	{
		foreach (Transform item in _availableResourcesHolder)
		{
			Object.Destroy(item.gameObject);
		}
		for (int i = 0; i < _playerResources.RuntimeResources.Count; i++)
		{
			GameObject gameObject = Object.Instantiate(_resourcePrefab);
			gameObject.GetComponentInChildren<Image>().sprite = _playerResources.RuntimeResources[i].Resource.Icon;
			gameObject.GetComponentInChildren<TextMeshProUGUI>().text = _playerResources.RuntimeResources[i].Amount.ToString();
			gameObject.transform.SetParent(_availableResourcesHolder, worldPositionStays: false);
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
			for (int j = 0; j < _expeditionData.StaticData.RequiredResources.Length; j++)
			{
				if (_playerResources.RuntimeResources[i].Resource.Equals(_expeditionData.StaticData.RequiredResources[j].Resource) && (_playerResources.RuntimeResources[i] < _expeditionData.StaticData.RequiredResources[j] || !_playerResources.RuntimeResources[i].CanLock()))
				{
					return false;
				}
			}
		}
		for (int k = 0; k < _expeditionData.StaticData.RequiredItems.Length; k++)
		{
			if (!_expeditionData.StaticData.RequiredItems[k].BaseRuntimeData.IsAvailable || !_expeditionData.StaticData.RequiredItems[k].IsLockable())
			{
				return false;
			}
		}
		return true;
	}

	public void SetExpeditionReactivated(bool reactivate)
	{
		Debug.LogWarning("Warning: As activating system is no longer used this method may not be working anymore!");
		if (!reactivate)
		{
			_expeditionData.RuntimeData.ShouldReactivate = false;
			for (int i = 0; i < _playerResources.RuntimeResources.Count; i++)
			{
				for (int j = 0; j < _expeditionData.StaticData.RequiredResources.Length; j++)
				{
					if (_playerResources.RuntimeResources[i].Resource.Equals(_expeditionData.StaticData.RequiredResources[j].Resource))
					{
						_playerResources.RuntimeResources[i].Unlock();
					}
				}
			}
			for (int k = 0; k < _expeditionData.StaticData.RequiredItems.Length; k++)
			{
				_expeditionData.StaticData.RequiredItems[k].Unlock();
			}
			return;
		}
		_expeditionData.RuntimeData.ShouldReactivate = true;
		for (int l = 0; l < _playerResources.RuntimeResources.Count; l++)
		{
			for (int m = 0; m < _expeditionData.StaticData.RequiredResources.Length; m++)
			{
				if (_playerResources.RuntimeResources[l].Resource.Equals(_expeditionData.StaticData.RequiredResources[m].Resource))
				{
					_playerResources.RuntimeResources[l].Lock();
				}
			}
		}
		for (int n = 0; n < _expeditionData.StaticData.RequiredItems.Length; n++)
		{
			_expeditionData.StaticData.RequiredItems[n].Lock();
		}
	}

	public void RepairExpedition()
	{
		Debug.LogWarning("Warning: As activating system is no longer used this method may not be working anymore!");
		for (int i = 0; i < _playerResources.RuntimeResources.Count; i++)
		{
			for (int j = 0; j < _expeditionData.StaticData.RequiredResources.Length; j++)
			{
				if (_playerResources.RuntimeResources[i].Resource.Equals(_expeditionData.StaticData.RequiredResources[j].Resource))
				{
					_playerResources.RuntimeResources[i].Unlock();
					_playerResources.RuntimeResources[i] -= _expeditionData.StaticData.RequiredResources[j];
				}
			}
		}
		for (int k = 0; k < _expeditionData.StaticData.RequiredItems.Length; k++)
		{
			_expeditionData.StaticData.RequiredItems[k].Remove();
			_expeditionData.StaticData.RequiredItems[k].Unlock();
		}
		_expeditionData.RuntimeData.IsActive = true;
		_expeditionData.RuntimeData.IsRepaired = true;
	}

	public void RefreshInteractable()
	{
		_closeButton.interactable = true;
	}
}
