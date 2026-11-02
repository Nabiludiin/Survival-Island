public class SubRegionComponent_NoWeatherParticle : SubregionComponent
{
	public override void AddSubregionComponent()
	{
		GetComponent<Subregion>().subregionComponents.Add(typeof(SubRegionComponent_NoWeatherParticle), this);
	}
}
