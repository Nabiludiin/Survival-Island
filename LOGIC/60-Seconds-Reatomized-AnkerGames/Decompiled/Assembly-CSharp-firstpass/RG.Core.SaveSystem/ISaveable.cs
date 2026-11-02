namespace RG.Core.SaveSystem;

public interface ISaveable
{
	string ID { get; }

	string Serialize();

	void Deserialize(string jsonData);

	void Register();

	void Unregister();

	void ResetData();
}
