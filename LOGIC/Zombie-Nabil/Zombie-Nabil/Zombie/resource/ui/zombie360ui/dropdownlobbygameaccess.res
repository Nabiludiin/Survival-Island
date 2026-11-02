"Resource/UI/DropDownLobbyGameAccess.res"
{
	"PnlBackground"
	{
		"ControlName"			"Panel"
		"fieldName"				"PnlBackground"
		"xpos"					"0"
		"ypos"					"0"
		"zpos"					"1"
		"wide"					"146" [$ENGLISH]
		"wide"					"180" [$!ENGLISH]
		"tall"					"65"	[$X360]
		"tall"					"45"	[$WIN32]
		"visible"				"1"
		"enabled"				"1"
		"paintbackground"		"1"
		"paintborder"			"1"
	}

	"BtnPrivate"	[$X360]
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnPrivate"
		"xpos"					"0"
		"ypos"					"0"
		"zpos"					"2"
		"wide"					"140"
		"tall"					"20"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"wrap"					"1"
		"navUp"					"BtnPublic"
		"navDown"				"BtnFriends"
		"labelText"				"#ZOMBIE360UI_Access_Invite"
		"tooltiptext"			"#ZOMBIE360UI_Lobby_MakePrivate_Tip"
		"style"					"FlyoutMenuButton"
		"command"				"#ZOMBIE360UI_Access_Invite"
	}	
	
	"BtnFriends"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnFriends"
		"xpos"					"0"
		"ypos"					"20"	[$X360]
		"ypos"					"0"		[$WIN32]
		"zpos"					"2"
		"wide"					"140"
		"tall"					"20"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"wrap"					"1"
		"navUp"					"BtnPrivate"	[$X360]
		"navUp"					"BtnPublic"		[$WIN32]
		"navDown"				"BtnPublic"
		"labelText"				"#ZOMBIE360UI_Access_Friends"
		"tooltiptext"			"#ZOMBIE360UI_Lobby_MakeFriendOnly_Tip"
		"style"					"FlyoutMenuButton"
		"command"				"#ZOMBIE360UI_Access_Friends"
	}	
	
	"BtnPublic"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnPublic"
		"xpos"					"0"
		"ypos"					"40"	[$X360]
		"ypos"					"20"	[$WIN32]
		"zpos"					"2"
		"wide"					"140"
		"tall"					"20"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"wrap"					"1"
		"navUp"					"BtnFriends"
		"navDown"				"BtnPrivate"	[$X360]
		"navDown"				"BtnFriends"	[$WIN32]
		"labelText"				"#ZOMBIE360UI_Access_Public"
		"tooltiptext"			"#ZOMBIE360UI_Lobby_OpenToPublic_Tip"
		"style"					"FlyoutMenuButton"
		"command"				"#ZOMBIE360UI_Access_Public"
	}	
}
