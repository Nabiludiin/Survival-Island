using UnityEngine;

[CreateAssetMenu(fileName = "ResourceRegenerativeObject", menuName = "Raft/Item/ResourceRegenerativeObject")]
public class SO_ResourceRegenerative : ScriptableObject
{
	public float originRegenTime;

	public Item_Base resource;

	public Item_Base collectionItem;

	public AimSprite aimAtSprite;
}
