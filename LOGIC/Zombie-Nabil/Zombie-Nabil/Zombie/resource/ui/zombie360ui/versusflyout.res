"Resource/UI/VersusFlyout.res"
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
		"navUp"					"BtnPlayVersusWithFriends"	[$X360]
		"navUp"					"BtnPlayOnGroupServer"		[$WIN32]
		"navDown"				"BtnPlayVersusWithAnyone"
		"labelText"				"#ZOMBIE360UI_QuickMatch"
		"tooltiptext"			"#ZOMBIE360UI_QuickMatch_Versus_Tip"
		"disabled_tooltiptext"	"#ZOMBIE360UI_QuickMatch_Tip_Disabled"
		"style"					"FlyoutMenuButton"
		"command"				"QuickMatch_versus"
	}
	
	"BtnPlayVersusWithAnyone"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnPlayVersusWithAnyone"
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
		"navDown"				"BtnPlayVersusWithFriends"
		"labelText"				"#ZOMBIE360UI_CustomMatch"	[$X360]
		"labelText"				"#ZOMBIE360UI_MainMenu_PlayOnline" [$WIN32]
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_Versus_Tip"
		"disabled_tooltiptext"	"#ZOMBIE360UI_MainMenu_PlayCoopWithAnyone_Tip_Disabled"
		"style"					"FlyoutMenuButton"
		"command"				"CustomMatch_versus"
	}	
	
	"BtnPlayVersusWithFriends"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnPlayVersusWithFriends"
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
		"navUp"					"BtnPlayVersusWithAnyone"
		"navDown"				"BtnPlayVersusWithTeam"
		"labelText"				"#ZOMBIE360UI_MainMenu_PlayCoopWithFriends"
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_PlayCoopWithFriends_Tip"
		"disabled_tooltiptext"	"#ZOMBIE360UI_MainMenu_PlayCoopWithFriends_Tip_Disabled"
		"style"					"FlyoutMenuButton"
		"command"				"FriendsMatch_versus"
	}
	
	"BtnPlayVersusWithTeam"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnPlayVersusWithTeam"
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
		"navUp"					"BtnPlayVersusWithFriends"
		"navDown"				"BtnPlayOnGroupServer"
		"labelText"				"#ZOMBIE360UI_MainMenu_PlayTeamVersus"
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_PlayTeam_Tip"
		"disabled_tooltiptext"	"#ZOMBIE360UI_MainMenu_PlayTeam_Tip_Disabled"
		"style"					"FlyoutMenuButton"
		"command"				"StartGameMode_teamversus"
	}

	"BtnPlayOnGroupServer" [$WIN32]
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnPlayOnGroupServer"
		"xpos"					"0"
		"ypos"					"80"
		"wide"					"150"
		"tall"					"20"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"wrap"					"1"
		"navUp"					"BtnPlayVersusWithTeam"
		"navDown"				"BtnQuickMatch"
		"labelText"				"#ZOMBIE360UI_MainMenu_PlayOnGroupServer"
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_PlayOnGroupServer_Tip"
		"disabled_tooltiptext"	"#ZOMBIE360UI_MainMenu_PlayOnGroupServer_Tip_Disabled"
		"style"					"FlyoutMenuButton"
		"command"				"GroupServer_versus"
	}
}