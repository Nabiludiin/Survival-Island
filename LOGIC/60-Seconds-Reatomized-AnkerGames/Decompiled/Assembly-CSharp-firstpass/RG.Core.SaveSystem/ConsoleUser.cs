using System;

namespace RG.Core.SaveSystem;

public class ConsoleUser
{
	private Guid _sessionId;

	public int ConsolePlayerId;

	public string ConsolePlayerUID = "";

	private string _consolePlayerName = "";

	public Guid SessionId => _sessionId;

	public string ConsolePlayerName
	{
		get
		{
			return _consolePlayerName;
		}
		set
		{
			_consolePlayerName = value;
		}
	}

	public ConsoleUser(int id = -1)
	{
		_sessionId = default;
		ConsolePlayerId = id;
	}
}
