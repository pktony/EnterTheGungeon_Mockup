using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    /// 노드 정보
    /// 1. 위치
    /// 2. 갈 수 있는 곳인지
    /// 3. F, G , H
    /// 4. 부모 정보
    /// 5.
    ///

    public int x;
    public int y;

    public bool movable;

    float G;
    float H;
    float F => G + H;

    Node parent;

    public Node(int x, int y, bool canMove = true)
    {
        this.x = x;
        this.y = y;
        this.movable = canMove;
    }
}
