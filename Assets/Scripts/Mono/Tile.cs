using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileData TileData;

    public void ToggleLeftEdge()
    {
        ToggleEdge(TileData.LeftEdge);
        TileData.LeftTile?.TileData.RightEdge.SetActive(TileData.LeftEdge.activeSelf);
    }

    public void ToggleRightEdge()
    {
        ToggleEdge(TileData.RightEdge);
        TileData.RightTile?.TileData.LeftEdge.SetActive(TileData.RightEdge.activeSelf);
    }

    public void ToggleTopEdge()
    {
        ToggleEdge(TileData.TopEdge);
        TileData.TopTile?.TileData.BottomEdge.SetActive(TileData.TopEdge.activeSelf);
    }

    public void ToggleBottomEdge()
    {
        ToggleEdge(TileData.BottomEdge);
        TileData.BottomTile?.TileData.TopEdge.SetActive(TileData.BottomEdge.activeSelf);
    }

    void ToggleEdge(GameObject edge)
    {
        edge?.gameObject.SetActive(!edge.activeSelf);
    }


}

[CustomEditor(typeof(Tile))]
public class TileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Tile tile = (Tile)target;
        if (GUILayout.Button("Toggle Left Edge"))
            tile.ToggleLeftEdge();
        if (GUILayout.Button("Toggle Right Edge"))
            tile.ToggleRightEdge();
        if (GUILayout.Button("Toggle Top Edge"))
            tile.ToggleTopEdge();
        if (GUILayout.Button("Toggle Bottom Edge"))
            tile.ToggleBottomEdge();
   
    }


}