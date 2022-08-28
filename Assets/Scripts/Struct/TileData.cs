using System;
using UnityEngine;

[Serializable]
public struct TileData
{
    public GameObject RightEdge;
    public GameObject LeftEdge;
    public GameObject TopEdge;
    public GameObject BottomEdge;

    public Tile RightTile;
    public Tile LeftTile;
    public Tile TopTile;
    public Tile BottomTile;

    public bool IsExitTile;
}
