using Godot;
public partial class Item : Node2D
{
    public Vector2I positionOnTileMap;

    public TileSet itemsTileSet;

    public string itemType;

    public void callClickedItem() {
        if (itemType == "chest") {
            var selectItemScene = GD.Load<PackedScene>("res://resources/scenes/select_item_scene.tscn");

            CanvasLayer selectItem =(CanvasLayer) selectItemScene.Instantiate();

            LevelGenerator.Instance.terrain.GetParent().AddChild(selectItem);
        }
    }
}