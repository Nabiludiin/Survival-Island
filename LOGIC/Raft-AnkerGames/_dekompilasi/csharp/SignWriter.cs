public class SignWriter : Block
{
	private TextWriterObject textWriter;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public override void OnFinishedPlacement()
	{
		base.OnFinishedPlacement();
		if (ObjectIndex == ObjectIndex)
		{
			textWriter = GetComponent<TextWriterObject>();
			textWriter.Initialize(this);
		}
	}
}
