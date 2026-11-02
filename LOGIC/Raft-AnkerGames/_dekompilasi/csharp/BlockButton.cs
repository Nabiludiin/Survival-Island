public class BlockButton : InteractableButton, IBlockPlaced
{
	private bool hasBlockBeenPlaced;

	void IBlockPlaced.OnBlockPlacedInterface()
	{
		hasBlockBeenPlaced = true;
	}

	protected override void HandleOnIsRayed()
	{
		if (hasBlockBeenPlaced)
		{
			base.HandleOnIsRayed();
		}
	}
}
