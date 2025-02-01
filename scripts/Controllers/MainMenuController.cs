using Godot;
using System;

public partial class MainMenuController : MarginContainer
{
	public void onStartButtonPressed() {
	 	SceneManager.Instance.changeScene("res://resources/scenes/basic_level.tscn");
	}

	public void onQuitButtonPressed() {
		GetTree().Quit();
	}
}
