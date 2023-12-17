using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ELevels
{
    Anger,
    Sadness,
    Happy,
    Fear,
}

[Serializable]
public class LevelInfo
{
    [SerializeField]
    ELevels level;
    [SerializeField]
    List<Tile> EdgeBlocks;
    [SerializeField]
    List<Tile> FloorBlocks;
    [SerializeField]
    List<Tile> DecorationBlocks;

    public Tile GetRandomEdge() { return EdgeBlocks[UnityEngine.Random.Range(0, EdgeBlocks.Count)]; }
    public Tile GetRandomFloor() { return FloorBlocks[UnityEngine.Random.Range(0, FloorBlocks.Count)]; }
    public Tile GetRandomDecoration() { return DecorationBlocks[UnityEngine.Random.Range(0, DecorationBlocks.Count)]; }

    public int GetEdgeCount() { return EdgeBlocks.Count; }
    public int GetFloorCount() { return FloorBlocks.Count; }
    public int GetDecorationCount() { return DecorationBlocks.Count; }

    public ELevels GetLevelType() { return level; }
}

public class MapGenerator : Singleton<MapGenerator>
{
    [SerializeField]
    private List<LevelInfo> LevelInfo = new List<LevelInfo>();
    [SerializeField]
    private Vector2Int MaxMapSize;
    LevelInfo SelectedLevel;
    Tilemap tilemap;

    // Start is called before the first frame update
    protected override void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        SelectedLevel = LevelInfo[UnityEngine.Random.Range(0, LevelInfo.Count)];
        for (int x = -MaxMapSize.x / 2; x <= MaxMapSize.y / 2; x++)
        {
            for (int y = -MaxMapSize.y / 2; y <= MaxMapSize.y / 2; y++)
            {
                if (Mathf.Abs(x) == MaxMapSize.x / 2 || Mathf.Abs(y) == MaxMapSize.y / 2)
                {
                    tilemap.SetTile(new Vector3Int(x, y, -1), SelectedLevel.GetRandomEdge());
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), SelectedLevel.GetRandomFloor());
                    if (SelectedLevel.GetDecorationCount() > 0)
                    {
                        tilemap.SetTile(new Vector3Int(x,y,-1), SelectedLevel.GetRandomDecoration());
                    }
                }
            }
        }
    }

    public Vector2Int GetMapSize()
    {
        return MaxMapSize;
    }


}
