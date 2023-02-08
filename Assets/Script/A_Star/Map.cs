using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전체 맵에 대한 정보
/// ############################# 필요 정보
/// 가로 세로 크기 정보 
/// 맵 그리드에서 Node 입히기
/// </summary>
public class Map
{
    Node[,] nodes;
    private int mapHeight;
    private int mapWidth;

    /// <summary>
    /// Map 생성자 
    /// </summary>
    /// <param name="_mapHeight">전체 맵의 세로 크기 </param>
    /// <param name="_mapWidth">전체 맵의 가로 크기 </param>
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

    /// <summary>
    /// 현 위치에 대한 노드 정보를 가져오는 함수 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public Node GetNode(int x, int y)
    {
        if(IsValidPosition(y, x) && nodes[y, x] != null)
        {
            return nodes[y, x];
        }
        return null;
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

    private bool IsValidPosition(int x, int y)
    {
        return y >= 0 && x >= 0 && y < mapWidth && x < mapHeight;
    }
}
