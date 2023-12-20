using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageFloor : MonoBehaviour
{
    public Tilemap[] tiles;
    public RuleTile ruleTile;

    public void Initialize()
    {
        if (StageManager.instance.stageData.stageFloor == null)
        {
            throw new NullReferenceException("스테이지 룰타일 정보가 없습니다.");
        }
        else
        {
            ruleTile = StageManager.instance.stageData.stageFloor;
        }

        foreach (var tile in tiles)
        {
            tile.CompressBounds();
            BoundsInt bounds = tile.cellBounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y =  bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);
                    tile.SetTile(position, ruleTile);
                }
            }
        }
    }
}
