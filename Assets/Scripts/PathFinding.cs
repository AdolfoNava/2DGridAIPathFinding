using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding {
    private const int ToMoveStright = 10;
    private const int ToMoveDiagonal = 1000;
    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> lockedList;
    public PathFinding(int width,int height)
    {
        grid = new Grid<PathNode>(width, height, 2f, new Vector3(-8f, -14f), (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }
    public Grid<PathNode> GetGrid()
    {
        return grid;
    }
    private List<PathNode> LocatePath(int startingX,int startingY,int endX,int endY)
    {
        PathNode StartingNode = grid.GetGridObject(startingX,startingY);
        PathNode EndingNode = grid.GetGridObject(endX, endY);

        if(StartingNode == null|| EndingNode== null)
        {
            return null;
        }

        openList = new List<PathNode> { StartingNode };
        lockedList = new List<PathNode>();
        for(int x=0; x < grid.GetWidth(); x++)
        {
            for(int y=0;y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.GCost=int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.PreviousNode = null;
            }
        }

        StartingNode.GCost = 0;
        StartingNode.HCost = CalculateDistanceCost(StartingNode, EndingNode);
        StartingNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList); 
            //For the Final Node in the path made
            if(currentNode == EndingNode)
            {
                return CalculatePath(EndingNode);
            }
            openList.Remove(currentNode);
            lockedList.Add(currentNode);

            foreach(PathNode neighborNode in CollectNeighborList(currentNode))
            {
                if (lockedList.Contains(neighborNode)) continue;
                int Cost = currentNode.GCost+CalculateDistanceCost(currentNode,neighborNode);
                if (Cost < neighborNode.GCost)
                {
                    neighborNode.PreviousNode = currentNode;
                    neighborNode.GCost =Cost;
                    neighborNode.HCost = CalculateDistanceCost(neighborNode, EndingNode);
                    neighborNode.CalculateFCost();

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }
        //When there are no more nodes in the openList
        return null;
    }
    //Brings in Individual nodes
    public PathNode GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }
    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(endWorldPosition, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path)
            {
                vectorPath.Add(new Vector3(pathNode.Xvalue, pathNode.Yvalue) * grid.GetCellsize() + Vector3.one * grid.GetCellsize() * .5f);
            }
            return vectorPath;
        }
    }
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        if (startNode == null || endNode == null)
        {
            // Invalid Path
            return null;
        }

        openList = new List<PathNode> { startNode };
        lockedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.GCost = 99999999;
                pathNode.CalculateFCost();
                pathNode.PreviousNode = null;
            }
        }

        startNode.GCost = 0;
        startNode.HCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        //PathfindingDebugStepVisual.Instance.ClearSnapshots();
        //PathfindingDebugStepVisual.Instance.TakeSnapshot(grid, startNode, openList, lockedList);

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                // Reached final node
               // PathfindingDebugStepVisual.Instance.TakeSnapshot(grid, currentNode, openList, lockedList);
                //PathfindingDebugStepVisual.Instance.TakeSnapshotFinalPath(grid, CalculatePath(endNode));
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            lockedList.Add(currentNode);

            foreach (PathNode neighbourNode in CollectNeighborList(currentNode))
            {
                if (lockedList.Contains(neighbourNode)) continue;
                //if (!neighbourNode.isWalkable)
                //{
                //    lockedList.Add(neighbourNode);
                //    continue;
                //}

                int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.GCost)
                {
                    neighbourNode.PreviousNode = currentNode;
                    neighbourNode.GCost = tentativeGCost;
                    neighbourNode.HCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
                //PathfindingDebugStepVisual.Instance.TakeSnapshot(grid, currentNode, openList, lockedList);
            }
        }

        // Out of nodes on the openList
        return null;
    }
    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        //The Endpath
        PathNode currentNode = endNode;
        while (currentNode.PreviousNode != null)
        {
            path.Add(currentNode.PreviousNode);
            currentNode = currentNode.PreviousNode;
        }
        return path;
    }
    private List<PathNode> CollectNeighborList(PathNode pathNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (pathNode.Xvalue - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(pathNode.Xvalue - 1, pathNode.Yvalue));
            // Left Down
            if (pathNode.Yvalue - 1 >= 0) neighbourList.Add(GetNode(pathNode.Xvalue - 1, pathNode.Yvalue - 1));
            // Left Up
            if (pathNode.Yvalue + 1 < grid.GetHeight()) neighbourList.Add(GetNode(pathNode.Xvalue - 1, pathNode.Yvalue + 1));
        }
        if (pathNode.Xvalue + 1 < grid.GetWidth())
        {
            // Right
            neighbourList.Add(GetNode(pathNode.Xvalue + 1, pathNode.Yvalue));
            // Right Down
            if (pathNode.Yvalue - 1 >= 0) neighbourList.Add(GetNode(pathNode.Xvalue + 1, pathNode.Yvalue - 1));
            // Right Up
            if (pathNode.Yvalue + 1 < grid.GetHeight()) neighbourList.Add(GetNode(pathNode.Xvalue + 1, pathNode.Yvalue + 1));
        }
        // Down
        if (pathNode.Yvalue - 1 >= 0) neighbourList.Add(GetNode(pathNode.Xvalue, pathNode.Yvalue - 1));
        // Up
        if (pathNode.Yvalue + 1 < grid.GetHeight()) neighbourList.Add(GetNode(pathNode.Xvalue, pathNode.Yvalue + 1));

        return neighbourList;
    }
    public int CalculateDistanceCost(PathNode a,PathNode b)
    {
        int XDistance = Mathf.Abs(a.Xvalue - b.Xvalue);
        int YDistance = Mathf.Abs(a.Yvalue - b.Yvalue);
        int RemainingDistanceLeft = Mathf.Abs(XDistance - YDistance);
        return ToMoveDiagonal * Mathf.Min(XDistance, YDistance) + ToMoveStright * RemainingDistanceLeft;
    }
    //To Determine the shortest path for the ai pathfinding
    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode LowestFCostNode = pathNodeList[0];
        for(int i = 1;i <pathNodeList.Count; i++)
        {
            if (pathNodeList[i].FCost < LowestFCostNode.FCost)
            {
                LowestFCostNode = pathNodeList[i];
            }
        }
        return LowestFCostNode;
    }
}
