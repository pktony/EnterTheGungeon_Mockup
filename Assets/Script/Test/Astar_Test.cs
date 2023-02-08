using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Astar_Test : MonoBehaviour
{
    int mapHeight = 5;
    int mapWidth = 5;

    private void Start()
    {
#if UNITY_EDITOR
        Map map = new Map(mapHeight, mapWidth);
        List<Vector2Int> test = new();
        test = A_Star.FindPath_Map(map, Vector2Int.zero, new Vector2Int(mapHeight - 1, mapWidth - 1));

        for (int i = 0; i < test.Count; i++)
        {
            Debug.Log(test[i]);
        }
#endif
    }
}
