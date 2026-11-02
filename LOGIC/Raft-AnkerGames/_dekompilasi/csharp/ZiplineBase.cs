using UnityEngine;

public class ZiplineBase : Block, IBlockPlaced
{
	[SerializeField]
	private MeshPathBase meshPath;

	void IBlockPlaced.OnBlockPlacedInterface()
	{
		NetworkIDManager.AddNetworkID(meshPath, typeof(MeshPathBase));
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (hasBeenPlaced)
		{
			NetworkIDManager.RemoveNetworkID(meshPath, typeof(MeshPathBase));
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Block(RGDType.Block_ZiplineBase, this);
	}
}
