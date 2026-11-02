using System.Collections.Generic;

public class DamageArea_DamageOnce : DamageArea_Tick
{
	protected List<Network_Entity> affectedEntitiesInArea = new List<Network_Entity>();

	protected override void DamageEntity(Network_Player player)
	{
		if (!affectedEntitiesInArea.Contains(player.Stats))
		{
			base.DamageEntity(player);
			affectedEntitiesInArea.Add(player.Stats);
		}
	}

	public void ClearAffectedEnemiesFromArea()
	{
		affectedEntitiesInArea.Clear();
	}
}
