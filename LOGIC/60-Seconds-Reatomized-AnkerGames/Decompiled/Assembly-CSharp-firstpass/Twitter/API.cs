using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using RG.Parsecs.Scavenge;
using RestSharp.Contrib;
using UnityEngine;
using UnityEngine.Networking;

namespace Twitter;

public class API
{
	public class GIFTweet
	{
		[Serializable]
		public class UploadInitResponse
		{
			public string media_id_string;

			public float expires_after_secs;
		}

		[Serializable]
		public class UploadFinalizeResponse
		{
			public processing_info processing_info;

			public string media_id_string;

			public float expires_after_secs;
		}

		[Serializable]
		public class processing_info
		{
			public int check_after_secs;

			public string state;
		}

		[Serializable]
		public class UploadCheckStatusResponse
		{
			public processing_info processing_info;

			public string media_id_string;

			public float expires_after_secs;
		}

		[Serializable]
		public class ErrorResponse
		{
			public List<TwitterError> errors;
		}

		[Serializable]
		public class TwitterError
		{
			public int code;

			public string message;
		}

		private const string MediaEndpointURL = "https://upload.twitter.com/1.1/media/upload.json";

		public GenericEvent<float> OnUploadProgressChanged = new GenericEvent<float>();

		public GenericEvent<bool> OnUploadFinished = new GenericEvent<bool>();

		public GenericEvent<bool> OnTweetPosted = new GenericEvent<bool>();

		private string _mediaID;

		private string _filePath;

		private string _text;

		private string _consumerKey;

		private string _consumerSecret;

		private AccessTokenResponse _response;

		private PostTweetCallback _callback;

		private FileInfo _fileInfo;

		private long _total_bytes;

		private bool _errorEncounterd;

		public GIFTweet(string filePath, string consumerKey, string consumerSecret, AccessTokenResponse response, PostTweetCallback callback)
		{
			_filePath = filePath;
			_consumerKey = consumerKey;
			_consumerSecret = consumerSecret;
			_response = response;
			_callback = callback;
		}

		public IEnumerator Upload()
		{
			FileInfo fileInfo = new FileInfo(_filePath);
			if (!fileInfo.Exists)
			{
				Debug.Log(string.Format("PostTweet - file does not exist", _filePath));
				_callback(success: false, -1);
				yield break;
			}
			_total_bytes = fileInfo.Length;
			OnUploadProgressChanged.Invoke(0.05f);
			yield return UploadInit();
			if (_errorEncounterd)
			{
				OnUploadFinished.Invoke(arg: false);
				yield break;
			}
			OnUploadProgressChanged.Invoke(0.1f);
			yield return UploadAppend();
			OnUploadProgressChanged.Invoke(0.9f);
			if (_errorEncounterd)
			{
				OnUploadFinished.Invoke(arg: false);
				yield break;
			}
			yield return UploadFinalize();
			OnUploadProgressChanged.Invoke(1f);
			OnUploadFinished.Invoke(arg: true);
		}

		public IEnumerator Tweet(string text)
		{
			if (_errorEncounterd)
			{
				Debug.Log($"Tweet - encountered an error but still trying to tweet");
				yield break;
			}
			_text = text;
			if (string.IsNullOrEmpty(_text) || _text.Length > 140)
			{
				Debug.Log($"PostTweet - text[{_text}] is empty or too long.");
				_callback(success: false, -1);
			}
			else
			{
				yield return Post();
			}
		}

		public IEnumerator UploadAndTweet(string text)
		{
			_text = text;
			if (string.IsNullOrEmpty(_text) || _text.Length > 140)
			{
				Debug.Log($"PostTweet - text[{_text}] is empty or too long.");
				_callback(success: false, -1);
				yield break;
			}
			FileInfo fileInfo = new FileInfo(_filePath);
			if (!fileInfo.Exists)
			{
				Debug.Log(string.Format("PostTweet - file does not exist", _filePath));
				_callback(success: false, -1);
				yield break;
			}
			_total_bytes = fileInfo.Length;
			yield return UploadInit();
			if (_errorEncounterd)
			{
				yield break;
			}
			yield return UploadAppend();
			if (!_errorEncounterd)
			{
				yield return UploadFinalize();
				if (!_errorEncounterd)
				{
					yield return Post();
				}
			}
		}

		private IEnumerator UploadInit()
		{
			string value = _total_bytes.ToString();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("command", "INIT");
			dictionary.Add("media_type", "image/gif");
			dictionary.Add("total_bytes", value);
			dictionary.Add("media_category", "tweet_gif");
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("command", "INIT");
			wWWForm.AddField("media_type", "image/gif");
			wWWForm.AddField("total_bytes", value);
			wWWForm.AddField("media_category", "tweet_gif");
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			dictionary2["Authorization"] = GetHeaderWithAccessToken("POST", "https://upload.twitter.com/1.1/media/upload.json", _consumerKey, _consumerSecret, _response, dictionary);
			WWW web = new WWW("https://upload.twitter.com/1.1/media/upload.json", wWWForm.data, dictionary2);
			yield return web;
			if (!string.IsNullOrEmpty(web.error))
			{
				Debug.Log($"PostTweet - UploadInit - failed. {web.error}\n{web.text}");
				ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(web.text);
				if (errorResponse != null && errorResponse.errors.Count > 0)
				{
					int errorCode = ((errorResponse.errors[0].code != 32) ? errorResponse.errors[0].code : 32);
					_callback(success: false, errorCode);
				}
				else if (web.error == "Cannot resolve destination host")
				{
					_callback(success: false, -1);
				}
				else
				{
					_callback(success: false, -1);
				}
				_errorEncounterd = true;
			}
			else
			{
				string value2 = Regex.Match(web.text, "<error>([^&]+)</error>").Groups[1].Value;
				if (!string.IsNullOrEmpty(value2))
				{
					Debug.Log($"PostTweet - UploadInit - failed. {value2}");
					_callback(success: false, -1);
					_errorEncounterd = true;
				}
				else
				{
					UploadInitResponse uploadInitResponse = JsonUtility.FromJson<UploadInitResponse>(web.text);
					_mediaID = uploadInitResponse.media_id_string;
					Debug.LogFormat("UploadInit _mediaID: {0}", _mediaID);
				}
			}
		}

		private IEnumerator UploadAppend()
		{
			int segment_id = 0;
			long bytes_sent = 0L;
			int chunkMaxSize = 4194304;
			byte[] chunk = new byte[chunkMaxSize];
			using (FileStream stream = File.OpenRead(_filePath))
			{
				while (stream.Position < stream.Length)
				{
					OnUploadProgressChanged.Invoke(0.1f + 0.8f * ((float)stream.Position / (float)stream.Length));
					int num = stream.Read(chunk, 0, chunkMaxSize);
					bytes_sent += num;
					if (num < chunkMaxSize)
					{
						Array.Resize(ref chunk, num);
					}
					Dictionary<string, string> parameters = new Dictionary<string, string>();
					WWWForm wWWForm = new WWWForm();
					wWWForm.AddField("command", "APPEND");
					wWWForm.AddField("media_id", _mediaID);
					wWWForm.AddField("segment_index", segment_id.ToString());
					wWWForm.AddBinaryData("media", chunk);
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary["Authorization"] = GetHeaderWithAccessToken("POST", "https://upload.twitter.com/1.1/media/upload.json", _consumerKey, _consumerSecret, _response, parameters);
					foreach (KeyValuePair<string, string> header in wWWForm.headers)
					{
						dictionary.Add(header.Key, header.Value);
					}
					WWW web = new WWW("https://upload.twitter.com/1.1/media/upload.json", wWWForm.data, dictionary);
					yield return web;
					if (!string.IsNullOrEmpty(web.error))
					{
						Debug.Log($"PostTweet - UploadAppend - failed. {web.error}\n{web.text}");
						_callback(success: false, -1);
						_errorEncounterd = true;
						yield break;
					}
					string value = Regex.Match(web.text, "<error>([^&]+)</error>").Groups[1].Value;
					if (!string.IsNullOrEmpty(value))
					{
						Debug.Log($"PostTweet - UploadAppend - failed. {value}");
						_callback(success: false, -1);
						_errorEncounterd = true;
						yield break;
					}
					Debug.LogFormat("Append Chunk {0} success", segment_id);
					segment_id++;
				}
			}
			yield return null;
		}

		private IEnumerator UploadFinalize()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("command", "FINALIZE");
			dictionary.Add("media_id", _mediaID);
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("command", "FINALIZE");
			wWWForm.AddField("media_id", _mediaID);
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			dictionary2["Authorization"] = GetHeaderWithAccessToken("POST", "https://upload.twitter.com/1.1/media/upload.json", _consumerKey, _consumerSecret, _response, dictionary);
			WWW web = new WWW("https://upload.twitter.com/1.1/media/upload.json", wWWForm.data, dictionary2);
			yield return web;
			if (!string.IsNullOrEmpty(web.error))
			{
				Debug.Log($"PostTweet - UploadFinalize - failed. {web.error}\n{web.text}");
				_callback(success: false, -1);
				_errorEncounterd = true;
				yield break;
			}
			string value = Regex.Match(web.text, "<error>([^&]+)</error>").Groups[1].Value;
			if (!string.IsNullOrEmpty(value))
			{
				Debug.Log($"PostTweet - UploadFinalize - failed. {value}");
				_callback(success: false, -1);
				_errorEncounterd = true;
				yield break;
			}
			UploadFinalizeResponse uploadFinalizeResponse = JsonUtility.FromJson<UploadFinalizeResponse>(web.text);
			_mediaID = uploadFinalizeResponse.media_id_string;
			Debug.LogFormat("UploadFinalize _mediaID: {0}", _mediaID);
			if (uploadFinalizeResponse.processing_info != null && !(uploadFinalizeResponse.processing_info.state == "succeeded"))
			{
				yield return new WaitForSeconds(uploadFinalizeResponse.processing_info.check_after_secs);
				yield return CheckStatus();
			}
			yield return null;
		}

		private IEnumerator CheckStatus()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("command", "STATUS");
			dictionary.Add("media_id", _mediaID);
			UriBuilder uriBuilder = new UriBuilder("https://upload.twitter.com/1.1/media/upload.json")
			{
				Port = -1
			};
			HttpUtility.HttpQSCollection httpQSCollection = HttpUtility.ParseQueryString(uriBuilder.Query);
			httpQSCollection["command"] = "STATUS";
			httpQSCollection["media_id"] = _mediaID;
			uriBuilder.Query = httpQSCollection.ToString();
			string text = uriBuilder.ToString();
			Debug.LogFormat("CheckStatus URL: {0}", text);
			UnityWebRequest web = UnityWebRequest.Get(text);
			web.SetRequestHeader("Authorization", GetHeaderWithAccessToken("GET", "https://upload.twitter.com/1.1/media/upload.json", _consumerKey, _consumerSecret, _response, dictionary));
			yield return web.SendWebRequest();
			if (!string.IsNullOrEmpty(web.error))
			{
				Debug.Log($"PostTweet - CheckStatus - failed. {web.error}\n{web.downloadHandler.text}");
				_callback(success: false, -1);
				_errorEncounterd = true;
				yield break;
			}
			string value = Regex.Match(web.downloadHandler.text, "<error>([^&]+)</error>").Groups[1].Value;
			if (!string.IsNullOrEmpty(value))
			{
				Debug.Log($"PostTweet - CheckStatus - failed. {value}");
				_callback(success: false, -1);
				_errorEncounterd = true;
				yield break;
			}
			UploadFinalizeResponse uploadFinalizeResponse = JsonUtility.FromJson<UploadFinalizeResponse>(web.downloadHandler.text);
			_mediaID = uploadFinalizeResponse.media_id_string;
			if (uploadFinalizeResponse.processing_info != null && !(uploadFinalizeResponse.processing_info.state == "succeeded"))
			{
				if (uploadFinalizeResponse.processing_info.state == "failed")
				{
					Debug.Log($"PostTweet - CheckStatus - failed. {value}");
					_callback(success: false, -1);
					_errorEncounterd = true;
					yield break;
				}
				yield return new WaitForSeconds(uploadFinalizeResponse.processing_info.check_after_secs);
				yield return CheckStatus();
			}
			yield return null;
		}

		private IEnumerator Post()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("status", _text);
			dictionary.Add("media_ids", _mediaID);
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("status", _text);
			wWWForm.AddField("media_ids", _mediaID);
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			dictionary2["Authorization"] = GetHeaderWithAccessToken("POST", "https://api.twitter.com/1.1/statuses/update.json", _consumerKey, _consumerSecret, _response, dictionary);
			WWW web = new WWW("https://api.twitter.com/1.1/statuses/update.json", wWWForm.data, dictionary2);
			yield return web;
			if (!string.IsNullOrEmpty(web.error))
			{
				Debug.Log($"PostTweet - failed. {web.error}\n{web.text}");
				_callback(success: false, -1);
				OnTweetPosted.Invoke(arg: false);
				yield break;
			}
			string value = Regex.Match(web.text, "<error>([^&]+)</error>").Groups[1].Value;
			if (!string.IsNullOrEmpty(value))
			{
				Debug.Log($"PostTweet - failed. {value}");
				_callback(success: false, -1);
				OnTweetPosted.Invoke(arg: false);
			}
			else
			{
				_callback(success: true, -1);
				OnTweetPosted.Invoke(arg: true);
			}
		}
	}

	private static readonly string RequestTokenURL = "https://api.twitter.com/oauth/request_token";

	private static readonly string AuthorizationURL = "https://api.twitter.com/oauth/authenticate?oauth_token={0}";

	private static readonly string AccessTokenURL = "https://api.twitter.com/oauth/access_token";

	private const string PostTweetURL = "https://api.twitter.com/1.1/statuses/update.json";

	private static readonly string[] OAuthParametersToIncludeInHeader = new string[7] { "oauth_version", "oauth_nonce", "oauth_timestamp", "oauth_signature_method", "oauth_consumer_key", "oauth_token", "oauth_verifier" };

	private static readonly string[] SecretParameters = new string[3] { "oauth_consumer_secret", "oauth_token_secret", "oauth_signature" };

	public static IEnumerator GetRequestToken(string consumerKey, string consumerSecret, RequestTokenCallback callback)
	{
		WWW web = WWWRequestToken(consumerKey, consumerSecret);
		yield return web;
		if (!string.IsNullOrEmpty(web.error))
		{
			Debug.Log($"GetRequestToken - failed. error : {web.error}");
			callback(success: false, null);
			yield break;
		}
		RequestTokenResponse requestTokenResponse = new RequestTokenResponse
		{
			Token = Regex.Match(web.text, "oauth_token=([^&]+)").Groups[1].Value,
			TokenSecret = Regex.Match(web.text, "oauth_token_secret=([^&]+)").Groups[1].Value
		};
		if (!string.IsNullOrEmpty(requestTokenResponse.Token) && !string.IsNullOrEmpty(requestTokenResponse.TokenSecret))
		{
			callback(success: true, requestTokenResponse);
			yield break;
		}
		Debug.Log($"GetRequestToken - failed. response : {web.text}");
		callback(success: false, null);
	}

	public static void OpenAuthorizationPage(string requestToken)
	{
		Application.OpenURL(string.Format(AuthorizationURL, requestToken));
	}

	public static IEnumerator GetAccessToken(string consumerKey, string consumerSecret, string requestToken, string pin, AccessTokenCallback callback)
	{
		WWW web = WWWAccessToken(consumerKey, consumerSecret, requestToken, pin);
		yield return web;
		if (!string.IsNullOrEmpty(web.error))
		{
			Debug.Log($"GetAccessToken - failed. error : {web.error}");
			callback(success: false, null);
			yield break;
		}
		AccessTokenResponse accessTokenResponse = new AccessTokenResponse
		{
			Token = Regex.Match(web.text, "oauth_token=([^&]+)").Groups[1].Value,
			TokenSecret = Regex.Match(web.text, "oauth_token_secret=([^&]+)").Groups[1].Value,
			UserId = Regex.Match(web.text, "user_id=([^&]+)").Groups[1].Value,
			ScreenName = Regex.Match(web.text, "screen_name=([^&]+)").Groups[1].Value
		};
		if (!string.IsNullOrEmpty(accessTokenResponse.Token) && !string.IsNullOrEmpty(accessTokenResponse.TokenSecret) && !string.IsNullOrEmpty(accessTokenResponse.UserId) && !string.IsNullOrEmpty(accessTokenResponse.ScreenName))
		{
			callback(success: true, accessTokenResponse);
			yield break;
		}
		Debug.Log($"GetAccessToken - failed. response : {web.text}");
		callback(success: false, null);
	}

	private static WWW WWWRequestToken(string consumerKey, string consumerSecret)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("oauth_callback", "oob");
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		AddDefaultOAuthParams(dictionary, consumerKey, consumerSecret);
		dictionary.Add("oauth_callback", "oob");
		Hashtable hashtable = new Hashtable();
		hashtable["Authorization"] = GetFinalOAuthHeader("POST", RequestTokenURL, dictionary);
		return new WWW(RequestTokenURL, wWWForm.data, hashtable);
	}

	private static WWW WWWAccessToken(string consumerKey, string consumerSecret, string requestToken, string pin)
	{
		byte[] postData = new byte[1] { 0 };
		Hashtable hashtable = new Hashtable();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		AddDefaultOAuthParams(dictionary, consumerKey, consumerSecret);
		dictionary.Add("oauth_token", requestToken);
		dictionary.Add("oauth_verifier", pin);
		hashtable["Authorization"] = GetFinalOAuthHeader("POST", AccessTokenURL, dictionary);
		return new WWW(AccessTokenURL, postData, hashtable);
	}

	private static string GetHeaderWithAccessToken(string httpRequestType, string apiURL, string consumerKey, string consumerSecret, AccessTokenResponse response, Dictionary<string, string> parameters)
	{
		AddDefaultOAuthParams(parameters, consumerKey, consumerSecret);
		parameters.Add("oauth_token", response.Token);
		parameters.Add("oauth_token_secret", response.TokenSecret);
		return GetFinalOAuthHeader(httpRequestType, apiURL, parameters);
	}

	public static IEnumerator PostTweet(string text, string consumerKey, string consumerSecret, AccessTokenResponse response, PostTweetCallback callback)
	{
		if (string.IsNullOrEmpty(text) || text.Length > 140)
		{
			Debug.Log($"PostTweet - text[{text}] is empty or too long.");
			callback(success: false, -1);
			yield break;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("status", text);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("status", text);
		Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
		dictionary2["Authorization"] = GetHeaderWithAccessToken("POST", "https://api.twitter.com/1.1/statuses/update.json", consumerKey, consumerSecret, response, dictionary);
		WWW web = new WWW("https://api.twitter.com/1.1/statuses/update.json", wWWForm.data, dictionary2);
		yield return web;
		if (!string.IsNullOrEmpty(web.error))
		{
			Debug.Log($"PostTweet - failed. {web.error}\n{web.text}");
			callback(success: false, -1);
			yield break;
		}
		string value = Regex.Match(web.text, "<error>([^&]+)</error>").Groups[1].Value;
		if (!string.IsNullOrEmpty(value))
		{
			Debug.Log($"PostTweet - failed. {value}");
			callback(success: false, -1);
		}
		else
		{
			callback(success: true, 0);
		}
	}

	public static IEnumerator PostGIFTweet(string filePath, string text, string consumerKey, string consumerSecret, AccessTokenResponse response, PostTweetCallback callback)
	{
		if (string.IsNullOrEmpty(text) || text.Length > 140)
		{
			Debug.Log($"PostTweet - text[{text}] is empty or too long.");
			callback(success: false, -1);
		}
		else
		{
			GIFTweet gIFTweet = new GIFTweet(filePath, consumerKey, consumerSecret, response, callback);
			yield return gIFTweet.UploadAndTweet(text);
		}
	}

	public static GIFTweet PrepareGIFTweet(string filePath, string consumerKey, string consumerSecret, AccessTokenResponse response, PostTweetCallback callback)
	{
		return new GIFTweet(filePath, consumerKey, consumerSecret, response, callback);
	}

	private static void AddDefaultOAuthParams(Dictionary<string, string> parameters, string consumerKey, string consumerSecret)
	{
		parameters.Add("oauth_version", "1.0");
		parameters.Add("oauth_nonce", GenerateNonce());
		parameters.Add("oauth_timestamp", GenerateTimeStamp());
		parameters.Add("oauth_signature_method", "HMAC-SHA1");
		parameters.Add("oauth_consumer_key", consumerKey);
		parameters.Add("oauth_consumer_secret", consumerSecret);
	}

	private static string GetFinalOAuthHeader(string HTTPRequestType, string URL, Dictionary<string, string> parameters)
	{
		string value = GenerateSignature(HTTPRequestType, URL, parameters);
		parameters.Add("oauth_signature", value);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("OAuth realm=\"{0}\"", "Twitter API");
		foreach (KeyValuePair<string, string> item in parameters.Where((KeyValuePair<string, string> p) =>
		{
			string[] oAuthParametersToIncludeInHeader = OAuthParametersToIncludeInHeader;
			KeyValuePair<string, string> keyValuePair = p;
			return oAuthParametersToIncludeInHeader.Contains(keyValuePair.Key);
		}).OrderBy((KeyValuePair<string, string> p) =>
		{
			KeyValuePair<string, string> keyValuePair = p;
			return keyValuePair.Key;
		}).ThenBy((KeyValuePair<string, string> p) =>
		{
			KeyValuePair<string, string> keyValuePair = p;
			return UrlEncode(keyValuePair.Value);
		}))
		{
			stringBuilder.AppendFormat(",{0}=\"{1}\"", UrlEncode(item.Key), UrlEncode(item.Value));
		}
		stringBuilder.AppendFormat(",oauth_signature=\"{0}\"", UrlEncode(parameters["oauth_signature"]));
		return stringBuilder.ToString();
	}

	private static string GenerateSignature(string httpMethod, string url, Dictionary<string, string> parameters)
	{
		IEnumerable<KeyValuePair<string, string>> parameters2 = parameters.Where((KeyValuePair<string, string> p) =>
		{
			string[] secretParameters = SecretParameters;
			KeyValuePair<string, string> keyValuePair = p;
			return !secretParameters.Contains(keyValuePair.Key);
		});
		string s = string.Format(CultureInfo.InvariantCulture, "{0}&{1}&{2}", httpMethod, UrlEncode(NormalizeUrl(new Uri(url))), UrlEncode(parameters2));
		string s2 = string.Format(CultureInfo.InvariantCulture, "{0}&{1}", UrlEncode(parameters["oauth_consumer_secret"]), parameters.ContainsKey("oauth_token_secret") ? UrlEncode(parameters["oauth_token_secret"]) : string.Empty);
		return Convert.ToBase64String(new HMACSHA1(Encoding.ASCII.GetBytes(s2)).ComputeHash(Encoding.ASCII.GetBytes(s)));
	}

	private static string GenerateTimeStamp()
	{
		return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
	}

	private static string GenerateNonce()
	{
		return new System.Random().Next(123400, int.MaxValue).ToString("X", CultureInfo.InvariantCulture);
	}

	private static string NormalizeUrl(Uri url)
	{
		string text = string.Format(CultureInfo.InvariantCulture, "{0}://{1}", url.Scheme, url.Host);
		if ((!(url.Scheme == "http") || url.Port != 80) && (!(url.Scheme == "https") || url.Port != 443))
		{
			text = text + ":" + url.Port;
		}
		return text + url.AbsolutePath;
	}

	private static string UrlEncode(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return string.Empty;
		}
		value = Uri.EscapeDataString(value);
		value = Regex.Replace(value, "(%[0-9a-f][0-9a-f])", (Match c) => c.Value.ToUpper());
		value = value.Replace("(", "%28").Replace(")", "%29").Replace("$", "%24")
			.Replace("!", "%21")
			.Replace("*", "%2A")
			.Replace("'", "%27");
		value = value.Replace("%7E", "~");
		return value;
	}

	private static string UrlEncode(IEnumerable<KeyValuePair<string, string>> parameters)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<string, string> item in parameters.OrderBy((KeyValuePair<string, string> p) =>
		{
			KeyValuePair<string, string> keyValuePair = p;
			return keyValuePair.Key;
		}).ThenBy((KeyValuePair<string, string> p) =>
		{
			KeyValuePair<string, string> keyValuePair = p;
			return keyValuePair.Value;
		}))
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append("&");
			}
			stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0}={1}", UrlEncode(item.Key), UrlEncode(item.Value)));
		}
		return UrlEncode(stringBuilder.ToString());
	}
}
