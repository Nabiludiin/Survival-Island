using UnityEngine;

public class LayerMasks : MonoBehaviour
{
	public LayerMask ignoreMask;

	public LayerMask waterWithObstruction;

	public LayerMask blockcreatorOverlap;

	public LayerMask groundMask;

	public LayerMask groundMask_NonRaft;

	public LayerMask groundMask_Raft;

	public LayerMask playerMask;

	public LayerMask projectileObstruction;

	public LayerMask raycastInteractable;

	public LayerMask ziplineCollision;

	public static LayerMask MASK_DomesticLandmarkCollision;

	public static LayerMask MASK_DomesticRaftCollision;

	public static LayerMask MASK_ZiplineCollision;

	public static LayerMask MASK_TransparentFX;

	public static LayerMask MASK_BlockCreatorOverlap;

	public static LayerMask MASK_Water_with_obstruction;

	public static LayerMask MASK_Water;

	public static LayerMask MASK_Item;

	public static LayerMask MASK_DroppedItem;

	public static LayerMask MASK_IgnorePlayer;

	public static LayerMask MASK_Block;

	public static LayerMask MASK_BuildQuad;

	public static LayerMask MASK_Obstruction;

	public static LayerMask MASK_RemotePlayer;

	public static LayerMask MASK_GroundMask;

	public static LayerMask MASK_GroundMask_NonRaft;

	public static LayerMask MASK_GroundMask_Raft;

	public static LayerMask MASK_Players;

	public static LayerMask MASK_ProjectileObstruction;

	public static LayerMask MASK_RaycastInteractable;

	public static LayerMask MASK_Default;

	public static LayerMask MASK_HandCamera;

	private void Awake()
	{
		MASK_ProjectileObstruction = projectileObstruction;
		MASK_IgnorePlayer = ignoreMask;
		MASK_Water_with_obstruction = waterWithObstruction;
		MASK_BlockCreatorOverlap = blockcreatorOverlap;
		MASK_GroundMask = groundMask;
		MASK_GroundMask_NonRaft = groundMask_NonRaft;
		MASK_GroundMask_Raft = groundMask_Raft;
		MASK_Players = playerMask;
		MASK_RaycastInteractable = raycastInteractable;
		MASK_ZiplineCollision = ziplineCollision;
		MASK_DomesticLandmarkCollision = LayerMask.NameToLayer("Default");
		MASK_DomesticRaftCollision = LayerMask.NameToLayer("CollideWithBlock");
		MASK_TransparentFX = 1 << LayerMask.NameToLayer("TransparentFX");
		MASK_Water = 1 << LayerMask.NameToLayer("Water");
		MASK_Item = 1 << LayerMask.NameToLayer("Item");
		MASK_DroppedItem = 1 << LayerMask.NameToLayer("DropItem");
		MASK_Block = 1 << LayerMask.NameToLayer("Block");
		MASK_BuildQuad = 1 << LayerMask.NameToLayer("BuildQuad");
		MASK_Obstruction = 1 << LayerMask.NameToLayer("Obstruction");
		MASK_RemotePlayer = 1 << LayerMask.NameToLayer("RemotePlayer");
		MASK_Default = LayerMask.NameToLayer("Default");
		MASK_HandCamera = LayerMask.NameToLayer("HandCamera");
	}
}
