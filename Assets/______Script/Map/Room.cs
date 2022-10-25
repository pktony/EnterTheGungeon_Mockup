using UnityEngine;

/// <summary>
/// 방 한 칸에 대한 정보 
/// </summary>
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
