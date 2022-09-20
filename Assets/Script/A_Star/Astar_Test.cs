using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Astar_Test : MonoBehaviour
{
    public Tilemap ground;
    public Tilemap wall;


    private void Start()
    {
        GridMap map = new GridMap(ground, wall);
    }
}
