using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A star에서 사용하는 노드 정보 
/// </summary>
public class Node : System.IComparable<Node>
{
    /// 노드 정보
    /// 1. 위치
    /// 2. 갈 수 있는 곳인지
    /// 3. F, G , H
    /// 4. 부모 정보

    public int x;
    public int y;

    public float G;
    public float H;
    public float F => G + H;

    public Node parent;

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void ClearDatas()
    {
        G = float.MaxValue;
        H = float.MaxValue;
        parent = null;
    }

    #region Operator Overloading ##############################################
    public int CompareTo(Node other)
    {
        if (other == null)
            return 1;

        return F.CompareTo(other.F);
    }

    public static bool operator ==(Node op1, Vector2Int op2)
    {
        return op1.x == op2.x && op1.y == op2.y;
    }

    public static bool operator !=(Node op1, Vector2Int op2)
    {
        return op1.x != op2.x || op1.y != op2.y;
    }

    public override bool Equals(object obj)
    {
        return obj is Node node && x == node.x && y == node.y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, G, H, F, parent);
    }
    #endregion
}
