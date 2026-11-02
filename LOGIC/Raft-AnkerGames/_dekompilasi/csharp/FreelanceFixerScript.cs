using UnityEngine;

public class FreelanceFixerScript : MonoBehaviour
{
	public Transform[] transforms;

	public Material customMaterial;

	public float positionDistance = 5f;

	public void Reposition()
	{
		for (int i = 0; i < transforms.Length; i++)
		{
			transforms[i].position = new Vector3((float)i * positionDistance, 0f, 0f);
		}
	}

	public void Rename()
	{
		for (int i = 0; i < transforms.Length; i++)
		{
			Transform obj = transforms[i];
			Transform child = obj.GetChild(0);
			obj.name = "VG_DecorationPrefabBase_" + child.name;
		}
	}

	public void FixLODGroup()
	{
		for (int i = 0; i < transforms.Length; i++)
		{
			Transform obj = transforms[i];
			Transform child = obj.GetChild(0);
			LODGroup componentInChildren = child.GetComponentInChildren<LODGroup>();
			if (componentInChildren != null)
			{
				Object.DestroyImmediate(componentInChildren);
			}
			LODGroup component = obj.GetComponent<LODGroup>();
			if (component != null)
			{
				LOD[] lODs = component.GetLODs();
				Renderer[] renderers = new Renderer[1] { child.GetComponentInChildren<Renderer>() };
				lODs[0].renderers = renderers;
				component.SetLODs(lODs);
				component.RecalculateBounds();
			}
		}
	}

	public void AddBoxCollider()
	{
		for (int i = 0; i < transforms.Length; i++)
		{
			Transform child = transforms[i].GetChild(0);
			if (child.GetComponent<BoxCollider>() == null)
			{
				child.gameObject.AddComponent<BoxCollider>();
			}
		}
	}

	public void ActivateAll()
	{
		for (int i = 0; i < transforms.Length; i++)
		{
			Transform obj = transforms[i];
			obj.GetChild(0).gameObject.SetActive(value: true);
			obj.gameObject.SetActive(value: true);
		}
	}

	public void FixAll()
	{
		Reposition();
		Rename();
		FixLODGroup();
		AddBoxCollider();
		ActivateAll();
	}

	public void SetMaterial()
	{
		if (customMaterial == null)
		{
			Debug.Log("Can't set material, nothing is assigned");
			return;
		}
		for (int i = 0; i < transforms.Length; i++)
		{
			MeshRenderer componentInChildren = transforms[i].GetComponentInChildren<MeshRenderer>();
			if (componentInChildren != null)
			{
				componentInChildren.sharedMaterial = customMaterial;
			}
		}
	}
}
