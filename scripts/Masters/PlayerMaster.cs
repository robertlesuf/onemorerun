using Godot;

public partial class PlayerMaster : Node2D 
{
    public static Vector2I mouseTile = Vector2I.Zero;

    public override void _Input(InputEvent @event)
    {
        if (CharacterSpawner.Instance.playableCharacter != null && Input.IsActionPressed("move_player") && !CharacterSpawner.Instance.playableCharacter.shouldMove) {
            var mouseLocation = GetGlobalMousePosition();
            bool isTileWalkable = Navigation.Instance.isGlobalPositionWalkable(mouseLocation);

            if (isTileWalkable) {
                var localTargetLocation = Navigation.Instance.getLocalTargetLocation(mouseLocation);
                CharacterMover.Instance.moveCharacter(CharacterSpawner.Instance.playableCharacter, localTargetLocation, LevelGenerator.Instance.terrain);
            } else {
                var tileLocation = Navigation.Instance.getTileLocation(mouseLocation);

                var itemsOnMap = ItemsSpawner.Instance.items;
             
                foreach (var item in itemsOnMap) {
                    if (tileLocation.Equals(item.positionOnTileMap)) {
                        item.callClickedItem();
                    }
                }
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (CharacterSpawner.Instance.playableCharacter == null ) {
            return;
        }

        var mousePosition = GetLocalMousePosition();

        if (mouseTile == Vector2I.Zero || LevelGenerator.Instance.terrain.LocalToMap(mousePosition) != mouseTile || CharacterSpawner.Instance.playableCharacter.shouldMove) {
            Highlighter.Instance.clearHighlights();
        }

        if (mouseTile == LevelGenerator.Instance.terrain.LocalToMap(mousePosition) || CharacterSpawner.Instance.playableCharacter.shouldMove) { 
            return;
        }

        mouseTile = LevelGenerator.Instance.terrain.LocalToMap(mousePosition);

        if (LevelGenerator.Instance.terrain.GetCellTileData(mouseTile) == null) {
            return;
        }

        if (LevelGenerator.Instance.obstacles.GetCellTileData(mouseTile) != null) {
             Highlighter.Instance.highlightTile(mouseTile, new Color(Colors.Orange, 0.4f));
            return;
        }

        var path = Navigation.Instance.aStarGrid2D.GetIdPath(LevelGenerator.Instance.terrain.LocalToMap(CharacterSpawner.Instance.playableCharacter.Position), mouseTile);

        foreach (Vector2I tile in path) {
            Highlighter.Instance.highlightTile(tile, new Color(Colors.White, 0.4f));
        }
    }
}