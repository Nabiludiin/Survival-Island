"Resource/UI/InGameDifficultySelect.res"
{
	"InGameDifficultySelect"
	{
		"ControlName"			"Frame"
		"fieldName"			"InGameDifficultySelect"
		"xpos"				"c-200" [$ENGLISH]
		"xpos"				"c-225" [$!ENGLISH]
		"ypos"				"c-76"
		"wide"				"400" [$ENGLISH]
		"wide"				"450" [$!ENGLISH]
		"tall"				"152"
		"autoResize"			"0"
		"pinCorner"			"0"
		"visible"			"1"
		"enabled"			"1"
		"tabPosition"			"0"
		"usetitlesafe"			"1"
	}
	
	"LblDifficultyTitle"
	{
		"ControlName"				"Label"
		"fieldName"					"LblDifficultyTitle"
		"xpos"						"10"
		"ypos"						"8"
		"wide"						"f0"
		"tall"						"24"
		"wrap"						"1"
		"autoResize"				"1"
		"pinCorner"					"0"
		"visible"					"1"
		"enabled"					"1"
		"tabPosition"				"0"
		"proportionalToParent"		"1"
		"usetitlesafe"				"0"
		"Font"						"FrameTitle"
		"LabelText"					"#ZOMBIE360UI_InGameDifficultySelect"
	}

	"LblDescription"
	{
		"ControlName"				"Label"
		"fieldName"					"LblDescription"
		"xpos"						"10"
		"ypos"						"31"
		"wide"						"f0"
		"tall"						"24"
		"wrap"						"1"
		"autoResize"				"1"
		"pinCorner"					"0"
		"visible"					"0"
		"enabled"					"1"
		"tabPosition"				"0"
		"proportionalToParent"		"1"
		"usetitlesafe"				"0"
		"Font"						"Default"
		"fgcolor_override"          "MediumGray"
		"LabelText"					"#ZOMBIE360UI_Difficulty_Description"
	}
		
	"LblCurrentDifficulty"
	{
		"ControlName"				"Label"
		"fieldName"					"LblCurrentDifficulty"
		"xpos"						"15"
		"ypos"						"34"
		"wide"						"350"
		"tall"						"20"
		"wrap"						"0"
		"autoResize"				"1"
		"pinCorner"					"0"
		"visible"					"1"
		"enabled"					"1"
		"tabPosition"				"0"
		"Font"						"Default"
		"fgcolor_override"          "MediumGray"
		"LabelText"					""
		"textAlignment"				"north-west"
	}

	"BtnEasy"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnEasy"
		"ypos"					"55"
		"xpos"					"15"
		"wide"					"230"
		"tall"					"20"	[$X360]
		"tall"					"15"	[$WIN32]
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"wrap"					"1"
		"navUp"					"BtnCancel"
		"navDown"				"BtnNormal"
		"labelText"				"#ZOMBIE360UI_Difficulty_Easy"
		"tooltiptext"				"#ZOMBIE360UI_Difficulty_Tooltip_Easy"
		"style"					"MainMenuButton"
		"command"				"Easy"
		"proportionalToParent"	"1"
		"usetitlesafe" 			"1"
	}

	"BtnNormal"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnNormal"
		"ypos"					"70"
		"xpos"					"15"
		"wide"					"230"
		"tall"					"20"	[$X360]
		"tall"					"15"	[$WIN32]
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"wrap"					"1"
		"navUp"					"BtnEasy"
		"navDown"				"BtnHard"
		"labelText"				"#ZOMBIE360UI_Difficulty_Normal"
		"tooltiptext"			"#ZOMBIE360UI_Difficulty_Tooltip_Normal"
		"style"					"MainMenuButton"
		"command"				"Normal"
		"proportionalToParent"	"1"
		"usetitlesafe" 			"1"
	}
	
	"BtnHard"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnHard"
		"ypos"					"85"
		"xpos"					"15"
		"wide"					"230"
		"tall"					"20"	[$X360]
		"tall"					"15"	[$WIN32]
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"wrap"					"1"
		"navUp"					"BtnNormal"
		"navDown"				"BtnImpossible"
		"labelText"				"#ZOMBIE360UI_Difficulty_Hard"
		"tooltiptext"			"#ZOMBIE360UI_Difficulty_Tooltip_Hard"
		"style"					"MainMenuButton"
		"command"				"Hard"
		"proportionalToParent"	"1"
		"usetitlesafe" 			"1"
	}
	
	"BtnImpossible"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnImpossible"
		"ypos"					"100"
		"xpos"					"15"
		"wide"					"230"
		"tall"					"20"	[$X360]
		"tall"					"15"	[$WIN32]
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"wrap"					"1"
		"navUp"					"BtnHard"
		"navDown"				"BtnCancel"
		"labelText"				"#ZOMBIE360UI_Difficulty_Impossible"
		"tooltiptext"			"#ZOMBIE360UI_Difficulty_Tooltip_Impossible"
		"style"					"MainMenuButton"
		"command"				"Impossible"
		"proportionalToParent"	"1"
		"usetitlesafe" 			"1"
	}
	
	"BtnCancel"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnCancel"
		"ypos"					"125"
		"xpos"					"15"
		"wide"					"230"
		"tall"					"20"	[$X360]
		"tall"					"15"	[$WIN32]
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"wrap"					"1"
		"navUp"					"BtnImpossible"
		"navDown"				"BtnEasy"
		"labelText"				"#ZOMBIE360UI_Cancel"
		"tooltiptext"			"#ZOMBIE360UI_Tooltip_Back"
		"style"					"MainMenuButton"
		"command"				"Cancel"
		"proportionalToParent"	"1"
		"usetitlesafe" 			"1"
	}
}