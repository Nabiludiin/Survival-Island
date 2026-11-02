public class dfFocusEventArgs : dfControlEventArgs
{
	public dfControl GotFocus => Source;

	public dfControl LostFocus { get; private set; }

	internal dfFocusEventArgs(dfControl GotFocus, dfControl LostFocus)
		: base(GotFocus)
	{
		this.LostFocus = LostFocus;
	}
}
