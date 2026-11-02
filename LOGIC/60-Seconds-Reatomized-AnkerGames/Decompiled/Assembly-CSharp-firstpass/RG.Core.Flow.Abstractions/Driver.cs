using RG.Core.Base;

namespace RG.Core.Flow.Abstractions;

public abstract class Driver<IN, OUT> : RGMonoBehaviour where IN : class where OUT : class
{
	private IN _input;

	private OUT _output;

	protected IN Input
	{
		get
		{
			return _input;
		}
		set
		{
			_input = value;
		}
	}

	protected OUT Output
	{
		get
		{
			return _output;
		}
		set
		{
			_output = value;
		}
	}
}
