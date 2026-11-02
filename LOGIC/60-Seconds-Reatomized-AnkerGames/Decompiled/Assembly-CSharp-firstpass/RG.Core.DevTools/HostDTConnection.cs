using UnityEngine;
using UnityEngine.Networking;

namespace RG.Core.DevTools;

public class HostDTConnection : MonoBehaviour, IDTConnection
{
	public int ServerPort = 9999;

	public const int MAX_CONNECTIONS = 5;

	private NetworkServerSimple _server;

	private bool _isInitialized;

	private static HostDTConnection _instance;

	private static int _fragmentedReliableChannelId;

	private static HostTopology _connectionConfig;
}
