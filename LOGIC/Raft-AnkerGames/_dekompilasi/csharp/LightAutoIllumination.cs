public class LightAutoIllumination : LightNightNDayToggler
{
	protected override bool IsValidForIllumination()
	{
		if (SkyManager.UsingCustomEnvironmentLight)
		{
			return true;
		}
		return base.IsValidForIllumination();
	}
}
