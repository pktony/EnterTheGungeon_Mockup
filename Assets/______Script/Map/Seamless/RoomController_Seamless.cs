using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomController : MonoBehaviour
{

    public Grid gridMap;

    public Tilemap ground;
    public Tilemap wall;

    int roomHeight;
    int roomWidth;



    public void SetupInitialPosition(int x, int y)
    {
        gridMap.transform.position = new Vector2(x * roomHeight, y * roomWidth);
    }
}