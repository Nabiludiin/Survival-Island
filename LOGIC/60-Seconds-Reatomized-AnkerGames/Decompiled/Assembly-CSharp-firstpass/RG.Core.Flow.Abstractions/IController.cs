namespace RG.Core.Flow.Abstractions;

public interface IController
{
	IStateMachine StateMachine { set; }
}
