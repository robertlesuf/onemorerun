using System;
using Godot;
using Godot.Collections;

public partial class CharacterSpawner : Node
{
   	private static CharacterSpawner instance;
    public static CharacterSpawner Instance => instance;

    public Movable playableCharacter;
    public Movable[] enemyCharacters;

    public override void _EnterTree()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            QueueFree();
        }
    }

	public Movable spawnCharacter(SpriteFrames spriteFrames, Vector2 position, TileMapLayer terrain) 
	{
		var character = new Movable();
        character.animatedSprite2D = new AnimatedSprite2D();
        character.navigationAgent2D = new NavigationAgent2D();
        character.navigationAgent2D.PathDesiredDistance = 1f;
        character.navigationAgent2D.TargetDesiredDistance = 1f;
        character.collisionShape2D = new CollisionShape2D();
        character.navigationAgent2D.Connect("velocity_computed", new Callable(character, "_on_navigation_agent_2d_velocity_computed"));
        character.navigationAgent2D.AvoidanceEnabled = true;
		character.animatedSprite2D.SpriteFrames = spriteFrames;
        character.collisionShape2D.Shape = GD.Load<Shape2D>("res://resources/collision_shapes/character_shape.tres");
        
		character.Position = position;
        character.animatedSprite2D.Position -= new Vector2(0, 10);
		character.animatedSprite2D.Play("idle");
		character.YSortEnabled = true;
		terrain.AddChild(character);
       
        character.AddChild(character.animatedSprite2D);
        character.AddChild(character.navigationAgent2D);
        character.AddChild(character.collisionShape2D);

        return character;
	}

    public void spawnPlayerCharacter(Vector2 position, TileMapLayer terrain, string characterType) 
    {
        var spriteFrames = GD.Load<SpriteFrames>(characterType);	

        playableCharacter = spawnCharacter(spriteFrames, position, terrain);

        // instanciate player ui

        var uiScene = GD.Load<PackedScene>("res://resources/scenes/character_ui.tscn");

        var characterUI = uiScene.Instantiate();

        var uiElements = characterUI.GetChildren();

        foreach (var uiElement in uiElements) {
            if (uiElement.GetType() == typeof(HealthBarPlayer)) {
    
                playableCharacter.healthBar = (TextureProgressBar) uiElement;
            }
        }

        LevelGenerator.Instance.canvasLayer.AddChild(characterUI);
    }

}