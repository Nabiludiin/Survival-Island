"Resource/UI/ExtrasFlyout.res"
{
	"PnlBackground"
	{
		"ControlName"		"Panel"
		"fieldName"			"PnlBackground"
		"xpos"				"0"
		"ypos"				"0"
		"zpos"				"-1"
		"wide"				"156" [$ENGLISH]
		"wide"				"236" [$!ENGLISH]
		"tall"				"45" [$X360]
		"tall"				"65" [$WIN32]
		"visible"			"1"
		"enabled"			"1"
		"paintbackground"	"1"
		"paintborder"		"1"
	}

	"BtnCommentary"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnCommentary"
		"xpos"					"0"
		"ypos"					"0"
		"wide"					"150"
		"tall"					"20"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"navUp"					"BtnAddons"  [$WIN32]
		"navUp"					"BtnCredits" [$X360]
		"navDown"				"BtnCredits"
		"tooltiptext"			"#ZOMBIE360UI_Extras_Commentary_Tip"
		"labelText"				"#ZOMBIE360UI_Extras_Commentary"
		"style"					"FlyoutMenuButton"
		"command"				"DeveloperCommentary"
	}

	"BtnCredits"
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnCredits"
		"xpos"					"0"
		"ypos"					"20"
		"wide"					"150"
		"tall"					"20"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"navUp"					"BtnCommentary"
		"navDown"				"BtnCommentary" [$X360]
		"navDown"				"BtnAddons" [$WIN32]
		"tooltiptext"			"#ZOMBIE360UI_Extras_Credits_Tip"
		"labelText"				"#ZOMBIE360UI_Extras_Credits"
		"style"					"FlyoutMenuButton"
		"command"				"Credits"
	}

	"BtnAddons"	[$WIN32]
	{
		"ControlName"			"ZOMBIE360HybridButton"
		"fieldName"				"BtnAddons"
		"xpos"					"0"
		"ypos"					"40"
		"wide"					"150"
		"tall"					"20"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"1"
		"enabled"				"1"
		"tabPosition"			"0"
		"proportionalToParent"	"1"
		"navUp"					"BtnCredits"
		"navDown"				"BtnCommentary"
		"tooltiptext"			"#ZOMBIE360UI_Extras_Addons_Tip"
		"labelText"				"#ZOMBIE360UI_Extras_Addons"
		"style"					"FlyoutMenuButton"
		"command"				"Addons"
	}
}