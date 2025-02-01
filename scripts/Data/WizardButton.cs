using Godot;
using System;

public partial class WizardButton : HasTooltip
{
	public override void _Ready()
	{
		base._Ready();

		tooltip_title = "Wizard";

		tooltip_description = "A wizard is never late.\n" + "He might or might not have a wand.";
	}
}