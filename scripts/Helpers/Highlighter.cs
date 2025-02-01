using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class Highlighter : Node2D
{
	public Dictionary[] tilesToHighlight;
	private static Highlighter instance;
    public static Highlighter Instance => instance;

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

	public void highlightTile(Vector2I coords, Color color) {
		Dictionary keyValuePairs = new Dictionary(){
				{ "coords", coords },
				{"color", color }
			};

		if (tilesToHighlight == null) {
			instance.tilesToHighlight = new Dictionary[10];

			instance.tilesToHighlight[0] = keyValuePairs;
		} else {

			instance.tilesToHighlight = instance.tilesToHighlight.Append(keyValuePairs).ToArray();
		}

		QueueRedraw();
	}

	public void clearHighlights() {
		instance.tilesToHighlight = new Dictionary[0];
		QueueRedraw();
	}


	public override void _Draw()
	{
		if (tilesToHighlight == null) {
			return;
		}
	
		for (int i = 0; i < instance.tilesToHighlight.Length; i++) {

		Vector2I tilePosition = (Vector2I) tilesToHighlight[i].GetValueOrDefault("coords", Vector2I.Zero);

		Vector2 tilePositionGlobal = LevelGenerator.Instance.terrain.MapToLocal( tilePosition);

		Vector2[] pointsFilter = new Vector2[5];

		pointsFilter[0] = new Vector2(tilePositionGlobal.X, tilePositionGlobal.Y - 8);

		// Left point
		pointsFilter[1] = new Vector2(tilePositionGlobal.X - 16, tilePositionGlobal.Y);

		// Bottom point
		pointsFilter[2] = new Vector2(tilePositionGlobal.X, tilePositionGlobal.Y + 8);

		// Right point
		pointsFilter[3] = new Vector2(tilePositionGlobal.X + 16, tilePositionGlobal.Y);

		pointsFilter[4] = new Vector2(tilePositionGlobal.X, tilePositionGlobal.Y - 8);

		var navigation = LevelGenerator.Instance.terrain.GetCellTileData(tilePosition).GetNavigationPolygon(0);
		Color color = (Color) tilesToHighlight[i].GetValueOrDefault("color", Colors.Green);

		DrawColoredPolygon(pointsFilter, color);
		}
	}
}
