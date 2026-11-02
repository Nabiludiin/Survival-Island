using System.Collections.Generic;

public class PipeSocket : Pipe
{
	public static List<PipeSocketTypeConnection> TypeConnections = new List<PipeSocketTypeConnection>
	{
		new PipeSocketTypeConnection(PipeSocketType.Input_Fuel, PipeSocketType.Output_Fuel),
		new PipeSocketTypeConnection(PipeSocketType.Input_Fuel, PipeSocketType.InputOutput_Fuel),
		new PipeSocketTypeConnection(PipeSocketType.InputOutput_Fuel, PipeSocketType.Output_Fuel),
		new PipeSocketTypeConnection(PipeSocketType.Input_Water, PipeSocketType.Output_Water)
	};

	public PipeSocketType socketType;
}
