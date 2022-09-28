using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

public class Node : System.IComparable<Node>
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

    public float G;
    public float H;
    public float F => G + H;

    public Node parent;

    public Node(int x, int y, bool canMove = true)
    {
        this.x = x;
        this.y = y;
        this.movable = canMove;
    }

    public int CompareTo(Node other)
    {
        if (other == null)
            return 1;

        return F.CompareTo(other.F);
    }

    public override bool Equals(object obj)
    {
        return obj is Node node && x == node.x && y == node.y;
    }

    public static bool operator ==(Node op1, Vector2Int op2)
    {
        return op1.x == op2.x && op1.y == op2.y;
    }

    public static bool operator !=(Node op1, Vector2Int op2)
    {
        return op1.x != op2.x || op1.y != op2.y;
    }
}
