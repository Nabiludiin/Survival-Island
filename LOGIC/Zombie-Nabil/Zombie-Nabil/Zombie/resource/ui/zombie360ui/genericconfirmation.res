"Resource/UI/GenericConfirmation.res"
{
	"GenericConfirmation"
	{
		"ControlName"		"Frame"
		"fieldName"			"GenericConfirmation"
		"autoResize"		"0"
		"pinCorner"			"0"
		"visible"			"1"
		"enabled"			"1"
		"tabPosition"		"0"
	}

	"CheckButton"
	{
		"ControlName"		"CvarToggleCheckButton_GameUI"
		"fieldName"		"CheckButton"
		"xpos"			"0"
		"ypos"			"0"
		"zpos"			"2"
		"wide"			"14"
		"tall"			"14"
		"autoResize"		"0"
		"pinCorner"		"0"
		"visible"		"0"
		"enabled"		"1"
		"tabPosition"		"2"
		"textAlignment"		"west"
		"dulltext"		"0"
		"brighttext"		"0"
		"wrap"			"0"
		"Default"		"0"
	}

	"LblOkButton"
	{
		"ControlName"			"Label"
		"fieldName"				"LblOkButton"
		"xpos"					"0"
		"ypos"					"0"
		"tall"					"35"
		"wide"					"35"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"0" [$WIN32]
		"enabled"				"1"
		"tabPosition"			"0"
	}

	"LblOkText"
	{
		"ControlName"			"Label"
		"fieldName"				"LblOkText"
		"xpos"					"0"
		"ypos"					"0"
		"wide"					"135"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"0" [$WIN32]
		"enabled"				"1"
		"tabPosition"			"0"
		"textAlignment"	        "north-west"
	}

	"LblCancelButton"
	{
		"ControlName"			"Label"
		"fieldName"				"LblCancelButton"
		"xpos"					"0"
		"ypos"					"0"
		"tall"					"35"
		"wide"					"35"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"0" [$WIN32]
		"enabled"				"1"
		"tabPosition"			"0"
	}

	"LblCancelText" 
	{
		"ControlName"			"Label"
		"fieldName"				"LblCancelText"
		"xpos"					"0"
		"ypos"					"0"
		"wide"					"135"
		"autoResize"			"1"
		"pinCorner"				"0"
		"visible"				"0" [$WIN32]
		"enabled"				"1"
		"tabPosition"			"0"
		"textAlignment"	        "north-west"
	}

	"BtnOK"
	{
		"ControlName"			"Button"
		"fieldName"				"BtnOK"
		"xpos"					"0"
		"ypos"					"0"
		"wide"					"45"
		"tall"					"16"
		"autoResize"			"0"
		"pinCorner"				"0"
		"visible"				"0" [$X360]
		"enabled"				"1"
		"tabPosition"			"0"
		"AllCaps"				"1"
		"labelText"				"#ZOMBIE_btn_OK"
		"textAlignment"			"center"
		"command"				"OK"
		"font"					"DefaultBold"
	}

	"BtnCancel"
	{
		"ControlName"			"Button"
		"fieldName"				"BtnCancel"
		"xpos"					"0"
		"ypos"					"0"
		"wide"					"45"
		"tall"					"16"
		"autoResize"			"0"
		"pinCorner"				"0"
		"visible"				"0" [$X360]
		"enabled"				"1"
		"tabPosition"			"0"
		"AllCaps"				"1"
		"labelText"				"#ZOMBIE_btn_cancel"
		"textAlignment"			"center"
		"command"				"cancel"
		"font"					"DefaultBold"
	}
}