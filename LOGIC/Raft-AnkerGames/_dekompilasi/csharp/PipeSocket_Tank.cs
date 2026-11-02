public class PipeSocket_Tank : PipeSocket
{
	public string text = string.Empty;

	public Tank connectedTank;

	private void OnValidate()
	{
		if (connectedTank == null)
		{
			connectedTank = GetComponentInParent<Tank>();
		}
	}
}
