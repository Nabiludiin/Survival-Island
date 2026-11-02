"Resource/UI/MainMenu.res"
{
	"MainMenu"
	{
		"ControlName"			"Frame"
		"fieldName"				"MainMenu"
		"xpos"					"0"
		"ypos"					"0"
		"wide"					"f0"
		"tall"					"f0"
		"autoResize"			"0"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"PaintBackgroundType"	"0"
	}
				
	// Play a campaign
	"BtnCoOp"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnCoOp"
		"xpos"					"100"
		"ypos"					"100"	[$X360]
		"ypos"					"120"	[$WIN32]
		"wide"					"180"	[$X360]
		"wide"					"180"	[$WIN32]
		"tall"					"20"	[$X360]
		"tall"					"15"	[$WIN32]
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"1"
		"navUp"					"BtnExtras" [$X360]
		"navUp"					"PnlQuickJoin" [$WIN32]
		"navDown"				"BtnVersus"
		"labelText"				"#ZOMBIE360UI_MainMenu_CoOp"
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_CoOp_Tip"
		"style"					"MainMenuButton"
		"command"				"FlmCampaignFlyout"
		"ActivationType"		"1"
	}
	
	// Play versus
	"BtnVersus"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnVersus"
		"xpos"					"100"
		"ypos"					"125"	[$X360]
		"ypos"					"135"	[$WIN32]
		"wide"					"180"	[$X360]
		"wide"					"180"	[$WIN32]
		"tall"					"20"	[$X360]
		"tall"					"15"	[$WIN32]
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"navUp"					"BtnCoOp"
		"navDown"				"BtnHoldoutCoop"
		"labelText"				"#ZOMBIE360UI_MainMenu_Versus"
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_Versus_Tip"
		"disabled_tooltiptext"	"#ZOMBIE360UI_MainMenu_Versus_Tip_Disabled"
		"style"					"MainMenuButton"
		"command"				"VersusSoftLock"
		"ActivationType"		"1"
		"FocusDisabledBorderSize" "1"
	}
	
	// Play holdout
	"BtnHoldoutCoop"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnHoldoutCoop"
		"xpos"					"100"
		"ypos"					"150"
		"wide"					"180"
		"tall"					"20" [$X360]
		"tall"					"15" [$WIN32]
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"navUp"					"BtnVersus"
		"navDown"				"BtnServerBrowser"
		"labelText"				"#ZOMBIE360UI_MainMenu_HoldoutCoop"
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_HoldoutCoop_Tip"
		"style"					"MainMenuButton"
		"command"				"HoldoutCoopCheck"
		"ActivationType"		"1"
	}
	
	"BtnServerBrowser"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnServerBrowser"
		"xpos"					"100"
		"ypos"					"155"	[$X360]
		"ypos"					"165"	[$WIN32]
		"wide"					"180"	[$X360]
		"wide"					"180"	[$WIN32]
		"tall"					"20"	[$X360]
		"tall"					"15"	[$WIN32]
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"navUp"					"BtnHoldoutCoop"
		"navDown"				"BtnPlaySolo"
		"labelText"				"#ZOMBIE360UI_MainMenu_ServerBrowser"
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_ServerBrowser_Tip"
		"style"					"MainMenuButton"
		"command"				"OpenServerBrowser"
		"ActivationType"		"1"
	}
	
	// Single player
	"BtnPlaySolo"
	{
		"ControlName"				"ZOMBIE360HybridButton"
		"fieldName"					"BtnPlaySolo"
		"xpos"						"100"
		"ypos"						"180"
		"wide"						"180"	[$X360]
		"wide"						"180"	[$WIN32]
		"tall"						"20"	[$X360]
		"tall"						"15"	[$WIN32]
		"autoResize"				"1"
		"pinCorner"					"0"
		"visible"					"1"
		"enabled"					"1"
		"tabPosition"				"0"
		"navUp"						"BtnVersus"
		"navDown"					"BtnStatsAndAchievements"
		"labelText"					"#ZOMBIE360UI_MainMenu_PlaySolo"
		"tooltiptext"				"#ZOMBIE360UI_MainMenu_PlaySolo_Tip"
		"disabled_tooltiptext"		"#ZOMBIE360UI_MainMenu_PlaySolo_Tip_Disabled"
		"style"						"MainMenuButton"
		"command"					"SoloPlay"
		"ActivationType"			"1"
		"FocusDisabledBorderSize"	"1"
	}
	
	"BtnStatsAndAchievements"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnStatsAndAchievements"
		"xpos"					"100"
		"ypos"					"230"	[$X360]
		"ypos"					"210"	[$WIN32]
		"wide"					"180"	[$X360]
		"wide"					"180"	[$WIN32]
		"tall"					"20"	[$X360]
		"tall"					"15"	[$WIN32]
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"navUp"					"BtnServerBrowser"
		"navDown"				"BtnOptions"
		"labelText"				"#ZOMBIE360UI_MainMenu_StatsAndAchievements"
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_StatsAndAchievements_Tip"	[$X360]
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_PCStatsAndAchievements_Tip"	[$WIN32]
		"style"					"MainMenuButton"
		"command"				"StatsAndAchievements"
		"ActivationType"		"1"
	}
	
	"BtnOptions"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnOptions"
		"xpos"					"100"
		"ypos"					"255"	[$X360]
		"ypos"					"225"	[$WIN32]
		"wide"					"180"	[$X360]
		"wide"					"180"	[$WIN32]
		"tall"					"20"	[$X360]
		"tall"					"15"	[$WIN32]
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"navUp"					"BtnStatsAndAchievements"
		"navDown"				"BtnExtras"
		"labelText"				"#ZOMBIE360UI_MainMenu_Options"
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_Options_Tip"
		"style"					"MainMenuButton"
		"command"				"FlmOptionsFlyout"
		"ActivationType"		"1"
	}
	
	"BtnExtras"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnExtras"
		"xpos"					"100"
		"ypos"					"280"	[$X360]
		"ypos"					"240"	[$WIN32]
		"wide"					"180"	[$X360]
		"wide"					"180"	[$WIN32]
		"tall"					"20"	[$X360]
		"tall"					"15"	[$WIN32]
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"navUp"					"BtnOptions"
		"navDown"				"BtnQuit" [$WIN32]
		"navDown"				"BtnCoOp" [$X360]
		"labelText"				"#ZOMBIE360UI_MainMenu_Extras"
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_Extras_Tip"
		"style"					"MainMenuButton"
		"command"				"FlmExtrasFlyout"
		"ActivationType"		"1"
	}
	
	"BtnQuit" [$WIN32]
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnQuit"
		"xpos"					"100"
		"ypos"					"255"
		"wide"					"180"
		"tall"					"15"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"navUp"					"BtnExtras"
		"navDown"				"PnlQuickJoin"
		"labelText"				"#ZOMBIE360UI_MainMenu_Quit"
		"tooltiptext"			"#ZOMBIE360UI_MainMenu_Quit_Tip"
		"style"					"MainMenuButton"
		"command"				"QuitGame"
		"ActivationType"		"1"
	}
				
	"FlmCampaignFlyout"
	{
		"ControlName"			"FlyoutMenu"
		"fieldName"				"FlmCampaignFlyout"
		"visible"				"0"
		"wide"					"0"
		"tall"					"0"
		"zpos"					"3"
		"InitialFocus"			"BtnQuickMatch"
		"ResourceFile"			"resource/UI/ZOMBIE360UI/CampaignFlyout.res"
	}
	
	"FlmHoldoutCoopFlyout"
	{
		"ControlName"			"FlyoutMenu"
		"fieldName"				"FlmHoldoutCoopFlyout"
		"visible"				"0"
		"wide"					"0"
		"tall"					"0"
		"zpos"					"3"
		"InitialFocus"			"BtnQuickMatch"
		"ResourceFile"			"resource/UI/ZOMBIE360UI/HoldoutCoopFlyout.res"
	}
	
	"FlmVersusFlyout"
	{
		"ControlName"			"FlyoutMenu"
		"fieldName"				"FlmVersusFlyout"
		"visible"				"0"
		"wide"					"0"
		"tall"					"0"
		"zpos"					"3"
		"InitialFocus"			"BtnQuickMatch"
		"ResourceFile"			"resource/UI/ZOMBIE360UI/VersusFlyout.res"
	}
	
	"FlmOptionsFlyout"
	{
		"ControlName"			"FlyoutMenu"
		"fieldName"				"FlmOptionsFlyout"
		"visible"				"0"
		"wide"					"0"
		"tall"					"0"
		"zpos"					"3"
		"InitialFocus"			"BtnAudioVideo"	[$X360]
		"InitialFocus"			"BtnVideo"	[$WIN32]
		"ResourceFile"			"resource/UI/ZOMBIE360UI/OptionsFlyout.res"
	}
	
	"FlmExtrasFlyout"
	{
		"ControlName"			"FlyoutMenu"
		"fieldName"				"FlmExtrasFlyout"
		"visible"				"0"
		"wide"					"0"
		"tall"					"0"
		"zpos"					"3"
		"InitialFocus"			"BtnCommentary"
		"ResourceFile"			"resource/UI/ZOMBIE360UI/ExtrasFlyout.res"
	}
			
	"PnlQuickJoin"
	{
		"ControlName"			"QuickJoinPanel"
		"fieldName"				"PnlQuickJoin"
		"ResourceFile"			"resource/UI/ZOMBIE360UI/QuickJoin.res"
		"visible"				"1"
		"wide"					"500"
		"tall"					"300"
		"xpos"					"r240"	[$X360]
		"xpos"					"80"	[$WIN32]
		"ypos"					"r120"	[$X360]
		"ypos"					"r75"	[$WIN32]
		"navUp"				"BtnQuit"
		"navDown"				"BtnCoOp"
	}

	"LblPlayer1GamerTag"	[$X360]
	{
		"ControlName"			"Label"
		"fieldName"				"LblPlayer1GamerTag"
		"xpos"					"100"
		"ypos"					"r160"
		"wide"					"300"
		"tall"					"16"
		"autoResize"			"0"
		"pinCorner"				"0"
		"visible"				"0"
		"enabled"				"1"
		"tabPosition"			"0"
		"Font"					"DefaultMedium"
		"fgcolor_override"		"125 125 125 255"
		"noshortcutsyntax"				"1"
	}
	
	"LblPlayer2GamerTag"	[$X360]
	{
		"ControlName"			"Label"
		"fieldName"				"LblPlayer2GamerTag"
		"xpos"					"100"
		"ypos"					"r140"
		"wide"					"300"
		"tall"					"16"
		"autoResize"			"0"
		"pinCorner"				"0"
		"visible"				"0"
		"enabled"				"1"
		"tabPosition"			"0"
		"Font"					"DefaultMedium"
		"fgcolor_override"		"125 125 125 255"
		"noshortcutsyntax"				"1"
	}
	
	"LblPlayer2Enable"	[$X360]
	{
		"ControlName"			"Label"
		"fieldName"				"LblPlayer2Enable"
		"xpos"					"100"
		"ypos"					"r120"
		"wide"					"300"
		"tall"					"16"
		"autoResize"			"0"
		"pinCorner"				"0"
		"visible"				"0"
		"enabled"				"1"
		"tabPosition"			"0"
		"labelText"				"#ZOMBIE360UI_MainMenu_SignInMessageSsStart2"
		"Font"					"DefaultMedium"
		"fgcolor_override"		"125 125 125 255"
	}
	
	"LblPlayer2DisableIcon"		[$X360]
	{
		"ControlName"			"Label"
		"fieldName"				"LblPlayer2DisableIcon"
		"xpos"					"100"
		"ypos"					"r120"
		"wide"					"30"
		"tall"					"16"
		"autoResize"			"0"
		"pinCorner"				"0"
		"visible"				"0"
		"enabled"				"1"
		"tabPosition"			"0"
		"Font"					"GameUIButtonsMini"
		"labelText"				"#GameUI_Icons_BACK"
	}
	
	"LblPlayer2Disable"		[$X360]
	{
		"ControlName"			"Label"
		"fieldName"				"LblPlayer2Disable"
		"xpos"					"125"
		"ypos"					"r120"
		"wide"					"300"
		"tall"					"16"
		"autoResize"			"0"
		"pinCorner"				"0"
		"visible"				"0"
		"enabled"				"1"
		"tabPosition"			"0"
		"Font"					"DefaultMedium"
		"labelText"				"#ZOMBIE360UI_MainMenu_SignInMessageSsDisable"
		"fgcolor_override"		"125 125 125 255"
	}
}
