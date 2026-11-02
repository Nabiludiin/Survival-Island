public class MeshPathConnection
{
	public MeshPathBase baseA;

	public MeshPathBase baseB;

	public MeshPath path;

	public MeshPathConnection(MeshPathBase baseA, MeshPathBase baseB, MeshPath path)
	{
		this.baseA = baseA;
		this.baseB = baseB;
		this.path = path;
	}

	public bool IsValid()
	{
		if (baseA != null && baseB != null)
		{
			return path != null;
		}
		return false;
	}
}
