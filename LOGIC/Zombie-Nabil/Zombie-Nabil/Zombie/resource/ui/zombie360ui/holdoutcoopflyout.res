"Resource/UI/SurvivalFlyout.res"
{
	"PnlBackground"
	{
		"ControlName"			"Panel"
		"fieldName"				"PnlBackground"
		"xpos"					"0"
		"ypos"					"0"
		"zpos"					"-1"
		"wide"					"180" [$ENGLISH]
		"wide"					"270" [$!ENGLISH]
		"tall"					"85" [$X360]
		"tall"					"105" [$WIN32]
		"visible"				"1"
		"enabled"				"1"
		"paintbackground"		"1"
		"paintborder"			"1"
	}

	"BtnQuickMatch"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnQuickMatch"
		"xpos"					"0"
		"ypos"					"0"
		"wide"					"150"
		"tall"					"20"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"wrap"					"1"
		"navUp"					"BtnPlaySurvivalLeaderboards"
		"navDown"				"BtnPlaySurvivalWithAnyone"
		"labelText"				"#ZOMBIE360UI_QuickMatch"
		"tooltiptext"			"#ZOMBIE360UI_QuickMatch_Survival_Tip"
		"disabled_tooltiptext"	"#ZOMBIE360UI_QuickMatch_Tip_Disabled"
		"style"					"FlyoutMenuButton"
		"command"				"QuickMatch_survival"
	}
	
	"BtnPlaySurvivalWithAnyone"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnPlaySurvivalWithAnyone"
		"xpos"					"0"
		"ypos"					"20"
		"wide"					"150"
		"tall"					"20"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"wrap"					"1"
		"navUp"					"BtnQuickMatch"
		"navDown"				"BtnPlaySurvivalWithFriends"
		"labelText"				"#ZOMBIE360UI_CustomMatch"	[$X360]
		"labelText"				"#ZOMBIE360UI_MainMenu_PlayOnline" [$WIN32]
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_PlaySurvivalWithAnyone_Tip"
		"disabled_tooltiptext"	"#ZOMBIE360UI_MainMenu_PlaySurvivalWithAnyone_Tip_Disabled"
		"style"					"FlyoutMenuButton"
		"command"				"CustomMatch_survival"
	}	
	
	"BtnPlaySurvivalWithFriends"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnPlaySurvivalWithFriends"
		"xpos"					"0"
		"ypos"					"40"
		"wide"					"150"
		"tall"					"20"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"wrap"					"1"
		"navUp"					"BtnPlaySurvivalWithAnyone"
		"navDown"				"BtnPlaySurvivalLeaderboards" [$X360]
		"navDown"				"BtnPlayOnGroupServer" [$WIN32]
		"labelText"				"#ZOMBIE360UI_MainMenu_PlayCoopWithFriends"
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_PlayCoopWithFriends_Tip"
		"disabled_tooltiptext"	"#ZOMBIE360UI_MainMenu_PlayCoopWithFriends_Tip_Disabled"
		"style"					"FlyoutMenuButton"
		"command"				"FriendsMatch_survival"
	}

	"BtnPlayOnGroupServer" [$WIN32]
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnPlayOnGroupServer"
		"xpos"					"0"
		"ypos"					"60"
		"wide"					"150"
		"tall"					"20"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"wrap"					"1"
		"navUp"					"BtnPlaySurvivalWithFriends"
		"navDown"				"BtnPlaySurvivalLeaderboards"
		"labelText"				"#ZOMBIE360UI_MainMenu_PlayOnGroupServer"
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_PlayOnGroupServer_Tip"
		"disabled_tooltiptext"	"#ZOMBIE360UI_MainMenu_PlayOnGroupServer_Tip_Disabled"
		"style"					"FlyoutMenuButton"
		"command"				"GroupServer_survival"
	}

	"BtnPlaySurvivalLeaderboards"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnPlaySurvivalLeaderboards"
		"xpos"					"0"
		"ypos"					"60" [$X360]
		"ypos"					"80" [$WIN32]
		"wide"					"150"
		"tall"					"20"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"wrap"					"1"
		"navUp"					"BtnPlayOnGroupServer" [$WIN32]
		"navUp"					"BtnPlaySurvivalWithFriends" [$X360]
		"navDown"				"BtnQuickMatch"
		"labelText"				"#ZOMBIE360UI_MainMenu_HoldoutCoopLeaderboards"
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_HoldoutCoopLeaderboards_Tip"
		"disabled_tooltiptext"	"#ZOMBIE360UI_MainMenu_HoldoutCoopLeaderboards_Tip_Disabled"
		"style"					"FlyoutMenuButton"
		"command"				"Leaderboards_survival"
	}
}