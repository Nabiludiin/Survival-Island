using UnityEngine;

namespace RG.Parsecs.Survival;

public class StarlogPanelController : MonoBehaviour
{
	[SerializeField]
	private EStarlogPanelType _panelType;

	public EStarlogPanelType PanelType => _panelType;
}
