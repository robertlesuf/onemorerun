using System;
using Godot;
using Godot.Collections;

public partial class LevelGenerator : Node
{
    public TileMapLayer terrain;
	public TileMapLayer obstacles;
	public CanvasLayer canvasLayer;
	private static LevelGenerator instance;
    public static LevelGenerator Instance => instance;

    public TileSetAtlasSource textureSource;

	public Vector2I baseCharacterPosition = new Vector2I(0, 2);
	public RandomNumberGenerator random = new RandomNumberGenerator();
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

	public void generateLevel()
    {
		canvasLayer = new CanvasLayer();

		GetParent().AddChild(canvasLayer);

		// select character 
		var selectCharacterScene = GD.Load<PackedScene>("res://resources/scenes/select_character.tscn");

		canvasLayer.AddChild(selectCharacterScene.Instantiate());

        // Create and configure the TileMap
        terrain = new TileMapLayer
        {
            Name = "GeneratedLevel",
            // center of parent
			Position = new Vector2(0, 0),
        };

        // Load the terrain TileSet
        TileSet terrainTileSet = GD.Load<TileSet>("res://resources/tile_maps/forest.tres");

		terrain.Visible = true;
		terrain.YSortEnabled = true;
		obstacles = new TileMapLayer
		{
			Name = "Obstacles",
			Position = new Vector2(0, 0),
		};

		obstacles.Visible = true;
		obstacles.YSortEnabled = true;
		GD.Print("it's working");

        if (terrainTileSet == null)
        {
            GD.PrintErr("Failed to load terrain TileSet. Check the path.");
            return;
        }

        terrain.TileSet = terrainTileSet;
		obstacles.TileSet = terrainTileSet;

        // Add the TileMap to the root of the scene tree
        GetParent().AddChild(terrain);

		terrain.AddChild(obstacles);

		var tileType = random.RandiRange(0, 1) == 1  ? "dirt" : "grass";

		// hardcoded to grass
		tileType = "grass";
		textureSource = terrain.TileSet.GetSource(0) as TileSetAtlasSource; 
        // Add some tiles
        addSection(tileType);

		var highlighter = new Highlighter();

		terrain.AddChild(highlighter);

		Tween terrainTween = CreateTween();

		terrainTween.TweenProperty(terrain, "position", new Vector2(terrain.Position.X, terrain.Position.Y - 1), 1)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.InOut);

		terrainTween.TweenProperty(terrain, "position", new Vector2(terrain.Position.X, terrain.Position.Y), 1)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);		

		terrainTween.SetLoops(99999);

		terrainTween.Play();		
    }

	public Dictionary<string, Vector2I> getTileMapAtlas() {
		return new Dictionary<string, Vector2I>() {
			{ "grass_1", new Vector2I(0, 0) },
			{ "grass_2", new Vector2I(1, 0) },
			{ "grass_3", new Vector2I(2, 0) },
			{ "grass_4", new Vector2I(3, 0) },
			{ "grass_tree", new Vector2I(0, 1) },
			{ "grass_mushroom", new Vector2I(1, 1) },
			{ "grass_chest", new Vector2I(2, 1) },
			{ "grass_rock", new Vector2I(3, 1) },
		};
	} 
	
	public void addSection(string tileType, bool onTheRight = false, string nextRoomType = "")
	{
		var atlas = getTileMapAtlas();

		// Determine the base tile and obstacle types
		var baseTilePrefix = tileType.ToLower() == "grass" ? "grass_" : "dirt_";
		var obstaclePrefix = tileType.ToLower() == "grass" ? "grass_obstacle_" : "dirt_obstacle_";

		
		// int pseudoRandomChance = 10;
		int length = 0;
		int width = 0; 

		Vector2I startPosition = new Vector2I(0,0);

		if (terrain.GetUsedRect().Size == Vector2I.Zero) {
			length = 5;
			width = 5;  
			Rect2I navigationBounds = new Rect2I(0, 0, length, width);

			Navigation.setupNavigation(terrain, obstacles, navigationBounds);

			GetTree().Root.AddChild(Navigation.Instance);
		} else {
			length = onTheRight ? (terrain.GetUsedRect().Size.X + 5) : terrain.GetUsedRect().Size.X ;
			width = onTheRight ? terrain.GetUsedRect().Size.Y  : (terrain.GetUsedRect().Size.Y + 5);

			startPosition = onTheRight ? new Vector2I(terrain.GetUsedRect().Size.X, terrain.GetUsedRect().Size.Y - 5) : new Vector2I(terrain.GetUsedRect().Size.X - 5, terrain.GetUsedRect().Size.Y);
		}
		// Generate the straight section
		for (int x = startPosition.X; x < length; x++)
		{
			for (int y = startPosition.Y; y < width; y++)
			{
				if (x == baseCharacterPosition.X && y == baseCharacterPosition.Y) {
					AddBaseTile(x, y, baseTilePrefix, atlas);
					continue;
				}

				if (x == startPosition.X + 2 && y == startPosition.Y + 4) {
					string roomType;

					if (startPosition.X == 0 && 0 == startPosition.Y) {
						roomType = "chest";
					} else {
						int roomTypeInt = random.RandiRange(0, 3);
						switch (roomTypeInt)
						{
							case 0: 
								roomType = "chest"; break;
								case 1:
								roomType = "rock"; break;
								case 2:
								roomType = "mushroom"; break;
							default:
								roomType = "mushroom";break;
						}
					}
					addAnimatedSpriteOnCell("arrow_left", x, y, roomType);
				}

				if (x == startPosition.X + 4 && y == startPosition.Y + 2) {
					string roomType;
					if (startPosition.X == 0 && 0 == startPosition.Y) {
						roomType = "rock";
					} else {
						int roomTypeInt = random.RandiRange(0, 3);
						switch (roomTypeInt)
						{
							case 0: 
								roomType = "chest"; break;
								case 1:
								roomType = "rock"; break;
								case 2:
								roomType = "mushroom"; break;
							default:
								roomType = "mushroom";break;
						}
					}
					addAnimatedSpriteOnCell("arrow_right", x, y, roomType);
				}
				
				if (nextRoomType != "") {
					if (x == startPosition.X + 2 && y == startPosition.Y + 2) {
						AddObstacle(x, y, obstaclePrefix, atlas["grass_" + nextRoomType], nextRoomType);
					}
				}

				AddBaseTile(x, y, baseTilePrefix, atlas);
			}
		}
	}

	private void AddBaseTile(int x, int y, string baseTilePrefix, Dictionary<string, Vector2I> atlas)
	{
		// Randomly choose a base tile from the atlas
		int tileVariant = baseTilePrefix == "grass_" ? random.RandiRange(1, 4) : random.RandiRange(1, 10); // Assuming up to 22 variants
		string tileKey = $"{baseTilePrefix}{tileVariant}";

		if (atlas.ContainsKey(tileKey))
		{
			var coords = atlas[tileKey];
    
			addSpriteAtCoords(coords, x, y);
		}
	}

	public Texture2D getTextureAtCoords(Vector2I coords, TileSetAtlasSource customTextureSource = null) {
		
		Rect2I region = new Rect2I();
		Image image = new Image();
		if (customTextureSource == null) {
			region = textureSource.GetTileTextureRegion(coords);
			  // Get the image of the texture from the source
       		image = textureSource.Texture.GetImage();
		} else {
			region = customTextureSource.GetTileTextureRegion(coords);
			  // Get the image of the texture from the source
       		image = customTextureSource.Texture.GetImage();
		}

        // Extract the region of the tile as a new image
        Image tileImage = image.GetRegion(region);

        // Create an ImageTexture from the tile image and return it
        ImageTexture tileTexture = ImageTexture.CreateFromImage(tileImage);

        return tileTexture;
	}
	private void AddObstacle(int x, int y, string obstaclePrefix, Vector2I obstacle, string nextRoomType)
	{
		addSpriteAtCoords(obstacle, x, y, true, nextRoomType);
	}

	public void addSpriteAtCoords(Vector2I coords, int x, int y, bool obstacle = false, string nextRoomType = "") 
	{
		var localPosition = terrain.MapToLocal(new Vector2I(x, y));
		localPosition = new Vector2(localPosition.X, localPosition.Y + 40);
		
		Sprite2D tempSprite = new Sprite2D
		{
			Texture = getTextureAtCoords(coords),
			Position = localPosition, // Start 100px below
			Modulate = new Color(1, 1, 1, 1) // Fully transparent initially
		};

		if (obstacle) {
			obstacles.AddChild(tempSprite);

			ItemsSpawner.Instance.spawnItem(nextRoomType, new Vector2I(x, y));
		} else {
			terrain.AddChild(tempSprite);
		}

		animateSprite(tempSprite, x, y, coords, localPosition, obstacle);
	} 


	public Dictionary<string, string> getSpritesAtlas() {
		return new Dictionary<string, string>() {
			{ "arrow_right", "res://resources/sprites/terrain/arrow-right.png" },
			{ "arrow_left", "res://resources/sprites/terrain/arrow-left.png" },
		};
	} 

	public void addAnimatedSpriteOnCell(string sprite, int x, int y, string roomType) 
	{
		var localPosition = terrain.MapToLocal(new Vector2I(x, y));

		string texturePath = getSpritesAtlas()[sprite];

		Spawner arrowNode = new Spawner();
		arrowNode.AddToGroup("spawners");
		arrowNode.roomType = roomType;
		arrowNode.Position = localPosition;
		
		Sprite2D tempSprite = new Sprite2D
		{
			Texture = GD.Load<Texture2D>(texturePath),
			Position = new Vector2(0, -2),
			Modulate = new Color(1.5f, 1.5f, 1.5f, 1) // Fully transparent initially
		};

		var rewardTexture = getTextureAtCoords(getTileMapAtlas()["grass_" + roomType]);

		var rewardPosition = sprite != "arrow_right" ? new Vector2(-10, 0) : new Vector2(10, 0); 

		Sprite2D rewardSprite = new Sprite2D
		{
			Texture =	rewardTexture,
			Position = rewardPosition,
			Scale = new Vector2(0.4f, 0.4f),
			Modulate = new Color(1, 1, 1, 0.5f) // Fully transparent initially
		};
		
		var tweenPosition = CreateTween();
		
		tweenPosition.TweenProperty(tempSprite, "position", new Vector2(0, - 4), 0.5f)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.InOut);

		tweenPosition.TweenProperty(tempSprite, "position", new Vector2(0, - 2), 0.5f)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.InOut);

		tweenPosition.SetLoops(9999);

		tweenPosition.Play();
		arrowNode.animation = tweenPosition;
		arrowNode.onTheRight = sprite == "arrow_right";
		arrowNode.AddChild(tempSprite);
		arrowNode.AddChild(rewardSprite);
		terrain.AddChild(arrowNode);
	}

	public void animateSprite(Sprite2D tempSprite, int x, int y, Vector2I coords, Vector2 localPosition, bool obstacle = false)
	{
	    Tween tweenPosition = CreateTween();
		float normalizedX = x;
		float normalizedY = y;
		if (terrain.GetUsedRect().Size.X != 0) {
			normalizedX = x - (terrain.GetUsedRect().Size.X - 3);
			normalizedY = y - (terrain.GetUsedRect().Size.Y - 3);
		}
	
        // Animate the position (move from below) and opacity (fade-in)
        tweenPosition.TweenProperty(tempSprite, "position", new Vector2(localPosition.X, localPosition.Y - 50), 0.5f * (normalizedX < 12 ? normalizedX/2f : (6) + (normalizedX/40f)) + 0.5f * (normalizedY / 3f))
            .SetTrans(Tween.TransitionType.Quad);

			// After the animation, set the tile in the TileMap and remove the temporary sprite
        tweenPosition.TweenCallback(Callable.From(() =>
        {
			
			if (obstacle) {
				addNewCellToLevel(coords, x, y, obstacles);
			}
			else {
				addNewCellToLevel(coords, x, y, terrain);
			}

            tempSprite.QueueFree();
        }));
        tweenPosition.Play();
	}

	public void addNewCellToLevel(Vector2I atlasCoords,int x, int y, TileMapLayer layer)
	{
		layer.SetCell(new Vector2I(x, y), 0, atlasCoords);
		Navigation.Instance.updateNavigation(terrain);
	}
}