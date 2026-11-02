using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityToolbag;

namespace RG.Parsecs.Socials;

public class ImgurClient
{
	public class OnImageUploadedEventArgs : EventArgs
	{
		public ImgurUploadResponse response;

		public bool success;

		public OnImageUploadedEventArgs(bool success, ImgurUploadResponse response)
		{
			this.success = success;
			this.response = response;
		}
	}

	public class OnImageUploadProgressEventArgs : EventArgs
	{
		public float progress;

		public OnImageUploadProgressEventArgs(float progress)
		{
			this.progress = progress;
		}
	}

	private string _title = "60 Parsecs!";

	private readonly string _clientId;

	private readonly string _mashapeKey;

	private const string _baseUploadUrl = "https://imgur-apiv3.p.rapidapi.com/3/image?gamegif";

	private const string _baseUploadUrlCredits = "https://imgur-apiv3.p.rapidapi.com/3/credits";

	private const string _baseApplicationRegisterUrl = "https://api.imgur.com/oauth2/addclient";

	public event EventHandler<OnImageUploadedEventArgs> OnImageUploaded;

	public event EventHandler<OnImageUploadProgressEventArgs> OnImageUploadProgress;

	public ImgurClient(string _clientId, string mashapeKey)
	{
		if (_clientId.Equals(string.Empty))
		{
			Debug.LogError("You need a client ID to use the API! You can get one from here: https://api.imgur.com/oauth2/addclient");
		}
		this._clientId = _clientId;
		_mashapeKey = mashapeKey;
		ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object o, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) => true));
	}

	public ImgurClient(string _clientId, string mashapeKey, string title)
	{
		if (_clientId.Equals(string.Empty))
		{
			Debug.LogError("You need a client ID to use the API! You can get one from here: https://api.imgur.com/oauth2/addclient");
		}
		this._clientId = _clientId;
		_mashapeKey = mashapeKey;
		_title = title;
		ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object o, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) => true));
	}

	public void TakeAndUploadScreenshot(string screenshotName, bool deleteLocalFileAfterUpload)
	{
		if (!Directory.Exists(Application.streamingAssetsPath))
		{
			Directory.CreateDirectory(Application.streamingAssetsPath);
		}
		string filePath = Path.Combine(Application.streamingAssetsPath, screenshotName);
		ScreenCapture.CaptureScreenshot(filePath);
		Thread thread = new Thread(() =>
		{
			while (!File.Exists(filePath))
			{
				Thread.Sleep(10);
			}
			string base64Image = Convert.ToBase64String(File.ReadAllBytes(filePath));
			Upload(base64Image, (bool b, ImgurUploadResponse response) =>
			{
				if (OnImageUploaded != null)
				{
					OnImageUploaded(this, new OnImageUploadedEventArgs(b, response));
				}
				if (deleteLocalFileAfterUpload && File.Exists(filePath))
				{
					File.Delete(filePath);
				}
			}, null);
		});
		thread.IsBackground = true;
		thread.Start();
	}

	public bool UploadImageFromFilePath(string filePath)
	{
		if (!File.Exists(filePath))
		{
			Debug.LogError("[Imgur] That file does not exist: " + filePath);
			return false;
		}
		byte[] inArray = File.ReadAllBytes(filePath);
		UploadImage(Convert.ToBase64String(inArray));
		return true;
	}

	public void UploadImage(string base64Image)
	{
		Upload(base64Image, (bool b, ImgurUploadResponse response) =>
		{
			if (OnImageUploaded != null)
			{
				OnImageUploaded(this, new OnImageUploadedEventArgs(b, response));
			}
		}, (object sender, UploadProgressChangedEventArgs e) =>
		{
			if (OnImageUploadProgress != null)
			{
				OnImageUploadProgress(this, new OnImageUploadProgressEventArgs(e.ProgressPercentage));
			}
		});
	}

	public void CheckCredits()
	{
		Thread thread = new Thread(() =>
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://imgur-apiv3.p.rapidapi.com/3/credits");
			httpWebRequest.Method = "GET";
			httpWebRequest.Headers.Add("Authorization", "Client-ID " + _clientId);
			httpWebRequest.Headers.Add("X-Mashape-Key", _mashapeKey);
			using HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			Stream responseStream = httpWebResponse.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream);
			streamReader.ReadToEnd();
			streamReader.Close();
			responseStream.Close();
		});
		thread.IsBackground = true;
		thread.Start();
	}

	private void Upload(string base64Image, Action<bool, ImgurUploadResponse> OnUploadCompleted, UploadProgressChangedEventHandler OnUploadProgress)
	{
		Dispatcher.RunCoroutine(UploadWithWebRequest(base64Image, OnUploadCompleted, OnUploadProgress));
	}

	private IEnumerator UploadWithWebRequest(string base64Image, Action<bool, ImgurUploadResponse> OnUploadCompleted, UploadProgressChangedEventHandler OnUploadProgress)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("image", base64Image);
		wWWForm.AddField("title", _title);
		UnityWebRequest www = UnityWebRequest.Post("https://imgur-apiv3.p.rapidapi.com/3/image?gamegif", wWWForm);
		www.SetRequestHeader("Authorization", "Client-ID " + _clientId);
		www.SetRequestHeader("X-Mashape-Key", _mashapeKey);
		yield return www.SendWebRequest();
		if (www.responseCode != 200)
		{
			Debug.Log(www.error);
			OnUploadCompleted(arg1: false, null);
		}
		else
		{
			OnUploadCompleted(arg1: true, JsonUtility.FromJson<ImgurUploadResponse>(www.downloadHandler.text));
		}
	}
}
