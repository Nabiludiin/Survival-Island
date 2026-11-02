using FMODUnity;
using UnityEngine;

public class Block_TicTacToe : Block
{
	[SerializeField]
	private TicTacToe ticTacToe;

	[SerializeField]
	private Mesh mesh_X;

	[SerializeField]
	private Mesh mesh_O;

	private InteractableButton_Networked_TicTacToe[] buttons;

	public EventReference er_takePiece;

	public EventReference er_dropPiece;

	public override void OnFinishedPlacement()
	{
		base.OnFinishedPlacement();
		NetworkIDManager.AddNetworkID(ticTacToe, typeof(TicTacToe));
		buttons = GetComponentsInChildren<InteractableButton_Networked_TicTacToe>();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (hasBeenPlaced)
		{
			NetworkIDManager.RemoveNetworkID(ticTacToe, typeof(TicTacToe));
		}
	}

	public bool InteractWithButton(int index)
	{
		if (index < 0 || index >= buttons.Length)
		{
			return false;
		}
		MeshRenderer renderer = buttons[index].renderer;
		bool flag = !renderer.enabled;
		if (flag)
		{
			int placedMeshCount = GetPlacedMeshCount(mesh_X);
			int placedMeshCount2 = GetPlacedMeshCount(mesh_O);
			if (IsFullOfPlacedBricks(placedMeshCount, placedMeshCount2))
			{
				return false;
			}
			Mesh sharedMesh = ((placedMeshCount > placedMeshCount2) ? mesh_O : mesh_X);
			buttons[index].filter.sharedMesh = sharedMesh;
		}
		if (flag)
		{
			RuntimeManager.PlayOneShot(er_dropPiece, transform.position);
		}
		else
		{
			RuntimeManager.PlayOneShot(er_takePiece, transform.position);
		}
		renderer.enabled = flag;
		return true;
	}

	public bool IsFullOfPlacedBricks()
	{
		int placedMeshCount = GetPlacedMeshCount(mesh_X);
		int placedMeshCount2 = GetPlacedMeshCount(mesh_O);
		return IsFullOfPlacedBricks(placedMeshCount, placedMeshCount2);
	}

	public bool IsFullOfPlacedBricks(int placedXes, int placedOes)
	{
		if (placedXes == 3)
		{
			return placedOes == 3;
		}
		return false;
	}

	public bool ShouldPlaceXNext()
	{
		int placedMeshCount = GetPlacedMeshCount(mesh_X);
		int placedMeshCount2 = GetPlacedMeshCount(mesh_O);
		return placedMeshCount <= placedMeshCount2;
	}

	private int GetPlacedMeshCount(Mesh meshToCompare)
	{
		int num = 0;
		for (int i = 0; i < buttons.Length; i++)
		{
			if (buttons[i].renderer.enabled && buttons[i].filter.sharedMesh == meshToCompare)
			{
				num++;
			}
		}
		return num;
	}

	public void RestoreRGD(RGD_Block_TicTacToe rgd)
	{
		if (!(rgd == null) && buttons != null && rgd.enabled.Length == buttons.Length)
		{
			for (int i = 0; i < rgd.enabled.Length; i++)
			{
				buttons[i].renderer.enabled = rgd.enabled[i];
				buttons[i].filter.sharedMesh = (rgd.meshType[i] ? mesh_X : mesh_O);
			}
		}
	}

	public override RGD_Block GetBlockCreationData()
	{
		RGD_Block_TicTacToe rGD_Block_TicTacToe = new RGD_Block_TicTacToe(RGDType.Block_TicTacToe, this);
		rGD_Block_TicTacToe.meshType = new bool[buttons.Length];
		rGD_Block_TicTacToe.enabled = new bool[buttons.Length];
		for (int i = 0; i < buttons.Length; i++)
		{
			rGD_Block_TicTacToe.enabled[i] = buttons[i].renderer.enabled;
			if (rGD_Block_TicTacToe.enabled[i])
			{
				rGD_Block_TicTacToe.meshType[i] = buttons[i].filter.sharedMesh == mesh_X;
			}
		}
		return rGD_Block_TicTacToe;
	}

	public override RGD Serialize_Save()
	{
		return GetBlockCreationData();
	}
}
