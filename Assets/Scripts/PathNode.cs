using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int Xvalue, Yvalue;
    private Grid<PathNode> grid;

    public int GCost, HCost, FCost;
    public PathNode PreviousNode;
    public PathNode(Grid<PathNode> grid,int x, int y)
    {
        this.grid = grid;
        Xvalue = x;
        Yvalue = y;
    }
    public void CalculateFCost()
    {
        FCost = GCost + FCost;
    }
    public string Coordinates()
    {
        return $"({Xvalue},{Yvalue})";
    }
}
