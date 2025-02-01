using Godot;
using System;
using System.Collections.Generic;

public partial class SelectItems : CanvasLayer
{
	public List<Texture2D> items = new List<Texture2D>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var itemsTileSet = GD.Load<TileSet>("res://resources/tile_maps/items.tres");
		TileSetAtlasSource source = itemsTileSet.GetSource(0) as TileSetAtlasSource;
		var swordTexture = LevelGenerator.Instance.getTextureAtCoords(new Vector2I(0, LevelGenerator.Instance.random.RandiRange(0,7)), source);
		var axeTexture = LevelGenerator.Instance.getTextureAtCoords(new Vector2I(1, LevelGenerator.Instance.random.RandiRange(0,7)), source);
		var wandTexture = LevelGenerator.Instance.getTextureAtCoords(new Vector2I(3, LevelGenerator.Instance.random.RandiRange(0,7)), source);
		
		PanelContainer panelContainer = new PanelContainer();
		panelContainer.SetAnchorsPreset(Control.LayoutPreset.Center);
		panelContainer.Scale = new Vector2I(5, 5);
	
		
		MarginContainer marginContainer = new MarginContainer();

		marginContainer.AddThemeConstantOverride("margin_left", 5);
		marginContainer.AddThemeConstantOverride("margin_right", 5);
		marginContainer.AddThemeConstantOverride("margin_top", 5);
		marginContainer.AddThemeConstantOverride("margin_bottom", 5);
		TextureButton sword = new TextureButton();
		sword.Name = "Sword";
		sword.TextureNormal = swordTexture;
		sword.SizeFlagsHorizontal = Control.SizeFlags.ShrinkBegin;

	    TextureButton axe = new TextureButton();
		axe.Name = "Sword";
		axe.TextureNormal = axeTexture;
		axe.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;

		TextureButton wand = new TextureButton();
		wand.Name = "Sword";
		wand.TextureNormal = wandTexture;
		wand.SizeFlagsHorizontal = Control.SizeFlags.ShrinkEnd;

		marginContainer.AddChild(sword);
		marginContainer.AddChild(axe);
		marginContainer.AddChild(wand);

		panelContainer.Size = new Vector2(40 , 8);

		panelContainer.AddChild(marginContainer);
		AddChild(panelContainer);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
