using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Star 알고리즘 수행 순서
/// 1. 시작 노드를 Open List에 넣는다 
/// 2. 아래를 반복한다 
///  2.1. Open List에서 F값이 가장 작은 것을 꺼내서 Current로 지정한다 
///  2.2. Current가 도착점인지 확인한다
///      2.2.1. 도착점이면 알고리즘 종료
///      2.2.2. 도착점이 아니, current를 close List로 넣는다
///  2.3. Current의 이웃의 G와 F를 갱신한다 (8방향)
///      2.3.1. 갈 수 없는 노드는 스킵 ( 벽, 맵 바깥, Close List에 있는 노드)
///     2.3.2. 갈 수 있는 노드
///          2.3.2.1. 원래 G값과 비교해서 작은걸 선택
///          2.3.2.2. G값이 바뀌면, Open List에 추가
///
/// ###################################### 필요한 정보
/// 필요한 변수
/// Node (그리드 한 칸)
/// F, G, H
/// 부모
/// Movable Bool

/// Open List
/// Close List
/// Current Node
/// 시작점 , 도착점
/// </summary>
public class A_Star : MonoBehaviour
{
    /// <summary>
    /// 랜덤 맵의 시작점부터 끝점까지 A Star를 활용한 길찾기 함수 
    /// </summary>
    /// <param name="map">맵 하나의 정보 </param>
    /// <param name="startPoint"> 시작지점  </param>
    /// <param name="endPoint">끝 지점 </param>
    /// <returns></returns>
    public static List<Vector2Int> FindPath_Map(Map map, Vector2Int startPoint, Vector2Int endPoint)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        List<Node> openList = new();
        List<Node> closeList = new();

        Node current = map.GetNode(startPoint);
        if (current != null)
        {
            current.G = 0;                                      // 주변 위치관련 변수 
            current.H = (endPoint - startPoint).sqrMagnitude;   // 휴리스틱 변수

            openList.Add(current);

            while (openList.Count > 0)
            {
                openList.Sort();    // 
                current = openList[0];
                openList.RemoveAt(0);

                if (current != endPoint)
                {
                    closeList.Add(current);
                    //Debug.Log($"{current.y}, {current.x}");
                    for (int y = -1; y < 2; y++)
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            Node node = map.GetNode(y + current.y, x + current.x);
                            if (node == null)   // 방이 없으면 
                                continue;
                            if (node == current) // 현재 방이면
                                continue;
                            if (closeList.Contains(node)) // Close 방에 있으면 
                                continue;
                            if (Mathf.Abs(x) == Mathf.Abs(y)) // 대각선 방이면 
                                continue;


                            if (node.G < current.G + 1)
                            { // G
                                node.G = current.G + 1;
                                if (node.parent == null)
                                {
                                    current.H = (endPoint - startPoint).sqrMagnitude;
                                    openList.Add(node);
                                }
                                node.parent = current;
                            }
                        }
                    }
                }
                else
                {// Open List에 아무것도 없으면 종료 
                    break;
                }
            }

            // 도착지점 도착 
            if (current == endPoint)
            {
                Node result = current;
                while (result != null)
                {
                    path.Add(new Vector2Int(result.x, result.y));
                    result = result.parent;
                }
                path.Reverse();
            }
        }
        return path;
    }
}