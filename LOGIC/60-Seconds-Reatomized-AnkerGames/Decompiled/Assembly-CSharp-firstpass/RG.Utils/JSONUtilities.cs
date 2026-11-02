using System.Collections.Generic;

namespace RG.Utils;

public class JSONUtilities
{
	public static Dictionary<string, string> SplitJSON(string json)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		json = json.Remove(0, 1);
		json = json.Remove(json.Length - 1, 1);
		int num = 0;
		while (num < json.Length)
		{
			int num2 = 0;
			num2 = json.IndexOf(':', num);
			string key = json.Substring(num, num2 - num);
			num = json.IndexOf('{', num);
			num2 = num + 1;
			int num3 = 1;
			while (num3 != 0)
			{
				num2 = json.IndexOfAny(new char[2] { '{', '}' }, num2);
				num3 = ((json[num2] != '}') ? (num3 + 1) : (num3 - 1));
				num2++;
				if (num3 == 0)
				{
					num2--;
					string value = json.Substring(num, num2 - num + 1);
					num = num2 + 2;
					dictionary.Add(key, value);
				}
			}
		}
		return dictionary;
	}
}
