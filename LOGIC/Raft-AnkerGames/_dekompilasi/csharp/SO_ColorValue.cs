using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorValue", menuName = "Raft/Miscellaneous/ColorValue", order = 1)]
public class SO_ColorValue : ScriptableObject
{
	[Header("Has to be unique for each color")]
	public uint uniqueColorIndex;

	[Header("MainColor")]
	public bool primaryColor;

	public SO_ColorValue darkColor;

	public SO_ColorValue brightColor;

	public int brightCostAmount;

	public int darkCostAmount;

	[Header("Colorpicker button color")]
	public Color buttonColor = Color.white;

	[Header("Actual color values to paint with")]
	public Color paintColor = Color.white;

	[Header("Cost to paint")]
	[HideInInspector]
	public List<Cost> PaintCost = new List<Cost>();

	public Cost redCost;

	public Cost yellowCost;

	public Cost blueCost;

	public Cost whiteCost;

	public Cost blackCost;

	private void OnValidate()
	{
		redCost.item = ItemManager.GetItemByName("Color_Red");
		yellowCost.item = ItemManager.GetItemByName("Color_Yellow");
		blueCost.item = ItemManager.GetItemByName("Color_Blue");
		blackCost.item = ItemManager.GetItemByName("Color_Black");
		whiteCost.item = ItemManager.GetItemByName("Color_White");
		if (darkColor != null)
		{
			darkColor.redCost.amount = (primaryColor ? Mathf.Clamp(redCost.amount - 2, 0, int.MaxValue) : Mathf.Clamp(redCost.amount - 1, 0, int.MaxValue));
			darkColor.yellowCost.amount = (primaryColor ? Mathf.Clamp(yellowCost.amount - 2, 0, int.MaxValue) : Mathf.Clamp(yellowCost.amount - 1, 0, int.MaxValue));
			darkColor.blueCost.amount = (primaryColor ? Mathf.Clamp(blueCost.amount - 2, 0, int.MaxValue) : Mathf.Clamp(blueCost.amount - 1, 0, int.MaxValue));
			darkColor.blackCost.amount = darkCostAmount;
			darkColor.whiteCost.amount = 0;
		}
		if (brightColor != null)
		{
			brightColor.redCost.amount = (primaryColor ? Mathf.Clamp(redCost.amount - 2, 0, int.MaxValue) : Mathf.Clamp(redCost.amount - 1, 0, int.MaxValue));
			brightColor.yellowCost.amount = (primaryColor ? Mathf.Clamp(yellowCost.amount - 2, 0, int.MaxValue) : Mathf.Clamp(yellowCost.amount - 1, 0, int.MaxValue));
			brightColor.blueCost.amount = (primaryColor ? Mathf.Clamp(blueCost.amount - 2, 0, int.MaxValue) : Mathf.Clamp(blueCost.amount - 1, 0, int.MaxValue));
			brightColor.blackCost.amount = 0;
			brightColor.whiteCost.amount = brightCostAmount;
		}
		PaintCost.Clear();
		if (redCost.amount != 0)
		{
			PaintCost.Add(redCost);
		}
		if (yellowCost.amount != 0)
		{
			PaintCost.Add(yellowCost);
		}
		if (blueCost.amount != 0)
		{
			PaintCost.Add(blueCost);
		}
		if (blackCost.amount != 0)
		{
			PaintCost.Add(blackCost);
		}
		if (whiteCost.amount != 0)
		{
			PaintCost.Add(whiteCost);
		}
	}

	public bool IsSameColor(SO_ColorValue other)
	{
		if (other == null && uniqueColorIndex == 0)
		{
			return true;
		}
		return other == this;
	}
}
