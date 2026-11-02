using UnityEngine;

public class ObjectPlacementScript : MonoBehaviour
{
	[HideInInspector]
	public LayerMask raycastMask;

	public SO_ObjectPlacement selectedAsset;

	[HideInInspector]
	public Transform root;

	[HideInInspector]
	public bool customMode;

	[HideInInspector]
	public PlacementGhost ghost = new PlacementGhost();

	[HideInInspector]
	public Transform ghostParent;

	[HideInInspector]
	public string assetPath;

	[HideInInspector]
	public float scaleSensitivity = 1f;

	[HideInInspector]
	public float rotateSensitivity = 2f;
}
