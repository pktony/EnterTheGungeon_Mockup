using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] maps;

    bool[,] roomExists;

    public const int MAP_HEIGHT = 5;
    public const int MAP_WIDTH = 5;

    private int room_Height = 18;
    private int room_Width = 22;

    private int room_Offset = 10;

    private void Awake()
    {
        roomExists = new bool[MAP_HEIGHT, MAP_WIDTH];
        Room[,] room = new Room[MAP_HEIGHT, MAP_WIDTH];
        RandomizeRooms();

        for (int i = 0; i < MAP_HEIGHT; i++)
        {
            for (int j = 0; j < MAP_WIDTH; j++)
            {
                if (roomExists[i, j])
                {
                    ///   #  
                    /// # # #
                    ///   #
                    /// i-1, j-1   i, j+1     i+1, j+1
                    /// i-1, j      i,j       i+1, j
                    /// i-1, j-1   i, j-1     i+1, j-1
                    room[i, j] = new Room(new Vector2(j * room_Width + room_Offset, i * room_Height + room_Offset));
                    for (int k = -1; k < 2; k++)
                    {
                        for (int w = -1; w < 2; w++)
                        {
                            if (k == 0 && w == 0)
                                continue;
                            else if (Mathf.Abs(k) == Mathf.Abs(w))
                                continue;
                            else if (i + k < 0 || i + k > MAP_HEIGHT - 1 || j + w < 0 || j + w > MAP_WIDTH - 1)
                                continue;

                            if (roomExists[i + k, j + w])
                            { // 0 ~ 2 까지인데 k,w 는 -1 ~ 1 까지임
                                room[i, j].hasAdjacentRoom[k + 1, w + 1] = true;
                            }
                        }
                    }
                }
            }
        }


        FindRoomDirection(room);
        MakeRooms(room);

    }

    private void FindRoomDirection(Room[,] room)
    {
        /// 2,0  2,1  2,2
        /// 1,0  1,1  1,2
        /// 0,0  0,1  0,2
        /// => 2,1 : U
        /// => 1,0 : L
        /// => 1,2 : R
        /// => 0,1 : D
        /// => 2,1 | 0,1 : UD
        /// => 1,0 | 1,2 : LR
        /// => 1,2 | 2,1 : UR
        /// => 1,0 | 2,1 : UL
        /// => 0,1 | 1,2 : DR
        /// => 0,1 | 1,0 : DL
        /// UD, LR, UR, UL, DR, DL, ULR, DLR, UDL, UDR

        for (int i = 0; i < MAP_HEIGHT; i++)
        {
            for (int j = 0; j < MAP_WIDTH; j++)
            {
                if (roomExists[i, j])
                {
                    if (room[i, j].hasAdjacentRoom[2, 1] && room[i, j].hasAdjacentRoom[1, 0] &&
                        room[i, j].hasAdjacentRoom[0, 1] && room[i, j].hasAdjacentRoom[1, 2])
                    {
                        room[i, j].doorDir = DoorDirection.All;
                    }
                    // ################ 3 ways
                    else if (room[i, j].hasAdjacentRoom[2, 1] && room[i, j].hasAdjacentRoom[1, 0]
                        && room[i, j].hasAdjacentRoom[1, 2])
                    {// Up Left Right
                        room[i, j].doorDir = DoorDirection.ULR;
                    }
                    else if (room[i, j].hasAdjacentRoom[0, 1] && room[i, j].hasAdjacentRoom[1, 0]
                        && room[i, j].hasAdjacentRoom[1, 2])
                    {// Down Left Right
                        room[i, j].doorDir = DoorDirection.DLR;
                    }
                    else if (room[i, j].hasAdjacentRoom[2, 1] && room[i, j].hasAdjacentRoom[0, 1]
                        && room[i, j].hasAdjacentRoom[1, 0])
                    {// Up Down Left
                        room[i, j].doorDir = DoorDirection.UDL;
                    }
                    else if (room[i, j].hasAdjacentRoom[2, 1] && room[i, j].hasAdjacentRoom[0, 1]
                        && room[i, j].hasAdjacentRoom[1, 2])
                    {// Up Down Right
                        room[i, j].doorDir = DoorDirection.UDR;
                    }
                    // ########### 2 Ways
                    else if (room[i, j].hasAdjacentRoom[2, 1] && room[i, j].hasAdjacentRoom[0, 1])
                    {// Up Down
                        room[i, j].doorDir = DoorDirection.UD;
                    }
                    else if (room[i, j].hasAdjacentRoom[1, 0] && room[i, j].hasAdjacentRoom[1, 2])
                    {// Left Right
                        room[i, j].doorDir = DoorDirection.LR;
                    }
                    else if (room[i, j].hasAdjacentRoom[1, 2] && room[i, j].hasAdjacentRoom[2, 1])
                    {// Up Right
                        room[i, j].doorDir = DoorDirection.UR;
                    }
                    else if (room[i, j].hasAdjacentRoom[1, 0] && room[i, j].hasAdjacentRoom[2, 1])
                    {// Up Left
                        room[i, j].doorDir = DoorDirection.UL;
                    }
                    else if (room[i, j].hasAdjacentRoom[0, 1] && room[i, j].hasAdjacentRoom[1, 2])
                    {// Down Right
                        room[i, j].doorDir = DoorDirection.DR;
                    }
                    else if (room[i, j].hasAdjacentRoom[0, 1] && room[i, j].hasAdjacentRoom[1, 0])
                    {// Down Left
                        room[i, j].doorDir = DoorDirection.DL;
                    }
                    // ################ 1 Way
                    else if (room[i, j].hasAdjacentRoom[2, 1])
                    {// Up
                        room[i, j].doorDir = DoorDirection.North;
                    }
                    else if (room[i, j].hasAdjacentRoom[1, 0])
                    {// Left
                        room[i, j].doorDir = DoorDirection.West;
                    }
                    else if (room[i, j].hasAdjacentRoom[1, 2])
                    {// Right
                        room[i, j].doorDir = DoorDirection.East;
                    }
                    else if (room[i, j].hasAdjacentRoom[0, 1])
                    {// Down
                        room[i, j].doorDir = DoorDirection.South;
                    }
                    else
                    {
                        room[i, j].doorDir = DoorDirection.Island;
                    }
                }
            }
        }
    }

    private void MakeRooms(Room[,] room)
    {
        for (int i = 0; i < MAP_HEIGHT; i++)
        {
            for (int j = 0; j < MAP_WIDTH; j++)
            {
                if (roomExists[i, j])
                {
                    if (room[i, j].doorDir == DoorDirection.Island)
                        continue;
                    GameObject obj = Instantiate(maps[(int)room[i, j].doorDir], room[i, j].gridPosition, Quaternion.identity);
                    obj.name = $"{i}, {j}";
                }
            }
        }
    }

    private void RandomizeRooms()
    {
        for (int i = 0; i < MAP_HEIGHT; i++)
        {
            for (int j = 0; j < MAP_WIDTH; j++)
            {
                roomExists[i, j] = Random.Range(0, 3) > 0;
            }
        }
    }
}
