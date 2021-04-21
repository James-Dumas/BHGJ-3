
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNodeQueue
{
    private LinkedList<PathFindingNode> data;
    private HashSet<Vector2Int> pointSet;

    public int Count {
        get { return data.Count; }
    }

    public PathfindingNodeQueue()
    {
        data = new LinkedList<PathFindingNode>();
        pointSet = new HashSet<Vector2Int>();
    }

    public void Add(PathFindingNode node)
    {
        bool existing = pointSet.Contains(node.Value);
        LinkedListNode<PathFindingNode> listNode = data.First;
        if(existing)
        {
            while(listNode != null && listNode.Value.Value != node.Value)
            {
                listNode = listNode.Next;
            }

            if(listNode != null && node.Cost < listNode.Value.Cost)
            {
                data.Remove(listNode);
                existing = false;
            }
        }

        if(!existing)
        {
            listNode = data.First;
            while(listNode != null && node.Cost >= listNode.Value.Cost)
            {
                listNode = listNode.Next;
            }

            if(listNode == null)
            {
                data.AddLast(node);
            }
            else
            {
                data.AddBefore(listNode, node);
            }

            pointSet.Add(node.Value);
        }
    }

    public PathFindingNode PopBestNode()
    {
        PathFindingNode bestNode = data.First.Value;
        data.RemoveFirst();
        pointSet.Remove(bestNode.Value);
        return bestNode;
    }

    public void Clear()
    {
        data.Clear();
        pointSet.Clear();
    }

    public bool Contains(PathFindingNode node)
    {
        return pointSet.Contains(node.Value);
    }
}

public class PathFindingNode
{
    public Vector2Int Value;
    public PathFindingNode? Parent;
    public float Distance;
    public float Heuristic;

    public float Cost {
        get { return Distance + Heuristic; }
    }

    public PathFindingNode(Vector2Int value, PathFindingNode? parent, float distance, float heuristic)
    {
        Value = value;
        Parent = parent;
        Distance = distance;
        Heuristic = heuristic;
    }
}
