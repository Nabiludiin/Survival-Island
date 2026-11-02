using System.Text;
using I2.Loc;
using RG.Parsecs.Survival;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Common;

public class CharacterTooltipContentHandler : TooltipContentHandler
{
	[Tooltip("Text handler of tooltip.")]
	[SerializeField]
	private TextMeshProUGUI _text;

	[Tooltip("Hex RGB code for color representing positive statuses, like #00FF00")]
	[SerializeField]
	private LocalizedString _positiveColor;

	[Tooltip("Hex RGB code for color representing negative statuses, like #FF0000")]
	[SerializeField]
	private LocalizedString _negativeColor;

	[Tooltip("Hex RGB code for color representing warning statuses, like #FF0000")]
	[SerializeField]
	private LocalizedString _warningColor;

	[SerializeField]
	private bool _specialHandlingOfDeadCharacterStatus = true;

	[SerializeField]
	private bool _useFullCharacterName = true;

	[Tooltip("The 'dead' character status. This needs to be set so that if the caracter is dead that will be the only status displayed")]
	[SerializeField]
	private CharacterStatus _deadCharacterStatus;

	public override void HandleContent(TooltipContent content)
	{
		if ((bool)(content as CharacterTooltipContent))
		{
			HandleCharacterTooltipContent((CharacterTooltipContent)content);
		}
	}

	private void HandleCharacterTooltipContent(CharacterTooltipContent content)
	{
		if (!(_text != null) || !(content.Character != null))
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (_useFullCharacterName)
		{
			stringBuilder.AppendLine(content.Character.StaticData.FullName);
		}
		else
		{
			stringBuilder.AppendLine(content.Character.StaticData.Name);
		}
		bool flag = false;
		if (content.Character.RuntimeData.CurrentStatuses != null)
		{
			if (_specialHandlingOfDeadCharacterStatus && _deadCharacterStatus != null && content.Character.RuntimeData.CurrentStatuses.Contains(_deadCharacterStatus))
			{
				stringBuilder.AppendLine(string.Empty);
				stringBuilder.AppendLine(string.Format("<color={1}>{0}</color>", _deadCharacterStatus.Name, _negativeColor));
			}
			else
			{
				for (int i = 0; i < content.Character.RuntimeData.CurrentStatuses.Count; i++)
				{
					if (content.Character.RuntimeData.CurrentStatuses[i].IsVisibleInUI)
					{
						if (!flag)
						{
							stringBuilder.AppendLine(string.Empty);
							flag = true;
						}
						if (content.Character.RuntimeData.CurrentStatuses[i].IsPositive && content.Character.RuntimeData.CurrentStatuses[i].IsNegative)
						{
							stringBuilder.AppendLine(string.Format("<color={1}>{0}</color>", content.Character.RuntimeData.CurrentStatuses[i].Name, _warningColor));
						}
						else if (content.Character.RuntimeData.CurrentStatuses[i].IsPositive)
						{
							stringBuilder.AppendLine(string.Format("<color={1}>{0}</color>", content.Character.RuntimeData.CurrentStatuses[i].Name, _positiveColor));
						}
						else if (content.Character.RuntimeData.CurrentStatuses[i].IsNegative)
						{
							stringBuilder.AppendLine(string.Format("<color={1}>{0}</color>", content.Character.RuntimeData.CurrentStatuses[i].Name, _negativeColor));
						}
						else
						{
							stringBuilder.AppendLine(content.Character.RuntimeData.CurrentStatuses[i].Name);
						}
					}
				}
			}
		}
		_text.text = stringBuilder.ToString();
	}
}
