using Godot;
using System;

public abstract partial class HasTooltip : Control
{
	public Tooltip tooltip;

	public string tooltip_title;
	public string tooltip_description;

	const string tooltipPath = "res://resources/scenes/tooltip.tscn";

	public override void _Ready()
    {
        // Connect the mouse signals
        Connect("mouse_entered", new Callable(this, nameof(on_mouse_entered)));
        Connect("mouse_exited", new Callable(this, nameof(on_mouse_exited)));
    }

    public void on_mouse_entered()
	{
		GD.Print("mouse");
		var tooltipScene = GD.Load<PackedScene>(tooltipPath);
		tooltip = (Tooltip) tooltipScene.Instantiate();
		
		tooltip.title = tooltip_title;
		tooltip.description = tooltip_description;

		tooltip.Position = GetGlobalMousePosition();

		LevelGenerator.Instance.canvasLayer.AddChild(tooltip);
	}

	public void on_mouse_exited()
	{
		tooltip.QueueFree();
	}
}
