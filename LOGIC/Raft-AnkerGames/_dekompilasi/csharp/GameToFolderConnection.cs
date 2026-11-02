using System.IO;

public class GameToFolderConnection
{
	public RGD_Game rgdGame;

	public DirectoryInfo directoryInfo;

	public DirectoryInfo gameDirectoryInfo;

	public long InGameTicks
	{
		get
		{
			if (rgdGame != null)
			{
				return rgdGame.lastPlayedDateTicks;
			}
			if (gameDirectoryInfo != null && SaveAndLoad.ConvertBackupFolderNameToDateTime(gameDirectoryInfo.Name, out var dateTime))
			{
				return dateTime.Ticks;
			}
			return -1L;
		}
	}
}
