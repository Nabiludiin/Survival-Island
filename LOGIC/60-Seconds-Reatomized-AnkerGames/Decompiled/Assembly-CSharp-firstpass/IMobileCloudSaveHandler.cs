using System;
using RG.Core.SaveSystem;

public interface IMobileCloudSaveHandler
{
	void LoadFromFile(string fileName, Action<byte[]> result);

	void WriteToFileAsync(byte[] content, string fileName, Action success, Action failed);

	void DeleteAsync(string fileName, Action result);

	void CheckIfFileExists(string fileName, Action<StorageDataManager.ESaveFileExists> result);
}
