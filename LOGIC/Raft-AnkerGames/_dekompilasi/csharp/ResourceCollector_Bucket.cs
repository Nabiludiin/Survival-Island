using FMODUnity;

public class ResourceCollector_Bucket : ResourceCollector
{
	public EventReference er_gatherMilk;

	public override void OnHarvest()
	{
		base.OnHarvest();
		if (!er_gatherMilk.IsNull)
		{
			RuntimeManager.PlayOneShot(er_gatherMilk, transform.position);
		}
	}
}
