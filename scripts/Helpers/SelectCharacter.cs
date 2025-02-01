using Godot;
using System;

public partial class SelectCharacter : Control
{
	public void _on_knight_pressed()
	{
		CharacterSpawner.Instance.spawnPlayerCharacter(LevelGenerator.Instance.terrain.MapToLocal(new Vector2I(0, 0)), LevelGenerator.Instance.terrain, "res://resources/units/knight_character.tres");

		CharacterSpawner.Instance.playableCharacter.character = new Knight();

		this.GetParent().GetParent().QueueFree();
	}


	public void _on_wizard_pressed()
	{
		CharacterSpawner.Instance.spawnPlayerCharacter(LevelGenerator.Instance.terrain.MapToLocal(new Vector2I(0, 0)), LevelGenerator.Instance.terrain, "res://resources/units/wizard_character.tres");
		
        CharacterSpawner.Instance.playableCharacter.character = new Wizard();
		
		this.GetParent().GetParent().QueueFree();
	}
}
