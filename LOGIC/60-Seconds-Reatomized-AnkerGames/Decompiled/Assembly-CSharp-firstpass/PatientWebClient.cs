using System;
using System.Net;

public class PatientWebClient : WebClient
{
	protected override WebRequest GetWebRequest(Uri uri)
	{
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
		WebRequest webRequest = base.GetWebRequest(uri);
		webRequest.Timeout = 1200000;
		return webRequest;
	}
}
