using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Star : MonoBehaviour
{
    // A Star 알고리즘 수행 순서
    // 1. 시작 노드를 Open List에 넣는.
    // 2. 아래를 반복한.
    //  2.1. Open List에서 F값이 가장 작은 것을 꺼내서 Current로 지정한.
    //  2.2. Current가 도착점인지 확인한다
    //      2.2.1. 도착점이면 알고리즘 종료
    //      2.2.2. 도착점이 아니, current를 close List로 넣는다
    //  2.3. Current의 이웃의 G와 F를 갱신한다 (8방향)
    //      2.3.1. 갈 수 없는 노드는 스킵 ( 벽, 맵 바깥, Close List에 있는 노드)
    //      2.3.2. 갈 수 있는 노드
    //          2.3.2.1. 원래 G값과 비교해서 작은걸 선택
    //          2.3.2.2. G값이 바뀌면, Open List에 추가

    /// 필요한 변수
    /// Node (그리드 한 칸)
    /// F, G, H
    /// 부모
    /// Movable Bool

    /// Open List
    /// Close List
    /// Current Node
    /// 시작점 , 도착점
    ///

    public static List<Vector2Int> FindPath(Grid gridMap, Vector2Int startPoint, Vector2Int endPoint)
    {
        List<Vector2Int> path = new List<Vector2Int>();
            
        List<Vector2Int> openList = new();
        List<Vector2Int> closeList = new();

        

        return path;
    }
}