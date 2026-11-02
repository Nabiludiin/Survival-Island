using UnityEngine;

public class RareMaterial : MonoBehaviour
{
	[SerializeField]
	private RendererMaterialConnection[] materialConnections;

	public Randomizer randomizer;

	public bool IsValid
	{
		get
		{
			if (materialConnections != null)
			{
				return randomizer.GetItemCount() > 0;
			}
			return false;
		}
	}

	private void Awake()
	{
		SaveOriginalMaterial();
	}

	public void Button_AssignRandomMaterial()
	{
		AssignRandomRareMaterial();
	}

	public void Button_AssignOriginalMaterial()
	{
		AssignOriginalMaterial();
	}

	public void AssignRandomRareMaterial()
	{
		if (IsValid)
		{
			Material randomItem = randomizer.GetRandomItem<Material>();
			AssignSpecificMaterial(randomItem);
		}
	}

	public void AssignOriginalMaterial()
	{
		randomizer.lastRandomizedIndex = 0u;
		RendererMaterialConnection[] array = materialConnections;
		foreach (RendererMaterialConnection rendererMaterialConnection in array)
		{
			rendererMaterialConnection.renderer.sharedMaterial = rendererMaterialConnection.originalMaterial;
		}
	}

	public void AssignSpecificMaterial(uint randomizerIndex)
	{
		if (IsValid)
		{
			Material itemByIndex = randomizer.GetItemByIndex<Material>(randomizerIndex);
			if (itemByIndex != null)
			{
				randomizer.lastRandomizedIndex = randomizerIndex;
				AssignSpecificMaterial(itemByIndex);
			}
		}
	}

	public void AssignSpecificMaterial(Material material)
	{
		if (!(material == null))
		{
			RendererMaterialConnection[] array = materialConnections;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].renderer.sharedMaterial = material;
			}
		}
	}

	private void SaveOriginalMaterial()
	{
		RendererMaterialConnection[] array = materialConnections;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SaveOriginalMaterial();
		}
	}
}
