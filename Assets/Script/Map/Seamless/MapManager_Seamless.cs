using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager_Seamless : MonoBehaviour
{
    RoomController[,] roomControllers;
    bool[,] roomExists;

    private const int MAP_HEIGHT = 3;
    private const int MAP_WIDTH = 3;

    AsyncOperation[,] asyncs;
    string[,] sceneNames;

    private void Awake()
    {
        roomExists = new bool[MAP_HEIGHT, MAP_WIDTH];
        asyncs = new AsyncOperation[MAP_HEIGHT, MAP_WIDTH];
        roomControllers = new RoomController[MAP_HEIGHT, MAP_WIDTH];

        for (int i = 0; i < MAP_HEIGHT; i++)
        {
            for (int j = 0; j < MAP_WIDTH; j++)
            {
                roomExists[i, j] = Random.Range(0, 2) == 1;
                if (roomExists[i, j])
                {
                    asyncs[i, j] = SceneManager.LoadSceneAsync("Seamless_Normal", LoadSceneMode.Additive);
                }
            }
        }
    }
}
