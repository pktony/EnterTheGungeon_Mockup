using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public DoorDirection doorDir;
    public Vector2 gridPosition;
    public bool[,] hasAdjacentRoom;

    // 방 한 칸의 사이즈 
    private readonly int ROOM_HEIGHT = 18;
    private readonly int ROOM_WIDTH = 22;

    public Room(Vector2 _gridPosition)
    {
        this.gridPosition = _gridPosition;
        hasAdjacentRoom = new bool[3, 3];
    }
}
