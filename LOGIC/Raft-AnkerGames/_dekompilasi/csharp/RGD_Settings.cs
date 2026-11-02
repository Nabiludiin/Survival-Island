using System;
using System.Runtime.Serialization;

[Serializable]
public class RGD_Settings : SerializableBehaviour
{
	public RGD_Settings_Controls controls;

	public RGD_Settings_Audio audio;

	public RGD_Settings_Graphics graphics;

	public RGD_Settings_Character character;

	public RGD_Settings_General general;

	public RGD_Settings()
	{
		SetDefaults(default);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Controls", controls);
		info.AddValue("Audio", audio);
		info.AddValue("Graphics", graphics);
		info.AddValue("Character", character);
		info.AddValue("General", general);
	}

	public RGD_Settings(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "Controls":
				controls = (RGD_Settings_Controls)item.Value;
				break;
			case "Audio":
				audio = (RGD_Settings_Audio)item.Value;
				break;
			case "Graphics":
				graphics = (RGD_Settings_Graphics)item.Value;
				break;
			case "Character":
				character = (RGD_Settings_Character)item.Value;
				break;
			case "General":
				general = (RGD_Settings_General)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		controls = new RGD_Settings_Controls();
		audio = new RGD_Settings_Audio();
		graphics = new RGD_Settings_Graphics();
		character = new RGD_Settings_Character();
		general = new RGD_Settings_General();
	}
}
