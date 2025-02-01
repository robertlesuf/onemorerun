using Godot;
using System.Collections.Generic;
public partial class ItemsSpawner : Node
{
   	private static ItemsSpawner instance;
    public static ItemsSpawner Instance => instance;

    public List<Item> items = new List<Item>();

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

	public Item spawnItem(string itemType, Vector2I position) 
    {
        var item = new Item();

        item.itemType = itemType;
        item.positionOnTileMap = position;

        AddChild(item);

        items.Add(item);

        return item;
    }
}