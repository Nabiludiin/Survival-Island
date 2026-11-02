using UnityEngine;

namespace RG.Parsecs.Survival;

public class Visual : MonoBehaviour
{
	[Tooltip("This will be used by the VisualsController to turn the visual on and off and needs to be set to a unique scriptable object instance.")]
	[SerializeField]
	private VisualId _id;

	public VisualId Id => _id;
}
