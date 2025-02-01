using Godot;
using System;

public partial class CameraMaster : Camera2D
{

	private Vector2 mousePosition = Vector2.Zero;
	private Vector2 movementDelta = Vector2.Zero;

	private static CameraMaster instance;
    public static CameraMaster Instance => instance;

	private Vector2 maxZoom = new Vector2(7f, 7f);
	private Vector2 minZoom = new Vector2(5f, 5f);

	private float sensitivity = 0.8f; // Adjust for smoother movement


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Zoom = minZoom;

		instance = this;
		GD.Print("Camera movement");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("move_camera"))
		{
			if (this.mousePosition == Vector2.Zero) {
				this.mousePosition = GetLocalMousePosition();
				Input.SetMouseMode(Input.MouseModeEnum.Captured);
			}

			 // Smoothly interpolate position towards the new target position
			Position -= movementDelta * (float)delta * 10; // Scale with delta and a factor
			movementDelta = Vector2.Zero; // Reset movement delta after applying it
		} else {
			this.mousePosition = Vector2.Zero;
			Input.SetMouseMode(Input.MouseModeEnum.Visible);
		}
	}

	 public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotionEvent && Input.IsActionPressed("move_camera"))
		{
		  // Accumulate the relative movement delta
			movementDelta += mouseMotionEvent.Relative * sensitivity;
		}

		if (Input.IsActionPressed("camera_zoom_in"))
		{
			if (Zoom < maxZoom)
			Zoom += new Vector2(0.1f, 0.1f);
		}

		if (Input.IsActionPressed("camera_zoom_out"))
		{
			if (Zoom > minZoom)
			Zoom -= new Vector2(0.1f, 0.1f);
		}
	}

}
