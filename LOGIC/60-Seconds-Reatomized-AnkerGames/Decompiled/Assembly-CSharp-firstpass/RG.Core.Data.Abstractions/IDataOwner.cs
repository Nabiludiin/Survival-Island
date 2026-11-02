namespace RG.Core.Data.Abstractions;

public interface IDataOwner<T> where T : DataObject
{
	T Data { set; }

	void ResetData();
}
