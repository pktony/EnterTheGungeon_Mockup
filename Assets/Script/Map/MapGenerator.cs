using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] maps;

    private bool[,] roomExists;

    // 전체 맵의 사이즈
    private const int MAP_SIZE = 7;

    // 방 한 칸의 사이즈
    private int ROOM_HEIGHT = 18;
    private int ROOM_WIDTH = 22;

    List<Vector2Int> path = new();

    private Transform player;

    public Transform startPoint;
    public Transform endPoint;
    public GameObject spawnerPrefab;

    /// <summary>
    /// C#에서 X,Y 좌표는 반대이다 
    /// </summary>
    private void Awake()
    {
        roomExists = new bool[MAP_SIZE, MAP_SIZE];
        Room[,] rooms = new Room[MAP_SIZE, MAP_SIZE];
        RandomizeRooms();
        FindAdjacentRooms(rooms);
        DecideRoomDirection(rooms);
        MakeRooms(rooms);
    }

    private void Start()
    {
        player = GameManager.Inst.Player.transform;
        player.position = startPoint.position;
    }

    /// <summary>
    /// 랜덤으로 방 정보를 생성하는 함수
    /// 첫 방에서 마지막 방까지 A star 알고리즘을 적용하기 떄문에 마지막 방만 지정해주면 된다
    /// 랜덤으로 지정되면 경로가 없는 경우가 생기기 때문에 경로가 있을 때까지 랜덤으로 생성 
    /// </summary>
    private void RandomizeRooms()
    {
        int failCount = 0;
        do
        {
            path.Clear();
            Map map = new Map(MAP_SIZE, MAP_SIZE);

            int randStart = Random.Range(0, MAP_SIZE);
            int randEnd = Random.Range(0, MAP_SIZE);

            path = A_Star.FindPath_Map(map, new Vector2Int(0, randStart), new Vector2Int(MAP_SIZE - 1, randEnd));
            failCount++;
        } while (path.Count < 1 && failCount < 100);

        // 스폰 지점 설정 
        startPoint.position = new Vector2(
            path[0].y * ROOM_WIDTH, path[0].x * ROOM_HEIGHT);
        // 마지막 방 설정
        Vector2 lastPos = new Vector2(
            path[path.Count - 1].y * ROOM_WIDTH, path[path.Count - 1].x * ROOM_HEIGHT);
        endPoint.position = lastPos;

        // 방 세부 위치 조절  
        Vector2 roomPos;
        for (int i = 0; i < path.Count; i++)
        {
            roomPos = path[i];
            roomPos.x = roomPos.x * ROOM_WIDTH;
            roomPos.y = roomPos.y * ROOM_HEIGHT;
            roomExists[path[i].x, path[i].y] = true;
        }
    }

    /// <summary>
    /// 방 주위에 다른 방이 있는지 확인 
    /// </summary>
    /// <param name="room"></param>
    private void FindAdjacentRooms(Room[,] room)
    {
        for (int i = 0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                if (roomExists[i, j])
                {
                    ///   #  
                    /// # # #
                    ///   #
                    /// i-1, j-1   i, j+1     i+1, j+1
                    /// i-1, j      i,j       i+1, j
                    /// i-1, j-1   i, j-1     i+1, j-1
                    room[i, j] = new Room(new Vector2(j * ROOM_WIDTH, i * ROOM_HEIGHT));
                    for (int k = -1; k < 2; k++)
                    {
                        for (int w = -1; w < 2; w++)
                        {
                            if (k == 0 && w == 0)
                                continue;   // 중앙 
                            else if (Mathf.Abs(k) == Mathf.Abs(w))
                                continue;   // 대각선
                            else if (i + k < 0 || i + k > MAP_SIZE - 1 || j + w < 0 || j + w > MAP_SIZE - 1)
                                continue;   // 경계선 
                            if (roomExists[i + k, j + w])
                            { // 0 ~ 2 까지인데 k,w 는 -1 ~ 1 까지임
                                room[i, j].hasAdjacentRoom[k + 1, w + 1] = true;
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 주변에 방이 있다면 문 위치 결정 
    /// </summary>
    /// <param name="room"></param>
    private void DecideRoomDirection(Room[,] room)
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

        for (int i = 0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
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
                    {// Island
                        room[i, j].doorDir = DoorDirection.Island;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 방 생성 
    /// </summary>
    /// <param name="room"></param>
    private void MakeRooms(Room[,] room)
    {
        for (int i = 0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                if (roomExists[i, j])
                {
                    // ------------------- 방 생성 
                    GameObject obj = Instantiate(maps[(int)room[i, j].doorDir],
                        room[i, j].gridPosition,
                        Quaternion.identity);
                    obj.name = $"{i}, {j}";


                    // ------------------- 스포너 생성
                    // 첫 번째 방은 스포너를 생성하지 않음 
                    if (i == path[0].x && j == path[0].y)
                        continue;
                    GameObject spawner = Instantiate(spawnerPrefab,
                        room[i, j].gridPosition,
                        Quaternion.identity);
                    spawner.transform.parent = obj.transform;   
                }
            }
        }

        
    }
}
