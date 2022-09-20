using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMap
{
    // 씬마다 하나씩 있는 그리드

    /// 필요 정보
    /// 가로 세로 크기 정
    /// 그리드에서 Node 입히기

    Node[,] nodes;
    private int height;
    private int width;

    Vector2Int offset;

    public GridMap(Tilemap ground, Tilemap wall)
    {
        height = ground.size.y;
        width = ground.size.x;

        // ground 왼쪽 아래 끝 지저
        offset = (Vector2Int)ground.cellBounds.min;

        nodes = new Node[height, width];
        for (int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                nodes[height - 1 - i, j] = new Node(i, j);
            }
        }

        // Wall Node 설정
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                TileBase tile = wall.GetTile(new(i, j));
                if(tile != null)
                {
                    Node node = GetNode(i, j);
                    node.movable = false;
                    Debug.Log($"{node.x}, {node.y} : {node.movable}");
                }
            }
        }
    }


    private Node GetNode(int y, int x)
    {
        return nodes[height - y, x - width];
    }
}
