using System.Collections.Generic;

namespace Knife.DeferredDecals;

public class DeferredDecalsManager
{
	private static DeferredDecalsManager m_Instance;

	internal List<Decal> m_Decals = new List<Decal>();

	public static DeferredDecalsManager instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = new DeferredDecalsManager();
			}
			return m_Instance;
		}
	}

	public void AddOrUpdateDecal(Decal d)
	{
		RemoveDecal(d);
		if (d.DecalMaterial == null)
		{
			return;
		}
		d.DecalMaterial.DisableKeyword("MULTI_TERRAIN_DECAL");
		d.DecalMaterial.EnableKeyword("NO_TERRAIN");
		if (m_Decals.Count == 0)
		{
			m_Decals.Add(d);
			return;
		}
		for (int i = 0; i <= m_Decals.Count; i++)
		{
			if (i == m_Decals.Count)
			{
				m_Decals.Add(d);
				break;
			}
			if (m_Decals[i].SortingOrder > d.SortingOrder)
			{
				m_Decals.Insert(i, d);
				break;
			}
		}
	}

	public void RemoveDecal(Decal d)
	{
		m_Decals.Remove(d);
	}
}
