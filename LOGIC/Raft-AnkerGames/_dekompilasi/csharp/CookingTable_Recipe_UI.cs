using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingTable_Recipe_UI : MonoBehaviour
{
	private static Dictionary<Item_Base, Material> generatedMaterials = new Dictionary<Item_Base, Material>();

	[SerializeField]
	private bool showRecipeInStart;

	[SerializeField]
	private SO_CookingTable_Recipe recipe;

	[SerializeField]
	private CostCollection costCollection;

	[SerializeField]
	private TextMeshProUGUI recipeTypeLabel;

	[SerializeField]
	private TextMeshProUGUI recipeLabel;

	[SerializeField]
	private Image recipeImage;

	[SerializeField]
	private MeshRenderer paperRenderer;

	[SerializeField]
	private Material normalMaterial;

	[SerializeField]
	private Material buffMaterial;

	[SerializeField]
	private Canvas canvas;

	[SerializeField]
	private Renderer renderedTexture;

	[SerializeField]
	private Material[] materialsAllowedToRender;

	private Material uiMaterialInstance;

	public SO_CookingTable_Recipe Recipe => recipe;

	private void Start()
	{
		if (showRecipeInStart)
		{
			ShowRecipe(recipe);
		}
	}

	public void ShowRecipe(SO_CookingTable_Recipe recipe)
	{
		if (!(recipe != null))
		{
			return;
		}
		recipeTypeLabel.text = recipe.GetRecipeTypeLabel();
		recipeLabel.text = recipe.Result.settings_Inventory.DisplayName;
		costCollection.ShowCost(recipe.RecipeCost);
		recipeImage.sprite = recipe.Result.settings_Inventory.Sprite;
		if (paperRenderer != null)
		{
			if (normalMaterial == null)
			{
				Debug.Log("Recipe has a paperRenderer assigned but normalMaterial is null");
			}
			if (buffMaterial == null)
			{
				Debug.Log("Recipe has a paperRenderer assigned but buffMaterial is null");
			}
			paperRenderer.sharedMaterial = (recipe.IsBuff ? buffMaterial : normalMaterial);
		}
		bool flag = false;
		for (int i = 0; i < materialsAllowedToRender.Length; i++)
		{
			if (materialsAllowedToRender[i] == renderedTexture.sharedMaterial)
			{
				flag = true;
				break;
			}
		}
		if (flag && canvas != null && renderedTexture != null)
		{
			if (!generatedMaterials.TryGetValue(recipe.Result, out uiMaterialInstance))
			{
				uiMaterialInstance = Object.Instantiate(renderedTexture.sharedMaterial);
				canvas.gameObject.SetActive(value: true);
				CanvasPrinter.RenderCanvasToMaterial(canvas, uiMaterialInstance);
				generatedMaterials.Add(recipe.Result, uiMaterialInstance);
			}
			canvas.gameObject.SetActive(value: false);
			renderedTexture.material = uiMaterialInstance;
		}
	}
}
