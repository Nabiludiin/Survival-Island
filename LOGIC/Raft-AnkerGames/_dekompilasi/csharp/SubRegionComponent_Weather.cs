public class SubRegionComponent_Weather : SubregionComponent
{
	public WeatherPoolType weatherPoolType;

	public override void AddSubregionComponent()
	{
		GetComponent<Subregion>().subregionComponents.Add(typeof(SubRegionComponent_Weather), this);
	}
}
