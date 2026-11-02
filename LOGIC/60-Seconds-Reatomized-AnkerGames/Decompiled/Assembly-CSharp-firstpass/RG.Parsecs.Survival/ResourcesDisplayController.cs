using System;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class ResourcesDisplayController : MonoBehaviour
{
	[Serializable]
	public class ResourcesAnalogSlot
	{
		[SerializeField]
		private Resource _resource;

		[SerializeField]
		private AnalogCounterController _analogCounterController;

		[SerializeField]
		private Image _icon;

		public void SetImage()
		{
			_icon.sprite = _resource.Icon;
		}

		public void RefreshAmount(PlayerResources resources)
		{
			_analogCounterController.SetCounterValue(resources.GetResourceValueForCraftingUI(_resource));
		}
	}

	[SerializeField]
	private PlayerResources _playerResources;

	[SerializeField]
	private ResourcesAnalogSlot[] _resourcesAnalogSlot;

	public void Show()
	{
		for (int i = 0; i < _resourcesAnalogSlot.Length; i++)
		{
			_resourcesAnalogSlot[i].SetImage();
			_resourcesAnalogSlot[i].RefreshAmount(_playerResources);
		}
	}
}
