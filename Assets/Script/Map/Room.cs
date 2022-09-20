using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public DoorDirection doorDir;
    public Vector2 gridPosition;
    public bool[,] hasAdjacentRoom;

    public Room(Vector2 _gridPosition)
    {
        this.gridPosition = _gridPosition;
        hasAdjacentRoom = new bool[3, 3];
    }
}
