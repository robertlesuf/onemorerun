using System;
using Godot;
using Godot.Collections;

public partial class CharacterMover : Node
{
   	private static CharacterMover instance;
    public static CharacterMover Instance => instance;

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

    public void moveCharacter(Movable character, Vector2 targetLocation, TileMapLayer terrain) 
    {
        character.navigationAgent2D.TargetPosition = targetLocation;
        var desiredTilePosition = terrain.LocalToMap(targetLocation);
        var characterTilePosition = terrain.LocalToMap(character.Position);
    
        Highlighter.Instance.highlightTile(desiredTilePosition, Colors.Aquamarine);

        var tileIds = Navigation.Instance.aStarGrid2D.GetIdPath(characterTilePosition, desiredTilePosition);
    
        var tilePositions = new Vector2[tileIds.Count];	
        for (int i = 0; i < tileIds.Count; i++) {
            tilePositions[i] = terrain.MapToLocal(tileIds[i]);
            Highlighter.Instance.highlightTile(tileIds[i], Colors.Aquamarine);
        }
        character.animatedSprite2D.Play("walk");
        character.startMovingTowardsPoint(targetLocation, tilePositions, terrain);
    }
}