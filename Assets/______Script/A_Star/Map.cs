using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    /// 필요 정보
    /// 가로 세로 크기 정보 
    /// 맵 그리드에서 Node 입히기

    Node[,] nodes;
    private int mapHeight;
    private int mapWidth;

    public Map(int _mapHeight, int _mapWidth)
    {
        mapHeight = _mapHeight;
        mapWidth = _mapWidth;

        nodes = new Node[_mapHeight, _mapWidth];

        for (int i = 0; i < _mapHeight; i++)
        {
            for (int j = 0; j < _mapWidth; j++)
            {
                float rand = UnityEngine.Random.value;
                if (rand < 0.3f && i != _mapHeight && j != _mapWidth)
                    continue;
                nodes[i, j] = new Node(i, j);
            }
        }
    }

    public Node GetNode(int x, int y)
    {
        if(IsValidPosition(y, x) && nodes[y, x] != null)
        {
            return nodes[y, x];
        }
        return null;
    }

    private bool IsValidPosition(int y, int x)
    {
        return y >= 0 && x >= 0 && y < mapWidth && x < mapHeight;
    }

    public Node GetNode(Vector2Int gridPosition)
    {
        return GetNode(gridPosition.y, gridPosition.x);
    }

    public void ClearNodeDatas()
    {
        foreach(var node in nodes)
        {
            node.ClearDatas();
        }
    }
}
