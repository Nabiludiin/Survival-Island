using System.Collections.Generic;

public class SubregionComponent_Sound : SubregionComponent
{
	public List<SubregionComponent_SoundType> soundTypes = new List<SubregionComponent_SoundType>();

	public override void AddSubregionComponent()
	{
		GetComponent<Subregion>().subregionComponents.Add(typeof(SubregionComponent_Sound), this);
	}
}
