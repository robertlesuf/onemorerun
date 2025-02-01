using Godot;
using System;

public partial class KnightButton : HasTooltip
{
	public override void _Ready()
	{
		base._Ready();

		tooltip_title = "Knight";

		tooltip_description = "A knight is never bored.\n" + "He is always ready to cook chicken!";
	}
}
