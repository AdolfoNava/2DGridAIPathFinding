using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private PathFinding pathfinding;
    // Start is called before the first frame update
    void Start()
    {
        //Grid grid = new Grid(10, 12, 2f);
        //grid.gridArray = new int[grid.Width, grid.Height];
        pathfinding = new PathFinding(10, 12);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = GetMouseworldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
            if (path != null)
            {
                for(int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].Xvalue, path[i].Yvalue) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].Xvalue, path[i + 1].Yvalue) * 10f + Vector3.one * 5f,Color.green);
                }
            }
        }
    }
    private static Vector3 GetMouseworldPosition()
    {
        Vector3 vec = GetMouseworldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    private static Vector3 GetMouseworldPositionWithZ()
    {
        return GetMouseworldPositionWithZ(Input.mousePosition, Camera.main);
    }
    private static Vector3 GetMouseworldPositionWtihZ(Camera worldCamera)
    {
        return GetMouseworldPositionWithZ(Input.mousePosition, Camera.main);
    }
    private static Vector3 GetMouseworldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
