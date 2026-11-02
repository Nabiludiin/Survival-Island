using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using Rewired;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Menu;

public class AssignableButtonPrefab : MonoBehaviour
{
	private InputAction action;

	private ControllerMap map;

	private Pole pole;

	private ActionElementMap aem;

	public TextMeshProUGUI actionName;

	public TextMeshProUGUI actionButton;

	private PoolButtonPanel poolButtonPanel;

	private bool initialized;

	public void Init(InputAction action, ControllerMap map, Pole pole, PoolButtonPanel poolButtonPanel)
	{
		this.action = action;
		this.map = map;
		this.pole = pole;
		this.poolButtonPanel = poolButtonPanel;
		initialized = true;
		RefreshDesc();
	}

	private void OnEnable()
	{
		RefreshDesc();
	}

	private void RefreshDesc()
	{
		if (!initialized)
		{
			return;
		}
		IEnumerable<ActionElementMap> enumerable = map.ElementMapsWithAction(action.id);
		if (action.type == InputActionType.Button)
		{
			actionName.text = new LocalizedString(action.descriptiveName);
			aem = enumerable.First();
		}
		else
		{
			foreach (ActionElementMap item in enumerable)
			{
				if (item.axisContribution == pole)
				{
					aem = item;
				}
			}
			if (pole == Pole.Positive)
			{
				actionName.text = new LocalizedString(action.positiveDescriptiveName);
			}
			else
			{
				actionName.text = new LocalizedString(action.negativeDescriptiveName);
			}
		}
		actionButton.text = aem.keyCode.ToString();
	}

	public void Click()
	{
		if (initialized)
		{
			Debug.Log("Pracujemy na " + action.name);
			poolButtonPanel.Show(this);
		}
	}

	public void Set(KeyCode keycode)
	{
		if (initialized)
		{
			map.ReplaceElementMap(aem.id, action.id, pole, keycode, ModifierKeyFlags.None);
			ReInput.players.GetPlayer(0);
			if (ReInput.userDataStore != null)
			{
				ReInput.userDataStore.Save();
			}
			RefreshDesc();
		}
	}
}
