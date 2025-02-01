using Godot;
using System;
using System.Net.Http.Headers;

public partial class Navigation : Node2D
{
	private static Navigation instance;
    public static Navigation Instance => instance;

    public float debounce = 0f;
	public AStarGrid2D aStarGrid2D;

	public static void setupNavigation(TileMapLayer terrain, TileMapLayer obstacles, Rect2I navigationBounds) {
		instance = new Navigation();
		instance.aStarGrid2D = new AStarGrid2D();
		instance.aStarGrid2D.Region = navigationBounds;
		instance.aStarGrid2D.CellSize = new Vector2I(1, 1);
		instance.aStarGrid2D.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never; 
		instance.aStarGrid2D.Update();

		instance.updateNavigation(terrain);
	}

	public void  updateNavigation(TileMapLayer terrain) {
		debounce = 0.5f;
	}

    public override void _Process(double delta)
    {
        if ( debounce > 0f ) {
			debounce -= (float)delta;

			if ( debounce < 0f ) {
				debounce = 0f;
				GD.Print(LevelGenerator.Instance.terrain.GetUsedRect());
				instance.aStarGrid2D.Region = LevelGenerator.Instance.terrain.GetUsedRect();
				instance.aStarGrid2D.Update();

				var obstacleCells = LevelGenerator.Instance.obstacles.GetUsedCells();

				foreach (var cell in obstacleCells) {
					instance.aStarGrid2D.SetPointSolid(cell);
				}

				for (int i = 0; i < instance.aStarGrid2D.Region.Size.X; i++) {
					for (int j = 0; j < instance.aStarGrid2D.Region.Size.Y; j++) {
						if (LevelGenerator.Instance.terrain.GetCellTileData(new Vector2I(i, j)) == null) {
							instance.aStarGrid2D.SetPointSolid(new Vector2I(i, j));
						}
					}
				}
			}
		}
    }

    public bool isTileWalkable(Vector2I tilePosition) 
	{
		return LevelGenerator.Instance.terrain.GetCellTileData(tilePosition) != null &&
			 LevelGenerator.Instance.terrain.GetCellTileData(tilePosition).GetNavigationPolygon(0) != null
            && !LevelGenerator.Instance.obstacles.GetUsedCells().Contains(tilePosition);
	}

	public bool onTheSameTile(Vector2 position1, Vector2 position2)
	{
		var tilePosition1 = LevelGenerator.Instance.terrain.LocalToMap(position1);
		var tilePosition2 = LevelGenerator.Instance.terrain.LocalToMap(position2);

		return (tilePosition1 == tilePosition2);
	}
	public bool isGlobalPositionWalkable(Vector2 globalPosition)
	{
		var localPosition = LevelGenerator.Instance.terrain.ToLocal(globalPosition);

		var tilePosition = LevelGenerator.Instance.terrain.LocalToMap(localPosition);

		return isTileWalkable(tilePosition);
	}

	public Vector2 getLocalTargetLocation(Vector2 globalPosition) {
		return LevelGenerator.Instance.terrain.ToLocal(globalPosition);
	}

	public Vector2I getTileLocation(Vector2 globalPosition) {
		return LevelGenerator.Instance.terrain.LocalToMap(LevelGenerator.Instance.terrain.ToLocal(globalPosition));
	}
}
