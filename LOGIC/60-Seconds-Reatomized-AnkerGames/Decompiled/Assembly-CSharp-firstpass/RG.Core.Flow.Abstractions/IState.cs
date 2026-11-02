namespace RG.Core.Flow.Abstractions;

public interface IState
{
	bool Active { get; }

	string Name { get; }

	void Init();

	void Start();

	void Stop();

	void Update();

	bool CanRun();

	bool CanStop();
}
