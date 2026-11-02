using RG.Core.Data.DataTypes;

namespace RG.Core.Flow.Abstractions;

public interface ISession
{
	DualDictionary Values { get; }

	void StartSession();

	void EndSession();
}
