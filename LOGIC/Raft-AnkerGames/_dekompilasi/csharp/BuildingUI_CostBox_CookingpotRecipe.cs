public class BuildingUI_CostBox_CookingpotRecipe : BuildingUI_CostBox
{
	public override void Refresh()
	{
		base.Refresh();
		SetLabel("x" + requiredAmount);
	}
}
