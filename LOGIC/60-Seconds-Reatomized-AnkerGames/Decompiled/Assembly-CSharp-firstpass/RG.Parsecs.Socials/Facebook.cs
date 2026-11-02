using UnityEngine;

namespace RG.Parsecs.Socials;

public class Facebook
{
	private const string FACEBOOK_APP_ID = "347312312500579";

	private const string FACEBOOK_URL = "http://www.facebook.com/dialog/feed";

	private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";

	private const string TWEET_LANGUAGE = "en";

	public static void Share(string linkParameter, string nameParameter, string captionParameter, string descriptionParameter, string pictureParameter, string redirectParameter)
	{
		Application.OpenURL("http://www.facebook.com/dialog/feed?app_id=347312312500579&link=" + WWW.EscapeURL(linkParameter) + "&name=" + WWW.EscapeURL(nameParameter) + "&caption=" + WWW.EscapeURL(captionParameter) + "&description=" + WWW.EscapeURL(descriptionParameter) + "&picture=" + WWW.EscapeURL(pictureParameter) + "&redirect_uri=" + WWW.EscapeURL(redirectParameter));
	}

	public static void Share(string appIdParameter, string linkParameter, string nameParameter, string captionParameter, string descriptionParameter, string pictureParameter, string redirectParameter)
	{
		Application.OpenURL("http://www.facebook.com/dialog/feed?app_id=" + appIdParameter + "&link=" + WWW.EscapeURL(linkParameter) + "&name=" + WWW.EscapeURL(nameParameter) + "&caption=" + WWW.EscapeURL(captionParameter) + "&description=" + WWW.EscapeURL(descriptionParameter) + "&picture=" + WWW.EscapeURL(pictureParameter) + "&redirect_uri=" + WWW.EscapeURL(redirectParameter));
	}
}
