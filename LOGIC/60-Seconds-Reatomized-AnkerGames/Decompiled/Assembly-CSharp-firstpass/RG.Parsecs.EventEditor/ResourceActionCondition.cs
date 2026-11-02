using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[CreateAssetMenu(fileName = "New Resource Action Condition", menuName = "60 Parsecs!/Event Editor/Action Conditions/New Resource Action Condition")]
public class ResourceActionCondition : BaseActionCondition
{
	[SerializeField]
	private GameResource _resource;

	[SerializeField]
	private bool _useResource;

	[SerializeField]
	private PlayerResources _playerResources;

	public GameResource Resource => _resource;

	public override bool CanSelect()
	{
		if (!_useResource)
		{
			return true;
		}
		ChoiceCardController playerChoice = EventManager.GetPlayerChoice();
		if (playerChoice != null)
		{
			GameResource gameResource = playerChoice.GameResource;
			if (gameResource != null)
			{
				if (!(gameResource.Resource != _resource.Resource))
				{
					return _playerResources.CanLock(_resource.Resource, _resource.Amount - gameResource.Amount);
				}
				return _playerResources.CanLock(_resource);
			}
		}
		return _playerResources.CanLock(_resource);
	}

	public override void OnSelect()
	{
		if (_useResource)
		{
			_playerResources.Lock(_resource);
		}
	}

	public override void OnDeselect()
	{
		if (_useResource)
		{
			_playerResources.Unlock(_resource);
		}
	}
}
