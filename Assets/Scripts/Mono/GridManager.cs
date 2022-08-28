using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Tile tilePrefab;
    [SerializeField] [Min(1)] int width;
    [SerializeField] [Min(1)] int height;
    [SerializeField] Vector2 tileSize;
    public Tile[,] Tiles;
    [SerializeField] Direction exitDirection;
    [SerializeField] int exitIndex;
    Tile exitTile;

    Transform gameArea;

    public void CreateGameArea()
    {
        if (gameArea)
            DestroyImmediate(gameArea.gameObject);

        gameArea = new GameObject("Game Area").transform;
        Tiles = new Tile[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Tiles[i, j]=CreateTile(i,j);

                Tiles[i, j].TileData.LeftEdge?.SetActive(j == 0);
                Tiles[i, j].TileData.RightEdge?.SetActive(j == width-1);
                Tiles[i, j].TileData.TopEdge?.SetActive(i == 0);
                Tiles[i, j].TileData.BottomEdge?.SetActive(i == height-1);
            }
        }

        AssignNeighbourTiles();
        CreateExitTile();
    }

    void AssignNeighbourTiles()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (j > 0)
                    Tiles[i, j].TileData.LeftTile = Tiles[i, j - 1];
                if (i > 0)
                    Tiles[i, j].TileData.TopTile = Tiles[i - 1, j];
                if (j < width - 1)
                    Tiles[i, j].TileData.RightTile = Tiles[i, j + 1];
                if (i < height - 1)
                    Tiles[i, j].TileData.BottomTile = Tiles[i + 1, j];
            }
        }
    }

    Tile CreateTile(int heightIndex, int widthIndex)
    {
        var tile = Instantiate(tilePrefab, gameArea);
        tile.transform.localScale = new Vector3(tileSize.x, tileSize.y, 1);
        tile.transform.localPosition = new Vector3(((1 - width) * .5f + widthIndex) * tileSize.x, ((height - 1) * .5f - heightIndex) * tileSize.y, 0);
        return tile;
    }

    public void CreateExitTile()
    {
        if (exitTile)
        {
            if (exitTile.TileData.LeftTile)
            {
                exitTile.TileData.LeftTile.ToggleRightEdge();
                exitTile.TileData.LeftTile.TileData.RightTile = null;
            }
            else if (exitTile.TileData.RightTile)
            {
                exitTile.TileData.RightTile.ToggleLeftEdge();
                exitTile.TileData.RightTile.TileData.LeftTile= null;
            }
            else if (exitTile.TileData.TopTile)
            {
                exitTile.TileData.TopTile.ToggleBottomEdge();
                exitTile.TileData.TopTile.TileData.BottomTile= null;
            }
            else if (exitTile.TileData.BottomTile)
            {
                exitTile.TileData.BottomTile.ToggleTopEdge();
                exitTile.TileData.BottomTile.TileData.TopTile= null;
            }
            DestroyImmediate(exitTile.gameObject);
        }
        Tile neighbourTile;
        switch (exitDirection)
        {
            case Direction.Left:
                exitTile = CreateTile(exitIndex, -1);
                neighbourTile = Tiles[exitIndex, 0];
                exitTile.TileData.RightTile = neighbourTile;
                neighbourTile.TileData.LeftTile = exitTile;
                exitTile.ToggleRightEdge();
                exitTile.ToggleRightEdge();
                exitTile.ToggleTopEdge();
                exitTile.ToggleBottomEdge();
                break;
            case Direction.Right:
                exitTile = CreateTile(exitIndex, width);
                neighbourTile=Tiles[exitIndex, width-1];
                exitTile.TileData.LeftTile = neighbourTile;
                neighbourTile.TileData.RightTile = exitTile;
                exitTile.ToggleLeftEdge();
                exitTile.ToggleLeftEdge();
                exitTile.ToggleTopEdge();
                exitTile.ToggleBottomEdge();
                break;
            case Direction.Up:
                exitTile = CreateTile(-1, exitIndex);
                neighbourTile = Tiles[0, exitIndex];
                exitTile.TileData.BottomTile = neighbourTile;
                neighbourTile.TileData.TopTile = exitTile;
                exitTile.ToggleLeftEdge();
                exitTile.ToggleRightEdge();
                exitTile.ToggleBottomEdge(); 
                exitTile.ToggleBottomEdge();
                break;
            case Direction.Down:
                exitTile = CreateTile(height, exitIndex);
                neighbourTile = Tiles[height-1, exitIndex];
                exitTile.TileData.TopTile = neighbourTile;
                neighbourTile.TileData.BottomTile= exitTile;
                exitTile.ToggleLeftEdge();
                exitTile.ToggleRightEdge();
                exitTile.ToggleTopEdge();
                exitTile.ToggleTopEdge();
                break;
            default:
                break;
        }
        exitTile.TileData.IsExitTile = true;
    }
}

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridManager gridManager = (GridManager)target;
        if (GUILayout.Button("Create Game Area"))
            gridManager.CreateGameArea();
        if (GUILayout.Button("Create ExitTile"))
            gridManager.CreateExitTile();
    }
}