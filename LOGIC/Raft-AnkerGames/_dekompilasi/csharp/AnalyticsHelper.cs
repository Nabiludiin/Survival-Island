using System.Collections.Generic;
using UnityEngine.Analytics;

public class AnalyticsHelper
{
	public static void CustomEvent(AnalyticsEvent analyticsEvent)
	{
		Analytics.CustomEvent(analyticsEvent.ToString());
	}

	public static void CustomEventString(AnalyticsEvent analyticsEvent, Dictionary<string, object> dict)
	{
		Analytics.CustomEvent(analyticsEvent.ToString(), dict);
	}
}
