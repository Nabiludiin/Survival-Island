using UnityEngine;

public class LODFixer : MonoBehaviour
{
	public LODFixer_LOD[] lods;

	private LODGroup lodGroup;

	private bool isDirty;

	private void Awake()
	{
		isDirty = true;
		if (lodGroup == null)
		{
			lodGroup = GetComponent<LODGroup>();
		}
		LODFixer_LOD[] array = lods;
		foreach (LODFixer_LOD lODFixer_LOD in array)
		{
			for (int j = 0; j < lODFixer_LOD.renderers.Length; j++)
			{
				lODFixer_LOD.renderers[j] = transform.Find(lODFixer_LOD.renderers[j].transform.name).GetComponent<Renderer>();
			}
		}
		OnValidate();
		Object.Destroy(this);
	}

	private void OnValidate()
	{
		if (!isDirty)
		{
			return;
		}
		isDirty = false;
		if (lodGroup == null)
		{
			lodGroup = GetComponent<LODGroup>();
		}
		LOD[] array = new LOD[lods.Length];
		for (int i = 0; i < array.Length; i++)
		{
			if (lods[i].renderers != null)
			{
				array[i].renderers = lods[i].renderers;
			}
			array[i].screenRelativeTransitionHeight = lods[i].screenHeight;
			if (i != 0 && array[i].screenRelativeTransitionHeight > array[i - 1].screenRelativeTransitionHeight - 0.05f)
			{
				array[i].screenRelativeTransitionHeight = array[i - 1].screenRelativeTransitionHeight - 0.05f;
				lods[i].screenHeight = array[i].screenRelativeTransitionHeight;
			}
		}
		lodGroup.SetLODs(array);
	}
}
